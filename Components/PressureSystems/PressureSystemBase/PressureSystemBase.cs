using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDouble;

using MCore.Comp;

namespace MCore.Comp.PressureSystem
{
    public partial class PressureSystemBase : CompSystem
    {
         #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public PressureSystemBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public PressureSystemBase(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Public Calls to do service

        /// <summary>
        /// Set a Pressure
        /// </summary>
        /// <param name="prChannel"></param>
        /// <param name="kPa"></param>
        public virtual void SetPressure(KPaChannel prChannel, KiloPascal kPa)
        {
            // Simulation
            prChannel.Value = kPa.Val;
        }

        /// <summary>
        /// Update all the parameters
        /// </summary>
        /// <returns>Returns the Pressure</returns>
        public virtual double UpdateAll()
        {
            // Simulation
            return 0.0;
        }




        #endregion Public Calls to do service

    }
}
