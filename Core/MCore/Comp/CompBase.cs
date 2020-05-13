using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading.Tasks;

using MDouble;

namespace MCore.Comp
{
    /// <summary>
    /// Base class for all
    /// </summary>
    public class CompBase : INotifyPropertyChanged, IDisposable, IConvertible, IComparable<CompBase>
    {
        #region Static constants
        /// <summary>
        /// Delegate that takes a CompBase arg
        /// </summary>
        /// <param name="comp"></param>
        public delegate void NameChangedEventHandler(CompBase comp, string oldID);
        /// <summary>
        /// Component handler
        /// </summary>
        /// <param name="comp"></param>
        public delegate void ComponentEventHandler(CompBase comp);
        /// <summary>
        /// Constant string used to append to class to indicate it is simulated
        /// </summary>
        public const string Sim = "Sim";
        /// <summary>
        /// Enum for simulation
        /// </summary>
        public enum eSimulate
        {
            /// <summary>Do not simulate</summary>
            None,
            /// <summary>Simulate and ask each time we load</summary>
            SimulateAsk,
            /// <summary>Simulate and don't ask each time we load</summary>
            SimulateDontAsk
        };
        #endregion Static constants

        #region Static Variables
        /// <summary>
        /// static reference to the machine object
        /// Will be null until it is created
        /// </summary>
        public static CompMachine s_machine = null;
        /// <summary>
        /// Event for changing of component name
        /// </summary>
        public static event NameChangedEventHandler OnChangedName = null;
        public static event ComponentEventHandler OnRemovingComponent = null;
        public static event ComponentEventHandler OnAddedComponent = null;
        public static event ComponentEventHandler OnSortedComponentChildren = null;
        #endregion Static Variables

        /// <summary>
        /// Event to be fired when disposed
        /// </summary>
        public event ComponentEventHandler OnDisposed = null;
        /// <summary>
        /// Event to be fired when changed
        /// </summary>
        public event ComponentEventHandler OnChanged = null;

        #region Non-Browsable Properties
        /// <summary>
        /// Use only for serialization
        /// </summary>
        [XmlAttribute("Name")]
        [Browsable(false)]
        public string SerName
        {
            get { return GetPropValue(() => Name); }
            set { SetPropValue(() => Name, value); }
        }

        #endregion Non-Browsable Properties

        /// <summary>
        ///  Returns the index suspension number which is at the end of the name string
        /// </summary>
        [Browsable(true)]
        [Category("Identification")]
        public int IndexFromName
        {
            get
            {
                string[] split = Name.Split(' ', '_');
                for( int i=split.Length - 1; i >= 0; i--)
                {
                    if (split[i] != null && char.IsDigit(split[i][0]))
                    {
                        try
                        {
                            return Convert.ToInt32(split[i]) - 1;
                        }
                        catch { }
                    }
                }
                return -1;
            }
        }


        #region Browsable Identification Properties
        /// <summary>
        /// Get the child name of this component.  Will be unique within scope of its siblings.
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Identification")]
        [Description("Unique child name")]
        public virtual string Name
        {
            get
            {
                return GetPropValue(() => Name);
            }
            set
            {
                try
                {
                    string oldID = ID;
                    string newName = ValidateName(Parent, value);
                    bool firstTime = false;
                    PropertyItem<string> propItem = GetPropertyItem(() => Name, out firstTime);
                    if (propItem != null && propItem.IsValueDifferent(newName))
                    {
                        if (IsRooted)
                        {

                            UnregisterRecurse();
                            propItem.Value = newName;
                            RegisterRecurse();
                        }
                        else
                        {
                            propItem.Value = newName;
                        }
                        if (!firstTime)
                        {
                            FirePropertyChangedEvents(() => Name, propItem);
                            FireOnChangedEvent();
                        }
                        if (IsRooted && OnChangedName != null)
                        {
                            OnChangedName(this, oldID);
                        }
                    }
                }
                catch (MCoreException mex)
                {
                    U.Log(mex);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Unable to change the name to '{0}'.", value);
                }
            }
        }
        /// <summary>
        /// Get the shortest unique ID for this Component
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Identification")]
        [Description("Shortest possible unique ID")]
        public string Nickname
        {
            get
            {
                string[] names = U.RootComp.GetUniqueNames(this);
                if (names.Length > 0)
                {
                    // Choose the shortest unique name
                    return names[0];
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Get the full unique ID for this Component
        /// (Machine.ParentName...Name)
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Identification")]
        [Description("Full path name for this component")]
        public string ID
        {
            get
            {
                if (Parent != null)
                {
                    List<string> names = U.RootComp.GetNameList(this);
                    if (names.Count > 0)
                    {
                        // The longest name will be the full ID
                        return names[names.Count - 1];
                    }
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// A list of all the unique names for this component
        /// </summary>
        [XmlIgnore]
        [DisplayName("Unique names")]
        [Browsable(true)]
        [Category("Identification")]
        public string[] UniqueNames
        {
            get
            {
                return U.RootComp.GetUniqueNames(this);
            }
        }
        #endregion Browsable Identification Properties

        #region Browsable Component Tree Properties
        /// <summary>
        /// The next child incrementer
        /// </summary>
        [Browsable(true)]
        [Category("Component Tree")]
        public int NextChildNo
        {
            get;
            set;
        }

        /// <summary>
        /// Parent object
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Tree")]
        public CompBase Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Get the index of the child
        /// </summary>
        public int IndexOf(CompBase child)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren == null || _listChildren.Count == 0 || !_listChildren.Contains(child))
                    return -1;
                return _listChildren.IndexOf(child);
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get the index of the child
        /// </summary>
        public CompBase ChildAt(int index)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren == null || index >= _listChildren.Count)
                    return null;
                return _listChildren[index];
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }


        /// <summary>
        /// Get the prev child
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public CompBase Prev
        {
            get
            {
                CompBase parent = Parent;
                if (parent != null)
                {
                    int index = parent.IndexOf(this);
                    if (index > 0)
                    {
                        return parent.ChildAt(index - 1);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Get the next child
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public CompBase Next
        {
            get
            {
                CompBase parent = Parent;
                if (parent != null)
                {
                    int index = parent.IndexOf(this);
                    if (index < parent.Count-1)
                    {
                        return parent.ChildAt(index + 1);
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Get the first child
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Tree")]
        public CompBase First
        {
            get
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren == null || _listChildren.Count == 0)
                        return null;
                    return _listChildren[0];
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Get the Last child
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Tree")]
        public CompBase Last
        {
            get
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren == null || _listChildren.Count == 0)
                        return null;
                    return _listChildren[_listChildren.Count - 1];
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }
        /// <summary>
        /// Used to add or get a CompnentBase array of the children
        /// Also used for serializing the children of this component
        /// </summary>
        [ReadOnly(true)]
        [Browsable(true)]
        [Category("Component Tree")]
        public CompBase[] ChildArray
        {
            get // Writing, Serialize, or convert to array
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren == null)
                    {
                        return null;
                    }
                    return _listChildren.ToArray();
                }
                catch
                {
                    return null;
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
            set  // Reading, Deserialize, or add array of children
            {
                if (value != null)
                {
                    foreach (CompBase child in value)
                    {
                        Add(child);
                    }
                }
            }
        }
        /// <summary>
        /// Gets the count of children for this object
        /// </summary>
        [Browsable(true)]
        [Category("Component Tree")]
        public virtual int Count
        {
            get
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren == null)
                        return 0;
                    return _listChildren.Count;
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Returns true if this object has children
        /// </summary>
        [Browsable(true)]
        [Category("Component Tree")]
        public bool HasChildren
        {
            [StateMachineEnabled]
            get { return Count != 0; }
        }


        /// <summary>
        /// Returns true if component is rooted
        /// </summary>
        [Browsable(true)]
        [Category("Component Tree")]
        public bool IsRooted
        {
            get
            {
                CompBase comp = this;
                do
                {
                    if (comp is CompRoot)
                        return true;
                    comp = comp.Parent;
                } while (comp != null);
                return false;
            }
        }
        #endregion Browsable Component Tree Properties

        #region Browsable Component Status Properties

        /// <summary>
        /// Flag to indicate if this component is to be simulated
        /// </summary>
        [Browsable(true)]
        [Category("Component Status")]
        public eSimulate Simulate
        {
            get { return GetPropValue(() => Simulate, eSimulate.None); }
            set { SetPropValue(() => Simulate, value); }
        }

        /// <summary>
        /// Type of this class
        /// </summary>
        [Browsable(true)]
        //[ReadOnly(true)]
        [Category("Component Status")]
        [XmlIgnore]
        public string ClassType
        {
            get { return GetType().Name; }
            set
            {
                if (OnChangedName != null)
                {
                    OnChangedName(this, string.Empty);
                }

            }
        }

        /// <summary>
        /// Sets the current status of the machine
        /// </summary>
        [StateMachineEnabled]
        public void MachineStatus(string status)
        {
            CompRoot.AppStatus(status);
        }
        /// <summary>
        /// Flag to indicate if this component is to be simulated
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Status")]
        public bool Initialized
        {
            get { return GetPropValue(() => Initialized); }
            set { SetPropValue(() => Initialized, value); }
        }

        /// <summary>
        /// Flag to indicate if this component has been modified
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Status")]
        public bool Dirty
        {
            get { return _dirty; }
            set
            {
                if (_dirty != value)
                {
                    _dirty = value;
                    if (!_dirty && Count > 0)
                    {
                        // Make all children !modified
                        foreach (CompBase child in ChildArray)
                        {
                            child.Dirty = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Flag to indicate if this component is destroying
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Component Status")]
        public bool IsDestroying
        {
            get { return _isDestroying; }
        }

        /// <summary>
        /// Returns true if this or a parent is set to simulate
        /// </summary>
        [XmlIgnore]
        [Browsable(true)]
        [Category("Component Status")]
        public bool Simulated
        {
            get
            {
                CompBase comp = this;
                do
                {
                    if (comp.Simulate != eSimulate.None)
                    {
                        return true;
                    }
                    comp = comp.Parent;
                } while (comp != null);
                return false;
            }
        }

        /// <summary>
        /// Enabled
        /// </summary>
        [Browsable(true)]
        [Category("Component Status")]
        public virtual bool Enabled
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => Enabled, true); }
            [StateMachineEnabled]
            set { SetPropValue(() => Enabled, value); }
        }
        #endregion Browsable Component Status Properties


        #region Ignore Super Class Pages List
        private List<Type> _ignorePageList = new List<Type>();

        /// <summary>
        /// Ignore Page List
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public List<Type> IgnorePageList
        {
            get { return _ignorePageList; }
            set { _ignorePageList = value; }
        }

        #endregion

        #region Private Variables

        private CompBase _parent = null;
        private volatile bool _dirty = false;
        private volatile bool _isDestroying = false;

        #endregion Private Variables

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CompBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public CompBase(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bRecursivley"></param>
        /// <returns></returns>
        public virtual CompBase Clone(string name, bool bRecursivley)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = Name;
            }
            CompBase comp = Activator.CreateInstance(GetType(), name) as CompBase;
            // Clone attributes of this class
            if (bRecursivley && HasChildren)
            {
                foreach (CompBase child in ChildArray)
                {
                    comp.Add(child.Clone(string.Empty, true));
                }
            }

            return comp;
        }

        #endregion Constructors

        #region Private methods and functions

        public void FireOnChangedEvent()
        {
            CompBase comp = this;
            while (comp != null)
            {
                comp.Dirty = true;
                if (comp.OnChanged != null)
                {
                    comp.OnChanged(comp);
                }
                comp = comp.Parent;
            }
        }

        protected virtual void OnRemoving() { }

        private void OnRemovingRecurse()
        {
            if (!IsRooted)
            {
                return;
            }
            OnRemoving();
            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    _listChildren.ForEach((c) => c.OnRemovingRecurse());
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Fire the component removal event
        /// </summary>
        /// <param name="comp"></param>
        private static void FireRemovingEvent(CompBase comp)
        {
            if (OnRemovingComponent != null)
            {
                OnRemovingComponent(comp);
            }
            comp.OnRemovingRecurse();
        }

        /// <summary>
        /// Fire the component sorted event
        /// </summary>
        /// <param name="comp"></param>
        private static void FireSortedChildrenEvent(CompBase comp)
        {
            if (OnSortedComponentChildren != null)
            {
                OnSortedComponentChildren(comp);
            }
        }
        protected virtual void OnAdded() { }
        private void OnAddedRecurse()
        {
            if (!IsRooted)
            {
                return;
            }
            OnAdded();
            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    _listChildren.ForEach((c) => c.OnAddedRecurse());
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Fire the component adding event
        /// </summary>
        /// <param name="comp"></param>
        private static void FireAddedEvent(CompBase comp)
        {
            if (OnAddedComponent != null)
            {
                OnAddedComponent(comp);
            }
            comp.OnAddedRecurse();
        }

        public string ValidateName(CompBase parent, string newName)
        {
            if (!(string.IsNullOrEmpty(newName)) && newName.IndexOfAny(new char[] { ',', ';', '[', ']', '.', '+', '-' }) >= 0)
            {
                throw new MCoreExceptionPopup("Component name '{0}' should not contain any of the following characters: ',;[].+-", newName);
            }
            if (parent != null)
            {
                if (string.IsNullOrEmpty(newName))
                {
                    // Empty new name means we want to auto-generate one
                    do
                    {
                        newName = string.Format("{0}_{1}", GetType().Name, parent.NextChildNo++);
                    } while (parent.FindChild(newName) != null);
                }

            }
            return newName;
        }

        public void DisposeRecurse()
        {
            Dispose();
            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren != null)
                    {
                        _listChildren.ForEach((c) => c.DisposeRecurse());
                    }
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        private void RegisterRecurse()
        {
            if (!IsRooted)
            {
                return;
            }
            Register();
            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren != null)
                    {
                        _listChildren.ForEach((c) => c.RegisterRecurse());
                    }
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        private void UnregisterRecurse()
        {
            if (!IsRooted)
            {
                return;
            }
            Unregister();
            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    _listChildren.ForEach((c) => c.UnregisterRecurse());
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }

        private List<CompBase> TempRemoveAllChildren()
        {
            __listChildrenLock.EnterWriteLock();
            try
            {
                List<CompBase> upRootedChildren = _listChildren;
                _listChildren = null;
                return upRootedChildren;
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
        }

        private void TempReplaceAllChildren(List<CompBase> children)
        {
            __listChildrenLock.EnterWriteLock();
            try
            {
                _listChildren = children;
                if (_listChildren != null)
                {
                    _listChildren.ForEach(c => c.Parent = this);
                }
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
        }

        #endregion Private methods and functions

        #region protected Methods
        /// <summary>
        /// Unregister this component with the Machine master list 
        /// </summary>
        protected void Unregister()
        {
            U.RootComp.UnregisterComponent(this);
        }
        /// <summary>
        /// Get property name of this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        protected string PropertyName<T>(Expression<Func<T>> property)
        {
            var memberExpression = property.Body as MemberExpression;
            return memberExpression.Member.Name;
        }
        #endregion protected Methods

        #region Public Methods
        /// <summary>
        /// Return the parent that matches the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetParent<T>()
        {
            CompBase comp = this.Parent;
            while (comp != null)
            {
                if (U.IsDerivedFrom(comp.GetType(), typeof(T)))
                {
                    return (T)(object)comp;
                }
                comp = comp.Parent;
            }
            return default(T);
        }

        /// <summary>
        /// Dump the timimg to file
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="prefix"></param>
        /// <param name="prevTicks"></param>
        public virtual void DumpTiming(StreamWriter writer, string prefix, ref long prevTicks)
        {
            TimingElement[] timingOps = ExtractTimingElements();
            if (timingOps != null)
            {
                DateTime now = DateTime.Now;
                DateTime midNight = new DateTime(now.Year, now.Month, now.Day);
                double duration = 0.0;
                foreach (TimingElement timingOp in timingOps)
                {
                    double deltaMS = 0;
                    if (prevTicks != 0)
                    {
                        deltaMS = U.TicksToMS(timingOp.ts - prevTicks);
                    }
                    duration += deltaMS;
                    prevTicks = timingOp.ts;
                    DateTime dateTime = new DateTime(timingOp.ts);
                    double msFromMidnight = U.TicksToMS(timingOp.ts - midNight.Ticks);
                    writer.WriteLine(string.Format("{0},{1},{2},{3},{4}",
                        timingOp.Operations,
                        dateTime.ToString("dd-MM-yy,HH.mm.ss.fff"),
                        msFromMidnight,
                        deltaMS,
                        duration));
                }
            }
        }
        /// <summary>
        /// Get the timing value
        /// </summary>
        public virtual string TimingValue
        {
            get { return Name; }
        }

        private List<TimingElement> _timingOperations = null;
        public bool HasTimingElements
        {
            get
            {
                if (_timingOperations == null)
                {
                    return false;
                }
                lock (_timingOperations)
                {
                    return _timingOperations.Count > 0;
                }
                
            }
        }

        /// <summary>
        /// Add a new timing element
        /// </summary>
        /// <param name="operations"></param>
        public virtual long AddTimingElement(params string[] operations)
        {

            if (_timingOperations == null)
            {
                _timingOperations = new List<TimingElement>();
            }
            string operationPacked = TimingValue;
            foreach(string operation in operations)
            {
                operationPacked += string.Format(",{0}", operation);
            }
            TimingElement timingElement = new TimingElement() { Operations = operationPacked };
            lock (_timingOperations)
            {
                _timingOperations.Add(timingElement);
            }
            long end = DateTime.Now.Ticks;

            return timingElement.ts;
        }

        private TimingElement[] ExtractTimingElements()
        {
            if (_timingOperations == null)
            {
                return null;
            }
            lock (_timingOperations)
            {
                TimingElement[] ret = _timingOperations.ToArray();
                _timingOperations.Clear();
                return ret;
            }
        }
        /// <summary>
        /// Transfer the timing elements from another component
        /// </summary>
        /// <param name="from"></param>
        public void TransferTimingElements(CompBase from)
        {
            if (from.HasTimingElements)
            {
                if (_timingOperations == null)
                {
                    _timingOperations = new List<TimingElement>();
                }
                TimingElement[] elems = from.ExtractTimingElements();
                foreach (TimingElement te in elems)
                {
                    te.ReplaceRoot(from.TimingValue, TimingValue);
                }
                lock (_timingOperations)
                {
                    _timingOperations.AddRange(elems);
                }
            }
        }

        /// <summary>
        /// Register this component with the Machine master list 
        /// </summary>
        public void Register()
        {
            U.RootComp.RegisterComponent(this);
        }

        /// <summary>
        /// Recursive initialization
        /// </summary>
        public virtual void Initialize()
        {
            // Do any initialization for this class
            Initialized = true;
            _dirty = false;
            // Now Recursively initialize the children
            CompBase[] arrComp = ChildArray;
            if (arrComp != null)
            {
                foreach (CompBase comp in arrComp)
                {
                    string plugInClassName = comp.GetType().Name;
                    if (comp.Simulate != eSimulate.SimulateDontAsk && plugInClassName.EndsWith(Sim))
                    {
                        // This is called only if the class is simulated and we want to try to use the real thing
                        plugInClassName = plugInClassName.Substring(0, plugInClassName.Length - 3);
                        Type tyPlugIn = CompRoot.GetTypeFromString(plugInClassName);
                        if (tyPlugIn != null)
                        {
                            // We have the real thing available.
                            // Temp create the real thing and try to initialize the real thing
                            try
                            {
                                CompBase compPlugIn = comp.ShallowClone(tyPlugIn);
                                // If successful, then use it.  Otherwise keep Sim
                                try
                                {
                                    compPlugIn.Parent = comp.Parent;
                                    compPlugIn.Initialize();
                                    ReplaceChild(comp, compPlugIn);
                                    compPlugIn.Simulate = eSimulate.None;
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    compPlugIn.Destroy();
                                    U.LogPopup(ex, "Attempt to use '{0}' plug-in unsuccessful", plugInClassName);
                                    comp.Simulate = eSimulate.SimulateAsk;
                                }
                            }
                            catch (Exception ex)
                            {
                                U.LogPopup(ex, "Unexpected error '{0}' plug-in unsuccessful", tyPlugIn.Name);
                                comp.Simulate = eSimulate.SimulateAsk;
                            }
                        }
                        else
                        {
                            // No real thing to try. But since it was marked as 'SimulateAsk', we report it
                            U.LogPopup("'{0}' plug-in is still unsuccessful", plugInClassName);
                            // Let the children initialize
                            comp.Simulate = eSimulate.SimulateDontAsk;
                            try
                            {
                                comp.Initialize();
                            }
                            catch { }
                            comp.Simulate = eSimulate.SimulateAsk;
                        }
                    }

                    try
                    {
                        comp.Initialize();
                    }
                    catch (Exception ex)  // Also catches ForceSimulateException
                    {
                        if (ex is ForceSimulateException)
                        {
                            Exception exInner = (ex as ForceSimulateException).InnerException;
                            if (exInner != null)
                                ex = exInner;
                        }
                        // If real thing, just convert to Sim
                        Type tyPlugInSim = CompRoot.GetTypeFromString(plugInClassName + Sim);
                        if (tyPlugInSim != null)
                        {
                            try
                            {
                                U.LogPopup(ex, "{0} Initialize failed but it will simulate.", plugInClassName);
                                CompBase newComp = comp.ShallowClone(tyPlugInSim);
                                comp.Destroy();
                                ReplaceChild(comp, newComp);
                                if (comp.Simulate == eSimulate.SimulateDontAsk)
                                {
                                    newComp.Simulate = eSimulate.SimulateDontAsk;
                                }
                                else
                                {
                                    newComp.Simulate = eSimulate.SimulateAsk;
                                }


                            }
                            catch (Exception exc)
                            {
                                U.LogPopup(exc, "{0} Initialize failed and is unable to simulate.", comp.Nickname);
                            }
                        }
                        else
                        {
                            U.LogPopup(ex, "{0} Initialize failed and is unable to simulate.", comp.Nickname);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opportunity to do any ID referencing for this class object
        /// Occurs after Initialize
        /// </summary>
        public virtual void InitializeIDReferences()
        {
            // Do any ID referencing for this class object

            // Recursively call

            if (Count != 0)
            {
                __listChildrenLock.EnterReadLock();
                try
                {
                    if (_listChildren != null)
                    {
                        _listChildren.ForEach((c) => c.InitializeIDReferences());
                    }
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
            CompRoot.AppStatus(string.Format("Init ID Ref {0}", Nickname));
        }

        /// <summary>
        /// Recursive before destroy
        /// </summary>
        public virtual void PreDestroy()
        {
            // Do any Predestroying for this class


            // Recursively perform Destruction
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null)
                {
                    _listChildren.ForEach((c) => c.PreDestroy());
                }
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }


        /// <summary>
        /// Recursive destroy
        /// </summary>
        public virtual void Destroy()
        {
            // Do any Destruction for this class
            _isDestroying = true;

            CompBase[] children = ChildArray;

            if (children != null)
            {
                foreach (CompBase comp in children)
                {
                    comp.Destroy();
                }
            }
            CompRoot.AppStatus(string.Format("Destroying {0}", Name));
        }
        /// <summary>
        /// Shallow clone  (Will not clone children)
        /// </summary>
        /// <param name="newType"></param>
        /// <returns>Returns the copy</returns>
        public CompBase ShallowClone(Type newType)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                List<CompBase> saveChildren = TempRemoveAllChildren();
                XmlSerializer serRead = new XmlSerializer(this.GetType());
                serRead.Serialize(ms, this);
                TempReplaceAllChildren(saveChildren);
                ms.Position = 0;
                XmlAttributeOverrides attr = new XmlAttributeOverrides();
                XmlAttributes attrs = new XmlAttributes();
                attrs.XmlRoot = new XmlRootAttribute(this.GetType().Name);
                attrs.XmlRoot.DataType = newType.Name;
                attr.Add(newType, attrs);
                XmlSerializer serWrite = new XmlSerializer(newType, attr);
                return serWrite.Deserialize(ms) as CompBase;
            }

        }

        /// <summary>
        /// Copy certain non-unique properties
        /// </summary>
        /// <param name="compTo"></param>
        public virtual void ShallowCopyTo(CompBase compTo)
        {
        }

        /// <summary>
        /// Abort any actions pending for this component
        /// </summary>
        [StateMachineEnabled]
        public virtual void Abort()
        {
        }
        
        /// <summary>
        /// Reset this component
        /// </summary>
        [StateMachineEnabled]
        public virtual void Reset()
        {
        }
        /// <summary>
        /// Export to xml
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportToXml(string filePath)
        {
            U.EnsureDirectory(Path.GetDirectoryName(filePath));
            MCoreXmlSerializer.SaveObjectToFile(filePath, this);
        }

        /// <summary>
        /// Replace the child if it exists, otherwise add new one
        /// </summary>
        /// <param name="newComp"></param>
        public void ReplaceChild(CompBase newComp)
        {
            if (newComp != null)
            {
                if (ChildExists(newComp.Name))
                {
                    ReplaceChild(this[newComp.Name], newComp);
                }
                else
                {
                    Add(newComp);
                }
            }
        }

        /// <summary>
        /// Move children from one component to another
        /// </summary>
        /// <param name="sourceParent"></param>
        public void ReplaceChildren(CompBase sourceParent)
        {
            if (sourceParent.IsRooted)
            {
                sourceParent.UnregisterRecurse();
            }
            if (_listChildren != null)
            {
                this.UnregisterRecurse();
                List<CompBase> saveChildren = sourceParent.TempRemoveAllChildren();
                TempReplaceAllChildren(saveChildren);
                this.RegisterRecurse();
            }
        }

        /// <summary>
        /// Replace a child
        /// </summary>
        /// <param name="oldComp"></param>
        /// <param name="newComp"></param>
        public void ReplaceChild(CompBase oldComp, CompBase newComp)
        {
            __listChildrenLock.EnterWriteLock();
            try
            {
                if (_listChildren != null)
                {
                    List<CompBase> saveChildren = oldComp.TempRemoveAllChildren();
                    int index = _listChildren.IndexOf(oldComp);
                    oldComp.Unregister();
                    _listChildren.RemoveAt(index);
                    _listChildren.Insert(index, newComp);
                    newComp.Parent = this;
                    newComp.TempReplaceAllChildren(saveChildren);
                    newComp.Register();
                }
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
            if (IsRooted && OnChangedName != null)
            {
                OnChangedName(newComp, oldComp.ID);
            }

        }

        #endregion Public Methods

        #region Public State Machine Methods
        /// <summary>
        /// Delay for a number of millisecond
        /// </summary>
        /// <param name="msecTimeout"></param>
        [StateMachineEnabled]
        public void Delay(int msecTimeout)
        {
            U.Delay(msecTimeout);
        }


        /// <summary>
        /// Delay With Thread Sleep
        /// </summary>
        /// <param name="msecTimeout"></param>
        [StateMachineEnabled]
        public void SleepDelay(int msecTimeout)
        {
            //var waitDelay = Task.Delay(msecTimeout);
            //waitDelay.Wait();
            System.Threading.Thread.Sleep(msecTimeout);
        }

        /// <summary>
        /// Popup a messagebox
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        [StateMachineEnabled]
        public void PopupMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }
        #endregion Public State Machine Methods


        #region Children related members
        private List<CompBase> _listChildren = null;

        private ReaderWriterLockSlim __listChildrenLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Perform action on each child
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<CompBase> action)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null && _listChildren.Count > 0)
                {
                    _listChildren.ForEach(action);
                }
            }
            catch
            {
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }

        }
        /// <summary>
        /// Get a single match for a type after searching recursively
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>The single instance, otherwise returns null</returns>
        public T RecursiveFilterByTypeSingle<T>()
        {
            T[] result = RecursiveFilterByType<T>();
            if (result == null || result.Length != 1)
            {
                return default(T);
            }
            return result[0];
        }
        /// <summary>
        /// Get an array of recursive children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T[] RecursiveFilterByType<T>()
        {
            List<T> resList = new List<T>(Filter<T>(c => c is T));
            if (HasChildren)
            {
                foreach (CompBase child in ChildArray)
                {
                    resList.AddRange(child.RecursiveFilterByType<T>());
                }
            }
            return resList.ToArray<T>();
        }

        /// <summary>
        /// Get an array of recursive children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Array RecursiveFilterByType(Type ty)
        {
            Array arrResult = FilterByType(ty);
            if (HasChildren)
            {
                foreach (CompBase child in ChildArray)
                {
                    Array arrChildren = child.RecursiveFilterByType(ty);
                    if (arrChildren != null && arrChildren.Length > 0)
                    {
                        Array arrNew = Array.CreateInstance(ty, arrChildren.Length + arrResult.Length);
                        Array.Copy(arrResult, 0, arrNew, 0, arrResult.Length);
                        Array.Copy(arrChildren, 0, arrNew, arrResult.Length, arrChildren.Length);
                        arrResult = arrNew;
                    }
                }
            }
            return arrResult;
        }

        /// <summary>
        /// Get an array of recursive children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Array RecursiveFilterByExactType(Type ty)
        {
            Array arrResult = FilterByExactType(ty);
            if (HasChildren)
            {
                foreach (CompBase child in ChildArray)
                {
                    Array arrChildren = child.RecursiveFilterByExactType(ty);
                    if (arrChildren != null && arrChildren.Length > 0)
                    {
                        Array arrNew = Array.CreateInstance(ty, arrChildren.Length + arrResult.Length);
                        Array.Copy(arrResult, 0, arrNew, 0, arrResult.Length);
                        Array.Copy(arrChildren, 0, arrNew, arrResult.Length, arrChildren.Length);
                        arrResult = arrNew;
                    }
                }
            }
            return arrResult;
        }

        /// <summary>
        /// Get an array of children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Array FilterByType(Type ty)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null && _listChildren.Count > 0)
                {
                    List<CompBase> list = new List<CompBase>();
                    foreach (CompBase comp in _listChildren)
                    {
                        if (comp.GetType().IsSubclassOf(ty) || comp.GetType().Equals(ty))
                        {
                            list.Add(comp);
                        }
                    }
                    Array arr = Array.CreateInstance(ty, list.Count);
                    Array.Copy(list.ToArray(), arr, list.Count);
                    return arr;
                    //return _listChildren.Where(c => c.GetType().IsSubclassOf(ty)).ToArray();
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get an Exact match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FilterByExactTypeSingle<T>()
        {
            Array arr = FilterByExactType(typeof(T));
            if (arr != null && arr.Length > 0)
            {
                return (T)arr.GetValue(arr.Length-1);
            }
            return default(T);
        }
        /// <summary>
        /// Get an array of children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Array FilterByExactType(Type ty)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null && _listChildren.Count > 0)
                {
                    List<CompBase> list = new List<CompBase>();
                    foreach (CompBase comp in _listChildren)
                    {
                        if (comp.GetType().Equals(ty))
                        {
                            list.Add(comp);
                        }
                    }
                    Array arr = Array.CreateInstance(ty, list.Count);
                    Array.Copy(list.ToArray(), arr, list.Count);
                    return arr;
                    //return _listChildren.Where(c => c.GetType().IsSubclassOf(ty)).ToArray();
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get an array of children that match the type
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T[] FilterByType<T>()
        {
            return Filter<T>(c => c is T);
        }
        /// <summary>
        /// Get an array of children that match the filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T[] Filter<T>(Func<CompBase, bool> predicate)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null && _listChildren.Count > 0)
                {
                    return _listChildren.Where(predicate).Cast<T>().ToArray();
                }
                return new T[0];
            }
            catch
            {
                return new T[0];
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get single child that match the type
        /// If two exists, throws an exception
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FilterByTypeSingle<T>()
        {
            return FilterSingle<T>(c => c is T);
        }
        /// <summary>
        /// Get single child that match the filter
        /// If two exist, returns null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FilterSingle<T>(Func<CompBase, bool> predicate)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                if (_listChildren != null && _listChildren.Count > 0)
                {
                    return _listChildren.Where(predicate).Cast<T>().SingleOrDefault();
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        ///// <summary>
        ///// Component factory
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="tGeneric"></param>
        ///// <param name="pluginType"></param>
        ///// <returns></returns>
        //public CompBase CreateComponent(string name, Type tGeneric, string pluginType)
        //{
        //    return CreateComponent(name, tGeneric, pluginType, null);
        //}

        ///// <summary>
        ///// Component factory
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="tGeneric"></param>
        ///// <param name="pluginType"></param>
        ///// <param name="firstTimeInit"></param>
        ///// <returns></returns>
        //public CompBase CreateComponent(string name, Type tGeneric, string pluginType, Action<CompBase> firstTimeInit)
        //{
        //    Type t = CompMachine.GetTypeFromString(pluginType);
        //    if (t == null)
        //    {                
        //        t = tGeneric;
        //    }

        //    CompBase foundChild = FindChild(name);
        //    if (foundChild != null)
        //    {
        //        // Already exists
        //        if (foundChild.GetType().Equals(t))
        //        {
        //            // (Generic and Generic) or (plug-in and plug-in)
        //            // If generic-generic, we can do nothing because it means there is no plugin
        //            // If plugin-plugin there is nothing to do
        //            return foundChild;
        //        }
        //        else
        //        {
        //            // Generic-Plugin combo
        //            // If foundChild is plugin, we should check if we don't ask 
        //            if (t.Equals(tGeneric) && foundChild.Simulate != eSimulate.SimulateDontAsk)
        //            {
        //                // No plug in, but we 
        //                return foundChild;
        //            }
        //            CompBase newComp = foundChild.ConvertClass(t);
        //            // Replace it
        //            Remove(foundChild);
        //            Add(newComp);
        //            return newComp;
        //        }
        //    }

        //    CompBase comp = Activator.CreateInstance(t, new object[] { name }) as CompBase;
        //    if (firstTimeInit != null)
        //    {
        //        firstTimeInit(comp);
        //    }
        //    return AddIfNotExists(comp);

        //}

        /// <summary>
        /// Find a matching child
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public CompBase FindChild(Func<CompBase, bool> predicate)
        {
            __listChildrenLock.EnterReadLock();
            try
            {
                IEnumerable<CompBase> items = _listChildren.Where(predicate);
                if (items.Any())
                {
                    return items.ElementAt(0);
                }
            }
            catch { }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
            return null;
        }

        /// <summary>
        /// Locate the child with matching childName
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        public CompBase FindChild(string childName)
        {
            if (string.IsNullOrEmpty(childName))
            {
                return null;
            }
            __listChildrenLock.EnterWriteLock();
            try
            {
                var items = _listChildren.Where(c => c.Name == childName);
                if (items.Count() == 1)
                    return items.ElementAt(0);
            }
            catch
            {
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
            return null;
        }

        /// <summary>
        /// Remove from its parent list
        /// </summary>
        public virtual void Delete()
        {
            if (Parent != null)
            {
                Parent.Remove(this);
            }
        }

        public void RemoveSelf()
        {
            if (Parent != null)
            {
                Parent.Remove(this);
            }
        }

        /// <summary>
        /// Remove a child from the children list
        /// </summary>
        /// <param name="child"></param>
        /// <returns>Returns the removed, unregistered child</returns>
        public virtual CompBase Remove(CompBase child)
        {
            FireRemovingEvent(child);
            __listChildrenLock.EnterWriteLock();
            try
            {
                if (_listChildren == null || !_listChildren.Contains(child))
                {
                    throw new Exception("Child not found");
                }

                // Recursively unregister if rooted
                child.UnregisterRecurse();
                _listChildren.Remove(child);
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
            FireOnChangedEvent();
            return child;
        }
        /// <summary>
        /// Removes the first child from the children list
        /// </summary>
        /// <returns>Returns the removed, unregistered child</returns>
        public virtual CompBase RemoveFirst()
        {
            return Remove(First);
        }

        /// <summary>
        /// Removes the last child from the children list
        /// </summary>
        public virtual CompBase RemoveLast()
        {
            return Remove(Last);
        }

        /// <summary>
        /// Append a new child right after the anchor child
        /// </summary>
        /// <param name="anchorChild"></param>
        /// <param name="newChild"></param>
        public virtual void AppendAt(CompBase anchorChild, CompBase newChild)
        {
            try
            {
                __listChildrenLock.EnterWriteLock();
                try
                {
                    if (_listChildren == null)
                    {
                        _listChildren = new List<CompBase>();
                    }
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }
                // Make sure it is not rooted
                newChild.Parent = null;
                // Make sure we have a valid name
                string newName = newChild.ValidateName(this, newChild.Name);
                // Set the new name.  But will not register because it is still not rooted
                newChild.Name = newName;
                __listChildrenLock.EnterWriteLock();
                try
                {
                    // Now attach to the parent
                    newChild.Parent = this;
                    if (_listChildren.Count == 0)
                    {
                        _listChildren.Add(newChild);
                    }
                    else if (anchorChild == null)
                    {
                        _listChildren.Insert(0, newChild);
                    }
                    else
                    {
                        int index = _listChildren.IndexOf(anchorChild);
                        if (index < 0 || index + 1 >= _listChildren.Count)
                        {
                            _listChildren.Add(newChild);
                        }
                        else
                        {
                            _listChildren.Insert(index + 1, newChild);
                        }
                    }
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }

                // Recursively register if all parents are rooted
                newChild.RegisterRecurse();
                FireAddedEvent(newChild);
                FireOnChangedEvent();

            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to append '{0}'", newChild.Name);
            }
        }

        /// <summary>
        /// Add a child to the list dynamically 
        /// </summary>
        /// <param name="child"></param>
        public virtual void Add(CompBase newChild)
        {
            try
            {
                __listChildrenLock.EnterWriteLock();
                try
                {
                    if (_listChildren == null)
                    {
                        _listChildren = new List<CompBase>();
                    }
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }
                // Make sure it is not rooted
                newChild.Parent = null;
                // Make sure we have a valid name
                string newName = newChild.ValidateName(this, newChild.Name);
                // Set the new name.  But will not register because it is still not rooted
                newChild.Name = newName;

                //Check Duplicate Child Name
                if(_listChildren.Any(child => child.Name == newName))
                {
                    return;
                }

                __listChildrenLock.EnterWriteLock();
                try
                {
                    // Now attach to the parent
                    newChild.Parent = this;
                    _listChildren.Add(newChild);
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }

                // Recursively register if all parents are rooted
                newChild.RegisterRecurse();
                FireAddedEvent(newChild);
                FireOnChangedEvent();

            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to add '{0}'", newChild.Name);
            }
        }



        /// <summary>
        /// Insert a child to the list dynamically 
        /// </summary>
        /// <param name="child"></param>
        public virtual void Insert(CompBase newChild,int index)
        {
            try
            {
                __listChildrenLock.EnterWriteLock();
                try
                {
                    if (_listChildren == null)
                    {
                        _listChildren = new List<CompBase>();
                    }
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }
                // Make sure it is not rooted
                newChild.Parent = null;
                // Make sure we have a valid name
                string newName = newChild.ValidateName(this, newChild.Name);
                // Set the new name.  But will not register because it is still not rooted
                newChild.Name = newName;
                __listChildrenLock.EnterWriteLock();
                try
                {
                    // Now attach to the parent
                    newChild.Parent = this;
                    _listChildren.Insert(index, newChild);
                }
                finally
                {
                    __listChildrenLock.ExitWriteLock();
                }

                // Recursively register if all parents are rooted
                newChild.RegisterRecurse();
                FireAddedEvent(newChild);
                FireOnChangedEvent();

            }
            catch (MCoreException mex)
            {
                U.Log(mex);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to add '{0}'", newChild.Name);
            }
        }

        /// <summary>
        /// Order the children according to the key selector
        /// </summary>
        [StateMachineEnabled]
        public void SortChildren()
        {
            __listChildrenLock.EnterWriteLock();
            try
            {
                _listChildren.Sort();
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error in ordering children for {0}", ID);
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
            FireSortedChildrenEvent(this);
            FireOnChangedEvent();
        }

        public void SortChildren(Comparison<CompBase> comparison)
        {
            __listChildrenLock.EnterWriteLock();
            try
            {
                _listChildren.Sort(comparison);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error in ordering children for {0}", ID);
            }
            finally
            {
                __listChildrenLock.ExitWriteLock();
            }
            FireSortedChildrenEvent(this);
            FireOnChangedEvent();
        }


        /// <summary>
        /// Get child based on index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [XmlIgnore]
        public CompBase this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    return null;
                __listChildrenLock.EnterReadLock();
                try
                {
                    return _listChildren[index];
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
            set
            {
                if (index >= 0 && index < Count)
                {
                    // Replace exisiting with another
                    __listChildrenLock.EnterWriteLock();
                    try
                    {
                        _listChildren[index].UnregisterRecurse();
                        _listChildren[index] = value;
                        _listChildren[index].RegisterRecurse();
                    }
                    finally
                    {
                        __listChildrenLock.ExitWriteLock();
                    }
                }
            }
        }
        /// <summary>
        /// Get child based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CompBase this[string name]
        {
            get
            {
                if (Count == 0)
                    return null;
                __listChildrenLock.EnterReadLock();
                CompBase item = null;
                try
                {
                    item = _listChildren.Where(c => c.Name == name).Single();
                    return item;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    __listChildrenLock.ExitReadLock();
                }
            }
        }


        public bool ChildExists(string name)
        {
            if (Count == 0 || string.IsNullOrEmpty(name))
                return false;
            __listChildrenLock.EnterReadLock();
            CompBase item = null;
            try
            {
                item = _listChildren.Where(c => c.Name == name).Single();
                return item != null;
            }
            catch
            {
                return false;
            }
            finally
            {
                __listChildrenLock.ExitReadLock();
            }
        }

        #endregion Children related members

        #region IConvertible
        public TypeCode GetTypeCode() { return TypeCode.Object; }
        public bool ToBoolean(IFormatProvider provider) { return false; }
        public byte ToByte(IFormatProvider provider) { return 0; }
        public char ToChar(IFormatProvider provider) { return '\0'; }
        public DateTime ToDateTime(IFormatProvider provider) { return DateTime.Now; }
        public decimal ToDecimal(IFormatProvider provider) { return 0; }
        public double ToDouble(IFormatProvider provider) { return 0; }
        public short ToInt16(IFormatProvider provider) { return 0; }
        public int ToInt32(IFormatProvider provider) { return 0; }
        public long ToInt64(IFormatProvider provider) { return 0; }
        public sbyte ToSByte(IFormatProvider provider) { return 0; }
        public float ToSingle(IFormatProvider provider) { return 0; }
        public string ToString(IFormatProvider provider) { return Name; }
        public ushort ToUInt16(IFormatProvider provider) { return 0; }
        public uint ToUInt32(IFormatProvider provider) { return 0; }
        public ulong ToUInt64(IFormatProvider provider) { return 0; }
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            if (U.IsDerivedFrom(this.GetType(), conversionType))
            {
                return this;
            }
            return null;
        }
        #endregion IConvertible


        #region IDisposable
        /// <summary>
        /// Dispose method.
        /// </summary>
        public virtual void Dispose()
        {
            if (OnDisposed != null)
            {
                OnDisposed(this);
            }
            FireOnChangedEvent();
            GC.SuppressFinalize(this);
        }


        #endregion IDisposable

        #region NotifyPropertyChanged
        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Helper function to convert from property to its name
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="property"></param>
        //public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        //{
        //    var memberExpression = property.Body as MemberExpression;
        //    string propName = memberExpression.Member.Name;
        //    if (PropertyChanged != null)
        //    {
        //        RaisePropertyChanged(this, new PropertyChangedEventArgs(propName));
        //    }
        //    __dictPropNotifiesLock.EnterReadLock();
        //    try
        //    {
        //        if (_dictPropNotifies != null && _dictPropNotifies.ContainsKey(propName))
        //        {
        //            if (_dictPropNotifies[propName].Fire() && propName != "Dirty")
        //            {
        //                FireOnChangedEvent();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        __dictPropNotifiesLock.ExitReadLock();
        //    }
        //}

        /// <summary>
        /// Helper function to convert from property to its name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        public T GetPropValue<T>(Expression<Func<T>> property)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            if (propItem == null)
            {
                T val = (T)U.GetDefault(typeof(T));
                if (val == null)
                    val = default(T);
                return val;
            }
            if (propItem.Value == null)
            {
                T val = (T)U.GetDefault(typeof(T));
                if (val == null)
                    val = default(T);
                propItem.Value = val;
            }
            return propItem.Value;
        }

        /// <summary>
        /// Helper function to convert from property to its name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="defaultVal"></param>
        public T GetPropValue<T>(Expression<Func<T>> property, T defaultVal)
        {
            string propName = PropertyName(property);

            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            if (firstTime)
            {
                propItem.Value = defaultVal;
            }
            if (propItem == null)
                return defaultVal;
            return propItem.Value;
        }

        private PropertyItem<T> GetPropertyItem<T>(Expression<Func<T>> property, out bool firstTime)
        {
            firstTime = false;
            PropertyItem<T> propItem = null;
            string propName = PropertyName(property);
            __dictPropNotifiesLock.EnterReadLock();
            try
            {
                if (_dictProperties != null && _dictProperties.ContainsKey(propName))
                {
                    propItem = (PropertyItem<T>)_dictProperties[propName];
                    return propItem;
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem reading the property item '{0}'", propName);
                return null;
            }
            finally
            {
                __dictPropNotifiesLock.ExitReadLock();
            }
            firstTime = true;
            __dictPropNotifiesLock.EnterWriteLock();
            try
            {
                if (_dictProperties == null)
                {
                    _dictProperties = new Dictionary<string, PropertyItemBase>();
                }
                    // Last chance to find the item
                else if (_dictProperties.ContainsKey(propName))
                {
                    propItem = (PropertyItem<T>)_dictProperties[propName];
                    return propItem;
                }

                propItem = new PropertyItem<T>(propName);
                _dictProperties.Add(propName, propItem);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem creating the property item '{0}'", propName);
            }
            finally
            {
                __dictPropNotifiesLock.ExitWriteLock();
            }
            return propItem;
        }

        /// <summary>
        /// Helper function to Set the property value without changing dirty flag
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="newVal"></param>
        public bool SetPropValueNoDirty<T>(Expression<Func<T>> property, T newVal)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            if (propItem == null)
                return false;
            if (propItem.IsValueDifferent(newVal))
            {
                propItem.Value = newVal;
                if (!firstTime)
                {
                    FirePropertyChangedEvents(property, propItem);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Helper function to Set the property value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="newVal"></param>
        public bool SetPropValue<T>(Expression<Func<T>> property, T newVal)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            if (propItem == null)
                return false;
            if (propItem.IsValueDifferent(newVal))
            {
                propItem.Value = newVal;
                if (!firstTime)
                {
                    FirePropertyChangedEvents(property, propItem);
                    FireOnChangedEvent();
                    return true;
                }
            }
            return false;
        }

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            if (propItem != null)
            {
                FirePropertyChangedEvents(property, propItem);
                FireOnChangedEvent();
            }
        }

        private void FirePropertyChangedEvents<T>(Expression<Func<T>> property, PropertyItem<T> propItem)
        {
            if (PropertyChanged != null)
            {
                string propName = PropertyName(property);
                RaisePropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
            propItem.Fire();
        }

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            Control c = U.GetDummyControl();
            if (c == null)
            {
                PropertyChanged(sender, e);
            }
            else
            {
                if (c.InvokeRequired)
                {
                    c.BeginInvoke(new PropertyChangedEventHandler(RaisePropertyChanged), new object[] { sender, e });
                    return;
                }
                else
                {
                    PropertyChanged(sender, e);
                }
            }
        }
        #endregion

        #region Specific Property Notification

        public abstract class NotifyItemBase<T>
        {
            public abstract void Fire(T value);
        }

        public class NotifyItem<T> : NotifyItemBase<T>
        {
            /// <summary>
            /// Delegate for the callback
            /// </summary>
            /// <param name="newVal"></param>
            public delegate void DelPropNotify(T newVal);
            private DelPropNotify _del = null;
            public NotifyItem(DelPropNotify del)
            {
                _del = del;
            }
            private delegate void FireToControlDel(T value);
            public override void Fire(T value)
            {
                if (_del.Target is Control)
                {
                    Control c = _del.Target as Control;
                    if (c.InvokeRequired)
                    {
                        c.BeginInvoke(new FireToControlDel(Fire), new object[] { value });
                        return;
                    }
                }
                try
                {
                    _del(value);
                }
                catch (Exception ex)
                {
                    if (_del.Target is Control)
                    {
                        U.LogPopup(ex, "Trouble with FireToControl for control '{0}'", (_del.Target as Control).Name);
                    }
                    else
                    {
                        U.LogPopup(ex, "Trouble with FireDelegate of delegate {0}", _del.ToString());
                    }

                }

            }
        }

        public class NotifyItemSender<S, T> : NotifyItemBase<T>
        {
            /// <summary>
            /// Delegate for the callback
            /// </summary>
            /// <param name="sender"></param>            
            /// <param name="newVal"></param>            
            public delegate void DelPropSenderNotify(S sender, T newVal);
            private DelPropSenderNotify _del = null;
            private S _sender = default(S);
            public NotifyItemSender(S sender, DelPropSenderNotify del)
            {
                _sender = sender;
                _del = del;
            }
            private delegate void FireToControlDel(T value);
            public override void Fire(T value)
            {
                if (_del.Target is Control)
                {
                    Control c = _del.Target as Control;
                    if (c.InvokeRequired)
                    {
                        c.BeginInvoke(new FireToControlDel(Fire), new object[] { value });
                        return;
                    }
                }
                try
                {
                    _del(_sender, value);
                }
                catch (Exception ex)
                {
                    if (_del.Target is Control)
                    {
                        U.LogPopup(ex, "Trouble with FireToControl of '{0}' for control '{1}'", _sender.ToString(), (_del.Target as Control).Name);
                    }
                    else
                    {
                        U.LogPopup(ex, "Trouble with FireDelegate of '{0}' delegate {1}", _sender.ToString(), _del.ToString());
                    }
                }
            }
        }

        public class PropertyItemBase
        {
            protected string _propName = string.Empty;

            public PropertyItemBase(string name)
            {
                _propName = name;
            }

        }

        /// <summary>
        /// Base class for Property notifier
        /// </summary>
        public class PropertyItem<T> : PropertyItemBase
        {
            /// <summary>
            /// Delegate for the callback
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="newVal"></param>
            public delegate void DelPropSenderNotify<S>(S sender, T newVal);

            private T _value = default(T);
            private ReaderWriterLockSlim __lockValue = new ReaderWriterLockSlim();
            private Dictionary<Delegate, NotifyItemBase<T>> _delList = new Dictionary<Delegate, NotifyItemBase<T>>();

            public PropertyItem(string name)
                : base(name)
            {
            }

            /// <summary>
            public bool IsValueDifferent(T newVal)
            {

                bool bChanged = false;
                __lockValue.EnterReadLock();
                try
                {
                    bChanged = !object.Equals(newVal, _value);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Trouble with Equal comparison for {0} type", typeof(T).Name);
                }
                finally
                {
                    __lockValue.ExitReadLock();
                }
                return bChanged;
            }
            public T Value
            {
                get
                {
                    __lockValue.EnterReadLock();
                    try
                    {
                        return _value; // (V)Convert.ChangeType(_curValue, typeof(V));
                    }
                    finally
                    {
                        __lockValue.ExitReadLock();
                    }
                }
                set
                {
                    __lockValue.EnterWriteLock();
                    try
                    {
                        _value = value; // (T)Convert.ChangeType(newVal, typeof(T));
                    }
                    catch (Exception ex)
                    {
                        U.LogPopup(ex, "Trouble with value assignment for '{0}' of type {1}", _propName, typeof(T).Name);
                    }
                    finally
                    {
                        __lockValue.ExitWriteLock();
                    }
                }
            }
            /// <summary>
            /// Add a callback to this property
            /// </summary>
            /// <param name="del"></param>
            public void Add(NotifyItem<T>.DelPropNotify del)
            {
                try
                {
                    _delList.Add(del, new NotifyItem<T>(del));
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Could not add a callback ({0}) to '{1}'", del.ToString(), _propName);
                }
            }
            /// <summary>
            /// Add a callback to this property
            /// </summary>
            /// <param name="del"></param>
            public void Add<S>(S sender, NotifyItemSender<S, T>.DelPropSenderNotify del)
            {
                _delList.Add(del, new NotifyItemSender<S, T>(sender, del));
            }
            /// <summary>
            /// Add a callback to this property
            /// </summary>
            /// <param name="del"></param>
            public void Remove(NotifyItem<T>.DelPropNotify del)
            {
                _delList.Remove(del);
            }
            /// <summary>
            /// Add a callback to this property
            /// </summary>
            /// <param name="del"></param>
            public void Remove<S>(NotifyItemSender<S, T>.DelPropSenderNotify del)
            {
                _delList.Remove(del);
            }
            /// Fire any delegates that this value has chenaged
            /// </summary>
            public void Fire()
            {
                T value = Value;

                foreach (NotifyItemBase<T> notifyItem in _delList.Values)
                {
                    notifyItem.Fire(value);
                }
            }
        }

        //public class PropNotifySenderItem<S,T> : PropNotifyItem<T>
        //{
        //    private S _sender = default(S);
        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    public PropNotifySenderItem(S sender)
        //    {
        //        _sender = sender;
        //    }
        //    protected override void FireToNonControl<NVT>(Delegate del, NVT newVal)
        //    {
        //        (del as DelPropSenderNotify<S,NVT>).BeginInvoke(_sender, newVal, null, null);                
        //    }

        //}
        ///// <summary>
        ///// Class for each property
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //public class PropNotifyItem<T>
        //{
        //    #region Public items
        //    #endregion Public items

        //    #region Privates
        //    private delegate void FireToControlDel(Delegate del, T newVal);


        //    #endregion Privates

        //    /// <summary>
        //    /// Constructor
        //    /// </summary>
        //    public PropNotifyItem()
        //    {
        //    }
        //    protected virtual void FireToNonControl<NVT>(Delegate del, NVT newVal)
        //    {
        //        (del as DelPropNotify<NVT>).BeginInvoke(newVal, null, null);
        //    }

        //    protected virtual void FireToControl<NVT>(Delegate del, NVT newVal)
        //    {
        //        Control c = del.Target as Control;
        //        if (c.InvokeRequired)
        //        {
        //            c.BeginInvoke(new FireToControlDel(FireToControl), new object[] {del, newVal} );
        //            return;
        //        }
        //        (del as DelPropNotify<NVT>)(newVal);
        //    }
        //}
        private Dictionary<string, PropertyItemBase> _dictProperties = null;
        protected ReaderWriterLockSlim __dictPropNotifiesLock = new ReaderWriterLockSlim();
        /// <summary>
        /// Register a property to notify to a specific callback.
        /// Only fires if the prperty has changed value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public void RegisterOnChanged<T>(Expression<Func<T>> property, NotifyItem<T>.DelPropNotify del)
        {
            // Call the get property to be sure it has been initialized properly
            property.Compile().Invoke();
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            propItem.Add(del);
        }

        /// <summary>
        /// Register a property to notify to a specific callback.
        /// Only fires if the property has changed value
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public void RegisterOnChanged<S, T>(Expression<Func<T>> property, NotifyItemSender<S, T>.DelPropSenderNotify del)
        {
            // Call the get property to be sure it has been initialized properly
            property.Compile().Invoke();
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            propItem.Add<S>((S)Convert.ChangeType(this, typeof(S)), del);
        }

        /// <summary>
        /// UnRegister a previously registered notification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public void UnRegisterOnChanged<T>(Expression<Func<T>> property, NotifyItem<T>.DelPropNotify del)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            propItem.Remove(del);
        }

        /// <summary>
        /// UnRegister a previously registered notification
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public void UnRegisterOnChanged<S, T>(Expression<Func<T>> property, NotifyItemSender<S, T>.DelPropSenderNotify del)
        {
            bool firstTime = false;
            PropertyItem<T> propItem = GetPropertyItem<T>(property, out firstTime);
            propItem.Remove(del);
        }

        /// <summary>
        /// Delegate to store change method
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DelegateOnChanged(Object newVal);
        #endregion Another Notification option

        #region Overrides
        /// <summary>
        /// Get the default string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Nickname))
                return Name;
            return Nickname;
        }

        /// <summary>
        /// Return if this is equal to another 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is CompBase))
                return false;
            return string.Equals(Name, (obj as CompBase).Name);
        }

        /// <summary>
        /// Retrun the hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion Overrides

        /// <summary>
        /// Log all the serialize properties
        /// </summary>
        public void Log()
        {
            Log(GetType().Name + ".csv");
        }

        /// <summary>
        /// Log all the serialize properties
        /// </summary>
        public void Log(string filename)
        {
            Type[] classes = U.GetClassHierarchy(GetType(), typeof(CompBase));
            string strLog = string.Empty;
            string strHeader = string.Empty;

            foreach (Type ty in classes)
            {
                try
                {
                    PropertyInfo[] props = ty.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    foreach (PropertyInfo pi in props)
                    {
                        try
                        {
                            if (!pi.CanRead || !pi.CanWrite)
                            {
                                continue;
                            }
                            object[] ca = pi.GetCustomAttributes(false);
                            if (ca.Count((c) => c.GetType().Equals(typeof(XmlIgnoreAttribute))) != 0)
                            {
                                continue;
                            }
                            if (!string.IsNullOrEmpty(strLog))
                            {
                                strLog += ",";
                                strHeader += ",";
                            }
                            object oVal = pi.GetValue(this, null);

                            if (oVal.GetType().Name.Contains('['))
                            {
                                continue;
                            }

                            if (oVal is MDoubleBase)
                            {
                                strLog += (oVal as MDoubleBase).Val.ToString("##0.###");
                                strHeader += string.Format("{0} ({1})", pi.Name, (oVal as MDoubleBase).UnitText);
                            }
                            else
                            {
                                if (oVal == null)
                                {
                                    strLog += "Null";
                                    strHeader += pi.Name;
                                }
                                else
                                {
                                    string valString = oVal.ToString();
                                    if (valString.StartsWith("{") && valString.EndsWith("}") && valString.Length > 2)
                                    {
                                        // Extract info
                                        string[] items = valString.Substring(1, valString.Length - 2).Split(',');
                                        for (int i = 0; i < items.Length; i++)
                                        {
                                            string item = items[i];
                                            string[] elems = item.Split('=', ' ');
                                            string val = string.Empty;
                                            string units = string.Empty;
                                            if (elems.Length > 0)
                                            {
                                                strHeader += string.Format("{0}.{1}", pi.Name, elems[0]);
                                                if (elems.Length > 2)
                                                {
                                                    strHeader += " (" + elems[2] + ")";
                                                }
                                            }
                                            else
                                            {
                                                strHeader += string.Format("{0}.Unkown", pi.Name);
                                            }
                                            if (elems.Length > 1)
                                            {
                                                strLog += elems[1];
                                            }
                                            if (i < items.Length - 1)
                                            {
                                                strLog += ",";
                                                strHeader += ",";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strLog += valString;
                                        strHeader += pi.Name;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                    } // End ForEach
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            string filepath = U.GetLogFilePath(filename);
            if (!File.Exists(filepath))
            {
                U.LogCustom(filename, strHeader);
            }

            U.LogCustom(filename, strLog);
        }

        /// <summary>
        /// Allow to be compared
        /// </summary>
        /// <param name="otherTriggerPt"></param>
        /// <returns></returns>
        public virtual int CompareTo(CompBase otherComp)
        {
            string[] split = Name.Split('_');
            string[] splitOther = otherComp.Name.Split('_');
            for (int i = 0; i < split.Length; i++)
            {
                if (i >= splitOther.Length)
                {
                    return split[i].CompareTo(string.Empty);
                }
                if (U.IsNumber(split[i]) && U.IsNumber(splitOther[i]))
                {
                    int cmpVal = Convert.ToInt32(split[i]).CompareTo(Convert.ToInt32(splitOther[i]));
                    if (cmpVal != 0)
                    {
                        return cmpVal;
                    }
                }
                else
                {
                    int cmpVal = split[i].CompareTo(splitOther[i]);
                    if (cmpVal != 0)
                    {
                        return cmpVal;
                    }
                }
            }
            return Name.CompareTo(otherComp.Name);
        }

        #region JJB Testing


        //public struct Prop<T>
        //{
        //    double dvAL;
        //    /// <summary>
        //    /// Delegate for the callback
        //    /// </summary>
        //    /// <param name="newVal"></param>
        //    public delegate void DelPropNotify(T newVal);
        //    /// <summary>
        //    /// Event to fire the changed event
        //    /// </summary>
        //    public event DelPropNotify OnChanged;
        //    private T _value;

        //    /// <summary>
        //    /// Use for set;
        //    /// </summary>
        //    /// <param name="val"></param>
        //    /// <returns></returns>
        //    public implicit operator = (T newValue)
        //    {
        //        return newValue;
        //    }
        //    private T Value
        //    {
        //        get
        //        {
        //            lock (this)
        //            {
        //                return _value;
        //            }
        //        }
        //        set
        //        {
        //            bool bChanged = false;
        //            lock (this)
        //            {
        //                bChanged = !object.Equals(_value, value);
        //                if (bChanged)
        //                {
        //                    _value = value;
        //                }
        //            }
        //            if (bChanged && OnChanged != null)
        //            {
        //                OnChanged(value);
        //            }
        //        }
        //    }
        //}

        #endregion JJB Testing

    }
    public class TimingElement
    {
        public long ts = U.DateTimeNow;
        public string Operations { get; set; }
        public void ReplaceRoot(string from, string to)
        {
            Operations = Operations.Replace(from, to);
        }
    }
}
