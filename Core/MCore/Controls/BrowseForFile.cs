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
    public partial class BrowseForFile : UserControl
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


        public OpenFileDialog OpenDialog
        {
            get { return this.openFileDlg; }
        }


        public BrowseForFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <param name="propertyLambda"></param>
        public void BindTwoWay(Expression<Func<string>> propertyLambda)
        {
            _property = new StringPropDelegate(propertyLambda, OnChanged);

            OnChanged();
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

        private void OnChanged()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(OnChanged));
                return;
            }
            try
            {
                filePath.Text = _property.SValue;
                filePath.Select(filePath.Text.Length, 0); 

            }
            catch
            {
                U.LogPopup("Unexpected text from Property({0}) = {1}", _property.Name, _property.SValue);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                _property.SValue = openFileDlg.FileName;
            }
        }
    }
    /// <summary>
    /// Class to handle auto binding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringPropDelegate : PropDelegate<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="delOnChanged"></param>
        public StringPropDelegate(Expression<Func<string>> propertyLambda, MethodInvoker delOnChanged)
            : base(propertyLambda, delOnChanged)
        {
        }

        /// <summary>
        /// Get or set the string value
        /// </summary>
        public override string SValue
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
                //_compTarget.Fire SetPropValue(_propertyLambda);
            }
        }
    }
}
