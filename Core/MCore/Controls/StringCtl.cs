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
    public partial class StringCtl : TextBox
    {
        private PropDelegate<string> _property = null;

        private bool _logChanges = true;
        /// <summary>
        /// Flag to indicate if we are to add a log entry if user changes the value
        /// </summary>
        public bool LogChanges
        {
            get { return _logChanges; }
            set { _logChanges = value; }
        }

        public StringCtl()
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
        public void BindTwoWay(Expression<Func<string>> propertyLambda)
        {
            _property = new PropDelegate<string>(propertyLambda, OnPropertyChanged);

            OnPropertyChanged();
            this.TextChanged += new EventHandler(StringCtl_TextChanged);
        }
        void StringCtl_TextChanged(object sender, EventArgs e)
        {
            string oldVal = _property.Value.ToString();
            if (this.Text != oldVal)
            {
                try
                {
                    _property.Value = this.Text;
                    if (LogChanges)
                    {
                        U.LogChange(_property, oldVal);
                    }
                }
                catch { }
            }
            
        }
        private void OnPropertyChanged()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnPropertyChanged));
                return;

            }
            if (this.Text != _property.Value)
            {
                this.Text = _property.Value;
            }
        }
    }
}
