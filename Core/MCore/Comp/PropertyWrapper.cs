using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MCore.Comp
{
    public class PropertyWrapper
    {
        private string _id = string.Empty;
        /// <summary>
        /// The ID which is serializes
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// The owner object of the property
        /// </summary>
        [XmlIgnore]
        public Object Target
        {
            get { return _target; }
        }
        /// <summary>
        /// The Property type
        /// </summary>
        [XmlIgnore]
        public Type PropertyType
        {
            get { return _type; }
            set { _type = value; }
        }
        private object _target = null;
        /// <summary>
        /// The Property Info
        /// </summary>
        private PropertyInfo _pi = null;
        private Type _type = null;
        private DT<object>.PropertyGetterDelegate _getter = null;
        private MethodInvoker _delOnChanged = null;
        private object _literalValue = null;
        [XmlIgnore]
        public bool IsLiteral
        {
            get { return _literalValue != null; }
        }
        /// <summary>
        /// Value to use in case Literal value is supplied
        /// </summary>
        [XmlIgnore]
        public object LiteralValue
        {
            get { return _literalValue; }
            set 
            {
                if (!_literalValue.Equals(value))
                {
                    _literalValue = value;
                    if (_delOnChanged != null)
                    {
                        _delOnChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        public PropertyWrapper()
        {
        }
        /// <summary>
        /// Dynamic creation constructor
        /// </summary>
        /// <param name="id"></param>
        public PropertyWrapper(string id)
        {
            _id = id;
            _type = null;
        }
        /// <summary>
        /// Dynamic creation constructor
        /// </summary>
        /// <param name="id"></param>
        public PropertyWrapper(string id, Type expectedType)
        {
            _id = id;
            _type = expectedType;
        }

        /// <summary>
        /// Based on the ID, generate the getter and everything else 
        /// </summary>
        public void Initialize()
        {
            string propPath = string.Empty;
            string propertyName = U.SplitPropertyID(ID, out propPath);
            _getter = null;
            if (U.ComponentExists(propPath))
            {
                _target = U.GetComponent(propPath);
                _pi = U.GetPropertyInfo(_target, propertyName, _type);
                if (_pi != null)
                {
                    _type = _pi.PropertyType;
                    _getter = D.CreatePropertyGetterDelegate(_pi);
                    if (_getter == null)
                    {
                        throw new Exception(string.Format("Could not create 'Getter' '{0}' from string {1}.", _pi.Name, ID));
                    }
                }
                else
                {
                    throw new Exception(string.Format("Could not find Property '{0}'", ID));
                }
            }
            else
            {
                // Literal.  We expect the Type to be specified
                // "(Int32)102"
                // "(string) My string"
                // (double) 1.23435
                string text = ID.TrimStart('(');
                string[] split = text.Split(')');
                if (split.Length == 2)
                {
                    
                    _type = Type.GetType("System." + split[0]);
                    if (_type == null)
                    {
                        _type = U.TryGetType(split[0]);
                    }

                    ConstructorInfo ci = _type.GetConstructor(new Type[] { typeof(string) });
                    if (ci != null)
                    {
                        _literalValue = Activator.CreateInstance(_type, split[1]);
                    }
                    else
                    {
                        _literalValue = Convert.ChangeType(split[1], _type);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Bad format for literal '{0}'", ID));
                }
            }
            //}
            //else
            //{

            //}

        }
        /// <summary>
        /// Get the lambda expression for this property
        /// </summary>
        /// <returns></returns>
        public Expression GetGetPropExpression<T>()
        {
            if (_target != null && _pi != null)
            {
                try
                {
                    var arg = Expression.Constant(_target, _target.GetType());
                    var body = Expression.Property(arg, _pi);
                    var lambda = Expression.Lambda<Func<T>>(body);
                    return lambda;  
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            return null;
        }
        /// <summary>
        /// Invoke the 
        /// </summary>
        /// <returns></returns>
        public object Invoke()
        {
            if (IsLiteral)
            {

                return _literalValue;
            }
            return _getter(_target);
        }

        /// <summary>
        /// Create an instance of a PropWrapper based on the supplied ID string
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expectedType"></param>
        /// <param name="delOnChanged"></param>
        /// <returns></returns>
        public static PropertyWrapper Create(string id, Type expectedType, MethodInvoker delOnChanged)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            try
            {
                PropertyWrapper propWrapper = new PropertyWrapper(id, expectedType) { _delOnChanged = delOnChanged };
                propWrapper.Initialize();
                return propWrapper;
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to create Property Wrapper");
            }
            return null;
        }
        /// <summary>
        /// Create an instance of a PropWrapper based on the supplied ID string
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expectedType"></param>
        /// <param name="delOnChanged"></param>
        /// <returns></returns>
        public static PropertyWrapper Create(string id, MethodInvoker delOnChanged)
        {
            return Create(id, null, delOnChanged);
        }

        /// <summary>
        /// Create an instance of a PropWrapper based on the supplied ID string
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static PropertyWrapper Create(string id)
        {
            return Create(id, null);
        }

        public static PropertyWrapper Create<T>(Expression<Func<T>> propertyLambda)
        {
            // Convert Property to ID
            MemberExpression member = propertyLambda.Body as MemberExpression;
            PropertyInfo propInfo = member.Member as PropertyInfo;
            // Get the target object
            Func<Object> delObj = Expression.Lambda<Func<Object>>(member.Expression).Compile();
            object objTarget = delObj();
            if (objTarget is CompBase)
            {

                return Create((objTarget as CompBase).ID + "." + propInfo.Name);
            }
            return null;
        }
    }
}
