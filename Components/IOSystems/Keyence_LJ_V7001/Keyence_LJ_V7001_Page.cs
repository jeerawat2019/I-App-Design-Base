using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;
using MCore.Controls;
using MDouble;

namespace MCore.Comp.IOSystem
{
    public partial class Keyence_LJ_V7001_Page : UserControl, IComponentBinding<Keyence_LJ_V7001>
    {

        private Keyence_LJ_V7001 _controller = null;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Keyence_LJ_V7001_Page()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Public Properties
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Keyence_LJ_V7001 Bind
        {
            get { return _controller; }
            set
            {
                _controller = value;
                //mdDisplacement.BindTwoWay(() => _controller.MMDRGInput.Value);
            }

        }


        /// <summary>
        /// Override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Settings";
        }





    }
}
