using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using MDouble;
using MCore.Comp.MotionSystem;

namespace MCore.Controls
{
    public class AxisMoveButtonMM : AxisMoveButtonBase
    {
        private PropDelegate<Millimeters> _positionProperty = null;
        /// <summary>
        /// Remove Binding
        /// </summary>
        public override void UnBind()
        {
            _positionProperty.UnBind();
            _positionProperty = null;
        }

        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <param name="propertyLambda"></param>
        public void BindTwoWay(Expression<Func<Millimeters>> propertyLambda)
        {
            _positionProperty = new PropDelegate<Millimeters>(propertyLambda);
            base.Init(_positionProperty.CompTarget as AxisBase);
        }

        /// <summary>
        /// Get the position
        /// </summary>
        /// <returns></returns>
        public override Millimeters Value
        {
            get
            {
                return _positionProperty.Value;
            }
        }
    }
}
