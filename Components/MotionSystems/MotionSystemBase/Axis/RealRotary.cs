using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;
using MCore.Comp;
using MDouble;

using MCore.Comp.IOSystem.Input;

namespace MCore.Comp.MotionSystem
{
    public class RealRotary : RotaryBase
    {
        private MotionSystemBase _motionSystem = null;
        // Used for controllers to wait for move completion
        // Same as Reset (Blocks the thread)
        private ManualResetEvent _waitMoveDone = new ManualResetEvent(false);

        #region Public Browsable Properties
        /// <summary>
        /// Default Speed
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        public DegreesPerSecond DefaultSpeed
        {
            get { return GetPropValue(() => DefaultSpeed, 10); }
            set { SetPropValue(() => DefaultSpeed, value); }
        }
        /// <summary>
        /// Axis no
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        public int AxisNo
        {
            get { return GetPropValue(() => AxisNo, 0); }
            set { SetPropValue(() => AxisNo, value); }
        }
        /// <summary>
        /// The approximate accel/decel
        /// </summary>
        [Browsable(true)]
        [Category("Rotary")]
        [Description("The approximate accel/decel")]
        public DegreesPerSecond AccelDecel
        {
            get { return GetPropValue(() => AccelDecel, 625); }
            set { SetPropValue(() => AccelDecel, value); }
        }

        /// <summary>
        /// The method used to home
        /// </summary>
        [Category("Rotary"), Description("The method used to home"), Browsable(true)]
        public int HomeMethod
        {
            get { return GetPropValue(() => HomeMethod, 0); }
            set { SetPropValue(() => HomeMethod, value); }
        }
        /// <summary>
        /// Enabled
        /// </summary>
        [Browsable(true)]
        [Category("Status")]
        [XmlIgnore]
        public bool Homed
        {
            get { return GetPropValue(() => Homed, false); }
            set { SetPropValue(() => Homed, value); }
        }

        private void OnEnabledChanged(bool enabled)
        {
            if (_motionSystem != null)
                _motionSystem.EnableAxis(this, Enabled);
            Homed = false;
        }

        /// <summary>
        /// PSO Count
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        public int PSOCount
        {
            get { return GetPropValue(() => PSOCount, 0); }
            set { SetPropValue(() => PSOCount, value); }
        }

        /// <summary>
        /// Move Done (in Position)
        /// </summary>
        [Browsable(true)]
        [Category("Status")]
        [ReadOnly(true)]
        [XmlIgnore]
        public bool MoveDone
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => MoveDone); }
            set { SetPropValue(() => MoveDone, value); }
        }

        /// <summary>
        /// The fault description
        /// </summary>
        [Browsable(true)]
        [Category("Axis")]
        [Description("The fault description")]
        [XmlIgnore]
        public string FaultDescription
        {
            get { return GetPropValue(() => FaultDescription); }
            set { SetPropValue(() => FaultDescription, value); }
        }
        #endregion Public Browsable Properties

        #region Public Non-Browsable Properties

        /// <summary>
        /// Delegate for RealRotary
        /// </summary>
        /// <param name="axis"></param>
        public delegate void delVoid_RealRotary(RealRotary axis);


        [Browsable(false)]
        public ManualResetEvent WaitMoveDone
        {
            get { return _waitMoveDone; }
        }

        #endregion Public Non-Browsable Properties


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public RealRotary()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public RealRotary(string name)
            : base(name)
        {            
        }

        #endregion Constructors

        /// <summary>
        /// Initialize the axis
        /// </summary>
        public override void Initialize()
        {
            IgnorePageList.Add(typeof(RotaryBasePage));
            base.Initialize();
            Enabled = false;
            Homed = false;
            RegisterOnChanged(() => Enabled, OnEnabledChanged);
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
                throw new MCoreExceptionPopup("The '{0}' axis must have a MotionSystemBase parent", Name);
            }
        }

        /// <summary>
        /// Home the axis and wait for completion
        /// </summary>
        [StateMachineEnabled]
        public void Home()
        {
            if (!Enabled)
            {
                U.LogPopup("Please Enable the axis first");
            }
            else
            {
                this.TargetMotorCounts = 0;
                if (_motionSystem != null)
                    _motionSystem.HomeAxis(this);
            }
        }

        /// <summary>
        /// Home the axis and don't wait for completion
        /// </summary>
        [StateMachineEnabled]
        public void HomeAsync()
        {
            U.AsyncCall(Home);
        }

        /// <summary>
        /// Enable the axis
        /// </summary>
        [StateMachineEnabled]
        public void Enable()
        {
            Enabled = true;
        }

        /// <summary>
        /// Enable the axis
        /// </summary>
        [StateMachineEnabled]
        public void Disable()
        {
            Enabled = false;
        }


        [StateMachineEnabled]
        public override void MoveScale(double scale)
        {
            MoveAbs(Range * scale + MinLimit);
        }

        /// <summary>
        /// Move to the absolute popsition at the default
        /// </summary>
        /// <param name="position"></param>
        public override void MoveAbs(Degrees position)
        {
            MoveAbs(position, DefaultSpeed);            
        }

        /// <summary>
        /// Move to the absolute popsition at the specified speed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <param name="waitForCompletion"></param>
        public void MoveAbs(Degrees position, DegreesPerSecond speed)
        {
            MoveAbs(position, speed, true);
        }

        /// <summary>
        /// Move to the absolute popsition at the specified speed
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <param name="waitForCompletion"></param>
        public override void MoveAbs(Degrees position, DegreesPerSecond speed, bool waitForCompletion)
        {
            if (!Homed && !_motionSystem.Simulated)
            {
                U.LogPopup("Please Home the axis first");
                return;
            }
            MoveDone = false;
            // Set tgt motor counts
            base.MoveAbs(position);
            // Now do the move
            _motionSystem.MoveAbsAxis(this, speed, waitForCompletion);
        }
        #region Simulate functions

        /// <summary>
        /// Simulate MoveAbs command
        /// </summary>
        /// <param name="speed"></param>
        public virtual void SimulateMoveAbs(MRotarySpeed speed)
        {
            CurrentMotorCounts = TargetMotorCounts;
        }

        /// <summary>
        /// Simulate Home command
        /// </summary>
        public virtual void SimulateHome()
        {
            TargetMotorCounts = 0.0;
            CurrentMotorCounts = 0.0;
            Homed = true;
        }

        #endregion Simulate functions

        /// <summary>
        /// Stop this axis if moving
        /// </summary>
        public virtual void StopAxis()
        {
            _motionSystem.StopAxis(this);
        }

        /// <summary>
        /// Locate the position where the boolean toggles and stop at that point
        /// </summary>
        /// <param name="boolInput"></param>
        /// <param name="tolerance"></param>
        /// <param name="speed"></param>
        /// <returns>Returns the position of the flag</returns>
        public Degrees FindSignalPos(BoolInput boolInput, Degrees tolerance, DegreesPerSecond speed)
        {
            if (boolInput == null)
            {
                U.LogPopup("Expected to find BoolInput in FindSignalPos of {0}}'", Nickname);
            }
            else
            {
                FindSignalChange(boolInput, tolerance, speed);
                FindSignalChange(boolInput, -tolerance, speed/2);
            }
            return CurrentPosition;
        }
        private void FindSignalChange(BoolInput boolInput, Degrees distance, DegreesPerSecond speed)
        {
            bool origVal = boolInput.Value;

            //System.Diagnostics.Debug.WriteLine(string.Format("About to move axis at {0}, {1}, {2}, {3}", TargetPosition, CurrentPosition, InPosition, boolInput.Value));
            MoveRel(distance, speed, false);
            U.SleepWithEvents(200);
            while (origVal == boolInput.Value)
            {
                //System.Diagnostics.Debug.WriteLine(string.Format("Moving axis at {0}, {1}, {2}, {3}", TargetPosition, CurrentPosition, InPosition, boolInput.Value));
                if (MoveDone)
                {
                    StopAxis();
                    throw new MCoreExceptionPopup("Error.  Expected '{0}' signal to change.", boolInput.Nickname);
                }
                U.SleepWithEvents(20);
            }
            //System.Diagnostics.Debug.WriteLine(string.Format("Stopping axis at {0}, {1}, {2}, {3}", TargetPosition, CurrentPosition, InPosition, boolInput.Value));
            StopAxis();
            U.SleepWithEvents(1000);
            //System.Diagnostics.Debug.WriteLine(string.Format("Axis Stopped at  {0}, {1}, {2}, {3}", TargetPosition, CurrentPosition, InPosition, boolInput.Value));
        }
    }    
}
