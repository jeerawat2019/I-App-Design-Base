using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MCore.Comp.SMLib.Path
{
    public class SMPathSegment : SMPath
    {

        private float _gridDistance = float.NaN;
        private SMPathSegment _nextSegement = null;
        private SMPathOut _firstSegment = null;
        private int _index = 0;
        private bool _vertical = false;
        private bool _initialized = false;

        #region Serialize properties
        /// <summary>
        /// Point where line terminates in Grid Units
        /// </summary>SMPath
        public float GridDistance 
        {
            get
            {
                return _gridDistance;
            }
            set
            {
                if (_initialized && _gridDistance != value)
                {
                    _gridDistance = value;
                    First.OnChanged();
                }
                else
                {
                    _gridDistance = value;
                }

            }
        }
        /// <summary>
        /// The next line segment
        /// </summary>
        [XmlElement(ElementName="Next")]
        public SMPathSegment SerNext
        {
            get
            {
                return _nextSegement;
            }
            set
            {
                _nextSegement = value;
            }
        }
        #endregion Serialize properties

        #region Other Public properties
        /// <summary>
        /// Is this segment vertical or horizontal?
        /// </summary>
        [XmlIgnore]
        public bool Vertical 
        {
            get
            {
                return _vertical;
            }
        }
        /// <summary>
        /// Index number for this segment
        /// </summary>
        [XmlIgnore]
        public int Index
        {
            get
            {
                return _index;
            }
        }
        [XmlIgnore]
        public SMPathOut First 
        {
            get
            {
                return _firstSegment;
            }
        }


        /// <summary>
        /// The next line segment
        /// </summary>
        public SMPathSegment Next
        {
            get
            {
                return _nextSegement;
            }
        }
        /// <summary>
        /// The previous line segment
        /// </summary>
        public SMPathSegment Previous
        {
            get
            {
                SMPathSegment pathSeg = First;
                while (pathSeg.Next != null)
                {
                    if (object.ReferenceEquals(pathSeg.Next, this))
                    {
                        return pathSeg;
                    }
                    pathSeg = pathSeg.Next;
                }
                return null;
            }
        }
        /// <summary>
        /// Get the last segment
        /// </summary>
        public SMPathSegment Last
        {
            get
            {
                SMPathSegment pathSeg = this;
                while (pathSeg.Next != null)
                {
                    pathSeg = pathSeg.Next;
                }
                return pathSeg;
            }
        }
        #endregion Other Public properties

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPathSegment()
        {
        }     
        #endregion Constructors
        /// Recursivley make a clone of this item 
        /// </summary>
        /// <returns>New cloned instance</returns>
        public override SMPath Clone()
        {
            SMPathSegment newPathOut = base.Clone() as SMPathSegment;
            newPathOut._gridDistance = _gridDistance;
            if (Next != null)
            {
                newPathOut._nextSegement = Next.Clone() as SMPathSegment;
            }
            return newPathOut;
        }

        /// <summary>
        /// Initialize this segment
        /// </summary>
        /// <param name="firstSegment"></param>
        /// <param name="vertical"></param>
        /// <param name="index"></param>
        public void Initialize(SMPathOut firstSegment, bool vertical, int index)
        {
            _index = index;
            _firstSegment = firstSegment;
            _vertical = vertical;
            _initialized = true;
        }
        /// <summary>
        /// Append a new Segment
        /// </summary>
        /// <returns></returns>
        public SMPathSegment Append()
        {
            SMPathSegment newSeg = new SMPathSegment();
            this._nextSegement = newSeg;
            First.InitializeRecurse();
            return newSeg;
        }
        /// <summary>
        /// Delete this segment, but never delete the first one
        /// </summary>
        public void Delete()
        {
            if (Index > 0)
            {
                Previous._nextSegement = null;
            }
        }
    }
}