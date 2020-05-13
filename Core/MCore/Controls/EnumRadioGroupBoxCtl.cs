using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace MCore.Controls
{
    public partial class EnumRadioGroupBoxCtl : GroupBox
    {
        private PropDelBase _property = null;

        public EnumRadioGroupBoxCtl()
        {
        }

        private bool _logChanges = true;
        /// <summary>
        /// Flag to indicate if we are to add a log entry if user changes the value
        /// </summary>
        public bool LogChanges
        {
            get { return _logChanges; }
            set { _logChanges = value; }
        }
        /// <summary>
        /// Remove Binding
        /// </summary>
        public void UnBind()
        {
            if (_property != null)
            {
                _property.UnBind();
                _property = null;
            }
        }
        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda"></param>
        public void BindTwoWay<T>(Expression<Func<T>> propertyLambda)
        {
            _property = new PropDelegate<T>(propertyLambda, OnPropertyChanged);

            string[] enumNames = Enum.GetNames(typeof(T));
            string origVal = _property.SValue;

            foreach (Control ctl in Controls)
            {
                if (ctl is RadioButton && ctl.TabIndex < enumNames.Length)
                {
                    RadioButton rb = ctl as RadioButton;
                    if (string.IsNullOrEmpty(rb.Text))
                    {
                        rb.Text = enumNames[ctl.TabIndex].Replace('_', ' ');
                    }
                    rb.Tag = enumNames[ctl.TabIndex];
                    //rb.Checked = origVal == rb.Tag.ToString();
                    rb.CheckedChanged += new EventHandler(OnCheckedChanged);
                }
            }

            OnPropertyChanged();
        }

        void OnCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            string oldVal = _property.SValue;
            string checkedVal = rb.Tag.ToString();
            if (rb.Checked && checkedVal != oldVal)
            {
                _property.OValue = Enum.Parse(_property.PropType, checkedVal);
                if (LogChanges)
                {
                    U.LogChange(_property, oldVal);
                }
            }
        }
        private void OnPropertyChanged()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnPropertyChanged));
                return;

            }
            string newVal = _property.SValue;
            foreach (Control rb in Controls)
            {
                if (rb is RadioButton && rb.Tag != null)
                {
                    string rbVal = rb.Tag.ToString();
                    if (newVal == rbVal && !(rb as RadioButton).Checked)
                    {
                        (rb as RadioButton).Checked = true;
                        return;
                    }
                }
            }
        }
    }
}
