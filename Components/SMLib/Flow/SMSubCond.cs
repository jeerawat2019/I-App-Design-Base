using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MCore.Comp.SMLib.Flow
{
    public class SMSubCond:SMSubCondBase
    {
        private PropertyWrapper _propWrapperSource = null;
        private PropertyWrapper _propWrapperCompare = null;

     
        private bool _needsRebuild = true;    
        private object _lockNotifier = new object();



        private object _fixCompareValue = null;


        public string ConditionID
        {
            get { return GetPropValue(() => ConditionID, string.Empty); }
            set
            {
                if (SetPropValue(() => ConditionID, value))
                {
                    _needsRebuild = true;
                    Rebuild();
                }
            }
        }

        public String ConditionValueString
        {
            get { return GetPropValue(() => ConditionValueString, string.Empty); }
            set { SetPropValue(() => ConditionValueString, value); }
        }

        public String OperatorString
        {
            get { return GetPropValue(() => OperatorString, string.Empty); }
            set { SetPropValue(() => OperatorString, value); }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMSubCond()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public SMSubCond(string childName)
            : base(childName)
        {
            
        }

        #endregion

        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            Rebuild();
        }

        /// <summary>
        /// Rebuild the internals
        /// </summary>
        public override void Rebuild()
        {
            _needsRebuild = true;
            DoRebuild();
        }

        private void DoRebuild()
        {
            lock (this)
            {
                if (!_needsRebuild)
                {
                    return;
                }

                //DoDestroy();

                _propWrapperSource = null;
                _propWrapperCompare = null;

                System.Reflection.PropertyInfo sourcePropInfo = U.GetPropertyInfo(this.ConditionID);
                Type sourcePropType = sourcePropInfo.PropertyType;
                _propWrapperSource = PropertyWrapper.Create(ConditionID, sourcePropType, null);

                //Convert Compare Value
                if(ConditionValueString.Contains("(Object)"))
                {
                    if (_propWrapperSource.PropertyType.IsSubclassOf(typeof(MDouble.MDoubleBase)))
                    {
                        var compareVal = Double.Parse(ConditionValueString.Replace("(Object)", ""));
                        _fixCompareValue = compareVal;
                    }
                    else
                    {
                        var compareVal = TypeDescriptor.GetConverter(_propWrapperSource.PropertyType).ConvertFromString(ConditionValueString.Replace("(Object)", ""));
                        _fixCompareValue = compareVal;
                    }
                }
                else
                {
                    System.Reflection.PropertyInfo comparePropInfo = U.GetPropertyInfo(this.ConditionValueString);
                    Type comparePropType = comparePropInfo.PropertyType;
                    _propWrapperCompare = PropertyWrapper.Create(ConditionValueString, comparePropType, null);
                }

                //Pre-Validate
                Validate();
                _needsRebuild = false;
            }
        }

        /// <summary>
        /// Destroy this Component
        /// </summary>
        public override void Destroy()
        {
          
            base.Destroy();
        }

        public override bool Validate()
        {
            Object sourceVal = null;
            Object compareVal = null;

            lock (this)
            {
                string x = _propWrapperSource.PropertyType.ToString();
                if (_propWrapperSource.PropertyType.ToString().Contains("System") || _propWrapperSource.PropertyType.ToString().Contains("+"))
                {
                    var sourceProp = _propWrapperSource.Invoke();
                    sourceVal = sourceProp;
                }
                else if (_propWrapperSource.PropertyType.IsSubclassOf(typeof(MDouble.MDoubleBase)))
                {
                    MDouble.MDoubleBase sourceProp = (MDouble.MDoubleBase)_propWrapperSource.Invoke();
                    sourceVal = sourceProp.Val;
                }


                if (_propWrapperCompare != null)
                {
                    if (_propWrapperCompare.PropertyType.ToString().Contains("System") || _propWrapperCompare.PropertyType.ToString().Contains("+"))
                    {
                        var compareProp = _propWrapperCompare.Invoke();
                        compareVal = compareProp;
                    }
                    else if (_propWrapperCompare.PropertyType.IsSubclassOf(typeof(MDouble.MDoubleBase)))
                    {
                        MDouble.MDoubleBase compareProp = (MDouble.MDoubleBase)_propWrapperCompare.Invoke();
                        compareVal = compareProp.Val;
                    }
                }
                else
                {
                    compareVal = _fixCompareValue;
                }
            }

            dynamic sVal = sourceVal;
            dynamic cVal = compareVal;

            //Compare Value
            if (OperatorString == "==")
            {
                ValidationResult = sVal == cVal;
                return ValidationResult;
            }
            else if (OperatorString == "!=")
            {
                ValidationResult = sVal != cVal;
                return ValidationResult;
            }
            else if (OperatorString == ">")
            {
                ValidationResult = sVal > cVal;
                return ValidationResult;
            }
            else if (OperatorString == ">=")
            {
                ValidationResult = sVal >= cVal;
                return ValidationResult;
            }
            else if (OperatorString == "<")
            {
                ValidationResult = sVal < cVal;
                return ValidationResult;
            }
            else if (OperatorString == "<=")
            {
                ValidationResult = sVal <= cVal;
                return ValidationResult;
            }
            return base.Validate();
        }

        public override CompBase Clone(string name, bool bRecursivley)
        {
            CompBase cloneComp = base.Clone(name, bRecursivley);
            (cloneComp as SMSubCond).ConditionID = this.ConditionID;
            (cloneComp as SMSubCond).ConditionValueString = this.ConditionValueString;
            return cloneComp;
        }
    }
}
