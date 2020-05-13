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
    public partial class DecisionEditorForm : Form
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
        private SMContainerPanel _containerPanel = null;
        private SMDecision _decisionItem = null;
        public DecisionEditorForm(SMContainerPanel containerPanel, SMDecision decisionItem)
        {
            _containerPanel = containerPanel;
            _decisionItem = decisionItem;
            InitializeComponent();
            tbText.Text = _decisionItem.Text;
            Text = decisionItem.Name;
            tbWaitTimeOut.Text = decisionItem.WaitTimeoutMS.ToString();

            foreach(SMPathOut path in _decisionItem.PathArray)
            {
                SMPathOutBool pathOutBool = path as SMPathOutBool;
                if (pathOutBool != null)
                {
                    if (pathOutBool.True)
                    {
                        tbTrueDelay.Text = pathOutBool.PathOutDelayMS.ToString();
                    }
                    else
                    {
                        tbFalseDelay.Text = pathOutBool.PathOutDelayMS.ToString();
                    }
                }
            }
            
            cbNested.Checked = _decisionItem.HasChildren;
            cbFlowTimeouttoStop.Checked = _decisionItem.FlowTimeoutToStopPath;
            DummyID = _decisionItem.ConditionID;
            booleanID.ScopeID = _decisionItem.ParentContainer.ScopeID;


            try
            {
                booleanID.BindTwoWay(() => DummyID);
            }
            catch
            {
                DummyID = "";
                booleanID.BindTwoWay(() => DummyID);
            }


            cbUseDryRunLogic.Checked = _decisionItem.UseDryRunLogic;
            bcbDryRunLogic.SelectedIndex = _decisionItem.DryRunLogic ? 1 : 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_decisionItem.Text != tbText.Text)
            {
                U.LogChange(string.Format("{0}.Label", _decisionItem.Nickname), _decisionItem.Text, tbText.Text);
                _decisionItem.Text = tbText.Text;

            }
            if (_decisionItem.ConditionID != DummyID)
            {
                U.LogChange(string.Format("{0}.ID", _decisionItem.Nickname), _decisionItem.ConditionID, DummyID);
                _decisionItem.ConditionID = DummyID;
            }

            _decisionItem.WaitTimeoutMS = Convert.ToInt32(tbWaitTimeOut.Text);
            foreach (SMPathOut path in _decisionItem.PathArray)
            {
                SMPathOutBool pathOutBool = path as SMPathOutBool;
                if (pathOutBool != null)
                {
                    try
                    {
                        if (pathOutBool.True)
                        {
                            pathOutBool.PathOutDelayMS = Convert.ToInt32(tbTrueDelay.Text);
                        }
                        else
                        {
                            pathOutBool.PathOutDelayMS = Convert.ToInt32(tbFalseDelay.Text);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid entry");
                        return;
                    }
                }
            }
            _containerPanel.Redraw(_decisionItem);
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_decisionItem);
                Close();
            }
        }

        private void cbNested_CheckedChanged(object sender, EventArgs e)
        {

            if (!cbNested.Focused)
            {
                return;
            }

            if (cbNested.Checked)
            {


                if (MessageBox.Show("The condition will be deleted if you make nested.  Are you sure?",
                   "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DummyID = "";
                    booleanID.BindTwoWay(() => DummyID);
                }
                else
                {
                    btnOk.Focus();
                    cbNested.Checked = false;
                    return;
                }

               
                if (!_decisionItem.HasChildren)
                {
                    _decisionItem.Destroy();
                    // Make it nested
                    _decisionItem.Add(new SMStart(string.Empty) { Text = "Enter", GridLoc = new PointF(2.5f, 0.5f) } );
                    _decisionItem.Add(new SMReturnYes(string.Empty) { GridLoc = new PointF(3.5f, 6.5f)});
                    _decisionItem.Add(new SMReturnNo(string.Empty) { GridLoc = new PointF(1.5f, 6.5f)});
                    _decisionItem.Add(new SMReturnStop(string.Empty) { GridLoc = new PointF(0.5f, 3.5f)});
                    _decisionItem.Initialize();
                    _containerPanel.Redraw(_decisionItem);
                }

            }
            else
            {
                if (_decisionItem.HasChildren && 
                    MessageBox.Show("The underlying sub-flow chart will be deleted.  Are you sure?", 
                    "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    while (_decisionItem.HasChildren)
                    {
                        _decisionItem.First.Delete();
                    }
                    _containerPanel.FlowChartCtlBasic.RemoveFlowContainer(_decisionItem as SMFlowContainer);
                    _containerPanel.Redraw(_decisionItem);
                }
                else
                {
                    btnOk.Focus();
                    cbNested.Checked = true;
                }
            }
        }

        private void cbFlowTimeouttoStop_CheckedChanged(object sender, EventArgs e)
        {
            _decisionItem.FlowTimeoutToStopPath = cbFlowTimeouttoStop.Checked;
        }


        private void cbUseDryRunLogic_CheckedChanged(object sender, EventArgs e)
        {
            _decisionItem.UseDryRunLogic = cbUseDryRunLogic.Checked;
        }

        private void bcbDryRunLogic_SelectedIndexChanged(object sender, EventArgs e)
        {
            _decisionItem.DryRunLogic = bcbDryRunLogic.SelectedIndex == 0 ? false : true;
        }

        
    }
}
