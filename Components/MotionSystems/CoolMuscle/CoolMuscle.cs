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
    /// Cool Muscle Driver Interface
    /// </summary>
    public class CoolMuscle:MotionSystemBase
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
        [Category("Communication Port")]
        public CmRS232Settings PortSetting
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


        [Browsable(true)]
        [Category("Motor")]
        [Description("Motor Resolution")]
        public Int32 MotorResolution
        {
            get { return GetPropValue(() => MotorResolution, 0); }
            set { SetPropValue(() => MotorResolution, value); }
        }


        [Browsable(true)]
        [Category("Axis")]
        [Description("Axis Cycle Pitch")]
        public MDouble.Millimeters AxisCyclePitch
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => AxisCyclePitch, 0); }
            [StateMachineEnabled]
            set { SetPropValue(() => AxisCyclePitch, value); }
        }


          /// <summary>
        /// Default constructor for xml streaming
        /// </summary>
        public CoolMuscle()
        {
            // Keep empty, xml will stream data into class
        }

        /// <summary>
        /// Constructor used for first-time construction
        /// </summary>
        /// <param name="name"></param>
        public CoolMuscle(string name)
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

                //_rs232Port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(OnDataReceived);

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
            
            //_updateLoop = new BackgroundWorker();
            //_updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
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


        //private void UpdateLoop(object sender, DoWorkEventArgs e)
        //{
        //    do
        //    {
        //        try
        //        {
        //            OnUpdateCurrentPositions();
        //            Thread.Sleep(100);
        //        }
        //        catch (Exception ex)
        //        {
        //            U.Log(ex);
        //            return;
        //        }
        //    } while (!_destroy);
        //}

        private void OnUpdateCurrentPositions()
        {
            // We assume only one axis
            RealAxis axis = _listRealAxis[0] as RealAxis;
            if (axis != null)
            {

                try
                {

                    string ret;
                    ret = SendPortCommand("?96."+axis.AxisNo.ToString(), PortSetting.ReadWriteTimeOut,true);

                    if (ret != null & ret.Contains("="))
                    {
                        string pulsePos = ret.Substring((ret.IndexOf("=") + 1), ret.Length - (ret.IndexOf("=") + 1));
                        double dPulsePos = double.Parse(pulsePos);
                        double linearPos =  PulsePosConvertToRealPos(dPulsePos);
                        axis.SetCurrentPosition(linearPos);
                    }

                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Error for set current position '{0}'", this.Nickname);
                }
            }
            
        }


        private void CheckStatus()
        {
            try
            {

                string ret;
                ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut, true);
                if (!ret.Contains("Ux"))
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
                SendPortCommand(").{0}", PortSetting.ReadWriteTimeOut,false);
                SendPortCommand("(.{0}", PortSetting.ReadWriteTimeOut, false);

                ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut, true);
                if (!ret.Contains("Ux"))
                {
                    throw new MCoreExceptionPopup("Reset Command Fail '{0}'", this.Nickname);
                }

                System.Threading.Thread.Sleep(100);
                OnUpdateCurrentPositions();

            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Reset Command Fail '{0}'", this.Nickname);
            }

        }

        public override void HomeAxis(RealAxis axis)
        {
            int axisNumber = axis.AxisNo + 1;
            //lock (this)
            //{
                try
                {

                    String ret;
                    SendPortCommand("|." + axisNumber.ToString(), PortSetting.ReadWriteTimeOut, false);
                    ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut, true);
                    if (!ret.Contains("Ux"))
                    {
                        axis.Homed = false;
                        throw new MCoreExceptionPopup("Origin command failed {0}", this.Nickname);

                    }


                }
                catch (Exception ex)
                {
                    axis.Homed = false;
                    throw new MCoreExceptionPopup(ex, "Origin command failed {0}", this.Nickname);
                }
                WaitForHomeDone(60000);
                axis.Homed = true;
                System.Threading.Thread.Sleep(2000);
                OnUpdateCurrentPositions();
            //}
        }

        public void WaitForHomeDone(int timeout)
        {
            Stopwatch timeoutStopw = new Stopwatch();
            timeoutStopw.Start();
            while (true)
            {
                try
                {

                    string ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut,true);
                    if (timeoutStopw.ElapsedMilliseconds > timeout)
                    {
                        throw new MCoreExceptionPopup("Cool Muscle [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                    }

                    if (ret.Contains("Ux.1=8"))
                    {

                        break;
                    }

                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup("Cool Muscle [{0}] Home timeout waiting for Origin State signal to be ON.", this.Nickname);
                }

                Thread.Sleep(100);
            }
        }


        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            string servoOnOff = "";
            if (bEnable == true)
            {
                servoOnOff = string.Format("(.{0}", axis.AxisNo);
            }
            else
            {
                servoOnOff = string.Format(").{0}", axis.AxisNo);
            }
            try
            {

                int retryCount = 3;
                string ret = "";
                for (int i = 0; i < retryCount; i++)
                {
                    string command =
                    SendPortCommand(servoOnOff, PortSetting.ReadWriteTimeOut,false);
                    ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut, true);
                    if (ret.Contains("Ux."))
                    {
                        break;
                    }
                }
                if (!ret.Contains("Ux."))
                {
                    
                    throw new MCoreExceptionPopup("Enable Axis Command fail {0}", this.Nickname);
                }
                axis.Enabled = bEnable;
                System.Threading.Thread.Sleep(100);
                OnUpdateCurrentPositions();
            }
            catch (Exception ex)
            {

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
                double dTargetPos = RealPosionConvertToPulsePos(axis.TargetPosition);
                ret = SendPortCommand(string.Format("P.{0}={1}",axis.AxisNo, (int)(dTargetPos)), PortSetting.ReadWriteTimeOut,false);
                //System.Threading.Thread.Sleep(10);
                double dTargetSpeed = RealSpeedConvertToPulseSpeed(speed);
                ret = SendPortCommand(string.Format("S.{0}={1}",axis.AxisNo, (int)dTargetSpeed), PortSetting.ReadWriteTimeOut,false);
                //System.Threading.Thread.Sleep(10);
                double dTargetAceel = RealSpeedConvertToPulseSpeed(axis.AccelDecel);
                ret = SendPortCommand(string.Format("A.{0}={1}",axis.AxisNo, (int)dTargetAceel), PortSetting.ReadWriteTimeOut,false);
                //System.Threading.Thread.Sleep(10);
                ret = SendPortCommand(string.Format("^.{0}",axis.AxisNo), PortSetting.ReadWriteTimeOut,false);
                //System.Threading.Thread.Sleep(10);

                ret = SendPortCommand(string.Format("?99.{0}",axis.AxisNo), PortSetting.ReadWriteTimeOut, true);
                System.Threading.Thread.Sleep(10);
                if (!ret.Contains("Ux"))
                {
                    throw new MCoreExceptionPopup("Move Absolute Command fail {0}", this.Nickname);
                }


                WaitForMoveDone(10000);

                System.Threading.Thread.Sleep(30);
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

                        string ret = SendPortCommand("?99.1", PortSetting.ReadWriteTimeOut,true);
                        if (timeoutStopw.ElapsedMilliseconds > timeout)
                        {
                            throw new MCoreExceptionPopup("Cool Muscle [{0}] error: Time out wait move absolute position.", this.Name);
                        }

                        if (ret.Contains("Ux.1=8"))
                        {

                            break;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new MCoreExceptionPopup(ex, "Wait Move Done Fail {0}", this.Nickname);
                    }
                    Thread.Sleep(10);
                }
          
        }

        private string SendPortCommand(string command, Miliseconds timeout,bool isNeedReply)
        {
            
            //lock (_waitPortRead)
            //{
                while (_rs232Port.BytesToRead > 0)
                {
                    string existingMsg = _rs232Port.ReadExisting();
                    System.Threading.Thread.Sleep(30);
                }

                //_waitPortRead.Reset();
                string cmCommand = command;
                _rs232Port.DiscardInBuffer();
                _rs232Port.WriteLine(command);
                System.Threading.Thread.Sleep(10);
            //}

            if (isNeedReply)
            {
                try
                {
                    //U.BlockOrDoEvents(_waitPortRead, timeout.ToInt);

                    //string repeatCommand  ="";
                    //do
                    //{
                    //    repeatCommand = _rs232Port.ReadLine();
                    //    System.Threading.Thread.Sleep(30);
                    //} while (repeatCommand != command && _rs232Port.BytesToRead > 0);
                    
                    string returnQuery = _rs232Port.ReadLine();
                    System.Threading.Thread.Sleep(10);
                   
                    return returnQuery;
                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Timeout waiting for read port of '{0}' of  command: {1}", this.Nickname, command);
                }
            }

            return null;
        }
        


        //private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        lock (_waitPortRead)
        //        {
        //            _waitPortRead.Set();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new MCoreExceptionPopup(ex, "'{0}' received unexpected", Nickname);
        //    }
        //}




        #region Convert Unit
        public double RealPosionConvertToPulsePos(double dPosition)
        {
            double axisMovePerStep = AxisCyclePitch / MotorResolution;  //(mm/pulse);
            double positionToController = dPosition / axisMovePerStep;  //(pulse)
            return positionToController;
        }

        public double RealSpeedConvertToPulseSpeed( double dSpeed)
        {

            double axisMovePerStep = AxisCyclePitch / MotorResolution;  //(mm/pulse)
            double controllerBaseSpeed = 100.0;                         //(pulse/second)
            double baseSpeed = controllerBaseSpeed * axisMovePerStep;   //(um/second)
            double speeedToController = dSpeed / baseSpeed;
            if(speeedToController<1)
            {
              return 1;
            }
            if(speeedToController>5000)
            {
              return 5000;
            }
            return speeedToController;
        }

        public double RealAccelConvertToPulseAccel(double dAccel)
        {
            double axisMovePerStep = AxisCyclePitch / MotorResolution;  //(mm/pulse)
            double baseAccel = 1000*axisMovePerStep;                    //(um/sec/sec)
            double accelTocontroller = dAccel / baseAccel;
            if(accelTocontroller <1)
            {
              return 1;
            }
            if(accelTocontroller >5000)
            {
              return 5000;
            }
            return accelTocontroller;
        }

        #endregion



        #region Convert pulse to real
        public double PulsePosConvertToRealPos(double dCurrPosInPulse)
        {
           
            double axisMovePerStep = AxisCyclePitch / MotorResolution;     //(mm/pulse);
            double currentRealPosition = dCurrPosInPulse*axisMovePerStep;  //(pulse)

            return currentRealPosition;
        }

        public double PulseSpeedConvertToRealSpeed( double dSpeedInPulse)
        {
         
            double axisMovePerStep = AxisCyclePitch / MotorResolution;  //(mm/pulse)
            double controllerBaseSpeed = 100.0;                         //(pulse/second)
            double baseSpeed = controllerBaseSpeed * axisMovePerStep;   //(um/second)

            double realSpeed = dSpeedInPulse * baseSpeed;
            return realSpeed;
        }

        public double PulseAccelConvertToRealAccel(double dAccelInPulse)
        {
  
            double axisMovePerStep = AxisCyclePitch / MotorResolution;  //(um/pulse)
            double controllerBaseSpeed = 100.0;                         //(pulse/second)
            double baseSpeed = controllerBaseSpeed * axisMovePerStep;   //(mm/second)
            double baseAccel = 1000 * axisMovePerStep;                  //(mm/sec/sec)
            
            double realAccel = dAccelInPulse * baseAccel;
            return realAccel;
        }
        #endregion



    }
}
