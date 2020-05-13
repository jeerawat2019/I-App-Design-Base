using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;
using MCore.Comp.Geometry;
using MDouble;

namespace MCore.Comp.MotionSystem
{
    public class RealAxes : AxesBase
    {
        private MotionSystemBase _motionSystem = null;
        private long _startTime = 0;
        private volatile bool _paused = false;
        private string _blendedMoveDetails = string.Empty;
        private G3DPoint _lastBlendedPt = new G3DPoint();
        #region Public Browsable Properties


        [XmlIgnore]
        public G3DPoint LastBlendedPt
        {
            get { return _lastBlendedPt; }
        }

        /// <summary>
        /// The default safe ZHt for robot transfer
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("The default safe ZHt for robot transfer")]
        public Millimeters SafeZHeight
        {
            get { return GetPropValue(() => SafeZHeight, 0); }
            set { SetPropValue(() => SafeZHeight, value); }
        }

        /// <summary>
        /// The approximate accel/decel of a contoured move
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("The approximate accel/decel of a contoured move")]
        public MillimetersPerSecond AccelDecel
        {
            get { return GetPropValue(() => AccelDecel, 1360); }
            set { SetPropValue(() => AccelDecel, value); }
        }

        /// <summary>
        /// The limit of change to blended velocities
        /// </summary>
        /// <remarks>Scale factor (2) means can only change by factor of 2 (or 1/2)</remarks>
        [Browsable(true)]
        [Category("Axes")]
        [Description("The limit of change to blended velocities")]
        public MDoubleNoUnits VelChangeFactorLimit
        {
            get { return GetPropValue(() => VelChangeFactorLimit, 2); }
            set { SetPropValue(() => VelChangeFactorLimit, value); }
        }

        /// <summary>
        /// The maximum response time for an axis
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("The maximum response time for an axis")]
        public Miliseconds AxisResponseTime
        {
            get { return GetPropValue(() => AxisResponseTime, 75); }
            set { SetPropValue(() => AxisResponseTime, value); }
        }


        /// <summary>
        /// Get a reference to the motion system
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public MotionSystemBase RefMotionSystem
        {
            get { return _motionSystem; }
        }


        /// <summary>
        /// For temp use
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool TempMoveDone
        {
            get;
            set;
        }

        /// <summary>
        /// For temp use
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool QueueEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// Flag if all axis are MoveDone
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("Flag if all axis are MoveDone")]
        [XmlIgnore]
        public bool MoveDone
        {
            get { return GetPropValue(() => MoveDone, true); }
            set { SetPropValue(() => MoveDone, value); }
        }

        /// <summary>
        /// Get the X RealAxis
        /// </summary>
        [Browsable(false)]
        public RealAxis AxisX
        {
            get 
            {
                return FilterSingle<RealAxis>((c) => (c is RealAxis) && (c as RealAxis).IsXDir);
            }
        }

        /// <summary>
        /// Get the X RealAxis
        /// </summary>
        [Browsable(false)]
        public RealAxis AxisY
        {
            get
            {
                return FilterSingle<RealAxis>((c) => (c is RealAxis) && (c as RealAxis).IsYDir);
            }
        }

        /// <summary>
        /// Get the X RealAxis
        /// </summary>
        [Browsable(false)]
        public RealAxis AxisZ
        {
            get
            {
                return FilterSingle<RealAxis>((c) => (c is RealAxis) && (c as RealAxis).IsZDir);
            }
        }

        /// <summary>
        /// Details of blended
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public string BlendedMoveDetails
        {
            get { return _blendedMoveDetails; }
            set { _blendedMoveDetails = value; }
        }
        /// <summary>
        /// Current PSOPulseOnOffTime in micro sec
        /// </summary>
        [Browsable(true)]
        [Category("RealAxis")]
        [Description("Current PSO Pulse ON+OFF Time")]
        [XmlIgnore]
        public double CurrentPSOPulseOnOffTime
        {
            get { return GetPropValue(() => CurrentPSOPulseOnOffTime, 4000); }
            set { SetPropValue(() => CurrentPSOPulseOnOffTime, value); }
        }

        /// <summary>
        /// Current PSOPulseOnTime in micro sec
        /// </summary>
        [Browsable(true)]
        [Category("RealAxis")]
        [Description("Current PSO Pulse ON Time")]
        [XmlIgnore]
        public double CurrentPSOPulseOnTime
        {
            get { return GetPropValue(() => CurrentPSOPulseOnTime, 2500); }
            set { SetPropValue(() => CurrentPSOPulseOnTime, value); }
        }

        /// <summary>
        /// True if PSO is dual
        /// </summary>
        [Browsable(true)]
        [Category("RealAxis")]
        [Description("True if PSO is dual")]
        public bool IsDualPSO
        {
            get { return GetPropValue(() => IsDualPSO); }
            set { SetPropValue(() => IsDualPSO, value); }
        }
        
        /// <summary>
        /// The overall blended move time so far (ms)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public long StartTime
        {
            get { return _startTime; }
            set 
            {
                if (value != 0)
                {
                    Paused = false;
                }
                _startTime = value; 
            }
        }
        /// <summary>
        /// The overall blended move time so far (ms)
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool Paused
        {
            get { return _paused; }
            set { _paused = value; }
        }
        /// <summary>
        /// the Sleep time in MS
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public int NumBlendedMoves
        {
            get { return _numBlendedMoves; }
        }
        /// <summary>
        /// Current Cam Table number
        /// </summary>
        [Browsable(true)]
        [Category("RealAxis")]
        [Description("Current Cam Table number")]
        [XmlIgnore]
        public int CamTableNum
        {
            get { return GetPropValue(() => CamTableNum, -1); }
            set { SetPropValue(() => CamTableNum, value); }
        }
        public void ResetBlendedMove(bool useCurrent)
        {
            StartTime = 0;
            Paused = false;
            _totalBlendedMoveDurationSec = 0.0;
            _prevVel = 0.0;
            _blendedMoveDetails = string.Empty;
            _numBlendedMoves = 0;
            _triggeringEnd = 0;
            if (useCurrent)
            {
                _lastBlendedPt.X.Val = AxisX.CurrentPosition;
                _lastBlendedPt.Y.Val = AxisY.CurrentPosition;
                _lastBlendedPt.Z.Val = AxisZ.CurrentPosition;
            }
            //Debug.WriteLine(string.Format("Reset LastX={0}, LastY={1}", _lastBlendedPt.X.Val, _lastBlendedPt.Y.Val));
        }

        public long SleepUntilTriggeringComplete()
        {
            if (_triggeringEnd == 0)
            {
                return U.SleepWithEvents((int)(_totalBlendedMoveDurationSec * 1000));
            }
            return U.SleepWithEvents(_triggeringEnd);

        }
        /// <summary>
        ///  Sleep until we only have ms time left
        /// </summary>
        /// <param name="ms"></param>
        public int SleepUntilRemaining(int ms)
        {
            if (StartTime == 0)
            {
                return 0;
            }
            long now = U.DateTimeNow;
            long endTime = _startTime + U.SecToTicks(Math.Max(_totalBlendedMoveDurationSec, AxisResponseTime.ToSeconds));
            long remainingDuration = endTime - now - U.MSToTicks(ms);
            if (remainingDuration > 0)
            {
                int msSleep = (int)U.TicksToMS(remainingDuration);
                _blendedMoveDetails += string.Format("Sleep({0})", msSleep);
                // Temp
                //long endSleepTime = now + remainingDuration;
                //do
                //{
                //    if (AxisX.InPosition && AxisY.InPosition && AxisZ.InPosition)
                //    {
                //        int jjb = 09;
                //    }

                //    U.SleepWithEvents(5);
                //} while (U.DateTimeNow < endSleepTime);
                // End temp
                //Debug.WriteLine(string.Format("Sleeping={0}", msSleep));
                U.SleepWithEvents(msSleep);
                return msSleep;
            }
            return 0;
        }

        private double _totalBlendedMoveDurationSec = 0.0;
        private double _prevVel = 0.0;
        private int _numBlendedMoves = 0;
        private int _triggeringEnd = 0;

        public double SetBlendedMoveTime(string name, double[] lineDistance, ref double lineSpeed,bool triggering)
        {
            if (lineDistance.Length < 2 || lineSpeed <= 0)
            {
                //Debug.WriteLine("lineSpeed == 0");
                return 0.0;
            }

            double secXYTravel = 0.0;
            double distX = lineDistance[0] - _lastBlendedPt.X.Val;
            double distY = lineDistance[1] - _lastBlendedPt.Y.Val;
            //double distZ = lineDistance[2] - _lastBlendedPt.Z.Val;

            // Limit Speed chnges
            //double diff = lineSpeed - _prevVel;
            //if (_prevVel != 0 && diff != 0.0)
            //{
            //    double velRatio = lineSpeed / _prevVel;
            //    double velFactor = VelChangeFactorLimit;
            //    if (diff > 0 && velRatio > velFactor)
            //    {
            //        lineSpeed = _prevVel * velFactor;
            //    }
            //    else if (diff < 0 && velRatio < (1.0 / velFactor))
            //    {
            //        lineSpeed = _prevVel / velFactor;
            //    }
            //}
            double distXY2 = distX * distX + distY * distY; // +distZ * distZ;
            if (distXY2 > 0.0)
            {
                double distXY = Math.Sqrt(distXY2);

                double A = AccelDecel;
                double V0 = _prevVel;
                double maxSpeed = lineSpeed;


                double Ta = Math.Abs(maxSpeed - V0) / 2.0 / A;

                double XYa = A * Ta * Ta + V0 * Ta;
                double Tv = 0;
                if (XYa >= distXY)
                {
                    // Will not achieve V
                    // Recompute Ta
                    Ta = (Math.Sqrt(V0 * V0 + 4.0 * A * XYa) - V0) / (2.0 * A);
                    maxSpeed = 2.0 * A * Ta + V0;
                }
                else
                {
                    Ta = Math.Abs(maxSpeed - V0) / (2.0 * A);
                    Tv = Math.Abs(distXY - XYa) / maxSpeed;
                }
                secXYTravel = Ta + Tv;

                double dt = Math.Round(secXYTravel * 1000.0);
                double dta = Math.Round(Ta * 1000.0);
                double dtv = Math.Round(Tv * 1000.0);
                string thisMoveDetails = string.Format("{0} dX={1} dY={2} v={3}({4}+{5}={6}) + ",
                    name, distX.ToString("0.##"), distY.ToString("0.##"), maxSpeed.ToString("0.#"), 
                    dta.ToString("#"),dtv.ToString("#"),dt.ToString("#"));
                _blendedMoveDetails += thisMoveDetails;
                _totalBlendedMoveDurationSec += secXYTravel;
                if (triggering)
                {
                    _triggeringEnd = (int)(_totalBlendedMoveDurationSec * 1000.0);
                }
                //Debug.WriteLine(string.Format("XAbs={0},{1}", lineDistance[0], thisMoveDetails));
                _lastBlendedPt.X.Val = lineDistance[0];
                _lastBlendedPt.Y.Val = lineDistance[1];
                _numBlendedMoves++;
                _prevVel = maxSpeed;
            }
            else
            {
                //Debug.WriteLine("distXY2 == 0");
            }
            return secXYTravel;
        }

        /// <summary>
        /// Get the number of RealAxis
        /// </summary>
        [Browsable(true)]
        [Category("Axes")]
        [Description("Get the number of RealAxis")]
        public int NumAxis
        {
            get { return FilterByType<RealAxis>().Count(); }
        }

        /// <summary>
        /// Get the PSO Primary axis given the key
        /// </summary>
        /// <param name="axisKey"></param>
        /// <returns></returns>
        public RealAxis GetPSOAxis(string axisKey)
        {
            //return FilterSingle<RealAxis>((c) => (c is RealAxis) && (c as RealAxis).PSOAxisKey.Contains(axisKey));
            RealAxis[] axes = this.RecursiveFilterByType<RealAxis>();
            foreach (RealAxis axis in axes)
            {
                if (axis.PSOAxisKey != null)
                {
                    foreach (string s in axis.PSOAxisKey)
                    {
                        if (s.Contains(axisKey))
                        {
                            return axis;
                        }
                    }
                }
            }
            return null;
        }

        #endregion Public Browsable Properties

        #region Public Non-Browsable Properties

        #endregion Public Non-Browsable Properties


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RealAxes()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RealAxes(string name)
            : this(name, 0)
        {            
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RealAxes(string name, int axesNo)
            : base(name, axesNo)
        {
        }

        #endregion Constructors

        #region Override functions
        /// <summary>
        /// Initialize the axis
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _motionSystem = GetParent<MotionSystemBase>();
            if (_motionSystem == null)
            {
                throw new MCoreExceptionPopup("The '{0}' axes must have a MotionSystemBase parent", Name);
            }
        }
        #endregion Override functions

        #region Public functions

        /// <summary>
        /// Move all the patterns in the workpiece
        /// </summary>
        /// <param name="workPiece"></param>
        /// <param name="patList"></param>
        /// <returns>True if completed with no abort</returns>
        public bool MoveWorkPiece(GWorkPiece wp, GPattern[] patList)
        {
            return _motionSystem.MoveWorkPiece(wp, this, patList);
        }

        /// <summary>
        /// Move up safely in Z, then move X and Y, then move to destination in Z
        /// </summary>
        /// <param name="mmX"></param>
        /// <param name="mmY"></param>
        /// <param name="mmZ"></param>
        public void MoveXYSafeZ(Millimeters mmX, Millimeters mmY, Millimeters mmZ)
        {
            // Temp
            //AxisX.MoveAbs(dest.X);
            //AxisY.MoveAbs(dest.Y);

            AxisZ.MoveAbs(SafeZHeight);
            AxisX.SetTarget(mmX);
            AxisY.SetTarget(mmY);
            _motionSystem.MoveLinearXY(this, AxisX.DefaultSpeed);
            AxisZ.MoveAbs(mmZ);
        }

        /// <summary>
        /// Move up safely in Z
        /// </summary>
        [StateMachineEnabled]
        public void MoveSafeZHeight()
        {
            AxisZ.MoveAbs(SafeZHeight);
        }


        /// <summary>
        /// Move up safely in Z, then move X and Y, then move to destination in Z
        /// </summary>
        /// <param name="dest"></param>
        public void MoveXYSafeZ(G3DPoint dest)
        {
            MoveXYSafeZ(dest.X, dest.Y, dest.Z);
        }

        #endregion Public functions
    }    
}
