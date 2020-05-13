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
    public partial class BoolComboBoxCtl : ComboBox
    {
        private PropDelegate<bool> _property = null;

        private bool _logChanges = true;
        /// <summary>
        /// Flag to indicate if we are to add a log entry if user changes the value
        /// </summary>
        public bool LogChanges
        {
            get { return _logChanges; }
            set { _logChanges = value; }
        }
        public BoolComboBoxCtl()
        {
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
        public void BindTwoWay(Expression<Func<bool>> propertyLambda)
        {
            _property = new PropDelegate<bool>(propertyLambda, OnPropertyChanged);

            if (Items.Count == 0)
            {
                Items.Add("False");
                Items.Add("True");
            }

            OnPropertyChanged();
            this.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
        }

        void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int iPropVal = _property.Value ? 1 : 0;
            string oldVal = _property.SValue;
            if (SelectedIndex != iPropVal)
            {
                _property.Value = SelectedIndex == 1;
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
            int iPropVal = _property.Value ? 1 : 0;
            if (SelectedIndex != iPropVal)
            {
                SelectedIndex = iPropVal;
            }
        }
    }
}
