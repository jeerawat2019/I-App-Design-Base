using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore.Controls
{
    public partial class GenericClassPropCtl : UserControl, IComponentBinding<object>
    {

        private object _objClass = null;
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericClassPropCtl()
        {
            InitializeComponent();

        }
        /// <summary>
        /// Bind to component
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Bind
        {
            get { return _objClass; }
            set
            {
                _objClass = value;
                propertyGrid.SelectedObject = _objClass;
            }
        }

        /// <summary>
        /// This will be the name for the tab page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Class Properties";
        }

    }
}
