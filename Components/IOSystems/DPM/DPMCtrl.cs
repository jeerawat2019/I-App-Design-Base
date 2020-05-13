using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp.Communications;
using MCore.Comp.IOSystem.Input;

using MDouble;

namespace MCore.Comp.IOSystem
{
    public class DPMCtrl : IOSystemBase
    {
        #region private Data

        private MilligramInput _milligramInput = null;
        // Used for controllers to wait for move completion
        // Same as Reset (Blocks the thread)
        private ManualResetEvent _waitMeasureDone = new ManualResetEvent(true);

        #endregion private Data

        #region Public Properties

        public MilligramInput milligramInput
        {
            get { return _milligramInput; }
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
        public DPMCtrl()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public DPMCtrl(string name)
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
                throw new ForceSimulateException("DPMCtrl needs an RS232 parent");
            }

            if (!Enabled || !Parent.Enabled)
            {
                return;
            }

            Port.FullyDefined = true;
            Port.RefreshPort();
            
            if (!Port.IsOpen)
            {
                throw new ForceSimulateException("Port is not opened");
            }

            try
            {
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
            if (!Enabled)
            {
                return;
            }
            _milligramInput = this.FilterByTypeSingle<MilligramInput>();
            if (milligramInput == null)
            {
                U.LogPopup("DPMCtrl needs a MilliGramInput");
            }
            else
            {
                OnChangedTriggerMode(milligramInput.TriggerMode);
                OnOutputTime(milligramInput.TriggerInterval);
                U.RegisterOnChanged(() => milligramInput.TriggerMode, OnChangedTriggerMode);
                U.RegisterOnChanged(() => milligramInput.TriggerInterval, OnOutputTime);
                Port.OnDataReceived += new DataRecievedEventHandler(OnDataReceived);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
        }
        #endregion Overrides

        private void OnChangedTriggerMode(CompMeasure.eTriggerMode triggerMode)
        {
            switch (triggerMode)
            {
                case CompMeasure.eTriggerMode.Idle:
                    break;
                case CompMeasure.eTriggerMode.SingleTrigger:
                    //Port.SendCommand = "*1A1";
                    Port.WriteLine("*1B1");
                    milligramInput.TriggerMode = CompMeasure.eTriggerMode.Idle;
                    break;
                case CompMeasure.eTriggerMode.TimedTrigger:
                    break;
                case CompMeasure.eTriggerMode.Live:
                    //Port.SendCommand = "*1A0";
                    break;
            }
        }
        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="timeout"></param>
        public override bool Measure(CompMeasure input,  Miliseconds timeout)
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
        private void OnOutputTime(Miliseconds outputTime)
        {
            try
            {
                //int iVal = Math.Max(1, outputTime.ToInt);
                //SetParameterInt("SP_OutputTime", iVal);
                //SensorCommand("Set_OutputTime");
            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
        }
        private void OnDataReceived(string text)
        {
            try
            {
                lock (_waitMeasureDone)
                {
                    milligramInput.Value = new Grams(Convert.ToDouble(text));
                    _waitMeasureDone.Set();
                }
            }
            catch (Exception ex)
            {
                U.LogError(ex, "'{0}' received unexpected text: '{1}'", Nickname, text); 
            }
        }
    }
}
