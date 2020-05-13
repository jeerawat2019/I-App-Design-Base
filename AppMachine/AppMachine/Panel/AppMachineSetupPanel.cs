using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;

using AppMachine.Control;
using AppMachine.Comp.Station;
using AppMachine.Panel.SubPanel;


namespace AppMachine.Panel
{
    public partial class AppMachineSetupPanel : AppUserControlBase
    {
        public static AppMachineSetupPanel This = null;
        public AppMachineSetupPanel()
        {
            
        }

        protected override void Initializing()
        {
            InitializeComponent();
            This = this;
            tpFeedSetup.Controls.Add(new AppDemoFeedSetupPanel() { Dock = DockStyle.Fill});
            tpVisionSetup.Controls.Add(new AppDemoVisonSetupPanel() { Dock = DockStyle.Fill });
            tpSemiAutoOpr.Controls.Add(new AppSemiSMOperateCtrl() { Dock = DockStyle.Fill });
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.AnySMRunning, AnySMRunningOnChange);
        }

        private void AnySMRunningOnChange(bool anySMRuniing)
        {
            foreach(TabPage tp in tcMachineSetup.TabPages)
            {
                tp.Controls[0].Enabled = !anySMRuniing;
            }
        }

        public override void RefreshData()
        {
            //Do Refresh Data Here 
        }
    }
}
