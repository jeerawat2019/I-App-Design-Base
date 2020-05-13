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
    public partial class LabelCtl : Label
    {
        private PropDelegate<string> _property = null;

        public LabelCtl()
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
        public void Bind(Expression<Func<string>> propertyLambda)
        {
            _property = new PropDelegate<string>(propertyLambda, OnPropertyChanged);

            OnPropertyChanged();
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
