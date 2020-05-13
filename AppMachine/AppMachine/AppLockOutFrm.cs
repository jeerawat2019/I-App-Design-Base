using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AppMachine
{
    /// <summary>
    /// Class for locking out the controls to provide safety for the operator
    /// </summary>
    public partial class AppLockOutFrm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AppLockOutFrm()
        {
            InitializeComponent();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}