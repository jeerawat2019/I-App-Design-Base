using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MCore
{
    #region Base Exceptions
    /// <summary>
    /// The general MCL Exception
    /// </summary>
    public class MCoreException : ApplicationException
    {
        private string _procedureName;
        private LogSeverity _logSeverity = LogSeverity.Error;

        /// <summary>
        /// Get the log severity
        /// </summary>
        public LogSeverity Severity
        {
            get { return _logSeverity; }
        }

        /// <summary>
        /// Get the Procedure
        /// </summary>
        public string Procedure
        {
            get { return _procedureName; }
        }
        /// <summary>
        /// Contructor with params and inner exceptions
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="severity"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public MCoreException(Exception innerException,
            LogSeverity severity,
            string msg, params object[] args)
            : base(string.Format(msg, args), innerException)
        {
            _logSeverity = severity;
            _procedureName = "Unknown Source";
            StackTrace st = new StackTrace();
            foreach (StackFrame frame in st.GetFrames())
            {
                MethodBase methodBase = frame.GetMethod();
                if (!methodBase.DeclaringType.IsSubclassOf(typeof(Exception)))
                {
                    _procedureName = string.Format("{0}.{1}", methodBase.DeclaringType.Name, methodBase.Name);
                    break;
                }
            }
        }
        /// <summary>
        /// Contructor with params
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreException(LogSeverity severity,
            string text, params object[] args)
            : this(null, severity, text, args)
        {
        }

        ///// <summary>
        ///// Execute the logging, etc
        ///// </summary>
        ///// <param name="ex"></param>
        //public static void Execute(Exception ex)
        //{
        //    if (ex is MCLException)
        //    {
                    
        //    }
        //    else
        //    {
        //        U.WriteLog(LogSeverity.Error, ex.GetType().Name, ex.Message);
        //        MessageBox.Show(ex.Message, ex.GetType().Name);
        //    }
        //}


        ///// <summary>
        ///// Execute the logging, etc
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <param name="source"></param>
        //public static void Execute(Exception ex, string source)
        //{
        //    Execute(ex, source, null);
        //}

        //private static ReaderWriterLockSlim _MCLExceptionLock = new ReaderWriterLockSlim();
        ///// <summary>
        ///// Execute the logging, etc
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <param name="source"></param>
        ///// <param name="mc"></param>
        //public static void Execute(Exception ex, string source, SMMethodCall mc)
        //{
        //    if (ex == null)
        //    {
        //        U.WriteLogFormat(LogSeverity.Error, "MCLException.Execute", "Unexpected null for exception. Source={0}", source);
        //        return;
        //    }
        //    if (ex is ThreadAbortException)
        //    {
        //        return;
        //    }
        //    _MCLExceptionLock.EnterWriteLock();
        //    try
        //    {
        //        // Don't take in the useless TargetInvocationException
        //        if ((ex is System.Reflection.TargetInvocationException) && ex.InnerException != null)
        //        {
        //            ex = ex.InnerException;
        //        }
        //        if (ex is MCLException)
        //        {
        //            MCLException mclEx = ex as MCLException;
        //            mclEx.Execute(mc, source);
        //        }
        //        else
        //        {
        //            // No MCL exception handling info 
        //            if (mc != null)
        //            {
        //                mc.HandleException(ActionOptions.Pause);
        //                new MCLExceptionErrorPopup(ex, source, string.Format("SM call {0}() reports an error.", mc.MethodName), ActionOptions.DumpProcess).Execute("NonMCL");
        //            }
        //            else
        //            {
        //                new MCLExceptionErrorPopup(ex, source, "Non SM error.", ActionOptions.DumpAll).Execute("NonMCL");
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _MCLExceptionLock.ExitWriteLock();
        //    }
        //}
    //    /// <summary>
    //    /// Execute the exception
    //    /// </summary>
    //    /// <remarks>Log it, pause or stop State machines.  Popup messagees, etc.</remarks>
    //    /// <param name="source"></param>
    //    public void Execute(string source)
    //    {
    //        Execute((SMMethodCall)null, source);
    //    }
    //    /// <summary>
    //    /// Execute the exception
    //    /// </summary>
    //    /// <remarks>Log it, pause or stop State machines.  Popup messagees, etc.</remarks>
    //    /// <param name="mc"></param>
    //    /// <param name="source"></param>
    //    public void Execute(SMMethodCall mc, string source)
    //    {
    //        if (mc != null)
    //        {
        //            mc.HandleException(_actionOptions);
    //        }
        //        if ((_actionOptions & ActionOptions.ViewAll) != 0)
    //        {
    //            // Concatenate Message of all nested Exceptions
    //            string msg = string.Empty;
    //            Exception ex = this;
    //            while (ex != null)
    //            {
    //                if (!string.IsNullOrEmpty(msg))
    //                {
    //                    msg += " ---> ";
    //                }
    //                msg += ex.Message;
    //                ex = ex.InnerException;
    //            }

    //            msg += " (See log for more detail).";
        //            if ((_actionOptions & ActionOptions.Alert) != 0)
    //            {
    //                // Use no-log form
    //                AlertForm.faultAlert = new AlertItem(msg);
    //                MainForm.MainFrm.PostPromptForm(null, "MPT.USN.CommonLib.AlertForm");
    //            }
        //            if ((_actionOptions & ActionOptions.Popup) != 0)
    //            {
    //                MessageBox.Show(msg, _procedureName);
    //            }
        //            _actionOptions &= ~ActionOptions.ViewAll;
    //        }
    //        // If no State Machine options are specified, set to Pause
        //        if ((_actionOptions & ActionOptions.StateAll) == 0)
        //            _actionOptions |= ActionOptions.Pause;
        //        U.WriteLog(_logSeverity, source + "-" + _procedureName, this.ToString(), _actionOptions, _customFileDumps);
    //    }
    }

    /// <summary>
    /// Exception derived class to handle data validation
    /// </summary>
    public class MCoreExceptionPopup : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Popup), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionPopup(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Popup, msg, args) { }
        /// <summary>
        /// Constructor. Default is (Error), (Popup, Pause), (DumpOptions).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionPopup(string msg, params object[] args) : this(null, msg, args) { }

    }

    /// <summary>
    /// Exception derived class to handle data validation
    /// </summary>
    public class MCoreExceptionAlert : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Alert, Pause), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionAlert(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Alert, msg, args) { }

        /// <summary>
        /// Constructor. Default is (Error), (Alert, Pause), (NoDump).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionAlert(string msg, params object[] args) : this(null, msg, args) { }
    }

    /// <summary>
    /// Exception derived class to handle non-pops and alert Errors
    /// </summary>
    public class MCoreExceptionError : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Alert, Pause), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionError(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Error, msg, args) { }

        /// <summary>
        /// Constructor. Default is (Error), (Alert, Pause), (NoDump).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionError(string msg, params object[] args) : this(null, msg, args) { }
    }

    /// <summary>
    /// Exception derived class to handle non-pops and alert Warnings
    /// </summary>
    public class MCoreExceptionWarning : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Alert, Pause), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionWarning(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Warning, msg, args) { }

        /// <summary>
        /// Constructor. Default is (Error), (Alert, Pause), (NoDump).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionWarning(string msg, params object[] args) : this(null, msg, args) { }
    }

    /// <summary>
    /// Exception derived class to handle non-pops and alert Debugs
    /// </summary>
    public class MCoreExceptionDebug : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Alert, Pause), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionDebug(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Debug, msg, args) { }

        /// <summary>
        /// Constructor. Default is (Error), (Alert, Pause), (NoDump).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionDebug(string msg, params object[] args) : this(null, msg, args) { }
    }

    /// <summary>
    /// Exception derived class to handle non-pops and alerts
    /// </summary>
    public class MCoreExceptionInfo : MCoreException
    {
        /// <summary>
        /// Construct with innerException, procedure and message. Default is (Error), (Alert, Pause), (DumpOptions).
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionInfo(Exception innerException, string msg, params object[] args) : base(innerException, LogSeverity.Info, msg, args) { }

        /// <summary>
        /// Constructor. Default is (Error), (Alert, Pause), (NoDump).
        /// </summary>
        /// <param name="msg">String that contins error message to be dispayed</param>
        /// <param name="args">optional argument for string.Format()</param>
        public MCoreExceptionInfo(string msg, params object[] args) : this(null, msg, args) { }
    }

    #endregion


    #region Controller Initialize Fail
    /// <summary>
    ///    Simulate Due to Initialize Fail by replacing class with Sim
    /// </summary>
    public class ForceSimulateException : MCoreException
    {
        /// <summary>
        /// Full constructor
        /// </summary>
        public ForceSimulateException(string text, params object[] args)
            : base(LogSeverity.Warning, text, args)
        {                        
        }
        public ForceSimulateException(Exception ex)
            : base(ex, LogSeverity.Warning, ex.Message)
        {
        }
        public ForceSimulateException(Exception ex, string msg, params object[] args)
            : base(ex, LogSeverity.Warning, msg, args)
        {
        }

    }
    #endregion

}
