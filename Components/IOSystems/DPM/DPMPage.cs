using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
using MDouble;

namespace MCore.Comp.IOSystem
{
    public partial class DPMPage : UserControl, IComponentBinding<DPMCtrl>
    {
        private DPMCtrl _controller = null;
        public DPMPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the MEDAQLib component
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DPMCtrl Bind
        {
            get { return _controller; }
            set
            {
                _controller = value;
                if (_controller.milligramInput != null)
                {
                    mdCurrentValue.BindTwoWay(() => _controller.milligramInput.Value);
                    triggerMode.Bind = _controller.milligramInput;
                }
            }
        }

        public override string ToString()
        {
            return "Settings";
        }
    }
}
