using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AppMachine.Comp.Users;
using ComponentFactory.Krypton.Toolkit;

using MCore;
using MCore.Comp;


namespace AppMachine.Comp.Users
{
    public partial class AppChangeUserFrm : Form
    {
        private CompBase _allUser = null;
        public AppChangeUserFrm()
        {
            InitializeComponent();
            _allUser = U.GetComponent(AppConstStaticName.ALL_USER);
        }

        private void strUserInputCode_KeyUp(object sender, KeyEventArgs e)
        {
            
            if(e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrEmpty(strUserInputCode.Text))
                {
                    return;
                }

               bool success = VerifyUser();
                if(success)
                {
                    this.Hide();
                    string message = String.Format("User Change to User: {0} En: {1} Level: {2}",AppMachine.This.CurrentUser.UserName,AppMachine.This.CurrentUser.UserEN,AppMachine.This.CurrentUser.UserLevel);
                    U.LogInfo(message);
                    AppUtility.ShowKryptonMessageBox("User Change Completed", "User Change Completed", message, TaskDialogButtons.OK, MessageBoxIcon.Information, this);
                }
                else
                {
                    string message = String.Format("Could not find user code: {0} ", strUserInputCode.Text);
                    this.Hide();
                    AppUtility.ShowKryptonMessageBox("User Change Fail", "User Change Fail", message, TaskDialogButtons.OK, MessageBoxIcon.Error, this);

                    strUserInputCode.Clear();
                    this.Show();
                }
            }
        }

        private bool VerifyUser()
        {
            foreach(AppUserInfo user in _allUser.ChildArray)
            {
                if(user.UserCode == strUserInputCode.Text)
                {
                    AppMachine.This.CurrentUser = user;
                    return true;
                }
            }
            return false;
        }

        private void AppChangeUserFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void PerformChangeUser()
        {
            this.ShowDialog();
            strUserInputCode.Clear();
            strUserInputCode.Select();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
