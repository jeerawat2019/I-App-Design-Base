using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

using MDouble;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;
using adlink.daqpilot.engine.v1;


namespace MCore.Comp.IOSystem.ADLink
{
    public class ADLinkIO : IOSystemBase
    {
        #region Fields and Properties
        private DAQPilotEngine _daqInputs = null;
        private DAQPilotEngine _daqOutputs = null;
        private string _taskInput = "JJBTaskInput";
        private string _taskOutput = "JJBTaskPortOutput";
        private Timer _timer = null;
        private Miliseconds _timerPeriod = 50.0;
        private Inputs _inputs = null;
        private Outputs _outputs = null;
        private enum eReadWrite { Read, Write };

        private List<byte> _outputValues = new List<byte>();

        /// <summary>
        /// Period of the timer
        /// </summary>
        [Browsable(true)]
        [Category("ADLink")]
        [Description("Input Timer Period")]
        [DisplayName("Input Timer Period")]
        public Miliseconds TimerPeriod
        {
            get { return _timerPeriod; }
            set 
            { 
                _timerPeriod = value; 
                NotifyPropertyChanged(() => TimerPeriod);
            }
        }
        /// <summary>
        /// Task Input name
        /// </summary>        
        [Browsable(true)]
        [Category("ADLink")]
        [Description("Task input file")]
        [DisplayName("ADLink task file for Digital Inputs")]
        public string TaskInput
        {
            get { return _taskInput; }
            set { _taskInput = value; }
        }

        /// <summary>
        /// Task output name
        /// </summary>        
        [Browsable(true)]
        [Category("ADLink")]
        [Description("Task output file")]
        [DisplayName("ADLink task file for Digital Outputs")]
        public string TaskOutput
        {
            get { return _taskOutput; }
            set { _taskOutput = value; }
        }


        #endregion Fields and Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ADLinkIO()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public ADLinkIO(string name)
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
            // Initialize Serial Port
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                _daqInputs = new DAQPilotEngine();
                _daqOutputs = new DAQPilotEngine();

                bool initializedInput = _daqInputs.LoadTask(_taskInput);
                if (!initializedInput || (_daqInputs.Status != (long)DAQPilotStatusID.DP_STATUS_NOERROR && _daqInputs.Status != 5))
                {
                    throw new ForceSimulateException("{0} initializing Inputs failed. Initialized={1}, Status={2}", Name, initializedInput, _daqInputs.Status);
                }
                bool initializedOutput = _daqOutputs.LoadTask(_taskOutput);
                if (!initializedOutput || (_daqOutputs.Status != (long)DAQPilotStatusID.DP_STATUS_NOERROR && _daqOutputs.Status != 5))
                {
                    throw new ForceSimulateException("{0} initializing Outputs failed. Initialized={1}, Status={2}", Name, initializedOutput, _daqOutputs.Status);
                }

                _inputs = FilterByTypeSingle<Inputs>();
                _outputs = FilterByTypeSingle<Outputs>();
                if (_outputs != null)
                {
                    BoolOutput[] boolOutputs = _outputs.FilterByType<BoolOutput>();
                    int maxChan = 0;
                    foreach (BoolOutput boolOutput in boolOutputs)
                    {
                        maxChan = Math.Max(maxChan, boolOutput.Channel);
                    }
                    int numChans = maxChan + 1;
                    if ((numChans % 8) != 0)
                    {
                        numChans++;
                    }

                    int numPorts = numChans / 8;

                    for (int i = 0; i < numPorts; i++)
                    {
                        _outputValues.Add(0xff);
                    }

                    foreach (BoolOutput boolOutput in boolOutputs)
                    {
                        SetOutputPortValue(boolOutput);
                    }
                    if (_inputs != null)
                    {
                        _timer = new Timer(OnTimer, this, 1000, _timerPeriod.ToInt);
                        RegisterOnChanged(() => TimerPeriod, OnChangeTimerPeriod);
                    }
                }
            }
            catch (ForceSimulateException fsex)
            {
                throw fsex;
            }
            catch (Exception ex)
            {
                throw new ForceSimulateException(ex);
            }
            Simulate = eSimulate.None;

        }

        /// <summary>
        /// Destroy this Compopnent
        /// </summary>
        public override void Destroy()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            base.Destroy();
            if (_daqInputs != null)
            {
                _daqInputs.EndTask();
                _daqInputs.Dispose();
            }
            if (_daqOutputs != null)
            {
                _daqOutputs.EndTask();
                _daqOutputs.Dispose();
            }
        }
        /// <summary>
        /// Set the boolean outputs
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public override void Set(BoolOutput boolOutput, bool value)
        {
            int portNum = boolOutput.Channel / 8;

            boolOutput.Value = value;
            SetOutputPortValue(boolOutput);

            CommandThreadSafe(eReadWrite.Write, portNum, _outputValues[portNum]);
        }

        #endregion Overrides

        private void SetOutputPortValue(BoolOutput boolOutput)
        {
            int portNum = boolOutput.Channel / 8;
            int bitNo = boolOutput.Channel % 8;
            int bit = 1;
            for (int i = 0; i < bitNo; i++)
            {
                bit <<= 1;
            }

            if (boolOutput.Value)
                _outputValues[portNum] &= (byte)~bit;  // 0 is ON
            else
                _outputValues[portNum] |= (byte)bit;  // 1 is OFF

        }

        private void OnChangeTimerPeriod(Miliseconds ms)
        {
            _timer.Change(0, ms.ToInt);
        }

        private static void OnTimer(Object state)
        {
            (state as ADLinkIO).ReadAll();
        }

        private object _lockCommand = new object();

        private object CommandThreadSafe(eReadWrite readWrite, int channel, object objVal)
        {
            lock (_lockCommand)
            {
                switch(readWrite)
                {
                  case eReadWrite.Read:
                    _daqInputs.EnableSingleChannel(channel);
                    object oData = _daqInputs.Read();
                    System.Type type = oData.GetType();
                    if (!type.IsArray)
                    {
                        return oData;
                    }
                    break;
                  case eReadWrite.Write:
                    _daqOutputs.EnableSingleChannel(channel);
                    _daqOutputs.Write(Convert.ToInt32(objVal) & 0xffff);
                    break;
                }
            }
            return null;
        }

        private void ReadAll()
        {
            BoolInput[] inputs = _inputs.FilterByType<BoolInput>();
            foreach (BoolInput boolInput in inputs)
            {
                if (boolInput.Enabled)
                {
                    object iData = CommandThreadSafe(eReadWrite.Read, boolInput.Channel, null);
                    boolInput.Value = System.Convert.ToBoolean(iData);
                }
            }
        }
    }
}
