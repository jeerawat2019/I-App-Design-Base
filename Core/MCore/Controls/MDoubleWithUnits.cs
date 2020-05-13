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
    public partial class MDoubleWithUnits : UserControl
    {
        private PropDelBase _propertyForRead = null;
        private PropDelBase _propertyForWrite = null;
        public event MethodInvoker OnValueChanged;
        public event MethodInvoker OnApplyValue;

        private bool _logChanges = true;
        /// <summary>
        /// Flag to indicate if we are to add a log entry if user changes the value
        /// </summary>
        public bool LogChanges
        {
            get { return _logChanges; }
            set { _logChanges = value; }
        }
        public string Label
        {
            get
            {
                return lblName.Text;
            }
            set
            {
                lblName.Text = value;
            }
        }

        public string UnitsLabel
        {
            get
            {
                return lblUnits.Text;
            }
            set
            {
                lblUnits.Text = value;
            }
        }

        public double DoubleVal
        {
            get
            {
                try
                {
                    return Convert.ToDouble(tbValue.Text);
                }
                catch
                {
                    return 0.0;
                }
            }
            set
            {
                tbValue.Text = value.ToString("#.###");
            }
        }

        /// <summary>
        /// Get/set the Text Box back ground color
        /// </summary>
        public Color TextBackColor
        {
            get
            {
                return tbValue.BackColor;
            }
            set
            {
                tbValue.BackColor = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MDoubleWithUnits()
        {
            InitializeComponent();
            Enabled = false;
            if (this.DesignMode)
            {
                Label = this.Name;
            }
            //tbValue.DataBindings.Add("Text", null, "");
        }
        /// <summary>
        /// Remove Binding
        /// </summary>
        public void UnBind()
        {
            if (_propertyForRead != null)
            {
                _propertyForRead.UnBind();
                _propertyForRead = null;
            }
            if (_propertyForWrite != null)
            {
                _propertyForWrite.UnBind();
                _propertyForWrite = null;
            }
            tbValue.Text = string.Empty;
            OnValueChanged = null;
            OnApplyValue = null;
            Enabled = false;
        }
        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambdaForReadWrite"></param>
        public void BindTwoWay<T>(Expression<Func<T>> propertyLambdaForReadWrite)
        {
            this.BindTwoWay(propertyLambdaForReadWrite, null);
        }
        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambdaForRead"></param>
        /// <param name="propertyLambdaForWrite"></param>
        public void BindTwoWay<T>(Expression<Func<T>> propertyLambdaForRead, Expression<Func<T>> propertyLambdaForWrite)
        {
            if (!typeof(T).Equals(typeof(MDouble.MDoubleBase)) && !(typeof(T).IsSubclassOf(typeof(MDouble.MDoubleBase))))
            {
                throw new Exception("Must be of type MDoubleBase");
            }
            _propertyForRead = new MDoublePropDelegate<T>(propertyLambdaForRead, OnChanged);
            if (propertyLambdaForWrite != null)
            {
                _propertyForWrite = new MDoublePropDelegate<T>(propertyLambdaForWrite);
            }

            if (string.IsNullOrEmpty(lblName.Text))
            {
                lblName.Text = _propertyForRead.Name;
            }
            tbValue.Multiline = false;
            OnChanged();
            Enabled = true;
        }
        /// <summary>
        /// Update the text
        /// </summary>
        public void UpdateText()
        {
            OnChanged();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            lblName.Location = new Point(3,3);
            base.OnSizeChanged(e);
        }

        private void OnValidated(object sender, EventArgs e)
        {
            string changedText = tbValue.Text;
            string strOrigValue = _propertyForRead != null ? _propertyForRead.SValue : string.Empty;
            if (_propertyForWrite != null)
            {
                _propertyForWrite.SValue = changedText;
                OnChanged();
            }
            else if (_propertyForRead != null)
            {
                try
                {
                    double dVal = Convert.ToDouble(changedText);
                    _propertyForRead.OValue = Activator.CreateInstance(_propertyForRead.PropType, dVal);
                }
                catch { }
            }
            else
            {
                // Both are null.  Probably UnBound
                return;
            }
            if (strOrigValue != _propertyForRead.SValue)
            {
                if (OnValueChanged != null)
                {
                    OnValueChanged();
                }
                if (LogChanges)
                {
                    if (_propertyForRead.CompTarget != null)
                    {
                        U.LogChange(string.Format("{0}.{1}", _propertyForRead.CompTarget.Nickname, lblName.Text), strOrigValue, _propertyForRead.SValue);
                    }
                    else
                    {
                        U.LogChange(string.Format("{0}", lblName.Text), strOrigValue, _propertyForRead.SValue);
                    }
                }
            }
        }

        private void OnChanged()
        {
            if (_propertyForRead == null)
                return;
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnChanged));
                return;
            }
            string strValue = _propertyForRead.SValue;
            string[] split = strValue.Split(' ');
            if (split.Length < 1)
            {
                throw new MCoreException(LogSeverity.Popup, "Unexpected text from MDouble({0}) ={1}", _propertyForRead.Name, strValue);
            }
            tbValue.Text = split[0];
            if (split.Length > 1)
            {
                lblUnits.Text = split[1];
            }
            else
            {
                lblUnits.Text = "";
            }
        }

        private void OnValidating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbValue.Text))
            {
                try
                {
                    double dVal = Convert.ToDouble(tbValue.Text);
                }
                catch
                {
                    e.Cancel = true;
                    U.LogPopup("Please enter a valid value for '{0}'", lblName.Text);
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnValidated(null, null);
                e.Handled = e.SuppressKeyPress = true;
                if (OnApplyValue != null)
                {
                    OnApplyValue();
                }
            }
        }
    }

}
