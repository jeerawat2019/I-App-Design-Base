using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading.Tasks;

using MCore;
using MCore.Comp;

namespace MCore.Comp.SMLib.Flow
{
    public class SMMethod : CompBase
    {
        private bool _needsRebuild = true;
        private bool _suppressRebuild = false;
        #region Serialized Properties

        /// <summary>
        /// The string that identifies the target object and method
        /// </summary>
        /// <remarks>Example: "Machine.Motion.Axis.Home()</remarks>
        /// <remarks>Example: "Machine.Motion.Axis.MoveRel(10 mm)</remarks>
        /// <remarks>Example: "X.MoveAbs(TargetPos, TargetSpeed)</remarks>
        //[XmlIgnore]
        public string MethodID
        {
            get { return GetPropValue(() => MethodID, string.Empty); }
            set
            {
                if (SetPropValue(() => MethodID, value) && !_suppressRebuild)
                {
                    _needsRebuild = true;
                    DoRebuild();
                    if (Parent is SMActionFlow)
                    {
                        (Parent as SMActionFlow).NeedsRebuild = true;
                    }
                    else if (Parent is SMActTransFlow)
                    {
                        (Parent as SMActTransFlow).NeedsRebuild = true;
                    }
                }
            }
        }

        private string GetFullID()
        {

            string scopeID = "";
            string prevScopeID = "";

            if (Parent is SMActionFlow)
            {
                scopeID = (Parent as SMActionFlow).ParentContainer.ScopeID;
                prevScopeID = (Parent as SMActionFlow).ParentContainer.PrevScopeID;
            }
            else if(Parent is SMActTransFlow)
            {
                scopeID = (Parent as SMActTransFlow).ParentContainer.ScopeID;
                prevScopeID = (Parent as SMActTransFlow).ParentContainer.PrevScopeID;
            }

            if (scopeID != prevScopeID)
            {
                string fullID = string.Empty;
                if (string.IsNullOrEmpty(prevScopeID))
                {
                    fullID = MethodID;
                }
                else
                {
                    fullID = string.Format("{0}.{1}", prevScopeID, MethodID);
                }
                if (!string.IsNullOrEmpty(scopeID))
                {
                    MethodID = fullID.Substring(scopeID.Length + 1);
                }
                else
                {
                    MethodID = fullID;
                }
            }

            if (!string.IsNullOrEmpty(scopeID) && !string.IsNullOrEmpty(MethodID))
            {
                return string.Format("{0}.{1}", scopeID, MethodID);
            }
            return MethodID;
        }

        ///// <summary>
        ///// Used for serialization
        ///// </summary>
        //[XmlElement(ElementName = "MethodID")]
        //public string SerMethodID
        //{
        //    get { return _methodID; }
        //    set { _methodID = value; }
        //}

        #endregion Serialized Properties

        #region non-serialized public Properties


        /// <summary>
        /// The wait handle used to trigger muulti-thread signalling
        /// </summary>
        [XmlIgnore]
        public ManualResetEvent waitHandle { get; set; }

        /// <summary>
        /// Class to store the results of the last call
        /// </summary>
        [XmlIgnore]
        public MethodAsyncReturn methodResults { get; set; }

        #endregion
        #region privates
        private Stopwatch _sw = new Stopwatch();
        //private MethodInvoker _spawnedThreadDel = null;
        private MethodWrapper _methodWrapper = null;

        //Add by Kasem for Faster Method
        //private AsyncCallback _completeCallBack = null;
        //private MethodInvoker _workerThreadDel = null;


        #endregion non-serialized public Properties


        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMMethod()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public SMMethod(string childName)
            : base(childName)
        {
        }

        ///// <summary>
        ///// Creation constructor
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="methodId"></param>
        ///// <param name="args"></param>
        //public SMAction(string text, string methodId, object[] args)
        //    : base(text)
        //{
        //    this.methodID = methodID;
        //    this.args = args; 
        //}

        #endregion
        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            Rebuild();
        }
        /// <summary>
        /// Clone this component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            SMMethod newComp = base.Clone(name, bRecursivley) as SMMethod;
            newComp.MethodID = MethodID;
            return newComp;
        }
        /// <summary>
        /// Use ToString to get underlying content
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MethodID;
        }
        /// <summary>
        /// Determines if the method call is valid and ready
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _methodWrapper != null;
            }
        }

        /// <summary>
        /// Check if the scope is doable
        /// </summary>
        /// <param name="proposedScope"></param>
        /// <returns></returns>
        public string ScopeCheck(string proposedScope)
        {
            if (proposedScope == SMFlowContainer.NOSCOPE || string.IsNullOrEmpty(MethodID))
            {
                return proposedScope;
            }
            string methodPath = string.Empty;
            if (MethodID.Contains('.'))
            {
                U.SplitMethodID(MethodID, out methodPath);
                int lastPeriod = methodPath.LastIndexOf('.');
                if (lastPeriod > 0)
                {
                    methodPath = methodPath.Substring(0, lastPeriod);
                }
                else
                {
                    methodPath = string.Empty;
                }

            }
            return SMFlowContainer.DetermineScope(proposedScope, methodPath);
        }

        /// <summary>
        /// Rebuild the internals
        /// </summary>
        public void Rebuild()
        {
            _needsRebuild = true;
            DoRebuild();
        }
        /// <summary>
        /// Rebuild the internals
        /// </summary>
        private void DoRebuild()
        {
            lock (this)
            {
                if (_needsRebuild)
                {
                    if (string.IsNullOrEmpty(MethodID))
                    {
                        return;
                    }
                    _suppressRebuild = true;
                    _methodWrapper = MethodWrapper.Create(GetFullID());
                    if (_methodWrapper != null)
                    {
                        if (waitHandle == null)
                            waitHandle = new ManualResetEvent(false);
                        else
                            waitHandle.Reset();

                    }
                    _needsRebuild = false;
                    _suppressRebuild = false;

                    //Add by Kasem for Improve Method Faster Start
                    //_workerThreadDel = new MethodInvoker(RunWorkerAsync);
                    //_spawnedThreadDel = new MethodInvoker(RunAction);
                    //_completeCallBack = new AsyncCallback(Completed);
                }
            }
        }

        /// <summary>
        /// Run the method A-Syncronously
        /// </summary>
        /// 
        public void RunAsync()
        {
            lock (this)
            {
                RunPrepare();
                //_workerThreadDel.BeginInvoke(null, null);

                Task doMethodAsync = Task.Run(() => RunAction());
                //doMethodAsync.ContinueWith(Completed);


                //RunPrepare();
                //if (_methodWrapper != null)
                //{
                //    if (_spawnedThreadDel == null)
                //    {
                //        _spawnedThreadDel = new MethodInvoker(RunAction);
                //    }
                //    //_spawnedThreadDel.BeginInvoke(new AsyncCallback(Completed), null);
                //    //Edit by Kasem for Faster Method Execute
                //    _spawnedThreadDel.BeginInvoke(_completeCallBack, null);
                //}
            }
        }


        //private void RunWorkerAsync()
        //{
        //    lock (this)
        //    {
        //        //RunPrepare();
        //        if (_methodWrapper != null)
        //        {
        //            if (_spawnedThreadDel == null)
        //            {
        //                _spawnedThreadDel = new MethodInvoker(RunAction);
        //            }
        //            //_spawnedThreadDel.BeginInvoke(new AsyncCallback(Completed), null);
        //            //Edit by Kasem for Faster Method Execute
        //            _spawnedThreadDel.BeginInvoke(_completeCallBack, null);
        //        }
        //    }
        //}


        /// <summary>
        /// Prepare the class members for a Sync or Async call
        /// </summary>
        private void RunPrepare()
        {
            if (_methodWrapper != null)
            {
                methodResults = new MethodAsyncReturn() { MethodName = _methodWrapper.MethodName };
                waitHandle.Reset();
                _sw.Reset();
                _sw.Start();
            }
        }
        /// <summary>
        /// Callback when thread is completed
        /// </summary>
        /// <remarks>The only purpose is to call endInvoke</remarks>
        /// <param name="iar"></param>
        //private void Completed(IAsyncResult iar)
        //{
        //    System.Runtime.Remoting.Messaging.AsyncResult ar = iar as System.Runtime.Remoting.Messaging.AsyncResult;
        //    if (iar == null || ar == null || ar.AsyncDelegate == null)
        //    {
        //        throw new Exception("Expected an IAsyncResult");
        //    }
        //    MethodInvoker spawnedDel = ar.AsyncDelegate as MethodInvoker;
        //    if (spawnedDel == null)
        //    {
        //        throw new Exception("Expected SpawnedThreadP1Del");
        //    }
        //    spawnedDel.EndInvoke(iar);

        //}
        /// <summary>
        /// Run the action, catch any exceptions, and store results
        /// </summary>
        private void RunAction()
        {
            try
            {
                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = _methodWrapper.MethodName;
                    U.AddThread(_methodWrapper.MethodName);
                }
                try
                {
                     _methodWrapper.Invoke();
                }
                catch (MCoreException mex)
                {
                    methodResults.Exception = mex;
                }
                catch (Exception ex)
                {
                    methodResults.Exception = new MCoreExceptionAlert(ex, "Error running {0}", _methodWrapper.MethodName);
                }
                //int workerThreads = 0;
                //int completionPortThreads = 0;
                //ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                //System.Diagnostics.Debug.WriteLine(string.Format("IsPoolThread={0}, Avail={1}", Thread.CurrentThread.IsThreadPoolThread.ToString(), workerThreads));

                _sw.Stop();
                waitHandle.Set();
                methodResults.TimeMsec = _sw.Elapsed.TotalMilliseconds;
            }
            finally
            {
                U.RemoveThread();
            }
        }
    }
    /// <summary>
    /// Class for holding return value from an RunAsync
    /// </summary>
    public class MethodAsyncReturn
    {
        /// <summary>
        /// Elapsed time in msec
        /// </summary>
        public double TimeMsec { get; set; }
        /// <summary>
        /// Running Method name
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Exception if any
        /// </summary>
        public MCoreException Exception { get; set; }
        /// <summary>
        /// Return value if any
        /// </summary>
        public object ReturnValue { get; set; }
    }
}
