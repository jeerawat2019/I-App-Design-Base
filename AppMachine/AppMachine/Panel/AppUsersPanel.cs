using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using MCore;
using MCore.Comp;


using AppMachine.Control;
using AppMachine.Comp.Users;
using MCore.Controls;


namespace AppMachine.Panel
{
    public partial class AppUsersPanel : AppUserControlBase
    {
        public static AppUsersPanel This = null;
        private CompBase _allUsers = null;

        public AppUsersPanel()
            : base()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;

            _allUsers = U.GetComponent(AppConstStaticName.ALL_USER);
            componentBrowser.Rebuild(_allUsers);
            componentBrowser.TabPages.ControlAdded += new ControlEventHandler(SetPropertyGridFilter);
            this.Update();
            #endregion


        }


        #region Standard Pattern
        private void cbDelRecipe_DropDown(object sender, EventArgs e)
        {
            cbDelUser.Items.Clear();
            cbDelUser.Items.AddRange(_allUsers.ChildArray);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if(tbNewUser.Text =="")
            {
                AppUtility.ShowKryptonMessageBox("No User Name", "Please input user name", "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                return;
            }

            if(_allUsers.ChildExists(tbNewUser.Text))
            {
                AppUtility.ShowKryptonMessageBox("Duplicate User Name", String.Format("User name \"{0}\" already exist.", tbNewUser.Text), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                return;
            }

            try
            {
                DefaultLogger logger = U.GetComponent(AppConstStaticName.DEFAULT_LOGGER) as DefaultLogger;
                logger.Abort();
                AppUserInfo newUser = new AppUserInfo(tbNewUser.Text);
                newUser.Initialize();
                _allUsers.Add(newUser);
                newUser.InitializeIDReferences();
                logger.Abort();
                componentBrowser.Rebuild(_allUsers);
                AppUtility.ShowKryptonMessageBox("Add New User Completed", String.Format("Add New User \"{0}\" Completed", tbNewUser.Text), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);
                tbNewUser.Clear();
                
            }
            finally
            {

            }
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            if(cbDelUser.SelectedItem != null &&
               _allUsers.ChildExists(cbDelUser.SelectedItem.ToString()))
            {

                DialogResult result = AppUtility.ShowKryptonMessageBox("Remove User", String.Format("Confirm to remove user \"{0}\".", cbDelUser.SelectedItem.ToString()), "",
                                                 ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK | ComponentFactory.Krypton.Toolkit.TaskDialogButtons.Cancel, MessageBoxIcon.Question, this);
                
                if(result != DialogResult.OK)
                {
                    return;
                }

                if (cbDelUser.SelectedItem.ToString() == AppMachine.Comp.AppMachine.This.CurrentUser.UserName ||
                    cbDelUser.SelectedItem.ToString() == AppConstStaticName.ADMIN_USER || cbDelUser.SelectedItem.ToString() == AppConstStaticName.GUEST_USER)
                {
                    AppUtility.ShowKryptonMessageBox("Remove User", String.Format("Unable to remove active user \"{0}\" ", cbDelUser.SelectedItem.ToString()), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);
                    return;
                }


                try
                {
                    _allUsers.Remove(cbDelUser.SelectedItem as CompBase);
                    componentBrowser.Rebuild(_allUsers);
                    AppUtility.ShowKryptonMessageBox("Remove User", String.Format("User \"{0}\" Removed", cbDelUser.SelectedItem.ToString()), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);

                    cbDelUser.Items.Clear();
                    cbDelUser.Text = "";
                }
                finally
                {

                }
            }
        }


        private void SetPropertyGridFilter(object sender, ControlEventArgs e)
        {
        
            foreach (TabPage tp in componentBrowser.TabPages.TabPages)
            {
                if (tp.Text == "Properties")
                {
                    
                    CompBasePropCtl compPropCtrl = tp.Controls[0] as CompBasePropCtl;
                    AttributeCollection attributesCollection = new AttributeCollection(new CategoryAttribute("User Info"));
                    compPropCtrl.BrowsableAttributes = attributesCollection;
                }
            }
        }

        public override void RefreshData()
        {
            componentBrowser.Rebuild(_allUsers);
        }

        #endregion

    }
}
