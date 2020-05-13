using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using MDouble;
using MCore.Comp.IOSystem.Output;

namespace MCore.Comp.IOSystem
{
    public class CompMeasure : CompBase
    {
        #region Privates

        private BoolOutput _externalTriggerOutput = null;

        #endregion Privates

        //public BoolOut 

        #region Public Definitions

        public BoolOutput RefExternalTriggerOutput
        {
            get 
            { 
                if (_externalTriggerOutput == null)
                {
                    _externalTriggerOutput = U.GetComponent(ExtTriggerId) as BoolOutput;
                }
                return _externalTriggerOutput; 

            }
        }

        /// <summary>
        /// Mode of the measuring device
        /// </summary>
        public enum eTriggerMode
        {
            /// <summary>No triggerring</summary>
            Idle,
            /// <summary>Single trigger</summary>
            SingleTrigger,
            /// <summary>Trigger at a specific interval</summary>
            TimedTrigger,
            /// <summary>Continuous or live triggerring</summary>
            Live
        }

        #endregion Public Definitions

        /// <summary>
        /// The External Trigger mode
        /// </summary>
        [Browsable(true)]
        [Category("Measure")]
        [Description("External Trigger mode")]
        public bool ExternalTrigger
        {
            get { return GetPropValue(() => ExternalTrigger, false); }
            set { SetPropValue(() => ExternalTrigger, value); }
        }

        /// <summary>
        /// The Trigger mode
        /// </summary>
        [Browsable(true)]
        [Category("Measure")]
        [Description("The Trigger mode")]
        [XmlIgnore]
        public eTriggerMode TriggerMode
        {
            get { return GetPropValue(()=> TriggerMode, eTriggerMode.Idle); }
            set { SetPropValue(()=> TriggerMode, value); }
        }

        /// <summary>
        /// The Id of the output that triggers the measure
        /// </summary>
        [Browsable(true)]
        [Category("Measure")]
        [Description("The Id of the output that triggers the measure")]
        public string ExtTriggerId
        {
            get { return GetPropValue(() => ExtTriggerId); }
            set { SetPropValue(() => ExtTriggerId, value); }
        }

        /// <summary>
        /// The interval for a Timed Trigger
        /// </summary>
        [Browsable(true)]
        [Category("Measure")]
        [Description("The interval for a Timed Trigger")]
        public Miliseconds TriggerInterval
        {
            get { return GetPropValue(()=> TriggerInterval, 500); }
            set { SetPropValue(()=> TriggerInterval, value); }
        }

        /// <summary>
        /// The Default timeout for the trigger
        /// </summary>
        [Browsable(true)]
        [Category("Measure")]
        [Description("The default timeout for a trigger")]
        public Miliseconds DefaultTimeout
        {
            get { return GetPropValue(() => DefaultTimeout, new Miliseconds(500.0)); }
            set { SetPropValue(() => DefaultTimeout, value); }
        }
        #region Public Properties


        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CompMeasure()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CompMeasure(string name)
            : base(name)
        {
        }
        #endregion Constructors
        
        /// <summary>
        /// Trigger and wait for measured result
        /// </summary>
        [StateMachineEnabled]
        public bool Measure()
        {
            return Measure(DefaultTimeout);
        }
        /// <summary>
        /// Trigger and wait for measured result
        /// </summary>
        /// <param name="timeout">Max time to wait</param>
        [StateMachineEnabled]
        public virtual bool Measure(Miliseconds timeout)
        {
            return true;
        }

        /// <summary>
        /// Trigger and wait for measured result
        /// </summary>
        /// <param name="timeout">Max time to wait</param>
        [StateMachineEnabled]
        public virtual void FireTrigger()
        {
            BoolOutput boolOut = RefExternalTriggerOutput;
            if (boolOut != null)
            {
                boolOut.SetTrue();
            }
        }

        private BackgroundWorker _measureLoop = null;
        private volatile bool _killMeasureLoop = false;

        /// <summary>
        /// Start the Measure Loop
        /// </summary>
        /// <param name="action"></param>
        public virtual void StartMeasureLoop(MethodInvoker action)
        {
            _killMeasureLoop = false;
            _measureLoop = new BackgroundWorker();
            _measureLoop.DoWork += new DoWorkEventHandler(MeasureLoop);
            _measureLoop.RunWorkerAsync(action);
        }

        private void MeasureLoop(object sender, DoWorkEventArgs e)
        {
            U.AddThread(Nickname);
            try
            {
                MethodInvoker action = e.Argument as MethodInvoker;
                do
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex, "Measure Loop Error");
                        StopMeasureLoop();
                    }
                    Thread.Sleep(TriggerInterval.ToInt);

                } while (!_killMeasureLoop);
            }
            finally
            {
                U.RemoveThread();
            }
        }
        /// <summary>
        /// Stop the measure loop
        /// </summary>
        public virtual void StopMeasureLoop()
        {
            _killMeasureLoop = true;
            _measureLoop = null;
        }
        public override void PreDestroy()
        {
            StopMeasureLoop();
            base.PreDestroy();
        }
    }
}
