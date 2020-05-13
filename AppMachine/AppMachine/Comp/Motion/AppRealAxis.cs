using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using MCore;
using MDouble;
using MCore.Comp.MotionSystem;
using MCore.Comp.IOSystem.Input;

using AppMachine.Comp.IO;

namespace AppMachine.Comp.Motion
{
    public class AppRealAxis:RealAxis
    {
        protected Inputs safetyInput = new Inputs("Safety Input");
        protected MotionSystemBase motionSystem = null;

        [XmlIgnore]
        [Category("Axis"), Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Inputs SafetyInputs
        {
            get { return safetyInput; }
            set { safetyInput = value; }
        }


        [Category("Axis"), Browsable(true)]
        public MDouble.Millimeters JogStep
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => JogStep,1); }
            [StateMachineEnabled]
            set { SetPropValue(() => JogStep, value); }
        }



        #region Constructor
        public AppRealAxis()
        {

        }

        public AppRealAxis(string name):base(name)
        {

        }
        #endregion

        [StateMachineEnabled]
        public override void MoveAbs(Millimeters position)
        {
            CheckSafetyInput();
            base.MoveAbs(position);
        }

        [StateMachineEnabled]
        public override void MoveAbs(Millimeters position, MillimetersPerSecond speed, bool waitForCompletion)
        {
            CheckSafetyInput();
            base.MoveAbs(position, speed, waitForCompletion);
        }

        public override void MoveRel(Millimeters position)
        {
            CheckSafetyInput();
            base.MoveRel(position);
        }

        public override void MoveRel(Millimeters position, MillimetersPerSecond speed, bool waitForCompletion)
        {
            CheckSafetyInput();
            base.MoveRel(position, speed, waitForCompletion);
        }

        [StateMachineEnabled]
        public override void Home()
        {
            CheckSafetyInput();
            base.Home();
        }

        public override void HomeAsync()
        {
            CheckSafetyInput();
            base.HomeAsync();
        }


        private void CheckSafetyInput()
        {
            if (SafetyInputs.ChildArray != null)
            {
                foreach (AppSafetyInput safetyInput in SafetyInputs.ChildArray)
                {
                    if (!safetyInput.IsSafety)
                    {
                        String safetyErr = String.Format("Safety Input \"{0}\" is not \"{1}\"", safetyInput.Name, safetyInput.SafetyValue.ToString());
                        throw new Exception(safetyErr);
                    }
                }
            }
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            motionSystem = GetParent<MotionSystemBase>();
        }

        [StateMachineEnabled]
        public override void Reset()
        {
            base.Reset();
            motionSystem.Reset();

        }
       
    }
}
