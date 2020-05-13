using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppMachine.Control;

using MCore;

namespace AppMachine.Comp.Part
{
    public partial class AppPartCtrl : AppUserControlBase
    {
        #region Standard Pattern
        AppPart _part = null;
        public AppPartCtrl(AppPart part)
        {
            _part = part;
            InitializeComponent();
        }
        #endregion

        //Add More Application Requirement in Below

        protected override void Initializing()
        {
            #region Example GUI Handler
            lblPartId.Text = (_part.PartId + 1).ToString();

            //Example of handle prperty changed event
            OnStatusChanged(_part.PartStatus);
            U.RegisterOnChanged(() => _part.PartStatus, OnStatusChanged);
            #endregion

            //Add More Application Requirement in Below

        }

        #region Example GUI Handler
        //Properties Change Handle Example in Below 
        private void OnStatusChanged(AppEnums.ePartStatus status)
        {
            switch(status)
            {
                case AppEnums.ePartStatus.None:
                    lblPartId.BackColor = Color.Yellow;
                    this.Update();
                    break;
                case AppEnums.ePartStatus.Fail:
                    lblPartId.BackColor = Color.Red;
                    this.Update();
                    break;
                case AppEnums.ePartStatus.Pass:
                    lblPartId.BackColor = Color.Lime;
                    this.Update();
                    break;
                case AppEnums.ePartStatus.Missing:
                    lblPartId.BackColor = Color.DimGray;
                    this.Update();
                    break;
            }
        }
        #endregion

        //Add More Application Requirement in Below

    }
}
