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
    public partial class MDoubleCtl : UserControl
    {
        private PropDelBase _property = null;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public MDoubleCtl()
        {
            InitializeComponent();
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
            if (!(typeof(T).IsSubclassOf(typeof(MDouble.MDoubleBase))))
            {
                throw new Exception("");
            }
            _property = new MDoublePropDelegate<T>(propertyLambda, OnChanged);

            if (string.IsNullOrEmpty(lblName.Text))
            {
                lblName.Text = _property.Name;
            }

            OnChanged();
        }

        private void OnValidated(object sender, EventArgs e)
        {
            if (_property != null)
            {
                string oldVal = _property.SValue;
                _property.SValue = tbValue.Text;
                if (LogChanges)
                {
                    U.LogChange(_property, oldVal);
                }
            }

        }
        private void OnChanged()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnChanged));
                return;
            }
            try
            {
                tbValue.Text =  _property.SValue;
            }
            catch 
            {
                U.LogPopup("Unexpected text from MDouble({0}) = {1}", _property.Name, _property.SValue);
            }
        }
    }

    /// <summary>
    /// Class to handle auto binding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MDoublePropDelegate<T> : PropDelegate<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="delOnChanged"></param>
        public MDoublePropDelegate(Expression<Func<T>> propertyLambda, MethodInvoker delOnChanged)
            : base(propertyLambda, delOnChanged)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyLambda"></param>
        public MDoublePropDelegate(Expression<Func<T>> propertyLambda)
            : base(propertyLambda)
        {
        }

        /// <summary>
        /// Get or set the string value
        /// </summary>
        public override string SValue
        {
            get
            {
                return _propertyGetter().ToString();
            }
            set
            {
                MDoubleBase mDouble = _propertyGetter() as MDoubleBase;
                mDouble.Val = U.SafeToDouble(value);
                if (CompTarget != null)
                {
                    CompTarget.NotifyPropertyChanged(_propertyLambda);
                }
            }
        }
    }
}
