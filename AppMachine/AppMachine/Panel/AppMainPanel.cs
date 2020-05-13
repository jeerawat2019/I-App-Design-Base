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
using MCore.Controls;
using MCore.Comp.SMLib;
using MCore.Comp.SMLib.SMFlowChart;

using AppMachine.Comp;
using AppMachine.Comp.Users;
using AppMachine.Control;

using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Navigator;

namespace AppMachine.Panel
{
    public partial class AppMainPanel : AppUserControlBase
    {

        public static AppMainPanel This = null;
        private AppProductionPanel _productionPanel = null;
        private AppRecipePanel _recipePanel = null;
        private AppGeneralPanel _generalPanel = null;
        private AppSpecPanel _specPanel = null;
        private AppMachineSetupPanel _machineSetupPanel = null;
        private AppCommonParamPanel _commonPararmPanel = null;
        private AppUtilityPanel _utilityPanel = null;
        private AppUsersPanel _userPanel;

        public AppMainPanel()
        {
           
        }


        #region Standard Pattern

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;
            base.Initializing();
            #endregion
        }
        public void RegisterMachineStatusEvent()
        {
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.RunStatus,RunStatusOnChange);
        }
        public void RebuildCompBrowser()
        {
            componentBrowser.Rebuild();
        }

        public void PrepareAllPanel()
        {

            _productionPanel = new AppProductionPanel();
            _productionPanel.Dock = DockStyle.Fill;
            kGroupOperation.Panel.Controls.Add(_productionPanel);
           

            _recipePanel = new AppRecipePanel();
            _recipePanel.Dock = DockStyle.Fill;
            kRecipeManagerPage.Controls.Add(_recipePanel);
            

            _generalPanel = new AppGeneralPanel();
            _generalPanel.Dock = DockStyle.Fill;
            kGeneralSetupPage.Controls.Add(_generalPanel);
            

            _specPanel = new AppSpecPanel();
            _specPanel.Dock = DockStyle.Fill;
            kSpecPage.Controls.Add(_specPanel);
            

            _machineSetupPanel = new AppMachineSetupPanel();
            _machineSetupPanel.Dock = DockStyle.Fill;
            kMachineSetupPage.Controls.Add(_machineSetupPanel);
           

            _commonPararmPanel = new AppCommonParamPanel();
            _commonPararmPanel.Dock = DockStyle.Fill;
            kCommonParamPage.Controls.Add(_commonPararmPanel);
            

            _utilityPanel = new AppUtilityPanel();
            _utilityPanel.Dock = DockStyle.Fill;
            kGroupUtility.Panel.Controls.Add(_utilityPanel);
            

            _userPanel = new AppUsersPanel();
            _userPanel.Dock = DockStyle.Fill;
            kUsersManagerPage.Controls.Add(_userPanel);
            

            //Default Selection Page
            try
            {
                kNavigatorMain.SelectedIndex = 0;
                kNavigatorMainSetup.SelectedIndex = 0;
                kNavigatorStateMachine.SelectedIndex = 0;
                kNavigatorSemiStateMachine.SelectedIndex = 0;
            }
            catch
            {

            }

        }

        public void AddStateMachinePage(SMStateMachine sm)
        {
            AppFloatableSMFlowChart floatSMFlowChart = new AppFloatableSMFlowChart();
            floatSMFlowChart.Dock = DockStyle.Fill;
            floatSMFlowChart.Bind = sm;
            KryptonPage smPage = new KryptonPage();
            smPage.Text = sm.Name;
            smPage.StateCommon.Tab.Back.Color1 = Color.White;
            smPage.StateCommon.Tab.Back.Color2 = Color.RoyalBlue;
            smPage.Controls.Add(floatSMFlowChart);
            kNavigatorStateMachine.Pages.Add(smPage);
        }

        public void AddSemiStateMachinePage(SMStateMachine sm)
        {
            AppFloatableSMFlowChart floatSMFlowChart = new AppFloatableSMFlowChart();
            floatSMFlowChart.Dock = DockStyle.Fill;
            floatSMFlowChart.Bind = sm;
            KryptonPage smSemiPage = new KryptonPage();
            smSemiPage.Text = sm.Name;
            smSemiPage.StateCommon.Tab.Back.Color1 = Color.White;
            smSemiPage.StateCommon.Tab.Back.Color2 = Color.LightBlue;
            smSemiPage.Controls.Add(floatSMFlowChart);
            kNavigatorSemiStateMachine.Pages.Add(smSemiPage);
        }

        public void AnySMRunningOnChange(bool anySMRuniing)
        {
            kNavigatorMainSetup.Enabled = !anySMRuniing;
            componentBrowser.Enabled = !anySMRuniing;
        }

        public void SetOperatorAccess()
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                foreach (KryptonPage setupPage in kNavigatorMainSetup.Pages)
                {
                    if (setupPage.Controls.Count > 0)
                    {
                        setupPage.Controls[0].Enabled = false;
                    }
                }
                componentBrowser.Enabled = false;
                kGroupUtility.Enabled = false;
                EnableSMOperationPage(false);
            });
        }

        public void SetSupervisorAccess()
        {
            this.BeginInvoke((MethodInvoker)delegate
           {
               foreach (KryptonPage setupPage in kNavigatorMainSetup.Pages)
               {
                   if (setupPage.Controls.Count > 0)
                   {
                       setupPage.Controls[0].Enabled = true;
                   }
               }
               componentBrowser.Enabled = true;
               kGroupUtility.Enabled = true;
               EnableSMOperationPage(true);
           });
        }

        private void EnableSMOperationPage(bool enable)
        {
            foreach (KryptonPage smPage in kNavigatorStateMachine.Pages)
            {
                if (smPage.Controls != null && smPage.Controls[0] is AppFloatableSMFlowChart)
                {
                    (smPage.Controls[0] as AppFloatableSMFlowChart).EnableSMOperation = enable;
                }
            }

            foreach (KryptonPage smPage in kNavigatorSemiStateMachine.Pages)
            {
                if (smPage.Controls != null && smPage.Controls[0] is AppFloatableSMFlowChart)
                {
                    (smPage.Controls[0] as AppFloatableSMFlowChart).EnableSMOperation = enable;
                }
            }
        }

        private void kNavigatorMainSetup_SelectedPageChanged(object sender, EventArgs e)
        {
            if (kNavigatorMainSetup.SelectedPage.Controls[0] != null && kNavigatorMainSetup.SelectedPage.Controls[0] is AppUserControlBase)
            {
                (kNavigatorMainSetup.SelectedPage.Controls[0] as AppUserControlBase).RefreshData();
            }
        }

        private void RunStatusOnChange(AppMachine.Comp.AppMachine.eRunStatus RunStatus)
        {
            switch (RunStatus)
            {
                case AppMachine.Comp.AppMachine.eRunStatus.Running:
                    EnableSMOperationPage(false);

                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Pause:
                    EnableSMOperationPage(false);

                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Stopping:
                    EnableSMOperationPage(false);

                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Stopped:
                    EnableSMOperationPage(AppMachine.Comp.AppMachine.This.CurrentUser.UserLevel == AppEnums.eAccessLevel.Supervisor);

                    break;
            }
        }

        
        #endregion

        

       
        

    }
}
