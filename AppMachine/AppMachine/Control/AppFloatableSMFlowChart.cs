using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCore.Comp.SMLib.SMFlowChart;

namespace AppMachine.Control
{
    public partial class AppFloatableSMFlowChart : SMFlowChartCtlBasic
    {
        #region Standard Pattern
        private System.Windows.Forms.Control _dockParentControl;
        private Size _dockSize;
        private Form _floatingForm;

        public AppFloatableSMFlowChart()
        {
            InitializeComponent();

            _floatingForm = new Form();
            _floatingForm.ShowInTaskbar = false;
            _floatingForm.TopMost = false;

            _floatingForm.FormClosing += new FormClosingEventHandler(_floatingForm_FormClosing);
            _floatingForm.Hide();
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected override void DestroyHandle()
        {
            if (_floatingForm != null)
            {
                _floatingForm.FormClosing -= new FormClosingEventHandler(_floatingForm_FormClosing);
                _floatingForm.Dispose();
            }

 	      base.DestroyHandle();
        }

        private void _floatingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoDocking();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }



        private void btnFloatDock_Click(object sender, EventArgs e)
        {
            if (btnFloatDock.Text == "Float")
            {
                DoFloating();
            }
            else
            {
                DoDocking();
            }
        }

      
        private void DoDocking()
        {
            if (this.Parent != _floatingForm)
            {
                return;
            }
     
            _floatingForm.Controls.Remove(this);
            this.Size = _dockSize;
            _dockParentControl.Controls.Add(this);
            _floatingForm.Hide();
            btnFloatDock.Text = "Float";
        }

        /// <summary>
        /// Float itself
        /// </summary>
        private void DoFloating()
        {
         
            if (this.Parent == _floatingForm)
            {
                return;
            }

            _dockParentControl = this.Parent;
            _dockSize = this.Size;
            this.Parent.Controls.Remove(this);
            _floatingForm.ClientSize = this.Size;
            _floatingForm.Controls.Add(this);
            _floatingForm.Show();
            btnFloatDock.Text = "Dock";

        }
        #endregion

        //Add More Application Requirement in Below
    }
}
