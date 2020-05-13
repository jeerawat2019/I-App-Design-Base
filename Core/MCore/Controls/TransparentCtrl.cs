using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCore.Controls
{
    public partial class TransparentCtrl : Form
    {
        public string Label
        {
            get { return lblLockOutText.Text; }
            set { lblLockOutText.Text = value; }
        }
        public TransparentCtrl()
        {
            InitializeComponent();
        }
    }
}
