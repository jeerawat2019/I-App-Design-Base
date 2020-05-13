using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;


namespace MCore.Comp.IOSystem.Output
{
    public class DoubleOutput : IOChannel
    {
        [Browsable(true)]
        [Category("Value")]
        [XmlIgnore]
        public double Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public DoubleOutput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public DoubleOutput(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Set the output 
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(double val)
        {
            if (val != Value)
            {
                GetParent<IOSystemBase>().Set(this, val);
            }
        }

    }
}
