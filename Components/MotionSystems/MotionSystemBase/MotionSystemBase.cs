using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

using MCore.Comp;
using MCore.Comp.Geometry;
using MCore.Comp.IOSystem;
using MDouble;

namespace MCore.Comp.MotionSystem
{
    public partial class MotionSystemBase : IOSystemBase
    {

        /// <summary>
        ///  PSO Trigger Path Planning
        ///  0: Point by Point with Software Simulate Trigger
        ///  1: Point by Point with PSO HW Trigger
        ///  2: Line by Line with PSO HW Trigger
        ///  3: Pattern blended move with PSO HW Trigger
        ///  4: Not Yet Complete: WorkPiece blended move with PSO HW Trigger
        /// </summary>
        [Browsable(true)]
        [Category("MotionSystem")]
        [Description("PSO Trigger Path Planning\n" +
            "   0: Point by Point with Software Simulate Trigger\n" +
            "   1: Point by Point with PSO HW Trigger\n" +
            "   2: Line by Line with PSO HW Trigger\n" +
            "   3: Pattern blended move with PSO HW Trigger\n" +
            "   4: Not Yet Complete: WorkPiece blended move with PSO HW Trigger")]
        public int PSOTriggerPathPlanning
        {
            get { return GetPropValue(() => PSOTriggerPathPlanning, 4); }
            set { SetPropValue(() => PSOTriggerPathPlanning, value); }
        }

        /// <summary>
        ///  Those Trigger IDs that require Simulated triggers
        /// </summary>
        /// <remarks>Separate with '/'</remarks>
        [Browsable(true)]
        [Category("MotionSystem")]
        [Description("Those Trigger IDs that require Simulated triggers (separated with '/')")]
        public string SimTriggers
        {
            get { return GetPropValue(() => SimTriggers); }
            set { SetPropValue(() => SimTriggers, value); }
        }
        
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MotionSystemBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public MotionSystemBase(string name) 
            : base (name)
        {
        }
        #endregion Constructors


        #region Public Calls to do service

        /// <summary>
        /// Clear all Faluts
        /// </summary>
        [StateMachineEnabled]
        public virtual void ClearAllFaults()
        {
        }
        /// <summary>
        /// EnableAxis an Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public virtual void EnableAxis(RealAxis axis, bool bEnable)
        {
            // Simulation
            axis.Enabled = bEnable;
        }

        /// <summary>
        /// EnableAxis an Axis
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="bEnable"></param>
        public virtual void EnableAxis(RealRotary axis, bool bEnable)
        {
            // Simulation
            axis.Enabled = bEnable;
        }

        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public virtual void HomeAxis(RealAxis axis)
        {
            // Simulation
            axis.SimulateHome();
        }
        /// <summary>
        /// Home an Axis
        /// </summary>
        /// <param name="axis"></param>
        public virtual void HomeAxis(RealRotary axis)
        {
            // Simulation
            axis.SimulateHome();
        }

        /// <summary>
        /// Stop Axis if moving
        /// </summary>
        /// <param name="axis"></param>
        public virtual void StopAxis(RealAxis axis)
        {
            // Simulation
            axis.TargetMotorCounts = axis.CurrentMotorCounts;
        }

        /// <summary>
        /// Stop Axis if moving
        /// </summary>
        /// <param name="axis"></param>
        public virtual void StopAxis(RealRotary axis)
        {
            // Simulation
            axis.TargetMotorCounts = axis.CurrentMotorCounts;
        }

        /// <summary>
        /// Move the axis to an absolute position (stored in CommandedPosition)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        /// <param name="waitForCompletion"></param>
        public virtual void MoveAbsAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            // Simulation
            axis.SimulateMoveAbs(speed);
        }

        /// <summary>
        /// Move the axis to an absolute position (stored in CommandedPosition)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="speed"></param>
        /// <param name="waitForCompletion"></param>
        public virtual void MoveAbsAxis(RealRotary axis, MRotarySpeed speed, bool waitForCompletion)
        {
            // Simulation
            axis.SimulateMoveAbs(speed);
        }


        public virtual void MoveRelAxis(RealAxis axis, MLengthSpeed speed, bool waitForCompletion)
        {
            // Simulation
            axis.SimulateMoveRel(speed);
        }

        /// <summary>
        /// Move the axis to an absolute position (stored in CommandedPosition)
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="speed"></param>
        public virtual void MoveLinearXY(RealAxes axes, MLengthSpeed speed)
        {
            // Simulation
        }


        /// <summary>
        /// Move the axis to an absolute position (stored in CommandedPosition)
        /// </summary>
        /// <param name="axes"></param>
        /// <param name="speed"></param>
        public virtual void MoveLinearXY(AxesBase axes, MLengthSpeed speed)
        {
            // Simulation
        }

        /// <summary>
        /// Move all the patterns in the workpiece
        /// </summary>
        /// <param name="workPiece"></param>
        /// <param name="axes"></param>
        /// <param name="patList"></param>
        /// <returns>True if completed with no abort</returns>
        public virtual bool MoveWorkPiece(GWorkPiece workPiece, RealAxes axes, GPattern[] patList)
        {
            // Simulation

            
            // Do work.   
            // Process one pattern at a time (including the link move to get to the pattern)
            // Return when everything completed
            // or return when interrupted (Check GPattern.AbortExecution flag)

            foreach (GPattern pattern in patList)
            {
                pattern.WaitForReady();
                if (workPiece.AbortRun)
                {
                    return false;
                }
                if (pattern.HasChildren)
                {
                    foreach (GLine line in pattern.Lines)
                    {
                        foreach (GTriggerPoint tPt in line.TriggerPts)
                        {
                            tPt.SimulateTrigger();
                            if (tPt.PulseWidth > 0)
                            {
                                FirePollingComplete();
                                double stopPolling = U.DateTimeNow + tPt.PulseWidth.ToTicks;
                                while (stopPolling >= U.DateTimeNow)
                                {
                                    U.SleepWithEvents(10);
                                    FirePollingComplete();
                                }
                            }
                        }
                    }
                }
            }

            return true;

        }



        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
        }


        #endregion Public Calls to do service


    }
}
