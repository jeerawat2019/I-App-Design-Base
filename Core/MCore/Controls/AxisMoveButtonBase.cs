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

using MCore;
using MCore.Comp;
using MCore.Comp.MotionSystem;
using MDouble;

namespace MCore.Controls
{
    public abstract partial class AxisMoveButtonBase : CheckBox
    {
        private bool _hideWhenInPosition = false;
        private AxisBase _axis = null;
      
        /// <summary>
        /// If true and when in position, hide it instead of disable
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DisplayName("Hide when in position")]
        [Description("When in position, hide it instead of disable")]
        public bool HideWhenInPosition
        {
            get { return _hideWhenInPosition; }
            set { _hideWhenInPosition = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AxisMoveButtonBase()
        {
            //tbValue.DataBindings.Add("Text", null, "");
            CheckedChanged += new EventHandler(OnCheckedChanged);
        }

        /// <summary>
        /// Remove Binding
        /// </summary>
        public abstract void UnBind();

        /// <summary>
        /// Get the position
        /// </summary>
        /// <returns></returns>
        public abstract Millimeters Value { get; }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Init(AxisBase axis)
        {
            _axis = axis;
            U.RegisterOnChanged(() => _axis.CurrentMotorCounts, OnMotorCountsChanged);
            U.RegisterOnChanged(() => _axis.CanMove, OnChangedCanMove);
            OnChangedCanMove(true);
        }
        
        private void OnCheckedChanged(object sender, EventArgs e)
        {
            if (this.Checked)
            {
                _axis.MoveAbs(Value);
            }
        }

        private void OnChangedCanMove(bool canMove)
        {
            // Check if is in position
            if (Value == _axis.CurrentPosition)
            {
                this.Checked = false;
                if (HideWhenInPosition)
                {
                    this.Hide();
                }
                else
                {
                    this.Enabled = false;
                }
            }
            else
            {                
                this.Show();
                this.Enabled = _axis.CanMove;
            } 
        }

        private void OnMotorCountsChanged(double mc)
        {
            OnChangedCanMove(true);
        }
    }

}
