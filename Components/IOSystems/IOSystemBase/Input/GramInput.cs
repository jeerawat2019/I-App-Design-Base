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
    public class GramInput : MDoubleInput
    {

        #region Public Properties

        [Browsable(true)]
        [Category("Grams Input")]
        public Grams Value
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
        public GramInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GramInput(string name)
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
