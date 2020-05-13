using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MCore.Comp
{
    public class MethodWrapper
    {
        /// <summary>
        /// The owner object of the method
        /// </summary>
        private CompBase _target { get; set; }
        
        /// <summary>
        /// The method info
        /// </summary>
        private MethodInfo _mi { get; set; }
        public MethodInfo MI
        {
            get { return _mi; }
        }
        
       
        private D.GenericDelegateWithArgs _delMethod = null;

        private PropertyWrapper[] _args = null;

        private Type[] _argTypes = null;
        public Type[] ArgTypes
        {
            get { return _argTypes; }
        }
        /// <summary>
        /// Get the method name
        /// </summary>
        public string MethodName
        {
            get { return _mi.Name; }
        }
        /// <summary>
        /// Create an instance of the Method wrapper.
        /// If error with id string, throws an exception
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MethodWrapper Create(string id)
        {
            try
            {
                return new MethodWrapper(id);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to create Method Wrapper");
            }
            return null;
        }
        private MethodWrapper(string id)
        {
            _args = null;

            // Assumes a MethodId exists      
            string methodName = string.Empty;
            string[] sArgs = null;
            string methodPath = string.Empty;

            if (!id.Contains('.'))
            {
                //throw new MCoreExceptionPopup("The MethodID should contain a Target component: '{0}'", id);
                _target = U.RootComp;
                sArgs = U.SplitMethodID(id, out methodPath);
                methodName = methodPath;
            }
            else
            {
                sArgs = U.SplitMethodID(id, out methodPath);

                if (methodPath.Contains("."))
                {
                    // Extract Target and Method name
                    int lastPeriod = methodPath.LastIndexOf('.');
                    _target = U.GetComponent(methodPath.Substring(0, lastPeriod));
                    methodName = methodPath.Substring(lastPeriod + 1);
                }
                else
                {
                    _target = U.RootComp;
                    methodName = methodPath;
                }
            }

            _argTypes = null;
            if (sArgs != null)
            {
                _args = new PropertyWrapper[sArgs.Length];
                _argTypes = new Type[sArgs.Length];
                for (int i = 0; i < sArgs.Length; i++)
                {

                    // Extract Target and Method name
                    PropertyWrapper propWrapper = PropertyWrapper.Create(sArgs[i]);
                    _args[i] = propWrapper;
                    _argTypes[i] = propWrapper.PropertyType;
                }
            }
            else
            {
                _argTypes = new Type[0];
            }


            _mi = _target.GetType().GetMethod(methodName, _argTypes);
            if (_mi == null)
            {
                throw new Exception(string.Format("Could not obtain the Method name from the MethodID: '{0}'", id));
            }

            _delMethod = D.CreateGenericDelegateWithArgs(_mi);
            

        }
        /// <summary>
        /// Invoke this Method and argument properties
        /// </summary>
        public void Invoke()
        {
            object[] args = null;
            if (_args != null)
            {
                args = new object[_args.Length];
                for (int i = 0; i < _args.Length; i++)
                {
                    args[i] = _args[i].Invoke();
                }
            }
            _delMethod(_target, args);
        }
        /// <summary>
        /// Invoke this Method and argument properties
        /// </summary>
        public void BeginInvoke()
        {
            object[] args = null;
            if (_args != null)
            {
                args = new object[_args.Length];
                for (int i = 0; i < _args.Length; i++)
                {
                    args[i] = _args[i].Invoke();
                }
            }
            else
            {
                args = new object[0];

            }
            try
            {
                _delMethod.BeginInvoke(_target, args, null, null);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
