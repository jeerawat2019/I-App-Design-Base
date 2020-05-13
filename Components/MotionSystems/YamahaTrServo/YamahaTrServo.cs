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
using System.Diagnostics;

using MCore.Comp.Communications;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MDouble;
namespace MCore.Comp.MotionSystem
{
    /// <summary>
    /// Yamaha Tr Servo Driver Interface
    /// </summary>
    public class YamahaTrServo:MotionSystemBase
    {
        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        private BackgroundWorker _updateLoop = null;
        private List<RealAxis> _listRealAxis = null;
        private volatile bool _destroy = false;
        private ManualResetEvent _waitPortRead = new ManualResetEvent(true);
        private SerialPort _rs232Port = null;

        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public RS232 Port
        //{
        //    get { return GetPropValue(() => Port);  }
        //    set { SetPropValue(() => Port,value); }
        //}

        /// <summary>
        /// class objects to store the port settings
        /// </summary>
        [Category("Communication Port")]
        public TsRS232Settings PortSetting
        {
            get { return GetPropValue(() => PortSetting); }
            set { SetPropValue(() => PortSetting, value); }
        }

        /// <summary>
        /// ReadTimeOut
        /// </summary>
        [Category("Communication Port")]
        public Miliseconds ReadTimeOut
        {
            get { return GetPropValue(()=>ReadTimeOut,500); }
            set { SetPropValue(() => ReadTimeOut,value); }

        }

          /// <summary>
        /// Default constructor for xml streaming
        /// </summary>
        public YamahaTrServo()
        {
            // Keep empty, xml will stream data into class
        }

        /// <summary>
        /// Constructor used for first-time construction
        /// </summary>
        /// <param name="name"></param>
        public YamahaTrServo(string name)
            : base(name)
        {
            // Only used to create 1st xml data file
            
        }

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                _rs232Port = new System.IO.Ports.SerialPort();
                _rs232Port.PortName = PortSetting.CommPort.ToString();
                _rs232Port.BaudRate = (int)PortSetting.BaudRate;
                _rs232Port.Parity = PortSetting.Parity;
                _rs232Port.StopBits = PortSetting.StopBits;
                _rs232Port.ReadTimeout = (int)PortSetting.ReadWriteTimeOut;
                _rs232Port.NewLine = "\r\n";

                _rs232Port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnDataReceived);

                if(_rs232Port.IsOpen)
                {
                    _rs232Port.Close();
                }
                _rs232Port.Open();

                if (!_rs232Port.IsOpen)
                {
                    throw new ForceSimulateException(string.Format("Port {0} is not opened", PortSetting.CommPort.ToString()));
                }

                CheckStatus();

                Simulate = eSimulate.None;

            }
            catch (Exception ex)
            {
                _rs232Port.Close();
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

            //if (!this.Simulated)
            //{
            //    _updateLoop.RunWorkerAsync();
            //}
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

            if (_rs232Port == null)
                return;

            if (_rs232Port != null && _rs232Port.IsOpen)
            {
            }
            _rs232Port.Close();
            base.Destroy();
        }

        private void OnSendCommand(string strCmd)
        {
            try
            {
                SendPortCommand(strCmd, PortSetting.ReadWriteTimeOut);
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
            // We assume only one axis
            RealAxis axis = _listRealAxis[0] as RealAxis;
            if (axis != null)
            {

                try
                {

                    string ret;
                    ret = SendPortCommand("?D0", PortSetting.ReadWriteTimeOut);

                    if (ret != null)
                    {
                        string[] recvPos = new string[4];
                        recvPos = ret.Split('=');
                        axis.SetCurrentPosition(double.Parse(recvPos[1]) / 100.0);
                    }

                }
                catch (Exception ex)
                {
                    //U.LogError(ex, "Error for set current position '{0}'", this.Nickname);
                    throw new MCoreExceptionPopup(ex, "Error for set current position '{0}'", this.Nickname);
                }
            }
            
        }


        private void CheckStatus()
        {
            try
            {

                string ret;
                ret = SendPortCommand("RESET", 2000);
                if (!ret.Contains("OK"))
                {
                    throw new MCoreExceptionPopup("Check Status Fail '{0}'", this.Nickname);
                }

            }
            catch (Exception ex)
            {
                //U.LogError(ex, "Check Status Fail '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Check Status Fail '{0}'", this.Nickname);
            }

        }


        public override void Reset()
        {
            base.Reset();

            try
            {

                string ret;
                ret = SendPortCommand("RESET", PortSetting.ReadWriteTimeOut);
                if (!ret.Contains("OK"))
                {
                    //U.LogError("Reset Command Fail '{0}'", this.Nickname);
                    throw new MCoreExceptionPopup("Reset Command Fail '{0}'", this.Nickname);
                }

                System.Threading.Thread.Sleep(100);
                OnUpdateCurrentPositions();

            }
            catch (Exception ex)
            {
                //U.LogError(ex, "Reset Command Fail '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Reset Command Fail '{0}'", this.Nickname);
            }

        }

        public override void HomeAxis(RealAxis axis)
        {
            int axisNumber = axis.AxisNo + 1;
            lock (this)
            {
                try
                {

                    String ret = SendPortCommand("ORG", PortSetting.ReadWriteTimeOut);
                    if (!ret.Contains("RUN") && !ret.Contains("OK"))
                    {
                        axis.Homed = false;
                       // U.LogError("Origin command failed {0}", this.Nickname);
                        throw new MCoreExceptionPopup("Origin command failed {0}", this.Nickname);

                    }


                }
                catch (Exception ex)
                {
                    axis.Homed = false;
                    //U.LogError(ex, "Origin command failed {0}", this.Nickname);
                    throw new MCoreExceptionPopup(ex, "Origin command failed {0}", this.Nickname);
                }
                WaitForHomeDone(60000);
                axis.Homed = true;
                System.Threading.Thread.Sleep(2000);
                OnUpdateCurrentPositions();
            }
        }

        public void WaitForHomeDone(int timeout)
        {
            Stopwatch timeoutStopw = new Stopwatch();
            timeoutStopw.Start();
            while (true)
            {
                try
                {

                    string ret = SendPortCommand("?D18", PortSetting.ReadWriteTimeOut);
                    if (timeoutStopw.ElapsedMilliseconds > timeout)
                    {
                        throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                    }


                    string ret2 = SendPortCommand("?ALM", PortSetting.ReadWriteTimeOut);
                    if (timeoutStopw.ElapsedMilliseconds > timeout)
                    {
                        throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] error: Time out wait move absolute position.", this.Name);
                    }

                    if (!ret2.Contains("OK"))
                    {
                        throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] error: Alarm Occure", this.Name);
                    }


                    if (ret.Contains("0"))
                    {

                        break;
                    }

                }
                catch (Exception ex)
                {

                    //U.LogError(ex, "Wait Home Done Fail {0}", this.Nickname);
                    throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                }

                Thread.Sleep(100);
            }
        }


        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            int servoOnOff = 1;
            if (bEnable == true)
            {
                servoOnOff = 1;
            }
            else
            {
                servoOnOff = 0;
            }
            try
            {

                int retryCount = 3;
                string ret = "";
                for (int i = 0; i < retryCount; i++)
                {
                    ret = SendPortCommand("SRVO" + servoOnOff, PortSetting.ReadWriteTimeOut);
                    if (ret.Contains("OK"))
                    {
                        break;
                    }
                }
                if (!ret.Contains("OK"))
                {
                    //U.LogError("Enable Axis Command fail {0}", this.Nickname);
                    throw new MCoreExceptionPopup("Enable Axis Command fail {0}", this.Nickname);
                }
                axis.Enabled = bEnable;
                System.Threading.Thread.Sleep(100);
                OnUpdateCurrentPositions();
            }
            catch (Exception ex)
            {
                //U.LogAlarmPopup(ex, "Enable Axis Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Enable Axis Command fail {0}", this.Nickname);
            }
        }

          /// <summary>
        /// MoveAbsAxis
        /// </summary>
        /// <param name="axis"></param>
        public override void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {


            try
            {

                string ret = "";
                ret = SendPortCommand(string.Format("P1={0}", (int)(axis.TargetPosition*100)), PortSetting.ReadWriteTimeOut);
                //System.Threading.Thread.Sleep(10);
                ret = SendPortCommand(string.Format("S1={0}", (int)speed), PortSetting.ReadWriteTimeOut);
                //System.Threading.Thread.Sleep(10);
                ret = SendPortCommand(string.Format("AC1={0}", (int)axis.AccelDecel), PortSetting.ReadWriteTimeOut);
                //System.Threading.Thread.Sleep(10);
                ret = SendPortCommand(string.Format("DC1={0}", (int)axis.AccelDecel), PortSetting.ReadWriteTimeOut);
                //System.Threading.Thread.Sleep(10);
                ret = SendPortCommand(string.Format("M1=1"), PortSetting.ReadWriteTimeOut);
                //System.Threading.Thread.Sleep(10);
                
                ret = SendPortCommand(String.Format("START1"), PortSetting.ReadWriteTimeOut);
                System.Threading.Thread.Sleep(10);
                if (!ret.Contains("RUN") && !ret.Contains("OK"))
                {
                    throw new MCoreExceptionPopup("Move Absolute Command fail {0}", this.Nickname);
                }


                WaitForMoveDone(10000);

                System.Threading.Thread.Sleep(25);
                OnUpdateCurrentPositions();

            }
            catch (Exception ex)
            {
                //U.LogError(ex, "Move Absolute Command fail {0}", this.Nickname);
                throw new MCoreExceptionPopup(ex,"Move Absolute Command fail {0}", this.Nickname);
            }
            
        }

        public void WaitForMoveDone(int timeout)
        {
          
                Stopwatch timeoutStopw = new Stopwatch();
                timeoutStopw.Start();
                while (true)
                {
                    try
                    {

                        string ret = SendPortCommand("?D18", PortSetting.ReadWriteTimeOut);
                        if (timeoutStopw.ElapsedMilliseconds > timeout)
                        {
                            throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] error: Time out wait move absolute position.", this.Name);
                        }

                        string ret2 = SendPortCommand("?ALM", PortSetting.ReadWriteTimeOut);
                        if (timeoutStopw.ElapsedMilliseconds > timeout)
                        {
                            throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] error: Time out wait move absolute position.", this.Name);
                        }

                        if(!ret2.Contains("OK"))
                        {
                            throw new MCoreExceptionPopup("Yamaha Motor TS-S [{0}] error: Alarm Occure", this.Name);
                        }


                        if (ret.Contains("0"))
                        {
                            break;
                        }

                    }
                    catch (Exception ex)
                    {

                        //U.LogError(ex, "Wait Move Done Fail {0}", this.Nickname);
                        throw new MCoreExceptionPopup(ex, "Wait Move Done Fail {0}", this.Nickname);
                    }
                    Thread.Sleep(100);
                }
          
        }

        private string SendPortCommand(string command, Miliseconds timeout)
        {
            lock (_waitPortRead)
            {
                //_waitPortRead.Reset();
                _rs232Port.DiscardInBuffer();
                string trServoCommFormat = ("@" + command);
                _rs232Port.WriteLine(trServoCommFormat);
                System.Threading.Thread.Sleep(10);

            }
            try
            {
                //U.BlockOrDoEvents(_waitPortRead, timeout.ToInt);
                System.Threading.Thread.Sleep(10);
                string s = _rs232Port.ReadLine();
                return s;
            }
            catch (Exception ex)
            {
                //U.LogError(ex, "Timeout waiting for read port of '{0}'", this.Nickname);
                throw new MCoreExceptionPopup(ex, "Timeout waiting for read port of '{0}' of  command: {1}", this.Nickname,command);
            }
        }
        


        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (_waitPortRead)
                {
                    _waitPortRead.Set();
                }
            }
            catch (Exception ex)
            {
                //U.LogError(ex, "'{0}' received unexpected", Nickname);
                throw new MCoreExceptionPopup(ex, "'{0}' received unexpected", Nickname);
            }
        }

    }
}
