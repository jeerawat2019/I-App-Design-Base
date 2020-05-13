using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;
using MCore.Comp;
using MCore.Comp.VisionSystem;

using AppMachine.Control;
using AppMachine.Comp;
using AppMachine.Comp.Station;
using AppMachine.Comp.IO;
using AppMachine.Comp.Recipe;
using AppMachine.Comp.Part;
using AppMachine.Comp.Users;

namespace AppMachine.Panel
{
    public partial class AppProductionPanel : AppUserControlBase
    {
        public static AppProductionPanel This = null;
        private delegate void _delParamAppPartCarrier(AppPartCarrier carrier);
        private delegate void _delParamVoid();
        private delegate void _delParamInt(int i);
        private delegate void _delParamDouble(double d);


        public AppProductionPanel()
        {

        }




        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;
            #endregion

            #region Example Pattern But Can Change Upto Developer
            panelRedLight.BackColor = Color.Red;

            strOperatorName.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentUser.UserName);
            strOperatorEN.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentUser.UserEN);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.CurrentUser, CurrentUser_OnChange);


            OnInputProductChange(AppMachine.Comp.AppMachine.This.InputsProduct);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.InputsProduct, OnInputProductChange);

            OnFailProductChange(AppMachine.Comp.AppMachine.This.FailedProduct);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.FailedProduct, OnFailProductChange);

            OnYieldChange(AppMachine.Comp.AppMachine.This.Yield);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.Yield, OnYieldChange);

            strProductRecipe.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentProdRecipeName);
            strLotId.BindTwoWay(() => AppMachine.Comp.AppMachine.This.LotId);
            intLotSize.BindTwoWay(() => AppMachine.Comp.AppMachine.This.LotSize);

            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.AnySMRunning, AnySMRunningOnChange);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.RunStatus, RunningStatusOnChange);
            #endregion

            /*Example Tower Light GUI Handle
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.IOList.BoolOutputCollection[AppMachine.AppConstStaticName.GREENLIGHT].Value,
                                GreenLightOnChange);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.IOList.BoolOutputCollection[AppMachine.AppConstStaticName.AMBERLIGHT].Value,
                                AmberLightOnChange);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.IOList.BoolOutputCollection[AppMachine.AppConstStaticName.REDLIGHT].Value,
                                RedLightOnChange);
            */
        }


        #region Standard GUI Handle
        private void OnInputProductChange(int inputCarrier)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new _delParamInt(OnInputProductChange), new object[] { inputCarrier });
                return;
            }

            lblInput.Text = inputCarrier.ToString();
        }

        private void OnFailProductChange(int failCarrier)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new _delParamInt(OnFailProductChange), new object[] { failCarrier });
                return;
            }

            lblFail.Text = failCarrier.ToString();
        }

        private void OnYieldChange(double yield)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new _delParamDouble(OnYieldChange), new object[] { yield });
                return;
            }

            lblYield.Text = yield.ToString("0.00");
        }



        private void AnySMRunningOnChange(bool anySMRuniing)
        {
            panelProductionInput.Enabled = !anySMRuniing;
        }

        private void GreenLightOnChange(bool onValue)
        {
            if (onValue)
            {
                panelGreenLight.BackColor = Color.Lime;
            }
            else
            {
                panelGreenLight.BackColor = Color.DarkGreen;
            }

            panelGreenLight.Update();
        }

        private void AmberLightOnChange(bool onValue)
        {
            if (onValue)
            {
                panelAmberLight.BackColor = Color.Yellow;
            }
            else
            {
                panelAmberLight.BackColor = Color.Olive;
            }
            panelAmberLight.Update();
        }

        private void RedLightOnChange(bool onValue)
        {
            if (onValue)
            {
                panelRedLight.BackColor = Color.Red;
            }
            else
            {
                panelRedLight.BackColor = Color.Maroon;
            }
            panelRedLight.Update();
        }


        private delegate void _delParamRunStatus(AppMachine.Comp.AppMachine.eRunStatus status);
        private void RunningStatusOnChange(AppMachine.Comp.AppMachine.eRunStatus status)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new _delParamRunStatus(RunningStatusOnChange), new object[] { status });
                return;
            }

            switch (status)
            {
                case AppMachine.Comp.AppMachine.eRunStatus.Running:
                    panelGreenLight.BackColor = Color.Lime;
                    panelRedLight.BackColor = Color.Maroon;
                    panelAmberLight.BackColor = Color.Olive;
                    CompRoot.AppStatus("Running");
                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Pause:
                    panelGreenLight.BackColor = Color.DarkGreen;
                    panelRedLight.BackColor = Color.Red;
                    panelAmberLight.BackColor = Color.Olive;
                    CompRoot.AppStatus("Pause");
                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Stopping:
                    panelGreenLight.BackColor = Color.Lime;
                    panelRedLight.BackColor = Color.Maroon;
                    panelAmberLight.BackColor = Color.Olive;
                    CompRoot.AppStatus("Stopping");
                    break;
                case AppMachine.Comp.AppMachine.eRunStatus.Stopped:
                    panelGreenLight.BackColor = Color.DarkGreen;
                    panelRedLight.BackColor = Color.Red;
                    panelAmberLight.BackColor = Color.Olive;
                    CompRoot.AppStatus("Stopped");
                    break;
            }
        }

        private void CurrentUser_OnChange(AppUserInfo currentUser)
        {
            strOperatorName.UnBind();
            strOperatorEN.UnBind();
            strOperatorName.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentUser.UserName);
            strOperatorEN.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentUser.UserEN);
        }

        #endregion GUI Handle


        public override void RefreshData()
        {
            //Do Refresh Data Here
        }

    }
  
}
