using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Threading.Tasks;

//
// MLib using declarations
//
using MCore.Comp.SMLib.Path;
using MDouble;

namespace MCore.Comp.SMLib.Flow
{
    /// <summary>
    /// Class to manage the State Machine Action object
    /// </summary>
    public class SMActTransFlow : SMFlowBase
    {
        private Stopwatch _stopWatch = new Stopwatch();
        private ManualResetEvent _timeoutHandle = new ManualResetEvent(false);


        public bool LoopTransitions
        {
            get { return GetPropValue(() => LoopTransitions,false); }
            set { SetPropValue(() => LoopTransitions, value); }
        }


        /// <summary>
        /// Enable Flow Timeout to Stop Path
        /// </summary>
        public bool FlowTimeoutToStopPath
        {
            get { return GetPropValue(() => FlowTimeoutToStopPath, false); }
            set { SetPropValue(() => FlowTimeoutToStopPath, value); }
        }


        public string TimeOutCaption
        {
            get { return GetPropValue(() => TimeOutCaption); }
            set { SetPropValue(() => TimeOutCaption, value); }
        }

        public string TimeOutMessage
        {
            get { return GetPropValue(() => TimeOutMessage); }
            set { SetPropValue(() => TimeOutMessage, value); }
        }


        public Miliseconds TransTimeOut
        {
            get { return GetPropValue(() => TransTimeOut, 0); }
            set { SetPropValue(() => TransTimeOut, value); }
        }

        public Miliseconds TransLoopTime
        {
            get { return GetPropValue(() => TransLoopTime, 10); }
            set { SetPropValue(() => TransLoopTime, value); }
        }

        /// <summary>
        /// Gets/sets if rebuild is needed
        /// </summary>
        [XmlIgnore]
        public bool NeedsRebuild { get; set; }

        /// <summary>
        /// Gets/sets Has Problem
        /// </summary>
        [XmlIgnore]
        public bool HasProblem
        {
            get { return GetPropValue(() => HasProblem, false); }
            set { SetPropValue(() => HasProblem, value); }
        }


        /// <summary>
        /// Returns true if this object has children
        /// </summary>
        [Browsable(true)]
        [Category("Component Tree")]
        public new bool HasChildren
        {
            [StateMachineEnabled]
            get 
            { 
                return (Count != 0 || TransitionList.Count !=0); 
            }
        }


        /// <summary>
        /// Returns true if this object has method
        /// </summary>
        [XmlIgnore]
        public bool HasMethod
        {
            [StateMachineEnabled]
            get
            {
                return (Count != 0);
            }
        }


        /// <summary>
        /// Returns true if this object has transition
        /// </summary>
        [XmlIgnore]
        public bool HasTransition
        {
            [StateMachineEnabled]
            get
            {
                return (TransitionList.Count != 0);
            }
        }

        //public delegate void DelPrarmVoid();
        //public event DelPrarmVoid evTransitionListOnChanged = null;
        public List<SMTransition> TransitionList = new List<SMTransition>();


        public bool DryRunSkipActions
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => DryRunSkipActions, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => DryRunSkipActions, value); }
        }

        public bool UseDryRunTrans
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => UseDryRunTrans, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => UseDryRunTrans, value); }
        }

        private SMPathOut _dryRunPath = new SMPathOut();

        /// <summary>
        /// The Transition Traget ID
        /// </summary>
        public string DryRunTransitionTargetID
        {
            get { return GetPropValue(() => DryRunTransitionTargetID, string.Empty); }
            set
            {
                if (SetPropValue(() => DryRunTransitionTargetID, value))
                {
                    Rebuild();
                }
            }
        }



        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMActTransFlow()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="text"></param>
        public SMActTransFlow(string text)
            : base(text)
        {
            // Must have a Path in and a path out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutStop(-0.5F);
            this[eDir.Down] = new SMPathOut(0.5F);
            this[eDir.Right] = new SMPathOutError(0.5F);
        }

      
        #endregion

        #region Overrides

        public override void Initialize()
        {
            // Temp
            if (this[typeof(SMPathOutError)] == null)
            {
                // Need an Error path
                if (this[eDir.Right] is SMPathOutPlug)
                    this[eDir.Right] = new SMPathOutError(0.5F);
                else if (this[eDir.Left] is SMPathOutPlug)
                    this[eDir.Left] = new SMPathOutError(-0.5F);
                else if (this[eDir.Down] is SMPathOutPlug)
                    this[eDir.Down] = new SMPathOutError(0.5F);
                else if (this[eDir.Up] is SMPathOutPlug)
                    this[eDir.Up] = new SMPathOutError(-0.5F);
            }
            base.Initialize();
            NeedsRebuild = true;
        }
        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            Rebuild();
        }
        /// <summary>
        /// Use ToString to get underlying content
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = string.Empty;
            text = "ID:" + this.Name + Environment.NewLine;
            if (HasMethod)
            {
                foreach (CompBase comp in ChildArray)
                {
                    SMMethod method = comp as SMMethod;
                    if (method.IsValid)
                    {
                        if (!string.IsNullOrEmpty(text))
                            text += "\n";
                        text += comp.ToString();
                    }
                }
            }

            if(HasTransition)
            {
                text += "\n";
                text += "\n";
                foreach (SMTransition transition in TransitionList)
                {
                    text += "Goto " + transition.TransitionTargetID + Environment.NewLine;
                }
            }
            
            return text;
        }
        /// <summary>
        /// Called when we add a Method to this action flow item
        /// </summary>
        /// <param name="child"></param>
        public override void Add(CompBase child)
        {
            base.Add(child);
            NeedsRebuild = true;
        }
        private List<SMMethod> validMethods = new List<SMMethod>();
        private ManualResetEvent[] waitHandles = null;

        private List<SMTransition> validTransitions = new List<SMTransition>();
        private ManualResetEvent[] waitHandlesTransitions = null;

        /// <summary>
        /// Check if the scope is doable
        /// </summary>
        /// <param name="proposedScope"></param>
        /// <returns></returns>
        public override string ScopeCheck(string proposedScope)
        {
            if (HasChildren && HasMethod)
            {
                foreach (SMMethod comp in ChildArray)
                {
                    proposedScope = comp.ScopeCheck(proposedScope);
                }
            }
            return proposedScope;
        }
        /// <summary>
        /// Rebuild this Flow Item
        /// </summary>
        public override void Rebuild()
        {

            lock (this)
            {
                //Prepare Valid Method
                validMethods.Clear();
                bool problem = false;
                if (HasChildren && HasMethod)
                {
                    foreach (CompBase comp in ChildArray)
                    {
                        SMMethod method = comp as SMMethod;
                        method.Rebuild();
                        if (method.IsValid)
                        {
                            validMethods.Add(method);
                        }
                        else
                        {
                            problem = true;
                        }
                    }
                }
                HasProblem = problem;
                if (validMethods.Count == 0)
                {
                    U.LogError("Need a valid method for '{0}'", Name);
                }
                waitHandles = new ManualResetEvent[validMethods.Count];
                for (int i = 0; i < validMethods.Count; i++)
                {
                    waitHandles[i] = validMethods[i].waitHandle;
                }

                //Prepare Valid Transition
                validTransitions.Clear();
                if (HasTransition)
                {
                    foreach (SMTransition transition in TransitionList)
                    {
                        transition.ParentContainer = this.ParentContainer;
                        transition.StateMachine = this.StateMachine;
                        transition.Rebuild();
                        if (transition.IsValid)
                        {
                            validTransitions.Add(transition);
                        }
                        else
                        {
                            problem = true;
                        }
                    }
                }

                waitHandlesTransitions = new ManualResetEvent[validTransitions.Count];
                for (int i = 0; i < validTransitions.Count; i++)
                {
                    waitHandlesTransitions[i] = TransitionList[i].waitHandle;
                }


                HasProblem = problem;
                if (validMethods.Count == 0 && validTransitions.Count == 0)
                {
                    U.LogError("Need a valid method and transition for '{0}'", Name);
                }
                NeedsRebuild = false;
            }
        }


        /// <summary>
        /// Returns the path to take from here
        /// </summary>
        /// <returns></returns>
        public override SMPathOut Run()
        {
            lock (this)
            {
                if (HasChildren || HasTransition)
                {
                    if (NeedsRebuild)
                    {
                        //Rebuild();
                    }

                    if (HasMethod)
                    {
                        //For dry run
                        if (CompMachine.s_machine.IsDryRun && this.DryRunSkipActions)
                        {
                            // Do no action
                        }
                        else
                        {
                            //Run All Method
                            //validMethods.ForEach(c => c.RunAsync());
                            Parallel.ForEach(validMethods, method => method.RunAsync());

                            //// Wait until they all complete
                            WaitHandle.WaitAll(waitHandles);
                        }
                    }



                    // Look for any exceptions made by the methods
                    List<Exception> exceptions = new List<Exception>();
                    foreach (SMMethod method in validMethods)
                    {
                        if (method.methodResults.Exception != null)
                        {
                            exceptions.Add(method.methodResults.Exception);
                        }
                    }

                    //Parallel.ForEach(validMethods, method =>
                    //{
                    //    if (method.methodResults.Exception != null)
                    //    {
                    //        exceptions.Add(method.methodResults.Exception);
                    //    }
                    //});


                    if (exceptions.Count > 0)
                    {
                        // We found at least one exception.  Store it with the SMPathOutError 
                        SMPathOutError pathError = this[typeof(SMPathOutError)] as SMPathOutError;
                        if (pathError != null)
                        {
                            pathError.Exceptions = exceptions;
                            return pathError;
                        }
                    }


                    if (HasTransition)
                    {
                        bool anyTransPassValidate = false;
                        do
                        {
                            var waitDelay = Task.Delay(10);
                            if (TransTimeOut > 0 && !_stopWatch.IsRunning)
                            {
                                _stopWatch.Restart();
                            }
                            //Run All Transition
                            validTransitions.ForEach(c => c.RunAsync());
                            //Parallel.ForEach(validTransitions, transition => transition.RunAsync());

                            //Wait until all transition run complete
                            WaitHandle.WaitAll(waitHandlesTransitions, 1000);

                            foreach (SMTransition transition in validTransitions)
                            {
                                anyTransPassValidate = anyTransPassValidate || transition.ValidationResult;
                                if (transition.TransitionResults.Exception != null)
                                {
                                    exceptions.Add(transition.TransitionResults.Exception);
                                }
                            }

                            //Parallel.ForEach(validTransitions, transition =>
                            //{
                            //    anyTransPassValidate = anyTransPassValidate || transition.ValidationResult;
                            //    if (transition.TransitionResults.Exception != null)
                            //    {
                            //        exceptions.Add(transition.TransitionResults.Exception);
                            //    }
                            //});



                            //For dry run
                            if(CompMachine.s_machine.IsDryRun && this.UseDryRunTrans && this.DryRunTransitionTargetID != "")
                            {
                                anyTransPassValidate = true;
                            }


                            if (exceptions.Count > 0)
                            {
                                // We found at least one exception.  Store it with the SMPathOutError 
                                SMPathOutError pathError = this[typeof(SMPathOutError)] as SMPathOutError;
                                if (pathError != null)
                                {
                                    pathError.Exceptions = exceptions;
                                    return pathError;
                                }
                            }


                            //Validate Timeout
                            if (TransTimeOut > 0 && !anyTransPassValidate && (_stopWatch.ElapsedMilliseconds > TransTimeOut))
                            {
                                SMPathOut stopPath = this[typeof(SMPathOutStop)];

                                //Display message in case no path error connected
                                if (stopPath == null || !stopPath.HasTargetID || !FlowTimeoutToStopPath)
                                {
                                    string caption = TimeOutCaption;
                                    if (caption == "")
                                    {
                                        caption = "Transition TimeOut";
                                    }
                                    string msg = TimeOutMessage;
                                    if (msg == "")
                                    {
                                        msg = String.Format("Transition TimeOut [{0}] on state [{1}]", this.PathText, this.StateMachine);
                                    }

                                    _stopWatch.Stop();
                                    _timeoutHandle.Reset();

                                    DisplayAsynchMsg(msg, caption);

                                    _timeoutHandle.WaitOne();
                                    _timeoutHandle.Reset();
                                }
                                else
                                {
                                    _stopWatch.Stop();
                                    _timeoutHandle.Reset();
                                    return this[typeof(SMPathOutStop)];
                                }

                            }
                            waitDelay.Wait();
                        }
                        while (!anyTransPassValidate && LoopTransitions && !StateMachine.ReceivedStop);
                    }

                    _stopWatch.Stop();
                    _timeoutHandle.Reset();
                }

                if (!StateMachine.ReceivedStop)
                {
                    //For dry run
                    if (CompMachine.s_machine.IsDryRun && this.UseDryRunTrans && this.DryRunTransitionTargetID != "")
                    {
                        _dryRunPath.TargetID = this.DryRunTransitionTargetID;
                        _dryRunPath.Initialize(this, true);
                        return _dryRunPath;
                    }

                    //Check First OK Transition Path Out
                    foreach (SMTransition transition in validTransitions)
                    {
                        if (transition.ValidationResult && transition.TransitionPath.TargetID != "")
                        {
                            return transition.TransitionPath;
                        }
                    }
                }
            }

            // Only one out
            return this[typeof(SMPathOut)];
        }


        public void ValidateTranstions()
        {
            try
            {
                //Run All Transition
                validTransitions.ForEach(c => c.RunAsync());
            }
            catch(Exception ex)
            {
                ex.ToString();
            }

            //Wait until all transition run complete
            WaitHandle.WaitAll(waitHandlesTransitions);

        }

        public override CompBase Clone(string name, bool bRecursivley)
        {
            CompBase cloneComp = base.Clone(name, bRecursivley);
            foreach(SMTransition trans in TransitionList )
            {
                (cloneComp as SMActTransFlow).TransitionList.Add(trans.Clone("", true) as SMTransition);
            }

            return cloneComp;
        }

        #endregion Overrides

        /// <summary>
        /// Process (Log) any incoming errors
        /// </summary>
        [StateMachineEnabled]
        public void ProcessIncomingErrors()
        {
            if (IncomingPath is SMPathOutError)
            {
            }
        }

        public void AddTransition(SMTransition transition)
        {
            TransitionList.Add(transition);
            //if (evTransitionListOnChanged != null)
            //{
            //    evTransitionListOnChanged();
            //}
        }

        public void RemoveTransition(SMTransition transition)
        {
            TransitionList.Remove(transition);
            //if(evTransitionListOnChanged != null)
            //{
            //    evTransitionListOnChanged();
            //}
        }


        private delegate void _delParamStringString(string str1,string str2);
        private void DisplayAsynchMsg(string message,string caption)
        {
            if (U.GetDummyControl().InvokeRequired)
            {
                U.GetDummyControl().BeginInvoke(new _delParamStringString(DisplayAsynchMsg), new object[] {message,caption });
                return;
            }
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            _timeoutHandle.Set();
            _stopWatch.Stop();
        }


    }
}
