using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

using MCore.Comp.SMLib.Path;

namespace MCore.Comp.SMLib.Flow
{
    /// <summary>
    /// Class to manage a decision
    /// </summary>
    /// <remarks></remarks>
    public class SMDecision : SMFlowContainer
    {


        #region privates

        private PropertyWrapper _propWrapper = null;
        private Expression<Func<bool>> _propertyExpression = null;
        private bool _needsRebuild = true;
        private volatile bool _ignoreIDChange = false;

        #endregion

        /// <summary>
        /// The time out for decision wait
        /// </summary>

        public int WaitTimeoutMS { get; set; }


        /// <summary>
        /// Enable Flow Timeout to Stop Path
        /// </summary>
        public bool FlowTimeoutToStopPath { get; set; }
       

        #region Serialized Publics
        /// <summary>
        /// The conditional ID
        /// </summary>
        //[XmlIgnore]
        public string ConditionID
        {
            get { return GetPropValue(() => ConditionID, string.Empty); }
            set 
            {
                if (SetPropValue(() => ConditionID, value) && !_ignoreIDChange)
                {
                    _needsRebuild = true;
                    Rebuild();
                }
            }
        }



        public bool UseDryRunLogic
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => UseDryRunLogic, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => UseDryRunLogic, value); }
        }


        public bool DryRunLogic
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => DryRunLogic, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => DryRunLogic, value); }
        }

        #endregion Serialized Publics

        [Browsable(true)]
        [ReadOnly(true)]
        [XmlIgnore]
        public bool HasCallback
        {
            get { return GetPropValue(() => HasCallback, false); }
            set { SetPropValue(() => HasCallback, value); }
        }

        [Browsable(true)]
        [ReadOnly(true)]
        [XmlIgnore]
        public bool HasDecision
        {
            get { return GetPropValue(() => HasDecision, false); }
            set { SetPropValue(() => HasDecision, value); }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMDecision()
        {
        }

        /// <summary>
        /// Manual creation Constructor
        /// </summary>
        /// <param name="text"></param>
        public SMDecision(string text)
            : base(text)
        {
            // Must have a Path in and 2 paths out
            this[eDir.Up] = new SMPathOutPlug();
            this[eDir.Down] = new SMPathOutBool(0.5F, false);
            this[eDir.Right] = new SMPathOutBool(0.5F, true);
            this[eDir.Left] = new SMPathOutStop(-0.5F);
        }

        #endregion Constructors

        #region Overrides

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
            text += ConditionID;
            return text;
        }
        /// <summary>
        /// Clone this component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public override CompBase Clone(string name, bool bRecursivley)
        {
            SMDecision newComp = base.Clone(name, bRecursivley) as SMDecision;
            newComp._ignoreIDChange = true;
            newComp.ConditionID = ConditionID;
            newComp._ignoreIDChange = false;
            return newComp;
        }
        /// <summary>
        /// Destroy this Component
        /// </summary>
        public override void Destroy()
        {
            DoDestroy();
            base.Destroy();
        }

        /// <summary>
        /// Runs this flow item
        /// </summary>
        /// <returns>Returns the path to take from here</returns>
        public override SMPathOut Run()
        {
            lock (this)
            {
                // If is a flow container
                if (HasChildren)
                {
                    // Run as a container
                    return base.Run();
                }
                DoRebuild();
                // Evaluate
                if (_propWrapper == null)
                {
                    return null;
                }

                //Comment Out by Kasem 15-May-2019
                //bool value = (bool)_propWrapper.Invoke();
                
                bool value = false;
                //Implement Dry Run Signal
                if(UseDryRunLogic && CompMachine.s_machine.IsDryRun)
                {
                    value = this.DryRunLogic;
                }
                else
                {
                    value = (bool)_propWrapper.Invoke();
                }

                foreach (SMPathOut pathOut in _pathList)
                {
                    SMPathOutBool pathOutBool = (pathOut as SMPathOutBool);
                    if (pathOutBool != null)
                    {
                        pathOutBool.ApplyPathDelay(ParentContainer);
                        if (pathOutBool.True == value)
                        {
                            return pathOut;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Rebuild the internals
        /// </summary>
        public override void Rebuild()
        {
            _needsRebuild = true;
            DoRebuild();
        }

        #endregion Overrides

        private string GetFullID()
        {
            string scopeID = ParentContainer.ScopeID;
            string prevScopeID = ParentContainer.PrevScopeID;
            if (scopeID != prevScopeID)
            {
                // Modify the Condition ID if Scope has changed

                string fullID = string.Empty;
                if (string.IsNullOrEmpty(prevScopeID))
                {
                    fullID = ConditionID;
                }
                else
                {
                    fullID = string.Format("{0}.{1}", prevScopeID, ConditionID);
                }
                _ignoreIDChange = true;
                if (!string.IsNullOrEmpty(scopeID))
                {
                    ConditionID = fullID.Substring(scopeID.Length + 1);
                }
                else
                {
                    ConditionID = fullID;
                }
                _ignoreIDChange = false;
            }

            if (!string.IsNullOrEmpty(scopeID) && !string.IsNullOrEmpty(ConditionID))
            {
                return string.Format("{0}.{1}", scopeID, ConditionID);
            }
            return ConditionID;
        }
        private void DoDestroy()
        {
            if (_propWrapper != null && _propertyExpression != null)
            {
                U.UnRegisterOnChanged(_propertyExpression, OnBoolPropertyChanged);
                _propertyExpression = null;
            }

        }
        private void DoRebuild()
        {
            lock (this)
            {
                if (!_needsRebuild || ParentContainer == null)
                {
                    return;
                }
                bool hasCallback = false;
                HasDecision = !string.IsNullOrEmpty(ConditionID);
                DoDestroy();
                _propWrapper = PropertyWrapper.Create(GetFullID(), typeof(bool), OnPropertyChanged);
                if (_propWrapper != null && _propWrapper.Target is CompBase)
                {
                    _propertyExpression = _propWrapper.GetGetPropExpression<bool>() as Expression<Func<bool>>;
                    if (_propertyExpression != null)
                    {
                        U.RegisterOnChanged(_propertyExpression, OnBoolPropertyChanged);
                        //(_propWrapper.Target as CompBase).RegisterOnChanged(_propertyExpression, OnBoolPropertyChanged);
                        hasCallback = true;
                    }
                    else
                    {
                        _propWrapper = null;
                    }
                }
                HasCallback = hasCallback;
                _needsRebuild = false;
            }
        }
        /// <summary>
        /// Check if the scope is doable
        /// </summary>
        /// <param name="proposedScope"></param>
        /// <returns></returns>
        public override string ScopeCheck(string proposedScope)
        {
            if (proposedScope == SMFlowContainer.NOSCOPE || string.IsNullOrEmpty(ConditionID) || ConditionID[0] == '(')
            {
                return proposedScope;
            }
            int lastPeriod = ConditionID.LastIndexOf('.');
            if (lastPeriod < 0)
            {
                U.LogPopup("Expected a period. {0}", ConditionID);
                return proposedScope;
            }
            string propPath = ConditionID.Substring(0, lastPeriod);

            return SMFlowContainer.DetermineScope(proposedScope, propPath);
        }

        public void SwitchLogic()
        {
            _pathList.Where(c => c is SMPathOutBool).Cast<SMPathOutBool>().ToList().ForEach(c => c.SwitchLogic());
        }
        private MethodInvoker _changeNotifier = null;
        private object _lockNotifier = new object();

        public void AddNotifier(MethodInvoker changeNotifier)
        {
            lock (_lockNotifier)
            {
                _changeNotifier = changeNotifier;
            }
        }
        public void ClearNotifier()
        {
            lock (_lockNotifier)
            {
                _changeNotifier = null;
            }
        }

        private void OnBoolPropertyChanged(bool val)
        {
            OnPropertyChanged();
        }

        public void OnPropertyChanged()
        {
            lock (_lockNotifier)
            {
                if (_changeNotifier != null)
                {
                    // Notify the container that the bool value has changed
                    _changeNotifier();
                }
            }
        }
    }
}


