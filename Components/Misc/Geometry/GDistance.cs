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
    public class GDistance
    {
        private Millimeters _distance = new Millimeters(1.0);
        private Miliseconds _leadTime = new Miliseconds(0.0);
        protected GLine _line = null;

        /// <summary>
        /// Distance from start point of current geometry component (line or arc).
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Distance from start point of current geometry component (line or arc).")]
        public Millimeters Distance
        {
            get { return _distance; }
            set { _distance.Val = value; }
        }

        /// <summary>
        /// Distance from start point of current geometry component (line or arc).
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [ReadOnly(true)]
        [Description("Distance from start point of current geometry component (line or arc).")]
        public Millimeters DistanceWithLead
        {
            get 
            {
                if (_line != null)
                {
                    return _distance - _leadTime.ToSeconds * _line.Speed;
                }
                return _distance;
            }
        }


        /// <summary>
        /// Lead time from start point of current geometry component (line or arc).
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Lead time from start point of current geometry component (line or arc).")]
        public Miliseconds LeadTime
        {
            get { return _leadTime; }
            set { _leadTime.Val = value; }
        }


        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GDistance()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GDistance(GLine line)
        {
            _line = line;
        }
        #endregion Constructors

        #region Virtuals
        public virtual void Initialize(GLine line)
        {
            _line = line;
        }
        public virtual void Dispose()
        {
        }
        /// <summary>
        /// Return the final robot commaded position
        /// </summary>
        public virtual G3DPoint RobotLoc
        {
            get
            {
                return GetRobotLoc(true, true);
            }
        }

        /// <summary>
        /// Return the final robot commaded position
        /// </summary>
        public virtual G3DPoint GetRobotLoc(bool applyToolOffset, bool applyTrajLead)
        {
            return _line.GetRobotLoc(DistanceWithLead, applyToolOffset, applyTrajLead);
        }
        #endregion Virtuals

        #region Public Methods
        
        /// <summary>
        /// Convert the point to pixel
        /// </summary>
        /// <param name="applyTrajLead"></param>
        /// <param name="applyToolOffset"></param>
        /// <returns></returns>
        public PointF ToPixel(bool applyToolOffset, bool applyTrajLead)
        {
            return _line.ToPixel(DistanceWithLead, applyToolOffset, applyTrajLead);
        }

        /// <summary>
        /// Convert the point to pixel
        /// </summary>
        /// <param name="applyTrajLead"></param>
        /// <param name="applyToolOffset"></param>
        /// <param name="lenFarther"></param>
        /// <returns></returns>
        public PointF ToPixel(bool applyToolOffset, bool applyTrajLead, Millimeters lenFarther)
        {
            return _line.ToPixel(DistanceWithLead + lenFarther, applyToolOffset, applyTrajLead);
        }

        public PointF RelativeLoc(bool applyToolOffset, bool applyTrajLead)
        {
            return _line.RelVectorPoint(DistanceWithLead, applyToolOffset, applyTrajLead);
        }

    }
    #endregion Public Methods
}
