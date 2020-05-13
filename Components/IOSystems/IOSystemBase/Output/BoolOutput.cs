using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDouble;

namespace MCore.Comp.IOSystem.Output
{
    public class BoolOutput : IOBoolChannel
    {
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public BoolOutput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public BoolOutput(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Set the output 
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(bool val)
        {
            GetParent<IOSystemBase>().Set(this, val);
        }
        /// <summary>
        /// Pulse a digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value">Which direction to pulse</param>
        /// <param name="duration">Length of the Pulse</param>
        public virtual void SetPulse(bool value, Miliseconds duration)
        {
            GetParent<IOSystemBase>().SetPulse(this, value, duration);
        }
        /// <summary>
        /// Set the output to True
        /// </summary>
        [StateMachineEnabled]
        public virtual void SetTrue()
        {
            Set(true);
        }
        /// <summary>
        /// Set the output to False
        /// </summary>
        [StateMachineEnabled]
        public virtual void SetFalse()
        {
            Set(false);
        }
        #endregion Constructors
    }
   
}
