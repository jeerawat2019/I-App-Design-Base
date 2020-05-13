using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public partial class LangUserControl : UserControl
    {
        //[EditorBrowsable(EditorBrowsableState.Always)]
        //[Browsable(true)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //[Bindable(true)]
        //public override string Text
        //{
        //    get { return base.Text; }
        //    set 
        //    { 
        //        base.Text = value;
        //    }
        //}
        public LangUserControl()
        {
            BackColor = SystemColors.Control;
        }
        public void UpdateControlText()
        {
            UpdateControlTextRecurse(GetType(), this);
        }
        private void UpdateControlTextRecurse(Type ty, Control ctl)
        {
            if (ctl.Controls.Count > 0)
            {
                foreach (Control c in ctl.Controls)
                {
                    CompMachine.SetLangString(ty, c);
                    UpdateControlTextRecurse(ty, c);
                }
            }
        }
        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    if (Parent != null)
        //    {
        //        this.Size = Parent.Size;
        //    }
        //    base.OnSizeChanged(e);
        //}
    }
}
