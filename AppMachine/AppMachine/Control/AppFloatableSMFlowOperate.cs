using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;

using MCore;
using MCore.Comp;
using MCore.Comp.SMLib.SMFlowChart;
using MCore.Controls;

namespace AppMachine.Control
{
    public partial class AppFloatableSMOperate : SMFlowChartOperateBasic
    {

        #region Standard Pattern
        private System.Windows.Forms.Control _dockParentControl;
        private Size _dockSize;
        private Form _floatingForm;

        public AppFloatableSMOperate()
        {
            InitializeComponent();

            _floatingForm = new Form();
            _floatingForm.ShowInTaskbar = false;
            _floatingForm.TopMost = true;
            _floatingForm.MaximizeBox = false;
            _floatingForm.MinimizeBox = false;
            _floatingForm.FormBorderStyle = FormBorderStyle.FixedSingle;

            _floatingForm.FormClosing += new FormClosingEventHandler(_floatingForm_FormClosing);
            _floatingForm.Hide();
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected override void DestroyHandle()
        {
            if (_floatingForm != null)
            {
                _floatingForm.FormClosing -= new FormClosingEventHandler(_floatingForm_FormClosing);
                _floatingForm.Dispose();
            }

 	      base.DestroyHandle();
        }

        private void _floatingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DoDocking();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }


        private void linkLblFloat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLblFloat.Text == "Float")
            {
                DoFloating();
            }
            else
            {
                DoDocking();
            }
        }

      
        private void DoDocking()
        {
            if (this.Parent != _floatingForm)
            {
                return;
            }
     
            _floatingForm.Controls.Remove(this);
            this.Size = _dockSize;
            _dockParentControl.Controls.Add(this);
            _floatingForm.Hide();
            linkLblFloat.Text = "Float";
        }

        /// <summary>
        /// Float itself
        /// </summary>
        private void DoFloating()
        {
         
            if (this.Parent == _floatingForm)
            {
                return;
            }

            _dockParentControl = this.Parent;
            _dockSize = this.Size;
            this.Parent.Controls.Remove(this);
            _floatingForm.ClientSize = this.Size;
            _floatingForm.Controls.Add(this);
            this.Location = new Point(0, 0);
            _floatingForm.Show();
            linkLblFloat.Text = "Dock";
        }
        #endregion

        //Add More Application Requirement in Below

        public void PluginControlParameter(System.Windows.Forms.Control plugInCtrl,string ParameterName)
        {
            plugInCtrl.Size = new Size(129, 21);
            plugInCtrl.Location = new Point(549, 1);
            this.Controls.Add(plugInCtrl);
            lblParamName.Text = ParameterName;
        }

        //public void BindingParameter<T>(Expression<Func<T>> paramProperty)
        //{
        //    CompBase comp = U.GetComponentFromPropExpression(paramProperty);
        //    object obj = comp.GetPropValue(paramProperty);
        //    Type t = obj.GetType();
        //    if(t.IsEnum)
        //    {
        //        EnumComboBoxCtl enumCbParam = new EnumComboBoxCtl();
        //        enumCbParam.Size = new Size(129, 21);
        //        enumCbParam.Location = new Point(549, 1);
        //        enumCbParam.BindTwoWay(paramProperty);
        //        this.Controls.Add(enumCbParam);
        //    }
        //    else if(t.IsSubclassOf(typeof(MDouble.MDoubleBase)))
        //    {
        //        MDoubleCtl mDoubleParam = new MDoubleCtl();
        //        mDoubleParam.Size = new Size(129, 21);
        //        mDoubleParam.Location = new Point(549, 1);
        //        mDoubleParam.BindTwoWay(paramProperty);
        //        this.Controls.Add(mDoubleParam);
        //    }
        //    else if (t.Is(typeof(Int32)))
        //    {
        //        IntegerCtl mIntParam = new IntegerCtl();
        //        mIntParam.Size = new Size(129, 21);
        //        mIntParam.Location = new Point(549, 1);
               
        //        //mIntParam.BindTwoWay(paramProperty);
        //        this.Controls.Add(mIntParam);
        //    }

        //}
    }


  
}
