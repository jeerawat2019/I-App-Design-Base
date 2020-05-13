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
    public partial class GridEditForm : Form
    {
        private SMContainerPanel _containerPanel = null;
        private PointF _ptGridPt = Point.Empty;
        private static string _sel = string.Empty;
        public GridEditForm(SMContainerPanel containerPanel, PointF ptGridLoc)
        {
            _containerPanel = containerPanel;
            _ptGridPt = ptGridLoc;
            InitializeComponent();
          
        }
        private void btnInsertRowBefore_Click(object sender, EventArgs e)
        {
            _containerPanel.InsertGrid(_ptGridPt, SMContainerPanel.eInsertGridMode.rowBefore);
            this.Close();
        }

        private void btnInsertRowAfter_Click(object sender, EventArgs e)
        {
            _containerPanel.InsertGrid(_ptGridPt, SMContainerPanel.eInsertGridMode.rowAfter);
            this.Close();
        }


        private void btnInsertColumnBefore_Click(object sender, EventArgs e)
        {
            _containerPanel.InsertGrid(_ptGridPt, SMContainerPanel.eInsertGridMode.columnBefore);
            this.Close();
        }

        private void btnInsertColumnAfter_Click(object sender, EventArgs e)
        {
            _containerPanel.InsertGrid(_ptGridPt, SMContainerPanel.eInsertGridMode.columnAfter);
            this.Close();
        }

      
      
    }
}
