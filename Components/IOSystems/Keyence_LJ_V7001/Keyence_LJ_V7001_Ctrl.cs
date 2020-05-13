using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Diagnostics;

using MCore.Comp;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.MeasurementSystem;



//using MCore.Comp.MeasurementSystem;
//using MCore.Comp.IOSystem;
using MDouble;


namespace MCore.Comp.MeasurementSystem
{
    public class Keyence_LJ_V7001_Ctrl: MeasurementSystemBase
    {
        #region Private data


        //private MillimeterInput _millimeterInput = null;
        private DispSensor _dispSensor = null;
        private string _millimeterInputString = "";

        private int _deviceID = 0;
        private LJV7IF_TARGET_SETTING _targetSetting;
        private byte _depth;
        private byte[] _data;

        #endregion



        #region Public Properties

        //public MillimeterInput millimeterInput
        //{
        //    get { return _millimeterInput; }
        //}


        public DispSensor DispSensor
        {
            get { return _dispSensor; }
        }


        //public string millimeterInputString
        //{
        //    get { return _millimeterInputString; }
        //}



        #endregion


        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Keyence_LJ_V7001_Ctrl()
        {
        }


        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public Keyence_LJ_V7001_Ctrl(string name) : base(name)

        {

        }



        #endregion Constructors




        #region Overrides


        /// <summary>
        /// Initailize this component
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }


            try
            {

                ConnectController();

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



        /// <summary>
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();

            //_millimeterInput = this.FilterByTypeSingle<MillimeterInput>();
            _dispSensor = this.FilterByTypeSingle<DispSensor>();

            if (_dispSensor == null)
            {
                U.LogPopup("Keyence LJ_V7001_Ctrl need millimeterInput");
            }
            else
            {
                OnChangeTriggerMode(_dispSensor.TriggerMode);
                U.RegisterOnChanged(() => _dispSensor.TriggerMode, OnChangeTriggerMode);
            }

        }



        public override void Destroy()
        {
            DisconnectController();
            base.Destroy();
        }


        /// <summary>
        /// Set Limit on Controller
        /// </summary>
        /// <param name="upperlimit"></param>
        /// <param name="lowerlimit"></param>
        public override void SetLimit(double limit)
        {

            _data = new byte[NativeMethods.ProgramSettingSize];
            _data[0] = (byte)limit;

        }


        //public void SetLimitSensor(string upperlimit, string lowerlimit, string programNo, string outputNo)
        //{

        //    // † There are three setting areas: a) the write settings area, b) the running area, and c) the save area.
        //    //   * Specify a) for the setting level when you want to change multiple settings. However, to reflect settings in the LJ-V operations, you have to call LJV7IF_ReflectSetting.
        //    //	 * Specify b) for the setting level when you want to change one setting but you don't mind if this setting is returned to its value prior to the change when the power is turned off.
        //    //	 * Specify c) for the setting level when you want to change one setting and you want this new value to be retained even when the power is turned off.

        //    // @Point
        //    //  As a usage example, we will show how to use SettingForm to configure settings such that sending a setting, with SettingForm using its initial values,
        //    //  will change the sampling period in the running area to "100 Hz."
        //    //  Also see the GetSetting function.

            
        //    // Set upper limit
        //    _depth = 0x01;						                        // Setting depth: Running settings area
        //    _targetSetting.byType = Convert.ToByte(programNo, 16);		// Setting type: Program number
        //    _targetSetting.byCategory = 0x06;	                        // Category: Output settings
        //    _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
        //    _targetSetting.byTarget1 = Convert.ToByte(outputNo);		// Setting target 1: output number
        //    _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
        //    _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
        //    _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
        //    SetLimit(Convert.ToInt16(upperlimit));

        //    uint dwError = 0;
        //    using (PinnedObject pin = new PinnedObject(_data))

        //    try 
        //    {
        //        Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

        //        if (rc != Rc.Ok)
        //        {
        //            U.LogPopup("Cannot set upper limit on sensor");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
        //    }

        //}




        public override void SetLimit(DispSensor dispSensor, double upperLimit, double lowerLimit)
        {
            base.SetLimit(dispSensor, upperLimit, lowerLimit);

            // † There are three setting areas: a) the write settings area, b) the running area, and c) the save area.
            //   * Specify a) for the setting level when you want to change multiple settings. However, to reflect settings in the LJ-V operations, you have to call LJV7IF_ReflectSetting.
            //	 * Specify b) for the setting level when you want to change one setting but you don't mind if this setting is returned to its value prior to the change when the power is turned off.
            //	 * Specify c) for the setting level when you want to change one setting and you want this new value to be retained even when the power is turned off.

            // @Point
            //  As a usage example, we will show how to use SettingForm to configure settings such that sending a setting, with SettingForm using its initial values,
            //  will change the sampling period in the running area to "100 Hz."
            //  Also see the GetSetting function.


            // Set upper limit
            _depth = 0x01;						                        // Setting depth: Running settings area
            _targetSetting.byType = (byte)(dispSensor.ProgramNo);		// Setting type: Program number
            _targetSetting.byCategory = 0x06;	                        // Category: Output settings
            _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
            _targetSetting.byTarget1 = (byte)(dispSensor.OutputNo);		// Setting target 1: output number
            _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
            _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
            _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
            SetLimit(upperLimit);

            uint dwError = 0;
            using (PinnedObject pin = new PinnedObject(_data))

                try
                {
                    Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

                    if (rc != Rc.Ok)
                    {
                        U.LogPopup("Cannot set upper limit on sensor");
                    }
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
                }





            // Set lower limit
            _depth = 0x01;						                        // Setting depth: Running settings area
            _targetSetting.byType = (byte)(dispSensor.ProgramNo);		// Setting type: Program number
            _targetSetting.byCategory = 0x06;	                        // Category: Output settings
            _targetSetting.byItem = 0x0E;		                        // Setting item: Upper limit
            _targetSetting.byTarget1 = (byte)(dispSensor.OutputNo);		// Setting target 1: output number
            _targetSetting.byTarget2 = 0x0;		                        // Setting target 2: None
            _targetSetting.byTarget3 = 0x0;		                        // Setting target 3: None
            _targetSetting.byTarget4 = 0x0;		                        // Setting target 4: None
            SetLimit(lowerLimit);


            using (PinnedObject pin = new PinnedObject(_data))

                try
                {
                    Rc rc = (Rc)NativeMethods.LJV7IF_SetSetting(_deviceID, _depth, _targetSetting, pin.Pointer, (uint)_data.Length, ref dwError);

                    if (rc != Rc.Ok)
                    {
                        U.LogPopup("Cannot set upper limit on sensor");
                    }
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Keyence_LJ_V7001_Ctrl.SetLimit error");
                }


        }
    



        private void ConnectController()
        {
            bool _initialized = false;

            // initialize DLL
            Rc rc = (Rc)NativeMethods.LJV7IF_Initialize();

            if (rc == Rc.Ok)
            {
                // Open USB port with deviceID=0
                rc = (Rc)NativeMethods.LJV7IF_UsbOpen(_deviceID);
                if (rc == Rc.Ok)
                {
                    _initialized = true;
                }
                else
                {
                    _initialized = false;
                }

            }
            else
            {
                _initialized = false;
            }


            Initialized = _initialized;

            if (Initialized)
            {
                U.LogInfo("Controller Connected");
            }
            else
            {
                U.LogPopup("Controller cannot be connected");
            }

        }

        private void DisconnectController()
        {
            bool _initialized = false;

            // Disconnect Controller
            Rc rc = (Rc)NativeMethods.LJV7IF_CommClose(_deviceID);

            if (rc == Rc.Ok)
            {
                // Disconnect DLL
                rc = (Rc)NativeMethods.LJV7IF_Finalize();
                if (rc == Rc.Ok)
                {
                    _initialized = false;
                }
                else
                {
                    _initialized = true;
                }

            }
            else
            {
                _initialized = true;
            }


            Initialized = _initialized;

            if (!Initialized)
            {
                U.LogInfo("Controller Disconnected");
            }
            else
            {
                U.LogPopup("Controller cannot be disconnected");
            }
        }



        private void OnChangeTriggerMode(CompMeasure.eTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case CompMeasure.eTriggerMode.Idle:
                    {
                        break;
                    }
                case CompMeasure.eTriggerMode.SingleTrigger:
                    {
                        SendBatchStart();
                        GetMeasurement();
                        SendBatchStop();

                        _dispSensor.TriggerMode = CompMeasure.eTriggerMode.Idle;
                        break;
                    }
                case CompMeasure.eTriggerMode.TimedTrigger:
                    {
                        break;
                    }
                case CompMeasure.eTriggerMode.Live:
                    {
                        break;
                    }
            }
        }


        private void SendBatchStart()
        {
            try
            {
                Rc rc = (Rc)NativeMethods.LJV7IF_StartMeasure(_deviceID);
                if (rc != Rc.Ok)
                {
                    U.LogPopup("Cannot send batch start to controller");
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Keyence_LJ_V7001.SendBatchStart");
            }
        }

        private void SendBatchStop()
        {
            try
            {
                Rc rc = (Rc)NativeMethods.LJV7IF_StopMeasure(_deviceID);
                if (rc != Rc.Ok)
                {
                    U.LogPopup("Cannot send batch stop to controller");
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Keyence_LJ_V7001.SendBatchStop");
            }

        }


        private void GetMeasurement()
        {
            int _timeout_ms = 1000;
            DateTime stop = DateTime.Now.AddTicks(_timeout_ms * 10000);

            try
            {
                LJV7IF_MEASURE_DATA[] measureData = new LJV7IF_MEASURE_DATA[NativeMethods.MeasurementDataCount];
                Rc rc1 = (Rc)NativeMethods.LJV7IF_GetMeasurementValue(_deviceID, measureData);
                if (rc1 != Rc.Ok) return;


                while ((measureData[0].byDataInfo != 0) && (DateTime.Now < stop))
                {
                    Rc rc = (Rc)NativeMethods.LJV7IF_GetMeasurementValue(_deviceID, measureData);
                    if (rc != Rc.Ok) return;
                    Application.DoEvents();
                   
                }
                
 
                MeasureData data = new MeasureData(measureData);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < NativeMethods.MeasurementDataCount; i++)
                {
                    sb.Append(string.Format("OUT {0:d2}:\t{1,0:f4}\r\n", (i + 1), measureData[i].fValue));
                }

                _dispSensor.ValueString = sb.ToString();

            }
            catch (Exception ex)
            {
                U.LogError(ex, "Keyence_LJ_V7001.GetMeasurement");
            }
        }


        #endregion Overrides



    }
}
