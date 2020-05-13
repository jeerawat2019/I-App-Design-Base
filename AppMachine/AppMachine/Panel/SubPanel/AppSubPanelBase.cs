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
using AppMachine.Comp.IO;

namespace AppMachine.Panel.SubPanel
{
    public partial class AppSubPanelBase : AppUserControlBase
    {
        public static AppSubPanelBase This = null;
        public AppSubPanelBase()
        {
            InitializeComponent();
        }

        protected override void Initializing()
        {
            This = this;

            try
            {
               panelIOStatus.Controls.Add(new AppIOCtrl() { EnaControlOutput = true });
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
            this.Update();
        }
    }
}
