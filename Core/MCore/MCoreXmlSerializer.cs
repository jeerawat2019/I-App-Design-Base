using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace MCore
{
    /// <summary>
    /// Class to manage XML serialization
    /// </summary>
    public class MCoreXmlSerializer
    {
        // Static mapping
        private static Dictionary<Type, MCoreXmlSerializer> s_serializers = new Dictionary<Type, MCoreXmlSerializer>();
        private static ReaderWriterLockSlim _serializerLock = new ReaderWriterLockSlim();
        private static XmlAttributeOverrides s_attrOverrides = null;

        // Local mapping
        private SortedList<string, ClassMapping> _classTypeList = null;
        private SortedList<string, Type> _shortNameTypeList = null;
 

        private const string _xsi = "http://www.w3.org/2001/XMLSchema-instance";
        private const string _CLASS_NOT_FOUND = "ClassNotFound";

        private Type _tyRoot = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tyRoot"></param>
        public MCoreXmlSerializer(Type tyRoot)
        {
            Setup(tyRoot, s_attrOverrides);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tyRoot"></param>
        /// <param name="overrides"></param>
        public MCoreXmlSerializer(Type tyRoot, XmlAttributeOverrides overrides)
        {
            Setup(tyRoot, overrides);
        }
        /// <summary>
        /// Define new serializer for this type
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        /// <remarks>We cache it because it takes so long to create the MclXmlSerializer object.</remarks>
        public static MCoreXmlSerializer DefineSerializer(Type ty, XmlAttributeOverrides overrides)
        {
            MCoreXmlSerializer ser = null;
            _serializerLock.EnterWriteLock();
            s_attrOverrides = overrides;
            try
            {
                if (!s_serializers.ContainsKey(ty))
                {
                    ser = new MCoreXmlSerializer(ty, overrides);
                    s_serializers.Add(ty, ser);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to define serializer for type {0}", ty.Name);
            }
            finally
            {
                _serializerLock.ExitWriteLock();
            }
            return ser;
        }
        /// <summary>
        /// Define new serializer for this type
        /// </summary>
        /// <param name="ty"></param>
        /// <returns></returns>
        /// <remarks>We cache it because it takes so long to create the MclXmlSerializer object.</remarks>
        public static MCoreXmlSerializer DefineSerializer(Type ty)
        {
            return DefineSerializer(ty, s_attrOverrides);
        }
        /// <summary>
        /// Get the cached serializer
        /// </summary>
        /// <param name="ty"></param>
        /// <returns></returns>
        /// <remarks>We cache it because it takes so long to create the MclXmlSerializer object.</remarks>
        private static MCoreXmlSerializer GetSerializer(Type ty)
        {
            _serializerLock.EnterReadLock();
            try
            {
                if (s_serializers.ContainsKey(ty))
                {
                    return s_serializers[ty];
                }
            }
            catch { }
            finally
            {
                _serializerLock.ExitReadLock();
            }
            return DefineSerializer(ty);
        }

        /// <summary>
        /// Deserialize the object from a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static object LoadObjectFromFile(string filePath, Type ty)
        {
            MCoreXmlSerializer ser = MCoreXmlSerializer.GetSerializer(ty);
            object obj = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                obj = ser.Deserialize(fs);
            }
            return obj;
        }

        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        public static void SaveObjectToFile(string filePath, object obj)
        {
            MCoreXmlSerializer ser = GetSerializer(obj.GetType());
            TextWriter tw = new StreamWriter(filePath, false);
            ser.Serialize(tw, obj);
            tw.Close();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tyRoot"></param>
        /// <param name="overrides"></param>
        private void Setup(Type tyRoot, XmlAttributeOverrides overrides)
        {
            _tyRoot = tyRoot;
            // Recurse to get Methods, names of all types involved with this root type
            // We try to reuse the static if possible
            Type tyFixed = GetArrayFixedType(_tyRoot);
            if (tyFixed == null)
                tyFixed = _tyRoot;
            // Must generate another mapping
            _classTypeList = new SortedList<string, ClassMapping>();
            _shortNameTypeList = new SortedList<string, Type>();          

            try
            {
                BuildClassTypeListRecurse(tyFixed, overrides);
                foreach (ClassMapping cm in _classTypeList.Values)
                {
                    if (!_shortNameTypeList.ContainsKey(cm.ty.Name))
                    {
                        _shortNameTypeList.Add(cm.ty.Name, cm.ty);
                    }
                    //U.ProcessEvent("ClassMapping", cm.ty.FullName);
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (MCoreException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Exception");
            }
        }
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <param name="tw"></param>
        /// <param name="obj"></param>
        public void Serialize(TextWriter tw, object obj)
        {
            if (obj == null)
                return;
            try
            {
                XmlDocument xDoc = new XmlDocument();
                SerializeClassObject(xDoc, xDoc, obj, null, null);
                xDoc.Save(tw);
            }
            catch (MCoreException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Exception");
            }
        }

        /// <summary>
        /// Deserialize the object
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public object Deserialize(FileStream fs)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fs);
                foreach (XmlNode xmlNode in xDoc.ChildNodes)
                {
                    // Deserialize the first element
                    if (xmlNode is XmlElement)
                    {

                        // It is Class, field, attribute, array
                        // Get the type for this node
                        // Create this object
                        return InstantiateAndPopulateClassRecurse(xmlNode, _tyRoot);
                    }
                }
            }
            catch (MCoreException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Exception");
            }
            return null;
        }
        
        private XmlElement CreateArrayXmlElement(XmlDocument xDoc, object objArrayItem, ClassField cf, string xmlArrayElementName)
        {
            Type objType = objArrayItem.GetType();
            // Try to get type from Arraymapping
            ArrayMapping am = cf.arrayMapping;
            bool bForceTypeDeclaration = cf.xmlArrayFixedType == null || objArrayItem.GetType() != cf.xmlArrayFixedType;
            if (am != null)
            {
                if (am.arrayNameList.ContainsValue(objType))
                {
                    bForceTypeDeclaration = false;
                    int iKey = am.arrayNameList.IndexOfValue(objType);
                    xmlArrayElementName = am.arrayNameList.Keys[iKey];
                }
                else if (am.arrayNameList.ContainsValue(null))
                {
                    int iKey = am.arrayNameList.IndexOfValue(null);
                    xmlArrayElementName = am.arrayNameList.Keys[iKey];
                }

            }
            if (xmlArrayElementName == null)
                xmlArrayElementName = cf.mi.Name;

            XmlElement elem = xDoc.CreateElement(xmlArrayElementName);

            if (bForceTypeDeclaration)
            {
                // Need to force the type
                XmlAttribute attrType = xDoc.CreateAttribute("xsi", "type", _xsi);
                attrType.Value = objType.Name;
                elem.Attributes.Append(attrType);
            }
            return elem;
        }

        private void SerializeClassObject(XmlDocument xDoc, XmlNode xmlNodeParent, object objClass, ClassField cfArray, string xmlClassElementName)
        {
            ClassMapping cm = null;
            XmlElement elemClass = null;
            try
            {
                Type classType = objClass.GetType();
                string classTypeFullName = classType.FullName;
                if (!_classTypeList.ContainsKey(classTypeFullName))
                {
                    U.LogPopup("Xml serializer is missing class '{0}'", classTypeFullName);
                    return;
                }
                cm = _classTypeList[classTypeFullName];

                if (xmlClassElementName == null)
                {
                    if (cfArray != null)
                    {
                        if (cfArray.xmlArrayFixedType != null)
                        {
                            xmlClassElementName = cfArray.xmlArrayFixedType.Name;
                        }
                        else
                        {
                            xmlClassElementName = cfArray.mi.Name;
                        }
                    }
                    else
                    {
                        xmlClassElementName = cm.xmlClassName;
                    }
                }

                if (cfArray != null)
                {
                    elemClass = CreateArrayXmlElement(xDoc, objClass, cfArray, xmlClassElementName);
                }
                else
                {
                    elemClass = xDoc.CreateElement(xmlClassElementName);
                }
                xmlNodeParent.AppendChild(elemClass);
            }
            catch
            {
                throw;
            }
            if (xmlNodeParent == xDoc)
            {
                try
                {

                    XmlAttribute attrXsiSchema = xDoc.CreateAttribute("xmlns:xsi");
                    attrXsiSchema.Value = _xsi;
                    elemClass.Attributes.Append(attrXsiSchema);

                    XmlAttribute attrXsdSchema = xDoc.CreateAttribute("xmlns:xsd");
                    attrXsdSchema.Value = _xsi;
                    elemClass.Attributes.Append(attrXsdSchema);

                }
                catch
                {
                    throw;
                }

            }

            foreach (AttrNameInfo ani in cm.xmlAttrNames.Values)
            {
                try
                {
                    string val = ani.cf.GetStringValue(objClass);
                    if (val!= null)
                    {
                        XmlAttribute attr = xDoc.CreateAttribute(ani.attribute.AttributeName);
                        attr.Value = val;
                        elemClass.Attributes.Append(attr);
                    }
                }
                catch
                {
                    throw;
                }
            }
            if (!string.IsNullOrEmpty(cm.xmlNameSpace))
            {
                XmlAttribute attrNamespace = xDoc.CreateAttribute("xmlns");
                attrNamespace.Value = cm.xmlNameSpace;
                elemClass.Attributes.Append(attrNamespace);
            }

            foreach (ClassField cf in cm.fields)
            {
                try
                {
                    object objValue = cf.GetValue(objClass);
                    if (objValue != null)
                    {
                        Type objValueType = objValue.GetType();
                        string xmlElementName = cf.GetElementName(objValueType);

                        if (cf.bArray)
                        {
                            if (!(objValue is System.Collections.IList))
                            {
                                throw new MCoreExceptionPopup("Unsupported array type ({0})", xmlElementName);
                            }
                            
                            if (objValueType.IsGenericType && objValueType.GetGenericArguments().Length != 1)
                            {
                                throw new MCoreExceptionPopup("Only one-dimensional generic arrays supported ({0})", xmlElementName);
                            }

                            System.Collections.IList list = (System.Collections.IList)objValue;

                            XmlElement elemArrayParent = null;
                            if (cf.arrayMapping == null)
                            {
                                // Keep on same level
                                elemArrayParent = elemClass;
                            }
                            else
                            {
                                // Create an array parent
                                elemArrayParent = xDoc.CreateElement(xmlElementName);
                                xmlElementName = null;
                                elemClass.AppendChild(elemArrayParent);
                            }

                            foreach (object objItem in list)
                            {
                                if (objItem == null)
                                {
                                    ArrayMapping am = cf.arrayMapping;
                                    if (am != null)
                                    {
                                        // Add a null field
                                        string itemName = xmlElementName;
                                        if (itemName == null)
                                        {
                                            if (cf.xmlArrayFixedType == typeof(string))
                                            {
                                                itemName = "string";
                                            }
                                            else if (am.arrayNameList.ContainsValue(null))
                                            {
                                                int iKey = am.arrayNameList.IndexOfValue(null);
                                                itemName = am.arrayNameList.Keys[iKey];
                                            }
                                            else if (am.arrayNameList.Count == 1)
                                            {
                                                itemName = am.arrayNameList.Keys[0];
                                            }
                                            else if (cf.xmlArrayFixedType != null)
                                            {
                                                itemName = cf.xmlArrayFixedType.Name;
                                            }
                                            else
                                            {
                                                itemName = cf.mi.Name;
                                            }
                                        }
                                        XmlElement elemField = xDoc.CreateElement(itemName);
                                        XmlAttribute attrNil = xDoc.CreateAttribute("xsi", "nil", _xsi);
                                        attrNil.Value = "true";
                                        elemField.Attributes.Append(attrNil);
                                        elemArrayParent.AppendChild(elemField);
                                    }
                                    continue;
                                }

                                if (_classTypeList.ContainsKey(objItem.GetType().FullName))
                                {
                                    SerializeClassObject(xDoc, elemArrayParent, objItem, cf, xmlElementName);
                                }
                                else
                                {
                                    // Regular array item type.  Not a class or array
                                    XmlElement elemField = CreateArrayXmlElement(xDoc, objItem, cf, xmlElementName);
                                    if (!(objItem is string) || !string.IsNullOrEmpty(objItem as string))
                                    {
                                        XmlText nodeText = xDoc.CreateTextNode(cf.ConvertObjToXmlString(objItem));
                                        elemField.AppendChild(nodeText);
                                    }
                                    elemArrayParent.AppendChild(elemField);
                                }
                            }
                        }
                        else
                        {
                            // Not an array
                            if (_classTypeList.ContainsKey(objValueType.FullName))
                            {
                                // It is a class
                                SerializeClassObject(xDoc, elemClass, objValue, null, xmlElementName);
                            }
                            else if (objValueType == typeof(string) && (objValue as string).Length == 0)
                            {
                                // string type.  Not a class or array
                                XmlElement elemField = xDoc.CreateElement(xmlElementName);
                                elemClass.AppendChild(elemField);
                            }
                            else
                            {
                                // Regular type.  Not a class or array
                                XmlElement elemField = xDoc.CreateElement(xmlElementName);
                                XmlText nodeText = xDoc.CreateTextNode(cf.ConvertObjToXmlString(objValue));
                                elemField.AppendChild(nodeText);
                                elemClass.AppendChild(elemField);
                            }
                        }
                    }
                    else
                    {
                        // Value is null
                        //if (!cf.bArray && cf.HasSingleElementName && cf.SingleElementType == typeof(string))
                        //{
                        //    XmlElement elemField = xDoc.CreateElement(cf.SingleElementName);
                        //    elemClass.AppendChild(elemField);
                        //}
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        private bool ValueTypeHasMultipleChildren(Type ty)
        {
            int nChildValues = 0;
            MemberInfo[] miList = ty.GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (MemberInfo mi in miList)
            {
                if (mi.MemberType == MemberTypes.Property)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    if (pi.CanRead && pi.CanWrite)
                    {
                        ParameterInfo[] paramInfo = pi.GetIndexParameters();
                        if (paramInfo.Length == 0)
                        {
                            nChildValues++;
                        }
                    }
                }
                else if (mi.MemberType == MemberTypes.Field)
                {
                    nChildValues++;
                }
            }
            return nChildValues > 1;
        }
        private bool HasChildValues(Type ty)
        {
            if (ty.IsValueType)
            {
                // Check if this value has multiple children values
                if (ValueTypeHasMultipleChildren(ty))
                {
                    return true;
                }
                return false;
            }

            if (ty.IsClass && ty != typeof(string))
            {
                return true;
            }
            return false;

        }
        private void BuildClassTypeListRecurse(Type tyClass, XmlAttributeOverrides overrides)
        {
            if (tyClass == null)
                return;
            bool bSubValues = false;
            if (tyClass.IsValueType)
            {
                // Check if this value has multiple children values
                bSubValues = HasChildValues(tyClass);
            }
            Type tyIList = tyClass.GetInterface("IList");
            if (!HasChildValues(tyClass) ||
                tyIList != null ||
                tyClass == typeof(System.Type) ||
                tyClass == typeof(System.Object)
                )
            {
                //U.ProcessEvent("AtemptedClass", tyClass.FullName);
                return;
            }
            // Only do this once per class
            if (_classTypeList.ContainsKey(tyClass.FullName))
                return;

            //if (bSubValues)
            //{
            //    U.ProcessEvent("SubValues", tyClass.FullName);
            //}

            List<Type> classIncludes = new List<Type>();
            string xmlNameSpace = null;
            string xmlClassName = tyClass.Name;
            object[] customAttrs = tyClass.GetCustomAttributes(false);
            // Attributes to the class declaration
            foreach (object customAttr in customAttrs)
            {
                if (customAttr is XmlRootAttribute)
                {
                    XmlRootAttribute xmlRoot = customAttr as XmlRootAttribute;
                    if (!string.IsNullOrEmpty(xmlRoot.ElementName))
                    {
                        xmlClassName = xmlRoot.ElementName;
                    }
                    if (!string.IsNullOrEmpty(xmlRoot.Namespace))
                    {
                        xmlNameSpace = xmlRoot.Namespace;
                    }
                }
                if (customAttr is XmlIncludeAttribute)
                {
                    XmlIncludeAttribute xmlInclude = customAttr as XmlIncludeAttribute;
                    if (xmlInclude.Type != null)
                        classIncludes.Add(xmlInclude.Type);
                }
            }
            XmlAttributes attrsClassOverrides = overrides == null ? null : overrides[tyClass];
            if (attrsClassOverrides != null)
            {
                if (attrsClassOverrides.XmlRoot != null)
                {
                    if (!string.IsNullOrEmpty(attrsClassOverrides.XmlRoot.ElementName))
                    {
                        xmlClassName = attrsClassOverrides.XmlRoot.ElementName;
                    }
                }
            }

            ClassMapping cm = new ClassMapping(tyClass, xmlClassName, xmlNameSpace);
            try
            {
                _classTypeList.Add(tyClass.FullName, cm);
            }
            catch (MCoreException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Unable to add '{0}' to '{1}'", tyClass.FullName, xmlClassName);
            }

            Type ty = tyClass;
            while (ty != null && ty != typeof(object))
            {
                try
                {
                    attrsClassOverrides = overrides == null ? null : overrides[ty];
                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Unable to set attrsClassOverrides (type= '{0}'", ty.Name);
                }
                MemberInfo[] miList = ty.GetMembers(BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.DeclaredOnly | BindingFlags.Public);

                // Add in reverse order because we traverse upward in hierarchy
                // By so doing, we end up with fields in the correct order
                for (int i = miList.Length-1; i >= 0; i--)
                {
                    MemberInfo mi = miList[i];
                    switch (mi.MemberType)
                    {
                        default:
                            break; 
                        case MemberTypes.Field:
                            try
                            {
                                MapFieldMember(overrides, cm, mi);
                            }
                            catch (MCoreException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new MCoreExceptionPopup(ex, "Unable to MapFieldMember (FieldName= '{0}')", mi.Name);
                            }
                            break;
                        case MemberTypes.Property:
                            try
                            {
                                PropertyInfo pi = mi as PropertyInfo;
                                if (pi.CanRead && pi.CanWrite)
                                {
                                    ParameterInfo[] paramInfo = null;
                                    try
                                    {
                                        paramInfo = pi.GetIndexParameters();
                                    }
                                    catch { }
                                    if (paramInfo != null && paramInfo.Length == 0)
                                    {
                                        MapFieldMember(overrides, cm, mi);
                                    }
                                }
                            }
                            catch (FileNotFoundException)
                            {
                                throw;
                            }
                            catch (MCoreException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new MCoreExceptionPopup(ex, "Unable to MapFieldMember (FieldName= '{0}')", mi.Name);
                            }
                            break;
                    }
                }
                ty = ty.BaseType;
            }
            foreach (Type tyClassToInclude in classIncludes)
            {
                BuildClassTypeListRecurse(tyClassToInclude, overrides);
            }

        }
        /// <summary>
        /// Get the Type according the the MemberInfo
        /// Must be either PropertyInfo ot FieldInfo
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public static Type GetTypeFromMemberInfo(MemberInfo mi)
        {
            if (mi is PropertyInfo)            
            {
                return (mi as PropertyInfo).PropertyType;
            }
            if (mi is FieldInfo)
            {
                return (mi as FieldInfo).FieldType;
            }


            return null;
        }

        private Type GetArrayFixedType(Type tyArray)
        {
            if (tyArray.IsGenericType)
            {
                Type[] tyGens = tyArray.GetGenericArguments();
                if (tyGens == null || tyGens.Length != 1)
                {
                    // Cannot support multiple generic argument 
                    return null;
                }
                return tyGens[0];
            }
            return tyArray.GetElementType();
        }

        private void MapFieldMember(XmlAttributeOverrides overrides,  ClassMapping cm, MemberInfo mi)
        {
            bool bIgnore = false;
            bool bArray = false;
            XmlAttributeAttribute attribute = null;
            object[] customFieldAttrs = mi.GetCustomAttributes(false);
            ArrayMapping arrayMapping = null;

            Type tyFieldBase = GetTypeFromMemberInfo(mi);
            if (tyFieldBase == null)
                return;

            Type xmlArrayFixedType = null;

            // This will be true for all array types.  Even for generic lists and fixed arrays
            bArray = tyFieldBase.GetInterface("IList") != null;
            if (bArray)
            {
                xmlArrayFixedType = GetArrayFixedType(tyFieldBase);
                if (xmlArrayFixedType != null && !tyFieldBase.IsGenericType)
                {
                    BuildClassTypeListRecurse(xmlArrayFixedType, overrides);
                }
            }

            string xmlElementName = null;
            string xmlArrayName = null;
            List<NameToType> fieldNameToTypes = new List<NameToType>();

            try
            {
                foreach (object customAttr in customFieldAttrs)
                {
                    if (customAttr is XmlIgnoreAttribute)
                    {
                        bIgnore = true;
                    }
                    else if (customAttr is XmlAttributeAttribute)
                    {
                        attribute = customAttr as XmlAttributeAttribute;
                        if (string.IsNullOrEmpty(attribute.AttributeName))
                        {
                            attribute.AttributeName = mi.Name;
                        }
                    }
                    else if (customAttr is XmlElementAttribute)
                    {
                        XmlElementAttribute xmlElement = (customAttr as XmlElementAttribute);
                        if (!string.IsNullOrEmpty(xmlElement.ElementName))
                        {
                            xmlElementName = xmlElement.ElementName.Replace(' ', '_');
                            Type xmlElementType = xmlElement.Type != null ? xmlElement.Type : tyFieldBase;
                            try
                            {
                                fieldNameToTypes.Insert(0, new NameToType(xmlElementName, xmlElementType));
                                if (xmlElement.Type != null && xmlElement.Type != tyFieldBase)
                                {
                                    BuildClassTypeListRecurse(xmlElementType, overrides);
                                }
                            }
                            catch (MCoreException)
                            {
                                throw;
                            }
                            catch (Exception ex)
                            {
                                throw new MCoreExceptionPopup(ex,
                                    "Could not insert '{0}' of type '{1}' into fieldNameToTypes", xmlElementName, xmlElementType.Name);
                            }
                        }
                    }
                    else if (customAttr is XmlArrayAttribute)
                    {
                        if (!bArray)
                            bIgnore = true;
                        else
                        {
                            XmlArrayAttribute xmlArray = (customAttr as XmlArrayAttribute);
                            if (!string.IsNullOrEmpty(xmlArray.ElementName))
                            {
                                xmlArrayName = xmlArray.ElementName.Replace(' ', '_');
                                try
                                {
                                    fieldNameToTypes.Insert(0, new NameToType(xmlArrayName, tyFieldBase));
                                }
                                catch (Exception ex)
                                {
                                    throw new MCoreExceptionPopup(ex,
                                        "Could not insert '{0}' of type '{1}' into fieldNameToTypes", xmlArrayName, tyFieldBase.Name);
                                }
                            }
                        }
                    }
                    else if (customAttr is XmlArrayItemAttribute)
                    {
                        if (!bArray)
                            bIgnore = true;
                        else
                        {

                            XmlArrayItemAttribute item = (customAttr as XmlArrayItemAttribute);
                            if (!string.IsNullOrEmpty(item.ElementName))
                            {
                                if (arrayMapping == null)
                                    arrayMapping = new ArrayMapping();
                                arrayMapping.Add(item.ElementName, item.Type);
                            }
                            if (item.Type != null)
                            {
                                BuildClassTypeListRecurse(item.Type, overrides);
                            }
                        }
                    }
                }
            }
            catch (MCoreException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Reading Custom Attributes");
            }
            try
            {
                Type ty = cm.ty;
                while (ty != null && ty != typeof(object))
                {
                    XmlAttributes attrsMemberOverrides = overrides == null ? null : overrides[ty, mi.Name];
                    // Do overrides
                    if (attrsMemberOverrides != null)
                    {
                        bIgnore = attrsMemberOverrides.XmlIgnore;                        
                        if (!bIgnore)
                        {
                            if (attrsMemberOverrides.XmlAttribute != null)
                            {
                                attribute = attrsMemberOverrides.XmlAttribute;
                                if (string.IsNullOrEmpty(attribute.AttributeName))
                                {
                                    attribute.AttributeName = mi.Name;
                                }
                            }
                            if (attrsMemberOverrides.XmlElements != null)
                            {
                                foreach (XmlElementAttribute xmlElement in attrsMemberOverrides.XmlElements)
                                {
                                    if (!string.IsNullOrEmpty(xmlElement.ElementName))
                                    {
                                        xmlElementName = xmlElement.ElementName.Replace(' ', '_');
                                        Type xmlElementType = xmlElement.Type != null ? xmlElement.Type : tyFieldBase;
                                        try
                                        {
                                            fieldNameToTypes.Insert(0, new NameToType(xmlElementName, xmlElementType));
                                            if (xmlElement.Type != null && xmlElement.Type != tyFieldBase)
                                            {
                                                BuildClassTypeListRecurse(xmlElementType, overrides);
                                            }
                                        }
                                        catch (MCoreException)
                                        {
                                            throw;
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new MCoreExceptionPopup(ex,
                                                "Could not add '{0}' of type '{1}' to fieldNameToTypes", xmlElementName, xmlElementType.Name);
                                        }
                                    }
                                }
                            }
                            if (attrsMemberOverrides.XmlArray != null)
                            {
                                if (!string.IsNullOrEmpty(attrsMemberOverrides.XmlArray.ElementName))
                                {
                                    try
                                    {
                                        xmlArrayName = attrsMemberOverrides.XmlArray.ElementName.Replace(' ', '_');
                                        fieldNameToTypes.Insert(0, new NameToType(xmlArrayName, tyFieldBase));
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new MCoreExceptionPopup(ex,
                                            "Could not Insert '{0}' of type '{1}' to fieldNameToTypes", xmlArrayName, tyFieldBase.Name);
                                    }
                                }
                            }
                            if (attrsMemberOverrides.XmlArrayItems != null && attrsMemberOverrides.XmlArrayItems.Count > 0)
                            {
                                // Replace old list
                                arrayMapping = new ArrayMapping();

                                foreach (XmlArrayItemAttribute arrayItem in attrsMemberOverrides.XmlArrayItems)
                                {
                                    if (arrayItem != null)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(arrayItem.ElementName))
                                            {
                                                arrayMapping.Add(arrayItem.ElementName, arrayItem.Type);
                                            }
                                            if (arrayItem.Type != null)
                                            {
                                                BuildClassTypeListRecurse(arrayItem.Type, overrides);
                                            }
                                        }
                                        catch (MCoreException ex)
                                        {
                                            throw ex;
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new MCoreExceptionPopup(ex,
                                               "Could not add '{0}' of type '{1}' to arrayMapping", arrayItem.ElementName, arrayItem.Type.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ty = ty.BaseType;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            if (!bIgnore)
            {
                try
                {
                    if (bArray && arrayMapping == null)
                    {
                        if (string.IsNullOrEmpty(xmlElementName) && xmlArrayFixedType == null)
                        {
                            arrayMapping = new ArrayMapping();
                            arrayMapping.Add("anyType", null);
                        }
                        else if (xmlArrayFixedType != null)
                        {
                            if (string.IsNullOrEmpty(xmlElementName) || !string.IsNullOrEmpty(xmlArrayName))
                            {
                                arrayMapping = new ArrayMapping();
                                // Double, String, etc
                                switch (xmlArrayFixedType.Name)
                                {
                                    default:
                                        arrayMapping.Add(xmlArrayFixedType.Name, xmlArrayFixedType);
                                        break;
                                    case "Double":
                                        arrayMapping.Add("double", typeof(double));
                                        break;
                                    case "String":
                                        arrayMapping.Add("string", typeof(string));
                                        break;
                                }
                            }
                            BuildClassTypeListRecurse(xmlArrayFixedType, overrides);
                        }
                    }

                    if (fieldNameToTypes.Count == 0)
                    {
                        fieldNameToTypes.Insert(0, new NameToType(mi.Name, tyFieldBase));
                    }
                    ClassField cf = new ClassField(bArray, xmlArrayFixedType, mi, arrayMapping, fieldNameToTypes);
                    if (attribute != null)
                    {
                        cm.AddAttribute(attribute, tyFieldBase, cf);
                    }
                    else
                    {
                        cm.AddField(cf);
                    }
                    if (!bArray)
                    {
                        BuildClassTypeListRecurse(tyFieldBase, overrides);
                    }
                }
                catch 
                {
                    throw;
                }
            }
        }

        //Type listType, Type fixedItemType, int count)
        private System.Collections.IList InstantiateArray(FieldNameInfo fni, object objParent, XmlNode xmlArrayNode)
        {
            if (xmlArrayNode == null && fni.curLevelList == null)
            {
                throw new MCoreExceptionPopup("fniCurLevelList.curLevelList is null: ({0})", fni.cf.mi.Name);
            }
            int count = 0;
            if (xmlArrayNode != null)
            {
                count = xmlArrayNode.HasChildNodes ? xmlArrayNode.ChildNodes.Count : 0;
            }
            else
            {
                count = fni.curLevelList.Count;
            }
            Type listType = fni.xmlElementType;
            if (listType.Name.Contains("[]"))
            {
                try
                {
                    List<Object> oList = new List<object>();
                    for (int i = 0; i < count; i++)
                    {
                        if (xmlArrayNode != null)
                        {
                            object objArrayItem = CreateArrayItem(fni.cf, xmlArrayNode.ChildNodes[i]);
                            if (!(objArrayItem is string) || (objArrayItem as string) != _CLASS_NOT_FOUND)
                                oList.Add(objArrayItem);
                        }
                        else
                        {
                            oList.Add(fni.curLevelList[i]);
                        }
                    }
                    System.Collections.IList iList = Array.CreateInstance(fni.cf.xmlArrayFixedType, oList.Count) as System.Collections.IList;
                    for (int i = 0; i < oList.Count; i++)
                    {
                        iList[i] = oList[i];
                    }
                    if (objParent != null)
                    {
                        fni.SetValue(objParent, iList);
                    }
                    return iList;
                }
                catch (MCoreExceptionPopup)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Unable create an Array: {0}", listType.FullName);
                }
            }
            else
            {
                try
                {
                    System.Collections.IList iList = Activator.CreateInstance(fni.xmlElementType, new object[] { count }) as System.Collections.IList;
                    for (int i = 0; i < count; i++)
                    {
                        if (xmlArrayNode != null)
                        {
                            object objArrayItem = CreateArrayItem(fni.cf, xmlArrayNode.ChildNodes[i]);
                            if (!(objArrayItem is string) || (objArrayItem as string) != _CLASS_NOT_FOUND)
                                iList.Add(objArrayItem);
                        }
                        else
                        {
                            iList.Add(fni.curLevelList[i]);
                        }
                    }
                    if (objParent != null)
                    {
                        fni.SetValue(objParent, iList);
                    }
                    return iList;
                }
                catch (MCoreException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Unable create an IList array: {0}", listType.FullName);
                }
            }
        }
        private object InstantiateAndPopulateClassRecurse(XmlNode xmlNode, Type tyClass)
        {
            // Node should be Class
            if (!(xmlNode is XmlElement))
                throw new Exception(String.Format("Expected an element ({0})", xmlNode.Name));

            XmlAttribute attrNil = xmlNode.Attributes["xsi:nil"];
            if (attrNil != null && attrNil.Value.ToUpper() == "TRUE")
            {
                return null;
            }

            bool bRootNode = xmlNode.ParentNode is XmlDocument;
            bool bRootArray = false;
            Type fixedArrayType = null;

            string className = tyClass.FullName;
            if (bRootNode)
            {
                bRootArray = tyClass.GetInterface("IList") != null;
                if (bRootArray)
                {
                    fixedArrayType = GetArrayFixedType(tyClass);
                    if (fixedArrayType == null)
                    {
                        return null;
                    }
                    className = fixedArrayType.FullName;
                }
            }

            if (!_classTypeList.ContainsKey(className))
            {
                throw new Exception(String.Format("Could not find class info for '{0}'\n\nClass:(1)", xmlNode.Name, tyClass.Name));
            }
            ClassMapping cm = _classTypeList[className];


            if (bRootArray)
            {
                // Root is Array
                ClassField cf = new ClassField(true, fixedArrayType, null, null, null); 
                FieldNameInfo fni = new FieldNameInfo(tyClass, null, cf);
                return InstantiateArray(fni, null, xmlNode);
            }
            if (xmlNode.ParentNode is XmlDocument && cm.xmlClassName != xmlNode.Name)
            {
                return null;
            }
           

            object objClass = null;
            try
            {
                // Instantiate the class
                objClass = Activator.CreateInstance(tyClass);
            }
            catch(Exception ex)
            {
                throw new MCoreExceptionPopup(ex, "Problem with default constructor for '{0}'", tyClass.FullName);
            }

            // Attributes
            XmlAttributeCollection attrs = xmlNode.Attributes;
            if (attrs != null && attrs.Count > 0)
            {
                for(int i=0; i<attrs.Count; i++)
                {
                    if (cm.xmlAttrNames.ContainsKey(attrs[i].Name))
                    {
                        AttrNameInfo ani = cm.xmlAttrNames[attrs[i].Name];
                        ani.SetXmlValue(objClass, attrs[i].Value);
                    }
                }
            }

            FieldNameInfo fniCurLevelList = null;
            string curLevelElementName = null;

            // Go through all the Node children
            for (int n = 0; n < xmlNode.ChildNodes.Count; n++)
            {
                XmlNode xmlChild = xmlNode.ChildNodes[n];
                // Is name in field list?
                if (cm.xmlElementNames.ContainsKey(xmlChild.Name))
                {
                    FieldNameInfo fni = cm.xmlElementNames[xmlChild.Name];
                    ClassField cf = fni.cf;
                    if (curLevelElementName != null && curLevelElementName != xmlChild.Name)
                    {
                        try
                        {
                            InstantiateArray(fniCurLevelList, objClass, null);
                            curLevelElementName = null;
                            fniCurLevelList = null;
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    if (cf.bArray && curLevelElementName == null && cf.arrayMapping == null)
                    {
                        curLevelElementName = xmlChild.Name;
                        fniCurLevelList = fni;
                        fniCurLevelList.curLevelList = new System.Collections.ArrayList();
                    }
                    if (curLevelElementName == xmlChild.Name)
                    {
                        // Add array Item
                        fniCurLevelList.curLevelList.Add(CreateArrayItem(cf, xmlChild));
                        continue;
                    }

                    if (fni.cf.bArray)
                    {
                        // Has children array items
                        try
                        {
                            InstantiateArray(fni, objClass, xmlChild);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    else if (HasChildValues(fni.xmlElementType))
                    {
                        // Not an array, but is a class or struct with more than one value
                        // (Not a string)
                        try
                        {
                            object obj = InstantiateAndPopulateClassRecurse(xmlChild, fni.xmlElementType);
                            fni.SetValue(objClass, obj);
                        }
                        catch (MCoreException)
                        {
                            throw;
                        }
                        catch
                        {
                            throw new MCoreExceptionPopup("Unknown Deserialize problem with '{0}'", fni.xmlElementType.FullName);
                        }
                    }
                    else
                    {
                        // All others.  Usually Strings, Doubles, etc
                        fni.SetXmlValue(objClass, xmlChild);
                    }
                }
                else
                {
                    // xmlElementName not found
                    // Could be it was deleted in code!
                    U.LogWarning("class= {0}  xmlElementName={1}", tyClass.FullName, xmlChild.Name);
                }
            }
            if (curLevelElementName != null)
            {
                try
                {
                    InstantiateArray(fniCurLevelList, objClass, null);
                }
                catch
                {
                    throw;
                }
            }

            return objClass;
        }

        private object CreateArrayItem(ClassField cf, XmlNode nodeArrayItem)
        {
            ArrayMapping am = cf.arrayMapping;

            Type arrayItemType = cf.xmlArrayFixedType;

            if (am != null)
            {
                if (am.arrayNameList.ContainsKey(nodeArrayItem.Name))
                {
                    Type ty = am.arrayNameList[nodeArrayItem.Name];
                    if (ty != null)
                        arrayItemType = ty;
                }
                else if (arrayItemType != null && arrayItemType.IsClass)
                {
                    // class not found.  insert null for item
                    U.LogPopup("Class not found: {0}", nodeArrayItem.Name);
                    return _CLASS_NOT_FOUND;
                }
            }

            // Look for explicit type in the attribute
            // Check if type is in "xsi:type" attribute
            XmlAttribute xmlAttrType = nodeArrayItem.Attributes["xsi:type"];

            if (xmlAttrType != null && _shortNameTypeList.ContainsKey(xmlAttrType.Value))
            {
                arrayItemType = _shortNameTypeList[xmlAttrType.Value];
            }
            XmlAttribute xmlAttrNil = nodeArrayItem.Attributes["xsi:nil"];
            if (xmlAttrNil != null && xmlAttrNil.Value.ToUpper() == "TRUE")
            {
                return null;
            }

            if (arrayItemType == null)
            {
                throw new MCoreExceptionPopup("Could not determine type for array item: {0}", nodeArrayItem.Value);
            }
            if (arrayItemType.IsClass && arrayItemType != typeof(string))
            {
                return InstantiateAndPopulateClassRecurse(nodeArrayItem, arrayItemType);
            }
            // All others.  Usually Strings, Doubles, etc

            if (nodeArrayItem.HasChildNodes)
            {
                if (nodeArrayItem.FirstChild is XmlText)
                {
                    XmlText xmlText = nodeArrayItem.FirstChild as XmlText;
                    return ParseForTypedValue(xmlText.Value, arrayItemType);
                }
                // Could be text with empty string

            }
            if (arrayItemType == typeof(string))
            {
                return (string)"";
            }
            throw new MCoreExceptionPopup("Could not find the value for array item: {0}", nodeArrayItem.Value);
        }
        /// <summary>
        /// Parses the string to obtain the value according to type
        /// </summary>
        /// <param name="valString"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public static object ParseForTypedValue(string valString, Type ty)
        {
            object valTrueType = null;
            try
            {
                if (valString == null)
                    return null;

                Type tyUnderlying = Nullable.GetUnderlyingType(ty);

                if (tyUnderlying != null)
                {
                    if (string.IsNullOrEmpty(valString))
                    {
                        valTrueType = Activator.CreateInstance(ty, null);
                    }
                    else
                    {
                        object e = Enum.Parse(tyUnderlying, valString);
                        valTrueType = Activator.CreateInstance(ty, e);
                    }
                                        
                }
                else if (ty.IsEnum)
                {
                    valTrueType = Enum.Parse(ty, valString);
                }
                else
                {
                    if (ty == typeof(string))
                    {
                        valTrueType = valString;
                    }
                    else if (ty == typeof(double))
                    {
                        if (valString == "-INF")
                        {
                            valTrueType = Double.NegativeInfinity;
                        }
                        else if (valString == "INF")
                        {
                            valTrueType = Double.PositiveInfinity;
                        }
                        else if (valString == "-0")
                        {
                            valTrueType = 1 / double.NegativeInfinity;
                        }
                        else
                        {
                            valTrueType = Double.Parse(valString);
                        }
                    }
                    else
                    {
                        MethodInfo mi = ty.GetMethod("Parse",
                            BindingFlags.Static | BindingFlags.Public, null,
                            CallingConventions.HasThis, new Type[] { typeof(string) }, null);

                        if (mi != null)
                        {
                            valTrueType = mi.Invoke(null, new object[] { valString });
                        }
                        else
                        {
                            try
                            {
                                valTrueType = Convert.ChangeType(valString, ty);
                            }
                            catch(Exception ex)
                            {
                                ex.ToString();
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return valTrueType;
        }
    }
    /// <summary>
    /// Manages all the member information for the class
    /// </summary>
    class ClassMapping
    {
        /// <summary>
        /// The type for the class
        /// </summary>
        public Type ty;
        public string xmlClassName;
        public string xmlNameSpace;
        /// <summary>
        /// Order in which to serialize.
        /// Created according to finding of fields with reflection
        /// </summary>
        public List<ClassField> fields = new List<ClassField>();
        /// <summary>
        /// Used to find the name for deserialize
        /// These are member names as they are found when reading the xml file
        /// </summary>
        public SortedList<string, FieldNameInfo> xmlElementNames = new SortedList<string, FieldNameInfo>();
        /// <summary>
        /// Used to find the name for deserialize
        /// These are member names as they are found when reading the xml file
        /// </summary>
        public SortedList<string, AttrNameInfo> xmlAttrNames = new SortedList<string, AttrNameInfo>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="xmlClassName"></param>
        /// <param name="xmlNameSpace"></param>
        public ClassMapping(Type ty, string xmlClassName, string xmlNameSpace)
        {
            this.ty = ty;
            this.xmlClassName = xmlClassName;
            this.xmlNameSpace = xmlNameSpace;
        }
        /// <summary>
        /// Add an Attribite to this class
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="xmlElementType"></param>
        /// <param name="cf"></param>
        public void AddAttribute(XmlAttributeAttribute attr, Type xmlElementType, ClassField cf)
        {
            if (attr == null || cf == null)
                return;
            if (!xmlAttrNames.ContainsKey(attr.AttributeName))
            {
                xmlAttrNames.Add(attr.AttributeName, new AttrNameInfo(xmlElementType, attr, cf));
            }
        }
        /// <summary>
        /// Add serializing field to this class
        /// </summary>
        /// <param name="cf"></param>
        public void AddField(ClassField cf)
        {
           
            // Could be the member is overridden         
            if (!fields.Contains(cf))
            {
                // Put in order of serialization
                fields.Insert(0, cf);
            }

            if (cf.HasSingleElementName)
            {
                try
                {
                    // Could be overridden
                    if (!xmlElementNames.ContainsKey(cf.SingleElementName))
                    {
                        FieldNameInfo fni = new FieldNameInfo(cf.SingleElementType, cf.SingleElementName, cf);
                        xmlElementNames.Add(cf.SingleElementName, fni);
                    }
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                // Multiple element names for this field
                // Add them all to the class name list
                // Each should be unique
                foreach (NameToType ntt in cf.nameToTypes)
                {
                    if (!xmlElementNames.ContainsKey(ntt.xmlElementName))
                    {
                        FieldNameInfo fni = new FieldNameInfo(ntt.ty, ntt.xmlElementName, cf);
                        xmlElementNames.Add(ntt.xmlElementName, fni);
                    }
                }
            }
        }
    }
    class NameToType
    {
        public string xmlElementName;
        public Type ty;
        public NameToType(string xmlElementName, Type ty)
        {
            this.xmlElementName = xmlElementName;
            this.ty = ty;
        }
    }
    class ClassField
    {
        public bool bArray = false;
        public Type xmlArrayFixedType = null;
        public MemberInfo mi;
        public ArrayMapping arrayMapping = null;
        public List<NameToType> nameToTypes = null;
        public SortedList<string, string> typeToNames = null;
        //public ClassField(MemberInfo mi)
        //{
        //    this.mi = mi;
        //}
        public ClassField(bool bArray, Type xmlArrayFixedType, MemberInfo mi, ArrayMapping arrayMapping, List<NameToType> nameToTypes)
        {
            this.bArray = bArray;
            this.xmlArrayFixedType = xmlArrayFixedType;
            this.mi = mi;
            this.arrayMapping = arrayMapping;
            this.nameToTypes = nameToTypes;
            if (nameToTypes != null)
            {
                if (nameToTypes.Count < 1)
                {
                    throw new Exception(string.Format("Expected at least one xmlName for {0}", mi.Name));
                }
                else if (nameToTypes.Count > 1)
                {
                    typeToNames = new SortedList<string, string>();
                    foreach (NameToType ntt in nameToTypes)
                    {
                        typeToNames.Add(ntt.ty.FullName, ntt.xmlElementName);
                    }

                }
            }
        }
        public bool HasSingleElementName
        {
            get { return nameToTypes.Count == 1; }
        }
        public string SingleElementName
        {
            get { return nameToTypes[0].xmlElementName; }
        }
        public Type SingleElementType
        {
            get { return nameToTypes[0].ty; }
        }
        public string GetElementName(Type ty)
        {
            if (HasSingleElementName)
            {
                return SingleElementName;
            }

            return typeToNames[ty.FullName];
        }

        public string ConvertObjToXmlString(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is DateTime)
            {
                DateTime dtObj = (DateTime)obj;
                return dtObj.ToString("o");
            }
            if (obj is double)
            {
                double dObj = (double)obj;
                if (double.IsPositiveInfinity(dObj))
                {
                    return "INF";
                }
                if (double.IsNegativeInfinity(dObj))
                {
                    return "-INF";
                }
                // Test for negative zero
                if ((1 / dObj) == double.NegativeInfinity)
                {
                    return "-0";
                }
                string strD = dObj.ToString("R");
                return strD;
            }
            if (obj is bool)
            {
                return ((bool)obj) ? "true" : "false";
            }
            return obj.ToString();
        }

        public string GetStringValue(object objClass)
        {
            object val = GetValue(objClass);
            return ConvertObjToXmlString(val);
        }
        public object GetValue(object objClass)
        {
            try
            {
                if (mi is FieldInfo)
                {
                    return (mi as FieldInfo).GetValue(objClass);
                }
                else if (mi is PropertyInfo)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    object objVal = pi.GetValue(objClass, null);
                    //if (objVal == null && pi.PropertyType == typeof(string))
                    //{
                    //    return "";
                    //}
                    //else
                        return objVal;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        /// <summary>
        /// Set the reference value
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="objRef"></param>
        public void SetValue(object objClass, object objRef)
        {
            try
            {
                if (mi is FieldInfo)
                {
                    (mi as FieldInfo).SetValue(objClass, objRef);
                }
                else if (mi is PropertyInfo)
                {
                    if (objRef != null)
                    {
                        Type ty = objRef.GetType();
                    }
                    (mi as PropertyInfo).SetValue(objClass, objRef, null);
                }
            }
            catch
            {
                throw;
            }
        }
    }
    class NameInfoBase
    {
        public Type xmlElementType;
        public NameInfoBase(Type xmlElementType)
        {
            this.xmlElementType = xmlElementType;
        }

        public object ParseForTypedValue(string valString)
        {
            return MCoreXmlSerializer.ParseForTypedValue(valString, xmlElementType);
        }
    }
    class AttrNameInfo : NameInfoBase
    {
        public XmlAttributeAttribute attribute;
        public ClassField cf;
        public AttrNameInfo(Type xmlElementType, XmlAttributeAttribute attribute, ClassField cf)
            : base(xmlElementType)
        {
            this.attribute = attribute;
            this.cf = cf;
        }
        /// <summary>
        /// Set the reference value
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="objRef"></param>
        public void SetValue(object objClass, object objRef)
        {
            cf.SetValue(objClass, objRef);
        }
        /// <summary>
        /// Set the value found in the XmlText
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="val"></param>
        public void SetXmlValue(object objClass, string val)
        {

            cf.SetValue(objClass, ParseForTypedValue(val));
        }
    }
    class FieldNameInfo : NameInfoBase
    {
        public System.Collections.ArrayList curLevelList = null;
        public string xmlElementName;
        public ClassField cf;
        public bool bEmbeddedClass = false;
        public FieldNameInfo(Type xmlElementType, string xmlElementName, ClassField cf) 
            : base(xmlElementType)
        {
            this.xmlElementName = xmlElementName;
            this.cf = cf;
        }
        /// <summary>
        /// Set the reference value
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="objRef"></param>
        public void SetValue(object objClass, object objRef)
        {
            cf.SetValue(objClass, objRef);
        }
        /// <summary>
        /// Set the value found in the XmlText
        /// </summary>
        /// <param name="objClass"></param>
        /// <param name="xmlNode"></param>
        public void SetXmlValue(object objClass, XmlNode xmlNode)
        {
            // We are expecting node to be XMLText
            object val = null;
            if (xmlNode.HasChildNodes)
            {
                if (xmlNode.FirstChild is XmlText)
                {
                    XmlText xmlText = xmlNode.FirstChild as XmlText;
                    val = ParseForTypedValue(xmlText.Value);
                }
                else
                {
                    U.LogPopup("Expected a Text Value node for ({0})", xmlNode.Name);
                }
            }
            else
            {
                try
                {
                    if (xmlElementType == typeof(string))
                    {
                        val = (string)"";
                    }
                    else
                    {
                        val = Convert.ChangeType(null, xmlElementType);
                    }
                }
                catch
                {
                    throw;
                }
            }

            // Set the value
            cf.SetValue(objClass, val);
        }

    }
    class ArrayMapping
    {
        public SortedList<string, Type> arrayNameList = new SortedList<string, Type>();
        public ArrayMapping()
        {
        }
        public void Add(string name, Type ty)
        {
            if (arrayNameList.ContainsKey(name))
            {
                U.LogPopup("Duplicate name in arrayNameList: Name={0}  Type={1}", name, ty.Name);
            }
            else
            {
                try
                {
                    arrayNameList.Add(name, ty);
                }
                catch(Exception ex)
                {
                    throw new MCoreExceptionPopup(ex, "Trouble with adding {0}", name);
                }
            }
        }
    }
}
