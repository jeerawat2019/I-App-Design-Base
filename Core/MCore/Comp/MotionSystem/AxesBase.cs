using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace MCore.Comp.MotionSystem
{
    public class AxesBase : CompBase
    {
        private object _lockObj = new object();

        #region Public Browsable Properties

        /// <summary>
        /// The lock object for this axes
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public object LockObj
        {
            get { return _lockObj; }
            set { _lockObj = value; }
        }
        /// <summary>
        /// The identifier for this Axes
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("The identifier for this Axes")]
        public int AxesNo
        {
            get { return GetPropValue(() => AxesNo, 0); }
            set { SetPropValue(() => AxesNo, value); }
        }


       
        #endregion Public Browsable Properties
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AxesBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public AxesBase(string name)
            : base(name)
        {            
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="axesNo">The identifier for this particular axes</param>
        public AxesBase(string name, int axesNo)
            : base(name)
        {
            AxesNo = axesNo;
        }
        #endregion Constructors
    }
}
