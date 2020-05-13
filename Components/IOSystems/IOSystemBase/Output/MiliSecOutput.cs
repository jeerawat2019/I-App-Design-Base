using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MDouble;


namespace MCore.Comp.IOSystem.Output
{
    public class MiliSecOutput : IOChannel
    {
        [Browsable(true)]
        [Category("Value")]
        public Miliseconds Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MiliSecOutput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MiliSecOutput(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Set the output 
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(Miliseconds val)
        {
            if (val.Val != Value.Val)
            {
                GetParent<IOSystemBase>().Set(this, val);
            }
        }
        /// <summary>
        /// Sesend the last command
        /// </summary>
        public virtual void Resend()
        {
            GetParent<IOSystemBase>().Set(this, Value);
        }

    }
}
