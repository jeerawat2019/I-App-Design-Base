using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MDouble;
using MCore.Helpers;
using MCore.Comp.XFunc;

namespace MCore.Comp.IOSystem.Input
{
    public class MDoubleInput : InputBase
    {
        protected BasicLinear _xferFunc = null;
        #region Public Properties
        /// <summary>
        /// The ZHt transfer function
        /// </summary>
        [Browsable(false)]
        public BasicLinear XferFunc
        {
            get 
            {
                if (_xferFunc != null)
                {
                    return _xferFunc;
                }
                _xferFunc = FilterByTypeSingle<BasicLinear>();
                return _xferFunc;
            }
        }

        [Browsable(true)]
        [Category("Raw Input")]
        public double RawInput
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RawInput, 0); }
            set { SetPropValue(() => RawInput, value); }
        }

        [Browsable(true)]
        [Category("Raw Input")]
        [Description("Minimum Value alowed for Raw Input")]
        public double RawInputMinValue
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => RawInputMinValue, 0); }
            set { SetPropValue(() => RawInputMinValue, value); }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Raw Input")]
        [Description("True if Raw Input is valid")]
        public bool RawInputIsValid
        {
            [StateMachineEnabled]
            get { return RawInput >= RawInputMinValue; }
        }


        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MDoubleInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MDoubleInput(string name)
            : base(name)
        {
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Trigger to set the input value
        /// </summary>
        public override void Trigger()
        {
            _ioSystem.Trigger(this);
        }

        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="timeout"></param>
        public override bool Measure(Miliseconds timeout)
        {
            return _ioSystem.Measure(this, timeout);
        }

        /// <summary>
        /// Reset this component
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            if (XferFunc != null)
            {
                XferFunc.Reset();
            }
        }
        #endregion Overrides
        /// <summary>
        /// Handle an incoming raw value
        /// </summary>
        /// <param name="dVal"></param>
        public virtual void ApplyRawValue(double dVal)
        {
            RawInput = dVal;
        }
        /// <summary>
        /// Teach the Xfer function with Response to stored raw value
        /// </summary>
        /// <param name="dResponse"></param>
        public void Teach(double dResponse)
        {
            if (XferFunc != null)
            {
                XferFunc.Teach(dResponse, RawInput);
            }
        }

    }
}
