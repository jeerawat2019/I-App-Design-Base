using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;


namespace MCore.Comp.PressureSystem
{
    public class KPaChannel : PrChannel
    {
        [Browsable(true)]
        [Category("Pressure Params")]
        public KiloPascal Value
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Value, 0); }
            set { SetPropValue(() => Value, value); }
        }
        /// <summary>
        /// The dispense time in ms
        /// </summary>
        [Browsable(true), Category("Pressure Params"), Description("The dispense time in ms")]
        public Miliseconds DispenseTime
        {
            get { return GetPropValue(() => DispenseTime); }
            set { SetPropValue(() => DispenseTime, value); }
        }

        /// <summary>
        /// The vacuum in kPa
        /// </summary>
        [Browsable(true), Category("Pressure Params"), Description("The vacuum in kPa")]
        public KiloPascal VacuumPressure
        {
            get { return GetPropValue(() => VacuumPressure); }
            set { SetPropValue(() => VacuumPressure, value); }
        }

        /// <summary>
        /// Timed Mode
        /// </summary>
        [Browsable(true), Category("Pressure Params"), Description("Timed Mode")]
        public bool TimedMode
        {
            get { return GetPropValue(() => TimedMode); }
            set { SetPropValue(() => TimedMode, value); }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public KPaChannel()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public KPaChannel(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Set the output 
        /// </summary>
        /// <param name="val"></param>
        public virtual void Set(KiloPascal val)
        {
            if (val != Value)
            {
                PrSystem.SetPressure(this, val);
            }
        }
        /// <summary>
        /// Sesend the last command
        /// </summary>
        public virtual void Resend()
        {
            PrSystem.SetPressure(this, Value);
        }

    }
}
