using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;

using MCore.Comp;
using MCore.Comp.Geometry;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using MCore.Comp.MotionSystem;
using MDouble;

// Aerotech References =>
using Aerotech.A3200;
using Aerotech.A3200.Properties;
using Aerotech.A3200.Units;
using Aerotech.A3200.Commands;
using Aerotech.A3200.Configuration;
using Aerotech.A3200.Tasks;
using Aerotech.A3200.Status;
using Aerotech.A3200.Status.Custom;
using Aerotech.A3200.Exceptions;
using Aerotech.A3200.Variables;
using Aerotech.A3200.Callbacks;
using Aerotech.Common.Collections;
using Aerotech.A3200.Parameters;
using Aerotech.A3200.DataCollection;
// <=

namespace MCore.Comp.MotionSystem
{
    public class AerotechV_XX : MotionSystemBase
    {
        [XmlIgnore]
        public bool UseTempPattern
        {
            get { return GetPropValue(() => UseTempPattern); }
            set { SetPropValue(() => UseTempPattern, value); }
        }

        // Create Aerotech Controller Object =>
        private Controller _myControler = null;
        private CustomDiagnostics _customDiagnostics = null;
        private string _taskState = null;
        private double[] _globalDouble = null;
        private double[] _taskDouble = null;
        BoolInput[] _WagoDigitalInputList = null;
        private ManualResetEvent _pollingDone = new ManualResetEvent(false);
         // <=


        private volatile bool _destroy = false;
        private Object _lockCommand = new Object();

        private int _loopInterval = 20;
        private BackgroundWorker _updateLoop = null;
        private BackgroundWorker _counterLoop = null;

        private const string INITCMDS =
            "ENCODER OUT X ON 4, 1, 2\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,27,0\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,26,0\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,5,0\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,15,0\r\n" +
            "PSOCONTROL YP RESET\r\n" +
            "PSOOUTPUT YP CONTROL 0 1\r\n" +
            "PSOTRACK YP INPUT 0\r\n" +
            "PSOPULSE YP TIME 4000, 2500\r\n" +
            "PSOOUTPUT YP PULSE\r\n" +
            "PSOCONTROL YYP RESET\r\n" +
            "PSOOUTPUT YYP CONTROL 0 1\r\n" +
            "PSOTRACK YYP INPUT 0\r\n" +
            "PSOPULSE YYP TIME 4000, 2500\r\n" +
            "PSOOUTPUT YYP PULSE\r\n" +
            "PSOCONTROL X RESET\r\n" +
            "PSOOUTPUT X CONTROL 0 0\r\n" +
            "PSOTRACK X INPUT 0\r\n" +
            "PSOPULSE X TIME 4000, 2500\r\n" +
            "PSOOUTPUT X PULSE\r\n" +
            "PSOCONTROL XXP RESET\r\n" +
            "PSOOUTPUT XXP CONTROL 0 1\r\n" +
            "PSOTRACK XXP INPUT 0\r\n" +
            "PSOPULSE XXP TIME 4000, 2500\r\n" +
            "PSOOUTPUT XXP PULSE\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,27,1\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,26,1\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,5,1\r\n" +
            "MODBUSBIT X,MASTEROUTPUTBITS,15,1\r\n" +
            "ABSOLUTE";

        private const string INITDIAGINPUTNAME = "$Input0to31\r\n" + "$Input32to63";
        
        //private const Millimeters INITWINDOWSAFTEYMARGIN = new Millimeters(0.1);
        //private const Millimeters INITROUNDEDCORNERTOLERANCE = new Millimeters(0.05);

        private const int CamTableMaxCount = 100;

        //private List<int> _camTableList = new List<int>();
        //private int GetCamTable(GLine line, RealAxes axes)
        //{
        //    int index = -1;
        //    if (line.CamID != 0)
        //    {
        //        try
        //        {
        //            index = _camTableList.FindIndex((c) => c == line.CamID);
        //        }
        //        catch (Exception ex)
        //        {
        //            U.LogError(ex, "Problem looking for index to Cam Table");
        //        }
        //        if (index < 0)
        //        {
        //            index = _camTableList.Count;
        //            if (index >= 99)
        //            {
        //                U.LogPopup("Exceeded number of Cam Tables.  Please restart application");
        //                return -1;
        //            }
        //            lock (axes)
        //            {
        //                this._myControler.Variables.Global.Doubles.SetMultiple(index * CamTableMaxCount, line.CamTableArray);
        //            }
        //            // Load table and add to list
        //            _camTableList.Add(line.CamID);
        //        }
        //    }
        //    return index;
        //}

        #region Browseable Property

        /// <summary>
        /// Set Abort to test problem recovery
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Set Abort to test problem recovery")]
        [XmlIgnore]
        public bool TestAbort
        {
            get { return GetPropValue(() => TestAbort); }
            set { SetPropValue(() => TestAbort, value); }
        }
        /// <summary>
        /// Use PSO Counter to verify PSO
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Use PSO Counter to verify PSO")]
        public bool UsePSOCounter
        {
            get { return GetPropValue(() => UsePSOCounter); }
            set { SetPropValue(() => UsePSOCounter, value); }
        }
        /// <summary>
        /// The initial commands to send to the controller
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The initial commands to send to the controller")]
        public string InitializeCommands
        {
            get { return GetPropValue(() => InitializeCommands, INITCMDS); }
            set { SetPropValue(() => InitializeCommands, value); }
        }

        /// <summary>
        /// TaskStates Timeout when to dump CPU Usage
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("TaskStates Timeout when to dump CPU Usage")]
        public Miliseconds TaskStatesTimeout
        {
            get { return GetPropValue(() => TaskStatesTimeout, 750); }
            set { SetPropValue(() => TaskStatesTimeout, value); }
        }

        
        /// <summary>
        /// The Names for Aerotech Custom Diagnostic Variables to update Inputs
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The initial commands to send to the controller")]
        public string InitializeDiagInputName
        {
            get { return GetPropValue(() => InitializeDiagInputName, INITDIAGINPUTNAME); }
            set { SetPropValue(() => InitializeDiagInputName, value); }
        }

        /// <summary>
        /// THe interval of the TaskStatus update loop in ms
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The interval of the TaskStatus update loop in ms")]
        public int LoopIntervalTask
        {
            get { return GetPropValue(() => LoopIntervalTask, 100); }
            set { SetPropValue(() => LoopIntervalTask, value); }
        }

        ///// <summary>
        ///// THe interval of the DiagStatus update loop in ms
        ///// </summary>
        //[Browsable(true)]
        //[Category("AeroTech")]
        //[Description("The interval of the TaskStatus update loop in ms")]
        //public int LoopIntervalDiag
        //{
        //    get { return GetPropValue(() => LoopIntervalDiag, 25); }
        //    set { SetPropValue(() => LoopIntervalDiag, value); }
        //}

        /// <summary>
        /// The interval of the Background Worker Thread update loop in ms
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The interval of the Background Worker Thread update loop in ms")]
        public int LoopInterval
        {
            get { return GetPropValue(() => LoopInterval, _loopInterval); }
            set { SetPropValue(() => LoopInterval, value); }
        }
        
        /// <summary>
        /// The Dwell command to use before manually Firing PSO
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The Dwell command to use before manually Firing PSO")]
        public string PreManualFireDwell
        {
            get { return GetPropValue(() => PreManualFireDwell, "DWELL 1"); }
            set { SetPropValue(() => PreManualFireDwell, value); }
        }
        /// <summary>
        /// The Dwell command to use before Arming PSO
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("The Dwell command to use before Arming PSO")]
        public string PreArmDwell
        {
            get { return GetPropValue(() => PreArmDwell, "DWELL 0.015"); }
            set { SetPropValue(() => PreArmDwell, value); }
        }
        #region Aerotech Settings


        /// <summary>
        /// Task State
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [ReadOnly(true)]
        public string GetTaskState
        {
            get { return _taskState; }
        }

        /// <summary>
        /// Global Double
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        public double[] GetGlobalDouble
        {
            get { return _globalDouble; }
        }

        /// <summary>
        /// Task Double
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        public double[] GetTaskDouble
        {
            get { return _taskDouble; }
        }


        /// <summary>
        ///  MFO (Manual Feedrate Override: Min=0.01; Max=200)
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Manual Feedrate Override: Min=0.01; Max=200")]
        public double MFO
        {
            get { return GetPropValue(() => MFO, 100); }
            set 
            { 
                if (value <= 0.01) 
                {
                    value = 0.01;
                }
                if (value >= 200) 
                {
                    value = 200;
                }
                SetPropValue(() => MFO, value);
            }
        }

        /// <summary>
        ///  Tolerance for Rounded Corners during Blended Moves
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Tolerance for Rounded Corners during Blended Moves")]
        public Millimeters RoundedCornerTolerance
        {
            get { return GetPropValue(() => RoundedCornerTolerance, 1); }
            set { SetPropValue(() => RoundedCornerTolerance, value); }
        }

        /// <summary>
        ///  Enable Rounded Corners during Blended Moves
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Enable Rounded Corners during Blended Moves")]
        public bool RoundedCornerEnable
        {
            get { return GetPropValue(() => RoundedCornerEnable); }
            set { SetPropValue(() => RoundedCornerEnable, value); }
        }

        /// <summary>
        /// Background Task for Status Update
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Set Background Task for Status Update")]
        public TaskId BackgroundTask
        {
            get { return GetPropValue(() => BackgroundTask, TaskId.T09); }
            set { SetPropValue(() => BackgroundTask, value); }
        }

        /// <summary>
        /// Task for Drive Info Update
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Set TaskId for DriveInfo Update")]
        public TaskId DriveInfoTask
        {
            get { return GetPropValue(() => DriveInfoTask, TaskId.T08); }
            set { SetPropValue(() => DriveInfoTask, value); }
        }

        /// <summary>
        ///  PSOPulseOnOffTime in micro sec
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Default PSO Pulse ON+OFF Time")]
        public double PSOPulseOnOffTime
        {
            get { return GetPropValue(() => PSOPulseOnOffTime, 4000); }
            set { SetPropValue(() => PSOPulseOnOffTime, value); }
        }

        /// <summary>
        ///  PSOPulseOnTime in micro sec
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Default PSO Pulse ON Time")]
        public double PSOPulseOnTime
        {
            get { return GetPropValue(() => PSOPulseOnTime, 2500); }
            set { SetPropValue(() => PSOPulseOnTime, value); }
        }

        /// <summary>
        ///  Number of max. TriggerPoints per Pattern
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Number of DAQ storage Values per Axis\n" +
            "(Dependend on Number of available Global Doubles)")]
        public int OffsetArrayDAQ
        {
            get { return GetPropValue(() => OffsetArrayDAQ, 500); }
            set { SetPropValue(() => OffsetArrayDAQ, value); }
        }

        /// <summary>
        ///  Limit TriggerPoints per Pattern with Blended Move
        ///  Default = 60;
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Limit of TriggerPoints per Pattern with Blended Move\n" +
            "Starting Task Double address for storing Window borders\n" +
            "(Dependend on Number of available Task Doubles)")]
        public int OffsetArrayWindow
        {
            get { return GetPropValue(() => OffsetArrayWindow, 100); }
            set { SetPropValue(() => OffsetArrayWindow, value); }
        }

        /// <summary>
        ///  Margin between LineStartPoint and start of window
        /// </summary>
        [Browsable(true)]
        [Category("AeroTech")]
        [Description("Margin between LineStartPoint and start of window")]
        public Millimeters WindowSafetyMargin
        {
            get { return GetPropValue(() => WindowSafetyMargin, 1); }
            set { SetPropValue(() => WindowSafetyMargin, value); }
        }


        #endregion Aerotech Settings

        #endregion Browseable Property

        #region Private

        /// <summary>
        /// Send Initialization Commands to the Controller
        /// </summary>
        private void SendInitialCommands()
        {
            ExecuteCommand(InitializeCommands);
        }

        private volatile bool _counterBusy = false;

        private object _counterLockObj = new object();
        private int _countCounter = 0;
        private double _minTime = double.MaxValue;
        private double _maxTime = double.MinValue;
        private Dictionary<string, double> _counterElapsed = new Dictionary<string, double>();
        /// <summary>
        /// Update Trigger Counter
        /// </summary>
        private int UpdateCounter(RealAxis psoAxisReport)
        {
            if (_counterBusy)
                return 0;
            _counterBusy = true;
            int counterRet = 0;
            long enterTick = U.DateTimeNow;
            lock (_counterLockObj)
            {
                RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
                double dElapsed = U.TicksToMS(U.DateTimeNow - enterTick);
                if (dElapsed > LoopIntervalTask * 2)
                {
                    U.LogError("Waited too long for lock {0} mS", dElapsed);
                }
                _countCounter++;
                foreach (RealAxis psoAxis in axisList)
                {
                    if (psoAxis.PSOAxisKey != null && psoAxis.PSOEncoders != null && psoAxis.PSOEncoders.Length == 2)
                    {
                        int counterVal = 0;
                        long now = U.DateTimeNow;
                        CounterInput[] currentCounter = psoAxis.FilterByType<CounterInput>();
                        if (this._myControler != null && currentCounter != null && currentCounter.Length > 0)
                        {
                            // => Update Counter
                            string psoAxisName = psoAxis.Name;
                            try
                            {
                                AxesBase axes = psoAxis.Parent as AxesBase;
                                string commandDriveInfo = "DRIVEINFO(" + psoAxisName + ", DRIVEINFO_DataAcquisitionSamples)";
                                lock (axes)
                                {
                                    // Prepare Aerotech Command to read PSO Counter
                                    //string commandDriveInfo = String.Format("TIMER 0 CLEAR{0}$task[0] = DRIVEINFO({1}, DRIVEINFO_DataAcquisitionSamples){0}$task[1] = TIMER(0)", Environment.NewLine, psoAxisName);

                                    now = U.DateTimeNow;
                                    if (TestAbort && axes.AxesNo == 1)
                                    {
                                        this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                                    }
                                    else
                                    {
                                        counterVal = Convert.ToInt32(this._myControler.Commands[DriveInfoTask].Execute(commandDriveInfo));
                                    }
                                }
                                dElapsed = U.TicksToMS(U.DateTimeNow - now);
                                _minTime = Math.Min(_minTime, dElapsed);
                                _maxTime = Math.Max(_maxTime, dElapsed);
                                if (!_counterElapsed.ContainsKey(psoAxisName))
                                {
                                    _counterElapsed.Add(psoAxisName, dElapsed);
                                }
                                else
                                {
                                    _counterElapsed[psoAxisName] += dElapsed;
                                }
                                // Debug.WriteLine("{0}.DRIVEINFO call {1} mS", psoAxisName, dElapsed);
                                if (dElapsed > 60)
                                {
                                    //U.LogWarning("Long wait for {0}.DRIVEINFO call {1} mS num DATAACQ pts={2}  Aerotime={3}", 
                                      //  psoAxisName, dElapsed, data[0], data[1]);
                                    U.LogWarning("Long wait for {0}.DRIVEINFO({1}) call {2} mS", psoAxisName, psoAxisReport != null ? psoAxisReport.Name : "" , dElapsed);
                                }
                                now = U.DateTimeNow;
                                // Set current PSO Count to 'axis.DISPCOUNTER'
                                if (currentCounter[0].Value != counterVal)
                                {
                                    currentCounter[0].Value = counterVal;
                                }
                            }// try 1st <==
                            catch (Exception ex)
                            {
                                try
                                {
                                    U.LogPopup(ex, "UpdateCounter({0}). 1st exception. axis={1}, time={2}",
                                        psoAxisReport != null, psoAxisName, U.TicksToMS(U.DateTimeNow - now));
                                    //ClearAllFaults();
                                    //this._myControler.Commands[BackgroundTask].Execute("DWELL 0.1");

                                    now = U.DateTimeNow;
                                    // Prepare Aerotech Command to read PSO Counter
                                    string commandDriveInfo = "DRIVEINFO(" + psoAxisName + ", DRIVEINFO_DataAcquisitionSamples)";

                                    counterVal = Convert.ToInt32(this._myControler.Commands[DriveInfoTask].Execute(commandDriveInfo));
                                    // Set current PSO Count to 'axis.DISPCOUNTER'
                                    currentCounter[0].Value = counterVal;
                                }// try 2nd <==
                                catch (Exception e)
                                {
                                    U.LogError(e, "UpdateCounter. 2nd exception time={0}", U.TicksToMS(U.DateTimeNow - now));
                                }// catch (Exception e) <==
                            }// catch (Exception ex) <==
                            // <= Update Counter
                            if (psoAxisReport != null && psoAxisReport.Name == psoAxisName)
                            {
                                counterRet = counterVal;
                            }
                        }// if (currentCounter != null && this._myControler != null) <==
                    }
                }
            }
            if (_countCounter >= 20 && _counterElapsed.Count > 0)
            {
                for (int i=0; i< _counterElapsed.Count; i++)
                {
                    KeyValuePair<string, double> keyVal = _counterElapsed.ElementAt(i);
                    double avg = keyVal.Value / _countCounter;
                    if (avg > 25)
                    {
                        U.LogInfo("Axis {0} average counter time = {1} ms   Min={2}  Max={3}", keyVal.Key, avg, _minTime, _maxTime);
                        _minTime = double.MaxValue;
                        _maxTime = double.MinValue;
                    }
                    _counterElapsed[keyVal.Key] = 0;                    
                }
                _countCounter = 1;
            }
            //Debug.WriteLine("Update duration={0}", dmS);
            _counterBusy = false;
            return counterRet;
        }

        /// <summary>
        /// Reset Trigger Counter
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="psoAxis"></param>
        private void ResetCounter(RealAxes axes)
        {
            RealAxis[] currentAxis = axes.FilterByType<RealAxis>();
            RealAxis psoAxis = null;

            foreach (RealAxis axis in currentAxis)
            {
                if (axis.PSOAxisKey != null && axis.PSOEncoders != null && axis.PSOEncoders.Length == 2)
                {
                    psoAxis = axis;
                    CounterInput[] currentCounter = psoAxis.FilterByType<CounterInput>();
            
                    if (currentCounter != null && this._myControler != null)
                    {

                        lock (axes)
                        {
                            // => Setup DAQ for TriggerPoint tracking
                            //DATAACQ X 1 INPUT 0 ;ENCODER SIGNAL TO COLLECT. 
                            this._myControler.Commands[getTaskID(axes.AxesNo)].DataAcquisition.Input(psoAxis.AxisNo, 0);
                            //DATAACQ X 1 TRIGGER 2 ;TRIGGER OFF OF PSO SIGNAL
                            this._myControler.Commands[getTaskID(axes.AxesNo)].DataAcquisition.Trigger(psoAxis.AxisNo, 2);
                            //DATAACQ X 1 ARRAY 5000 1000 ;COLLECT MORE POINTS THAN YOU WILL USE.
                            this._myControler.Commands[getTaskID(axes.AxesNo)].DataAcquisition.ArraySetup(psoAxis.AxisNo, OffsetArrayDAQ, OffsetArrayDAQ - 1);
                            // <=
                            currentCounter[0].Value = 0;
                        }// lock (axes) <==

                    }// if (currentCounter != null && this._myControler != null) <==
                }// if (axis.PSOAxisKey != null && axis.PSOEncoders != null && axis.PSOEncoders.Length == 2) <==
            }// foreach (RealAxis axis in currentAxis) <==

            if (psoAxis == null)
            {
                U.LogInfo("Error: ResetCounter can not find psoAxis for axes {0}", axes.AxesNo.ToString());
            }
           
        }

        /// <summary>
        /// Update Axis Bool Inputs
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="vals"></param>
        private void UpdateAxisInput(RealAxis axis, int vals)
        {
            Inputs inputs = axis.FilterByTypeSingle<Inputs>();
            if (inputs != null)
            {
                BoolInput[] inputList = inputs.FilterByType<BoolInput>();

                if (inputList != null)
                {
                    foreach (BoolInput input in inputList)
                    {
                        int bit = 1;
                        for (int i = 0; i < input.Channel; i++)
                        {
                            bit <<= 1;
                        }
                        bool bVal = (vals & bit) != 0;
                        if (bVal != input.Value)
                        {
                            input.Value = bVal;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Temporary Helper for Simulate Trigger
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="psoAxis"></param>
        private void SimulateTriggerPoint(GTriggerPoint tp, RealAxis psoAxis)
        {
            foreach (string s in psoAxis.PSOAxisKey)
            {
                string[] settingArray = Regex.Split(s, "/ ");

                if (tp.TriggerID.Contains(settingArray[0]))
                {
                    if (settingArray.Length > 1)
                    {
                        for (int i = 1; i < settingArray.Length; i++)
                        {
                                // Do Work
                            U.InvokeNoArgMethod(settingArray[i]);                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load PSO Distances from LineList with applied WindowSafetyMarging to Task Doubles
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="triggerList"></param>
        /// <param name="iStart"></param>
        /// <param name="MyTask"></param>
        private void loadPSODistancesWithWindows(RealAxis axis, GLine[] lineList, int iStart, TaskId MyTask)
        {
            try
            {
                List<double> dList = new List<double>();
                
                Millimeters distanceWindowSaftyMargin;
                RealAxes axes = axis.GetParent<RealAxes>();
                foreach (GLine line in lineList)
                {
                    GTriggerPoint[] triggerList = line.TriggerPts;
                    GTriggerPoint previousTrigger = null;

                    if (Math.Abs(Math.Cos(line.RobotLoc.Yaw.Val)) > 0.1)
                    {
                        distanceWindowSaftyMargin = Math.Abs(WindowSafetyMargin / Math.Cos(line.RobotLoc.Yaw.Val));
                    }
                    else
                    {
                        distanceWindowSaftyMargin = WindowSafetyMargin;
                    }

                    foreach (GTriggerPoint trigger in triggerList)
                    {
                        if (previousTrigger == null)
                        {
                            dList.Add(axis.ToMotorCounts(trigger.DistanceWithLead - distanceWindowSaftyMargin));
                        }
                        else
                        {
                            dList.Add(axis.ToMotorCounts(trigger.DistanceWithLead - previousTrigger.Distance - distanceWindowSaftyMargin));
                        }
                       
                        previousTrigger = trigger;
                    }// foreach (GTriggerPoint trigger in triggerList) <==
                }// foreach (GLine line in lineList) <==

                if (dList.Count >= OffsetArrayDAQ)
                {
                    U.LogAlarmPopup("Error loadPSODistances (i >= OffsetArrayDAQ)");
                }
                else
                {
                    dList.Add(0.0);
                    axis.PSOCount = dList.Count;
                    lock (axes)
                    {
                        this._myControler.Variables.Tasks[MyTask].Doubles.SetMultiple(iStart, dList.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error loadPSODistances from LineList with applied WindowSaftyMargin to Task Doubles.");
            }
        }

        /// <summary>
        /// Load PSO Distances based on X-Axis from GLine's to Task Doubles
        /// </summary>
        /// <param name="movePattern"
        /// <param name="axis"></param>
        /// <param name="lineList"></param>
        /// <param name="iStart"></param>
        /// <param name="MyTask"></param>
        private void loadPSODistancesXAxis(GPattern movePattern, RealAxis axis, GLine[] lineList, int iStart, TaskId MyTask)
        {
            if (lineList == null || lineList.Length == 0)
            {
                return;
            }
            try
            {
                List<double> dList = new List<double>();
                G3DPoint previousTriggerPoint = null;
                //Do Work
                //int i = iStart;
                RealAxes axes = axis.GetParent<RealAxes>();
                G3DPoint pt0RobotLoc = lineList[0].RobotLoc;
                foreach (GLine line in lineList)
                {
                    GTriggerPoint[] triggerList = line.TriggerPts;

                    if (triggerList != null)
                    {
                            foreach (GTriggerPoint trigger in triggerList)
                            {
                                G3DPoint triggerPoint = line.GetRobotLoc(trigger.DistanceWithLead);

                                if (previousTriggerPoint == null)
                                {
                                    if (triggerPoint.X - pt0RobotLoc.X != 0)
                                    {
                                        dList.Add(axis.ToMotorCounts(triggerPoint.X - pt0RobotLoc.X));
                                    }
                                    else
                                    {
                                        U.LogAlarmPopup("Error loadPSODistancesXAxis with GLine (X.Val = 0) override to Val = 1");
                                        dList.Add(1.0);
                                    }
                                }
                                else
                                {
                                    if (triggerPoint.X - previousTriggerPoint.X != 0)
                                    {
                                        dList.Add(axis.ToMotorCounts(triggerPoint.X - previousTriggerPoint.X));
                                    }
                                    else
                                    {
                                        U.LogAlarmPopup("Error loadPSODistancesXAxis with GLine (X.Val = 0) override to Val = 1");
                                        dList.Add(1.0);
                                    }
                                }
                                //i++;
                                previousTriggerPoint = triggerPoint;
                            }// foreach (GTriggerPoint trigger in triggerList) <==
                    }// if (triggerList != null) <==
                }// foreach (GLine line in lineList) <==

                movePattern.AddTiming("loadPSODistancesXAxisPackaged");
                if (dList.Count >= OffsetArrayDAQ)
                {
                    U.LogAlarmPopup("Error loadPSODistancesXAxis with GLine (i >= OffsetArrayDAQ)");
                }
                else
                {
                    axis.PSOCount = dList.Count;
                    //if (dList.Count > 3)
                    //{
                    //    Debug.WriteLine(string.Format("Task={0}, Arr[0]={1}, Arr[1]={2}", MyTask.ToString(), dList[0], dList[1]));
                    //}
                    if (this._myControler != null)
                    {
                        lock (axes)
                        {
                            this._myControler.Variables.Tasks[MyTask].Doubles.SetMultiple(iStart, dList.ToArray());
                        }
                    }
                    if (LogCommandScript)
                    {
                        for (int i = 0; i < dList.Count; i++)
                        {
                            LogAeroBasicScript(string.Format("$task[{0}]={1}", i, dList[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error loadPSODistancesXAxis with GLine's to Task Doubles.");
            }
        }

        /// <summary>
        /// Load PSO Distances based on XY trigger distances to Task Doubles
        /// </summary>
        /// <param name="movePattern"
        /// <param name="axis"></param>
        /// <param name="lineList"></param>
        /// <param name="iStart"></param>
        /// <param name="MyTask"></param>
        private void loadPSODistancesXYAxis(GPattern movePattern, RealAxis axis, GLine[] lineList, int iStart, TaskId MyTask)
        {
            if (lineList == null || lineList.Length == 0)
            {
                return;
            }
            try
            {
                List<double> dList = new List<double>();
                Millimeters previousTriggerDist = 0;
                //Do Work
                //int i = iStart;
                RealAxes axes = axis.GetParent<RealAxes>();
                foreach (GLine line in lineList)
                {
                    GTriggerPoint[] triggerList = line.TriggerPts;

                    if (triggerList != null)
                    {
                        foreach (GTriggerPoint trigger in triggerList)
                        {
                            Millimeters delTriggerDist = trigger.DistanceWithLead - previousTriggerDist;
                            previousTriggerDist = trigger.DistanceWithLead;
                            if (delTriggerDist != 0)
                            {
                                dList.Add(axis.ToMotorCounts(delTriggerDist));
                            }
                            else
                            {
                                U.LogAlarmPopup("Error loadPSODistancesXYAxis with GLine (Y.Val = 0) override to Val = 1");
                                dList.Add(1.0);
                            }
                        }// foreach (GTriggerPoint trigger in triggerList) <==
                    }// if (triggerList != null) <==
                }// foreach (GLine line in lineList) <==
                movePattern.AddTiming("loadPSODistancesXYAxisPackaged");
                if (dList.Count >= OffsetArrayDAQ)
                {
                    U.LogAlarmPopup("Error loadPSODistancesXYAxis with GLine (i >= OffsetArrayDAQ)");
                }
                else
                {
                    axis.PSOCount = dList.Count;
                    //if (dList.Count > 3)
                    //{
                    //    Debug.WriteLine(string.Format("Task={0}, Arr[0]={1}, Arr[1]={2}", MyTask.ToString(), dList[0], dList[1]));
                    //}
                    if (this._myControler != null)
                    {
                        lock (axes)
                        {
                            this._myControler.Variables.Tasks[MyTask].Doubles.SetMultiple(iStart, dList.ToArray());
                        }
                        if (LogCommandScript)
                        {
                            for (int i = 0; i < dList.Count; i++)
                            {
                                LogAeroBasicScript(string.Format("$task[{0}]={1}", i, dList[i]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error loadPSODistancesXYAxis with GLine's to Task Doubles.");
            }
        }

        /// <summary>
        /// Load PSO Distances based on Y-Axis from GLine's to Task Doubles
        /// </summary>
        /// <param name="movePattern"
        /// <param name="axis"></param>
        /// <param name="lineList"></param>
        /// <param name="iStart"></param>
        /// <param name="MyTask"></param>
        private void loadPSODistancesYAxis(GPattern movePattern, RealAxis axis, GLine[] lineList, int iStart, TaskId MyTask)
        {
            if (lineList == null || lineList.Length == 0)
            {
                return;
            }
            try
            {
                List<double> dList = new List<double>();
                G3DPoint previousTriggerPoint = null;
                //Do Work
                //int i = iStart;
                RealAxes axes = axis.GetParent<RealAxes>();
                G3DPoint pt0RobotLoc = lineList[0].RobotLoc;
                foreach (GLine line in lineList)
                {
                    GTriggerPoint[] triggerList = line.TriggerPts;

                    if (triggerList != null)
                    {
                        foreach (GTriggerPoint trigger in triggerList)
                        {
                            G3DPoint triggerPoint = line.GetRobotLoc(trigger.DistanceWithLead);

                            if (previousTriggerPoint == null)
                            {
                                if (triggerPoint.Y - pt0RobotLoc.Y != 0)
                                {
                                    dList.Add(axis.ToMotorCounts(triggerPoint.Y - pt0RobotLoc.Y));
                                }
                                else
                                {
                                    U.LogAlarmPopup("Error loadPSODistancesYAxis with GLine (Y.Val = 0) override to Val = 1");
                                    dList.Add(1.0);
                                }
                            }
                            else
                            {
                                if (triggerPoint.Y - previousTriggerPoint.Y != 0)
                                {
                                    dList.Add(axis.ToMotorCounts(triggerPoint.Y - previousTriggerPoint.Y));
                                }
                                else
                                {
                                    U.LogAlarmPopup("Error loadPSODistancesYAxis with GLine (Y.Val = 0) override to Val = 1");
                                    dList.Add(1.0);
                                }
                            }
                            //i++;
                            previousTriggerPoint = triggerPoint;
                        }// foreach (GTriggerPoint trigger in triggerList) <==
                    }// if (triggerList != null) <==
                }// foreach (GLine line in lineList) <==

                movePattern.AddTiming("loadPSODistancesYAxisPackaged");
                if (dList.Count >= OffsetArrayDAQ)
                {
                    U.LogAlarmPopup("Error loadPSODistancesYAxis with GLine (i >= OffsetArrayDAQ)");
                }
                else
                {
                    axis.PSOCount = dList.Count;
                    //if (dList.Count > 3)
                    //{
                    //    Debug.WriteLine(string.Format("Task={0}, Arr[0]={1}, Arr[1]={2}", MyTask.ToString(), dList[0], dList[1]));
                    //}
                    lock (axes)
                    {
                        this._myControler.Variables.Tasks[MyTask].Doubles.SetMultiple(iStart, dList.ToArray());
                    }
                    if (LogCommandScript)
                    {
                        for (int i = 0; i < dList.Count; i++)
                        {
                            LogAeroBasicScript(string.Format("$task[{0}]={1}", i, dList[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error loadPSODistancesXAxis with GLine's to Task Doubles.");
            }
        }

        /// <summary>
        /// Load PSO Windows based on X-Axis to Task Doubles
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="line"></param>
        /// <param name="iStart"></param>
        /// <param name="MyTask"></param>
        private void loadPSOWindowsXAxis(RealAxis axis, GPattern windowPattern, int iStart, TaskId MyTask)
        {
            try
            {
                List<double> dList = new List<double>();

                G3DPoint refWindowPoint = null;
                RealAxes axes = axis.GetParent<RealAxes>();
                if (windowPattern != null && windowPattern.Lines != null)
                {

                    foreach (GLine window in windowPattern.Lines)
                    {
                        if (window.RobotLoc.X == window.EndPoint.RobotLoc.X)
                        {
                            U.LogAlarmPopup("Error loadPSOWindowsXAxis with GPattern Window Width = 0");
                        }
                        else
                        {
                            if (refWindowPoint == null)
                            {
                                refWindowPoint = window.RobotLoc;
                            }
                            if (window.RobotLoc.X < window.EndPoint.RobotLoc.X)
                            {
                                dList.Add(axis.ToMotorCounts(window.RobotLoc.X - refWindowPoint.X + WindowSafetyMargin));
                                
                                dList.Add(axis.ToMotorCounts(window.EndPoint.RobotLoc.X - refWindowPoint.X));
                            }
                            else
                            {
                                dList.Add(axis.ToMotorCounts(window.EndPoint.RobotLoc.X - refWindowPoint.X));
                                
                                dList.Add(axis.ToMotorCounts(window.RobotLoc.X - refWindowPoint.X - WindowSafetyMargin));
                            }
                        }
                    }// foreach (GLine window in windowPattern.ChildArray) <==

                    dList.Add(0.0);

                    lock (axes)
                    {
                        this._myControler.Variables.Tasks[MyTask].Doubles.SetMultiple(iStart, dList.ToArray());
                    }
                    if (LogCommandScript)
                    {
                        for (int i = 0; i < dList.Count; i++)
                        {
                            LogAeroBasicScript(string.Format("$task[{0}]={1}", i, dList[i]));
                        }
                    }
                }// if (windowPattern != null && windowPattern.ChildArray != null) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error loadPSOWindowsXAxis with GPattern to Task Doubles.");
            }
        }

        /// <summary>
        /// Get assigned Task
        /// </summary>
        /// <param name="MyAxes"></param>
        /// <returns></returns>
        private TaskId getTaskID(int myTask)
        {
            // Set the TaskNo to run
            if (myTask < 0)
            {
                return BackgroundTask;
            }

            switch (myTask)
            {
                case 0:
                    return TaskId.T02;
                    
                case 1:
                    return TaskId.T03;

                case 2:
                    return TaskId.T04;

                case 3:
                    return TaskId.T05;

                case 4:
                    return TaskId.T06;

                case 5:
                    return TaskId.T07;

                case 6:
                    return TaskId.T08;

                default:
                    return TaskId.T01;
            }
            
        }


        /// <summary>
        /// Do Save Transfer to workPiece starting point
        /// </summary>
        /// <param name="workPiece"></param>
        /// <param name="axes"></param>
        /// <param name="patList"></param>
        private void safetyTransferMove(GWorkPiece workPiece, RealAxes axes, GPattern pattern)
        {
            int iX = 0;
            int iY = 0;
            int iZ = 0;

            foreach (CompBase compChild in axes.ChildArray)
            {
                if (compChild is RealAxis)
                {
                    if ((compChild as RealAxis).IsXDir && (compChild as RealAxis).Enabled == true)
                    {
                        iX = iX + 1;
                    }
                    if ((compChild as RealAxis).IsYDir && (compChild as RealAxis).Enabled == true)
                    {
                        iY = iY + 1;
                    }
                    if ((compChild as RealAxis).IsZDir && (compChild as RealAxis).Enabled == true)
                    {
                        iZ = iZ + 1;
                    }
                }
            }

            try
            {
                if (iX > 0 && iY > 0 && iZ > 0)
                {
                    U.LogInfo("safetyTransferMove XYZ Start");
                    //// Move to Save Z-Position before move to pattern start point
                    moveAbsWaitMotion(axes.AxisZ, workPiece.ZSafeTransfer, workPiece.ZTransferSpeed, getTaskID(axes.AxesNo));
                    workPiece.AddTimingElement("TransferMovePreZUP");

                    //// Determine and execute move to pattern start point xy
                    GLine patternStart = pattern.Lines[0];
                    G3DPoint ptRobotLoc = patternStart.RobotLoc;
                    moveLinearXYZWait(axes, new double[] { ptRobotLoc.X, ptRobotLoc.Y, workPiece.ZSafeTransfer }, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));
                    workPiece.AddTimingElement("TransferMoveWXY");
                    //moveAbsWaitMotion(axes.AxisX, patternStart.RobotLoc.X, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));
                    //moveAbsWaitMotion(axes.AxisY, patternStart.RobotLoc.Y, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));

                    // Determine and Move to pattern start point z
                    moveAbsWaitMotion(axes.AxisZ, ptRobotLoc.Z, workPiece.ZTransferSpeed, getTaskID(axes.AxesNo));
                    workPiece.AddTimingElement("TransferMoveZDN");

                    //Thread.Sleep(100);

                    U.LogInfo("safetyTransferMove XYZ Complete");
                    return;
                }

                if (iX > 0 && iY > 0)
                {
                    U.LogInfo("safetyTransferMove XY Start");
                    // Determine and execute move to pattern start point xy
                    GLine patternStart = pattern.Lines[0];
                    moveLinearXYZWait(axes, new double[] { patternStart.RobotLoc.X, patternStart.RobotLoc.Y }, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));
                    //moveAbsWaitMotion(axes.AxisX, patternStart.RobotLoc.X, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));
                    //moveAbsWaitMotion(axes.AxisY, patternStart.RobotLoc.Y, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));

                    U.LogInfo("safetyTransferMove XY Complete");
                    return;
                }

                if (iX > 0)
                {
                    U.LogInfo("safetyTransferMove X Start");
                    //// Determine and execute move to pattern start point x
                    GLine patternStart = pattern.Lines[0];
                    moveAbsWaitMotion(axes.AxisX, patternStart.RobotLoc.X, workPiece.XYTransferSpeed, getTaskID(axes.AxesNo));

                    U.LogInfo("safetyTransferMove X Complete");
                    return;
                }

                U.LogInfo("safetyTransferMove No axis declared");

            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error safetyTransferMove.");
            }
            finally
            {
                workPiece.AddTimingElement("EndSafetyTransferMove");
            }

        }


        private delegate void _delMoveLinearASynch(int[] AxisArray, double[] LineDistance, double LineSpeed, TaskId MyTask);

        private void moveLinearASynch(int[] AxisArray, double[] LineDistance, double LineSpeed, TaskId MyTask)
        {
            this._myControler.Commands[MyTask].Motion.Linear(AxisArray, LineDistance, LineSpeed);
        }
        /// <summary>
        /// Linear Move Multi Axis on TaskId
        /// </summary>
        /// <param name="name"
        /// <param name="AxisArray"></param>
        /// <param name="LineDistance"></param>
        /// <param name="LineSpeed"></param>
        /// <param name="MyTask"
        /// <returns></returns>
        private void moveLinear(string name, RealAxis[] AxisArray, double[] LinePos, double LineSpeed, TaskId MyTask, bool bTriggering)
        {
            RealAxes axes = null;
            //int ZAxisNo = -1;
            //double Vz = 0;
            int[] XYAxisNo = null;
            double[] XYPos = null;
            //double ZPos = 0.0;
            try
            {
                axes = AxisArray[0].GetParent<RealAxes>();
                if (LinePos.Length > 2)
                {
                    XYAxisNo = new int[3] { AxisArray[0].AxisNo, AxisArray[1].AxisNo, AxisArray[2].AxisNo };
                    XYPos = LinePos;
                    axes.LastBlendedPt.Z = LinePos[2];
                }
                else
                {
                    XYAxisNo = new int[2] { AxisArray[0].AxisNo, AxisArray[1].AxisNo };
                    XYPos = new double[2] { LinePos[0], LinePos[1] };
                }

                //AxisNo = new int[AxisArray.Length];

                //double distX = LinePos[0] - axes.LastBlendedPt.X.Val;

                double xySecTravel = axes.SetBlendedMoveTime(name, XYPos, ref LineSpeed, bTriggering);
                if (xySecTravel <= 0.0 && LinePos.Length < 3)
                {
                    return;
                }
                lock (axes)
                {
                    if (axes.StartTime == 0 && !axes.Paused && this._myControler != null)
                    {  // Don't pause if already running with LinearMove
                        //Debug.WriteLine(string.Format("Pause and LinearMove-{0}", name));
                        this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Debug.Pause();
                        axes.Paused = true;
                    }
                    else
                    {
                        //Debug.WriteLine(string.Format("LinearMove-{0}", name));
                    }
                    //if (LinePos.Length > 2)
                    //{
                    //    Debug.WriteLine(string.Format("LINEARMOVE={0}, {1}, {2} Speed={3}", XYPos[0], XYPos[1], XYPos[2], LineSpeed));
                    //}
                    //else
                    //{
                    //    Debug.WriteLine(string.Format("LINEARMOVE={0}, {1} Speed={2}", XYPos[0], XYPos[1], LineSpeed));
                    //}
                    if (this._myControler != null)
                    {
                        this._myControler.Commands[MyTask].Motion.Linear(XYAxisNo, XYPos, LineSpeed);
                    }
                    LogAeroBasicScript(string.Format("; MoveLinear '{0}'", name));
                    if (LinePos.Length > 2)
                    {
                        LogAeroBasicScript(string.Format("LINEAR {0} {1} {2} {3} {4} {5} F{6}",
                            AxisArray[0].Name, XYPos[0],
                            AxisArray[1].Name, XYPos[1],
                            AxisArray[2].Name, XYPos[2],
                            LineSpeed));
                    }
                    else
                    {
                        LogAeroBasicScript(string.Format("LINEAR {0} {1} {2} {3} F{4}",
                            AxisArray[0].Name, XYPos[0],
                            AxisArray[1].Name, XYPos[1],
                            LineSpeed));
                    }
                }

            }
            catch (Exception ex)
            {
                HandleAxesException(ex, axes, string.Format("moveLinear multi axis exception. Name= '{0}'", name));
                //try
                //{
                //    ClearAllFaults();
                //    lock (axes)
                //    {
                //        this._myControler.Commands[MyTask].Execute("DWELL 0.1");
                //        //if (Vz > 0)
                //        //{
                //        //    this._myControler.Commands[MyTask].Motion.MoveAbs(ZAxisNo, ZPos, Vz);
                //        //}
                //        //this._myControler.Commands[MyTask].Motion.Linear(XYAxisNo, XYPos, LineSpeed);
                //    }
                //}
                //catch (Exception e)
                //{
                //    U.LogAlarmPopup(e, "Error moveLinear multi axis on Task {0}.", MyTask.ToString());

                //}
                
            }
        }

        private void LogAeroBasicScript(string text)
        {
            if (LogCommandScript)
            {
                U.LogCustom("AeroBasicScript", text);
            }
        }

        /// <summary>
        /// Linear Move Multi Axis and wait motion complete on TaskId
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Position"></param>
        /// <param name="LineSpeed"></param>
        /// <returns></returns>
        private void moveLinearXYZWait(RealAxes axes, double[] LineDistance, double LineSpeed, TaskId MyTask)
        {
            try
            {
                lock (axes)
                {
                    //Stopwatch sw = Stopwatch.StartNew();
                    this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                    //axes.MSTimeSpentWaitingForMotionComplete += sw.ElapsedMilliseconds;
                    this._myControler.Commands[MyTask].Motion.Linear(new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo }, LineDistance, LineSpeed);
                    U.SleepWithEvents(20);
                    //sw = Stopwatch.StartNew();
                    this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                    //axes.MSTimeSpentWaitingForMotionComplete += sw.ElapsedMilliseconds;
                }
            }// try 1st <==
            catch (Exception ex)
            {
                try
                {
                    U.LogError(ex, "moveLinearXYZWait 1st exception.  Will retry");
                    ClearAllFaults();
                    lock (axes)
                    {
                        this._myControler.Commands[MyTask].Execute("DWELL 0.1");
                        this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                        this._myControler.Commands[MyTask].Motion.Linear(new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo }, LineDistance, LineSpeed);
                        this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                    }
                }// try 2nd <==
                catch (Exception e)
                {
                    U.LogAlarmPopup(e, "Error moveLinearXYZWait multi axis on Task {0}.", MyTask.ToString());
                }// catch (Exception e) <==
            }// catch (Exception ex) <==

        }

        /// <summary>
        /// Absolute Move and wait motion complete on TaskId
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Position"></param>
        /// <param name="MoveSpeed"></param>
        private void moveAbsWaitMotion(RealAxis axis, double Position, double MoveSpeed, TaskId MyTask)
        {
            RealAxes axes = null;
            try
            {
                axes = axis.GetParent<RealAxes>();
                lock (axes)
                {
                    //Stopwatch sw = Stopwatch.StartNew();
                    this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                    //axes.MSTimeSpentWaitingForMotionComplete += sw.ElapsedMilliseconds;
                    //System.Diagnostics.Debug.WriteLine(string.Format("moveAbsWaitMotion axes {0} axis {1} distance={2}", axes.Name, axis.Name, axis.CurrentPosition - Position));
                    this._myControler.Commands[MyTask].Motion.MoveAbs(axis.AxisNo, Position, MoveSpeed);
                    U.SleepWithEvents(20);
                    //U.SleepWithEvents(100);
                    
                    //sw = Stopwatch.StartNew();
                    this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                    //axes.MSTimeSpentWaitingForMotionComplete = sw.ElapsedMilliseconds;
                }
                
                U.LogInfo("moveAbsWaitMotion {0} Complete", axis.Name);
            }// try 1st <==
            catch (Exception ex)
            {
                try
                {
                    U.LogError(ex, "moveAbsWaitMotion 1st exception.  Will retry");
                    ClearAllFaults();
                    lock (axes)
                    {
                        this._myControler.Commands[MyTask].Execute("DWELL 0.1");

                        this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                        this._myControler.Commands[MyTask].Motion.MoveAbs(axis.AxisNo, Position, MoveSpeed);
                        this._myControler.Commands[MyTask].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                    }
                }// try 2nd <==
                catch (Exception e)
                {
                    U.LogAlarmPopup(e, "Error moveAbsWaitMotion {0} on Task {1}.", new string[] { axis.AxisNo.ToString(), MyTask.ToString() });
                }// catch (Exception e) <==
            }// catch (Exception ex) <==

        }

        /// <summary>
        /// Setup PSO Type and return PSO Primary Axis
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="triggerID"></param>
        /// <param name="pulseSetup"></param>
        /// <returns></returns>
        private RealAxis setupPSOType(RealAxes axes, string triggerID, out bool pulseSetup)
        {
            pulseSetup = false;
            try
            {
                TaskId myTask = getTaskID(axes.AxesNo);
                RealAxis psoAxis = axes.GetPSOAxis(triggerID);
                if (psoAxis != null)
                {
                    if (psoAxis.PSOAxisKey != null)
                    {
                        foreach (string s in psoAxis.PSOAxisKey)
                        {
                            string[] settingArray = Regex.Split(s, "/ ");

                            if (triggerID.Contains(settingArray[0]))
                            {
                                if (settingArray.Length > 1 && psoAxis.PSOEncoders != null)
                                {
                                    for (int i = 1; i < settingArray.Length; i++)
                                    {
                                        string setting = settingArray[i];
                                        // => Execute PSO Setup Commands
                                        if (setting.StartsWith("PSOPULSE"))
                                        {
                                            pulseSetup = true;
                                            axes.CurrentPSOPulseOnTime = 1.0;
                                        }
                                        try
                                        {
                                            //Debug.WriteLine(string.Format("PSO Setup for {0} = '{1}'", myTask.ToString(), setting));
                                            lock (axes)
                                            {
                                                CommandExecute(setting, myTask);
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                U.LogError(ex, "setupPSOType.({0}) First exception.  Will retry", setting);
                                                ClearAllFaults();
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[myTask].Execute("DWELL 0.1");
                                                    this._myControler.Commands[myTask].Execute(setting);
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {
                                                U.LogAlarmPopup(e, "setupPSOType.({0}) second exception", setting);
                                                return null;
                                            }// catch (Exception e) <==
                                        }// catch (Exception ex) <==
                                        // <= Execute PSO Setup Commands

                                    }// for (int i = 1; i < settingArray.Length; i++) <==
                                }// if (settingArray.Length > 1 && psoAxis.PSOEncoders != null) <==
                                return psoAxis;

                            }//  if (triggerID.Contains(settingArray[0])) <==
                        }// foreach (string s in psoAxis.PSOAxisKey) <==
                    }//  if (psoAxis.PSOAxisKey != null) <==
                }//  if (psoAxis != null) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error setupPSOType axes:{0} triggerID:{1}.", axes.AxesNo.ToString(), triggerID);
            }

            return null;
            
        }

        private bool IsSimTrigger(string triggerID)
        {
            if (!string.IsNullOrEmpty(SimTriggers))
            {
                return SimTriggers.Contains(triggerID);
            }
            return false;
        }

        /// <summary>
        /// Return PSO Primary Axis based on triggerID
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="triggerID"></param>
        /// <returns></returns>
        private RealAxis returnPSOPrimaryAxis(RealAxes axes, string triggerID)
        {
            try
            {
                RealAxis psoAxis = axes.GetPSOAxis(triggerID);
                if (psoAxis != null)
                {
                    if (psoAxis.PSOAxisKey != null)
                    {
                        foreach (string s in psoAxis.PSOAxisKey)
                        {
                            string[] settingArray = Regex.Split(s, "/ ");

                            if (triggerID.Contains(settingArray[0]))
                            {
                                return psoAxis;
                            }//  if (triggerID.Contains(settingArray[0])) <==
                        }// foreach (string s in psoAxis.PSOAxisKey) <==
                    }//  if (psoAxis.PSOAxisKey != null) <==
                }//  if (psoAxis != null) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error returnPSOPrimaryAxis axes:{0} triggerID:{1}.", axes.AxesNo.ToString(), triggerID);
            }

            return null;

        }

        /// <summary>
        /// Move Pattern Point by Point with Software Triggering
        /// </summary>
        /// <param name="PSOPrimaryAxis"></param>
        /// <param name="axes"></param>
        /// <param name="movePattern"></param>
        public void MovePatternSimulateTrigger(RealAxis PSOPrimaryAxis, RealAxes axes, GPattern movePattern)
        {
            try
            {
                RealAxis[] ax3 = new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ };
                foreach (GLine line in movePattern.Lines)
                {
                    G3DPoint lineStart = line.RobotLoc;
                    G3DPoint lineStop = line.EndPoint.RobotLoc;
                    // => Move Gantry to Line Start Point
                    moveLinear("SimTrigger-LineStart", ax3, new double[] { lineStart.X, lineStart.Y, lineStart.Z }, line.Speed, getTaskID(axes.AxesNo), false);
                    // <=

                    if (line.NumTriggerPts == 0)
                    {
                        // Do nothing
                    }
                    else
                    {
                        // => Move and Stop for each TriggerPoint position
                        foreach (GTriggerPoint tp in line.TriggerPts)
                        {
                            double distCoeff = tp.DistanceWithLead.Val / line.XYLength;
                            G3DPoint trigPos = new G3DPoint();
                            trigPos.X = lineStart.X + (lineStop.X - lineStart.X) * distCoeff;
                            trigPos.Y = lineStart.Y + (lineStop.Y - lineStart.Y) * distCoeff;
                            trigPos.Z = lineStart.Z + (lineStop.Z - lineStart.Z) * distCoeff;
                            // => Make move to next trigger position
                            moveLinearXYZWait(axes, new double[] { trigPos.X.Val, trigPos.Y.Val, trigPos.Z.Val }, line.Speed, getTaskID(axes.AxesNo));
                            // <=
                            if (PSOPrimaryAxis != null)
                            {
                                SimulateTriggerPoint(tp, PSOPrimaryAxis);
                            }
                            else
                            {
                                tp.DoStationaryAction();
                            }
                        }
                        // <=
                    }
                    // => Move Gantry to Line End Point
                    moveLinear("SimTrigger-LineEnd", ax3, new double[] { lineStop.X, lineStop.Y, lineStop.Z }, line.Speed, getTaskID(axes.AxesNo), false);
                    
                    // => Wait for motion to Line End Point complete
                    try
                    {
                        lock (axes)
                        {
                            // => Wait Motion Complete
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                            // <= Wait Motion Complete
                        }
                    }// try 1st <==
                    catch (Exception ex)
                    {
                        try
                        {
                            U.LogError(ex, "MovePatternSimulateTrigger.WaitForMotionDone 1st exception");
                            ClearAllFaults();
                            lock (axes)
                            {
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                // => Wait Motion Complete
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                                // <= Wait Motion Complete
                            }
                        }// try 2nd <==
                        catch (Exception e)
                        {
                            U.LogAlarmPopup(e, "MovePatternSimulateTrigger.WaitForMotionDone 2nd exception");
                            return;
                        }// catch (Exception e) <==
                    }// catch (Exception ex) <==
                    // <= Wait for motion to Line End Point complete

                } //foreach (GLine line in pattern.ChildArray) <==

            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error MovePatternSmulateTrigger axes:{0} pattern:{1}.", axes.AxesNo.ToString(), movePattern.ToString());
            }
        }

        /// <summary>
        /// Move Pattern Point by Point with PSO Hardware Triggering
        /// </summary>
        /// <param name="PSOPrimaryAxis"></param>
        /// <param name="axes"></param>
        /// <param name="movePattern"></param>
        public void MovePatternPointByPoint(RealAxis PSOPrimaryAxis, RealAxes axes, GPattern movePattern)
        {
            try
            {
                RealAxis[] ax3 = new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ };
                foreach (GLine line in movePattern.Lines)
                {
                    G3DPoint lineStart = line.RobotLoc;
                    G3DPoint lineStop = line.EndPoint.RobotLoc;
                    // => Move Gantry to Line Start Point
                    moveLinear("PointByPoint-LineStart", ax3, new double[] { lineStart.X, lineStart.Y, lineStart.Z }, line.Speed, getTaskID(axes.AxesNo), false);
                    // <=

                    if (line.NumTriggerPts == 0)
                    {
                        // Do nothing
                    }
                    else
                    {
                        // => Move to TriggerPoint position with PSO Hardware Trigger
                        foreach (GTriggerPoint tp in line.TriggerPts)
                        {
                            double distCoeff = tp.DistanceWithLead.Val / line.XYLength;
                            G3DPoint trigPos = new G3DPoint();
                            trigPos.X = lineStart.X + (lineStop.X - lineStart.X) * distCoeff;
                            trigPos.Y = lineStart.Y + (lineStop.Y - lineStart.Y) * distCoeff;
                            trigPos.Z = lineStart.Z + (lineStop.Z - lineStart.Z) * distCoeff;
                            // => Make move to next trigger position
                            moveLinearXYZWait(axes, new double[] { trigPos.X.Val, trigPos.Y.Val, trigPos.Z.Val }, line.Speed, getTaskID(axes.AxesNo));
                            // <= Make move to next trigger position

                            // => Manual PSO Hardware Firing
                            try
                            {
                                lock (axes)
                                {
                                    // => Manual PSO Hardware Firing
                                    this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                                    // <= Manual PSO Hardware Firing
                                }
                            }// try 1st <==
                            catch (Exception ex)
                            {
                                try
                                {
                                    U.LogError(ex, "MovePatternPointByPoint. SINGLE PSO TRIGGER TO CONTROLER 1st exception");
                                    ClearAllFaults();
                                    lock (axes)
                                    {
                                        //this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                        // => Manual PSO Hardware Firing
                                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                                        // <= Manual PSO Hardware Firing
                                    }
                                }// try 2nd <==
                                catch (Exception e)
                                {
                                    U.LogAlarmPopup(e, "MovePatternPointByPoint.SINGLE PSO TRIGGER TO CONTROLER 2nd exception");
                                    return;
                                }// catch (Excetpion e) <==
                            }// catch (Exception ex) <==
                            // <= Manual PSO Hardware Firing

                        }// foreach (GTriggerPoint tp in line.TriggerPts)
                        // <= Move to TriggerPoint position with PSO Hardware Trigger

                    }// if (line.NumTriggerPts == 0) {} else <==

                    // => Move Gantry to Line End Point
                    moveLinear("PointByPoint-LineEnd", ax3, new double[] { lineStop.X, lineStop.Y, lineStop.Z }, line.Speed, getTaskID(axes.AxesNo), false);
                    // <= Move Gantry to Line End Point
                } //foreach (GLine line in pattern.ChildArray) <==

            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error MovePatternPointByPoint axes:{0} pattern:{1}.", axes.AxesNo.ToString(), movePattern.ToString());
            }
        }

        /// <summary>
        /// Move Pattern Line by Line with PSO Hardware Triggering
        /// </summary>
        /// <param name="PSOPrimaryAxis"></param>
        /// <param name="axes"></param>
        /// <param name="movePattern"></param>
        public void MovePatternLineByLine(RealAxis PSOPrimaryAxis, RealAxes axes, GPattern movePattern, ref bool firstMove)
        {
            try
            {
                LogAeroBasicScript(string.Format("; MovePatternLineByLine on axis '{0}'", PSOPrimaryAxis.Nickname));
                GWorkPiece workPiece = movePattern.Workpiece;
                TaskId taskId = getTaskID(axes.AxesNo);
                RealAxis[] ax3 = new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ };
                foreach (GLine line in movePattern.Lines)
                {
                    G3DPoint lineStart = line.RobotLoc;
                    G3DPoint lineStop = line.EndPoint.RobotLoc;
                    bool bIntermediateLineStopPoint = false;

                    int camTable = -1;
                    if (line.CamTableCount > 0)
                    {
                        camTable = axes.AxesNo;
                        //Debug.WriteLine(string.Format("Start Z = {0}", lineStart.Z.ToString()));
                    }
                    if (!line.IgnoreStartPt)
                    {
                        // => Move Gantry to Line Start Point
                        //System.Diagnostics.Debug.WriteLine(string.Format("Speed to Start of pattern line = {0}", line.Speed.ToString()));
                        MillimetersPerSecond speed = movePattern.TransitionSpeed;
                        if (firstMove)
                        {
                            moveLinear("LineByLine SafeUp", ax3, new double[] { ax3[0].CurrentPosition, ax3[1].CurrentPosition, workPiece.ZSafeTransfer }, workPiece.ZTransferSpeed, taskId, false);
                            moveLinear("LineByLine Safe XY", ax3, new double[] { lineStart.X, lineStart.Y, workPiece.ZSafeTransfer }, workPiece.XYTransferSpeed, taskId, false);
                            speed = workPiece.ZTransferSpeed;
                            firstMove = false;
                        }
                        moveLinear("LineByLine ToStart", ax3, new double[] { lineStart.X, lineStart.Y, lineStart.Z }, speed, taskId, false);
                        //movePattern.AddTiming("LineByLine ToStart Move");
                        // <= Move Gantry to Line Start Point
                    }
                    //System.Diagnostics.Debug.WriteLine(string.Format("LineStart = {0}", lineStart.ToString()));
                    if (line.NumTriggerPts == 0)
                    {
                        // Move to end point
                        WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtDualLineStart", axes, lineStart.Z);
                    }
                    else
                    {
                        if (PSOPrimaryAxis.PSOEncoders.Length == 1)
                        {
                            Millimeters lineDistanceX = Math.Abs(lineStart.X - lineStop.X);
                            if (lineDistanceX != 0)
                            {
                                WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtLineByLineBeforePSOSetup", axes, lineStart.Z);
                                if (line.TriggerPts.Length > 1)
                                {
                                    // => Send Single PSO Trigger to Controller
                                    Millimeters newDist = lineDistanceX / line.TriggerPts.Length;
                                    try
                                    {
                                        if (this._myControler != null)
                                        {
                                            lock (axes)
                                            {
                                                // => Add evenly distributed PSO Trigger along the line to Controller
                                                this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                //Debug.WriteLine(string.Format("{0}-DistanceFixed({1})", PSOPrimaryAxis.Name, newDist));
                                                // <= Add evenly distributed PSO Trigger along the line to Controller
                                            }
                                        }
                                    }// try 1st <==
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            U.LogError(ex, "MovePatternLineByLine. SINGLE PSO TRIGGER TO CONTROLER 1st exception");
                                            ClearAllFaults();
                                            lock (axes)
                                            {
                                                this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                                // => Add evenly distributed PSO Trigger along the line to Controller
                                                this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                // <= Add evenly distributed PSO Trigger along the line to Controller
                                            }
                                        }// try 2nd <==
                                        catch (Exception e)
                                        {
                                            U.LogAlarmPopup(e, "MovePatternLineByLine.SINGLE PSO TRIGGER TO CONTROLER 2nd exception");
                                            return ;
                                        }// catch (Excetpion e) <==
                                    }// catch (Exception ex) <==
                                    movePattern.AddTiming("*SinglePSOTrigger1");
                                    // <= Send Single PSO Trigger to Controller
                                    
                                    U.LogInfo("MovePatternLineByLine: Auto-Override to evenly distributed trigger/line for PSO.DistanceFixed {0}", line.ID);
                                }// if (line.TriggerPts.Length > 1) <==
                                else
                                {
                                    G3DPoint firstTrigger = line.TriggerPts[0].RobotLoc;
                                    if (Math.Abs(lineStart.X - firstTrigger.X) > 0.5 * lineDistanceX && Math.Abs(lineStart.X - firstTrigger.X) < lineDistanceX)
                                    {
                                        Millimeters newDist = Math.Abs(lineStart.X - firstTrigger.X);

                                        // <= Send Single PSO Trigger to Controller
                                        try
                                        {
                                            if (this._myControler != null)
                                            {
                                                lock (axes)
                                                {
                                                    // => Add Single PSO Trigger to Controller
                                                    this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                    //Debug.WriteLine(string.Format("{0}-DistanceFixed({1})", PSOPrimaryAxis.Name, newDist));
                                                    // <= Add Single PSO Trigger to Controller
                                                }
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                U.LogError(ex, "MovePatternLineByLine. SINGLE PSO TRIGGER TO CONTROLER 1st exception");
                                                ClearAllFaults();
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                                    // => Add Single PSO Trigger to Controller
                                                    this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                    // <= Add Single PSO Trigger to Controller
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {
                                                U.LogAlarmPopup(e, "MovePatternLineByLine.SINGLE PSO TRIGGER TO CONTROLER 2nd exception");
                                                return;
                                            }// catch (Excetpion e) <==
                                        }// catch (Exception ex) <==
                                        movePattern.AddTiming("*SinglePSOTrigger2");
                                        // <= Send Single PSO Trigger to Controller
                                    }// if (Math.Abs(lineStart.X - firstTrigger.X) > 0.5 * lineDistanceX && Math.Abs(lineStart.X - firstTrigger.X) < lineDistanceX) <==
                                    else
                                    {// Add intermediate Stop Point
                                        Millimeters newDist = Math.Abs(lineStart.X - firstTrigger.X);
                                        
                                        // <= Send Single PSO Trigger to Controller
                                        try
                                        {
                                            if (this._myControler != null)
                                            {
                                                lock (axes)
                                                {
                                                    // => Add Single PSO Trigger to Controller
                                                    this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                    //Debug.WriteLine(string.Format("{0}-DistanceFixed({1})", PSOPrimaryAxis.Name, newDist));
                                                    this._myControler.Commands[taskId].Execute(PreArmDwell);
                                                    // => Arm PSO Output
                                                    this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                                    //Debug.WriteLine(string.Format("{0}-Control(ARM)", PSOPrimaryAxis.Name));
                                                    // <= Add Single PSO Trigger to Controller
                                                }
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                HandleAxesException(ex, axes, "MovePatternLineByLine. Auto-Override Intermediate Line Stop Point 1st exception");
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                                    // => Add Single PSO Trigger to Controller
                                                    this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                                    // <= Add Single PSO Trigger to Controller
                                                    // => Arm PSO Output
                                                    this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {
                                                U.LogAlarmPopup(e, "MovePatternLineByLine.Auto-Override Intermediate Line Stop Point 2nd exception");
                                                return;
                                            }// catch (Excetpion e) <==
                                        }// catch (Exception ex) <==
                                        movePattern.AddTiming("*SetupIntermediate");
                                        // <= Send Single PSO Trigger to Controller
                                        
                                        U.LogInfo("MovePatternLineByLine: Auto-Override Intermediate Line Stop Point and PSO.DistanceFixed {0}", line.ID);

                                        // => Move Gantry to Intermediate Stop Point
                                        G3DPoint intermediateStopPoint;
                                        intermediateStopPoint = line.GetRobotLoc(newDist + newDist * 0.5);

                                        moveLinear("*LineByLine ToIntermediate", new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { intermediateStopPoint.X, intermediateStopPoint.Y }, line.Speed, taskId, false);
                                        // <= Move Gantry to Intermediate Stop Point
                                        string details = axes.BlendedMoveDetails;
                                        WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtLineByLineToIntermediate", axes, intermediateStopPoint.Z);
                                        // => Turn Off PSO
                                        try
                                        {
                                            if (this._myControler != null)
                                            {
                                                lock (axes)
                                                {
                                                    // => Turn PSO Output Off
                                                    this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                                    //Debug.WriteLine(string.Format("{0}-Control(OFF)", PSOPrimaryAxis.Name));
                                                    // <= Turn PSO Output Off
                                                }
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            HandleAxesException(ex, axes, "MovePatternLineByLine.PSOMODE.OFF First exception.  Will retry");
                                            try
                                            {
                                                lock (axes)
                                                {
                                                    // => Turn PSO Output Off
                                                    this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                                    // <= Turn PSO Output Off
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {
                                                U.LogAlarmPopup(e, "MovePatternLineByLine.PSOMODE.OFF second exception");
                                                return;
                                            }// catch (Exception e) <==
                                        }// catch (Exception ex) <==
                                        // <= Turn Off PSO
                                        movePattern.AddTiming("*PSO Off");

                                        bIntermediateLineStopPoint = true;

                                    }// if (Math.Abs(lineStart.X - firstTrigger.X) > 0.5 * lineDistanceX && Math.Abs(lineStart.X - firstTrigger.X) < lineDistanceX)  {} else <==
                                }// if (line.ChildArray.Length > 1) {} else <==
                            }// if (lineDistanceX != 0) <==
                        }// if (PSOPrimaryAxis.PSOEncoders.Length == 1) <==

                        if (PSOPrimaryAxis.PSOEncoders.Length == 2)
                        {
                            StartBlendedMoves(movePattern.Workpiece, axes);
                            // Sleep through first prior move
                            long ms = axes.SleepUntilTriggeringComplete();
                            movePattern.AddTiming("Sleep thru triggering move", ms.ToString());
                            
                            if (line.NumTriggerPts > 0 && line.TriggerPts[0].Repeating)
                            {
                                // <= Use Fixd Distance
                                Millimeters newDist = line.TriggerPts[0].Distance;
                                try
                                {
                                    if (this._myControler != null)
                                    {
                                        lock (axes)
                                        {
                                            // => Set up fixed distance PSO
                                            double dmotorCounts = PSOPrimaryAxis.ToMotorCounts(newDist);
                                            this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, dmotorCounts);
                                            LogAeroBasicScript(string.Format("PSODISTANCE {0} FIXED {1}", PSOPrimaryAxis.Name, dmotorCounts));
                                            // <= Add Single PSO Trigger to Controller
                                        }
                                    }
                                }// try 1st <==
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        U.LogError(ex, "MovePatternLineByLine. PSO FIXED DISTANCE TO CONTROLER 1st exception");
                                        ClearAllFaults();
                                        lock (axes)
                                        {
                                            this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                            // => Add Single PSO Trigger to Controller
                                            this._myControler.Commands[taskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(newDist));
                                            // <= Add Single PSO Trigger to Controller
                                        }
                                    }// try 2nd <==
                                    catch (Exception e)
                                    {
                                        U.LogAlarmPopup(e, "MovePatternLineByLine. PSO FIXED DISTANCE TO CONTROLER 2nd exception");
                                        return;
                                    }// catch (Excetpion e) <==
                                }// catch (Exception ex) <==
                                movePattern.AddTiming("*FixedDist PSO");
                                WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtDualLineStart", axes, lineStart.Z);
                                line.TriggerPts[0].DoStationaryAction();
                                U.SleepWithEvents(10);
                                try
                                {
                                    if (this._myControler != null)
                                    {
                                        lock (axes)
                                        {
                                            // => Set up fixed distance PSO
                                            this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                            //Debug.WriteLine(string.Format("{0}-DistanceFixed({1})", PSOPrimaryAxis.Name, newDist));
                                            // <= Add Single PSO Trigger to Controller
                                            LogAeroBasicScript(string.Format("PSOCONTROL {0} ARM", PSOPrimaryAxis.Name));
                                        }
                                    }
                                }// try 1st <==
                                catch (Exception ex)
                                {
                                    U.LogError(ex, "MovePatternLineByLine. PSO Control ARM 1st exception");
                                    ClearAllFaults();
                                }
                                // <= Use Fixd Distance
                                moveLinear(string.Format("Fixed Pitch triggering({0})", line.Name), new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ }, new double[] { lineStop.X, lineStop.Y, lineStop.Z }, line.Speed, taskId, true);
                                WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtDualLineStart", axes, lineStart.Z);
                                TurnOffPSO(movePattern.Workpiece, axes, PSOPrimaryAxis);
                                LogAeroBasicScript(string.Format("PSOCONTROL {0} OFF", PSOPrimaryAxis.Name));
                                continue;
                            }
                            else // !(line.NumTriggerPts > 0 && line.TriggerPts[0].Repeating)
                            {

                                if (UsePSOCounter)
                                {
                                    int counter = UpdateCounter(PSOPrimaryAxis);
                                    movePattern.AddTiming("UpdateCounter", counter.ToString());
                                }
                                else
                                {
                                    UpdateAxisCounter(PSOPrimaryAxis);
                                }
                                GTriggerPoint[] NewTP = line.TriggerPts;
                                int iStartTaskDouble = 0;
                                string sStartPosTask = "$task[" + iStartTaskDouble.ToString() + "]";

                                if (axes.IsDualPSO)
                                {
                                    //// => Converted Two Axis Motion into Single Axis Tracking
                                    loadPSODistancesXYAxis(movePattern, PSOPrimaryAxis, new GLine[] { line }, iStartTaskDouble, taskId);
                                }
                                else if (PSOPrimaryAxis.PSOEncoders[0].Contains('Y'))
                                {
                                    //// => Converted Two Axis Motion into Single Axis Tracking
                                    loadPSODistancesYAxis(movePattern, PSOPrimaryAxis, new GLine[] { line }, iStartTaskDouble, taskId);
                                }
                                else
                                {
                                    //// => Converted Two Axis Motion into Single Axis Tracking
                                    loadPSODistancesXAxis(movePattern, PSOPrimaryAxis, new GLine[] { line }, iStartTaskDouble, taskId);
                                }
                                movePattern.AddTiming("*loadPSODistancesXAxis");

                                //// <= Converted Two Axis Motion into Single Axis Tracking
                                // <= Send Dual PSO Trigger to Controller

                                // => Add PSO Trigger to Distance Array
                                try
                                {
                                    lock (axes)
                                    {
                                        // => Add PSO Trigger to Distance Array
                                        if (this._myControler != null)
                                        {
                                            this._myControler.Commands[taskId].PSO.Array(PSOPrimaryAxis.AxisNo, sStartPosTask, 0, NewTP.Length);
                                        }
                                        LogAeroBasicScript(string.Format("ARRAY {0} WRITE {1} {2} {3}", PSOPrimaryAxis.Name, sStartPosTask, 0, NewTP.Length));

                                        //Debug.WriteLine(string.Format("{0}-Array({1},{2})", PSOPrimaryAxis.Name, NewTP[0], NewTP[1]));
                                        //this._myControler.Commands[taskId].DataAcquisition.ArrayRead(PSOPrimaryAxis.AxisNo, 0, 0, NewTP.Length);
                                        //if (NewTP[0].PulseWidth.Val > 0)
                                        //{
                                        //    double triggerOnTime = NewTP[0].PulseWidth.Val;
                                        //    double triggerOnOffTime = triggerOnTime + PSOPulseOnTime;
                                        //    this._myControler.Commands[taskId].PSO.Pulse(PSOPrimaryAxis.AxisNo, triggerOnOffTime, triggerOnTime);
                                        //}
                                        if (this._myControler != null)
                                        {
                                            this._myControler.Commands[taskId].PSO.DistanceArray(PSOPrimaryAxis.AxisNo, 0, NewTP.Length);
                                        }
                                        LogAeroBasicScript(string.Format("PSODISTANCE {0} ARRAY {1} {2}", PSOPrimaryAxis.Name, 0, NewTP.Length));
                                        //Debug.WriteLine(string.Format("{0}-DistanceArray({1})", PSOPrimaryAxis.Name, NewTP.Length));
                                        // <= Add PSO Trigger to Distance Array
                                    }
                                }// try 1st <==
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        HandleAxesException(ex, axes, "MovePatternLineByLine.Setup PSO ARRAY First exception.  Will retry");
                                        lock (axes)
                                        {
                                            this._myControler.Commands[taskId].Execute("DWELL 0.1");
                                            // => Add PSO Trigger to Distance Array
                                            this._myControler.Commands[taskId].PSO.Array(PSOPrimaryAxis.AxisNo, sStartPosTask, 0, NewTP.Length);
                                            this._myControler.Commands[taskId].PSO.DistanceArray(PSOPrimaryAxis.AxisNo, 0, NewTP.Length);
                                            // <= Add PSO Trigger to Distance Array
                                        }
                                    }// try 2nd <==
                                    catch (Exception e)
                                    {

                                        U.LogAlarmPopup(e, "MovePatternLineByLine.Setup PSO ARRAY second exception");
                                        return;
                                    }// catch Excetpion e) <==
                                }// catch (Exception ex) <==
                                movePattern.AddTiming("*PSO.DistanceArray");

                                // Now its safe to set up this PSO
                                //if (axes.StartTime == 0)
                                //{
                                //    lock (axes)
                                //    {
                                //        // => Add PSO Trigger to Distance Array
                                //        this._myControler.Tasks[taskId].Program.Start();
                                //    }
                                //    axes.StartTime = U.DateTimeNow;
                                //}
                                // => Send Dual PSO Trigger to Controller
                                // <= Add PSO Trigger to Distance Array
                            }
                        }// if (PSOPrimaryAxis.PSOEncoders.Length == 2) <==

                        WaitAllBlendedMovesDone(movePattern.Workpiece, "WaitAtDualLineStart", axes, lineStart.Z);
                        // => Turn On PSO
                        if (!bIntermediateLineStopPoint)
                        {
                            try
                            {
                                lock (axes)
                                {
                                    if (this._myControler != null)
                                    {
                                        this._myControler.Commands[taskId].Execute(PreArmDwell);
                                        this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                    }
                                    //Debug.WriteLine(string.Format("{0}-Control(ARM)", PSOPrimaryAxis.Name));
                                    LogAeroBasicScript(PreArmDwell);
                                    LogAeroBasicScript(string.Format("PSOCONTROL {0} ARM", PSOPrimaryAxis.Name));
                                    if (camTable >= 0)
                                    {
                                        int globalIndex = camTable * CamTableMaxCount;
                                        axes.CamTableNum = camTable;
                                        if (this._myControler != null)
                                        {
                                            this._myControler.Variables.Global.Doubles.SetMultiple(globalIndex, line.CamTableArray);
                                            if (line.CamDirY)
                                            {
                                                this._myControler.Commands[taskId].Camming.LoadCamVariablesUnits(axes.AxisY.AxisNo, camTable,
                                                        axes.AxisZ.AxisNo, CammingInterpolationType.CubicSpline, lineStart.Y, line.CamTableDelta,
                                                        globalIndex, line.CamTableCount, CammingTrackingMode.PositionCommand);
                                            }
                                            else
                                            {
                                                this._myControler.Commands[taskId].Camming.LoadCamVariablesUnits(axes.AxisX.AxisNo, camTable,
                                                        axes.AxisZ.AxisNo, CammingInterpolationType.CubicSpline, lineStart.X, line.CamTableDelta,
                                                        globalIndex, line.CamTableCount, CammingTrackingMode.PositionCommand);
                                            }
                                            this._myControler.Commands[taskId].Camming.CamSync(axes.AxisZ.AxisNo, camTable, CammingSyncMode.Relative);
                                        }
                                        if (LogCommandScript)
                                        {
                                            for (int i = 0; i < line.CamTableArray.Length; i++)
                                            {
                                                LogAeroBasicScript(string.Format("$GLOBAL[{0}]={1}", i + globalIndex, line.CamTableArray[i]));
                                            }
                                            // LOADCAMVAR XX 1 ZZ 1 RANGE 100 1 $global[0] 10 1 UNITS
                                            LogAeroBasicScript(string.Format("LOADCAMVAR {0} {1} {2} 1 RANGE {3} {4} $GLOBAL[{5}] {6} 1 UNITS",
                                                axes.AxisX.Name, camTable, axes.AxisZ.Name, /* RANGE*/ lineStart.X.Val, line.CamTableDelta,
                                                globalIndex, line.CamTableCount));
                                            //SYNC ZZ 1 1
                                            LogAeroBasicScript(string.Format("SYNC {0} {1} 1", axes.AxisZ.Name, camTable));
                                        }
                                    }
                                }
                            }// try 1st <==
                            catch (Exception ex)
                            {
                                try
                                {
                                    U.LogError(ex, "MovePatternLineByLine.ARM PSO First exception.  Will retry");
                                    ClearAllFaults();
                                    lock (axes)
                                    {
                                        this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                        // => Turn PSO OFF to reset counters
                                        this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                        // <=Turn PSO OFF to reset counters

                                        this._myControler.Commands[taskId].Execute("DWELL 0.01");

                                        // => Arm PSO Output
                                        this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                        // <= Arm PSO Output
                                    }
                                }// try 2nd <==
                                catch (Exception e)
                                {
                                    U.LogAlarmPopup(e, "MovePatternLineByLine.ARM PSO second exception");
                                    return;
                                }// catch (Exception e) <==
                            }// catch (Exception ex) <==
                            movePattern.AddTiming("*PSO.ARM");
                        }// if (!bIntermediateLineStopPoint) <==
                        // <= Wait for Motion and Turn On PSO
                    } // if (line.NumTriggerPts == 0){} else <==

                    //if (camTable >= 0 && line.NumProfilePts == 6)
                    //{
                    //    G3DPoint pt1 = line.GetRobotLoc(line.ProfilePts[0].Distance, true, true);
                    //    G3DPoint pt2 = line.GetRobotLoc(line.ProfilePts[1].Distance, true, true);
                    //    G3DPoint pt3 = line.GetRobotLoc(line.ProfilePts[3].Distance, true, true);
                    //    G3DPoint pt4 = line.GetRobotLoc(line.ProfilePts[4].Distance, true, true);
                    //    moveLinear("Blended pt2", new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { pt2.X, pt2.Y }, line.Speed, taskId, true);
                    //    moveLinear("Blended pt1", new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { pt1.X, pt1.Y }, line.Speed, taskId, true);
                    //    moveLinear("Blended pt4", new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { pt4.X, pt4.Y }, line.Speed, taskId, true);
                    //    moveLinear("Blended pt3", new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { pt3.X, pt3.Y }, line.Speed, taskId, true);
                    //}
                    // => Make Line move with triggering
                    //System.Diagnostics.Debug.WriteLine(string.Format("Speed to End of PSOLine = {0}", line.Speed.ToString()));                    
                    if (camTable >= 0)
                    {
                        moveLinear(string.Format("Blended triggering({0})", line.Name), new RealAxis[] { axes.AxisX, axes.AxisY }, new double[] { lineStop.X, lineStop.Y }, line.Speed, taskId, true);
                    }
                    else
                    {
                        moveLinear(string.Format("Blended triggering({0})", line.Name), new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ }, new double[] { lineStop.X, lineStop.Y, lineStop.Z }, line.Speed, taskId, true);
                    }

                    TurnOffPSO(movePattern.Workpiece, axes, PSOPrimaryAxis);
                    LogAeroBasicScript(string.Format("PSOCONTROL {0} OFF", PSOPrimaryAxis.Name));
                    if (camTable >= 0)
                    {

                        if (this._myControler != null)
                        {
                            lock (axes)
                            {
                                this._myControler.Commands[taskId].Camming.CamSync(axes.AxisZ.AxisNo, camTable, CammingSyncMode.Stop);
                            }
                        }
                        LogAeroBasicScript(string.Format("SYNC {0} {1} 0", axes.AxisZ.Name, camTable));
                    }

                    // <= Make Line move with triggering

                    //TurnOffPSO(movePattern.Workpiece, axes, PSOPrimaryAxis);

                } //foreach (GLine line in pattern.ChildArray) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error MovePatternLineByLine axes:{0} pattern:{1}.", axes.AxesNo.ToString(), movePattern.ToString());
            }
        }

        private volatile bool _suppressAxisTaskErrorDetection = false;
        private void HandleAxesException(Exception ex, AxesBase axes, string msg)
        {
            U.SleepWithEvents(200);
            _suppressAxisTaskErrorDetection = true;
            lock (_lockCommand)
            {
                // Acknowledge All Faults.  
                this._myControler.Commands.AcknowledgeAll();
            }
            if (ex == null)
            {
                _suppressAxisTaskErrorDetection = false;
                return;
            }
            msg += string.Format("-Axes({0})", axes.Name);
            TaskId taskId = getTaskID(axes.AxesNo);
            for(int i=0; i<32; i++)
            {
                if ( _taskStates[i] == TaskState.Error)
                {
                    if (_taskErrorDescription[i].Contains("An axis fault occurred"))
                    {
                        RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
                        foreach (RealAxis axis in axisList)
                        {
                            if (axis.FaultDescription != "None")
                            {
                                RealAxes realAxes = axis.GetParent<RealAxes>();
                                lock (realAxes)
                                {
                                    try
                                    {
                                        this._myControler.Commands.Axes[axis.AxisNo].Motion.Enable();
                                        this._myControler.Commands.Axes[axis.AxisNo].Motion.Home();
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
                
            _suppressAxisTaskErrorDetection = false;
            U.LogError(ex, msg);
        }

                    // => Turn Off PSO
        private void TurnOffPSO(GWorkPiece workPiece, AxesBase axes, RealAxis psoPrimaryAxis)
        {

            if (psoPrimaryAxis == null)
            {
                return;
            }
            try
            {
                lock (axes)
                {
                    if (this._myControler != null)
                    {
                        // => Turn PSO Output Off
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(psoPrimaryAxis.AxisNo, PsoMode.Off);
                    }
                    //Debug.WriteLine(string.Format("{0}-Control(OFF)", psoPrimaryAxis.Name));
                    workPiece.AddTimingElement("Turned OFF PSO");
                    // <= Turn PSO Output Off
                }
            }// try 1st <==
            catch (Exception ex)
            {
                try
                {
                    U.LogError(ex, "MovePatternLineByLine.PSOMODE.OFF First exception.  Will retry");
                    ClearAllFaults();
                    lock (axes)
                    {
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                        // => Turn PSO Output Off
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(psoPrimaryAxis.AxisNo, PsoMode.Off);
                        // <= Turn PSO Output Off
                    }
                }// try 2nd <==
                catch (Exception e)
                {
                    U.LogAlarmPopup(e, "MovePatternLineByLine.PSOMODE.OFF second exception");
                    return;
                }// catch (Exception e) <==
            }// catch (Exception ex) <==
             
        } // <= TurnOffPSO
        

        /// <summary>
        /// Move Pattern in Blended Moves with Single Axis PSO Hardware Triggering
        /// </summary>
        /// <param name="PSOPrimaryAxis"></param>
        /// <param name="axes"></param>
        /// <param name="movePattern"></param>
        public void MovePatternBlended(RealAxis PSOPrimaryAxis, RealAxes axes, GPattern movePattern)
        {
            try
            {
                RealAxis[] ax3 = new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ };
                // => Move Gantry to Line Start Point
                moveLinear("PatternBlended ToStart", ax3, new double[] { movePattern.Lines[0].RobotLoc.X, movePattern.Lines[0].RobotLoc.Y, movePattern.Lines[0].RobotLoc.Z }, movePattern.Lines[0].Speed, getTaskID(axes.AxesNo), false);
                // <=

                if (PSOPrimaryAxis.PSOEncoders.Length == 2)
                {
                    GPattern blendedLines = new GPattern();
                    GPattern blendedWindows = new GPattern();
                    Millimeters dummyWindowOffset = 1;
                    Millimeters dummyWindowWidth = 0.5;
                    GLine line = null;
                    foreach (GLine newLine in movePattern.Lines)
                    {
                        if (line != null)
                        {
                            blendedLines.AddLine(line);
                            blendedWindows.AddLine(line);
                            G3DPoint lineStart = line.RobotLoc;
                            G3DPoint lineStop = line.EndPoint.RobotLoc;
                            G3DPoint newLineStart = newLine.RobotLoc;
                            G3DPoint newLineStop = newLine.EndPoint.RobotLoc;

                            if (line.NumTriggerPts == 0)
                            {
                                // Do nothing 
                            }
                            else
                            {

                                if (lineStart.X < lineStop.X)
                                {
                                    // line Direction Left-to-Right (L-R)
                                    if (newLineStart.X < newLineStop.X)
                                    {
                                        //line and newLine same x direction
                                        // line(L-R) and newLine(L-R)
                                        if (newLineStart.X > lineStop.X)
                                        {
                                            //no intermediate window required
                                            
                                        }
                                        else
                                        {
                                            //Add 1 line to create dummy trigger and dummy window
                                            G3DPoint dummyLineStart = newLine.GetRobotLoc(-1 * (dummyWindowOffset + dummyWindowWidth));
                                            G3DPoint dummyLineStop = newLine.GetRobotLoc(-1 * (dummyWindowOffset - dummyWindowWidth));
                                            GLine dummyLine = new GLine(dummyLineStart, dummyLineStop) { Speed = newLine.Speed };
                                            
                                            ////Dummy Trigger outside Intermediate Window
                                            //GTriggerPoint dummyTrigger = new GTriggerPoint();
                                            //dummyTrigger.Distance = 0.75 * dummyLine.XYLength;
                                            //dummyLine.AddTrigger(dummyTrigger);
                                            blendedLines.AddLine(dummyLine);

                                            //Intermediate window
                                            G3DPoint dummyWindowStart = dummyLine.GetRobotLoc(0.5 * dummyLine.XYLength);
                                            G3DPoint dummyWindowStop = dummyLine.GetRobotLoc(dummyLine.XYLength);
                                            GLine dummyWindow = new GLine(dummyWindowStart, dummyWindowStop) { Speed = newLine.Speed };
                                            blendedWindows.AddLine(dummyWindow);
                                        }
                                    }// if (newLine.AbsBeginPoint.X < newLine.AbsEndPoint.X) <==
                                    else
                                    {
                                        //line and newLine opposite x direction
                                        // line(L-R) and newLine(R-L)
                                        if (newLineStart.X < lineStart.X)
                                        {
                                            //no intermediate window required
                                            
                                        }
                                        else
                                        {
                                            //Add 1 line to create dummy trigger and dummy window
                                            G3DPoint dummyLineStart = newLine.GetRobotLoc(-1 * (dummyWindowOffset + dummyWindowWidth));
                                            G3DPoint dummyLineStop = newLine.GetRobotLoc(-1 * (dummyWindowOffset - dummyWindowWidth));
                                            GLine dummyLine = new GLine(dummyLineStart, dummyLineStop) { Speed = newLine.Speed };
                                            
                                            ////Dummy Trigger outside Intermediate Window
                                            //GTriggerPoint dummyTrigger = new GTriggerPoint();
                                            //dummyTrigger.Distance = 0.75 * dummyLine.XYLength;
                                            //dummyLine.AddTrigger(dummyTrigger);
                                            blendedLines.AddLine(dummyLine);

                                            //Intermediate window
                                            G3DPoint dummyWindowStart = dummyLine.GetRobotLoc(0.5 * dummyLine.XYLength);
                                            G3DPoint dummyWindowStop = dummyLine.GetRobotLoc(dummyLine.XYLength);
                                            GLine dummyWindow = new GLine(dummyWindowStart, dummyWindowStop) { Speed = newLine.Speed };
                                            blendedWindows.AddLine(dummyWindow);
                                        }
                                    }// if (newLine.AbsBeginPoint.X < newLine.AbsEndPoint.X) {} else <==
                                }// if (line.AbsBeginPoint.X < line.AbsEndPoint.X) <==
                                else
                                {
                                    // line Direction Right-to-Left (R-L)
                                    if (newLineStart.X > newLineStop.X)
                                    {
                                        //line and newLine same x direction
                                        // line(R-L) and newLine(R-L)
                                        if (newLineStart.X < lineStop.X)
                                        {
                                            //no intermediate window required
                                            
                                        }
                                        else
                                        {
                                            //Add 1 line to create dummy trigger and dummy window
                                            G3DPoint dummyLineStart = newLine.GetRobotLoc(-1 * (dummyWindowOffset + dummyWindowWidth));
                                            G3DPoint dummyLineStop = newLine.GetRobotLoc(-1 * (dummyWindowOffset - dummyWindowWidth));
                                            GLine dummyLine = new GLine(dummyLineStart, dummyLineStop) { Speed = newLine.Speed };
                                            
                                            ////Dummy Trigger outside Intermediate Window
                                            //GTriggerPoint dummyTrigger = new GTriggerPoint();
                                            //dummyTrigger.Distance = 0.75 * dummyLine.XYLength;
                                            //dummyLine.AddTrigger(dummyTrigger);
                                            blendedLines.AddLine(dummyLine);

                                            //Intermediate window
                                            G3DPoint dummyWindowStart = dummyLine.GetRobotLoc(0.5 * dummyLine.XYLength);
                                            G3DPoint dummyWindowStop = dummyLine.GetRobotLoc(dummyLine.XYLength);
                                            GLine dummyWindow = new GLine(dummyWindowStart, dummyWindowStop) { Speed = newLine.Speed };
                                            blendedWindows.AddLine(dummyWindow);
                                        }
                                    }// if (newLine.AbsBeginPoint.X > newLine.AbsEndPoint.X) <==
                                    else
                                    {
                                        //line and newLine opposite x direction
                                        // Line(R-L) and newLine(L-R)
                                        if (newLineStart.X > lineStart.X)
                                        {
                                            //no intermediate window required
                                            
                                        }
                                        else
                                        {
                                            //Add 1 line to create dummy trigger and dummy window
                                            G3DPoint dummyLineStart = newLine.GetRobotLoc(-1 * (dummyWindowOffset + dummyWindowWidth));
                                            G3DPoint dummyLineStop = newLine.GetRobotLoc(-1 * (dummyWindowOffset - dummyWindowWidth));
                                            GLine dummyLine = new GLine(dummyLineStart, dummyLineStop) { Speed = newLine.Speed };
                                            
                                            ////Dummy Trigger outside Intermediate Window
                                            //GTriggerPoint dummyTrigger = new GTriggerPoint();
                                            //dummyTrigger.Distance = 0.75 * dummyLine.XYLength;
                                            //dummyLine.AddTrigger(dummyTrigger);
                                            blendedLines.AddLine(dummyLine);

                                            //Intermediate window
                                            G3DPoint dummyWindowStart = dummyLine.GetRobotLoc(0.5 * dummyLine.XYLength);
                                            G3DPoint dummyWindowStop = dummyLine.GetRobotLoc(dummyLine.XYLength);
                                            GLine dummyWindow = new GLine(dummyWindowStart, dummyWindowStop) { Speed = newLine.Speed };
                                            blendedWindows.AddLine(dummyWindow);
                                        }

                                    }// if (newLine.AbsBeginPoint.X > newLine.AbsEndPoint.X) {} else <==
                                }// if (line.AbsBeginPoint.X < line.AbsEndPoint.X) {} else <==
                            }// if (line.NumTriggerPts == 0) {} else <==
                        }// if (line != null) <==
                                               
                        line = newLine;
                    }// foreach (GLine newLine in movePattern.ChildArray) <==
                    blendedLines.AddLine(line);
                    blendedWindows.AddLine(line);

                    GTriggerPoint[] NewTP = blendedLines.TriggerPts;
                    GLine[] lineArray = blendedLines.Lines;
                    int iStartTaskDistanceDouble = 0;
                    int iStartTaskWindowDouble = OffsetArrayWindow;
                    string sStartDistancePosTask = "$task[" + iStartTaskDistanceDouble.ToString() + "]";
                    string sStartWindowPosTask = "$task[" + iStartTaskWindowDouble.ToString() + "]";

                    // => Converted Two Axis Motion into Single Axis Tracking
                    loadPSODistancesWithWindows(PSOPrimaryAxis, lineArray, iStartTaskDistanceDouble, getTaskID(axes.AxesNo));
                    // <=
                    
                    // => PSO Windows
                    loadPSOWindowsXAxis(PSOPrimaryAxis, blendedWindows, iStartTaskWindowDouble, getTaskID(axes.AxesNo));
                    // <=

                    lock (axes)
                    {
                        // => Add PSO Trigger to Distance Array
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Array(PSOPrimaryAxis.AxisNo, sStartDistancePosTask, iStartTaskDistanceDouble, NewTP.Length);
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.DistanceArray(PSOPrimaryAxis.AxisNo, iStartTaskDistanceDouble, NewTP.Length);
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.WindowInput(PSOPrimaryAxis.AxisNo, 1, 0);
                        
                        // =>Debug
                        if (PSOPrimaryAxis.Name == "XXP")
                        {
                            this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.WindowInput(PSOPrimaryAxis.AxisNo, 1, 1);
                        }

                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Array(PSOPrimaryAxis.AxisNo, sStartWindowPosTask, iStartTaskWindowDouble, blendedWindows.Lines.Length * 2);
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.WindowRangeArrayEdge(PSOPrimaryAxis.AxisNo, 1, iStartTaskWindowDouble, blendedWindows.Lines.Length * 2, 0);
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.OutputPulseWindowMask(PSOPrimaryAxis.AxisNo, 0);
                        // <=
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.TrackReset(PSOPrimaryAxis.AxisNo, 1 + 32 + 64);
                    }


                    // => Wait for motion to Line Start Position complete
                    U.SleepWithEvents(100); 
                    axes.AxisX.WaitMoveDone.Reset();
                    axes.AxisY.WaitMoveDone.Reset();
                    U.BlockOrDoEvents(axes.AxisX.WaitMoveDone, 20000);
                    U.BlockOrDoEvents(axes.AxisY.WaitMoveDone, 20000);
                    // <=
                    lock (axes)
                    {
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("VELOCITY ON");
                        
                        // => Enable Windowing
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.WindowOn(PSOPrimaryAxis.AxisNo, 1);
                        // <=
                        // => Arm PSO Output
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                        // <=
                    }

                    foreach (GLine moveLine in blendedLines.Lines)
                    {
                        G3DPoint moveLineStart = moveLine.RobotLoc;
                        G3DPoint moveLineStop = moveLine.EndPoint.RobotLoc;
                        // => Move Gantry to Line Start Point
                        moveLinear("PatternBlended Start2", ax3, new double[] { moveLineStart.X, moveLineStart.Y, moveLineStart.Z }, moveLine.Speed, getTaskID(axes.AxesNo), false);
                        // <=

                        // => Make Line move with triggering
                        moveLinear("PatternBlended triggering", ax3, new double[] { moveLineStop.X, moveLineStop.Y, moveLineStop.Z }, moveLine.Speed, getTaskID(axes.AxesNo), false);
                        // <=
                        if (UsePSOCounter)
                        {
                            // <=Debug
                            UpdateCounter(null);
                        }

                    } //foreach (GLine line in pattern.ChildArray) <==

                    lock (axes)
                    {
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });

                        // => Turn PSO Output Off
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                        // <=
                        // => Turn PSO Window Off
                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.WindowOff(PSOPrimaryAxis.AxisNo, 1);
                        // <=

                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("VELOCITY OFF");
                        
                    }
                }// if (PSOPrimaryAxis.PSOEncoders.Length == 2) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error MovePatternLineByLine axes:{0} pattern:{1}.", axes.AxesNo.ToString(), movePattern.ToString());
            }
        }


        #region Temp for testing blended moves
//        private void TestDispense2(GWorkPiece workPiece, RealAxes axes)
//        {
//            workPiece.AddTimingElement(axes.Name, "TestDispense2 Entry");
//            TaskId myTask = getTaskID(axes.AxesNo);
//            RealAxis psoAxis = returnPSOPrimaryAxis(axes, "DispTrigger");
//            string[] threeAxis = null;
//            string axisPSO = string.Empty;
//            double xPos = 0;
//            if (axes.AxesNo == 0)
//            {
//                threeAxis = new string[] { "XX", "YY", "ZZ" };
//                axisPSO = "XXP";
//                xPos = 50.0;
//            }
//            else
//            {
//                threeAxis = new string[] { "X", "Y", "Z" };
//                axisPSO = "X";
//                xPos = -150.0;
//            }
//            lock (axes)
//            {
//                //LINEAR XX 100 YY 200 ZZ 37 F150	' To Start
//                this._myControler.Tasks[myTask].Program.Start();
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 0, 200, 37 }, 50);
//                this._myControler.Commands[myTask].Execute("DWELL 0.2");
//                this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, threeAxis);
//                this._myControler.Tasks[myTask].Program.Debug.Pause();
//            }
//            workPiece.AddTimingElement(axes.Name, "TestDispense2 Start");
//            lock (axes)
//            {
//                //LINEAR XX 100 YY 200 ZZ 37 F150	' To Start
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 10, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 20, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 30, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 40, 200, 37 }, 150);
//            }
//            workPiece.AddTimingElement(axes.Name, "4 linear commands sent");
//            lock (axes)
//            {
//                this._myControler.Tasks[myTask].Program.Start();
//            }
//            workPiece.AddTimingElement(axes.Name, "Program.Start");
//            lock (axes)
//            {
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 50, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 60, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 70, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 80, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 90, 200, 37 }, 150);
//                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos + 100, 200, 37 }, 150);
//            }
//            workPiece.AddTimingElement(axes.Name, "All commands Sent");
//            long ms = U.SleepWithEvents(600);
//            workPiece.AddTimingElement( axes.Name, "Sleep", ms.ToString());
//            lock (axes)
//            {
//                //WAIT MOVEDONE XX YY
//                this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, threeAxis);
//            }
//            workPiece.AddTimingElement(axes.Name, "WaitForMotionDone");
//            do
//            {
//                _pollingDone.Reset();
//                _pollingDone.WaitOne(100);
//            } while (!axes.QueueEmpty);

//            lock (axes)
//            {
//                this._myControler.Tasks[myTask].Program.Debug.Pause();
//            }
//        }
        private void TestDispense(GWorkPiece workPiece, RealAxes axes, int i)
        {
            TaskId myTask = getTaskID(axes.AxesNo);
            RealAxis psoAxis = returnPSOPrimaryAxis(axes, "DispTrigger");
            string[] threeAxis = null;
            string axisPSO = string.Empty;
            double xPos = 0;
            if (axes.AxesNo == 0)
            {
                threeAxis = new string[] { "XX", "YY", "ZZ" };
                axisPSO = "XXP";
                xPos = 100.0 + i*10;
            }
            else
            {
                threeAxis = new string[] { "X", "Y", "Z" };
                axisPSO = "X";
                xPos = -100.0;
            }

            lock (axes)
            {
                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos, 120, 42.5 }, 150);
                this._myControler.Tasks[myTask].Program.Start();
            }
            long ms = U.SleepWithEvents(300);
            workPiece.AddTimingElement("Sleep through blended move", ms.ToString());
            lock (axes)
            {
                this._myControler.Variables.Tasks[myTask].Doubles.SetMultiple(0, new double[] {
                -6984,
                -2506,
                -2506,
                -2506,
                -2506,
                -16276,
                -2506,
                -2506,
                -2506,
                -2506
                });
                this._myControler.Commands[myTask].PSO.Array(axisPSO, "$Task[0]", 0, 10);
                this._myControler.Commands[myTask].PSO.DistanceArray(axisPSO, 0, 10);
            }
            lock (axes)
            {
                this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, threeAxis);
                this._myControler.Commands[myTask].Execute(PreArmDwell);
                this._myControler.Commands[myTask].PSO.Control(axisPSO, PsoMode.Arm);
                this._myControler.Tasks[myTask].Program.Debug.Pause();
                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos - 5, 120, 42.5 }, 20);
                this._myControler.Commands[myTask].PSO.Control(axisPSO, PsoMode.Off);
            }
            //' Bottom Move
            lock (axes)
            {
                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos - 5, 118.5, 42.5 }, 150);
                this._myControler.Tasks[myTask].Program.Start();
            }
            ms = U.SleepWithEvents(300);
            lock (axes)
            {
                this._myControler.Variables.Tasks[myTask].Doubles.SetMultiple(0, new double[] {
                6012,
                2644,
                2644,
                2644,
                2644,
                16675,
                2351,
                2351,
                2351,
                2351
                });
                this._myControler.Commands[myTask].PSO.Array(axisPSO, "$Task[0]", 0, 10);
                this._myControler.Commands[myTask].PSO.DistanceArray(axisPSO, 0, 10);
            }
            lock (axes)
            {
                this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, threeAxis);
                this._myControler.Commands[myTask].Execute(PreArmDwell);
                this._myControler.Commands[myTask].PSO.Control(axisPSO, PsoMode.Arm);
                this._myControler.Tasks[myTask].Program.Debug.Pause();
                this._myControler.Commands[myTask].Motion.Linear(threeAxis, new double[] { xPos, 118.5, 42.5 }, 20);
                this._myControler.Commands[myTask].PSO.Control(axisPSO, PsoMode.Off);
            }
            ms = U.SleepWithEvents(18);
        }
        #endregion Temp for testing blended moves
        private double _zHt = 0.0;

        private bool MoveWorkPieceBlended(GWorkPiece workPiece, RealAxes axes, GPattern[] patList)
        {


            //Do Work
            Boolean TransfereActive = true;
            RealAxis PSOPrimaryAxis = null;
            RealAxis[] ax3 = new RealAxis[] { axes.AxisX, axes.AxisY, axes.AxisZ };
            bool firstMove = true;
            bool pulseSetup = false;
            try
            {
                workPiece.AddTimingElement("StartWorkpieceBlended");
                LogAeroBasicScript(";");
                LogAeroBasicScript(string.Format("; StartWorkpieceBlended for {0}", workPiece.CurrentOperation));
                LogAeroBasicScript(";");
                if (TransfereActive == true)
                {
                    //safetyTransferMove(workPiece, axes, patList[0]);
                    axes.ResetBlendedMove(true);
                    FireWorkpieceBegin();
                    //workPiece.AddTimingElement("FireWorkpieceBegin");

                    TransfereActive = false;
                }

                TaskId axesTaskId = getTaskID(axes.AxesNo);
                
                G3DPoint newLineStartFixedPitchX = null;
                lock (axes)
                {
                    if( _myControler != null)
                    {
                        this._myControler.Parameters.Tasks[axesTaskId].Motion.Lookahead.CoordinatedAccelLimit.Value = axes.AccelDecel;
                        this._myControler.Commands[axesTaskId].Motion.Setup.RampRate(axes.AccelDecel);
                    }
                    axes.ResetBlendedMove(true);
                    CommandExecute("VELOCITY ON", axesTaskId);

                    if (RoundedCornerEnable)
                    {
                        // => Setup Rounded Corners
                        string sRoundCornerCmd = "ROUNDING AXES " + axes.AxisX.Name + " " + axes.AxisY.Name;
                        CommandExecute(sRoundCornerCmd, axesTaskId);

                        sRoundCornerCmd = "ROUNDING TOLERANCE " + RoundedCornerTolerance.Val.ToString();
                        CommandExecute(sRoundCornerCmd, axesTaskId);

                        CommandExecute("ROUNDING ON", axesTaskId);
                    }
                    // <= Setup Rounded Corners
                }
                
                if (workPiece.TriggerPointIDList != null)
                {
                    foreach (string triggerID in workPiece.TriggerPointIDList)
                    {
                        RealAxis psoAxis = returnPSOPrimaryAxis(axes, triggerID);
                        if (psoAxis != null && psoAxis.PSOEncoders != null && psoAxis.PSOEncoders.Length > 0)
                        {
                            setupPSOType(axes, triggerID, out pulseSetup);
                            if (!pulseSetup)
                            {
                                lock (axes)
                                {
                                    // => PSO Setup to Controller
                                    if (this._myControler != null)
                                    {
                                        this._myControler.Commands[axesTaskId].PSO.Pulse(psoAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnTime);
                                    }
                                    //Debug.WriteLine(string.Format("{0}-Pulse({1}, {2}", psoAxis.Name, PSOPulseOnOffTime, PSOPulseOnTime));
                                    // <= PSO Setup to Controller
                                }
                                axes.CurrentPSOPulseOnOffTime = PSOPulseOnOffTime;
                                axes.CurrentPSOPulseOnTime = PSOPulseOnTime;
                            }
                        }
                    }
                    workPiece.AddTimingElement("setupPSOType");
                }

                #region Temp for testing Blended moves
                // Temp
                if (UseTempPattern && workPiece.TriggerPointIDList[0] == "DispTrigger")
                {
                    lock (axes)
                    {
                        this._myControler.Tasks[axesTaskId].Program.Debug.Pause();
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        TestDispense(workPiece, axes, i);
                    }
                    lock (axes)
                    {
                        this._myControler.Tasks[axesTaskId].Program.Start();
                        U.SleepWithEvents(100);
                        this._myControler.Commands[axesTaskId].Motion.WaitForMotionDone(WaitOption.MoveDone, new string[] { "XX", "YY", "ZZ" });
                    }

                    return true;
                }

                // End Temp
                #endregion Temp for testing Blended moves

                RealAxis PSODualPrimaryAxis = null;

                foreach (GPattern pattern in patList)
                {
                    PSOPrimaryAxis = null;
                    pattern.PrepareForExecute();
                    if (pattern.WaitingForReady)
                    {
                        StartBlendedMoves(workPiece, axes);
                    }
                    pattern.WaitForReady();
                    //System.Diagnostics.Debug.WriteLine(string.Format("Aerotech StartedMotion {0}", pattern.Lines[0].Nickname));

                    if (workPiece.AbortRun)
                    {
                        if (this._myControler != null)
                        {
                            lock (axes)
                            {
                                ClearAllFaults();
                                if (workPiece.TriggerPointIDList != null)
                                {
                                    foreach (string triggerID in workPiece.TriggerPointIDList)
                                    {
                                        RealAxis psoAxis = returnPSOPrimaryAxis(axes, triggerID);
                                        if (psoAxis != null && psoAxis.PSOEncoders != null && psoAxis.PSOEncoders.Length > 0)
                                        {
                                            TurnOffPSO(workPiece, axes, psoAxis);
                                        }
                                    }
                                    workPiece.AddTimingElement("setupPSOType");
                                }
                                if (RoundedCornerEnable)
                                {
                                    this._myControler.Commands[axesTaskId].Execute("ROUNDING OFF");
                                    LogAeroBasicScript("ROUNDING OFF");
                                }
                                this._myControler.Commands[axesTaskId].Execute("VELOCITY OFF");
                                LogAeroBasicScript("VELOCITY OFF");
                                this._myControler.Tasks[axesTaskId].Program.Start();
                                do
                                {
                                    _pollingDone.Reset();
                                    _pollingDone.WaitOne(100);
                                } while (!axes.QueueEmpty);

                                this._myControler.Tasks[axesTaskId].Program.Stop();
                                //this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Reset);

                            }
                            U.LogInfo("MoveWorkPiece: AbortRun: Reason={0}", workPiece.AbortReason);
                            return false;
                        }
                        workPiece.AbortRun = false;
                    }

                    // Any triggers?
                    GTriggerPoint[] triggers = pattern.TriggerPts;
                    if (triggers.Length > 0)
                    {
                        PSOPrimaryAxis = returnPSOPrimaryAxis(axes, triggers[0].TriggerID);
                        //workPiece.AddTimingElement("returnPSOPrimaryAxis");

                        if (PSOPrimaryAxis != null)
                        {
                            // Is PSO fully setup for PSO external triggerring?
                            bool isPSOSetup = PSOPrimaryAxis.PSOEncoders != null && PSOPrimaryAxis.PSOEncoders.Length > 0;
                            if (isPSOSetup && (!pulseSetup || triggers[0].PulseWidth.Val != 0))
                            {
                                double triggerOnTime = 0;
                                double onOffTime = 0;
                                if (triggers[0].PulseWidth.Val != 0)
                                {
                                    triggerOnTime = triggers[0].PulseWidth.Val;
                                    onOffTime = triggerOnTime + 2500;
                                }
                                else
                                {
                                    triggerOnTime = PSOPulseOnTime;
                                    onOffTime = PSOPulseOnOffTime;
                                }
                                if (triggerOnTime != axes.CurrentPSOPulseOnTime)
                                {
                                    // => PSO Setup to Controller
                                    try
                                    {
                                        lock (axes)
                                        {
                                            // => PSO Setup to Controller
                                            if (triggerOnTime < 0.0)
                                            {
                                                //this._myControler.Commands[axesTaskId].PSO.Pulse(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnTime);
                                                if (this._myControler != null)
                                                {
                                                    this._myControler.Commands[axesTaskId].PSO.OutputToggle(PSOPrimaryAxis.AxisNo);
                                                }
                                                //LogAeroBasicScript(string.Format("PSOPULSE {0} TIME {1} {2}", PSOPrimaryAxis.Name, PSOPulseOnOffTime, PSOPulseOnTime));
                                                LogAeroBasicScript(string.Format("PSOOUTPUT {0} TOGGLE", PSOPrimaryAxis.Name));
                                            }
                                            else
                                            {
                                                if (this._myControler != null)
                                                {
                                                    this._myControler.Commands[axesTaskId].PSO.Pulse(PSOPrimaryAxis.AxisNo, onOffTime, triggerOnTime);
                                                }
                                                LogAeroBasicScript(string.Format("PSOPULSE {0} TIME {1} {2}", PSOPrimaryAxis.Name, onOffTime, triggerOnTime));
                                            }
                                            //Debug.WriteLine(string.Format("{0}-Pulse({1}, {2}", PSOPrimaryAxis.Name, onOffTime, triggerOnTime));
                                            // <= PSO Setup to Controller
                                        }
                                    }// try 1st <==
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            U.LogError(ex, "MoveWorkpieceBlended.PSO SETUP TO CONTROLER 1st exception");
                                            ClearAllFaults();
                                            lock (axes)
                                            {
                                                this._myControler.Commands[axesTaskId].Execute("DWELL 0.1");

                                                // => PSO Setup to Controller
                                                if (triggerOnTime < 0.0)
                                                {
                                                    this._myControler.Commands[axesTaskId].PSO.OutputToggle(PSOPrimaryAxis.AxisNo);
                                                }
                                                else
                                                {
                                                    this._myControler.Commands[axesTaskId].PSO.Pulse(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, triggerOnTime);
                                                    // <= PSO Setup to Controller
                                                }
                                            }
                                        }// try 2nd <==
                                        catch (Exception e)
                                        {
                                            U.LogAlarmPopup(e, "MoveWorkpieceBlended.PSO SETUP TO CONTROLER 2nd exception");
                                            return false;
                                        }// catch (Excetpion e) <==
                                    }// catch (Exception ex) <==
                                    axes.CurrentPSOPulseOnTime = triggerOnTime;
                                    workPiece.AddTimingElement("PSO Pulse/TrackDirection");
                                }
                                // <= PSO Setup to Controller
                            } // if (PSOPrimaryAxis.PSOEncoders != null && PSOPrimaryAxis.PSOEncoders.Length > 0) <==

                            GTriggerPoint[] NewTP = pattern.TriggerPts;
                            foreach (GTriggerPoint tp in NewTP)
                            {
                                tp.PrepareCallback(isPSOSetup);
                            }
                            // <=
                            workPiece.AddTimingElement("Prepare Callback");


                            if (PSOPrimaryAxis.PSOEncoders.Length == 1 || pattern.FixedPitchX)
                            {
                                G3DPoint LineEndFixedPitchX = pattern.Lines[0].EndPoint.RobotLoc;
                                if (pattern.FixedPitchX == true)
                                {
                                    // Do Work: Add option for FixedPitch

                                    if (newLineStartFixedPitchX == null)
                                    {
                                        // => Set Line Start Point to first Trigger Point
                                        //Millimeters lineTriggerDistanceX = pattern.Lines[0].TriggerPts[0].DistanceWithLead * Math.Cos(pattern.Lines[0].RobotLoc.Yaw);

                                        newLineStartFixedPitchX = pattern.Lines[0].GetRobotLoc(pattern.Lines[0].TriggerPts[0].DistanceWithLead);
                                        // <= Set Line Start Point to first Trigger Point

                                        //System.Diagnostics.Debug.WriteLine(string.Format("Speed to Start of FixedPSOLine = {0}", pattern.Lines[0].Speed.ToString()));
                                        // => Move Gantry to new Line Start Point
                                        double speed = workPiece.XYTransferSpeed;
                                        if (firstMove)
                                        {
                                            moveLinear("FixedX SafeUp", ax3, new double[] { ax3[0].CurrentPosition, ax3[1].CurrentPosition, workPiece.ZSafeTransfer }, workPiece.ZTransferSpeed, axesTaskId, false);
                                            moveLinear("FixedX Safe XY", ax3, new double[] { newLineStartFixedPitchX.X, newLineStartFixedPitchX.Y, workPiece.ZSafeTransfer }, workPiece.XYTransferSpeed, axesTaskId, false);
                                            speed = workPiece.ZTransferSpeed;
                                        }
                                        moveLinear("FixedX-FirstTrigger", ax3, new double[] { newLineStartFixedPitchX.X, newLineStartFixedPitchX.Y, newLineStartFixedPitchX.Z }, speed, axesTaskId, false);
                                        firstMove = false;
                                        // <= Move Gantry to new Line Start Point

                                        // => Wait Motion Complete
                                        WaitAllBlendedMovesDone(workPiece, "WaitAtFixedX-First Trigger", axes, newLineStartFixedPitchX.Z);
                                        //U.SleepWithEvents(100);
                                        // <= Wait Motion Complete

                                        // => PSO Trigger Fixed Distance to Controller
                                        try
                                        {
                                            if (this._myControler != null)
                                            {
                                                lock (axes)
                                                {
                                                    // => PSO Trigger Fixed Distance to Controller
                                                    this._myControler.Commands[axesTaskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(pattern.FixedPitchXDistance));
                                                    // <= PSO Trigger Fixed Distance to Controller
                                                    if (!string.IsNullOrEmpty(PreManualFireDwell))
                                                    {
                                                        this._myControler.Commands[axesTaskId].Execute(PreManualFireDwell);
                                                    }
                                                    // => Force First Trigger
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);

                                                    // => Arm PSO Output
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                                    // <= Arm PSO Output
                                                }
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                HandleAxesException(ex, axes, "MoveWorkpieceBlended.Fire First Trigger: First exception.  Will retry");
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[axesTaskId].Execute("DWELL 0.1");
                                                    // => PSO Trigger Fixed Distance to Controller
                                                    this._myControler.Commands[axesTaskId].PSO.DistanceFixed(PSOPrimaryAxis.AxisNo, PSOPrimaryAxis.ToMotorCounts(pattern.FixedPitchXDistance));
                                                    // <= PSO Trigger Fixed Distance to Controller

                                                    // => Force First Trigger
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);

                                                    // => Arm PSO Output
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Arm);
                                                    // <= Arm PSO Output
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {
                                                U.LogAlarmPopup(e, "MoveWorkpieceBlended.Fire First Trigger: second exception");
                                                return false;
                                            }// catch (Exception e) <==
                                        }// catch(Exception ex) <==
                                        workPiece.AddTimingElement("Fixed Distance Setup And FIRE");
                                        if (!StartBlendedMoves(workPiece, axes))
                                        {
                                            workPiece.AbortRun = true;
                                            workPiece.AbortReason = "Program Halted";
                                            return false;
                                        }
                                        // Wait until all cmds processed
                                        if (this._myControler != null)
                                        {
                                            do
                                            {
                                                _pollingDone.Reset();
                                                _pollingDone.WaitOne(100);
                                            } while (!axes.QueueEmpty);
                                        }
                                        workPiece.AddTimingElement("Wait for commands");
                                        // <= PSO Trigger Fixed Distance to Controller

                                       // G3DPoint LineEndFixedPitchX = pattern.Lines[0].EndPoint.RobotLoc;
                                        // => Move Gantry to Line End Point
                                        //System.Diagnostics.Debug.WriteLine(string.Format("Speed to End of 1st FixedPSOLine = {0}", pattern.Lines[0].Speed.ToString()));
                                        moveLinear("FixedX-FirstTrigger-ToEnd", ax3, new double[] { LineEndFixedPitchX.X, LineEndFixedPitchX.Y, LineEndFixedPitchX.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                                        // <= Move Gantry to Line End Point
                                    }// if (newLineStartFixedPitchX == null) <==
                                    else
                                    {
                                        if (!pattern.Lines[0].IgnoreStartPt)
                                        {
                                            // => Move Gantry to Line Begin Point
                                            G3DPoint LineStartFixedPitchX = pattern.Lines[0].RobotLoc;
                                            //System.Diagnostics.Debug.WriteLine(string.Format("Speed to start of blended FixedPSOLine = {0}", pattern.Lines[0].Speed.ToString()));
                                            moveLinear("FixedX-ToStart", ax3, new double[] { LineStartFixedPitchX.X, LineStartFixedPitchX.Y, LineStartFixedPitchX.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                                            // <= Move Gantry to Line Begin Point
                                        }

                                        //G3DPoint LineEndFixedPitchX = pattern.Lines[0].EndPoint.RobotLoc;
                                        // => Move Gantry to Line End Point
                                        //System.Diagnostics.Debug.WriteLine(string.Format("Speed to end of blended FixedPSOLine = {0}", pattern.Lines[0].Speed.ToString()));
                                        moveLinear("FixedX-Triggering", ax3, new double[] { LineEndFixedPitchX.X, LineEndFixedPitchX.Y, LineEndFixedPitchX.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                                        // <= Move Gantry to Line End Point
                                    }// if (newLineStartFixedPitchX == null) { } else <==
                                    workPiece.AddTimingElement("MoveGantryToLineEndPoint");
                                    if (axes.NumBlendedMoves > 4)
                                    {
                                        StartBlendedMoves(workPiece, axes);
                                    }
                                    if (pattern.FixedPitchXLast == true)
                                    {
                                        // <= Stop Fix Distance Trigger after last pattern
                                        try
                                        {
                                            if (this._myControler != null)
                                            {
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                                }
                                            }
                                        }// try 1st <==
                                        catch (Exception ex)
                                        {
                                            try
                                            {
                                                U.LogError(ex, "MoveWorkpieceBlended.PSOMODE.OFF First exception.  Will retry");
                                                ClearAllFaults();
                                                lock (axes)
                                                {
                                                    this._myControler.Commands[axesTaskId].Execute("DWELL 0.1");
                                                    this._myControler.Commands[axesTaskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                                }
                                            }// try 2nd <==
                                            catch (Exception e)
                                            {

                                                U.LogAlarmPopup(e, "MoveWorkpieceBlended.PSOMODE.OFF second exception");
                                                return false;
                                            }// catch (Exception e) <==
                                        }// catch (Exception ex) <==
                                        workPiece.AddTimingElement("StopFixDistanceTrigger");
                                        // <= Stop Fix Distance Trigger after last pattern

                                    }// if (pattern.FixedPitchXLast == true) <==
                                }// if (pattern.FixedPitchX == true) <==
                                else
                                {

                                    MovePatternLineByLine(PSOPrimaryAxis, axes, pattern, ref firstMove);
                                    workPiece.AddTimingElement("EndMovePatternLineByLine");

                                }// if (pattern.FixedPitchX == true) { } else <==
                            }// if (PSOPrimaryAxis.PSOEncoders.Length == 1) <==
                            else if (PSOPrimaryAxis.PSOEncoders.Length == 2)
                            {
                                PSODualPrimaryAxis = PSOPrimaryAxis;
                                MovePatternLineByLine(PSOPrimaryAxis, axes, pattern, ref firstMove);

                            }// else if (PSOPrimaryAxis.PSOEncoders.Length == 2) <==

                        } // if (PSOPrimaryAxis != null) <==
                        else
                        {
                            // => Move Gantry to Line Start Point
                            G3DPoint startPt = pattern.Lines[0].RobotLoc;
                            G3DPoint endPt = pattern.Lines[pattern.Lines.Length-1].EndPoint.RobotLoc;
                            double speed = workPiece.XYTransferSpeed;
                            if (firstMove)
                            {
                                moveLinear("NonTrigger SafeUp", ax3, new double[] { ax3[0].CurrentPosition, ax3[1].CurrentPosition, workPiece.ZSafeTransfer }, workPiece.ZTransferSpeed, axesTaskId, false);
                                moveLinear("NonTrigger Safe XY", ax3, new double[] { startPt.X, startPt.Y, workPiece.ZSafeTransfer }, workPiece.XYTransferSpeed, axesTaskId, false);
                                speed = workPiece.ZTransferSpeed;
                                firstMove = false;
                            }
                            moveLinear("NonTrigger-ToStart", ax3, new double[] { startPt.X, startPt.Y, startPt.Z }, speed, axesTaskId, false);
                            // <= Move Gantry to Line Start Point
                            foreach (GTriggerPoint tpt in triggers)
                            {
                                G3DPoint tptLoc = tpt.RobotLoc;
                                moveLinear("MoveToSimTrigger", ax3, new double[] { tptLoc.X, tptLoc.Y, tptLoc.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                                WaitAllBlendedMovesDone(workPiece, "WaitMoveToSimTrigger", axes, tptLoc.Z);
                                // Temp until we find a way to quicken ZHt response time
                                _pollingDone.Reset();
                                U.SleepWithEvents(15);
                                _pollingDone.WaitOne(100);
                                if (IsSimTrigger(tpt.TriggerID))
                                {
                                    tpt.SimulateTrigger();
                                }
                                else
                                {
                                    tpt.DoStationaryAction();
                                }
                            }

                            // => Move Gantry to Line End Point
                            moveLinear("NonTrigger-ToEnd", ax3, new double[] { endPt.X, endPt.Y, endPt.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                            // <= Move Gantry to Line End Point
                        }// if (PSOPrimaryAxis != null) {} else <==
                    } // if (triggers.Length > 0) <==
                    else
                    {
                        // => Move Gantry to Line Start Point
                        moveLinear("NonTrigger-ToStart", ax3, new double[] { pattern.Lines[0].RobotLoc.X, pattern.Lines[0].RobotLoc.Y, pattern.Lines[0].RobotLoc.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                        // <= Move Gantry to Line Start Point

                        // => Move Gantry to Line End Point
                        moveLinear("NonTrigger-ToEnd", ax3, new double[] { pattern.Lines[0].EndPoint.RobotLoc.X, pattern.Lines[0].EndPoint.RobotLoc.Y, pattern.Lines[0].EndPoint.RobotLoc.Z }, pattern.Lines[0].Speed, axesTaskId, false);
                        // <= Move Gantry to Line End Point
                    } // if (triggers.Length > 0) {} else <==
                }// foreach (GPattern pattern in patListRun) <==

                // => Wait Motion Complete
                WaitAllBlendedMovesDone(workPiece, "WaitForAnyFinalBlendedMoves", axes, axes.LastBlendedPt.Z);

                TurnOffPSO(workPiece, axes, PSOPrimaryAxis);
                workPiece.AddTimingElement("PSO Off");

                workPiece.WaitForComplete();

                if (this._myControler != null)
                {
                    // <= Stop Blended Moves
                    lock (axes)
                    {
                        if (RoundedCornerEnable)
                        {
                            this._myControler.Commands[axesTaskId].Execute("ROUNDING OFF");
                        }
                        this._myControler.Commands[axesTaskId].Execute("VELOCITY OFF");
                        this._myControler.Tasks[axesTaskId].Program.Start();
                    }
                    do
                    {
                        _pollingDone.Reset();
                        _pollingDone.WaitOne(100);
                    } while (!axes.MoveDone || !axes.QueueEmpty);
                    lock (axes)
                    {
                        this._myControler.Tasks[axesTaskId].Program.Stop();
                    }
                }
                #region Temp for testing duty cycle
                //if (axes.AxisX.Name == "QQQ")
                //{
                //    TaskId myTask = axesTaskId;
                //    int[] axisNo = new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo };

                //    double x = axes.AxisX.CurrentPosition;
                //    double y = axes.AxisY.CurrentPosition;
                //    double z = axes.AxisZ.CurrentPosition;
                //    lock (axes)
                //    {
                //        //this._myControler.Commands[myTask].Execute("LOOKAHEAD FAST"); 
                //        this._myControler.Commands[myTask].Execute("VELOCITY ON");
                //        // => Setup Rounded Corners
                //        string sRoundCornerCmd = "ROUNDING AXES " + axes.AxisX.Name + " " + axes.AxisY.Name;
                //        this._myControler.Commands[myTask].Execute(sRoundCornerCmd);

                //        sRoundCornerCmd = "ROUNDING TOLERANCE " + RoundedCornerTolerance.Val.ToString();
                //        this._myControler.Commands[myTask].Execute(sRoundCornerCmd);

                //        this._myControler.Commands[myTask].Execute("ROUNDING ON");
                //        //this._myControler.Tasks[myTask].ExecutionMode = TaskExecutionMode.RunOver;
                //        //this._myControler.Commands[myTask].Execute("DWELL 4");
                //        //this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, axisNo);
                //    }
                //    //do
                //    //{
                //    //    _pollingDone.Reset();
                //    //    _pollingDone.WaitOne(100);
                //    //} while (!axes.MoveDone || !axes.QueueEmpty);
                //    long sw0, sw1, sw2, sw3, sw4=0;


                //    for (int i = 0; i < 4; i++)
                //    {
                //        sw0 = U.DateTimeNow;
                //        lock (axes)
                //        {
                //            this._myControler.Tasks[myTask].Program.Debug.Pause();
                //            this._myControler.Commands[myTask].Motion.Linear(axisNo, new double[] { x - 5, y + 0, z }, 20);
                //            this._myControler.Tasks[myTask].Program.Debug.Pause();
                //            this._myControler.Commands[myTask].Motion.Linear(axisNo, new double[] { x - 5, y - 1.5, z }, 50);
                //            this._myControler.Tasks[myTask].Program.Start();
                //            sw1 = U.DateTimeNow;
                //            //do
                //            //{
                //            //    _pollingDone.Reset();
                //            //    _pollingDone.WaitOne(100);
                //            //} while (!axes.MoveDone);
                //        }
                //        U.SleepWithEvents(100);
                //        lock (axes)
                //        {
                //            this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, axisNo);
                //        }
                //        //do
                //        //{
                //        //    _pollingDone.Reset();
                //        //    _pollingDone.WaitOne(100);
                //        //} while (!axes.MoveDone || !axes.QueueEmpty);
                //        sw2 = U.DateTimeNow;
                //        //U.SleepWithEvents(400);
                //        //WaitAllBlendedMovesDone(workPiece, "Testing 1", axes, PSOPrimaryAxis, false);
                //        lock (axes)
                //        {
                //            this._myControler.Tasks[myTask].Program.Debug.Pause();
                //            this._myControler.Commands[myTask].Motion.Linear(axisNo, new double[] { x + 0, y - 1.5, z }, 20);
                //            this._myControler.Tasks[myTask].Program.Debug.Pause();
                //            this._myControler.Commands[myTask].Motion.Linear(axisNo, new double[] { x + 0, y + 0, z }, 50);
                //            this._myControler.Tasks[myTask].Program.Start();
                //            sw3 = U.DateTimeNow;
                //            //do
                //            //{
                //            //    _pollingDone.Reset();
                //            //    _pollingDone.WaitOne(100);
                //            //} while (!axes.MoveDone);
                //            //this._myControler.Tasks[myTask].Program.Debug.Pause();
                //            //this._myControler.Commands[myTask].Execute("DWELL 0.02");
                //            //this._myControler.Tasks[myTask].Program.Start();
                //        }
                //        U.SleepWithEvents(100);
                //        lock (axes)
                //        {
                //            this._myControler.Commands[myTask].Motion.WaitForMotionDone(WaitOption.MoveDone, axisNo);
                //        }
                //        //do
                //        //{
                //        //    _pollingDone.Reset();
                //        //    _pollingDone.WaitOne(100);
                //        //} while (!axes.MoveDone || !axes.QueueEmpty);
                //        sw4 = U.DateTimeNow;
                //        //WaitAllBlendedMovesDone(workPiece, "Testing 2", axes, PSOPrimaryAxis, false);
                //        Debug.WriteLine(string.Format("Moves1={0}, waitForDone={1}, Moves2={2}, waitForDone={3}",
                //            U.TicksToMS(sw1 - sw0),
                //            U.TicksToMS(sw2 - sw1),
                //            U.TicksToMS(sw3 - sw2),
                //            U.TicksToMS(sw4 - sw3)));

                //    }
                //    lock (axes)
                //    {
                //        this._myControler.Commands[myTask].Execute("ROUNDING OFF");
                //        this._myControler.Commands[myTask].Execute("VELOCITY OFF");
                //        this._myControler.Tasks[myTask].Program.Start();
                //        do
                //        {
                //            _pollingDone.Reset();
                //            _pollingDone.WaitOne(100);
                //        } while (!axes.MoveDone || !axes.QueueEmpty);
                //        this._myControler.Tasks[myTask].Program.Stop();
                //    }
                //}
                #endregion Temp for testing duty cycle

            }
            catch (Exception ex)
            {
                TestAbort = false;
                U.LogAlarmPopup(ex, "MoveWorkpieceBlended: Exception");
                workPiece.AbortReason = "Program Halted";
                ClearAllFaults();
                RealAxis[] currentAxis = axes.FilterByType<RealAxis>();
                foreach (RealAxis axis in currentAxis)
                {
                    if (axis.Enabled)
                    {
                        EnableAxis(axis, true);
                    }
                }
                return false;
            }
            LogAeroBasicScript(string.Format("; End of StartWorkpieceBlended for {0}", workPiece.CurrentOperation));
            return !workPiece.AbortRun;
        }
        private void UpdateAxisCounter(RealAxis psoAxis)
        {
            CounterInput[] currentCounter = psoAxis.FilterByType<CounterInput>();
            if (currentCounter != null && currentCounter.Length > 0)
            {
                currentCounter[0].Value += psoAxis.PSOCount;
                psoAxis.PSOCount = 0;
            }
        }



        private bool StartBlendedMoves(GWorkPiece workPiece, RealAxes axes)
        {
            if (axes.StartTime == 0)
            {
                TaskId taskId = getTaskID(axes.AxesNo);
                try
                {
                    if (this._myControler != null)
                    {
                        lock (axes)
                        {
                            this._myControler.Tasks[taskId].Program.Start();
                        }
                    }
                    axes.StartTime = U.DateTimeNow;
                    workPiece.AddTimingElement("#Program.Start");
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Error starting program on {0}({1})", axes.Name, taskId);
                    return false;
                }

            }
            return true;
        }

        private void WaitAllBlendedMovesDone(GWorkPiece workPiece, string name, RealAxes axes, double Z)
        {
            // 
            long ms = 0;
            StartBlendedMoves(workPiece, axes);

            //if (axes.BlendedSleepTime != 0 && updateCounter && PSOPrimaryAxis != null)
            //{
            //    // Move with enough time that all trigger are expended with first move.
            //    ms = U.SleepWithEvents(axes.BlendedSleepTime);
            //    workPiece.AddTimingElement("SleepUntilUpdateCounter", ms.ToString());
            //    int counter = UpdateCounter(PSOPrimaryAxis, string.Empty);
            //    workPiece.AddTimingElement("UpdateCounter", counter.ToString());
            //}
            //Debug.WriteLine(string.Format("{0} - Details={1}", name, axes.BlendedMoveDetails));

            ms = axes.SleepUntilRemaining(30);
            workPiece.AddTimingElement("SleepUntilMoveEnd", ms.ToString());
            TaskId taskId = getTaskID(axes.AxesNo);
            try
            {
                lock (axes)
                {
                    if (this._myControler != null)
                    {
                        this._myControler.Commands[taskId].Motion.WaitForMotionDone(WaitOption.MoveDone, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo }, 10000);
                    }
                    LogAeroBasicScript(string.Format("WAIT MOVEDONE {0} {1} {2}", axes.AxisX.Name, axes.AxisY.Name, axes.AxisZ.Name));
                    //this._myControler.Commands[taskId].Motion.MoveAbs(axes.AxisZ.AxisNo, Z, 200.0);
                    ////if (Math.Abs(axes.AxisZ.CurrentPosition - Z) > 0.5)
                    //{
                    //    this._myControler.Commands[taskId].Execute("DWELL 0.05");
                    //    //Debug.WriteLine(string.Format("MovAbs Z = {0}, Dwell 0.05", Z));
                    //}
                    ////else
                    ////{
                    ////    Debug.WriteLine(string.Format("MovAbs Z = {0}", Z));
                    ////}
                    //this._myControler.Commands[taskId].Motion.WaitForMotionDone(WaitOption.MoveDone, axes.AxisZ.AxisNo);
                    //this._myControler.Commands[taskId].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                }
                //bool bPrevInPos = false;
                //bool bThisInPos = false;
                //do
                //{
                //_pollingDone.Reset();
                
                //    bThisInPos = axes.AxisX.InPosition && axes.AxisY.InPosition && axes.AxisZ.InPosition;
                //    if (bThisInPos) // && bPrevInPos)
                //    {
                //        break;
                //    }
                //    bPrevInPos = bThisInPos;
                     
                //}  while (true);
            }// try 1st <==
            catch (Exception ex)
            {
                try
                {
                    U.LogError(ex, "WaitBlendedMoveDone 1st exception");
                    ClearAllFaults();
                    lock (axes)
                    {
                        this._myControler.Commands[taskId].Execute("DWELL 0.1");

                        // => Wait Motion Complete
                        this._myControler.Commands[taskId].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                        // <= Wait Motion Complete
                    }
                }// try 2nd <==
                catch (Exception e)
                {
                    U.LogAlarmPopup(e, "WaitBlendedMoveDone 2nd exception");
                }// catch (Exception e) <==
            }// catch (Exception ex) <==
            workPiece.AddTimingElement(name, axes.BlendedMoveDetails);
            //_pollingDone.WaitOne(100);
            //Debug.WriteLine(string.Format("Wait For MoveDone complete : {0}", axes.BlendedMoveDetails));
            axes.ResetBlendedMove(false);
            if (axes.CamTableNum >= 0 && this._myControler != null)
            {
                this._myControler.Commands[taskId].Camming.FreeCamTable(axes.CamTableNum);
            }
        //    while (!axes.QueueEmpty)
        //    {
        //        _pollingDone.Reset();
        //        _pollingDone.WaitOne(100);
        //    }                 //bool bPrevInPos = false;
        //    workPiece.AddTimingElement("Wait for QueueEmpty");
        }
       
        #endregion Private

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AerotechV_XX()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public AerotechV_XX(string name)
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

            try
            {                
                // Establish Communication with Aerotech Controller
                ConnectControler();

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
        /// Set the anaolog output
        /// </summary>
        /// <param name="dblOutput"></param>
        /// <param name="value"></param>
        public override void Set(DoubleOutput dblOutput, double value)
        {
            try
            {

                // => Set Output for Normal
                try
                {
                    lock (_lockCommand)
                    {
                        if (this._myControler != null)
                        {
                            this._myControler.Commands[BackgroundTask].IO.AnalogOutput(dblOutput.Channel, 0, value);
                        }
                    }
                }// try 1st <==
                catch (Exception ex)
                {
                    try
                    {
                        U.LogError(ex, "Set.Set DblOutput 1st exception");
                        ClearAllFaults();
                        lock (_lockCommand)
                        {
                            this._myControler.Commands[BackgroundTask].Execute("DWELL 0.1");

                            if (this._myControler != null)
                            {
                                this._myControler.Commands[BackgroundTask].IO.AnalogOutput(dblOutput.Channel, 0, value);
                            }
                        }
                    }// try 2nd <==
                    catch (Exception e)
                    {
                        U.LogAlarmPopup(e, "Set.Set DblOutput 2nd exception");
                        return;
                    }// catch (Exception e) <==
                }// catch (Exception ex) <==
                // <= Set Output Normal
                
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error setting Modbus output. dblOutput={0}; Value={1}", dblOutput.Name, value.ToString());
            }
        }

        /// <summary>
        /// Set the Digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {
            try
            {
                //RealAxes axes = boolOutput.GetParent<RealAxes>();
                if (boolOutput.OutputType == BoolOutput.eType.Normal)
                {
                    if (this._myControler != null)
                    {                       
                        string exString = string.Empty;

                        if (boolOutput.Parent.Name.Contains("Wago"))
                        {
                            string strInputName = boolOutput.Name.Replace(" ", "");
                            exString = string.Format("${0}={1}", strInputName, value);
                        }
                        else
                        {

                            //$DO[6].X = 1
                            RealAxis ax = boolOutput.GetParent<RealAxis>();
                            if (ax != null)
                            {
                                exString = string.Format("$DO[{0}].{1}={2}", boolOutput.Channel, ax.Name, value);
                            }
                        }


                        if (!String.IsNullOrEmpty(exString))
                        {
                            // => Set Output for Normal
                            try
                            {
                                lock (_lockCommand)
                                {
                                    this._myControler.Commands[BackgroundTask].Execute(exString);
                                }
                            }// try 1st <==
                            catch (Exception ex)
                            {
                                try
                                {
                                    U.LogError(ex, "Set.Set Output Normal 1st exception");
                                    ClearAllFaults();
                                    lock (_lockCommand)
                                    {
                                        this._myControler.Commands[BackgroundTask].Execute("DWELL 0.1");

                                        this._myControler.Commands[BackgroundTask].Execute(exString);
                                    }
                                }// try 2nd <==
                                catch (Exception e)
                                {
                                    U.LogAlarmPopup(e, "Set.Set Output Normal 2nd exception");
                                    return;
                                }// catch (Exception e) <==
                            }// catch (Exception ex) <==
                            // <= Set Output Normal

                            boolOutput.Value = value;
                        }
                    }
                }
                else if (boolOutput.OutputType == BoolOutput.eType.PSOAxes)
                {
                    //fire PSO
                    RealAxes axes = boolOutput.GetParent<RealAxes>();
                    bool pulseSetup = false;
                    RealAxis PSOPrimaryAxis = setupPSOType(axes, boolOutput.ChannelID, out pulseSetup);
                    TaskId taskId = getTaskID(axes.AxesNo);
                    // => Set Output for PSO
                    try
                    {
                        lock (axes)
                        {
                            // => Manual PSO Hardware Firing
                            this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                            LogAeroBasicScript(string.Format("PSOCONTROL {0} FIRE", PSOPrimaryAxis.Name));
                            //Debug.WriteLine(string.Format("{0}-Fire", PSOPrimaryAxis.Name));
                            // <= Manual PSO Hardware Firing
                        }
                    }// try 1st <==
                    catch (Exception ex)
                    {
                        try
                        {
                            U.LogError(ex, "Set.Set Output PSO 1st exception");
                            ClearAllFaults();
                            lock (axes)
                            {
                                //this._myControler.Commands[taskId].Execute("DWELL 0.1");

                                // => Manual PSO Hardware Firing
                                this._myControler.Commands[taskId].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                                // <= Manual PSO Hardware Firing
                            }
                        }// try 2nd <==
                        catch (Exception e)
                        {
                            U.LogAlarmPopup(e, "Set.Set Output PSO 2nd exception");
                            return;
                        }// catch (Exception e) <==
                    }// catch (Exception ex) <==
                    // <= Set Output PSO
                   
                    boolOutput.Value = value;
                }
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error setting Modbus output. boolOutput={0}; Value={1}", boolOutput.Name, value.ToString());
            }
        }

        /// <summary>
        /// Set a Pulse with Duration in ms
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value">Which direction to pulse</param>
        /// <param name="duration">Length of the Pulse</param>
        public override void SetPulse(BoolOutput boolOutput, bool value, Miliseconds duration)
        {
            // Aerotech HW Implementation
            try
            {
                 if (this._myControler != null)
                 {
                     if (boolOutput.OutputType == BoolOutput.eType.Normal)
                     {
                        string strInputName = boolOutput.Name.Replace(" ", "");
                        string exString = string.Format("${0}={1}", strInputName, value);

                        // => Pulse Start for Normal
                        try
                        {
                            lock (_lockCommand)
                            {
                                this._myControler.Commands[BackgroundTask].Execute(exString);
                            }
                            LogAeroBasicScript(exString);
                        }// try 1st <==
                        catch (Exception ex)
                        {
                            try
                            {
                                U.LogError(ex, "SetPulse.Pulse Start for Normal 1st exception");
                                ClearAllFaults();
                                lock (_lockCommand)
                                {
                                    this._myControler.Commands[BackgroundTask].Execute("DWELL 0.1");

                                    this._myControler.Commands[BackgroundTask].Execute(exString);
                                }
                            }// try 2nd <==
                            catch (Exception e)
                            {
                                U.LogAlarmPopup(e, "SetPulse.Pulse Start for Normal 2nd exception");
                                return;
                            }// catch (Exception e) <==
                        }// catch (Exception ex) <==
                        // <= Pulse Start for Normal
                        
                        U.SleepWithEvents(duration.ToInt);

                        // => Pulse Stop for Normal
                         exString = string.Format("${0}={1}", strInputName, !value);
                        try
                        {
                            lock (_lockCommand)
                            {
                                this._myControler.Commands[BackgroundTask].Execute(exString);
                            }
                        }// try 1st <==
                        catch (Exception ex)
                        {
                            try
                            {
                                U.LogError(ex, "SetPulse.Pulse Stop  for Normal 1st exception");
                                ClearAllFaults();
                                lock (_lockCommand)
                                {
                                    this._myControler.Commands[BackgroundTask].Execute("DWELL 0.1");

                                    this._myControler.Commands[BackgroundTask].Execute(exString);
                                }
                            }// try 2nd <==
                            catch (Exception e)
                            {
                                U.LogAlarmPopup(e, "SetPulse.Pulse Stop for Normal 2nd exception");
                                return;
                            }// catch (Exception e) <==
                        }// catch (Exception ex) <==
                        // <= Pulse Stop for Normal

                        boolOutput.Value = !value;
                     
                     }//if (boolOutput.OutputType == BoolOutput.eType.Normal) <==
                     else if (boolOutput.OutputType == BoolOutput.eType.PSOAxes)
                     {
                         // Set PSO Pulse
                         RealAxes axes = boolOutput.GetParent<RealAxes>();
                         bool pulseSetup = false;
                         RealAxis PSOPrimaryAxis = setupPSOType(axes, boolOutput.ChannelID, out pulseSetup);
                         if (!pulseSetup)
                         {
                             // => Manual PSO Pulse Start for PSOAXES
                             try
                             {
                                 lock (axes)
                                 {
                                     // => Manual PSO Pulse Start
                                     if (value)
                                     {
                                         this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.On);
                                         LogAeroBasicScript(string.Format("PSOCONTROL {0} ON", PSOPrimaryAxis.Name));
                                     }
                                     else
                                     {
                                         // => Temporary for inverse logic
                                         // => Setup Pulse Train Duration
                                         double singlePulseDuration = PSOPulseOnOffTime / 1000;
                                         if (singlePulseDuration > duration)
                                         {
                                             this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.PulseCyclesOrDelayCyclesOnly(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnOffTime / 2, 1);
                                         }
                                         else
                                         {
                                             this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.PulseCyclesOrDelayCyclesOnly(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnOffTime / 2, duration / singlePulseDuration);
                                         }
                                         // <= Setup Pulse Train Duration

                                         // => Manual PSO Hardware Firing
                                         this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                                         // <= Manual PSO Hardware Firing
                                         // <= Temporary for inverse logic

                                         // => Original
                                         //this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                         // <= Original
                                     }// if (value) {} else <==
                                     // <= Manual PSO Pulse Start  for PSOAXES
                                 }
                             }// try 1st <==
                             catch (Exception ex)
                             {
                                 try
                                 {
                                     U.LogError(ex, "SetPulse.Pulse Start  for PSOAXES 1st exception");
                                     ClearAllFaults();
                                     lock (axes)
                                     {
                                         this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                         // => Manual PSO Pulse Start
                                         if (value)
                                         {
                                             this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.On);
                                         }
                                         else
                                         {
                                             // => Temporary for inverse logic
                                             // => Setup Pulse Train Duration
                                             double singlePulseDuration = PSOPulseOnOffTime / 1000;
                                             if (singlePulseDuration > duration)
                                             {
                                                 this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.PulseCyclesOrDelayCyclesOnly(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnOffTime / 2, 1);
                                             }
                                             else
                                             {
                                                 this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.PulseCyclesOrDelayCyclesOnly(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, PSOPulseOnOffTime / 2, duration / singlePulseDuration);
                                             }
                                             // <= Setup Pulse Train Duration

                                             // => Manual PSO Hardware Firing
                                             this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Fire);
                                             // <= Manual PSO Hardware Firing
                                             // <= Temporary for inverse logic

                                             // => Original
                                             //this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                             // <= Original
                                         }
                                         // <= Manual PSO Pulse Start
                                     }
                                 }// try 2nd <==
                                 catch (Exception e)
                                 {
                                     U.LogAlarmPopup(e, "SetPulse.Pulse Start  for PSOAXES 2nd exception");
                                     return;
                                 }// catch (Exception e) <==
                             }// catch (Exception ex) <==
                             // <= Manual PSO Pulse Start  for PSOAXES
                         }
                         U.SleepWithEvents(duration.ToInt);

                         // => Manual PSO Pulse Stop  for PSOAXES
                         try
                         {
                             lock (axes)
                             {
                                 // => Manual PSO Pulse Stop
                                 if (value)
                                 {
                                     this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                 }
                                 else
                                 {
                                     this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                 }
                                 // <= Manual PSO Pulse Stop
                             }
                         }// try 1st <==
                         catch (Exception ex)
                         {
                             try
                             {
                                 U.LogError(ex, "SetPulse.Pulse Stop for PSOAXES 1st exception");
                                 ClearAllFaults();
                                 lock (axes)
                                 {
                                     this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                     // => Manual PSO Pulse Stop
                                     if (value)
                                     {
                                         this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                     }
                                     else
                                     {
                                         this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Control(PSOPrimaryAxis.AxisNo, PsoMode.Off);
                                     }
                                     // <= Manual PSO Pulse Stop   
                                 }
                             }// try 2nd <==
                             catch (Exception e)
                             {
                                 U.LogAlarmPopup(e, "SetPulse.Pulse Stop for PSOAXES 2nd exception");
                                 return;
                             }// catch (Exception e) <==
                         }// catch (Exception ex) <==
                         // <= Manual PSO Pulse Stop for PSOAXES
                         
                         boolOutput.Value = !value;
                     }//else if (boolOutput.OutputType == BoolOutput.eType.PSOAxes) <==
                }//if (this._myControler != null) <==
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error SetPulse output. boolOutput={0}; Value={1}; Duration={2}", boolOutput.Name, value.ToString(), duration.ToString());
            }
        }

        /// <summary>
        /// Enable an Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public override void EnableAxis(RealAxis axis, bool bEnable)
        {
            // Aerotech implementation call for enable axis
            if (this._myControler != null)
            {
                try
                {
                    AxesBase axes = axis.Parent as AxesBase;
                    lock (_lockCommand)
                    {
                        if (bEnable == true)
                        {
                            this._myControler.Commands.Axes[axis.AxisNo].Motion.Enable();
                            U.LogInfo("EnableAxis {0} TRUE Complete", axis.Name);
                        }
                        else
                        {
                            this._myControler.Commands.Axes[axis.AxisNo].Motion.Disable();
                            U.LogInfo("EnableAxis {0} FALSE Complete", axis.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    U.LogAlarmPopup(ex, "Error EnableAxis {0}.", axis.Name);
                }
            }
            else
            {
                axis.Enabled = true;
            }
        }

        /// <summary>
        /// Clear All Faults from Aerotech Controler
        /// </summary>
        public override void ClearAllFaults()
        {
            try
            {
                if (_myControler != null)
                {
                    lock (_lockCommand)
                    {
                        // Acknowledge All Faults.  
                        this._myControler.Commands.AcknowledgeAll();
                    }
                    U.LogInfo("Clear All Fault: Aerotech");
                }
                else
                {
                    U.LogInfo("Clear All Fault: _myController == null");
                }
                RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
                foreach (RealAxis axis in axisList)
                {
                    axis.FaultDescription = "None";
                }

            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error ClearAllFaults: Aerotech");
            }
        }


        /// <summary>
        /// Abort Motion of all Axes
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public override void Abort()
        {
            // Aerotech implementation call for abort axis
            try
            {
                // Abort motion of all axes
                RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
                foreach (RealAxis axis in axisList)
                {
                    lock (_lockCommand)
                    {
                        this._myControler.Commands.Axes[axis.AxisNo].Motion.Abort();
                    }
                }
                U.LogInfo("Abort Motion of all Axes Complete");
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error Abort Motion of all Axes ");
            }
        }

        /// <summary>
        /// Stop Axis if moving
        /// </summary>
        /// <param name="axis"></param>
        public override void StopAxis(RealAxis axis)
        {
            AxesBase axes = axis.Parent as AxesBase;
            lock (axes)
            {                
                this._myControler.Commands.Motion.Abort(axis.AxisNo);
                this._myControler.Commands.Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
            }
            axis.TargetMotorCounts = axis.CurrentMotorCounts;
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public override void HomeAxis(RealAxis axis)
        {
            if (this._myControler != null)
            {
                try
                {
                    axis.FaultDescription = "None";
                    AxesBase axes = axis.Parent as AxesBase;
                    U.LogInfo("HomeAxis {0} Begin", axis.Name);
                    lock (axes)
                    {
                        // Aerotech implementation call for home axis
                        this._myControler.Commands.Motion.Abort(axis.AxisNo);
                        this._myControler.Commands.Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                    }
                    U.SleepWithEvents(100);
                    lock (axes)
                    {
                        this._myControler.Commands.Motion.Advanced.HomeAsync(axis.AxisNo);
                    }
                    U.SleepWithEvents(150);
                    axis.WaitMoveDone.Reset();
                    U.BlockOrDoEvents(axis.WaitMoveDone, 20000);
                    if (!string.IsNullOrEmpty(axis.FaultDescription) && axis.FaultDescription != "None")
                    {
                        throw new Exception(axis.FaultDescription);
                    }
                    axis.Homed = true;
                    U.LogInfo("HomeAxis {0} Complete", axis.Name);
                }
                catch (Exception ex)
                {
                    U.LogAlarmPopup(ex, "Error HomeAxis {0}.", axis.Name);
                }
            }
            else
            {
                axis.Homed = true;
            }
        }

        /// <summary>
        /// Move Axis to absolute position
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        /// <param name="waitForCompletion"></param>
        public override void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            if (this._myControler != null)
            {
                try
                {
                    Millimeters mmStart = axis.CurrentPosition;
                    RealAxes axes = axis.GetParent<RealAxes>();

                    // => Move Abs Axis
                    try
                    {
                        lock (axes)
                        {
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                            //System.Diagnostics.Debug.WriteLine(string.Format("MoveAbsAxis axes {0} axis {1}", axes.Name, axis.Name));
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.MoveAbs(axis.AxisNo, axis.TargetPosition.Val, speed.Val);
                        }
                    }// try 1st <==
                    catch (Exception ex)
                    {
                        try
                        {
                            U.LogError(ex, "MoveAbsAxis. 1st exception");
                            ClearAllFaults();
                            lock (axes)
                            {
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, axis.AxisNo);
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.MoveAbs(axis.AxisNo, axis.TargetPosition.Val, speed.Val);
                            }
                        }// try 2nd <==
                        catch (Exception e)
                        {
                            U.LogAlarmPopup(e, "MoveAbsAxis. 2nd exception");
                            return;
                        }// catch (Exception e) <==
                    }// catch (Exception ex) <==
                    // <= Move Abs Axis

                    if (waitForCompletion)
                    {
                        double mmDist = Math.Abs(axis.TargetPosition - mmStart);
                        int sleep = Math.Max(100, (int)Math.Round(750.0 * mmDist / speed));
                        long trueSleep = U.SleepWithEvents(sleep);
                        axis.WaitMoveDone.Reset();
                        U.BlockOrDoEvents(axis.WaitMoveDone, 20000);
                        //U.LogInfo("MoveAbsAxis {0} Complete command={1} start={2} finish={3} sleep={4} trueSleep={5}", 
                        //    axis.Name, axis.TargetPosition.ToString(), mmStart.ToString(), axis.CurrentPosition.ToString(), sleep, trueSleep);
                    }
                }
                catch (Exception ex)
                {
                    U.LogAlarmPopup(ex, "Error MoveAbsAxis {0}.", axis.Name);
                }
            }
            else
            {
                axis.CurrentMotorCounts = axis.TargetMotorCounts;
            }
        }

        /// <summary>
        /// Move Linear on Axis XY
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        public override void MoveLinearXY(RealAxes axes, MLengthSpeed speed)
        {
            if (this._myControler == null)
            {
                return;
            }

            // Convert to mm
            Millimeters TargetPointX = axes.AxisX.TargetPosition;
            Millimeters TargetPointY = axes.AxisY.TargetPosition;

            try
            {
                // => Move Linear XY
                try
                {
                    lock (axes)
                    {
                        //Stopwatch sw = Stopwatch.StartNew();
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo });
                        //axes.MSTimeSpentWaitingForMotionComplete += sw.ElapsedMilliseconds;

                        //sw = Stopwatch.StartNew();
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Linear(new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo }, new double[] { TargetPointX.Val, TargetPointY.Val }, speed.Val);
                        //axes.MSTimeSpentWaitingForLinearMove += sw.ElapsedMilliseconds;
                        U.SleepWithEvents(20);
                        //sw = Stopwatch.StartNew();
                        this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo });
                        //axes.MSTimeSpentWaitingForMotionComplete += sw.ElapsedMilliseconds;
                    }
                }// try 1st <==
                catch (Exception ex)
                {
                    try
                    {
                        U.LogError(ex, "MoveLinearXY. 1st exception");
                        ClearAllFaults();
                        lock (axes)
                        {
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo });
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Linear(new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo }, new double[] { TargetPointX.Val, TargetPointY.Val }, speed.Val);
                            U.SleepWithEvents(50); 
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo });
                        }
                    }// try 2nd <==
                    catch (Exception e)
                    {
                        U.LogAlarmPopup(e, "MoveLinearXY. 2nd exception");
                        return;
                    }// catch (Exception e) <==
                }// catch (Exception ex) <==
                // <= Move Linear XY
                
                U.LogInfo("MoveLinearXY {0}/{1} Complete", axes.AxisX.Name, axes.AxisY.Name);
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error MoveLinearXY {0}/{1}.", axes.AxisX.Name, axes.AxisY.Name);
            }
        }

        /// <summary>
        /// Move all the patterns in the workpiece
        /// </summary>
        /// <param name="workPiece"></param>
        /// <param name="axes"></param>
        /// <param name="patList"></param>
        /// <returns>True if completed with no abort</returns>
        public override bool MoveWorkPiece(GWorkPiece workPiece, RealAxes axes, GPattern[] patList)
        {  
            // Process one pattern at a time (including the link move to get to the pattern)
            // Return when everything completed
            // or return when interrupted (Check GPattern.AbortExecution flag)
            bool firstMove = true;
            Boolean TransfereActive = true;
            RealAxis PSOPrimaryAxis = null;
            axes.CamTableNum = -1;
            bool bRet = true;
            try
            {
                if (workPiece==null || axes.HasChildren==false || patList==null)
                {
                    // Do nothing
                    U.LogInfo("MoveWorkPiece: Missing Parameter on MoveWorkPiece");
                    return true;
                }
                else
                {
                    
                    ResetCounter(axes);
                    workPiece.AddTimingElement("ResetCounter");

                    // => WorkPiece Start Program
                    try
                    {
                        if (this._myControler != null)
                        {
                            lock (axes)
                            {
                                this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                                // Temp Check


                                //loadPSODistancesXAxis(movePattern, PSOPrimaryAxis, new GLine[] { line }, iStartTaskDouble, getTaskID(axes.AxesNo));
                                //movePattern.AddTiming("*loadPSODistancesXAxis");

                                // => Add PSO Trigger to Distance Array
                                //this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Array(PSOPrimaryAxis.AxisNo, sStartPosTask, 0, NewTP.Length);
                                //this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.DistanceArray(PSOPrimaryAxis.AxisNo, 0, NewTP.Length);
                                // <= Add PSO Trigger to Distance Array





                                // End Temp Check
                                this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.InitializeQueue();
                                this._myControler.Commands.Tasks.MFO(getTaskID(axes.AxesNo), MFO);
                                //System.Diagnostics.Debug.WriteLine(string.Format("MFO({0}-{1}) ={2}", axes.Name, patList[0].Lines[0].GetType().Name, MFO));
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Setup.Absolute();
                            }
                        }
                    }// try 1st <==
                    catch (Exception ex)
                    {
                        try
                        {
                            U.LogError(ex, "MoveWorkPiece.WorkPiece Start Program 1st exception");
                            ClearAllFaults();
                            lock (axes)
                            {
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                                this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.InitializeQueue();
                                this._myControler.Commands.Tasks.MFO(getTaskID(axes.AxesNo), MFO);
                                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Setup.Absolute();
                            }
                        }// try 2nd <==
                        catch (Exception e)
                        {
                            U.LogAlarmPopup(e, "MoveWorkPiece.WorkPiece Start Program 2nd exception");
                            return false;
                        }// catch (Exception e) <==
                    }// catch (Exception ex) <==
                    // <= WorkPiece Start Program

                    workPiece.AddTimingElement("InitializeQueue");
                    //U.LogInfo("MoveWorkPiece: InitializeQueue on MoveWorkPiece with Axes={0}", axes.Name);
                }

                if (PSOTriggerPathPlanning == 4)
                {
                    // Do Work: Fully Blended move for WorkPiece
                    if (!MoveWorkPieceBlended(workPiece, axes, patList))
                    {
                        bRet = false;
                    }                    
                }// if (PSOTriggerPathPlanning == 4) <==
                else
                {
                    foreach (GPattern pattern in patList)
                    {
                        if (pattern == null)
                        {
                            // Do Nothing
                            U.LogInfo("MoveWorkPiece: pattern==null");
                        }
                        else
                        {

                            if (pattern.HasChildren == false)
                            {
                                // Do Nothing
                                U.LogInfo("MoveWorkPiece: pattern.HasCildren=={0}", pattern.HasChildren.ToString());
                            }
                            else
                            {
                                 if (TransfereActive == true)
                                {
                                    safetyTransferMove(workPiece, axes, pattern);
                                    FireWorkpieceBegin();
                                    TransfereActive = false;
                                }
                                
                                pattern.PrepareForExecute();
                                pattern.WaitForReady();

                                if (workPiece.AbortRun)
                                {
                                    lock (axes)
                                    {
                                        this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("VELOCITY OFF");
                                    }
                                    U.LogInfo("MoveWorkPiece: AbortRun");
                                    return false;
                                }

                                // Any triggers?
                                GTriggerPoint[] triggers = pattern.TriggerPts;
                                if (triggers.Length > 0)
                                {
                                    bool pulseSetup = false;
                                    PSOPrimaryAxis = setupPSOType(axes, triggers[0].TriggerID, out pulseSetup);

                                    if (!pulseSetup && PSOPrimaryAxis != null)
                                    {
                                        // Is PSO fully setup for PSO external triggerring?
                                        bool isPSOSetup = PSOPrimaryAxis.PSOEncoders != null && PSOPrimaryAxis.PSOEncoders.Length > 0;
                                        if (isPSOSetup)
                                        {
                                            double triggerOnTime = 0;
                                            if (triggers[0].PulseWidth.Val > 0)
                                            {
                                                triggerOnTime = triggers[0].PulseWidth.Val;
                                            }
                                            else
                                            {
                                                triggerOnTime = PSOPulseOnTime;
                                            }

                                            // => PSO Setup to Controller
                                            try
                                            {
                                                lock (axes)
                                                {
                                                    // => PSO Setup to Controller
                                                    this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Pulse(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, triggerOnTime);
                                                    //Debug.WriteLine(string.Format("{0}-Pulse({1}, {2}", PSOPrimaryAxis.Name, PSOPulseOnOffTime, triggerOnTime));
                                                    this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.TrackDirection(PSOPrimaryAxis.AxisNo, 0);
                                                    //Debug.WriteLine(string.Format("{0}-TrackDirection(0)", PSOPrimaryAxis.Name));
                                                    // <= PSO Setup to Controller
                                                }
                                            }// try 1st <==
                                            catch (Exception ex)
                                            {
                                                try
                                                {
                                                    U.LogError(ex, "MoveWorkPiece.PSO SETUP TO CONTROLER 1st exception");
                                                    ClearAllFaults();
                                                    lock (axes)
                                                    {
                                                        this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                                        // => PSO Setup to Controller
                                                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.Pulse(PSOPrimaryAxis.AxisNo, PSOPulseOnOffTime, triggerOnTime);
                                                        this._myControler.Commands[getTaskID(axes.AxesNo)].PSO.TrackDirection(PSOPrimaryAxis.AxisNo, 0);
                                                        // <= PSO Setup to Controller
                                                    }
                                                }// try 2nd <==
                                                catch (Exception e)
                                                {
                                                    U.LogAlarmPopup(e, "MoveWorkPiece.PSO SETUP TO CONTROLER 2nd exception");
                                                    return false;
                                                }// catch (Excetpion e) <==
                                            }// catch (Exception ex) <==
                                            // <= PSO Setup to Controller
                                        } // if (PSOPrimaryAxis.PSOEncoders != null && PSOPrimaryAxis.PSOEncoders.Length > 0) <==

                                        // => Pattern Status Update
                                        GTriggerPoint[] NewTP = pattern.TriggerPts;
                                        foreach (GTriggerPoint tp in NewTP)
                                        {
                                            tp.PrepareCallback(isPSOSetup);
                                        }
                                        // <= 
                                    } // if (PSOPrimaryAxis != null) <==
                                } // if (triggers.Length > 0) <==
                                
                                if (PSOTriggerPathPlanning == 0 || PSOPrimaryAxis == null || PSOPrimaryAxis.PSOEncoders == null)
                                {
                                    MovePatternSimulateTrigger(PSOPrimaryAxis, axes, pattern);
                                }
                                else
                                {
                                    
                                    switch (PSOTriggerPathPlanning)
                                    {
                                        case 1:
                                            MovePatternPointByPoint(PSOPrimaryAxis, axes, pattern);
                                            break;
                                        case 2:
                                            MovePatternLineByLine(PSOPrimaryAxis, axes, pattern, ref firstMove);
                                            break;
                                        case 3:
                                            if (PSOPrimaryAxis.PSOEncoders.Length == 1)
                                            {
                                                goto case 2;
                                            }
                                            MovePatternBlended(PSOPrimaryAxis, axes, pattern);
                                            break;
                                        default:
                                            U.LogAlarmPopup("MoveWorkPiece: 'PSOTriggerPathPlanning {0}' not Defined", PSOTriggerPathPlanning.ToString());
                                            break;
                                    }
                                }// if (PSOTriggerPathPlanning == 0 || PSOPrimaryAxis == null || PSOPrimaryAxis.PSOEncoders == null) {} else <==

                                // => Wait Motion Complete
                                try
                                {
                                    lock (axes)
                                    {
                                        // => Wait Motion Complete
                                        this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                                        // <= Wait Motion Complete
                                    }
                                }// try 1st <==
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        U.LogError(ex, "MoveWorkPiece.WaitForMotionDone StartDispensing 1st exception");
                                        ClearAllFaults();
                                        lock (axes)
                                        {
                                            this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                                            // => Wait Motion Complete
                                            this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.WaitForMotionDone(WaitOption.InPosition, new int[] { axes.AxisX.AxisNo, axes.AxisY.AxisNo, axes.AxisZ.AxisNo });
                                            // <= Wait Motion Complete
                                        }
                                    }// try 2nd <==
                                    catch (Exception e)
                                    {
                                        U.LogAlarmPopup(e, "MoveWorkPiece.WaitForMotionDone StarDispensing 2nd exception");
                                        return false;
                                    }// catch (Exception e) <==
                                }// catch (Exception ex) <==
                                // <= Wait Motion Complete


                                if (PSOPrimaryAxis != null)
                                {
                                    TurnOffPSO(workPiece, axes, PSOPrimaryAxis);
                                   
                                    // => Setup DAQ for TriggerPoint tracking
                                    if (PSOPrimaryAxis.PSOAxisKey != null && PSOPrimaryAxis.PSOEncoders != null)
                                    {
                                        if (PSOPrimaryAxis.PSOEncoders.Length == 2 && UsePSOCounter)
                                        {
                                            UpdateCounter(null);
                                            U.LogInfo("MoveWorkPiece: Update number trigger on primary PSO axis {0}", PSOPrimaryAxis.Name);
                                        }
                                    }
                                    // <=
                                }// if (PSOPrimaryAxis != null) <==
                            }// if (pattern.HasChildren == false || pattern.Enabled == false){}  else <==
                        }// if (pattern == null){}  else <==
                    }// foreach (GPattern pattern in patList) <==
                }// if (PSOTriggerPathPlanning == 4){} else <==

                // => WorkPiece Complete Stop Program
                try
                {
                    if (this._myControler != null)
                    {
                        lock (axes)
                        {
                            //DATAACQ X 1 OFF
                            if (PSOPrimaryAxis != null)
                            {
                                this._myControler.Commands[getTaskID(axes.AxesNo)].DataAcquisition.Off(PSOPrimaryAxis.AxisNo);
                            }

                            this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                        }
                    }
                }// try 1st <==
                catch (Exception ex)
                {
                    try
                    {
                        U.LogError(ex, "MoveWorkPiece.WorkPiece Complete Stop Program 1st exception");
                        ClearAllFaults();
                        lock (axes)
                        {
                            this._myControler.Commands[getTaskID(axes.AxesNo)].Execute("DWELL 0.1");

                            //DATAACQ X 1 OFF
                            if (PSOPrimaryAxis != null)
                            {
                                this._myControler.Commands[getTaskID(axes.AxesNo)].DataAcquisition.Off(PSOPrimaryAxis.AxisNo);
                            }
                            
                            this._myControler.Tasks[getTaskID(axes.AxesNo)].Program.Stop();
                        }
                    }// try 2nd <==
                    catch (Exception e)
                    {
                        U.LogAlarmPopup(e, "MoveWorkPiece.WorkPiece Complete Stop Program 2nd exception");
                        return false;
                    }// catch (Exception e) <==
                }// catch (Exception ex) <==
                // <= WorkPiece Complete Stop Program
                
                //U.LogInfo("MoveWorkPiece: Complete");
                workPiece.WaitForComplete();
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "MoveWorkPiece: Error MoveWorkpiece");
                return false;
            }

            if (workPiece.AbortRun)
            {
                return false;
            }
            return bRet;
        }
        #endregion Overrides

        #region Public Calls to do service

        /// <summary>
        /// Execute Command
        /// </summary>
        public override void ExecuteCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                string[] commands = Regex.Split(command, "\r\n");
                // Execute Commands Line by Line
                foreach (string s in commands)
                {
                    try
                    {
                        lock (_lockCommand)
                        {
                            // Execute Command.  
                            this._myControler.Commands.Execute(s);
                        }
                        U.LogInfo("Execute Command {0}", s);
                    }
                    catch (Exception ex)
                    {
                        U.LogAlarmPopup(ex, "Error Execute Command {0}", s);
                    }
                }
            }
        }

        /// <summary>
        /// Execute Command on Specific Task
        /// </summary>
        public void CommandExecute(string command, TaskId myTask)
        {
            try
            {
                if (this._myControler != null)
                {
                    lock (_lockCommand)
                    {
                        // Execute command on specific task.  
                        this._myControler.Commands[myTask].Execute(command);                             
                    }

                }
                LogAeroBasicScript(command);
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error Execute Command {0} on Task{1}", command, myTask.ToString());
            }
        }
        /// <summary>
        /// Execute Command on Specific Task
        /// </summary>
        public void ExecuteCommand(string command, int myTask)
        {
            try
            {
                if (command.Contains("Dwell"))
                {
                    string[] commandLine = Regex.Split(command, "ll");
                    int iTime = Convert.ToInt32(Convert.ToDouble(commandLine[1]) * 1000);
                    Thread.Sleep(iTime);
                }
                else
                {
                    lock (_lockCommand)
                    {
                        // Execute command on specific task.  
                        this._myControler.Commands[myTask].Execute(command);
                    }
                    
                }
                U.LogInfo("Execute Command {0} on Task{1}", command, myTask.ToString());
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error Execute Command {0} on Task{1}", command, myTask.ToString());
            }
        }

          /// <summary>
        /// Reset Controler
        /// </summary>
        public void ResetControler()
        {
            try
            {
                
                // Acknowledge all faults
                ClearAllFaults();

                // Reset the A3200 controller.  
                lock (_lockCommand)
                {
                    this._myControler.Reset();
                }

                // Send Initialization Commands
                SendInitialCommands();
                
                U.LogInfo("Reset Controler");
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error ResetControler");
            }
        }
        
        /// <summary>
        /// Connect to Controller
        /// </summary>
        public void ConnectControler()
        {
            // Connect to A3200 controller. 
            do
            {
                try
                {
                    this._myControler = Controller.Connect();
                    break;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show("Could not connect to the Aerotech controller.  Please recycle NPAQ power then click Retry.",
                        "Error", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                    {
                        throw ex;
                    }
                }
            }
            while (true);

            Connected = true;

            // Acknowledge all faults
            ClearAllFaults();

            _customDiagnostics = new CustomDiagnostics(_myControler);

            // => Input Update by Aerotech Custom Diagnostics
            _WagoDigitalInputList = null;
            Inputs wagoInputs = this["Wago Inputs"] as Inputs;
            if (wagoInputs != null)
            {
                _WagoDigitalInputList = wagoInputs.FilterByType<BoolInput>();
            }
                
            //// => Word wise Input update
            if (!string.IsNullOrEmpty(InitializeDiagInputName))
            {
                string[] commands = Regex.Split(InitializeDiagInputName, "\r\n");
   
                // Add Int32 variable names to Diagnostic List
                foreach (string s in commands)
                {
                    _customDiagnostics.Configuration.Variable.Add(s);
                }
            }
            //// <= Word wise Input update

            //// => Analog Axis Input Update
            RealAxis[] axisList = RecursiveFilterByType<RealAxis>();

            foreach (RealAxis axis in axisList)
            {
                MDoubleInput[] analogInputs = axis.RecursiveFilterByType<MDoubleInput>();
                foreach (MDoubleInput mdInput in analogInputs)
                {
                    switch (mdInput.Channel)
                    {
                        case 0:
                            _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AnalogInput0, axis.AxisNo);
                            break;
                        case 1:
                            _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AnalogInput1, axis.AxisNo);
                            break;
                        case 2:
                            _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AnalogInput2, axis.AxisNo);
                            break;
                        case 3:
                            _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AnalogInput3, axis.AxisNo);
                            break;
                    }
                }// foreach (MDoubleInput mdInput in analogInputs) <==

                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.DigitalInput, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.PositionFeedback, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.VelocityFeedback, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.VelocityCommand, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.DriveStatus, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AxisStatus, axis.AxisNo);
                _customDiagnostics.Configuration.Axis.Add(AxisStatusSignal.AxisFault, axis.AxisNo);
                // DRIVEINFO(" + psoAxis.Name + ", DRIVEINFO_DataAcquisitionSamples
            }// foreach (RealAxis axis in axisList) <==

            RealAxes[] axesList = RecursiveFilterByType<RealAxes>();
            if (axesList != null && axesList.Length > 0)
            {
                foreach (RealAxes ax in axesList)
                {
                    TaskId taskId = getTaskID(ax.AxesNo);
                    _customDiagnostics.Configuration.Task.Add(TaskStatusSignal.QueueStatus, taskId);
                    //_customDiagnostics.Configuration.Task.Add(TaskStatusSignal.TaskState, taskId);
                }
            }
            //// <= Analog Axis Input Update
            // <= Input Update by Aerotech Custom Diagnostics

            // => Initial Controler Setup
            SendInitialCommands();

            RealAxes axes = U.GetComponent("Right XYZ Axes") as RealAxes;
            lock (axes)
            {
                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Setup.Absolute();
            }

            axes = U.GetComponent("Left XXYYZZ Axes") as RealAxes;
            lock (axes)
            {
                this._myControler.Commands[getTaskID(axes.AxesNo)].Motion.Setup.Absolute();
            }
            // <= Initial Controler Setup

            // => Register task state and diagPacket arrived events
            this._myControler.ControlCenter.TaskStates.RefreshInterval = LoopIntervalTask;
            this._myControler.ControlCenter.TaskStates.NewTaskStatesArrived += new EventHandler<NewTaskStatesArrivedEventArgs>(TaskStates_NewTaskStatesArrived);
            // <= Register task state and diagPacket arrived events

            // => Register Background Worker Thread
            _updateLoop = new BackgroundWorker();
            _updateLoop.DoWork += new DoWorkEventHandler(UpdateLoop);
            _updateLoop.RunWorkerAsync();
            // => Register Background Worker Thread
            if (UsePSOCounter)
            {
                _counterLoop = new BackgroundWorker();
                _counterLoop.DoWork += new DoWorkEventHandler(CounterLoop);
                _counterLoop.RunWorkerAsync();
            }
            // <= Register Background worker Thread

            U.LogInfo("Controler Connected");

        }

        /// <summary>
        /// Disconnect Controller
        /// </summary>
        public void DisconnectControler()
        {
            
            // Disable axes
            RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
            foreach (RealAxis axis in axisList)
            {
                if (axis.Dir != RealAxis.eDir.Unknown)
                {
                    this.EnableAxis(axis, false);
                }
            }

            // => Suspend Background Worker Thread
            _updateLoop.DoWork -= new DoWorkEventHandler(UpdateLoop);
            if (_counterLoop != null)
            {
                _counterLoop.DoWork -= new DoWorkEventHandler(CounterLoop);
            }

            // <= Suspend Background Worker Thread

            // => Suspend ControlCenter
            //this._myControler.ControlCenter.Diagnostics.Suspend();
            this._myControler.ControlCenter.TaskStates.Suspend();
            this._myControler.ControlCenter.TaskStates.NewTaskStatesArrived -= new EventHandler<NewTaskStatesArrivedEventArgs>(TaskStates_NewTaskStatesArrived);
            // <= Suspend ControlCenter

            Thread.Sleep(100);

            // Disconnect A3200 controller.
            lock (_lockCommand)
            {
                Controller.Disconnect();
                Connected = false;
                this._myControler = null;
            }           
            U.LogInfo("Controller Disconnected");
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
        /// Destroy
        /// </summary>
        public override void Destroy()
        {
            _destroy = true;
            if (_myControler != null)
            {
                DisconnectControler();
            }
            base.Destroy();
        }

        /// <summary>
        /// Update Global double
        /// </summary>
        public void UpdateGlobalDouble()
        {
            _globalDouble = null;

            try
            {
                // Get Global Doubles from Aerotech Controler
                Aerotech.A3200.Variables.TypedVariable<double>[] GlobalDoubleArray = null;
                lock (_lockCommand)
                {
                    GlobalDoubleArray = _myControler.Variables.Global.Doubles.ToArray();
                }

                int count = GlobalDoubleArray.Length;
                _globalDouble = new double[count];

                // Set Value of Aerotech Global Doubles into private _globalDouble array
                for (int i = 0; i < count; i++)
                {
                    _globalDouble[i] = GlobalDoubleArray[i].Value;
                }

                U.LogInfo("Global Double Updated");
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error updateGlobalDouble");
            }

        }

        /// <summary>
        /// Update Task double
        /// </summary>
        public void UpdateTaskDouble(TaskId MyTask)
        {
            _taskDouble = null;

            try
            {
                // Get Task Doubles from Aerotech Controler
                Aerotech.A3200.Variables.TypedVariable<double>[] TaskDoubleArray = null;
                lock (_lockCommand)
                {
                    TaskDoubleArray = _myControler.Variables.Tasks[MyTask].Doubles.ToArray();
                }

                int count = TaskDoubleArray.Length;
                _taskDouble = new double[count];

                // Set Value of Aerotech Task Doubles into private _taskDouble array
                for (int i = 0; i < count; i++)
                {
                    _taskDouble[i] = TaskDoubleArray[i].Value;
                }
                U.LogInfo("{0} Double Updated", MyTask.ToString());
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error updateTaskDouble(TaskId)");
            }

        }

        /// <summary>
        /// Update Task double
        /// </summary>
        public void UpdateTaskDouble(int MyTask)
        {
            _taskDouble = null;

            try
            {
                // Get Task Doubles from Aerotech Controler
                Aerotech.A3200.Variables.TypedVariable<double>[] TaskDoubleArray = null;
                lock (_lockCommand)
                {
                    TaskDoubleArray = _myControler.Variables.Tasks[MyTask].Doubles.ToArray();
                }

                int count = TaskDoubleArray.Length;
                _taskDouble = new double[count];

                // Set Value of Aerotech Task Doubles into private _taskDouble array
                for (int i = 0; i < count; i++)
                {
                    _taskDouble[i] = TaskDoubleArray[i].Value;
                }

                U.LogInfo("{0} Double Updated", MyTask.ToString());
            }
            catch (Exception ex)
            {
                U.LogAlarmPopup(ex, "Error updateTaskDouble(int)");
            }
        }

        #endregion Public Calls to do service


        #region ControllerEvents

        TaskState[] _taskStates = new TaskState[32];
        string[] _taskErrorDescription = new string[32];
        long _lastTaskStat = 0;
        /// <summary>
        /// Handle task state arrived event. Invoke setTaskState to process data
        /// </summary>
        private void TaskStates_NewTaskStatesArrived(object sender, NewTaskStatesArrivedEventArgs e)
        {
            if (_destroy)
            {
                return;
            }
            long now = U.DateTimeNow;
            double elapsed = U.TicksToMS(now -_lastTaskStat);
            if (_lastTaskStat != 0 && elapsed > LoopIntervalTask * 2)
            {
                U.LogWarning("TaskStates_handler is late at {0} mS", elapsed);
                if (elapsed > TaskStatesTimeout.Val)
                {
                    U.DumpProcessUsage("Late TaskStates_handler");
                }
            }
            _lastTaskStat = now;
            try
            {
                //URL: http://msdn.microsoft.com/en-us/library/ms171728.aspx
                //How to: Make Thread-Safe Calls to Windows Forms Controls
                //this.Invoke(new Action<NewTaskStatesArrivedEventArgs>(setTaskState), e);
                //labelTaskState.Text = e.TaskStates[this.taskIndex].ToString();

                //System.Diagnostics.Debug.WriteLine("Entering TaskStates_NewTaskStatesArrived");
                _taskState = string.Empty;
                for (int i = 1; i < e.TaskStates.Count; i++)
                {
                    if (e.TaskStates[i] != TaskState.Inactive)
                    {
                        _taskState = _taskState + "Task " + Convert.ToString(i) + ": " + e.TaskStates[i].ToString() + "\r\n";
                        if (!_suppressAxisTaskErrorDetection)
                        {
                            if (_taskErrorDescription[i] == null)
                            {
                                _taskErrorDescription[i] = e.Statuses[i].Error.Description;
                                _taskStates[i] = e.TaskStates[i];
                            }
                            if (e.TaskStates[i] != _taskStates[i] && e.TaskStates[i] != TaskState.Idle && e.TaskStates[i] != TaskState.Queue)
                            {
                                U.LogWarning("Task.State changed on Task{0}: '{1}'", i, e.TaskStates[i].ToString());
                                _taskStates[i] = e.TaskStates[i];
                            }
                            if (e.Statuses[i].Error.Description != _taskErrorDescription[i])
                            {
                                U.LogError("Task.Status changed on Task{0}: '{1}'", i, e.Statuses[i].Error.Description);
                                _taskErrorDescription[i] = e.Statuses[i].Error.Description;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Error TaskStates_NewTaskStatesArrived.");
            }
        }


        private Stopwatch _swStart = null;
        private int _count = 0;
        /// <summary>
        /// Internal Update Loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateLoop(object sender, DoWorkEventArgs e)
        {
            do
            {
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    long timeInputUpdate = 0;

                    if (_count == 0)
                    {
                        _swStart = Stopwatch.StartNew();
                    }
                    _count++;
                    if (_count >= 100)
                    {
                        //System.Diagnostics.Debug.WriteLine(string.Format("Polling Time = {0}", _swStart.ElapsedMilliseconds / 100));
                        //U.LogWarning(string.Format("Polling Time = {0}", _swStart.ElapsedMilliseconds / 100));
                        _count = 0;
                    }


                    //long now = U.DateTimeNow;
                    CustomDiagnosticsResults result = _customDiagnostics.Retrieve();
                    //if (_count == 0)
                    //{
                    //    Debug.WriteLine(string.Format("Time to Retrieve={0}", U.TicksToMS(U.DateTimeNow - now)));
                    //}

                    // => Update by Aerotech Custom Diagnostic
                    timeInputUpdate = sw.ElapsedMilliseconds;
                    //// => Word wise Aerotech Diagnostic WAGO Input Update
                    string[] commands = Regex.Split(InitializeDiagInputName, "\r\n");
                    Int32[] IntUpdateArray = new Int32[commands.Length];
                    int i = 0;
                    foreach (string strInt32Input in commands)
                    {
                        IntUpdateArray[i] = result.Variable[strInt32Input].ConvertValueInt32();
                        i++;
                    }

                    BitArray bitArray = new BitArray(IntUpdateArray);
                    if (_WagoDigitalInputList != null)
                    {
                        foreach (BoolInput input in _WagoDigitalInputList)
                        {
                            input.Value = bitArray[input.Channel];
                        }
                    }
                    //// <= Word wise Aerotech Diagnostic WAGO Input Update
                    timeInputUpdate = sw.ElapsedMilliseconds - timeInputUpdate;

                    //// => Analog Axis Input Update
                    RealAxis[] axisList = RecursiveFilterByType<RealAxis>();
                    RealAxes[] axesList = RecursiveFilterByType<RealAxes>();
                    if (axesList != null && axesList.Length > 0)
                    {
                        foreach (RealAxes axes in axesList)
                        {
                            axes.TempMoveDone = true;
                            QueueStatus queueStatus = new QueueStatus(result.Task[TaskStatusSignal.QueueStatus, getTaskID(axes.AxesNo)].ConvertValueInt32());
                            axes.QueueEmpty = queueStatus.QueueBufferEmpty;
                        }
                    }

                    foreach (RealAxis axis in axisList)
                    {
                        RealAxes axes = axis.Parent as RealAxes;
                        MDoubleInput[] analogInputs = axis.RecursiveFilterByType<MDoubleInput>();
                        foreach (MDoubleInput mdInput in analogInputs)
                        {
                            switch (mdInput.Channel)
                            {
                                case 0:
                                    mdInput.ApplyRawValue(result.Axis[AxisStatusSignal.AnalogInput0, axis.AxisNo].Value);
                                    break;
                                case 1:
                                    mdInput.ApplyRawValue(result.Axis[AxisStatusSignal.AnalogInput1, axis.AxisNo].Value);
                                    if (axis.Name == "XXP")
                                    {
                                        _zHt = (mdInput as MillimeterInput).Value;
                                    }
                                    break;
                                case 2:
                                    mdInput.ApplyRawValue(result.Axis[AxisStatusSignal.AnalogInput2, axis.AxisNo].Value);
                                    break;
                                case 3:
                                    mdInput.ApplyRawValue(result.Axis[AxisStatusSignal.AnalogInput3, axis.AxisNo].Value);
                                    break;
                            }
                        }// foreach (MDoubleInput mdInput in analogInputs) <==

                        double curMotorCount = result.Axis[AxisStatusSignal.PositionFeedback, axis.AxisNo].Value * axis.MotorCountsPerMM;
                        double curVel = result.Axis[AxisStatusSignal.VelocityFeedback, axis.AxisNo].Value;
                        axis.CurrentMotorCounts = curMotorCount;
                        axis.CurrentVelocity = curVel;
                        //if (axis.Name == "X")
                        //{
                        //    Debug.WriteLine(string.Format("X.CurPos={0}", axis.CurrentPosition));
                        //}

                        if (!_suppressAxisTaskErrorDetection)
                        {
                            RetrievedAxisSignalEntry retrievedAxisFault = result.Axis[AxisStatusSignal.AxisFault, axis.AxisNo];
                            AxisFault axisFault = retrievedAxisFault.ConvertValueAxisFault();
                            string axisFaultDescription = axisFault.ToString(true);
                            if (string.IsNullOrEmpty(axis.FaultDescription))
                            {
                                axis.FaultDescription = axisFaultDescription;
                            }
                            else if (axisFaultDescription != axis.FaultDescription)
                            {
                                double velCmd = result.Axis[AxisStatusSignal.VelocityCommand, axis.AxisNo].Value;
                                U.LogError("Axis Fault changed ({0}) - '{1}'  VelCmd={2}", axis.Name, axisFaultDescription, velCmd);
                                axis.FaultDescription = axisFaultDescription;
                            }
                        }
                        RetrievedAxisSignalEntry retrievedAxis = result.Axis[AxisStatusSignal.AxisStatus, axis.AxisNo];
                        AxisStatus axisStatus = retrievedAxis.ConvertValueAxisStatus();
                        
                        bool moveDone = axisStatus.MoveDone;
                        axis.MoveDone = moveDone;
                        //// Need to double check "&& !e.Data[axis.AxisNo].DriveStatus.MoveActive"
                        if (moveDone)
                        {
                            // Let it continue
                            axis.WaitMoveDone.Set();
                        }
                        else if (axes != null)
                        {
                            axes.TempMoveDone = false;
                        }

                        int iAxisDigitalInput = result.Axis[AxisStatusSignal.DigitalInput, axis.AxisNo].ConvertValueInt32();
                        UpdateAxisInput(axis, iAxisDigitalInput);
                            
                    }// foreach (RealAxis axis in axisList) <==

                    if (axesList != null && axesList.Length > 0)
                    {
                        foreach (RealAxes axes in axesList)
                        {
                            axes.MoveDone = axes.TempMoveDone;
                        }
                    }
                    //// <= Analog Axis Input Update
                    // <= Update by Aerotech Custom Diagnostic

                    FirePollingComplete();
                    _pollingDone.Set();
                    Thread.Sleep(LoopInterval);

                    //// => Temporary for Timing Analysis
                    //Debug.WriteLine(string.Format("Time spent in NewDiag = {0}, Time in InputUpdate = {1}",
                    //   sw.ElapsedMilliseconds, timeInputUpdate));
                    //// <=
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Error UpdateLoop: ");
                }
            } while (!_destroy);
        }


        private void CounterLoop(object sender, DoWorkEventArgs e)
        {
            do
            {
                try
                {
                    if (!UsePSOCounter)
                    {
                        return;
                    }
                    UpdateCounter(null);
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Error CounterLoop: ");
                }
            } while (!_destroy);
        }

        #endregion ControllerEvents
    }
}
