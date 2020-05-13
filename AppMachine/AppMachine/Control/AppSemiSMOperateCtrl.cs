using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MCore;
using MCore.Comp;
using MCore.Comp.SMLib;
using MCore.Controls;
using AppMachine.Comp.Station;

namespace AppMachine.Control
{
    public partial class AppSemiSMOperateCtrl : UserControl
    {

        private System.Windows.Forms.Control _dockParentControl;
        private Size _dockSize;
        private Form _floatingForm = null;

        public AppSemiSMOperateCtrl()
        {
            InitializeComponent();

            _floatingForm = new Form();
            _floatingForm.ShowInTaskbar = false;
            _floatingForm.TopMost = true;
            _floatingForm.MaximizeBox = false;
            _floatingForm.MinimizeBox = false;
            _floatingForm.FormBorderStyle = FormBorderStyle.FixedSingle;

            _floatingForm.FormClosing += new FormClosingEventHandler(_floatingForm_FormClosing);
            _floatingForm.Hide();
            InitialSemiAutoOperationGUI();
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


        private void linkLblFloat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLblFloat.Text == "Float")
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
            linkLblFloat.Text = "Float";
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
            linkLblFloat.Text = "Dock";

        }

        private void InitialSemiAutoOperationGUI()
        {
            CompBase _allSemiStateMachine = U.GetComponent(AppConstStaticName.ALL_SEMI_AUTO_STATE_MACHINE) as CompBase;
            CompBase _allStation = U.GetComponent(AppConstStaticName.ALL_STATIONS) as CompBase;
           

            foreach (CompBase childState in _allSemiStateMachine.ChildArray)
            {
                SMStateMachine sm = childState as SMStateMachine;
                if (sm != null)
                {
                    System.Windows.Forms.Panel smStorePanel = new System.Windows.Forms.Panel(); ;
                    smStorePanel.Size = new System.Drawing.Size(1100, 30);
                    AppFloatableSMOperate floatSMFlowChart = new AppFloatableSMOperate();
                    floatSMFlowChart.Bind = sm;
                    switch(sm.Name)
                    {
                        case AppConstStaticName.SM_SEMI_MAIN:
                            {
                                StringCtl plugInCtrlParam = new StringCtl();
                                plugInCtrlParam.BindTwoWay(() => AppMachine.Comp.AppMachine.This.SemiAutoMainParam);
                                floatSMFlowChart.PluginControlParameter(plugInCtrlParam, "Param");
                                floatSMFlowChart.BackColor = Color.Green;
                            }
                            break;
                        case AppConstStaticName.SM_SEMI_RESET:
                            {
                                StringCtl plugInCtrlParam = new StringCtl();
                                plugInCtrlParam.BindTwoWay(() => AppMachine.Comp.AppMachine.This.SemiAutoResetParam);
                                floatSMFlowChart.PluginControlParameter(plugInCtrlParam, "Param");
                                floatSMFlowChart.BackColor = Color.MediumPurple;
                            }
                            break;
                        
                    }
                    
                    smStorePanel.Controls.Add(floatSMFlowChart);
                    flpSemiAutoOpr.Controls.Add(smStorePanel);
                }
            }

        }
    }
}
