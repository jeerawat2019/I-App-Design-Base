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

namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    public partial class TransitionEditorForm : Form
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


        private SMContainerPanel _containerPanel = null;
        private SMTransition _transitionItem = null;
      
        public TransitionEditorForm(SMContainerPanel containerPanel, SMTransition transitionItem)
        {
            _containerPanel = containerPanel;
            _transitionItem = transitionItem;
            InitializeComponent();

            mcbTimeoutToStopPath.BindTwoWay(() => _transitionItem.FlowTimeoutToStopPath);
            mCbLoopTransition.BindTwoWay(() => _transitionItem.LoopTransition);
            strTimeOutCaption.BindTwoWay(() => _transitionItem.TimeOutCaption);
            strTimeOutMsg.BindTwoWay(() => _transitionItem.TimeOutMessage);
            mDoubleTimeOut.BindTwoWay(() => _transitionItem.TransTimeOut);
            mDoubleLoopTime.BindTwoWay(() => _transitionItem.TransLoopTime);
            mcbUseDryRunTrans.BindTwoWay(() => _transitionItem.UseDryRunTrans);

            tbText.Text = _transitionItem.Text;
            Text = transitionItem.Name;

            GetFlowList(_transitionItem.StateMachine);
            cmbTransitionID.DataSource = null;
            cmbTransitionID.DataSource = _flowNameList;

            if (_transitionItem.TransitionTargetID != "")
            {
                cmbTransitionID.SelectedItem = _transitionItem.TransitionTargetID;
            }

            cmbDryRunTransitionID.DataSource = null;
            cmbDryRunTransitionID.DataSource = _flowNameList2;

            if (_transitionItem != null)
            {
                if (cmbDryRunTransitionID.Items.Contains(_transitionItem.DryRunTransitionTargetID))
                {
                    cmbDryRunTransitionID.SelectedItem = _transitionItem.DryRunTransitionTargetID;
                }
                else
                {
                    cmbDryRunTransitionID.SelectedItem = null;
                }

            }

            smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);

            Rebuild();
            cbOperator.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_transitionItem.Text != tbText.Text)
            {
                U.LogChange(string.Format("{0}.Label", _transitionItem.Nickname), _transitionItem.Text, tbText.Text);
                _transitionItem.Text = tbText.Text;

            }

            _transitionItem.Rebuild();
            _containerPanel.Redraw(_transitionItem);

            Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_transitionItem);
                Close();
            }
        }

       

        private void Rebuild()
        {
            try
            { 
                treeComponents.BeginUpdate();
                BuildTree(treeComponents.Nodes,_transitionItem);
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
                selComp = _transitionItem;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if ((selComp == _transitionItem && (_transitionItem.ChildArray != null && _transitionItem.ChildArray.Length != 0)) ||
                selComp is SMSubCond)
            {
                return;
            }


            selComp.Add(new SMAndCond());

            Rebuild();
        }

        private void btnAddOR_Click(object sender, EventArgs e)
        {
            CompBase selComp = null;
            if (treeComponents.SelectedNode == null)
            {
                selComp = _transitionItem;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if ((selComp == _transitionItem && (_transitionItem.ChildArray != null && _transitionItem.ChildArray.Length != 0)) ||
                selComp is SMSubCond)
            {
                return;
            }



            selComp.Add(new SMOrCond());

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
                selComp = _transitionItem;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if((selComp == _transitionItem && (_transitionItem.ChildArray !=  null && _transitionItem.ChildArray.Length != 0)) || 
                selComp is SMSubCond)
            {
                return;
            }


            if(ConditionaValue =="")
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
            smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            smValueID.PrimaryStatement.Clear();
            ConditionaValue = String.Empty;
            smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
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

            if (!(selComp is SMSubCondBase))
            {
                return;
            }

            selComp.Parent.Remove(selComp);
            treeComponents.Nodes.Clear();
            Rebuild();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            foreach(CompBase comp in _transitionItem.ChildArray )
            {
                if(comp is SMSubCondBase)
                {
                    (comp as SMSubCondBase).Validate();
                }
            }
        }

        private void cmbTransitionID_DropDown(object sender, EventArgs e)
        {
            GetFlowList(_transitionItem.StateMachine);
            cmbTransitionID.DataSource = null;
            cmbTransitionID.DataSource = _flowNameList;
        }

        private void cmbTransitionID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _transitionItem.TransitionTargetID = cmbTransitionID.Text;
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
           SMSubCondBase selSubCond = e.Node.Tag as SMSubCondBase;
            if(!(selSubCond is SMSubCond))
            {
                return;
            }
            SMSubCond subCond = selSubCond as SMSubCond;
            DummyID = subCond.ConditionID;
            smPropID.UnBind();
            smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);
            cbOperator.SelectedItem = subCond.OperatorString;

            ConditionaValue = subCond.ConditionValueString;
            smValueID.UnBind();
            smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);
            


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
                selComp = _transitionItem;
            }
            else
            {
                selComp = treeComponents.SelectedNode.Tag as CompBase;
            }

            //Prevent incorrect added
            if (!(selComp is SMSubCond))
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
            smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            ConditionaValue = String.Empty;
            smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);

            cbOperator.SelectedIndex = 0;
        }

        private void btnClearSelect_Click(object sender, EventArgs e)
        {
            smPropID.PrimaryStatement.Clear();
            DummyID = String.Empty;
            smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            smValueID.PrimaryStatement.Clear();
            ConditionaValue = String.Empty;
            smValueID.BindTwoWay(() => ConditionaValue);
        }

        private void TransitionEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cmbDryRunTransitionID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _transitionItem.TransitionTargetID = cmbDryRunTransitionID.Text;
        }

        private void cmbDryRunTransitionID_DropDown(object sender, EventArgs e)
        {
            GetFlowList(_transitionItem.StateMachine);
            cmbDryRunTransitionID.DataSource = null;
            cmbDryRunTransitionID.DataSource = _flowNameList2;
        }
    }
}
