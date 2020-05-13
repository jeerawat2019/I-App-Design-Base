using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

using MCore.Comp.SMLib.Path;
using MDouble;


namespace MCore.Comp.SMLib.Flow
{
    public class SMTransition : SMFlowBase
    {
        private Stopwatch _stopWatch = new Stopwatch();
        private ManualResetEvent _timeoutHandle = new ManualResetEvent(false);

        public bool LoopTransition
        {
            get { return GetPropValue(() => LoopTransition, false); }
            set { SetPropValue(() => LoopTransition, value); }
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


        private SMPathOut _transitionPath = new SMPathOut();

        [XmlIgnore]
        public SMPathOut TransitionPath
        {
            get { return _transitionPath; }
        }


        [XmlIgnore]
        public TreeNode RefNode = null;

        private Boolean _validationResult = false;
        [XmlIgnore]
        public Boolean ValidationResult
        {
            get
            {
                return _validationResult;
            }
            set
            {
                _validationResult = value;
                SetNodeGUI(value);
            }
        }

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
        /// Class to store the results of the last call
        /// </summary>
        [XmlIgnore]
        public MethodAsyncReturn TransitionResults { get; set; }


        /// <summary>
        /// The wait handle used to trigger muulti-thread signalling
        /// </summary>
        [XmlIgnore]
        public ManualResetEvent waitHandle { get; set; }

        private bool _isValid = false;
        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
        }


        /// <summary>
        /// The Transition Traget ID
        /// </summary>
        public string TransitionTargetID
        {
            get { return GetPropValue(() => TransitionTargetID, string.Empty); }
            set
            {
                if (SetPropValue(() => TransitionTargetID, value))
                {
                    Rebuild();
                }
            }
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
        public SMTransition()
        {
        }
        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="text"></name>
        public SMTransition(string name)
            : base(name)
        {
            // Must have a Path in and a path out
            // Must have a Path in and a path out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutStop(-0.5F);
            this[eDir.Down] = new SMPathOut(0.5F);
            this[eDir.Right] = new SMPathOutError(0.5F);
        }
        /// <summary>
        /// Text to be displayed in cursor
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = string.Empty;
            text = "ID:" + this.Name + Environment.NewLine;
            text += "Goto " + TransitionTargetID;
            return text;
        }
        #endregion Constructors

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
        }

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            Rebuild();
        }

        public override void Rebuild()
        {

            lock (this)
            {
                _isValid = false;
                try
                {
                    _transitionPath.TargetID = TransitionTargetID;
                    _transitionPath.Initialize(this, true);

                    //if(!this.HasChildren)
                    //{
                    //    return;
                    //}

                    if (TransitionTargetID == "")
                    {
                        HasProblem = true;
                        return;
                    }

                    if (this.HasChildren)
                    {
                        foreach (CompBase comp in this.ChildArray)
                        {
                            if (comp is SMSubCondBase)
                            {
                                (comp as SMSubCondBase).Rebuild();
                            }
                        }
                    }
                    _isValid = true;
                    
                }
                catch(Exception ex)
                {
                    HasProblem = true;
                    U.LogPopup(ex, String.Format("Stae Machine '{0}' Unable to create Transition",this.StateMachine.Name));
                }
                
                if (waitHandle == null)
                    waitHandle = new ManualResetEvent(false);
                else
                    waitHandle.Reset();
            }
        }

        public override SMPathOut Run()
        {
            lock (this)
            {
                if (HasChildren)
                {
                    do
                    {
                        var waitDelay = Task.Delay(10);

                        if (TransTimeOut > 0 && !_stopWatch.IsRunning)
                        {
                            _stopWatch.Restart();
                        }

                        try
                        {
                            if (this.HasChildren)
                            {
                                ValidationResult = (this.ChildArray[0] as SMSubCondBase).Validate();
                            }

                            //For dry run
                            if (CompMachine.s_machine.IsDryRun && this.UseDryRunTrans && this.DryRunTransitionTargetID != "")
                            {
                                ValidationResult = true;
                                _dryRunPath.TargetID = this.DryRunTransitionTargetID;
                                _dryRunPath.Initialize(this, true);
                                return _dryRunPath;
                            }


                            if (ValidationResult && _transitionPath.TargetID != "")
                            {
                                return _transitionPath;
                            }
                        }
                        catch (Exception ex)
                        {
                            _stopWatch.Stop();
                            _timeoutHandle.Reset();

                            // We found at least one exception.  Store it with the SMPathOutError 
                            SMPathOutError pathError = this[typeof(SMPathOutError)] as SMPathOutError;
                            if (pathError != null)
                            {
                                pathError.Exceptions.Add(ex);
                                return pathError;
                            }
                        }

                        //Validate Timeout
                        if (TransTimeOut > 0 && !ValidationResult && (_stopWatch.ElapsedMilliseconds > TransTimeOut))
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
                    } while (!ValidationResult && LoopTransition && !StateMachine.ReceivedStop);

                    _stopWatch.Stop();
                    _timeoutHandle.Reset();
                }
                //Always true if no any condition in child array
                else
                {
                    ValidationResult = true;
                    if (ValidationResult && _transitionPath.TargetID != "")
                    {
                       return _transitionPath;
                    }
                }
            }

            // Only one out
            return this[typeof(SMPathOut)];
        }

        public override CompBase Clone(string name, bool bRecursivley)
        {
            CompBase cloneComp = base.Clone(name, bRecursivley);
            (cloneComp as SMTransition).TransitionTargetID = this.TransitionTargetID;

            return cloneComp;
        }

        #region Support Run Async
        /// <summary>
        /// Run the method A-Syncronously
        /// </summary>
        /// 
        public void RunAsync()
        {
            lock (this)
            {
                RunPrepare();
                Task.Run(()=>RunTransition());
            }
        }



        private void RunTransition()
        {
            try
            {
                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = this.ID;
                    U.AddThread(this.ID);
                }
                try
                {
                    Run();
                }
                catch (MCoreException mex)
                {
                    TransitionResults.Exception = mex;
                }
                catch (Exception ex)
                {
                    TransitionResults.Exception = new MCoreExceptionAlert(ex, "Error running {0}", this.ID);
                }
             
              
                waitHandle.Set();
               
            }
            finally
            {
                U.RemoveThread();
            }
        }


        private void RunPrepare()
        {
            TransitionResults = new MethodAsyncReturn() { MethodName = this.ID };
            waitHandle.Reset();
        }
        #endregion


        protected void SetNodeGUI(Boolean resultOK)
        {
            if (RefNode != null)
            {
                RefNode.ForeColor = resultOK ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
            }
        }


        private delegate void _delParamStringString(string str1, string str2);
        private void DisplayAsynchMsg(string message, string caption)
        {
            if (U.GetDummyControl().InvokeRequired)
            {
                U.GetDummyControl().BeginInvoke(new _delParamStringString(DisplayAsynchMsg), new object[] { message, caption });
                return;
            }
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
            _timeoutHandle.Set();
            _stopWatch.Stop();
        }

    }
}
