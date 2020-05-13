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
    public abstract class RotaryBase : CompBase
    {

        #region Public Browsable Properties

        /// <summary>
        /// Target Motor Counts used for moving or homing
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
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
        [Category("Rotary")]
        public string PositiveText
        {
            get { return GetPropValue(() => PositiveText, string.Empty); }
            set { SetPropValue(() => PositiveText, value); }
        }

        /// <summary>
        /// Text to describe Negative direction
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
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
        [Category("Rotary")]
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
        /// Current Rotary Position
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [ReadOnly(true)]
        public virtual Degrees CurrentPosition
        {
            get { return FromMotorCounts(CurrentMotorCounts); }
        }

        /// <summary>
        /// Current Rotary Velocity
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [ReadOnly(true)]
        public virtual DegreesPerSecond CurrentVelocity
        {
            get { return GetPropValue(() => CurrentVelocity, 0); }
            set { SetPropValue(() => CurrentVelocity, value); }
        }

        /// <summary>
        /// Text to describe positive direction
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [ReadOnly(true)]
        public Degrees TargetPosition
        {
            get { return FromMotorCounts(TargetMotorCounts); }
        }

        /// <summary>
        /// Minimum Limit of travel
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        public Degrees MinLimit
        {
            get { return GetPropValue(() => MinLimit, new Degrees()); }
            set { SetPropValue(() => MinLimit, value); }
        }
        /// <summary>
        /// Maximum Limit of travel
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        public Degrees MaxLimit
        {
            get { return GetPropValue(() => MaxLimit, new Degrees()); }
            set { SetPropValue(() => MaxLimit, value); }
        }
        /// <summary>
        /// Motor counts per degree
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [Description("Motor counts per degree")]
        public double MotorCountsPerDeg
        {
            get { return GetPropValue(() => MotorCountsPerDeg, 100.0); }
            set { SetPropValue(() => MotorCountsPerDeg, value); }
        }

        /// <summary>
        /// Range of of travel
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [Description("Range of motion as defined by MaxLimit and MinLimit")]
        public virtual Degrees Range
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
        public RotaryBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RotaryBase(string name)
            : base(name)
        {            
        }

        #endregion Constructors

        /// <summary>
        /// Initialize the Rotary
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
        public virtual void SetCurrentPosition(Degrees deg)
        {
            CurrentMotorCounts = deg.Val * MotorCountsPerDeg;
        }

        /// <summary>
        /// Get the motor counts given the length
        /// </summary>
        /// <param name="ml"></param>
        /// <returns></returns>
        public virtual double ToMotorCounts(MAngle mA)
        {
            return Math.Round(mA.ToDegrees * MotorCountsPerDeg);
        }

        /// <summary>
        /// Get the generic (deg) given the motor counts
        /// </summary>
        /// <param name="motorCounts"></param>
        /// <returns></returns>
        public virtual Degrees FromMotorCounts(double motorCounts)
        {
            return new Degrees(motorCounts / MotorCountsPerDeg);
        }

        /// <summary>
        /// Set the Target which sets the motor counts
        /// </summary>
        /// <param name="position"></param>
        public void SetTarget(Degrees position)
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
        public virtual void MoveAbs(Degrees position)
        {
            SetTarget(position);
        }

        /// <summary>
        /// Move to the absolute popsition at the specified speed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        public virtual void MoveAbs(Degrees position, DegreesPerSecond speed, bool waitForCompletion)
        {
            SetTarget(position);
        }

        public abstract void MoveScale(double scale);

        [StateMachineEnabled]
        public virtual void MoveRel(Degrees position)
        {
            Degrees mAbsPos = CurrentPosition + position;
            MoveAbs(mAbsPos);
        }

        public virtual void MoveRel(Degrees position, DegreesPerSecond speed, bool waitForCompletion)
        {
            Degrees mAbsPos = CurrentPosition + position;
            MoveAbs(mAbsPos, speed, waitForCompletion);
        }

        #region Simulate functions


        #endregion Simulate functions
    }    
}
