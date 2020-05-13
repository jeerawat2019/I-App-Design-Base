using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MDouble;

using MCore.Comp;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;

namespace MCore.Comp.IOSystem
{
    public partial class IOSystemBase : CompSystem
    {
        public event ComponentEventHandler OnPollingComplete = null;
        public event ComponentEventHandler OnWorkpieceBegin = null;

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public IOSystemBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public IOSystemBase(string name)
            : base(name)
        {
        }
        #endregion Constructors


        #region Public Calls to do service

        /// <summary>
        /// Fire the polling complete event
        /// </summary>
        public void FirePollingComplete()
        {
            if (OnPollingComplete != null)
            {
                OnPollingComplete(this);
            }
        }
        /// <summary>
        /// Fire the workpiece begin event
        /// </summary>
        public void FireWorkpieceBegin()
        {
            if (OnWorkpieceBegin != null)
            {
                OnWorkpieceBegin(this);
            }
        }
        /// <summary>
        /// Set a digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value"></param>
        public virtual void Set(BoolOutput boolOutput, bool value)
        {
            // Simulation
            boolOutput.Value = value;
        }

        /// <summary>
        /// Set a digital output
        /// </summary>
        /// <param name="boolOutput"></param>
        /// <param name="value">Which direction to pulse</param>
        /// <param name="duration">Length of the Pulse</param>
        public virtual void SetPulse(BoolOutput boolOutput, bool value, Miliseconds duration)
        {
            // Simulation
            boolOutput.Value = value;
            U.SleepWithEvents(duration.ToInt);
            boolOutput.Value = !value;
        }

        /// <summary>
        /// Set a digital output
        /// </summary>
        /// <param name="dblOutput"></param>
        /// <param name="value"></param>
        public virtual void Set(DoubleOutput dblOutput, double value)
        {
            // Simulation
            dblOutput.Value = value;
        }

        /// <summary>
        /// Set a Integer output
        /// </summary>
        /// <param name="intOutput"></param>
        /// <param name="value"></param>
        public virtual void Set(IntOutput intOutput, int value)
        {
            // Simulation
            intOutput.Value = value;
        }

        /// <summary>
        /// Set a milisecond output
        /// </summary>
        /// <param name="msOutput"></param>
        /// <param name="value"></param>
        public virtual void Set(MiliSecOutput msOutput, Miliseconds value)
        {
            // Simulation
            msOutput.Value = value;
        }

        /// <summary>
        /// Trigger the input to set the value
        /// </summary>
        /// <param name="input"></param>
        public virtual void Trigger(CompMeasure input)
        {
            input.TriggerMode = CompMeasure.eTriggerMode.SingleTrigger;

        }
        /// <summary>
        /// Do the measure, trigger and wait for result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="timeout"></param>
        public virtual bool Measure(CompMeasure input, Miliseconds timeout)
        {
            // Simulation
            return true;
        }

        /// <summary>
        /// Stop Measure
        /// </summary>
        /// <param name="input"></param>
        public virtual void StopMeasure(Array2DInput input)
        {
            // Simulation
        }


        /// <summary>
        /// Set Program number
        /// </summary>
        /// <param name="nPrg"></param>
        [StateMachineEnabled]
        public virtual void SetProgram(int nPrg)
        {
        }

        #endregion Public Calls to do service
    }
}
