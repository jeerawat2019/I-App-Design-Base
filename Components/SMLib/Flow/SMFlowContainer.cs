using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Threading;

using MCore.Comp.SMLib.Path;
using MDouble;

namespace MCore.Comp.SMLib.Flow
{
    public class SMFlowContainer : SMFlowBase
    {
        #region Private properties
        private SMFlowBase _currentFlowItem = null;
        private int _decisionTimeout = -1;
        private string _prevScope = string.Empty;
        private Stopwatch _stopWatch = new Stopwatch();
        private ManualResetEvent _timeoutHandle = new ManualResetEvent(false);

        private SMStateMachine stateMachine = null;
        private ManualResetEvent waitHandle = null;
        private ManualResetEvent decisionWaitHandle = null;
        private List<SMDecision> decisionLoopList = null;
        private SMPathOut pathOut = null;
        private SMFlowBase flowItemForPathOut = null;
        private int _gridWidht = 20;
        private int _gridHeight = 40;
       
        #endregion Private properties

        public const string NOSCOPE = "**No Scope**";

        #region Serialize properties
        /// <summary>
        /// Upper left corner used for current scroll position
        /// </summary>
        public Point GridCorner 
        {
            get { return GetPropValue(() => GridCorner, Point.Empty); }
            set { SetPropValue(() => GridCorner, value); }
        }
        /// <summary>
        /// The total size of the grid drawing
        /// </summary>
        public Size GridSize
        {
            get { return GetPropValue(() => GridSize, new Size(_gridWidht,_gridHeight)); }
            set 
            { 
                SetPropValue(() => GridSize, value);
                _gridWidht = value.Width;
                _gridHeight = value.Height;
            }
        }

        /// <summary>
        /// The scope ID
        /// </summary>
        public string ScopeID
        {
            get { return GetPropValue(() => ScopeID, string.Empty); }
            set { SetPropValue(() => ScopeID, value); }
        }
        #endregion
        /// <summary>
        /// The Prev scope ID
        /// </summary>
        public string PrevScopeID
        {
            get { return _prevScope; }
            set { _prevScope = value; }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMFlowContainer()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></name>
        public SMFlowContainer(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region overrides
        public override void Initialize()
        {
            U.RegisterOnChanged(() => ScopeID, OnScopeChanged);

            if(GridSize.Width != _gridWidht || GridSize.Height != _gridHeight)
            {
                GridSize = new Size(_gridWidht,_gridHeight);
            }

           
            base.Initialize();
            _prevScope = ScopeID;
        }

        public override void Destroy()
        {
            base.Destroy();
            U.UnRegisterOnChanged(() => ScopeID, OnScopeChanged);
        }
        #endregion overrides


        private void OnScopeChanged(string scopeID)
        {
            foreach(SMFlowBase flowBase in ChildArray)
            {
                flowBase.Rebuild();
            }
            _prevScope = ScopeID;
        }
        /// <summary>
        /// Add Flow Item
        /// </summary>
        /// <param name="flowItemAdded"></param>
        public void AddFlowItem(SMFlowBase flowItemAdded)
        {
            Add(flowItemAdded);
            flowItemAdded.Initialize();
        }
        /// <summary>
        /// Get the flow item for the target in the specified pathOut
        /// </summary>
        /// <param name="pathOut"></param>
        /// <returns></returns>
        public SMFlowBase GetFlowTarget(SMPathOut pathOut)
        {
            if (pathOut.HasTargetID)
            {
                SMFlowBase[] list = null;
                if (!pathOut.TargetID.Contains("."))
                {
                    //list = FilterByType<SMFlowBase>();
                    list = pathOut.Owner.Parent.FilterByType<SMFlowBase>();
                }
                //Sport Transition
                else
                {
                    SMFlowBase smPathOut = U.GetComponent(pathOut.TargetID) as SMFlowBase;
                    return smPathOut;
                }
                

                if (list.Length > 0)
                {
                    try
                    {
                        return list.First(c => c.Name == pathOut.TargetID);
                    }
                    catch
                    {
                        pathOut.DeletedTarget();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Copy certain non-unique properties
        /// </summary>
        /// <param name="compTo"></param>
        public override void ShallowCopyTo(CompBase compTo)
        {
            base.ShallowCopyTo(compTo);
        }
        /// <summary>
        /// Clone this component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            SMFlowContainer newComp = base.Clone(name, bRecursivley) as SMFlowContainer;
            newComp.ScopeID = ScopeID;
            newComp.GridCorner = GridCorner;
            newComp.GridSize = GridSize;

            return newComp;
        }

        private void FindStart()
        {
            _currentFlowItem = ChildArray.ToList().Find(c => c is SMStart) as SMFlowBase;
            if (_currentFlowItem == null)
            {
                throw new Exception("Unable to find the Start Flow chart element");
            }
        }

        private bool FindStop()
        {
            SMFlowBase exitFlowItem = ChildArray.ToList().Find(c => c is SMReturnStop) as SMFlowBase;
            if (exitFlowItem != null)
            {
                _currentFlowItem = exitFlowItem;
                return true;
            }
            return false;
        }

        
        /// <summary>
        /// Move everything in this level
        /// </summary>
        /// <param name="moveDist"></param>
        public void MoveAll(PointF moveDist)
        {
            foreach (SMFlowBase flowItem in ChildArray)
            {
                if (!flowItem.GridLoc.IsEmpty)
                {
                    flowItem.GridLoc = new PointF(flowItem.GridLoc.X + moveDist.X, flowItem.GridLoc.Y + moveDist.Y); 
                }
            }
        }
        /// <summary>
        /// Find the target at the specified endpoint
        /// </summary>
        /// <param name="searcher"></param>
        /// <param name="endGridPt"></param>
        /// <returns></returns>
        public SMFlowBase FindTarget(SMFlowBase searcher, PointF endGridPt)
        {
            foreach (SMFlowBase flowItem in ChildArray)
            {
                if (!object.ReferenceEquals(searcher, flowItem) && flowItem.Contains(endGridPt))
                {
                    return flowItem;
                }

            }
            return null;
        }

        public void AutoScope(bool autoScope)
        {
            string oldScope = ScopeID;
            string newScope = string.Empty;
            if (autoScope)
            {
                if (string.IsNullOrEmpty(ScopeID))
                {

                    foreach (SMFlowBase flowBase in ChildArray)
                    {
                        newScope = flowBase.ScopeCheck(newScope);
                    }
                    if (newScope == NOSCOPE)
                    {
                        newScope = string.Empty;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                newScope = string.Empty;
            }
            if (oldScope != newScope)
            {
                ScopeID = newScope;
                U.LogChange(string.Format("{0}.Scope", Nickname), oldScope, newScope); 
            }

        }

        public static string DetermineScope(string proposedScope, string path)
        {

            if (string.IsNullOrEmpty(proposedScope))
            {
                // First occurance
                return path;
            }

            // pathA.pathB.id
            // pathA.PathB.PathC (proposed)
            string newProposed = NOSCOPE;
            for (int i = 0; i >= 0 && i < proposedScope.Length;)
            {
                string check = string.Empty;
                i = proposedScope.IndexOf('.', i);
                if (i < 0)
                {
                    check = proposedScope;
                }
                else
                {
                    check = proposedScope.Substring(0, i++);
                }
                if (!path.StartsWith(check))
                {
                    return newProposed;
                }
                newProposed = check;
            }


            return newProposed;
        }
        /// <summary>
        /// Evaluate all the Path Segments and look for targets
        /// </summary>
        public void DetermineAllChildTargets()
        {

            ChildArray.ToList().ForEach(c => (c as SMFlowBase).DetermineAllPathTargets());
        }

        private string _editRegisterationID = String.Empty;

        /// <summary>
        /// Register the editing
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RegisterEdit(string ID)
        {
            if (string.IsNullOrEmpty(_editRegisterationID))
            {
                _editRegisterationID = ID;
                U.LogChange(string.Format("{0}, Start editing", Nickname));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if editing
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool IsEditing(string ID)
        {
            return _editRegisterationID == ID;
        }
        /// <summary>
        /// Unregister the editing
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool UnregisterEdit(string ID)
        {
            if (_editRegisterationID == ID)
            {
                U.LogChange(string.Format("{0}, Finish editing", Nickname));
                _editRegisterationID = string.Empty;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enter the path delay if it is less than any others
        /// </summary>
        /// <param name="mSec"></param>
        public void EnterPathDelay(int mSec)
        {
            if (mSec > 0)
            {
                if (_decisionTimeout == -1)
                {
                    _decisionTimeout = 0x7fffffff;
                }
                _decisionTimeout = Math.Min(_decisionTimeout, mSec);
            }
        }

        private void EnterFlowItem()
        {
            StateMachine.EnterFlowItem(_currentFlowItem);
        }
        private void ExitFlowItem()
        {
            StateMachine.ExitFlowItem(_currentFlowItem);
        }


        private bool _isAlreadyPreRun = false;
        protected virtual void PreRun()
        {
            try
            {
                if (!_isAlreadyPreRun)
                {
                    if (stateMachine == null)
                    {
                        stateMachine = StateMachine;
                    }
                    _timeoutHandle.Reset();
                    waitHandle = stateMachine.WaitHandle;
                    decisionWaitHandle = stateMachine.DecisionWaitHandle;
                    decisionLoopList = stateMachine.DecisionLoopList;
                    pathOut = null;
                    flowItemForPathOut = null;
                    _decisionTimeout = -1;
                    ExitFlowItem();
                    FindStart();
                    EnterFlowItem();
                    _isAlreadyPreRun = true;
                }
            }
            catch
            {
                _isAlreadyPreRun = false;
            }
        }



        /// <summary>
        /// Run method for the main state machine thread
        /// </summary>
        /// <returns></returns>
        public override SMPathOut Run()
        {
            //SMStateMachine stateMachine = StateMachine;
            try
            {
                //_timeoutHandle.Reset();
                //ManualResetEvent waitHandle = stateMachine.WaitHandle;
                //ManualResetEvent decisionWaitHandle = stateMachine.DecisionWaitHandle;
                //List<SMDecision> decisionLoopList = stateMachine.DecisionLoopList;
                //SMPathOut pathOut = null;
                //SMFlowBase flowItemForPathOut = null;
                //_decisionTimeout = -1;
                //ExitFlowItem();
                //FindStart();
                //// Hightlight start
                //EnterFlowItem();

                PreRun();

                while (true)
                {
                    Thread.Sleep(0);
                    // If Reset will block thread (Pause) until Step or Run (Set)
                    if (stateMachine.Mode == SMStateMachine.eMode.Pause)
                    {
                        waitHandle.Reset();
                        // Wait until(Pause) until Step or Run (Set)
                        EnterFlowItem();
                        waitHandle.WaitOne();
                        ExitFlowItem();
                    }

                    if (_currentFlowItem is SMDecision)
                    {
                        SMDecision decisionItem = _currentFlowItem as SMDecision;
                        if (decisionLoopList.Contains(decisionItem))
                        {
                            // We have closed the loop of nothing but decision items
                            EnterFlowItem();
                            //stateMachine.EnterPathItem(flowItemForPathOut, flowItemForPathOut.OutgoingPath);

                            // Modify by Kasem 12-Feb-2018
                            if (decisionItem.WaitTimeoutMS <= 0)
                            {
                                decisionWaitHandle.WaitOne(_decisionTimeout);
                            }
                            else
                            {
                                Boolean signal = false;
                                signal =  decisionWaitHandle.WaitOne(decisionItem.WaitTimeoutMS);
                                //Trigger Timeout
                                if(!signal)
                                {
                                   _timeoutHandle.Set();
                                }
                               
                            }
                            //--------------------------------

                            _decisionTimeout = -1;
                            //stateMachine.ExitPathItem(flowItemForPathOut, flowItemForPathOut.OutgoingPath);
                            ExitFlowItem();
                            stateMachine.ClearDecisionList();
                        }
                        else
                        {
                            if (decisionLoopList.Count == 0)
                            {
                                // First one.  Reset early in case something changes quickly
                              
                                decisionWaitHandle.Reset();
                               
                            }
                           
                            decisionItem.AddNotifier(StateMachine.OnDecisionChanged);
                            stateMachine.AddToDecisionList(decisionItem, flowItemForPathOut, pathOut);
                        }

                       
                    }
                    else
                    {
                        stateMachine.ClearDecisionList();
                    }
                    //
                    //  Run this item
                    //


                    //Dump Process Usage Timing
                    if (CompMachine.s_machine.ProcessUsageTrigger)
                    {
                        if (!(_currentFlowItem is SMFlowContainer))
                        {
                            StateMachine.AddTimingElement(">Run: " + _currentFlowItem.Text);
                        }
                        else
                        {
                            StateMachine.AddTimingElement(">Enter: " + _currentFlowItem.Text);
                        }
                    }


                    EnterFlowItem();
                    pathOut = _currentFlowItem.Run();
                    ExitFlowItem();

                     //Dump Process Usage Timing
                    if (CompMachine.s_machine.ProcessUsageTrigger)
                    {
                        if (!(_currentFlowItem is SMFlowContainer))
                        {
                            StateMachine.AddTimingElement("<Run: " + _currentFlowItem.Text);
                        }
                        else
                        {
                            StateMachine.AddTimingElement("<Exit: " + _currentFlowItem.Text);
                        }
                    }

                    //Add by Kasem 12-Feb-2018
                    if(_currentFlowItem is SMDecision)
                    {
                        //Determine Start to Capture Timeout
                        SMDecision decisionItem = _currentFlowItem as SMDecision;

                        //No Partout Meaning Waiting Condition
                        if (pathOut != null && !pathOut.HasTargetID && decisionItem.WaitTimeoutMS > 0)
                        {
                            //Start Stop watch if Nested Condition
                            if (!_stopWatch.IsRunning && _currentFlowItem.HasChildren)
                            {
                                _stopWatch.Restart();
                            }
                            else if(_stopWatch.IsRunning && !_currentFlowItem.HasChildren)
                            {
                                _stopWatch.Stop();
                            }
                            else
                            {
                                if ((_currentFlowItem.HasChildren) && (_stopWatch.ElapsedMilliseconds > decisionItem.WaitTimeoutMS) ||
                                    (!_currentFlowItem.HasChildren) && _timeoutHandle.WaitOne(0))
                                {
                                    //Check Stop Path and Flow to Timeout Flag
                                    SMPathOut stopPath = _currentFlowItem[typeof(SMPathOutStop)];

                                    //Popup in case no path for Timeout
                                    if (!decisionItem.FlowTimeoutToStopPath ||
                                        stopPath == null || !stopPath.HasTargetID)
                                    {
                                        U.LogWarning("Decision Wait Time Out [{0}] on state [{1}]", decisionItem.PathText, this.StateMachine);
                                        string msg = String.Format("Decision Wait Time Out [{0}] on state [{1}]", decisionItem.PathText, this.StateMachine);

                                        _stopWatch.Stop();
                                        _timeoutHandle.Reset();
                                        
                                        DisplayAsynchMsg(msg);
                                        _timeoutHandle.WaitOne();
                                        _timeoutHandle.Reset();
                                    }
                                    //Case Timeout path available 
                                    else
                                    {
                                        _stopWatch.Stop();
                                        _timeoutHandle.Reset();
                                        pathOut = stopPath;
                                    }
                      
                                }
                            }

                        }
                        //Force pathout to null incase of pathout is stop path but we use FlowTimeoutToStopPath option
                        else if(pathOut is SMPathOutStop && decisionItem.FlowTimeoutToStopPath)
                        {
                            pathOut = null;
                            _stopWatch.Stop();
                        }
                        //Stop capture time in case decision meet wait condition
                        else
                        {
                            _stopWatch.Stop();
                        }

                    }
                    
                    //Modify by Kasem 13-Feb-2018
                    if (stateMachine.ReceivedStop )
                    {

                        if (!(_currentFlowItem is SMDecision) && !(_currentFlowItem is SMTransition) && !(_currentFlowItem is SMActTransFlow))
                        {
                            
                            stateMachine.ReceivedStop = false;
                            // Redirect the path to the PathOutStop
                            pathOut = _currentFlowItem[typeof(SMPathOutStop)];
                        }
                        else
                        {
                            stateMachine.ReceivedStop = false;
                            _stopWatch.Stop();
                            if (_currentFlowItem is SMDecision)
                            {
                                if (!(_currentFlowItem as SMDecision).FlowTimeoutToStopPath)
                                {
                                    pathOut = _currentFlowItem[typeof(SMPathOutStop)];
                                }
                                else
                                {
                                    pathOut = null;

                                }
                            }
                            else if (_currentFlowItem is SMTransition)
                            {
                                if (!(_currentFlowItem as SMTransition).FlowTimeoutToStopPath)
                                {
                                    pathOut = _currentFlowItem[typeof(SMPathOutStop)];
                                }
                                else
                                {
                                    pathOut = null;

                                }
                            }
                            else if (_currentFlowItem is SMActTransFlow)
                            {
                                if (!(_currentFlowItem as SMActTransFlow).FlowTimeoutToStopPath)
                                {
                                    pathOut = _currentFlowItem[typeof(SMPathOutStop)];
                                }
                                else
                                {
                                    pathOut = null;

                                }
                            }
                        }
                    }
                    //--------------------------------

                    if (pathOut == null)
                    {
                        // Will stop the whole State Machine
                        return null;
                    }

                    SMPathOutError pathOutError = pathOut as SMPathOutError;
                    if (pathOutError != null)
                    {
                        pathOutError.ProcessErrors();
                    }

                    if (pathOut.HasTargetID)
                    {
                        flowItemForPathOut = _currentFlowItem;

                        // We are on the target path.
                        _currentFlowItem = GetFlowTarget(pathOut);
                        _currentFlowItem.IncomingPath = pathOut;
                    }
                    else if ((_currentFlowItem is SMDecision) && !(pathOut is SMPathOutStop) && !(pathOut is SMPathOutError))
                    {
                        // Keep same current flow item. Let it loop to itself
                    }
                    else
                    {
                        // No target to go to
                        // Will stop the whole State Machine

                        if ((pathOut is SMPathOutStop) || (pathOut is SMPathOutError))
                        {
                            if (!FindStop())
                                FindStart();
                            EnterFlowItem();
                        }
                        _currentFlowItem.IncomingPath = null;
                        return null;
                    }

                    if (_currentFlowItem == null)
                    {
                        throw new Exception(string.Format("Could not locate Flowitem from ID '{0}' in StateMachine '{1}'.  State Machine has paused.",
                           pathOut.TargetID, Text));
                    }
                    if (_currentFlowItem is SMExit)
                    {
                        EnterFlowItem();
                        if (this is SMDecision)
                        {
                            foreach (SMPathOut path in PathArray)
                            {
                                if (_currentFlowItem is SMReturnNo)
                                {
                                    if (path is SMPathOutBool && !(path as SMPathOutBool).True)
                                    {
                                        return path;
                                    }
                                }
                                else if (_currentFlowItem is SMReturnYes)
                                {
                                    if (path is SMPathOutBool && (path as SMPathOutBool).True)
                                    {
                                        return path;
                                    }
                                }
                                else if (_currentFlowItem is SMReturnStop)
                                {
                                    if (path is SMPathOutStop)
                                    {
                                        return path;
                                    }
                                }
                            }
                        }
                        if (this is SMSubroutine && _currentFlowItem is SMReturnStop)
                        {
                            return this[typeof(SMPathOutStop)];
                        }
                        return this[typeof(SMPathOut)];
                    }
                }//End While True;
            }
            finally
            {
                stateMachine.ClearDecisionList();
                _isAlreadyPreRun = false;
                System.Threading.Tasks.Task.Run(() => PreRun());
            }
        }


        private delegate void _delParamObject(object obj);
        private void DisplayAsynchMsg(object param)
        {
            if (U.GetDummyControl().InvokeRequired)
            {
                U.GetDummyControl().BeginInvoke(new _delParamObject(DisplayAsynchMsg), new object[] { param });
                return;
            }
            MessageBox.Show(param as String, "Decision Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            _timeoutHandle.Set();
            _stopWatch.Stop();
        }


      
    }
}
