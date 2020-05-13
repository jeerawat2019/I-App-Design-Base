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

namespace MCore.Comp.MotionSystem.Axis
{
    public partial class RealRotaryPage : UserControl, IComponentBinding<RealRotary>
    {
        private RealRotary _axis = null;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RealRotaryPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RealRotary Bind
        {
            get { return _axis; }
            set
            {
                _axis = value;
                tbCurrentPos.BindTwoWay(() => _axis.CurrentPosition);
                tbDeafultSpeed.BindTwoWay(() => _axis.DefaultSpeed);
                tbMinLimit.BindTwoWay(() => _axis.MinLimit);
                tbMaxLimit.BindTwoWay(() => _axis.MaxLimit);
                cbEnable.BindTwoWay(() => _axis.Enabled);
                rotaryBasePage.Bind = _axis;
            }
        }

        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Real Rotary";
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            _axis.Home();
        }

    }
}
