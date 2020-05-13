using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading;
using MDouble;

namespace MCore.Comp.Geometry
{
    /// <summary>
    /// The class to manage a workpiece
    /// It's children are the Patterns in the order of
    /// </summary>
    public class GWorkPiece : G3DCompBase
    {
        #region Privates

        /// <summary>
        /// Default to let pass
        /// </summary>
        private ManualResetEvent _waitComplete = new ManualResetEvent(true);

        #endregion Privates

        
        #region Public Properties

        /// <summary>
        /// The safe ZHt for robot transfer to this workpiece
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("The safe ZHt for robot transfer to this workpiece")]
        public Millimeters ZSafeTransfer
        {
            get { return GetPropValue(() => ZSafeTransfer, 10); }
            set { SetPropValue(() => ZSafeTransfer, value); }
        }

        /// <summary>
        /// The safe ZHt for robot transfer to this workpiece
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("The safe ZHt for robot transfer to this workpiece")]
        public MillimetersPerSecond XYTransferSpeed
        {
            get { return GetPropValue(() => XYTransferSpeed, 10); }
            set { SetPropValue(() => XYTransferSpeed, value); }
        }


        /// <summary>
        /// The speed to move the Z axis after Safe transfer
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("The speed to move the Z axis after Safe transfer")]
        public MillimetersPerSecond ZTransferSpeed
        {
            get { return GetPropValue(() => ZTransferSpeed, 10); }
            set { SetPropValue(() => ZTransferSpeed, value); }
        }

        /// <summary>
        /// Margin before/after first/last trigger
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("Margin before/after first/last trigger")]
        public Millimeters TriggerMargin
        {
            get { return GetPropValue(() => TriggerMargin, 0); }
            set { SetPropValue(() => TriggerMargin, value); }
        }

        /// <summary>
        /// Flag is true if we need to abort the run
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("Flag is true if we need to abort the run")]
        public bool AbortRun
        {
            get { return GetPropValue(() => AbortRun, false); }
            set { SetPropValue(() => AbortRun, value); }
        }

        /// <summary>
        /// Reason for the Run Abort 
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("Reason for the Run Abort")]
        public string AbortReason
        {
            get { return GetPropValue(() => AbortReason, string.Empty); }
            set { SetPropValue(() => AbortReason, value); }
        }

        /// <summary>
        /// The currently-running operation 
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("The currently-running operation")]
        public string CurrentOperation
        {
            get { return GetPropValue(() => CurrentOperation, string.Empty); }
            set { SetPropValue(() => CurrentOperation, value); }
        }

        /// <summary>
        /// The timeout to wait for the last trigger to complete
        /// </summary>
        [Browsable(true)]
        [Category("GWorkPiece")]
        [Description("The timeout to wait for the last trigger to complete")]
        [XmlIgnore]
        public Miliseconds CompletionTimeout
        {
            get { return GetPropValue(() => CompletionTimeout, 0); }
            set { SetPropValue(() => CompletionTimeout, value); }
        }

        /// <summary>
        /// Set to true if completed
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public ManualResetEvent WaitCompleteHandle
        {
            get { return _waitComplete; }
        }


        /// <summary>
        /// Set to true if completed
        /// </summary>
        [XmlIgnore]
        public bool Completed
        {
            set
            {
                if (value)
                {
                    // Let is pass
                    _waitComplete.Set();
                }
                else
                {
                    // Block thread
                    _waitComplete.Reset();
                }
            }
        }
        #endregion Public Properties


        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GWorkPiece()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GWorkPiece(string name)
            : base(name)
        {
        }
        #endregion Constructors

        /// <summary>
        /// Resets this component.  Prepares for execution
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }

        protected string[] _triggerPointIDList = null;
        /// <summary>
        /// TriggerPointID List for this MoveWorkpiece
        /// </summary>
        [XmlIgnore]
        public string[] TriggerPointIDList
        {
            get { return _triggerPointIDList; }
            set { _triggerPointIDList = value; }
        }

        /// <summary>
        /// Call this to wait for the last trigger to complete
        /// </summary>
        /// <returns>false if timeout occurs</returns>
        public bool WaitForComplete()
        {
            bool bReceivedSignal = true;
            if (CompletionTimeout.ToInt > 0)
            {
                //System.Diagnostics.Debug.WriteLine("Waiting for complete");
                long timeout = U.DateTimeNow + U.MSToTicks(CompletionTimeout.ToInt);
                do
                {
                    
                    U.SleepWithEvents(20);
                    bReceivedSignal = _waitComplete.WaitOne(0);
                    //System.Diagnostics.Debug.WriteLine("Waiting Complete(20 ms) Thread={0}", Thread.CurrentThread.ManagedThreadId);
                } while (!bReceivedSignal && U.DateTimeNow < timeout);
                //System.Diagnostics.Debug.WriteLine(string.Format("Complete occured={0}", bRet));
                if (!bReceivedSignal)
                {
                    U.LogPopup("Timeout waiting for Workpiece completion");
                }
            }
            AddTimingElement("WaitForComplete");
            return bReceivedSignal;
        }

    }
}
