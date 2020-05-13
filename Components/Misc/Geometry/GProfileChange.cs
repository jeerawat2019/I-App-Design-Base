using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.Geometry
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class GProfileChange : GDistance
    {
        private Millimeters _zOffset = new Millimeters(0.0);
        private string _zChangeFormulae = string.Empty;
        /// <summary>
        /// Z Offset
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Z Offset")]
        public Millimeters ZOffset
        {
            get { return _zOffset; }
            set { _zOffset = value; }
        }

        ///// <summary>
        ///// Z Change Adjust
        ///// </summary>
        ///// <remarks>"1.0 + 2.0*c + 1.2*r"</remarks>
        //[Browsable(true)]
        //[Category("Location")]
        //[Description("Z change Formulae")]
        //public string ZChangeFormulae
        //{
        //    get { return _zChangeFormulae; }
        //    set { _zChangeFormulae = value; }
        //}

        //private MillimetersPerSecond _speed = 0;
        //private string _speedFormulae = string.Empty;
        ///// <summary>
        ///// For Speed change
        ///// </summary>
        //[Browsable(true)]
        //[Category("Location")]
        //[Description("Speed")]
        //public MillimetersPerSecond Speed
        //{
        //    get { return _speed; }
        //    set { _speed = value; }
        //}

        ///// <summary>
        ///// Speed Adjustment
        ///// </summary>
        ///// <remarks>"1.0 + 2.0*r + 1.2*c"</remarks>
        //[Browsable(true)]
        //[Category("Location")]
        //[Description("Speed change Formulae")]
        //public string SpeedFormulae
        //{
        //    get { return _speedFormulae; }
        //    set { _speedFormulae = value; }
        //}

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GProfileChange()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GProfileChange(GLine line)
            : base(line)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="newProfilePt"></param>
        /// <returns></returns>
        public GProfileChange Clone(GProfileChange newProfilePt)
        {
            newProfilePt.Distance = Distance;
            newProfilePt.LeadTime = LeadTime;
            newProfilePt.ZOffset.Val = ZOffset;
            //newProfilePt.Speed.Val = Speed;
            //newProfilePt.ZChangeFormulae = ZChangeFormulae;
            //newProfilePt.SpeedFormulae = SpeedFormulae;
            return newProfilePt;
        }
        /// <summary>
        /// Return the final robot commaded position
        /// </summary>
        public override G3DPoint GetRobotLoc(bool applyToolOffset, bool applyTrajLead)
        {
            G3DPoint pt = base.GetRobotLoc(applyToolOffset, applyTrajLead);
            pt.Z.Val += ZOffset;
            return pt;
        }
        ///// <summary>
        ///// Adjust parmeters according to formulae dependent on Row/Col
        ///// </summary>
        ///// <param name="row"></param>
        ///// <param name="col"></param>
        //public override void ApplyFormulaes(int row, int col)
        //{
        //    base.ApplyFormulaes(row, col);

        //    string rowAssignment = string.Format("r={0}", row);
        //    string colAssignment = string.Format("c={0}", col);
        //    if (!string.IsNullOrEmpty(ZChangeFormulae))
        //    {
        //        ZChange = U.Formulae(ZChangeFormulae, rowAssignment, colAssignment);
        //    }

        //    if (!string.IsNullOrEmpty(SpeedFormulae))
        //    {
        //        Speed = U.Formulae(SpeedFormulae, rowAssignment, colAssignment);
        //    }            
        //}
    }
}
