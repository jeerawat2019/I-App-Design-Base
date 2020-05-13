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
    public partial class SubroutineEditorForm : Form
    {
        private SMContainerPanel _containerPanel = null;
        private SMSubroutine _subroutineItem = null;
        public SubroutineEditorForm(SMContainerPanel containerPanel, SMSubroutine subroutineItem)
        {
            _containerPanel = containerPanel;
            _subroutineItem = subroutineItem;
            InitializeComponent();
            Text = subroutineItem.Name;
            tbText.Text = _subroutineItem.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_subroutineItem.Text != tbText.Text)
            {
                U.LogChange(_subroutineItem.Nickname, _subroutineItem.Text, tbText.Text);
                _subroutineItem.Text = tbText.Text;
                _containerPanel.Redraw(_subroutineItem);
            }
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This Subroutine Flow item will be deleted.  Are you sure?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _containerPanel.DeleteFlowItem(_subroutineItem);
                Close();
            }
        }
    }
}
