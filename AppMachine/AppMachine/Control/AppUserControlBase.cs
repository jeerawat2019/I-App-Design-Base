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

namespace AppMachine.Control
{
    public partial class AppUserControlBase : UserControl
    {
        #region Standard Pattern
        private bool _firstContruction = true;
        private CompBase _comp = null;
        public bool PreventDispose = false;

        private delegate void _delParamVoid();
       

        public AppUserControlBase()
        {
            this.Handle.ToString();
            InitializeComponent();

          
        }

        public AppUserControlBase(CompBase comp)
        {
            _comp = comp;
            this.Handle.ToString();
            InitializeComponent();
        }


        /// <summary>
        /// Initializing 
        /// </summary>
        protected virtual void Initializing()
        {
            //Override method in sub class
            
        }

       

        /// <summary>
        /// Release resource
        /// </summary>
        public virtual void RecursiveDispose(System.Windows.Forms.Control parentControl)
        {
            if (parentControl.Controls.Count > 0)
            {
                foreach (System.Windows.Forms.Control childControl in parentControl.Controls)
                {
                    RecursiveDispose(childControl);
                }
                if (parentControl is AppUserControlBase)
                {
                    (parentControl as AppUserControlBase).Dispose();
                }
                parentControl.Dispose();
            }
            else
            {
                if (parentControl is AppUserControlBase)
                {
                    (parentControl as AppUserControlBase).Dispose();
                }
                parentControl.Dispose();
            }
        }

        protected virtual void UserControlBase_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent == null && !PreventDispose)
            {
                RecursiveDispose(sender as System.Windows.Forms.Control);
            }
            else if (_firstContruction)
            {
                _firstContruction = false;
                IntPtr pointerHandle = this.Handle;
                Initializing();
            }
        }

        public virtual void RefreshData()
        {
            //Override Refresh Data on Sub Class
        }

        #endregion 

        //Add More Application Requirement in Below
        

    }
}
