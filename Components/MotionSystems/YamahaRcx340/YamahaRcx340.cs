using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.Threading;
using System.IO.Ports;
using System.ComponentModel;

using MCore.Comp.Communications;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MDouble;

namespace MCore.Comp.MotionSystem
{
    /// <summary>
    /// YamahaRcx340 driver interface
    /// </summary>
    public class YamahaRcx340 : MotionSystemBase
    {
        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        private BackgroundWorker _updateLoop = null;
        private List<RealAxis> _listRealAxis = null;

        private TcpClient _tcpClient = null;

        private volatile bool _destroy = false;

        /// <summary>
        /// Get the Global IP address
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("TCPIP")]
        public string IPAddress
        {
            get { return GetPropValue(() => IPAddress,"192.168.0.2"); }
            set { SetPropValue(() => IPAddress, value); }
        }


        /// <summary>
        /// Get the Comm Port
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("TCPIP")]
        public ushort Port
        {
            get { return GetPropValue(() => Port,(ushort)23); }
            set { SetPropValue(() => Port, value); }
        }


        /// <summary>
        /// Get Last TCP Response
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("TCPIP")]
        [XmlIgnore]
        public string LastTcpResponse
        {
            get { return GetPropValue(() => LastTcpResponse, ""); }
            set { SetPropValue(() => LastTcpResponse, value); }
        }


        [Category("TCPIP")]
        public Miliseconds ReadTimeOut
        {
            get { return GetPropValue(() => ReadTimeOut,1000); }
            set { SetPropValue(() => ReadTimeOut, value); }

        }

        [Browsable(true)]
        [Category("Axes")]
        [Description("Axes Move Mode")]
        public Int32 AxesMoveMode
        {
            get { return GetPropValue(() => AxesMoveMode, 0); }
            set { SetPropValue(() => AxesMoveMode, value); }
        }


        /// <summary>
        /// Get Lat TCP Response
        /// </summary>
        /// <returns></returns>
        [Browsable(true)]
        [Category("Controller Config")]
        public Miliseconds CommandWaitTime
        {
            get { return GetPropValue(() => CommandWaitTime, 500); }
            set { SetPropValue(() => CommandWaitTime, value); }
        }


       

        /// <summary>
        /// Default constructor for xml streaming
        /// </summary>
        public YamahaRcx340()
        {
            // Keep empty, xml will stream data into class
        }

        /// <summary>
        /// Constructor used for first-time construction
        /// </summary>
        /// <param name="name"></param>
        public YamahaRcx340(string name)
            : base(name)
        {
            // Only used to create 1st xml data file
            
        }


        /// <summary>
        /// Initialize this component
        /// </summary>
        /// <returns></returns>
        public override void Initialize()
        {
            base.Initialize();
            try
            {

                _tcpClient = new TcpClient();
               
                _tcpClient.Connect(System.Net.IPAddress.Parse(IPAddress),Port);
                string ret = WaitForCommand();
                CheckStatus();
                Simulate = eSimulate.None;
            }
            catch (Exception ex)
            {
                _tcpClient.Close();
                //throw new ForceSimulateException(ex);

                U.LogAlarmPopup(ex, this.Nickname);
                this.Simulate = eSimulate.SimulateDontAsk;

            }


            _updateLoop = new BackgroundWorker();
            _updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
        }


        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _listRealAxis = this.RecursiveFilterByType<RealAxis>().ToList();

            foreach (RealAxis realAxis in _listRealAxis)
            {
                //Do some Axis Initialize Task
            }



            ClearAllFaults();

            if (this.Simulate == eSimulate.None)
            {
                _updateLoop.RunWorkerAsync();
            }
        }


        /// <summary>
        /// Destroy
        /// </summary>
        public override void PreDestroy()
        {
            _destroy = true;
            base.PreDestroy();
        }



        /// <summary>
        /// Opportunity to cleanup
        /// </summary>
        public override void Destroy()
        {

            if (_tcpClient == null)
                return;

            if (_tcpClient.Client != null && _tcpClient.Connected)
            {
            }
            _tcpClient.Close();
            base.Destroy();
        }



        private void OnSendCommand(string strCmd)
        {
            try
            {
                SendCommand(strCmd);
                LastTcpResponse = string.Empty;
                LastTcpResponse = WaitForCommand();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        private void UpdateLoop(object sender, DoWorkEventArgs e)
        {
            do
            {
                try
                {
                    OnUpdateCurrentPositions();
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    U.Log(ex);
                    return;
                }
            } while (!_destroy);
        }



        private void OnUpdateCurrentPositions()
        {
            try
            {
                lock (this)
                {
                    SendCommand(string.Format("?WHRXY"));
                    string ret = WaitForCommand();

                    string[] d = ret.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    //string d0 = d[0];
                    //string d1 = d[1];
                    //d[0] = d1;
                    //d[1] = d0;

                    foreach (RealAxis axis in _listRealAxis)
                    {

                        int axisNumber = axis.AxisNo;
                        if (axisNumber < d.Length)
                        {
                            axis.SetCurrentPosition(System.Convert.ToDouble(d[axisNumber]));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Error for set current position '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Error for set current position '{0}'", this.Nickname);
            }
        }


        public void CheckStatus()
        {
            base.Reset();
            string ret = "";
            try
            {
                lock (this)
                {
                    SendCommand("STOP");
                    ret = WaitForCommand();
                    //CheckOKReplied("STOP", ret,"Welcome to RCX340");
                   CheckOKReplied("STOP", ret, "OK");

                }

            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Check Status Command Fail '{0}'", this.Nickname);
            }
            finally
            {

            }
        }



        /// <summary>
        /// Reset this component
        /// </summary>
        [StateMachineEnabled]
        public override void Reset()
        {
            base.Reset();
            string ret = "";
            try
            {
                lock (this)
                {
                    SendCommand("STOP");
                    ret = WaitForCommand();
                    CheckOKReplied("STOP", ret,"OK");

                    SendCommand("ALMRST");
                    ret = WaitForCommand();
                    CheckOKReplied("ALMRST", ret,"RUN");
                    ret = WaitForCommand();
                    CheckOKReplied("ALMRST", ret, "END");
                }

            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Reset Command Fail '{0}'", this.Nickname);
            }
            finally
            {

            }
        }

       
        public override void HomeAxis(RealAxis axis)
        {
            int axisNumber = axis.AxisNo + 1;
            axis.Homed = false;
            try
            {
                lock (this)
                {
                    string ret = string.Empty;
                    SendCommand("STOP");
                    ret = WaitForCommand();
                    CheckOKReplied("STOP", ret,"OK");

                    SendCommand("ALMRST");
                    ret = WaitForCommand();
                    CheckOKReplied("ALMRST", ret,"RUN");
                    ret = WaitForCommand();
                    CheckOKReplied("ALMRST", ret, "END");

                    SendCommand("ASPEED 100");
                    ret = WaitForCommand();
                    CheckOKReplied("ASPEED", ret,"OK");

                    SendCommand(string.Format("DRIVE ({0},0.0),S=1", axisNumber));
                    ret = WaitForCommand();
                    CheckOKReplied("WaitHomeDone", ret, "RUN");
                    ret = WaitForCommand(60000);
                    CheckOKReplied("WaitHomeDone", ret,"END");
                    axis.Homed = true;
                }

                lock (this)
                {
                    OnUpdateCurrentPositions();
                }
               
            }
            catch (Exception ex)
            {
                axis.Homed = false;
                //U.LogAlarmPopup(ex, "Origin command failed {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "origin command failed {0}", this.Nickname);
            }
           
        }


        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            string ret = string.Empty;
            
            try
            {
                lock (this)
                {
                    // Check current status
                    
                    SendCommand(string.Format("?SERVO"));
                    ret = WaitForCommand();
                    
                    
                    if (ret.Length < 9)
                    {
                        throw new Exception("Unexpected response");
                    }
                    string[] split = ret.Split(',');
                    if (split.Length < 4)
                    {
                        throw new Exception("Drive is not ON");
                    }

                    int axisNumber = axis.AxisNo + 1;

                    if (bEnable)
                    {
                        //Reset alarm
                        SendCommand("STOP");
                        ret = WaitForCommand();
                        CheckOKReplied("STOP", ret, "OK");
                        SendCommand("ALMRST");
                        ret = WaitForCommand();
                        CheckOKReplied("ALMRST", ret, "RUN");
                        ret = WaitForCommand();
                        CheckOKReplied("ALMRST", ret, "END");

                        SendCommand(string.Format("SERVO ON({0})", axisNumber));
                        ret = WaitForCommand();
                        CheckOKReplied("Enable", ret, "RUN");
                        ret = WaitForCommand();
                        CheckOKReplied("Enable", ret, "END");
                    }
                    else
                    {
                        SendCommand(string.Format("SERVO OFF({0})", axisNumber));
                        ret = WaitForCommand();
                        CheckOKReplied("Enable", ret, "RUN");
                        ret = WaitForCommand();
                        CheckOKReplied("Enable", ret, "END");
                    }
                    axis.Enabled = bEnable;
                }
            }
            catch (Exception ex)
            {
                //U.LogError(ex, "Failed to enable '{0}'", axis.Name);
                throw new MCoreExceptionPopup(ex, "Failed to enable '{0}'", axis.Name);
            }
            
        }


        public override void MoveLinearXY(AxesBase axes, MLengthSpeed speed)
        {
            
            Millimeters mmSpeed = new Millimeters(speed);

            try
            {
                double xPos = (axes.ChildArray[0] as RealAxis).TargetPosition;
                double yPos = (axes.ChildArray[1] as RealAxis).TargetPosition;
                double zPos = (axes.ChildArray[2] as RealAxis).TargetPosition;
                double rPos = (axes.ChildArray[3] as RealAxis).TargetPosition;
                double percent = mmSpeed;
                lock (this)
                {
                    string cmd = string.Format("MOVE P,{0:F2} {1:F2} {2:F2} {3:F2} 0.0 0.0 {4},S={5:F2}", xPos, yPos, zPos, rPos,this.AxesMoveMode, percent);
                    SendCommand(cmd);
                    string ret = WaitForCommand();
                    CheckOKReplied("MOVE P", ret, "RUN");
                    ret = WaitForCommand(30000);
                    CheckOKReplied("WaitMoveDone", ret, "END");
                }

                lock (this)
                {
                    OnUpdateCurrentPositions();
                }
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "MoveLinearXY Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "MoveLinearXY Command fail {0}", this.Nickname);
            }

        }

        /// <summary>
        /// MoveAbsAxis
        /// </summary>
        /// <param name="axis"></param>
        public override void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            Millimeters mmSpeed = new Millimeters(speed);
            try
            {
                lock (this)
                {
                    int axisNumber = axis.AxisNo + 1;
                    double percent = mmSpeed;
                    double dPos = axis.TargetPosition;
                    string cmd = string.Format("DRIVE({0},{1:F2}),S={2:F2}", axisNumber, dPos, percent);
                    SendCommand(cmd);

                    string ret = WaitForCommand();
                    CheckOKReplied("DRIVE", ret, "RUN");
                    ret = WaitForCommand(30000);
                    CheckOKReplied("WaitMoveDone", ret,"END");
                }

                lock (this)
                {
                    OnUpdateCurrentPositions();
                }
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
            }
            
        }


        public override void MoveRelAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            Millimeters mmSpeed = new Millimeters(speed);
            try
            {
                lock (this)
                {
                    int axisNumber = axis.AxisNo + 1;
                    double percent = mmSpeed;
                    double dPos = axis.TargetPosition;
                    string cmd = string.Format("DRIVEI({0},{1:F2}),S={2:F2}", axisNumber, dPos, percent);
                    SendCommand(cmd);

                    string ret = WaitForCommand();
                    CheckOKReplied("DRIVEI", ret, "RUN");
                    ret = WaitForCommand(30000);
                    CheckOKReplied("WaitMoveDone", ret, "END");
                }

                lock (this)
                {
                    OnUpdateCurrentPositions();
                }
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Move Absolute Command fail {0}", this.Nickname);
            }

        }


        /// <summary>
        /// Excecute Special Command
        /// </summary>
        /// <param name="command"></param>
        public override void ExecuteCommand(string command)
        {
            base.ExecuteCommand(command);

            if(command.Contains("Axes Move Mode="))
            {
                string sMode = command.Replace("Axes Move Mode=","");
                try
                {
                    // 0 = Default
                    // 1 = Righty
                    // 2 = Lefty
                    int iMode = int.Parse(sMode);
                    AxesMoveMode = iMode;
                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Axes Move Mode Command fail {0}", this.Nickname);
                }
            }
            
          
        }


        /// <summary>
        /// Send a user-defined command
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(string command)
        {
            try
            {
                if (_tcpClient.Available > 0)
                {
                    string rcv = string.Empty;
                    byte[] rcvBuf = new byte[1024];
                    int rcvSize = 0;
                    _tcpClient.ReceiveTimeout = 1000;
                    rcvSize = _tcpClient.Client.Receive(rcvBuf);
                    byte[] tmp = new byte[rcvSize];
                    Array.Copy(rcvBuf, tmp, rcvSize);
                    rcv = Encoding.ASCII.GetString(tmp);
                }

                string fcmd = "@" + command + "\r\n";
                _tcpClient.Client.Send(Encoding.ASCII.GetBytes(fcmd));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} about to ?SERVO", ex.Message));
            }
        }

        private string WaitForCommand()
        {
            return WaitForCommand((int)ReadTimeOut);
        }

        private string WaitForCommand(int ms)
        {
            try
            {
                string rcv = string.Empty;
                byte[] rcvBuf = new byte[1024];
                int rcvSize = 0;
                _tcpClient.Client.Blocking = true;
                _tcpClient.ReceiveTimeout = ms;
                rcvSize = _tcpClient.Client.Receive(rcvBuf);
                byte[] tmp = new byte[rcvSize];
                Array.Copy(rcvBuf, tmp, rcvSize);
                rcv = Encoding.ASCII.GetString(tmp);
                return rcv.Trim();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckOKReplied(string action, string returned)
        {
            if (!returned.Contains("OK"))
            {
                U.LogError(action, string.Format("Error in command for {0}.  String returned= '{1}'", action, returned));
                throw new MCoreExceptionPopup("Error in command for {0}.  String returned= '{1}'", action, returned);
            }
        }

        private void CheckOKReplied(string action, string returned,string exspecReturn)
        {
            if (!returned.Contains(exspecReturn))
            {
                U.LogError( action, string.Format("Error in command for {0}.  String returned= '{1}'", action, returned));
                throw new MCoreExceptionPopup("Error in command for {0}.  String returned= '{1}'", action, returned);
            }
        }

    }
}
