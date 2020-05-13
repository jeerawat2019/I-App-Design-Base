using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace MCore.Comp.IOSystem.Input
{
    public class InputBase : IOChannel
    {

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public InputBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public InputBase(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Trigger to set the input value
        /// </summary>
        [StateMachineEnabled]
        public virtual void Trigger()
        {
        }
    }
}
