using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
//
// MLib using declarations
//

using MCore.Comp;
using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib
{
    public class SMStateMachine : SMFlowContainer
    {
        /// <summary>
        /// eMode handler
        /// </summary>
        /// <param name="mode"></param>
        public delegate void eModeHandler(eMode mode);
        #region Privates
        //private BackgroundWorker _bw = null;
        private Task _workerTask = null;
        private CancellationTokenSource _workerTaskCancelSource = new CancellationTokenSource();
       
        private ManualResetEvent _waitHandle = new ManualResetEvent(true);
        protected ReaderWriterLockSlim __receivedStopLock = new ReaderWriterLockSlim();
        private bool _bReceivedStop = false;
        private List<SMFlowChartCtlBase> _listFlowPanels = new List<SMFlowChartCtlBase>();
        private ManualResetEvent _decisionWaitHandle = new ManualResetEvent(false);
        private List<SMDecision> _decisionLoopList = new List<SMDecision>();
        private Dictionary<SMPathOut, SMFlowBase> _pathLoopDict = new Dictionary<SMPathOut, SMFlowBase>();
        #endregion Privates

        /// <summary>
        /// Possible modes
        /// </summary>
        public enum eMode { Pause, Run }


        #region public properties
        private string _lockText = string.Empty;
        /// <summary>
        /// Lock the state machine and this text will be displayed.
        /// (Don't use Get/SetPropValue because we don't want modification to get set for this
        /// </summary>
        public string LockText
        {
            get { return _lockText; }
            set { _lockText = value; }
        }

        /// <summary>
        /// Returns true if state machine is running
        /// </summary>
        [XmlIgnore]
        public bool IsRunning
        {
            get { return GetPropValue(() => IsRunning); }
        }

        /// <summary>
        /// Get the wait handle
        /// </summary>
        public ManualResetEvent WaitHandle
        {
            get { return _waitHandle; }
        }
        /// <summary>
        /// Get the decision Wait Handle
        /// </summary>
        public ManualResetEvent DecisionWaitHandle
        {
            get { return _decisionWaitHandle; }
        }

        /// <summary>
        /// Get the decision Wait Handle
        /// </summary>
        public List<SMDecision> DecisionLoopList
        {
            get { return _decisionLoopList; }
        }

        #endregion public properties
        /// <summary>
        /// Indicates if we've recieved a stop
        /// </summary>
        [XmlIgnore]
        public bool ReceivedStop
        {
            get
            {
                __receivedStopLock.EnterReadLock();
                bool bReceivedStop = _bReceivedStop;
                __receivedStopLock.ExitReadLock();
                return bReceivedStop;
            }
            set
            {
                __receivedStopLock.EnterWriteLock();
                _bReceivedStop = value;
                __receivedStopLock.ExitWriteLock();
            }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMStateMachine()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></name>
        public SMStateMachine(string name)
            : base(name)
        {
            Text = name;
            CreateDefaultItems();
        }

        private void CreateDefaultItems()
        {
            // Must have a Path in and a path out
            // Must have a Path in and a path out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Down] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutPlug();
            this[eDir.Right] = new SMPathOutPlug();
            Add(new SMStart(string.Empty) { GridLoc = new PointF(2.5f, 0.5f), Text = "Start" } );
            Add(new SMReturnStop(string.Empty) { GridLoc = new PointF(2.5f, 6.5f), Text = "Stop" } );
        }
        #endregion Constructors

        /// <summary>
        /// Initialize this object
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            //_bw = new BackgroundWorker() { WorkerSupportsCancellation = true };
            //_bw.DoWork += new DoWorkEventHandler(RunThread);
            //_bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnHaltThread);
            //_workerTask = new Task(() => RunThread(),_workerTaskCancelSource.Token);
            
        }


        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            U.RegisterOnChanged(()=>CompMachine.s_machine.ProcessUsageTrigger,ProcessUsageTrigger_OnChanged);


            //Early Start State Machine Thread;
            _workerTaskCancelSource = new CancellationTokenSource();
            _workerTask = new Task(() => RunThread(), _workerTaskCancelSource.Token, TaskCreationOptions.LongRunning);
            _workerTask.Start();
        }

        /// <summary>
        /// Register a panel to display this flow chart
        /// </summary>
        public void RedrawFlowChartPanels()
        {
            lock (_listFlowPanels)
            {
                foreach (SMFlowChartCtlBase flowChartBase in _listFlowPanels)
                {
                    flowChartBase.Invalidate();
                }
            }
        }
        /// <summary>
        /// Rebuild all panels to for this flow chart
        /// </summary>
        public void RebuildFlowChartPanels()
        {
            lock (_listFlowPanels)
            {
                foreach (SMFlowChartCtlBase flowChartBase in _listFlowPanels)
                {
                    flowChartBase.Rebuild();
                }
            }
        }
        /// <summary>
        /// Register a panel to display this flow chart
        /// </summary>
        /// <param name="flowChartBase"></param>
        public void RegisterFlowPanel(SMFlowChartCtlBase flowChartBase)
        {
            lock (_listFlowPanels)
            {
                if (!_listFlowPanels.Contains(flowChartBase))
                {
                    _listFlowPanels.Add(flowChartBase);
                    flowChartBase.Rebuild();
                }
            }
        }

        /// <summary>
        /// Unregister a panel
        /// </summary>
        /// <param name="flowChartBase"></param>
        public void UnregisterFlowPanel(SMFlowChartCtlBase flowChartBase)
        {
            lock (_listFlowPanels)
            {
                if (_listFlowPanels.Contains(flowChartBase))
                {
                    _listFlowPanels.Remove(flowChartBase);
                }
            }
        }

        /// <summary>
        /// Enter a flow item
        /// </summary>
        public void EnterFlowItem(SMFlowBase currentFlowItem)
        {
            lock (_listFlowPanels)
            {
                currentFlowItem.Highlighted = true;
                if (currentFlowItem.IncomingPath != null)
                {
                    currentFlowItem.IncomingPath.Highlighted = true;
                }
                foreach (SMFlowChartCtlBase flowChartBase in _listFlowPanels)
                {
                    flowChartBase.RefreshFlowItem(currentFlowItem, Mode == eMode.Pause);
                }
            }
        }
        /// <summary>
        /// Exit a flow item
        /// </summary>
        public void ExitFlowItem(SMFlowBase currentFlowItem)
        {
            lock (_listFlowPanels)
            {
                if (currentFlowItem != null)
                {
                    currentFlowItem.Highlighted = false;
                    if (currentFlowItem.IncomingPath != null)
                    {
                        currentFlowItem.IncomingPath.Highlighted = false;
                    }
                    foreach (SMFlowChartCtlBase flowChartBase in _listFlowPanels)
                    {
                        flowChartBase.RefreshFlowItem(currentFlowItem, Mode == eMode.Pause);
                    }
                }
            }
        }
#region Execution Command virtuals

        /// <summary>
        /// Run the state machine
        /// </summary>
        [XmlIgnore]
        public eMode Mode
        {
            get { return GetPropValue(() => Mode, eMode.Pause); }
            set
            {
                SetPropValueNoDirty(() => Mode, value);
                switch (Mode)
                {
                    case eMode.Run:
                        _waitHandle.Set(); // Let it run
                        //if (!_bw.IsBusy)
                        //{
                        //    _bw.RunWorkerAsync();
                        //}

                        if(_workerTask.Status == TaskStatus.RanToCompletion)
                        {
                            _workerTask.Dispose();
                            _workerTaskCancelSource.Dispose();
                            _workerTaskCancelSource = new CancellationTokenSource();
                            _workerTask = new Task(() => RunThread(), _workerTaskCancelSource.Token, TaskCreationOptions.LongRunning);
                        }

                        if (_workerTask.Status == TaskStatus.Created && _workerTask.Status != TaskStatus.Running)
                        {
                            _workerTask.Start();
                        }
                        break;
                    case eMode.Pause:
                        _waitHandle.Reset();  // Make it pause
                        break;
                }
            }
        }

        /// <summary>
        /// Run the state machine
        /// </summary>
        [StateMachineEnabled]
        public virtual void Go()
        {
            
            Mode = eMode.Run;
        }
        /// <summary>
        /// Step the state machine
        /// </summary>
        public virtual void Pause()
        {
            // If not in Pause mode, forces pause mode
            Mode = eMode.Pause;
       }
        /// <summary>
        /// Step the state machine
        /// </summary>
        public virtual void Step()
        {
            // If not in Pause mode, forces pause mode
            Mode = eMode.Pause;
            //if (!_bw.IsBusy)
            //{
            //    _bw.RunWorkerAsync();
            //}

            if (_workerTask.Status == TaskStatus.RanToCompletion)
            {
                _workerTask.Dispose();
                _workerTaskCancelSource.Dispose();
                _workerTaskCancelSource = new CancellationTokenSource();
                _workerTask = new Task(() => RunThread(), _workerTaskCancelSource.Token, TaskCreationOptions.LongRunning);
            }
       
            if (_workerTask.Status == TaskStatus.Created && _workerTask.Status != TaskStatus.Running)
            {
                _workerTask.Start();
            }
            else
            {
                // Free up current flowItem block
                _waitHandle.Set();
            }
        }
        /// <summary>
        /// Stop and do not return until completed
        /// If not running it simply returns immediately
        /// </summary>
        /// <remarks>Intended for state machine</remarks>
        [StateMachineEnabled]
        public void StopAndWait()
        {
            while (IsRunning)
            {
                Stop();
                WaitHandle.WaitOne();
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// Stop the state machine
        /// </summary>
        public virtual void Stop()
        {
            ReceivedStop = true;
            _decisionWaitHandle.Set();

            if (IsRunning && Mode == eMode.Pause)
            {
                Step();
            }
        }
        /// <summary>
        /// ForceStop the state machine
        /// </summary>
        public override void Abort()
        {
            _decisionWaitHandle.Set();
            //_bw.CancelAsync();
            _workerTaskCancelSource.Cancel();
            base.Abort();
        }
        #endregion Execution Command virtuals

        /// <summary>
        /// Run and do not return until completed
        /// If already running, then it waits until completion
        /// </summary>
        /// <remarks>Intended for state machine</remarks>
        [StateMachineEnabled]
        public void Call()
        {
            if (!IsRunning)
            {
                Go();
            }
            while (IsRunning)
            {
                WaitHandle.WaitOne();
                Thread.Sleep(10);
            }
        }

        #region Handle internal execution
       // private void RunThread(object sender, DoWorkEventArgs e)
        private void RunThread()
        {

            if (!IsRunning)
            {
                U.AddThread("State Machine '{0}'", Text);
            }

            try
            {
                if (!IsRunning)
                {
                    SetPropValueNoDirty(() => IsRunning, true);
                    Run();
                }
                Mode = eMode.Pause;
                SetPropValueNoDirty(() => IsRunning, false);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "State Machine '{0}' error", Text);
            }
            finally
            {
                U.RemoveThread();
            }
        }
        #endregion Handle internal execution

        /// <summary>
        /// Called by a Decision that has experienced a change to its value
        /// This indicates that the Decision loop needs to be reevaluated
        /// </summary>
        public void OnDecisionChanged()
        {
            _decisionWaitHandle.Set();
        }

        public void AddToDecisionList(SMDecision decisionItem, SMFlowBase flowItemForPathOut, SMPathOut pathOut)
        {
            _decisionLoopList.Add(decisionItem);
            _pathLoopDict.Add(pathOut, flowItemForPathOut);
        }
        public void ClearDecisionList()
        {
            _decisionLoopList.ForEach(c => c.ClearNotifier());
            _decisionLoopList.Clear();

            _pathLoopDict.Clear();
        }
        #region Private Methods


        private void OnHaltThread(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
        }


        private void ProcessUsageTrigger_OnChanged(Boolean trigger)
        {
            if(!trigger)
            {
                try
                {
                    string filepath = string.Format(@"{0}\TimingDumps\Timing Dump {1}.csv", U.RootComp.RootFolder,this.Name);
                    bool needsHeader = true;
                    U.EnsureDirectory(filepath);
                    using (StreamWriter logWriter = new StreamWriter(filepath, false))
                    {
                        if (needsHeader)
                        {
                            logWriter.WriteLine("State Machine,Items,Date,Time,FromMidnight (ms), delta (ms), Elapsed (ms)");
                        }
                        long prevTicks = 0;
                        DumpTiming(logWriter, string.Empty, ref prevTicks);
                        logWriter.Close();
                    }
                }
                catch (Exception ex)
                {
                    U.LogError(ex, "Unable to Write to timing file");
                }
            }
        }
        #endregion Private Methods
    }
}
