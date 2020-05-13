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
    public partial class StopEditorForm : Form
    {
        private SMContainerPanel _containerPanel = null;
        private SMExit _stopItem = null;
        public StopEditorForm(SMContainerPanel containerPanel, SMExit stopItem)
        {
            _containerPanel = containerPanel;
            _stopItem = stopItem;
            InitializeComponent();
            tbText.Text = _stopItem.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_stopItem.Text != tbText.Text)
            {
                U.LogChange(_stopItem.Nickname, _stopItem.Text, tbText.Text);
                _stopItem.Text = tbText.Text;
                _containerPanel.Redraw(_stopItem);
            }
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_stopItem);
                Close();
            }
        }
    }
}
