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
using AppMachine.Comp.CommonParam;


namespace AppMachine.Panel
{
    public partial class AppCommonParamPanel : AppUserControlBase
    {
        public static AppCommonParamPanel This = null;
        public AppCommonParamPanel()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;
            pgCommonParam.SelectedObject = AppCommonParam.This;
            #endregion
            

        }

        public override void RefreshData()
        {
            //Do Refresh Data Here 
        }
    }
}
