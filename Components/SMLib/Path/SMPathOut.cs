using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

using MCore.Comp.SMLib.Flow;


namespace MCore.Comp.SMLib.Path
{
    public class SMPathOut : SMPathSegment
    {
        #region Serialize properties

        private string _targetId = string.Empty;
        /// <summary>
        /// Target id.  Use parent to find.
        /// </summary>
        public string TargetID 
        {
            get { return _targetId; }
            set 
            {
                if (Owner == null)
                {
                    _targetId = value;
                }
                else
                {
                    string oldVal = _targetId;
                    SMFlowBase oldTgt = Owner.ParentContainer.GetFlowTarget(this);
                    _targetId = value;
                    SMFlowBase newTgt = Owner.ParentContainer.GetFlowTarget(this);

                    if (_targetId != oldVal)
                    {
                        U.LogChange(string.Format("{0}.{1}", Owner.Nickname, GetType().Name), oldTgt == null ? "Null" : oldTgt.BestDisplayText, newTgt == null ? "Null" : newTgt.BestDisplayText);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if we have reached a target
        /// </summary>
        public virtual bool HasTargetID
        {
            get { return !string.IsNullOrEmpty(TargetID); }
        }

        /// <summary>
        /// Determines if this segment path is selected or not
        /// </summary>
        #endregion

        private bool _highlighted = false;
        private SMFlowBase _flowBase = null;

        /// <summary>
        /// Get the owner of the path
        /// </summary>
        [XmlIgnore]
        public SMFlowBase Owner
        {
            get { return _flowBase; }
        }

        /// <summary>
        /// Returns true if the segment is highlighted because it is currently active during run of state machine
        /// </summary>
        [XmlIgnore]
        public bool Highlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                _highlighted = value;
            }
        }


        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPathOut()
        {
        }
        /// <summary>
        /// Manual creation Constructor
        /// </summary>
        /// <param name="initialGridDistance"></param>
        public SMPathOut(float initialGridDistance)
        {
            // Starts out to the left
            GridDistance = initialGridDistance;
        }
        #endregion Constructors

        /// <summary>
        /// Initialize this segment
        /// </summary>
        /// <param name="vertical"></param>
        public virtual void Initialize(SMFlowBase flowBase, bool vertical)
        {
            _flowBase = flowBase;
            Initialize(this, vertical, 0);
            InitializeRecurse();
        }

        /// <summary>
        /// Recursivley make a clone of this item 
        /// </summary>
        /// <returns>New cloned instance</returns>
        public override SMPath Clone()
        {
            SMPathOut newPathOut = base.Clone() as SMPathOut;
            newPathOut.TargetID = TargetID;
            newPathOut.Highlighted = Highlighted;
            return newPathOut;
        }


        public void OnChanged()
        {
            _flowBase.FireOnChangedEvent();
        }
        /// <summary>
        /// Set vertical, Index ,  and FirstSegment
        /// </summary>
        public void InitializeRecurse()
        {
            bool vertical = Vertical;
            int index = 1;
            SMPathSegment pathSeg = Next;
            while (pathSeg != null)
            {
                vertical = !vertical;
                pathSeg.Initialize(this, vertical, index++);
                pathSeg = pathSeg.Next;
            }
        }

        public void DeletedTarget()
        {
            _targetId = string.Empty;
        }

        public PointF FindEndPoint(PointF gridLoc)
        {
            SMPathSegment pathSeg = this;
            while (pathSeg != null)
            {
                if (pathSeg.Vertical)
                    gridLoc.Y += pathSeg.GridDistance;
                else
                    gridLoc.X += pathSeg.GridDistance;

                pathSeg = pathSeg.Next;
            }
            return gridLoc;
        }
    }
}
