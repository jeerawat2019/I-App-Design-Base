using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.SMLib
{
    /// <summary>
    /// State Machine Manager
    /// </summary>
    public static class SMMgr
    {
        private static List<SMStateMachine> _listStateMachines = new List<SMStateMachine>();
        /// <summary>
        /// Register a state machine
        /// </summary>
        /// <param name="sm"></param>
        public static void Register(SMStateMachine sm)
        {
            lock (_listStateMachines)
            {
                if (!_listStateMachines.Contains(sm))
                {
                    _listStateMachines.Add(sm);
                    sm.Initialize();
                }
            }
        }
        /// <summary>
        /// Un-Register a state machine
        /// </summary>
        /// <param name="sm"></param>
        public static void UnRegister(SMStateMachine sm)
        {
            lock (_listStateMachines)
            {
                if (_listStateMachines.Contains(sm))
                {
                    _listStateMachines.Remove(sm);
                }
            }
        }
        /// <summary>
        /// Run all state machines
        /// </summary>
        public static void RunAll()
        {
            lock (_listStateMachines)
            {
                foreach (var sm in _listStateMachines)
                {
                    sm.Run();
                }
            }
        }
        /// <summary>
        /// Step all state machines. 
        /// This will move to next state if conditions allow. 
        /// Then perform actions of next state.  
        /// At most, one set of conditions and one set of actions
        /// </summary>
        public static void StepAll()
        {
            lock (_listStateMachines)
            {
                foreach (var sm in _listStateMachines)
                {
                    sm.Step();
                }
            }
        }
        /// <summary>
        /// Stop all state machines.
        /// This will stop only after all actions are completed
        /// </summary>
        public static void StopAll()
        {
            lock (_listStateMachines)
            {
                foreach (var sm in _listStateMachines)
                {
                    sm.Stop();
                }
            }
        }
        /// <summary>
        /// Gaurantees all state machines will terminate
        /// This will stop even if actions are not completed
        /// </summary>
        public static void AbortAll()
        {
            lock (_listStateMachines)
            {
                foreach (var sm in _listStateMachines)
                {
                    sm.Abort();
                }
            }
        }
    }
}
