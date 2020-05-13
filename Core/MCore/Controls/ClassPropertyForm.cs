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
    public partial class ClassPropertyForm : Form
    {
        public ClassPropertyForm(object obj, string name)
        {
            InitializeComponent();
            Text = name;
            genericClassPropCtl.Bind = obj;
        }

    }
}
