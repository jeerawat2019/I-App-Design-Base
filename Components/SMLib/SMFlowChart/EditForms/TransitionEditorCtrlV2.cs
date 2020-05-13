using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;
using MCore.Comp.SMLib.Path;
using MDouble;

namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    public partial class TransitionEditorCtrlV2 : UserControl
    {
        
        private string _dummyID = string.Empty;
        /// <summary>
        /// Used for control binding
        /// </summary>
        public string DummyID
        {
            get
            {
                return _dummyID;
            }
            set
            {
                _dummyID = value;
            }
        }


        private string _conditionValue = string.Empty;
        /// <summary>
        /// Used for control binding
        /// </summary>
        public string ConditionaValue
        {
            get
            {
                return _conditionValue;
            }
            set
            {
                _conditionValue = value;
            }
        }


        //private SMContainerPanel _containerPanel = null;
        private SMActTransFlow _actTransFlow = null;
        private SMTransition _currentTransitionItem = null;
      
        public TransitionEditorCtrlV2(SMActTransFlow actTransFlow)
        {
            //_containerPanel = containerPanel;
            _actTransFlow = actTransFlow;
            InitializeComponent();

            mcbTimeoutToStopPath.BindTwoWay(() => _actTransFlow.FlowTimeoutToStopPath);
            mCbLoopTransition.BindTwoWay(() => _actTransFlow.LoopTransitions);
            strTimeOutCaption.BindTwoWay(() => _actTransFlow.TimeOutCaption);
            strTimeOutMsg.BindTwoWay(() => _actTransFlow.TimeOutMessage);
            mDoubleTimeOut.BindTwoWay(() => _actTransFlow.TransTimeOut);
            mDoubleLoopTime.BindTwoWay(() => _actTransFlow.TransLoopTime);
            mcbUseDryRunTrans.BindTwoWay(() => _actTransFlow.UseDryRunTrans);

            GetFlowList(_actTransFlow.StateMachine);
            cmbTransitionID.DataSource = null;
            cmbTransitionID.DataSource = _flowNameList;

            cmbDryRunTransitionID.DataSource = null;
            cmbDryRunTransitionID.DataSource = _flowNameList2;

            if (_actTransFlow != null)
            {
                if (cmbDryRunTransitionID.Items.Contains(_actTransFlow.DryRunTransitionTargetID))
                {
                    cmbDryRunTransitionID.SelectedItem = _actTransFlow.DryRunTransitionTargetID;
                }
                else
                {
                    cmbDryRunTransitionID.SelectedItem = null;
                }

            }


           
            smPropID.BindTwoWay(() => DummyID);

            
            smValueID.BindTwoWay(() => ConditionaValue);

            Rebuild();
            cbOperator.SelectedIndex = 0;

            //gbTransEdit.Enabled = !_actTransFlow.StateMachine.IsRunning;
            //gbCondEdit.Enabled = !_actTransFlow.StateMachine.IsRunning;

        }

        private void btnNewTrans_Click(object sender, EventArgs e)
        {
            SMTransition transition = new SMTransition("");
            string newName = transition.ValidateName(_actTransFlow, "");
            transition.Name = newName;
            transition.ParentContainer = _actTransFlow.ParentContainer;
            transition.StateMachine = _actTransFlow.StateMachine;
            _actTransFlow.AddTransition(transition);
            treeComponents.Nodes.Clear();
            _actTransFlow.Rebuild();
            Rebuild();
        }

        private void btnDeleteTrans_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (_currentTransitionItem != null)
                {
                    _actTransFlow.RemoveTransition(_currentTransitionItem);
                    _currentTransitionItem = null;
                    treeComponents.Nodes.Clear();
                    _actTransFlow.Rebuild();
                    Rebuild();
                }
            }
        }

        private void btnMorePrioruty_Click(object sender, EventArgs e)
        {
            if(_currentTransitionItem == null)
            {
                return;
            }

            int currentIndex = _actTransFlow.TransitionList.IndexOf(_currentTransitionItem);
            if(currentIndex <= 0)
            {
                return;
            }

            SMTransition previousTrans = _actTransFlow.TransitionList[currentIndex - 1];
            _actTransFlow.TransitionList[currentIndex - 1] = _currentTransitionItem;
            _actTransFlow.TransitionList[currentIndex] = previousTrans;
            treeComponents.Nodes.Clear();
            _actTransFlow.Rebuild();
            Rebuild();
        }

        private void btnLessPriority_Click(object sender, EventArgs e)
        {
            if (_currentTransitionItem == null)
            {
                return;
            }

            int currentIndex = _actTransFlow.TransitionList.IndexOf(_currentTransitionItem);
            if (currentIndex >= _actTransFlow.TransitionList.Count-1)
            {
                return;
            }

            SMTransition nextTrans = _actTransFlow.TransitionList[currentIndex + 1];
            _actTransFlow.TransitionList[currentIndex + 1] = _currentTransitionItem;
            _actTransFlow.TransitionList[currentIndex] = nextTrans;
            treeComponents.Nodes.Clear();
            _actTransFlow.Rebuild();
            Rebuild();
        }


        private void Rebuild()
        {
            try
            { 
                treeComponents.BeginUpdate();
                foreach (SMTransition transition in _actTransFlow.TransitionList)
                {

                    TreeNode nd = null;
                    if (transition.TransitionTargetID != "")
                    {
                        nd = treeComponents.Nodes.Add(transition.Name, "Goto " + transition.TransitionTargetID);
                    }
                    else
                    {
                        nd = treeComponents.Nodes.Add(transition.Name, transition.Name);
                    }
                    nd.Tag = transition;
                    transition.RefNode = nd;
                    BuildTree(nd.Nodes, transition);
                }
                treeComponents.ExpandAll();
                treeComponents.EndUpdate();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not rebuild component tree.");
            }
        }

        private void BuildTree(TreeNodeCollection treeNode, CompBase compParent)
        {
            if (compParent.HasChildren)
            {
                foreach (CompBase compChild in compParent.ChildArray)
                {
                    TreeNode nd = null;
                    if (treeNode.ContainsKey(compChild.ID))
                    {
                        nd = treeNode.Find(compChild.ID, false)[0];
                        nd.Tag = compChild;
                    }
                    else
                    {
                        string nodeDisplayText = "";
                        if(compChild is SMAndCond)
                        {
                            nodeDisplayText = "AND";
                        }
                        else if (compChild is SMOrCond)
                        {
                            nodeDisplayText = "OR";
                        }
                        else if( compChild is SMSubCond)
                        {
                            nodeDisplayText = string.Format("{0} {1} [{2}]", (compChild as SMSubCond).ConditionID, (compChild as SMSubCond).OperatorString, (compChild as SMSubCond).ConditionValueString);
                        }
                        else
                        {
                            nodeDisplayText = compChild.Name;
                        }

                        nd = treeNode.Add(compChild.ID, nodeDisplayText);
                        nd.Tag = compChild;
                    }

                    BuildTree(nd.Nodes, compChild);
                    (compChild as SMSubCondBase).RefNode = nd;
                }
            }
        }

        private void btnAddCond_Click(object sender, EventArgs e)
        {
            AddSubCondition();
        }

        private void btnAddAND_Click(object sender, EventArgs e)
        {
            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                selComp = null;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
                if(selComp is SMTransition)
                {
                    _currentTransitionItem = selComp as SMTransition;
                }
                else
                {
                    _currentTransitionItem = GetTransitionParent(selComp);
                }
            }

            if(selComp == null)
            {
                return;
            }

            //Prevent incorrect added
            if (((selComp is SMTransition) && ((selComp as SMTransition).ChildArray != null && (selComp as SMTransition).ChildArray.Length != 0)) ||
                selComp is SMSubCond)
            {
                return;
            }


            selComp.Add(new SMAndCond());
            treeComponents.Nodes.Clear();
            Rebuild();
        }

        private SMTransition GetTransitionParent(CompBase subCon)
        {
            if(subCon.Parent is SMTransition)
            {
                return subCon.Parent as SMTransition;
            }
            else if(subCon.Parent is SMSubCondBase)
            {
                return GetTransitionParent(subCon.Parent as SMSubCondBase);
            }
            return null;
        }

        private void btnAddOR_Click(object sender, EventArgs e)
        {
            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                selComp = null;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            if (selComp == null)
            {
                return;
            }


            //Prevent incorrect added
            if (((selComp is SMTransition) && ((selComp as SMTransition).ChildArray != null && (selComp as SMTransition).ChildArray.Length != 0)) ||
                selComp is SMSubCond)
            {
                return;
            }



            selComp.Add(new SMOrCond());
            treeComponents.Nodes.Clear();
            Rebuild();
        }

        private void btnDelCond_Click(object sender, EventArgs e)
        {
            DeletedCond();
        }

     

        private void AddSubCondition()
        {
            if (smPropID.ID == null || smPropID.ID == "")
            {
                return;
            }

            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                selComp = null;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if (((selComp is SMTransition) && ((selComp as SMTransition).ChildArray != null && (selComp as SMTransition).ChildArray.Length != 0)) ||
                selComp is SMSubCond)
            {
                return;
            }

            if (ConditionaValue =="")
            {
                MessageBox.Show("Please sepicify condition value.");
                return;
            }

            //System.Reflection.PropertyInfo propInfo = U.GetPropertyInfo(smPropID.ID);
            //Type propType = propInfo.PropertyType;

            //try
            //{
            //    TypeDescriptor.GetConverter(propType).ConvertFromString(ConditionaValue);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    return;
            //}
        

            SMSubCond subCond = new SMSubCond();
            subCond.ConditionID = smPropID.ID;
            subCond.ConditionValueString = ConditionaValue;
            subCond.OperatorString = cbOperator.Text;

            try
            {
                subCond.Rebuild();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }


            selComp.AppendAt(null, subCond);

            treeComponents.Nodes.Clear();
            Rebuild();

            smPropID.PrimaryStatement.Clear();
            DummyID = String.Empty;
            
            smPropID.BindTwoWay(() => DummyID);

            smValueID.PrimaryStatement.Clear();
            ConditionaValue = String.Empty;
            
            smValueID.BindTwoWay(() => ConditionaValue);

            cbOperator.SelectedIndex = 0;
        }


        private void DeletedCond()
        {
            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                return;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            if(!(selComp is SMSubCondBase))
            {
                return;
            }

            selComp.Parent.Remove(selComp);
            treeComponents.Nodes.Clear();
            Rebuild();
        }

        
        private void btnValidate_Click(object sender, EventArgs e)
        {
            _actTransFlow.Rebuild();
            U.AsyncCall(_actTransFlow.ValidateTranstions);
        }

        private void cmbTransitionID_DropDown(object sender, EventArgs e)
        {
            GetFlowList(_actTransFlow.StateMachine);
            cmbTransitionID.DataSource = null;
            cmbTransitionID.DataSource = _flowNameList;
        }

        private void cmbTransitionID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_currentTransitionItem != null)
            {
                _currentTransitionItem.TransitionTargetID = cmbTransitionID.Text;
                TreeNode nd = treeComponents.Nodes[_currentTransitionItem.Name];
                nd.Text = "Goto " + _currentTransitionItem.TransitionTargetID;
            }
        }

        List<String> _flowNameList = new List<string>();
        List<String> _flowNameList2 = new List<string>();
        private void GetFlowList(CompBase SMParent)
        {
            if (SMParent is SMStateMachine)
            {
                _flowNameList.Clear();
                _flowNameList2.Clear();
            }

            SMFlowBase[] _smFlows = null;
           _smFlows = SMParent.FilterByType<SMFlowBase>();
            foreach (SMFlowBase flow in _smFlows)
            {
               
                _flowNameList.Add(flow.ID);
                _flowNameList2.Add(flow.ID);

                if(flow is SMFlowContainer && flow.HasChildren)
                {
                    GetFlowList(flow);
                }
            }
           
        }

        private void treeComponents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Tag is SMTransition)
            {
                _currentTransitionItem = e.Node.Tag as SMTransition;
                
                if (_currentTransitionItem.TransitionTargetID != "")
                {
                    cmbTransitionID.SelectedItem = _currentTransitionItem.TransitionTargetID;
                }
                return;
            }

            SMSubCondBase selSubCond = e.Node.Tag as SMSubCondBase;
            if(!(selSubCond is SMSubCond))
            {
                return;
            }
            SMSubCond subCond = selSubCond as SMSubCond;
            DummyID = subCond.ConditionID;
            smPropID.UnBind();
            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);
            cbOperator.SelectedItem = subCond.OperatorString;

            ConditionaValue = subCond.ConditionValueString;
            smValueID.UnBind();
            //smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);

            _currentTransitionItem = GetTransitionParent(selSubCond);
            if (_currentTransitionItem.TransitionTargetID != "")
            {
                cmbTransitionID.SelectedItem = _currentTransitionItem.TransitionTargetID;
            }

            //if (!ConditionaValue.Contains("(Object)"))
            //{
            //    smValueID.UnBind();
            //    smValueID.BindTwoWay(() => ConditionaValue);
            //    smValueID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            //}
            //else
            //{
            //    smValueID.UnBind();
            //    smValueID.BindTwoWay(() => ConditionaValue);
            //    smValueID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            //}

        }

        private void btnUpdateCond_Click(object sender, EventArgs e)
        {
            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                selComp = null;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if ((selComp == null) || !(selComp is SMSubCond))
            {
                return;
            }

            if (ConditionaValue == "")
            {
                MessageBox.Show("Please sepicify condition value.");
                return;
            }

            
            SMSubCond subCond = selComp as SMSubCond;
            string backCondID = subCond.ConditionID;
            string backCondValue = subCond.ConditionValueString;

            try
            {

                subCond.ConditionValueString = ConditionaValue;
                subCond.ConditionID = smPropID.ID;
                subCond.OperatorString = cbOperator.Text;

                subCond.Rebuild();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                subCond.ConditionValueString = backCondValue;
                subCond.ConditionID = backCondID;
            }

            treeComponents.Nodes.Clear();
            Rebuild();
            smPropID.PrimaryStatement.Clear();

            DummyID = String.Empty;
            
            smPropID.BindTwoWay(() => DummyID);

            ConditionaValue = String.Empty;
            
            smValueID.BindTwoWay(() => ConditionaValue);

            cbOperator.SelectedIndex = 0;
        }

        private void cmbDryRunTransitionID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_actTransFlow != null)
            {
                _actTransFlow.DryRunTransitionTargetID = cmbDryRunTransitionID.Text;
            }
        }

        private void cmbDryRunTransitionID_DropDown(object sender, EventArgs e)
        {
            GetFlowList(_actTransFlow.StateMachine);
            cmbDryRunTransitionID.DataSource = null;
            cmbDryRunTransitionID.DataSource = _flowNameList2;
            cmbDryRunTransitionID.Update();
        }

       
    }
}
