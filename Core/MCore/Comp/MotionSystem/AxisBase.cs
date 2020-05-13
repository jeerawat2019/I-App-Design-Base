using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;
using MDouble;

namespace MCore.Comp.MotionSystem
{
    public abstract class AxisBase : CompBase
    {

        #region Public Browsable Properties

        /// <summary>
        /// Target Motor Counts used for moving or homing
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [XmlIgnore]
        public double TargetMotorCounts
        {
            get { return GetPropValue(() => TargetMotorCounts, 0); }
            set { SetPropValue(() => TargetMotorCounts, value); }
        }

        /// <summary>
        /// Text to describe positive direction
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        public string PositiveText
        {
            get { return GetPropValue(() => PositiveText, string.Empty); }
            set { SetPropValue(() => PositiveText, value); }
        }

        /// <summary>
        /// Text to describe Negative direction
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        public string NegativeText
        {
            get { return GetPropValue(() => NegativeText, string.Empty); }
            set { SetPropValue(() => NegativeText, value); }
        }

        /// <summary>
        /// Returns true if Ok to move
        /// </summary>
        [Browsable(true)]
        [Category("Status")]
        [XmlIgnore]
        public bool CanMove
        {
            get { return GetPropValue(() => CanMove, true); }
            set { SetPropValue(() => CanMove, value); }
        }


        /// <summary>
        /// Current Motor Counts
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [ReadOnly(true)]
        [XmlIgnore]
        public virtual double CurrentMotorCounts
        {
            get { return GetPropValue(() => CurrentMotorCounts, 0); }
            set 
            { 
                SetPropValue(() => CurrentMotorCounts, value);
                SetPropValue(() => CurrentPosition, CurrentPosition);
            }
        }

        /// <summary>
        /// Current Axis Position
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [ReadOnly(true)]
        public virtual Millimeters CurrentPosition
        {
            get { return FromMotorCounts(CurrentMotorCounts); }
        }

        /// <summary>
        /// Current Axis Velocity
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [ReadOnly(true)]
        public virtual MillimetersPerSecond CurrentVelocity
        {
            get { return GetPropValue(() => CurrentVelocity, 0); }
            set { SetPropValue(() => CurrentVelocity, value); }
        }

        /// <summary>
        /// Text to describe positive direction
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [ReadOnly(true)]
        public Millimeters TargetPosition
        {
            get { return FromMotorCounts(TargetMotorCounts); }
        }

        /// <summary>
        /// Minimum Limit of travel
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        public Millimeters MinLimit
        {
            get { return GetPropValue(() => MinLimit, new Millimeters()); }
            set { SetPropValue(() => MinLimit, value); }
        }
        /// <summary>
        /// Maximum Limit of travel
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        public Millimeters MaxLimit
        {
            get { return GetPropValue(() => MaxLimit, new Millimeters()); }
            set { SetPropValue(() => MaxLimit, value); }
        }
        /// <summary>
        /// Maximum Limit of travel
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [Description("Motor counts per millimeter")]
        public double MotorCountsPerMM
        {
            get { return GetPropValue(() => MotorCountsPerMM, 100.0); }
            set { SetPropValue(() => MotorCountsPerMM, value); }
        }

        /// <summary>
        /// Range of of travel
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [Description("Range of motion as defined by MaxLimit and MinLimit")]
        public virtual Millimeters Range
        {
            get
            {
                return MaxLimit - MinLimit;
            }
        }
        #endregion Public Browsable Properties

        #region Public Non-Browsable Properties

        #endregion Public Non-Browsable Properties


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public AxisBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public AxisBase(string name)
            : base(name)
        {            
        }

        #endregion Constructors

        /// <summary>
        /// Initialize the axis
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.TargetMotorCounts = this.CurrentMotorCounts;
        }

        /// <summary>
        /// Set the motor counts given a position
        /// </summary>
        /// <param name="ml"></param>
        /// <returns></returns>
        public virtual void SetCurrentPosition(Millimeters mm)
        {
            CurrentMotorCounts = mm.Val * MotorCountsPerMM;
        }

        /// <summary>
        /// Get the motor counts given the length
        /// </summary>
        /// <param name="ml"></param>
        /// <returns></returns>
        public virtual double ToMotorCounts(MLength ml)
        {
            Millimeters mm = new Millimeters(ml);
            return Math.Round(mm.Val * MotorCountsPerMM);
        }

        /// <summary>
        /// Get the generic (um) given the motor counts
        /// </summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        public virtual Millimeters FromMotorCounts(double motorCounts)
        {
            return new Millimeters(motorCounts / MotorCountsPerMM);
        }

        /// <summary>
        /// Set the Target which sets the motor counts
        /// </summary>
        /// <param name="position"></param>
        public void SetTarget(Millimeters position)
        {
            position = Math.Min(position, MaxLimit);
            position = Math.Max(position, MinLimit);
            this.TargetMotorCounts = ToMotorCounts(position);
        }

        /// <summary>
        /// Move to the absolute popsition at the default
        /// </summary>
        /// <param name="position"></param>
        [StateMachineEnabled]
        public virtual void MoveAbs(Millimeters position)
        {
            SetTarget(position);
        }

        /// <summary>
        /// Move to the absolute popsition at the specified speed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        public virtual void MoveAbs(Millimeters position, MillimetersPerSecond speed, bool waitForCompletion)
        {
            SetTarget(position);
        }

        public abstract void MoveScale(double scale);

        [StateMachineEnabled]
        public virtual void MoveRel(Millimeters position)
        {
            Millimeters mAbsPos = CurrentPosition + position;
            MoveAbs(mAbsPos);
        }

        public virtual void MoveRel(Millimeters position, MillimetersPerSecond speed, bool waitForCompletion)
        {
            Millimeters mAbsPos = CurrentPosition + position;
            MoveAbs(mAbsPos, speed, waitForCompletion);
        }

        #region Simulate functions


        #endregion Simulate functions
    }    
}
