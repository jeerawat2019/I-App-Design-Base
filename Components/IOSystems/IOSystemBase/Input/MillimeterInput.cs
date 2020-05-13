using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MDouble;
using MCore.Helpers;

namespace MCore.Comp.IOSystem.Input
{
    public class MillimeterInput : MDoubleInput
    {
        private TriggerQueue<MillimeterEventHandler> _triggerQue = null;
        #region Public Properties

        /// <summary>
        /// The trigger queue
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public TriggerQueue<MillimeterEventHandler> TriggerQue
        {
            get { return _triggerQue; }
            set { _triggerQue = value; }
        }
        [Browsable(true)]
        [Category("Millimeter Input")]
        public Millimeters Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MillimeterInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MillimeterInput(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Overrides
        /// <summary>
        /// Initialize this compionent
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            RegisterOnChanged(() => Value, OnChangedValue);
        }
        /// <summary>
        /// Handle an incoming raw value
        /// </summary>
        /// <param name="dVal"></param>
        public override void ApplyRawValue(double dVal)
        {
            base.ApplyRawValue(dVal);
            if (XferFunc != null)
            {
                Value = XferFunc.Evaluate(dVal);
            }
            else
            {
                Value = dVal;
            }
        }
        #endregion Overrides

        private void OnChangedValue(Millimeters val)
        {
            if (TriggerQue != null)
            {
                MillimeterEventHandler delCallback = TriggerQue.GetNextCallback();
                if (delCallback != null)
                {
                    delCallback(val);
                }
            }
        }
    }
}
