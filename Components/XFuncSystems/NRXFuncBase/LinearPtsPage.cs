using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.XFunc
{
    public partial class LinearPtsPage : UserControl, IComponentBinding<BasicLinear>
    {
        private BasicLinear _basicLinear = null;
        public LinearPtsPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Define the name for this page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Points Editor";
        }
        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BasicLinear Bind
        {
            get { return _basicLinear; }
            set
            {
                _basicLinear = value;
                UpdatePage();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            _basicLinear.Data = dataGridView.DataSource as DataTable;
        }

        public void UpdatePage()
        {
            DataTable dt = _basicLinear.Data;
            dataGridView.DataSource = dt;
            int nVars = dt.Columns.Count;
            for (int iVar = 0; iVar < nVars; iVar++)
            {
                dataGridView.Columns[iVar].Width = 170;
            }
            dataGridView.Columns[nVars-1].ReadOnly = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePage();
        }

        private void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dt = dataGridView.DataSource as DataTable;
            int nVars = dt.Columns.Count;
            double[] dVals = new double[nVars];

            DataGridViewRow row = dataGridView.Rows[e.RowIndex];
            for (int iVar = 0; iVar < nVars; iVar++)
            {
                object objVal = row.Cells[iVar].Value;
                if (!Convert.IsDBNull(objVal))
                {
                    dVals[iVar] = Convert.ToDouble(objVal);
                }
            }
            double error = _basicLinear.GetError(dVals);
            dataGridView.Rows[e.RowIndex].Cells[nVars-1].Value = error;
        }

        private void cbAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
