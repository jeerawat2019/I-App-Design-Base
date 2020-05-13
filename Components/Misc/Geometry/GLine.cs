using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp.XFunc;
using MDouble;

namespace MCore.Comp.Geometry
{
    /// <summary>
    /// Class to manage a line or arc (Arc is derived from this object)
    /// The children of this component are the trigger points
    /// </summary>
    public class GLine : G3DCompBase
    {

        #region Privates
        // X/Y/Z is offset in mm from Location point of nearest parent that is derived from G3DPoint
        protected List<GTriggerPoint> _listTriggerPts = new List<GTriggerPoint>();
        protected List<GProfileChange> _listProfilePts = new List<GProfileChange>();
        private bool _fixedPitchX = false;
        private bool _ignoreStartPt = false;
        private bool _snapEndPoints = false;
        private ManualResetEvent _lastInWorkpiece = null;

        #endregion Privates

        
        #region Public Properties

        /// <summary>
        ///  Get the End point which is a child and should always be there
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Get the End point which is a child and should always be there")]
        public GProfileChange EndPoint
        {
            get 
            {
                if (_listProfilePts.Count == 0)
                {
                    _listProfilePts.Add(new GProfileChange(this) { Distance = 1.0 });
                }
                return _listProfilePts.Last();
            }
        }

        /// <summary>
        ///  Get the number of trigger points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Get the number of trigger points")]
        [XmlIgnore]
        public int NumTriggerPts
        {
            get { return _listTriggerPts.Count; }
        }

        /// <summary>
        ///  Get the number of trigger groups
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Get the number of trigger groups")]
        [XmlIgnore]
        public int NumTriggerGroups
        {
            get 
            {
                int numTriggerAnchors = 0;
                if (NumTriggerPts > 0)
                {
                    foreach (GTriggerPoint tPt in _listTriggerPts)
                    {
                        if (tPt.Anchor)
                        {
                            numTriggerAnchors++;
                        }
                    }
                }
                return Math.Max(1, numTriggerAnchors / 2); 
            }
        }

        

        /// <summary>
        ///  The array of all the trigger points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The array of all the trigger points")]
        [XmlIgnore]
        public GTriggerPoint[] TriggerPts
        {
            get
            {
                return _listTriggerPts.ToArray();
            }
            set
            {
                _listTriggerPts.Clear();
                _listTriggerPts.AddRange(value);
            }
        }

        /// <summary>
        ///  Get the number of Profile points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Get the number of Profile points")]
        [XmlIgnore]
        public int NumProfilePts
        {
            get { return _listProfilePts.Count; }
        }

        ///// <summary>
        ///// The unique ID for this Cam definition
        ///// </summary>
        //[Browsable(true)]
        //[Category("GLine")]
        //[Description("The unique ID for this Cam definition")]
        //public int CamID
        //{
        //    get { return GetPropValue(() => CamID, 0); }
        //    set { SetPropValue(() => CamID, value); }
        //}

        /// <summary>
        /// The Cam table count
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The Cam table count")]
        [XmlIgnore]
        public int CamTableCount
        {
            get { return _camTableArray == null ? 0 : _camTableArray.Length; }
        }

        /// <summary>
        /// Whether the camming with Z is in X dir or Y
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Whether the camming with Z is in X dir or Y")]
        public bool CamDirY
        {
            get { return GetPropValue(() => CamDirY); }
            set { SetPropValue(() => CamDirY, value); }
        }

        /// <summary>
        /// The factor applied to Z values
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The factor applied to Z values")]
        public string ZFactor
        {
            get { return GetPropValue(() => ZFactor); }
            set { SetPropValue(() => ZFactor, value); }
        }

        /// <summary>
        /// The Cam table delta distance betwee array points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The Cam table delta distance between array points")]
        public double CamTableDelta
        {
            get { return GetPropValue(() => CamTableDelta, 0); }
            set { SetPropValue(() => CamTableDelta, value); }
        }

        private double[] _camTableArray = null;
        /// <summary>
        /// The Cam Table values for serialization
        /// </summary>
        [Browsable(false)]
        [XmlElement("CamTableArray")]
        public double[] SerCamTableArray
        {
            get { return _camTableArray; }
            set { _camTableArray = value; }
        }
        /// <summary>
        /// The Cam Table values
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The Cam Table values")]
        [XmlIgnore]
        public double[] CamTableArray
        {
            get { return _camTableArray; }
            set 
            { 
                _camTableArray = value;
                //if (_camTableArray != null)
                //{
                //    Random r = new Random();
                //    CamID = r.Next(1, int.MaxValue);
                //}
                //else
                //{
                //    CamID = 0;
                //}
            } 
        }

        /// <summary>
        /// The formulae to use for speed
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The formulae to use for speed")]
        public string SpeedFormulae
        {
            get { return GetPropValue(() => SpeedFormulae); }
            set { SetPropValue(() => SpeedFormulae, value); }
        }

        ///// <summary>
        ///// The Speed for the line
        ///// </summary>
        //[Browsable(true)]
        //[Category("Editor")]
        //[Description("The Speed for this Component")]
        //[XmlIgnore]
        //public override MillimetersPerSecond Speed
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_speedFormulae))
        //        {
        //            return base.Speed;
        //        }

        //        return U.Formulae(_speedFormulae, "", "";
        //    }
        //    set { base.Speed = value; }
        //}
        /// <summary>
        /// The array of all the profile points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("The array of all the profile points")]
        public GProfileChange[] ProfilePts
        {
            get
            {
                return _listProfilePts.ToArray();
            }
            set
            {
                _listProfilePts.Clear();
                _listProfilePts.AddRange(value);
            }
        }

        /// <summary>
        /// Whether the trigger is on a specific interval or varied
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Whether the trigger is on a specific interval or varied")]
        public bool FixedPitchX
        {
            get { return _fixedPitchX; }
            set { _fixedPitchX = value; }
        }

        /// <summary>
        /// Dont Move to start point first
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Dont Move to start point first")]
        public bool IgnoreStartPt
        {
            get { return _ignoreStartPt; }
            set { _ignoreStartPt = value; }
        }

        /// <summary>
        /// Snap the end points
        /// </summary>
        [Browsable(true)]
        [Category("GLine")]
        [Description("Snap the end points")]
        public bool SnapEndPoints
        {
            get { return _snapEndPoints; }
            set { _snapEndPoints = value; }
        }

        /// <summary>
        /// The XY length of the line
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Location")]
        [Description("The XY length of the line in pixels")]
        [XmlIgnore]
        public Millimeters XYLength
        {
            get
            {
                return EndPoint.Distance;
            }
            set
            {
                EndPoint.Distance = value;
            }
        }

        /// <summary>
        /// The Center point in X
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Location")]
        [Description("Center in X")]
        [XmlIgnore]
        public double CenterX
        {
            get
            {
                G3DPoint pt = GetRobotLoc(XYLength / 2.0, false, false);
                return pt.X;
            }
            set
            {
                if (Math.Abs(Loc.Yaw) <= (Math.PI / 2.0))
                {
                    Loc.X = value;
                }
                else
                {
                    Loc.X = -value;
                }
            }
        }

        /// <summary>
        /// The Center point in Y
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Location")]
        [Description("Center in Y")]
        [XmlIgnore]
        public double CenterY
        {
            get
            {
                G3DPoint pt = GetRobotLoc(XYLength / 2.0, false, false);
                return pt.Y;
            }
            set
            {
                if (Loc.Yaw > 0 )
                {
                    Loc.Y = value;
                }
                else
                {
                    Loc.Y = -value;
                }
            }
        }

        /// <summary>
        /// Last line in workpiece
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Last line in workpiece")]
        [XmlIgnore]
        public ManualResetEvent LastInWorkpiece
        {
            get { return _lastInWorkpiece; }
            set { _lastInWorkpiece = value; }
        }

        /// <summary>
        /// ID of the Reference for the line
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("ID of the Reference for the line")]
        public string RefID
        {
            get { return GetPropValue(() => RefID, string.Empty); }
            set { SetPropValue(() => RefID, value); }
        }

        /// <summary>
        /// ID of the line defining the profile
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("ID of the line defining the profile")]
        public string ProfileLinkID
        {
            get { return GetPropValue(() => ProfileLinkID, string.Empty); }
            set { SetPropValue(() => ProfileLinkID, value); }
        }

        /// <summary>
        /// Timeout for execution
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Timeout for execution")]
        public Miliseconds Timeout
        {
            get { return GetPropValue(() => Timeout, 1000); }
            set { SetPropValue(() => Timeout, value); }
        }

        private int _order = 0;

        /// <summary>
        /// For internal use only
        /// </summary>
        [XmlIgnore]
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GLine()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GLine(string name)
            : base(name)
        {
            Loc.Yaw = Math.PI / 2;
        }

        /// <summary>
        /// Create a GLine given the absolute coordinate positions
        /// </summary>
        /// <param name="ptStart"></param>
        /// <param name="ptEnd"></param>
        public GLine(G3DPoint ptStart, G3DPoint ptEnd)
            : base()
        {
            SetAbsStartPoint(ptStart);
            SetAbsEndPoint(ptEnd);
        }

        #endregion Constructors

        #region Virtuals

        /// <summary>
        /// Prepare this pattern for Execution
        /// </summary>
        public virtual void PrepareForExecute()
        {
        }

        /// <summary>
        /// Returns true if we are waiting for a dependent action
        /// </summary>
        /// <remarks>Returning true might Pump the commands</remarks>
        [XmlIgnore]
        public virtual bool WaitingForReady
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// Pauses execution until this pattern is ready to send to the motion controller
        /// </summary>
        /// <returns>Returns true if can continue.  Returns false if need to abort.</returns>
        public virtual bool WaitForReady()
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("Sleeping(10) {0}", Nickname));             
            //Thread.Sleep(10);
            return true;
        }

        #endregion Virtuals

        #region Overrides

        /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            GLine newLine = base.Clone(name, bRecursivley) as GLine;
            CopyTo(newLine);
            newLine.RefID = RefID;
            newLine.ProfileLinkID = ProfileLinkID;

            return newLine;
        }

        public virtual void CopyTo(GLine targetLine)
        {
            targetLine._pulseWidth.Val = _pulseWidth;
            targetLine.Timeout = Timeout;
            targetLine.FixedPitchX = FixedPitchX;
            targetLine.ZFactor = ZFactor;
            targetLine.SpeedFormulae = SpeedFormulae;
            targetLine.SnapEndPoints = SnapEndPoints;
            targetLine.IgnoreStartPt = IgnoreStartPt;
            //targetLine.CamID = CamID;
            targetLine._listTriggerPts.Clear();
            foreach (GTriggerPoint triggerPt in _listTriggerPts)
            {
                targetLine._listTriggerPts.Add(triggerPt.Clone(targetLine.CreateTrigger()));
            }
            targetLine._listProfilePts.Clear();
            foreach (GProfileChange profilePt in _listProfilePts)
            {
                targetLine._listProfilePts.Add(profilePt.Clone(new GProfileChange(targetLine)));
            }

            if (CamTableCount > 0)
            {
                targetLine._camTableArray = new double[CamTableCount];
                Array.Copy(_camTableArray, targetLine._camTableArray, CamTableCount);
            }
            targetLine.CamTableDelta = CamTableDelta;
            targetLine.CamDirY = CamDirY;
        }

        /// <summary>
        /// Initialize this comonent
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            _listTriggerPts.ForEach(c => c.Initialize(this));
            _listProfilePts.ForEach(c => c.Initialize(this));
        }

        /// <summary>
        /// Dispose this component
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            _listTriggerPts.ForEach(c => c.Dispose());
            _listProfilePts.ForEach(c => c.Dispose());
        }
        #endregion Overrides
        private const int MaxCamTableLen = 100;
        /// <summary>
        /// Populate the CamTableArray
        /// </summary>
        /// <param name="zFactor"></param>
        public void BuildCamTable(double zFactor)
        {
            if (NumProfilePts <= 1 && (EndPoint.ZOffset == 0 || this.XYLength == 0))
            {
                CamTableArray = null;
                CamTableDelta = 0;
            }
            else
            {
                if (double.IsNaN(zFactor))
                {
                    if (!string.IsNullOrEmpty(ZFactor))
                    {
                        zFactor = U.Formulae(ZFactor, "r=0", "c=0");
                    }
                    else
                    {
                        zFactor = 1.0;
                    }
                }
                double cosTheta = CamDirY ? Math.Sin(Loc.Yaw) : Math.Cos(Loc.Yaw);
                double xDist = EndPoint.Distance * cosTheta;
                double absXDist = Math.Abs(xDist);
                double delta = Math.Round(absXDist / (MaxCamTableLen-1), 3);
                if (delta == 0)
                {
                    delta = 0.001;
                }
                CamTableDelta = delta;
                int nPts = Math.Min(MaxCamTableLen, (int)(absXDist / delta) + 1);
                _camTableArray = new double[nPts];
                double xyDelta = Math.Abs(delta / cosTheta);
                double prevDist = 0;
                double prevZ = 0;
                int i = 0;
                if (xDist < 0)
                {
                    i = nPts - 1;
                }
                int n = 0;
                foreach (GProfileChange profile in ProfilePts)
                {
                    double profileZOffset = profile.ZOffset * zFactor;
                    double L = profile.Distance - prevDist;
                    double slope = (profileZOffset - prevZ) / L;
                    for (double xy = n * xyDelta - prevDist; xy < L; xy += xyDelta, n++)
                    {
                        _camTableArray[i] = prevZ + xy*slope;
                        if (xDist < 0)
                        {
                            i--;
                            if (i < 0)
                            {
                                return;
                            }
                        }
                        else
                        {
                            i++;
                            if (i >= nPts)
                            {
                                return;
                            }
                        }
                    }
                    prevDist = profile.Distance;
                    prevZ = profileZOffset;
                }
            }
        }

        /// <summary>
        /// Set the absolute start point
        /// </summary>
        /// <param name="ptStart"></param>
        public void SetAbsStartPoint(G3DPoint ptStart)
        {
            Loc = new G3DPoint(ptStart);
            Loc.XYYawMode = G3DPoint.eMode.Absolute;
            Loc.ZMode = G3DPoint.eMode.Absolute;
        }

        /// <summary>
        /// Set the absolute end point
        /// </summary>
        /// <param name="ptEnd"></param>
        public void SetAbsEndPoint(G3DPoint ptEnd)
        {
            Loc.Yaw = Loc.YawTo(ptEnd);
            EndPoint.Distance = Loc.XYDistanceTo(ptEnd);
            EndPoint.ZOffset = ptEnd.Z - Loc.Z;
        }
        /// <summary>
        /// The Speed for the line
        /// </summary>
        [Browsable(true)]
        [Category("Editor")]
        [Description("The Speed for this Component")]
        [XmlIgnore]
        public override MillimetersPerSecond Speed
        {
            get
            {
                if (!string.IsNullOrEmpty(SpeedFormulae))
                {
                    return U.Formulae(SpeedFormulae, "r=0", "c=0");
                }
                return base.Speed;
            }
            set
            {
                base.Speed = value;
            }
        }

        /// <summary>
        /// Adjust parmeters according to formulae dependent on Row/Col
        /// </summary>
        /// <param name="prevLine"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void ApplyFormulaes(GLine prevLine, int row, int col)
        {
            // Ensure that (within a pattern group) all lines connect (end-start) in Z
            if (prevLine != null)
            {
                Loc.Z.Val = prevLine.Loc.Z + prevLine.EndPoint.ZOffset;
            }
            string rowAssignment = string.Format("r={0}", row);
            string colAssignment = string.Format("c={0}", col);
            if (!string.IsNullOrEmpty(ZFactor) && CamTableCount > 0)
            {
                double zFactor = U.Formulae(ZFactor, rowAssignment, colAssignment);
                BuildCamTable(zFactor);
            }
            if (!string.IsNullOrEmpty(SpeedFormulae))
            {
                Speed = U.Formulae(SpeedFormulae, rowAssignment, colAssignment);
            }
        }

        ///// <summary>
        ///// Split the GLine into lines defined by Profile entries
        ///// </summary>
        //public void SplitProfiles()
        //{
        //    if (NumProfilePts < 2 || Parent == null)
        //    {
        //        return;
        //    }
        //    // This line will split into multiple lines
        //    // Last profile point remains as the end point
        //    GProfileChange[] profilePts = ProfilePts;
        //    ProfilePts = new GProfileChange[] { profilePts[0] };
        //    double prevDistance = profilePts[0].Distance;
        //    GLine prevLine = this;

        //    for (int i = 1; i < profilePts.Length; i++)
        //    {
        //        // Create second, third, fourth, etc
        //        GLine newLine = Activator.CreateInstance(GetType(), string.Format("{0}_{1}", Name, i)) as GLine;
        //        newLine.ProfilePts = new GProfileChange[] { profilePts[i] };
        //        profilePts[i].Initialize(newLine);
        //        newLine.Loc = ExtendPoint(prevDistance);
        //        newLine.Loc.Z.Val = prevLine.Loc.Z + prevLine.EndPoint.ZChange;
        //        double thisDist = profilePts[i].Distance;
        //        profilePts[i].Distance = thisDist - prevDistance;
        //        prevDistance = thisDist;
        //        Parent.AppendAt(prevLine, newLine);
        //        prevLine = newLine;
        //    }
        //}

        class TrigGrp
        {
            public Millimeters Start { get; set; }
            public Millimeters End { get; set; }
        }

        public void AutoFillTriggers(int numDots)
        {            
            int minDots = 1;
            if (numDots > 0)            
            {
                List<TrigGrp> trGrps = new List<TrigGrp>();
                TrigGrp trCurGrp = null;
                foreach (GTriggerPoint tPt in _listTriggerPts)
                {
                    tPt.LeadTime = 0.0;
                    Millimeters thisDist = new Millimeters(tPt.Distance.Val);
                    if (trCurGrp == null)
                    {
                        trCurGrp = new TrigGrp();
                        trGrps.Add(trCurGrp);
                    }
                    if (tPt.Anchor && minDots == 1)
                    {
                        minDots = 2;
                    }
                    if (tPt.Anchor && trCurGrp.Start != null)
                    {
                        // End of a group.  Signal to start a new one
                        trCurGrp.End = thisDist;
                        trCurGrp = null;
                    }
                    else
                    {
                        // Contiue to add to a group
                        if (trCurGrp.Start == null)
                        {
                            trCurGrp.Start = thisDist;
                        }
                        trCurGrp.End = thisDist;
                    }
                }

                if (trGrps.Count == 0)
                {
                    trGrps.Add(new TrigGrp() { Start = 0.0, End = XYLength });
                }
                // Handle case for not enough dots
                int numEachGrp = Math.Max(minDots, numDots / trGrps.Count);
                numDots = numEachGrp * trGrps.Count;
                int iTrg = 0;
                foreach(TrigGrp trGrp in trGrps)
                {
                    double gap = (trGrp.End - trGrp.Start) / (numEachGrp - 1);
                    for (int i=0; i < numEachGrp; i++, iTrg++)
                    {
                        bool bAnchor = i == 0 || (i == numEachGrp - 1);
                        double thisDist = double.IsInfinity(gap) || double.IsNaN(gap) ? XYLength / 2.0 : (double)(gap * i + trGrp.Start);
                        if (iTrg < _listTriggerPts.Count)
                        {
                            _listTriggerPts[iTrg].Distance.Val = thisDist;
                            _listTriggerPts[iTrg].Anchor = bAnchor;
                        }
                        else
                        {
                            GTriggerPoint newTriggerPt = CreateTrigger();
                            newTriggerPt.Distance.Val = thisDist;
                            newTriggerPt.Anchor = bAnchor;
                            _listTriggerPts.Add(newTriggerPt);
                            newTriggerPt.Initialize(this);
                        }
                    }
                }
            }
            // Get rid of excess
            while ( _listTriggerPts.Count > numDots)
            {
                RemoveTrigger(_listTriggerPts[numDots]);
                
            }
            U.LogChange(string.Format("{0}.AutoFillTriggers", Nickname));
            SortTriggers();
            FireOnChangedEvent();
        }

        public void MoveTriggerPt(GTriggerPoint TPt, double dDistAdjust)
        {
            int iTrigger = _listTriggerPts.IndexOf(TPt);
            double proposedDist = TPt.Distance + dDistAdjust;

            if (dDistAdjust < 0.0)
            {
                if (iTrigger > 0)
                {
                    // Check that movement is not before previous trigger
                    if (proposedDist < _listTriggerPts[iTrigger - 1].Distance)
                    {
                        TPt.Distance.Val = _listTriggerPts[iTrigger - 1].Distance + 0.001;
                        return;
                    }
                }
            }
            else
            {
                if (iTrigger < _listTriggerPts.Count-1)
                {
                    // Check that movement is not after next trigger
                    if (proposedDist > _listTriggerPts[iTrigger + 1].Distance)
                    {
                        TPt.Distance.Val = _listTriggerPts[iTrigger + 1].Distance - 0.001;
                        return;
                    }
                }
            }
            TPt.Distance.Val = proposedDist;
        }


        /// <summary>
        /// Get the Tool Delay from the tool being moved
        /// </summary>
        /// <returns></returns>
        public virtual Microseconds GetToolDelay()
        {
            return 0.0;
        }

        /// <summary>
        /// Get the absolute robot coordinates for the Loc Used for Begin
        /// Tool Offset and trajectory leads are applied automatically
        /// </summary>
        /// <param name="distance">Distance along the Loc vector (angle of Yaw) </param>
        /// <returns></returns>
        public G3DPoint GetRobotLoc(Millimeters distance)
        {
            return GetRobotLoc(distance, true, true);
        }
        /// <summary>
        /// Get the absolute robot coordinates for the Loc Used for Begin
        /// </summary>
        /// <param name="applyToolOffset"> Add the tool offset to adjust for tool moounting position</param>
        /// <returns></returns>
        public override G3DPoint GetRobotLoc(bool applyToolOffset)
        {
            return GetRobotLoc(0.0, applyToolOffset, true);
        }
        ///// <summary>
        ///// Get the absolute robot coordinates (Used for End point)
        ///// </summary>
        ///// <param name="distance">Distance along the Loc vector (angle of Yaw) </param>
        ///// <param name="applyToolOffset"> Add the tool offset to adjust for tool moounting position</param>
        ///// <param name="applyTrajLead">Subtract to the distance according to the delay and speed.</param>
        ///// <returns></returns>
        //public G3DPoint GetRobotLoc(Millimeters distance, bool applyToolOffset, bool applyTrajLead)
        //{
        //    return GetRobotLoc(distance, applyToolOffset, applyTrajLead, Speed);
        //}
        /// <summary>
        /// Get the absolute robot coordinates (Used for Trigger Point)
        /// </summary>
        /// <param name="distance">Distance along the Loc vector (angle of Yaw) </param>
        /// <param name="applyToolOffset"> Ad the too offset to adjust for tool moounting position</param>
        /// <param name="applyTrajLead">Subtract to the distance according to the delay and speed.</param>
        /// <returns></returns>
        public G3DPoint GetRobotLoc(Millimeters distance, bool applyToolOffset, bool applyTrajLead)
        {

            MillimetersPerSecond speed = Speed;

            if (applyTrajLead)
            {
                distance -= speed * GetToolDelay().ToSeconds;
            }
            return GetRobotLoc(distance, applyToolOffset);
        }
        /// <summary>
        /// Convert the location to pixel space, (Used for TriggerPts)
        /// </summary>
        /// <param name="applyTrajLead"></param>
        public PointF ToPixel(Millimeters distance, bool applyToolOffset, bool applyTrajLead)
        {
            return AbsPointToPixel(GetRobotLoc(distance, applyToolOffset, applyTrajLead));
        }

        /// <summary>
        /// Return the Relative point from the line origin
        /// With no speed arg, we can only be looking at Endpoint
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="applyToolOffset"></param>
        /// <param name="applyTrajLead"></param>
        /// <returns></returns>
        public PointF RelVectorPoint(Millimeters distance, bool applyToolOffset, bool applyTrajLead)
        {
            float X = (float)(distance * Math.Cos(Loc.Yaw));
            float Y = (float)(distance * Math.Sin(Loc.Yaw));
            return new PointF(X, Y);
        }

        /// <summary>
        /// Get extended point in XY
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public G3DPoint ExtendPoint(Millimeters distance)
        {
            G3DPoint pt = new G3DPoint(Loc);
            //double cos = Math.Cos(Loc.Yaw);
            //double sin = Math.Sin(Loc.Yaw);
            //double cosx = distance * cos;
            //double siny = distance * sin;
            //double x = pt.X + cosx;
            //double y = pt.Y + siny;
            //pt.X = x;
            //pt.Y = y;
            pt.X = pt.X + distance * Math.Cos(Loc.Yaw);
            pt.Y = pt.Y + distance * Math.Sin(Loc.Yaw);
            
            return pt;
        }

        /// <summary>
        /// Called after a child trigger has completed successfuly
        /// </summary>
        /// <param name="trigger"></param>
        public virtual void TriggerCompleted(GTriggerPoint trigger)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("TriggerComplete for {0}", Nickname));
            if (LastInWorkpiece != null && Object.ReferenceEquals(trigger, _listTriggerPts[NumTriggerPts - 1]))
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("TriggerComplete LastInWorkPiece Set for {0}", Name));
                LastInWorkpiece.Set();
            }
        }

        /// <summary>
        /// Create a trigger suitable for this line
        /// </summary>
        /// <returns></returns>
        public virtual GTriggerPoint CreateTrigger()
        {
            return new GTriggerPoint(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerPt"></param>
        public void RemoveTrigger(GTriggerPoint triggerPt)
        {
            if (_listTriggerPts.Contains(triggerPt))
            {
                _listTriggerPts.Remove(triggerPt);
                triggerPt.Dispose();
            }
        }

        /// <summary>
        /// Snap the line begin and endpoints to the grid
        /// </summary>
        /// <param name="gridSize"></param>
        public void Snap(Millimeters gridSize)
        {
            G3DPoint ptEnd = ExtendPoint(EndPoint.Distance);
            Loc.X = U.Snap(Loc.X, gridSize);
            Loc.Y = U.Snap(Loc.Y, gridSize);
            ptEnd.X = U.Snap(ptEnd.X, gridSize);
            ptEnd.Y = U.Snap(ptEnd.Y, gridSize);
            PointF ptLoc = Loc.ToPointF;
            PointF ptfEnd = ptEnd.ToPointF;
            PointF ptDiff = U.Subtract(ptfEnd, ptLoc);
            EndPoint.Distance = U.GetDistance(ptDiff);
            Loc.Yaw = U.GetAngle(ptDiff);
        }

        /// <summary>
        /// Snap all triggers to the grid
        /// </summary>
        /// <param name="gridSize"></param>
        public void SnapTriggers(Millimeters gridSize)
        {
            foreach (GTriggerPoint triggerPt in _listTriggerPts)
            {
                triggerPt.Distance = U.Snap(triggerPt.Distance, gridSize);
            }

        }

        /// <summary>
        /// Ensure minimum spacing
        /// </summary>
        /// <param name="spacing"></param>
        public void MinSpacingTriggers(Millimeters spacing)
        {
            Millimeters prevPt = -spacing;
            SortTriggers();
            foreach (GTriggerPoint triggerPt in _listTriggerPts)
            {
                if ((triggerPt.Distance - prevPt) < spacing)
                {
                    triggerPt.Distance = prevPt + spacing;
                }
                prevPt = triggerPt.Distance;
            }

        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerPt"></param>
        public void AddTrigger(GTriggerPoint triggerPt)
        {
            if (!_listTriggerPts.Contains(triggerPt))
            {
                triggerPt.Initialize(this);
                _listTriggerPts.Add(triggerPt);
                SortTriggers();
            }
        }

        public void SortTriggers()
        {
            _listTriggerPts.Sort();
        }

    }
}
