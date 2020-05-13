using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDouble;
using System.Linq.Expressions;
using MCore.Comp.MotionSystem;

namespace MCore.Controls
{
    public class AxisTeachButton : AxisTeachButtonBase
    {
        private PropDelegate<MLength> _positionProperty = null;
        /// <summary>
        /// Remove Binding
        /// </summary>
        public override void UnBind()
        {
            _positionProperty.UnBind();
        }

        public override MLength Value 
        { 
            get
            {
                return _positionProperty.Value;
            }
            set
            {
                _positionProperty.Value = new Millimeters(value);
            }
        }
        
        /// <summary>
        /// Provide two-way binding
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda"></param>
        public void BindTwoWay(Expression<Func<MLength>> propertyLambda)
        {
            _positionProperty = new PropDelegate<MLength>(propertyLambda);
            Init(_positionProperty.CompTarget as AxisBase);
        }
    }
}
