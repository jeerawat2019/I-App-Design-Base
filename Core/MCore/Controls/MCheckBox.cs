using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;
using MDouble;

namespace MCore.Controls
{
    public partial class MCheckBox : CheckBox
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
        /// <summary>
        /// Constructor
        /// </summary>
        public MCheckBox()
        {
            //tbValue.DataBindings.Add("Text", null, "");
            CheckedChanged += new EventHandler(OnCheckedChanged);
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

            OnPropertyChanged();
        }
        
        void OnCheckedChanged(object sender, EventArgs e)
        {
            if (_property != null && this.Checked != _property.Value)
            {
                string oldVal = _property.SValue;
                _property.Value = this.Checked;
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
            if (_property != null && this.Checked != _property.Value)
            {
                this.Checked = _property.Value;
            }
        }
    }

}
