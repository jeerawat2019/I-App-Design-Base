using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppMachine.Control;

namespace AppMachine.Comp.Part
{
    public partial class AppPartCarrierCtrl : AppUserControlBase
    {
        #region Standard Pattern
        AppPartCarrier _partCarrier = null;
        Type _tyPartCtrl = null;
        #endregion

        //Add More Application Requirement in Below

        public AppPartCarrierCtrl(AppPartCarrier partCarrier,Type tyPartCtrl):base(partCarrier)
        {
            #region Standard Pattern
            _partCarrier = partCarrier;
            _tyPartCtrl = tyPartCtrl;
            InitializeComponent();
            #endregion

            //Add More Application Requirement in Below
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            foreach (AppPart part in _partCarrier.ChildArray)
            {
                AppUserControlBase childControl =  Activator.CreateInstance(_tyPartCtrl, new object[] { part }) as AppUserControlBase;
                if (childControl != null)
                {
                    flpParts.Controls.Add(childControl);
                }
            }
            #endregion

            //Add More Application Requirement in Below

        }
    }
}
