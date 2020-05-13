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
    public partial class MBoolRadio : UserControl
    {
        private PropDelegate<bool> _property = null;
        private string _trueText = string.Empty;
        private string _falseText = string.Empty;
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
        /// Text for the True radio button
        /// </summary>
        public string TrueText
        {
            get { return _trueText; }
            set 
            { 
                _trueText = value;
                rbTrue.Text = _trueText;
            }
        }
        /// Text for the False radio button
        /// </summary>
        public string FalseText
        {
            get { return _falseText; }
            set 
            { 
                _falseText = value;
                rbFalse.Text = _falseText;
            }
        }
        public MBoolRadio()
        {
            InitializeComponent();
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
        private void OnPropertyChanged()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnPropertyChanged));
                return;

            }
            if (_property != null && rbTrue.Checked != _property.Value)
            {
                rbTrue.Checked = _property.Value;
            }
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            if (_property != null && rbTrue.Checked != _property.Value)
            {
                string oldVal = _property.SValue;
                _property.Value = rbTrue.Checked;
                if (LogChanges)
                {
                    U.LogChange(_property, oldVal);
                }
            }
        }
    }
}
