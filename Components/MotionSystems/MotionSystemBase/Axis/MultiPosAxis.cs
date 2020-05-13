using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MDouble;
using MCore.Comp.MotionSystem;

namespace MCore.Comp.MotionSystem.Axis
{
    public  class MultiPosAxis : RealAxis
    {

        private Millimeters _posA = new Millimeters();
        private Millimeters _posB = new Millimeters();
        private Millimeters _posC = new Millimeters();
        private Millimeters _posD = new Millimeters();
        private Millimeters _posE = new Millimeters();
        private Millimeters _posF = new Millimeters();

        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare A")]
        public virtual Millimeters PositionA
        {
            get { return GetPropValue(() => PositionA, 0); }
            set { SetPropValue(() => PositionA, value); }
        }

        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare B")]
        public virtual Millimeters PositionB
        {
            get { return GetPropValue(() => PositionB, 0); }
            set { SetPropValue(() => PositionB, value); }
        }
        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare C")]
        public virtual Millimeters PositionC
        {
            get { return GetPropValue(() => PositionC, 0); }
            set { SetPropValue(() => PositionC, value); }
        }
        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare D")]
        public virtual Millimeters PositionD
        {
            get { return GetPropValue(() => PositionD, 0); }
            set { SetPropValue(() => PositionD, value); }
        }
        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare E")]
        public virtual Millimeters PositionE
        {
            get { return GetPropValue(() => PositionE, 0); }
            set { SetPropValue(() => PositionE, value); }
        }
        [Browsable(true)]
        [Category("Positions")]
        [DisplayName("Spare F")]
        public virtual Millimeters PositionF
        {
            get { return GetPropValue(() => PositionF, 0); }
            set { SetPropValue(() => PositionF, value); }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MultiPosAxis()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MultiPosAxis(string name)
            : base(name)
        {            
        }

        #endregion Constructors

        /// <summary>
        /// Move to Position A (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("Position Cmd")]
        public virtual bool AtPositionA
        {
            get
            {
                return CurrentPosition == PositionA;
            }
        }
        /// <summary>
        /// Move to Position B (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("Position Cmd")]
        public virtual bool AtPositionB
        {
            get
            {
                return CurrentPosition == PositionB;
            }
        }

        /// <summary>
        /// Move to Position C (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("At Positions?")]
        public virtual bool AtPositionC
        {
            get
            {
                return CurrentPosition == PositionC;
            }
        }
        /// <summary>
        /// Move to Position D (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("Position Cmd")]
        public virtual bool AtPositionD
        {
            get
            {
                return CurrentPosition == PositionD;
            }
        }
        /// <summary>
        /// Move to Position E (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("Position Cmd")]
        public virtual bool AtPositionE
        {
            get
            {
                return CurrentPosition == PositionE;
            }
        }
        /// <summary>
        /// Move to Position f (set) or determine if at Position (get)
        /// </summary>
        [Browsable(true)]
        [Category("Position Cmd")]
        public virtual bool AtPositionF
        {
            get
            {
                return CurrentPosition == PositionF;
            }
        }
        /// <summary>
        /// Teach position A
        /// </summary>
        public void TeachPositionA()
        {
            PositionA = CurrentPosition;
        }
        /// <summary>
        /// Teach position B
        /// </summary>
        public void TeachPositionB()
        {
            PositionB = CurrentPosition;
        }
        /// <summary>
        /// Teach position C
        /// </summary>
        public void TeachPositionC()
        {
            PositionC = CurrentPosition;
        }
        /// <summary>
        /// Teach position D
        /// </summary>
        public void TeachPositionD()
        {
            PositionD = CurrentPosition;
        }
        /// <summary>
        /// Teach position E
        /// </summary>
        public void TeachPositionE()
        {
            PositionE = CurrentPosition;
        }
        /// <summary>
        /// Teach position F
        /// </summary>
        public void TeachPositionF()
        {
            PositionF = CurrentPosition;
        }
    }
}
