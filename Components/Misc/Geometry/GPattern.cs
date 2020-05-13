using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;

namespace MCore.Comp.Geometry
{
    /// <summary>
    /// Pattern class contains Line/Arc children which are mainatined as children
    /// </summary>
    public class GPattern
    {
        #region Privates

        private List<GLine> _lineList = new List<GLine>();
        private bool _fixedPitchX = false;
        private bool _fixedPitchXLast = false;
        private GWorkPiece _wp = null;
        private Millimeters _fixedPitchXDistance = null;
        private MillimetersPerSecond _transitionSpeed = new MillimetersPerSecond(300.0);

        #endregion Privates



        #region Public Properties

        /// <summary>
        /// The workpiece owner
        /// </summary>
        public GWorkPiece Workpiece
        {
            get { return _wp; }
            set { _wp = value; }
        }

        /// <summary>
        /// Whether the trigger is on a specific interval or varied
        /// </summary>
        public bool FixedPitchX
        {
            get { return _fixedPitchX; }
            set { _fixedPitchX = value; }
        }

        /// <summary>
        /// Whether this GPattern is the last of a series of FixedPitchX
        /// </summary>
        public bool FixedPitchXLast
        {
            get { return _fixedPitchXLast; }
            set { _fixedPitchXLast = value; }
        }

        /// <summary>
        /// Get the fixed distance if FixedPitchX is true
        /// </summary>
        public Millimeters FixedPitchXDistance
        {
            get { return _fixedPitchXDistance; }
            set { _fixedPitchXDistance = value; }
        }
        /// <summary>
        /// Get the Transition Speed
        /// </summary>
        public MillimetersPerSecond TransitionSpeed
        {
            get { return _transitionSpeed; }
            set { _transitionSpeed = value; }
        }
        public bool HasChildren
        {
            get
            {
                return _lineList.Count > 0;
            }
        }

        public GLine FirstLine
        {
            get { return _lineList.First(); }
        }

        public GLine LastLine
        {
            get { return _lineList.Last(); }
        }

        public GLine[] Lines
        {
            get { return _lineList.ToArray(); }
        }

        public void AddLine(GLine line)
        {
            _lineList.Add(line);
        }

        public void AddLines(GLine[] lines)
        {
            _lineList.AddRange(lines);
        }

        public void ClearLines()
        {
            _lineList.Clear();
        }

        public GTriggerPoint[] TriggerPts
        {
            get
            {
                List<GTriggerPoint> list = new List<GTriggerPoint>();
                if (HasChildren)
                {
                    foreach (GLine line in _lineList)
                    {
                        list.AddRange(line.TriggerPts);
                    }
                }
                return list.ToArray();
            }
        }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GPattern()
        {
        }
        #endregion Constructors


        #region Public Functions

        string _dumpString = string.Empty;
        /// <summary>
        /// Prepare this pattern for Execution
        /// </summary>
        public void PrepareForExecute()
        {
            foreach (GLine line in _lineList)
            {
                line.PrepareForExecute();
            }
            AddTiming("PrepareForExecute");
        }

        /// <summary>
        /// Prepare this pattern for Execution
        /// </summary>
        public void Reset()
        {
            foreach (GLine line in _lineList)
            {
                line.Reset();
            }
        }

        /// <summary>
        /// Adjust parmeters according to formulae dependent on Row/Col
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void ApplyFormulaes(int row, int col)
        {
            GLine prev = null;
            foreach (GLine line in _lineList)
            {
                line.ApplyFormulaes(prev, row, col);
                prev = line;
            }
        }

        /// <summary>
        /// Pauses execution until this pattern is ready to send to the motion controller
        /// </summary>
        /// <returns>Returns true if can continue.  Returns false if need to abort.</returns>
        public bool WaitingForReady
        {
            get
            {
                if (HasChildren)
                {
                    return _lineList[0].WaitingForReady;
                }
                return false;
            }
        }
        /// <summary>
        /// Pauses execution until this pattern is ready to send to the motion controller
        /// </summary>
        /// <returns>Returns true if can continue.  Returns false if need to abort.</returns>
        public bool WaitForReady()
        {
            bool bRet = true;
            // Default is no pausing
            if(HasChildren)
            {
                bRet = _lineList[0].WaitForReady();
            }
            AddTiming("WaitForReady");
            return bRet;
        }

        public void AddTiming(params string[] operations)
        {
            if (_wp != null)
            {
                _wp.AddTimingElement(operations);
            }
        }


        #endregion Virtuals



    }
}
