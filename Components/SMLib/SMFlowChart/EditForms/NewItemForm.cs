using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp.SMLib.Flow;

namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    public partial class NewItemForm : Form
    {
        private SMContainerPanel _containerPanel = null;
        private PointF _ptGridPt = Point.Empty;
        private static string _sel = string.Empty;
        public NewItemForm(SMContainerPanel containerPanel, PointF ptGridLoc)
        {
            _containerPanel = containerPanel;
            _ptGridPt = ptGridLoc;
            InitializeComponent();
            rbAction.Tag = typeof(SMActionFlow);
            rbDecision.Tag = typeof(SMDecision);
            rbStop.Tag = typeof(SMExit);
            rbSubroutine.Tag = typeof(SMSubroutine);
            rbStopReturn.Tag = typeof(SMReturnStop);
            rbTransition.Tag = typeof(SMTransition);
            rbActTrans.Tag = typeof(SMActTransFlow);
            if (_containerPanel.FlowContainer is SMSubroutine)
            {
                rbStop.Text = "Return";
                rbStopReturn.Show();
            }
            else
            {
                rbStop.Text = "Stop";
                rbStopReturn.Hide();
            }
            if (string.IsNullOrEmpty(_sel))
            {
                _sel = rbAction.Name;
            }            
            (gbFlowItems.Controls[_sel] as RadioButton).Checked = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _containerPanel.AddNewFlowItem((gbFlowItems.Controls[_sel] as RadioButton).Tag as Type, _ptGridPt, tbText.Text);
            this.Close();
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            _sel = (sender as RadioButton).Name;
        }

 
    }
}
