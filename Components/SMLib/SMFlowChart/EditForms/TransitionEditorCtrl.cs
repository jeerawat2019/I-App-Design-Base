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
    public partial class TransitionEditorCtrl : UserControl
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
        private SMTransition _transitionItem = null;
      
        public TransitionEditorCtrl(SMActTransFlow actTransFlow, SMTransition transitionItem)
        {
            //_containerPanel = containerPanel;
            _actTransFlow = actTransFlow;
            _transitionItem = transitionItem;
            InitializeComponent();

            tbText.Text = _transitionItem.Text;
            Text = transitionItem.Name;

            GetFlowList(_transitionItem.StateMachine);
            cmbTransitionID.DataSource = null;
            cmbTransitionID.DataSource = _flowNameList;

            if (_transitionItem.TransitionTargetID != "")
            {
                cmbTransitionID.SelectedItem = _transitionItem.TransitionTargetID;
            }

            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            //smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);

            Rebuild();
            cbOperator.SelectedIndex = 0;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_transitionItem.Text != tbText.Text)
            {
                U.LogChange(string.Format("{0}.Label", _transitionItem.Nickname), _transitionItem.Text, tbText.Text);
                _transitionItem.Text = tbText.Text;

            }
            _transitionItem.Rebuild();
            //_containerPanel.Redraw(_transitionItem);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _actTransFlow.RemoveTransition(_transitionItem);
                (this.Parent.Parent as TabControl).TabPages.Remove(this.Parent as TabPage);
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
            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            smValueID.PrimaryStatement.Clear();
            ConditionaValue = String.Empty;
            //smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
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

            selComp.Parent.Remove(selComp);
            treeComponents.Nodes.Clear();
            Rebuild();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //this.Close();
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
        private void GetFlowList(CompBase SMParent)
        {
            if (SMParent is SMStateMachine)
            {
                _flowNameList.Clear();
            }

            SMFlowBase[] _smFlows = null;
           _smFlows = SMParent.FilterByType<SMFlowBase>();
            foreach (SMFlowBase flow in _smFlows)
            {
               
                _flowNameList.Add(flow.ID);

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
            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);
            cbOperator.SelectedItem = subCond.OperatorString;

            ConditionaValue = subCond.ConditionValueString;
            smValueID.UnBind();
            //smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
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
            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            ConditionaValue = String.Empty;
            //smValueID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smValueID.BindTwoWay(() => ConditionaValue);

            cbOperator.SelectedIndex = 0;
        }

        private void btnClearSelect_Click(object sender, EventArgs e)
        {
            smPropID.PrimaryStatement.Clear();
            DummyID = String.Empty;
            //smPropID.ScopeID = _transitionItem.ParentContainer.ScopeID;
            smPropID.BindTwoWay(() => DummyID);

            smValueID.PrimaryStatement.Clear();
            ConditionaValue = String.Empty;
            smValueID.BindTwoWay(() => ConditionaValue);
        }

        private void tbText_TextChanged(object sender, EventArgs e)
        {
            _transitionItem.Text = tbText.Text;
        }
    }
}
