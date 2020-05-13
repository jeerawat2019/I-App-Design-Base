using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MCore.Comp.IOSystem
{
    public class IOBoolChannel : IOChannel
    {
        [Browsable(true)]
        [Category("Bool Channel")]
        public bool Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, false); }
            set { SetPropValue(() => Value, value); }
        }
        public enum eType { Normal, PSOAxes };


        /// <summary>
        /// Get/Set the channel
        /// </summary>
        [Browsable(true)]
        [Category("Id")]
        public eType OutputType
        {
            get { return GetPropValue(() => OutputType, eType.Normal); }
            set { SetPropValue(() => OutputType, value); }
        }


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public IOBoolChannel()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="channel"></param>
        public IOBoolChannel(string name)
            : base(name)
        {            
        }

        #endregion Constructors
    }
}
