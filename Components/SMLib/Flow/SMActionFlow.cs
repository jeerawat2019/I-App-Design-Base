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

namespace MCore.Comp.SMLib.Flow
{
    /// <summary>
    /// Class to manage the State Machine Action object
    /// </summary>
    public class SMActionFlow : SMFlowBase
    {
        /// <summary>
        /// Gets/sets if rebuild is needed
        /// </summary>
        [XmlIgnore]
        public bool NeedsRebuild { get; set; }

        /// <summary>
        /// Gets/sets if rebuild is needed
        /// </summary>
        [XmlIgnore]
        public bool HasProblem
        {
            get { return GetPropValue(() => HasProblem, false); }
            set { SetPropValue(() => HasProblem, value); }
        }


        public bool DryRunSkipActions
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => DryRunSkipActions, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => DryRunSkipActions, value); }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMActionFlow()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="text"></param>
        public SMActionFlow(string text)
            : base(text)
        {
            // Must have a Path in and a path out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Left] = new SMPathOutStop(-0.5F);
            this[eDir.Down] = new SMPathOut(0.5F);
            this[eDir.Right] = new SMPathOutError(0.5F);
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
            if (HasChildren)
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
            else
            {
                text = Name;
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

        /// <summary>
        /// Check if the scope is doable
        /// </summary>
        /// <param name="proposedScope"></param>
        /// <returns></returns>
        public override string ScopeCheck(string proposedScope)
        {
            if (HasChildren)
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
                validMethods.Clear();
                bool problem = false;
                if (HasChildren)
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
                if (HasChildren)
                {
                    if (NeedsRebuild)
                    {
                        //Rebuild();
                    }

                    //For dry run
                    if (CompMachine.s_machine.IsDryRun && this.DryRunSkipActions)
                    {
                        // Do no action
                    }
                    else
                    {
                        validMethods.ForEach(c => c.RunAsync());
                        //Parallel.ForEach(validMethods, method => method.RunAsync());
                        //// Wait until they all complete
                        WaitHandle.WaitAll(waitHandles);
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

                }
            }

            // Only one out
            return this[typeof(SMPathOut)];
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
    }
}
