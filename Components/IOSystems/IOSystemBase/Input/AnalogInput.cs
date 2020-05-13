using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace MCore.Comp.IOSystem.Input
{
    public class AnalogInput : IOChannel
    {
        private ReaderWriterLockSlim __listChildrenLock = new ReaderWriterLockSlim();

        [Browsable(true)]
        [Category("Analog Input")]
        public double Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0.0); }
            set { SetPropValue(() => Value, value); }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AnalogInput()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public AnalogInput(string name)
            : base(name)
        {
        }
        #endregion Constructors
    }
}
