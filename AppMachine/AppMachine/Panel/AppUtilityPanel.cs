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


namespace AppMachine.Panel
{
    public partial class AppUtilityPanel : AppUserControlBase
    {
        public static AppUtilityPanel This = null;
        public AppUtilityPanel()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;
            #endregion

        }

        public override void RefreshData()
        {
            //Do Refresh Data Here
        }
    }
}
