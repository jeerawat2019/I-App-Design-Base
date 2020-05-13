using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MCore.Comp.IOSystem.Output
{
    public class IntOutput : IOChannel
    {
        [Browsable(true)]
        [Category("Value")]
        public int Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public IntOutput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public IntOutput(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Set the output 
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(int val)
        {
            if (val != Value)
            {
                GetParent<IOSystemBase>().Set(this, val);
            }
        }

    }
}
