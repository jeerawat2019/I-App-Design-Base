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
    public partial class EnumComboBoxCtl : ComboBox
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
        public EnumComboBoxCtl()
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
        public void BindTwoWay<T>(Expression<Func<T>> propertyLambda)
        {
            _property = new PropDelegate<T>(propertyLambda, OnPropertyChanged);

            Items.AddRange(Enum.GetNames(typeof(T)));

            OnPropertyChanged();
            this.SelectedIndexChanged += new EventHandler(EnumCtl_SelectedIndexChanged);
        }

        void EnumCtl_SelectedIndexChanged(object sender, EventArgs e)
        {

            string oldVal = _property.SValue;
            if (Text != oldVal)
            {
                _property.OValue = Enum.Parse(_property.PropType, Text);
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
            if (SelectedIndex == -1 || Text != _property.SValue)
            {
                int iSel = FindStringExact(_property.SValue);
                SelectedIndex = iSel;                                
            }
        }
    }
}
