using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MDouble;
using MCore.Helpers;

namespace MCore.Comp.IOSystem.Input
{
    public class MilligramInput : MDoubleInput
    {

        #region Public Properties

        [Browsable(true)]
        [Category("MilliGram Input")]
        public Milligrams Value
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
        public MilligramInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MilligramInput(string name)
            : base(name)
        {
        }

        #endregion Constructors
        #region Overrides
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


    }
}
