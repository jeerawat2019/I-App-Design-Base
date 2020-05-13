using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.Geometry
{
    public class GArc : GLine
    {

        #region Privates

        #endregion Privates

        
        #region Public Properties

        /// <summary>
        /// Radius
        /// </summary>
        /// <remarks>Negative creates arc  greater than or equal 180 deg.  Positive makes arc less than 180</remarks>
        [Browsable(true)]
        [Category("Location")]
        [Description("Negative creates arc >= 180 deg")]
        public Millimeters Radius
        {
            get { return GetPropValue(() => Radius, 1); }
            set { SetPropValue(() => Radius, value); }
        }

        /// <summary>
        /// Direction of the arc
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Clockwise if true")]
        public bool CW
        {
            get { return GetPropValue(() => CW, true); }
            set { SetPropValue(() => CW, value); }
        }
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GArc()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GArc(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            GArc newComp = base.Clone(name, bRecursivley) as GArc;
            newComp.Radius = Radius;
            newComp.CW = CW;
            return newComp;
        }
        #endregion Constructors

    }
}
