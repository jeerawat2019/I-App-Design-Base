using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading;

using MCore.Helpers;
using MCore.Comp.Communications;
using MCore.Comp;
using MCore.Comp.IOSystem.Input;

using MDouble;

namespace MCore.Comp.IOSystem
{
    public class MEDAQLib : IOSystemBase
    {
        #region privates

        private UInt32 _iSensor = 0;
        private UInt32 _hSensorDataReady = 0;
        private AbortableBackgroundWorker _bw = null;
        private const UInt32 INFINITE = 0xffffffff;
        private MillimeterInput _mmInput = null;
        private object _lockCommands = new object();
        // Used for controllers to wait for move completion
        // Same as Reset (Blocks the thread)
        private ManualResetEvent _waitMeasureDone = new ManualResetEvent(true);

        #endregion private Data

        #region Public definitions
        public enum eExtInputMode
        {
            Used_for_teaching,
            Used_as_trigger
        }
        public enum eMeasurementRate
        {
            kHz_1_5 = 0,
            kHz_1_0 = 1,
            Hz_750 = 2,
            Hz_375 = 3,
            Hz_50 = 4,
        }
        public enum eMedianChoices
        {
            Median_3 = 3,
            Median_5 = 5,
            Median_7 = 7,
            Median_9 = 9
        }
        public enum eAcquisitionModes
        {
            Continuous,
            Time_based,
            Hardware_triggerred,
        }

        public enum eGetInfo
        {
            SA_Sensor,
            SA_SensorType,
            SA_ArticleNumber,
            SA_Option,
            SA_SerialNumber,
            SA_Range,
            SA_Softwareversion,
            SA_BootLoaderVer,
            SA_Date,
            SA_OutputType,
        }
        #endregion Public definitions

        #region Public Properties

        public MillimeterInput MMInput
        {
            get { return _mmInput; }
        }

        /// <summary>
        /// Get / Set the Ext Input mode
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("The Ext Input mode")]
        public eExtInputMode ExtInputMode
        {
            get { return GetPropValue(() => ExtInputMode, eExtInputMode.Used_as_trigger); }
            set { SetPropValue(() => ExtInputMode, value); }
        }


        /// <summary>
        /// Get / Set the measurement speed
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("The measurement speed")]
        public eMeasurementRate MeasurementRate
        {
            get { return GetPropValue(() => MeasurementRate, eMeasurementRate.kHz_1_5); }
            set { SetPropValue(() => MeasurementRate, value); }
        }


        /// <summary>
        /// Moving count for averaging
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("Moving count for averaging")]
        public int MovingCount
        {
            get { return GetPropValue(() => MovingCount, 1); }
            set { SetPropValue(() => MovingCount, value); }
        }


        /// <summary>
        /// True if median averaging
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("True if median averaging")]
        public bool MedianAvg
        {
            get { return GetPropValue(() => MedianAvg); }
            set { SetPropValue(() => MedianAvg, value); }
        }

        /// <summary>
        /// Get / Set the median choices
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("The median choices")]
        public eMedianChoices MedianChoice
        {
            get { return GetPropValue(() => MedianChoice, eMedianChoices.Median_3); }
            set { SetPropValue(() => MedianChoice, value); }
        }

        /// <summary>
        /// Get / Set the Read value
        /// </summary>
        [Browsable(true)]
        [Category("Measurement")]
        [Description("The Read value")]
        public Millimeters ReadValue
        {
            get { return GetPropValue(() => ReadValue, 0); }
            set { SetPropValue(() => ReadValue, value); }
        }

        /// <summary>
        /// Get the RS 232 device
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public RS232 Port
        {
            get { return GetPropValue(() => Port); }
            set { SetPropValue(() => Port, value); }
        }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MEDAQLib()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MEDAQLib(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Initialize this Component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Initialize 
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            Port = GetParent<RS232>();
            if (Port == null)
            {
                throw new ForceSimulateException("MEDAQLib needs an RS232 parent");
            }

            try
            {
                _iSensor = MEDAQLibDLL.CreateSensorInstance(MEDAQLibDLL.ME_SENSOR.SENSOR_ILD1402);
                if (_iSensor == 0)
                {
                    throw new Exception("CreateSensorInstance: Error occured, no sensor created");
                }

                lock (_lockCommands)
                {
                    Open();
                }

                SetupGetInfo();
                SetupCallback();

                // Write to controller
                OnMeasurementRate(MeasurementRate);
                SetAverageParameters();
                OnExtInputMode(ExtInputMode);
                _mmInput = this.FilterByTypeSingle<MillimeterInput>();
                if (MMInput == null)
                {
                    throw new ForceSimulateException("MEDAQLib needs a MDoubleInput");
                }

                OnChangedExternalTrigger(_mmInput.ExternalTrigger);
                OnChangedTriggerMode(MMInput.TriggerMode);
                OnOutputTime(MMInput.TriggerInterval);

                U.RegisterOnChanged(() => MMInput.TriggerMode, OnChangedTriggerMode);
                U.RegisterOnChanged(() => MMInput.ExternalTrigger, OnChangedExternalTrigger);
                U.RegisterOnChanged(() => MMInput.TriggerInterval, OnOutputTime);
                U.RegisterOnChanged(() => MeasurementRate, OnMeasurementRate);
                U.RegisterOnChanged(() => MedianAvg, OnMedianAvg);
                U.RegisterOnChanged(() => MovingCount, OnMovingCount);
                U.RegisterOnChanged(() => MedianChoice, OnMedianChoice);
                U.RegisterOnChanged(() => ExtInputMode, OnExtInputMode);
               _bw.RunWorkerAsync();
                Simulate = eSimulate.None;
            }
            catch (ForceSimulateException fsex)
            {
                throw fsex;
            }
            catch (Exception ex)
            {
                throw new ForceSimulateException(ex);
            }
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
        }
        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="timeout"></param>
        public override bool Measure(CompMeasure input, Miliseconds timeout)
        {

            lock (_waitMeasureDone)
            {
                _waitMeasureDone.Reset();
                Trigger(input);
            }
            try
            {
                U.BlockOrDoEvents(_waitMeasureDone, timeout.ToInt);
                return true;
            }
            catch
            {
                U.LogError("Timeout waiting for measure of '{0}'", input.Nickname);
            }
            return false;

        }
        /// <summary>
        /// Destroy this component
        /// </summary>
        public override void Destroy()
        {
            base.Destroy();  // Sets IsDestroying flag

            try
            {
                if (_iSensor != 0)
                {
                    if (_hSensorDataReady != 0)
                    {
                        // Complete worker thread
                        MEDAQLibDLL.SetEvent(_hSensorDataReady);
                        Thread.Sleep(50);
                    }
                    MEDAQLibDLL.CloseSensor(_iSensor);
                    MEDAQLibDLL.ReleaseSensorInstance(_iSensor);
                }
                if (_bw != null)
                {
                    if (_bw.IsBusy)
                    {
                        _bw.CancelAsync();
                    }
                    _bw.Dispose();
                    _bw = null;
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Error disposing '{0}'", Nickname);
            }
            finally
            {
                _bw = null;
            }
        }

        #endregion Overrides

        private void OnChangedExternalTrigger(bool externalTrigger)
        {
            if (externalTrigger)
            {
                SetTriggerMode(eAcquisitionModes.Hardware_triggerred);
            }
            else
            {
                SetTriggerMode(eAcquisitionModes.Continuous);
            }
        }

        private void OnChangedTriggerMode(CompMeasure.eTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case CompMeasure.eTriggerMode.Idle:
                    //DataOff();
                    break;
                case CompMeasure.eTriggerMode.SingleTrigger:
                    SetTriggerMode(eAcquisitionModes.Continuous);
                    DataOn();
                    GetSingleTrigger(MMInput);
                    MMInput.TriggerMode = CompMeasure.eTriggerMode.Idle;
                    break;
                case CompMeasure.eTriggerMode.TimedTrigger:
                    SetTriggerMode(eAcquisitionModes.Time_based);
                    DataOn();
                    break;
                case CompMeasure.eTriggerMode.Live:
                    SetTriggerMode(eAcquisitionModes.Continuous);
                    DataOn();
                    break;
            }
        }

        private void SetupCallback()
        {
            SetParameterInt("IP_EventOnAvailableValues", 1);
            SensorCommand("DataAvail_Event");
            _hSensorDataReady = GetParameterDWORD("IA_DataAvailEvent");

            if (_hSensorDataReady != 0)
            {
                _bw = new AbortableBackgroundWorker() { WorkerSupportsCancellation = true };
                _bw.DoWork += new DoWorkEventHandler(WaitForData);
            }
        }

        #region Set Commands


        private void DataOn()
        {
            try
            {
                SensorCommand("Dat_Out_On");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }

        private void DataOff()
        {
            try
            {
                //SetParameterInt("IP_ClearRingBuffer", 1);
                SensorCommand("Dat_Out_Off");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }

        private void SetTriggerMode(eAcquisitionModes acqMode)
        {
            try
            {
                SetParameterInt("SP_OutputMode", (int)acqMode);
                SensorCommand("Set_OutputMode");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }

        private void OnOutputTime(Miliseconds outputTime)
        {
            try
            {
                int iVal = Math.Max(1, outputTime.ToInt);
                SetParameterInt("SP_OutputTime", iVal);
                SensorCommand("Set_OutputTime");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }

        private void OnExtInputMode(eExtInputMode extInputMode)
        {
            try
            {
                SetParameterInt("SP_ExtInputMode", (int)extInputMode);
                SensorCommand("Set_ExtInputMode");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }
        private void OnMeasurementRate(MEDAQLib.eMeasurementRate en)
        {
            try
            {
                SetParameterInt("SP_Speed", (int)en);
                SensorCommand("Set_Speed");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }


        private void SetAverageParameters()
        {
            try
            {
                int movingCount = Math.Max(MovingCount, 1);
                movingCount = Math.Min(movingCount, 129);
                SetParameterInt("SP_AvType", MedianAvg ? 1 : 0);
                SetParameterInt("SP_MovingCount", movingCount);
                SetParameterInt("SP_MedianIndex", (int)MedianChoice);
                SensorCommand("Set_Av");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }

        private void OnMedianAvg(bool medianAvg)
        {
            SetAverageParameters();
        }

        private void OnMovingCount(int movingCount)
        {
            SetAverageParameters();
        }
        private void OnMedianChoice(eMedianChoices medianChoices)
        {
            SetAverageParameters();
        }
        #endregion Set Commands

        #region Get commands

        private string GetError()
        {
            StringBuilder sbSensorError = new StringBuilder(1024);
            MEDAQLibDLL.GetError(_iSensor, sbSensorError, (UInt32)sbSensorError.Capacity);

            return sbSensorError.ToString();
        }

        private void SetParameterInt(string paramName, int iVal)
        {
            if (_iSensor == 0)
            {
                return;
            }
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
                iRet = MEDAQLibDLL.SetParameterInt(_iSensor, paramName, iVal);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with SetParameterInt('{0}', '{1}') : '{2}'", paramName, iVal, GetError());
                }
            }
        }

        private void SetParameterString(string paramName, string val)
        {
            if (_iSensor == 0)
            {
                return;
            }
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
                iRet = MEDAQLibDLL.SetParameterString(_iSensor, paramName, val);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with SetParameterString('{0}', '{1}') : '{2}'", paramName, val, GetError());
                }
            }
        }

        private void SensorCommand(string cmd)
        {
            if (_iSensor == 0)
            {
                return;
            }
            SetParameterString("S_Command", cmd);
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
                iRet = MEDAQLibDLL.SensorCommand(_iSensor);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with SensorCommand : '{0}'", GetError());
                }
            }
        }
        private string GetParameterString(string paramName)
        {
            if (_iSensor == 0)
            {
                return string.Empty;
            }
            string ret = string.Empty;
            lock (_lockCommands)
            {
                UInt32 iMaxLen = 1024;
                StringBuilder sb = new StringBuilder((int)iMaxLen);
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.GetParameterString(_iSensor, paramName, sb, ref iMaxLen);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with GetParameterString('{0}' : '{1}'", paramName, GetError());
                }
                ret = sb.ToString();
            }
            return ret;
        }
        private UInt32 GetParameterDWORD(string paramName)
        {
            if (_iSensor == 0)
            {
                return 0;
            }
            UInt32 dword = 0;
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
                iRet = MEDAQLibDLL.GetParameterDWORD(_iSensor, paramName, ref dword);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with GetParameterDWORD('{0}' : '{1}'", paramName, GetError());
                }
            }
            return dword;
        }
        private double GetParameterDouble(string paramName)
        {
            if (_iSensor == 0)
            {
                return 0;
            }
            double dVal = 0;
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.GetParameterDouble(_iSensor, paramName, ref dVal);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with GetParameterDouble('{0}' : '{1}'", paramName, GetError());
                }
            }
            return dVal;
        }

        private int GetParameterInt(string paramName)
        {
            if (_iSensor == 0)
            {
                return 0;
            }
            int iVal = 0;
            lock (_lockCommands)
            {
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.GetParameterInt(_iSensor, paramName, ref iVal);
                if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    throw new MCoreExceptionPopup("Error with GetParameterInt('{0}' : '{1}'", paramName, GetError());
                }
            }
            return iVal;
        }
        public void SetupGetInfo()
        {
            try
            {
                SensorCommand("Get_Info");
                // Give time for sensor answer...;
                Thread.Sleep(300);
            }
            catch( MCoreException mex)
            {
                U.Log(mex);
            }
        }
        public string GetInfoItem(eGetInfo getInfo)
        {
            string ret = string.Empty;
            try
            {
                switch (getInfo)
                {
                    case eGetInfo.SA_OutputType:
                        // int
                        int iVal = GetParameterInt(getInfo.ToString());
                        ret = iVal.ToString();
                        break;
                    case eGetInfo.SA_Range:
                        // double
                        double dVal = GetParameterDouble(getInfo.ToString());
                        ret = dVal.ToString();
                        break;
                    default:
                        // Assume string
                        ret = GetParameterString(getInfo.ToString());
                        break;
                }            
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
            return ret;
            
        }

        #endregion Get commands

        /// <summary>
        /// Trigger this input and set the value
        /// </summary>
        /// <param name="input"></param>
        private void GetSingleTrigger(MillimeterInput input)
        {
            if (input != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    double[] vals = GetData();
                    if (vals != null && vals.Length > 0)
                    {
                        double val = vals[vals.Length - 1];
                        if (val != -1.0)
                        {
                            input.Value = vals[vals.Length - 1];
                            _waitMeasureDone.Set();
                            return;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
        }



        private MEDAQLibDLL.ERR_CODE Open()
        {
            if (_iSensor == 0)
            {
                return MEDAQLibDLL.ERR_CODE.ERR_CANNOT_OPEN;
            }
            MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
            iRet = MEDAQLibDLL.ClearAllParameters(_iSensor);
            if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            {
                throw new Exception("MEDAQLibDLL.ClearAllParameters");
            }

            SetParameterString("IP_Interface", "RS232");
            SetParameterString("IP_Port", Port.CommPort.ToString());

            //iRet = MEDAQLibDLL.SetParameterInt(_iSensor, "IP_Baudrate", (int)CommSettings.BaudRate);
            //if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            //{
            //    throw new Exception("SetParameterInt(IP_Baudrate)");
            //}

            //iRet = MEDAQLibDLL.SetParameterInt(_iSensor, "IP_Stopbits", (int)CommSettings.StopBits);
            //if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            //{
            //    throw new Exception("SetParameterInt(IP_Stopbits)");
            //}

            //iRet = MEDAQLibDLL.SetParameterInt(_iSensor, "IP_Parity", (int)CommSettings.Parity);
            //if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            //{
            //    throw new Exception("SetParameterInt(IP_Parity)");
            //}

            //iRet = MEDAQLibDLL.SetParameterInt(_iSensor, "IP_ByteSize", (int)CommSettings.DataLength);
            //if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            //{
            //    throw new Exception("SetParameterInt(IP_ByteSize)");
            //}

            iRet = MEDAQLibDLL.OpenSensor(_iSensor);
            if (iRet != MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
            {
                throw new Exception(string.Format("OpenSensor {0}", Port.ToString()));
            }
            return MEDAQLibDLL.ERR_CODE.ERR_NOERROR;
        }
        /// <summary>
        /// Get the current data values
        /// </summary>
        /// <returns></returns>
        public double[] GetData()
        {
            lock (_lockCommands)
            {
                int iAvail = 0;
                int iRead = 0;
                MEDAQLibDLL.ERR_CODE iRet = MEDAQLibDLL.DataAvail(_iSensor, ref iAvail);
                if (iRet == MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                {
                    // Get no more than 10
                    int iMax = System.Math.Min(iAvail, 10);
                    int[] iRawData = new Int32[iMax];
                    double[] dScaledData = new double[iMax];
                    iRet = MEDAQLibDLL.TransferData(_iSensor, iRawData, dScaledData, iMax, ref iRead);
                    if (iRet == MEDAQLibDLL.ERR_CODE.ERR_NOERROR)
                    {
                        return dScaledData;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Update the "Laser Level" data parameter
        /// </summary>
        /// <param name="input"></param>
        private void WaitForData(object sender, DoWorkEventArgs e)
        {
            while (!IsDestroying)
            {
                try
                {
                    switch (MEDAQLibDLL.WaitForSingleObject(_hSensorDataReady, 500))
                    {
                        case MEDAQLibDLL.WAIT_OBJECT_0:
                            lock (_waitMeasureDone)
                            {
                                double[] vals = GetData();
                                if (vals != null && vals.Length > 0)
                                {
                                    double val = vals[vals.Length - 1];
                                    MMInput.Value = val;
                                    if (val != -1.0)
                                    {
                                        _waitMeasureDone.Set();
                                    }
                                }
                            }
                            break;
                        case MEDAQLibDLL.WAIT_TIMEOUT:
                            Thread.Sleep(20);
                            break;
                        case MEDAQLibDLL.WAIT_FAILED:
                            Thread.Sleep(20);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Problem reading sensor.");
                    Thread.Sleep(500);
                }
            }
            U.LogInfo("{0} thread completed.", Nickname);
        }
    }
}
