using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using MCore.Comp;

namespace MCore
{
    public static class D
    {
        #region delegate definitions
        /// <summary>
        /// A generic delegate for method that return void and received any parameters
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="args"></param>
        public delegate void GenericDelegateWithArgs(object instance, object[] args);

        /// <summary>
        /// Delegate for Property Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DelPropertyChanged(object sender, PropertyChangedEventArgs e);

        /// <summary>
        /// Delegate for bool arg
        /// </summary>
        /// <param name="bVal"></param>
        public delegate void delVoid_Bool(bool bVal);

        /// <summary>
        /// Delegate for bool-long arg
        /// </summary>
        /// <param name="bVal"></param>
        /// <param name="lVal"></param>
        public delegate void delVoid_BoolLong(bool bVal, long lVal);

        /// <summary>
        /// Delegate for double arg
        /// </summary>
        /// <param name="dVal"></param>
        public delegate void delVoid_Double(double dVal);

        /// <summary>
        /// Delegate for 2 double args
        /// </summary>
        /// <param name="dVal1"></param>
        /// <param name="dVal2"></param>
        public delegate void delVoid_DoubleDouble(double dVal1, double dVal2);

        /// <summary>
        /// Delegate for int arg
        /// </summary>
        /// <param name="iVal"></param>
        public delegate void delVoid_Int(int iVal);

        /// <summary>
        /// Delegate for int arg
        /// </summary>
        /// <param name="sVal"></param>
        public delegate void delVoid_String(string sVal);

        /// <summary>
        /// Delegate for int arg
        /// </summary>
        /// <param name="iVal"></param>
        /// <returns></returns>
        public delegate bool delBool_Int(int iVal);


        /// <summary>
        /// Delegate for object arg
        /// </summary>
        /// <param name="obj"></param>
        public delegate void ObjectEventHandler(object obj);

        /// <summary>
        /// Delegate for Image arg
        /// </summary>
        /// <param name="obj"></param>
        public delegate void ImageEventHandler(Image obj);

        /// <summary>
        /// Delegate for control arg
        /// </summary>
        /// <param name="control"></param>
        public delegate void ControlEventHandler(Control control);

        /// <summary>
        /// Delegate for Bitmap arg
        /// </summary>
        /// <param name="bitmap"></param>
        public delegate void BitmapEventHandler(Bitmap bitmap);

        #endregion delegate definitions

        public static DT<object>.PropertyGetterDelegate CreatePropertyGetterDelegate(PropertyInfo propInfo)
        {
            return DT<object>.CreatePropertyGetterDelegate(propInfo);
        }

        ///// <summary>
        /////  Create a dynamic delegate of type PropertyGetterDelegate. NOTE: call to this only once for each method, save the cached to get the benefit of expression. It is no use to create the delegate all time because it take time to compile
        ///// </summary>
        ///// <param name="prop"></param>
        ///// <returns>PropertyGetterDelegate</returns>
        //public static PropertyGetterDelegate CreatePropertyGetterDelegate(PropertyInfo prop)
        //{
        //    var instance = Expression.Parameter(typeof(object), "instance");
        //    var instanceCast = Expression.Convert(instance, prop.DeclaringType);

        //    var call = Expression.Call(instanceCast, prop.GetGetMethod());
        //    var callCast = Expression.TypeAs(call, typeof(object));

        //    var lambda = Expression.Lambda<PropertyGetterDelegate>(callCast, new ParameterExpression[] { instance });

        //    return lambda.Compile();
        //}

        /// <summary>
        /// Create a dynamic delegate of type GenericActionDelegate. NOTE: call to this only once for each method, save the cached to get the benefit of expression. It is no use to create the delegate all time because it take time to compile
        /// </summary>
        /// <param name="method"></param>
        /// <returns>A GenericActionDelegate (void function call with args)</returns>
        public static GenericDelegateWithArgs
            CreateGenericDelegateWithArgs(MethodInfo method)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var instanceCast = Expression.Convert(instance, method.DeclaringType);

            var args = Expression.Parameter(typeof(object[]), "args");
            var argsCast = CreateParameterExpressions(method, args);

            var lambda = Expression.Lambda<GenericDelegateWithArgs>(Expression.Call(instanceCast, method, argsCast), new ParameterExpression[] { instance, args });

            return lambda.Compile();
        }
        private static Expression[] CreateParameterExpressions(MethodInfo method, Expression args)
        {
            return method.GetParameters().Select((parameter, index) =>
              Expression.Convert(
                Expression.ArrayIndex(args, Expression.Constant(index)), parameter.ParameterType)).ToArray();
        }

    }
    /// <summary>
    /// Abstract class to handle auto binding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PropDelBase
    {
        public abstract string SValue { get; set; }
        public abstract object OValue { set; }
        public abstract Type PropType{ get; }
        protected PropertyInfo _propInfo = null;
        protected Object _target = null;
        /// <summary>
        /// Get the component target
        /// </summary>
        public CompBase CompTarget
        {
            get { return _target as CompBase; }
        }
        /// <summary>
        /// Get the name to use for this property
        /// </summary>
        public string Name
        {
            get 
            {
                DisplayNameAttribute[] displayAttr = _propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true) as DisplayNameAttribute[];
                if (displayAttr.Length > 0)
                {
                    return displayAttr[0].DisplayName;
                }
                return _propInfo.Name; 
            }
        }
        public abstract void UnBind();
    }
    /// <summary>
    /// Class to handle auto binding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropDelegate<T> : PropDelBase
    {
        private bool _canWrite = false;
        /// <summary>
        /// Flag to indicate if we have a Set ability
        /// </summary>
        public bool CanWrite
        {
            get { return _canWrite; }
        }
        /// <summary>
        ///  Set the value
        /// </summary>
        public T Value
        {
            get
            {
                return _propertyGetter();
            }
            set
            {
                if (_propertySetter != null)
                {
                    _propertySetter(value);
                }
            }
        }

        protected Func<T> _propertyGetter = null;
        private Action<T> _propertySetter = null;

        private MethodInvoker _delOnChanged = null;
        protected Expression<Func<T>> _propertyLambda;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyLambda"></param>
        public PropDelegate(Expression<Func<T>> propertyLambda) : this(propertyLambda, null)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyLambda"></param>
        /// <param name="delOnChanged"></param>
        public PropDelegate(Expression<Func<T>> propertyLambda, MethodInvoker delOnChanged)
        {
            _propertyLambda = propertyLambda;
            _delOnChanged = delOnChanged;
            try
            {
                MemberExpression member = propertyLambda.Body as MemberExpression;
                _propInfo = member.Member as PropertyInfo;
                // Get the getter delegate
                _propertyGetter = propertyLambda.Compile();
                // Get the target object
                Func<Object> delObj = Expression.Lambda<Func<Object>>(member.Expression).Compile();
                _target = delObj();
                _propInfo = _target.GetType().GetProperty(_propInfo.Name);

                if (delOnChanged != null && CompTarget != null)
                {
                    U.RegisterOnChanged(propertyLambda, OnChanged);
                }
                if (_propInfo.CanWrite)
                {
                    // Create the setter delegate
                    _propertySetter = Delegate.CreateDelegate(typeof(Action<T>), _target, "set_" + _propInfo.Name, true) as Action<T>;
                    _canWrite = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to build get/set delegates for {0}.  Reason='{1}'", _propInfo.Name, ex.Message));
            }
        }
        public override void UnBind()
        {
            if (_delOnChanged != null && CompTarget != null)
            {
                U.UnRegisterOnChanged(_propertyLambda, OnChanged);
            }
        }

        public override Type PropType 
        {
            get { return typeof(T); }
        }
        public override string SValue
        {
            get
            {
                return _propertyGetter().ToString();
            }
            set { }
        }
        public override object OValue
        {
            set
            {
                if (_propertySetter != null)
                {
                    _propertySetter((T)value);
                }
            }
        }
        private void OnChanged(T propObj)
        {
            _delOnChanged();
        }

    }
    public class DT<T>
    {
        /// <summary>
        /// Delegate for expression based property getter
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public delegate T PropertyGetterDelegate(object instance);

        /// <summary>
        ///  Create a dynamic delegate of type PropertyGetterDelegate. NOTE: call to this only once for each method, save the cached to get the benefit of expression. It is no use to create the delegate all time because it take time to compile
        /// </summary>
        /// <param name="prop"></param>
        /// <returns>PropertyGetterDelegate</returns>
        public static PropertyGetterDelegate CreatePropertyGetterDelegate(PropertyInfo prop)
        {
            try
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var instanceCast = Expression.Convert(instance, prop.DeclaringType);
                var call = Expression.Call(instanceCast, prop.GetGetMethod());
                var callCast = Expression.Convert(call, typeof(T));
                var lambda = Expression.Lambda<PropertyGetterDelegate>(callCast, new ParameterExpression[] { instance });
                return lambda.Compile();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }

    }
}
