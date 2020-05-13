using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MDouble;

using MCore.Comp;
using MCore.Controls;

namespace MCore.Comp.IOSystem
{
    public partial class CompMeasureCtl : UserControl, IComponentBinding<CompMeasure>
    {
        private CompMeasure _compMeasure = null;

        public string Label
        {
            get
            {
                return gbTriggerMode.Text;
            }
            set
            {
                gbTriggerMode.Text = value;
            }
        }

        public CompMeasureCtl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// The title of this control
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Trigger Mode";
        }
        /// <summary>
        /// Bind to the CognexCamera7_1 component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CompMeasure Bind
        {
            get { return _compMeasure; }
            set
            {
                _compMeasure = value;
                gbTriggerMode.BindTwoWay(() => _compMeasure.TriggerMode);
                mdInterval.BindTwoWay(() => _compMeasure.TriggerInterval);
            }
        }
    }
}
