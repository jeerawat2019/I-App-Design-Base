using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using MCore.Controls;

namespace MCore.Comp
{
    public class CompRoot : CompBase
    {
        #region Private data
        private string _rootFolder = string.Empty;
        private Dictionary<string, object> _dictComponents = new Dictionary<string, object>();
        private static Dictionary<Type, List<Type>> s_controlTypeList = new Dictionary<Type, List<Type>>();
        private static List<Type> s_loadedCompTypes = new List<Type>();
        private static List<Type> s_pluginCompTypes = new List<Type>();
        private PerformanceCounter _cpuCounter = null;

        #endregion

        #region Properties

        #endregion
        #region Public Properties

        public const string RootComponent = "Root Component";

        public delegate void DelLogEntry(LogEntry logEntry);

        public string PluginFolder
        {
            get { return string.Format(@"{0}\PlugIn\", _rootFolder); }
        }
        /// <summary>
        /// The folder where to put the data
        /// </summary>
        [XmlIgnore]
        public string RootFolder
        {
            get { return _rootFolder; }
            set { _rootFolder = value; }
        }

        /// <summary>
        /// Gewts the CPU perc used currenlty
        /// </summary>
        public string CPUPerc
        {
            get 
            {
                return _cpuCounter.NextValue() + "%"; 
            }
        }


        public enum eCleanUpMode
        {
            None,
            FirstLayer,
            AllLayer,
        }
        
        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public CompRoot()
        {
            _dictComponents.Add(RootComponent, this);
        }
        #endregion Constructors

        /// <summary>
        /// Get a type from string
        /// </summary>
        /// <param name="strType"></param>
        /// <returns></returns>
        public static Type GetTypeFromString(string strType)
        {
            Type ty = s_loadedCompTypes.Find(c => c.Name == strType);
            if (ty == null)
            {
                ty = s_pluginCompTypes.Find(c => c.Name == strType);
            }
            if (ty == null)
            {
                ty = Type.GetType(strType);
            }
            return ty;
        }

        public override void Add(CompBase child)
        {
            CompBase compRead = ReadXmlFile(child.GetType(), child.Name);
            if (compRead != null)
            {
                RecurseConstructChild(child, compRead);
                child = compRead;
            }
            base.Add(child);
            child.Initialize();
        }

        public void Add(CompBase child,eCleanUpMode cleanUpMode = eCleanUpMode.None)
        {
            CompBase compRead = ReadXmlFile(child.GetType(), child.Name);
            if (compRead != null)
            {
                RecurseConstructChild(child, compRead);
                RecurseCleanUpChild(child, compRead,cleanUpMode);
                child = compRead;
            }
            base.Add(child);
            child.Initialize();
        }

        private void RecurseConstructChild(CompBase child,CompBase readComp)
        {
            if(child.ChildArray != null)
            {
                foreach(CompBase subChild in child.ChildArray)
                {
                   CompBase foundChild = readComp.FindChild(subChild.Name);
                    if(foundChild != null)
                    {
                        if(subChild.ChildArray!=null)
                        {
                            RecurseConstructChild(subChild, foundChild);
                        }
                    }
                    else
                    {
                        readComp.Add(subChild);
                    }
                }
            }
        }

        private void RecurseCleanUpChild(CompBase child,CompBase readComp,eCleanUpMode cleanUpMode)
        {
            if(cleanUpMode == eCleanUpMode.None)
            {
                return;
            }

            if (readComp.ChildArray != null)
            {
                foreach (CompBase subChild in readComp.ChildArray)
                {
                    CompBase foundChild = child.FindChild(subChild.Name);
                    if (foundChild != null)
                    {
                        if (subChild.ChildArray != null)
                        {
                            if (cleanUpMode == eCleanUpMode.AllLayer)
                            {
                                RecurseCleanUpChild(foundChild, subChild, cleanUpMode);
                            }
                        }
                    }
                    else
                    {
                        readComp.Remove(subChild);
                    }
                }
            }
        }


        public void Add(ComponentDefinition compDef,eCleanUpMode cleanUpMode = eCleanUpMode.None)
        {

            foreach (Type tyLoaded in s_loadedCompTypes)
            {
                compDef.ConfirmNotPluginType(tyLoaded);
            }
            foreach (Type tyPlugIn in s_pluginCompTypes)
            {
                compDef.AssignPlugInType(tyPlugIn);
            }

            List<ComponentDefinition> unresolvedPlugins = new List<ComponentDefinition>();
            compDef.GetUnloadedPlugins(unresolvedPlugins);
            // Assigns Sims, adds overrides to Serializer
            CreateSimClasses(compDef, unresolvedPlugins);

            AppStatus(string.Format("Reading {0}", compDef.Name));

            //
            CompBase compRead = ReadXmlFile(compDef.CompType, compDef.Name);
            if (compRead == null)
            {
                compRead = Activator.CreateInstance(compDef.CompType, compDef.Name) as CompBase;
            }
            else
            {
                CompBase child = Activator.CreateInstance(compDef.CompType, compDef.Name) as CompBase;
                compDef.CreateComponents(child);
                RecurseCleanUpChild(child, compRead,cleanUpMode);

            }
            compDef.CreateComponents(compRead);
            base.Add(compRead);
            compRead.Initialize();
        }

        /// <summary>
        /// Read the xml file and create a Component tree
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static CompBase ImportFile(Type ty, string filePath)
        {
            DefineXmlSerializer(ty);
            // Try to deserialize it
            return MCoreXmlSerializer.LoadObjectFromFile(filePath, ty) as CompBase;
        }

        private CompBase ReadXmlFile(Type ty, string name)
        {
            string path = GetXmlPath(name);

            if (File.Exists(path))
            {
                try
                {
                    return ImportFile(ty, path);
                }
                catch (Exception ex)
                {
                    U.LogPopup(ex, "Could not read '{0}'", path); ;
                }

            }
            return null; ;
        }

        public static Type[] GetSiblingTypes(Type tySibling)
        {
            Type baseType = tySibling.BaseType;
            if (baseType != null)
            {
                List<Type> types = new List<Type>();
                foreach (Type t in s_loadedCompTypes)
                {
                    if (t.BaseType.Name == baseType.Name)
                    {
                        types.Add(t);
                    }
                }
                return types.ToArray();
            }
            return null;
        }


        private static void DefineXmlSerializer(Type ty)
        {
            XmlAttributes attrs = new XmlAttributes();

            foreach (Type t in s_loadedCompTypes)
            {
                XmlArrayItemAttribute attr = new XmlArrayItemAttribute { ElementName = t.Name, Type = t };
                attrs.XmlArrayItems.Add(attr);
            }

            foreach (Type t in s_pluginCompTypes)
            {
                XmlArrayItemAttribute attr = new XmlArrayItemAttribute { ElementName = t.Name, Type = t };
                attrs.XmlArrayItems.Add(attr);
            }

            XmlAttributeOverrides attrOverrides = new XmlAttributeOverrides();
            attrOverrides.Add(typeof(CompBase), "ChildArray", attrs);
            MCoreXmlSerializer.DefineSerializer(ty, attrOverrides);
        }


        public override void Destroy()
        {
            base.Destroy();
            U.LogClose();
        }

        private string GetXmlPath(string name)
        {
            return string.Format(@"{0}\{1}.xml", RootFolder, name);
        }

        private static string GetSimSource(Type baseType, string name, string sim)
        {
            string src = string.Format(
@"
using System;
namespace {0}
{{
    public class {1}{3} : {2}
    {{
        public {1}{3}()
        {{
        }}
        public {1}{3}(string name) : base(name)
        {{
        }}
        public {1}{3}(string name, object[] initParms) : base(name)
        {{
        }}
        public override void Initialize()
        {{
            base.Initialize();
        }}	 
", baseType.Namespace, name, baseType.Name, sim);
            if (baseType.Name == "LoggerBase")
            {
                src += "protected override void OnLog(LogEntry logEntry) { }";
            }
            src += "}}";
            return src;
        }

        /// <summary>
        /// Add Sim class pair item for all loaded plugin classes 
        /// </summary>
        private void CreateSimClasses()
        {
            List<string> listSources = new List<string>();
            foreach (Type ty in s_pluginCompTypes)
            {
                if (ty.BaseType != null)
                {
                    // Make sure that we have all the constructors we need in the base class
                    ConstructorInfo constructor0 = ty.BaseType.GetConstructor(Type.EmptyTypes);
                    ConstructorInfo constructor1 = ty.BaseType.GetConstructor(new Type[] { typeof(string) });

                    if (constructor0 != null &&
                        constructor1 != null)
                    {
                        listSources.Add(GetSimSource(ty.BaseType, ty.Name, CompBase.Sim));
                    }
                }
            }
            if (listSources.Count > 0)
            {
                s_pluginCompTypes.AddRange(CompileClasses("LoadedPlugins", listSources));
            }
        }
        /// <summary>
        /// Add Sim class pairings for all unloaded plugin classes 
        /// </summary>
        /// <param name="compDef"></param>
        /// <param name="unresolvedPlugins"></param>
        private void CreateSimClasses(ComponentDefinition compDef, List<ComponentDefinition> unresolvedPlugins)
        {
            if (unresolvedPlugins.Count == 0)
            {
                return;
            }
            //
            // Create classes for unresolved plugins
            //
            List<string> listSources = new List<string>();
            foreach (ComponentDefinition def in unresolvedPlugins)
            {
                listSources.Add(GetSimSource(def.PluginBaseType, def.PluginName, string.Empty));
                listSources.Add(GetSimSource(def.PluginBaseType, def.PluginName, CompBase.Sim));
            }

            Type[] createdTypes = CompileClasses(compDef.Name, listSources);
            s_pluginCompTypes.AddRange(createdTypes);

            compDef.AssignUnresolvedPlugin(createdTypes);
        }

        private Type[] CompileClasses(string assemblyName, List<string> listSources)
        {
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();

            CompilerParameters cpTemp = new CompilerParameters();
            cpTemp.OutputAssembly = assemblyName;
            cpTemp.GenerateExecutable = false;
            cpTemp.GenerateInMemory = true;


            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p=>!p.IsDynamic).ToArray();
          
            foreach (Assembly assembly in loadedAssemblies)
            {
                cpTemp.ReferencedAssemblies.Add(assembly.Location);
            }

            CompilerResults crTemp = provider.CompileAssemblyFromSource(cpTemp, listSources.ToArray());
            if (crTemp.Errors.HasErrors)
            {
                throw new MCoreExceptionPopup("Unable to compile Plugin Sims. First error:\n{0}", crTemp.Errors[0].ErrorText);
            }
            return crTemp.CompiledAssembly.GetTypes();
        }
        /// <summary>
        /// Returns true if the Component exists
        /// /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool ComponentExists(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return false;
            }
            lock (_dictComponents)
            {
                if (_dictComponents.ContainsKey(ID))
                {
                    object obj = _dictComponents[ID];
                    return obj is CompBase;
                }
                return false;
            }
        }
        /// <summary>
        /// Retrieve the Component based on the ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CompBase GetComponent(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return null;
            }
            lock (_dictComponents)
            {
                if (_dictComponents.ContainsKey(ID))
                {
                    object obj = _dictComponents[ID];
                    if (obj is CompBase)
                    {
                        return obj as CompBase;
                    }
                    else if (obj is List<CompBase>)
                    {
                        List<CompBase> list = obj as List<CompBase>;
                        if (list.Count == 1)
                        {
                            return list[0];
                        }
                    }
                }
                throw new Exception(string.Format("Component '{0}' does not exist in the Component dictionary", ID));
            }
        }

        /// <summary>
        /// Retrieve the all components that match the ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CompBase[] GetComponents(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return null;
            }
            lock (_dictComponents)
            {
                if (_dictComponents.ContainsKey(ID))
                {
                    object obj = _dictComponents[ID];
                    if (obj is CompBase)
                    {
                        return new CompBase[] { obj as CompBase };
                    }
                    else if (obj is List<CompBase>)
                    {
                        List<CompBase> list = obj as List<CompBase>;
                        return list.ToArray();
                    }
                }
                throw new Exception(string.Format("No components named '{0}' exist in the Component dictionary", ID));
            }
        }

        /// <summary>
        /// Register all the possible unique names for this component
        /// </summary>
        /// <param name="comp"></param>
        public void RegisterComponent(CompBase comp)
        {

            List<string> namelist = GetNameList(comp);

            lock (_dictComponents)
            {
                string[] ids = namelist.ToArray();
                for(int i=0; i< ids.Length; i++)
                {
                    string id = ids[i];
                    if (_dictComponents.ContainsKey(id))
                    {
                        if (i == ids.Length-1)
                        {
                            // It is entitled to own this key because it is the full path to the component
                            _dictComponents[id] = comp;
                        }
                        else
                        {
                            // Not unique.  Make list or add to it
                            object obj = _dictComponents[id];
                            if (obj is CompBase && (obj as CompBase).ID != id)
                            {
                                // Convert to List
                                List<CompBase> compList = new List<CompBase>();
                                compList.Add(obj as CompBase);
                                compList.Add(comp);
                                _dictComponents[id] = compList;
                            }
                            else if (obj is List<CompBase>)
                            {
                                (obj as List<CompBase>).Add(comp);
                            }
                        }
                    }
                    else
                    {
                        _dictComponents.Add(id, comp);
                    }
                }
            }
        }
        /// <summary>
        /// Get the list of all IDs for this component
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public List<string> GetNameList(CompBase comp)
        {
            List<string> list = new List<string>();
            if (!(comp is CompRoot))
            {
                string id = comp.Name;
                while (true)
                {
                    list.Add(id);
                    comp = comp.Parent;
                    if (comp == null || comp is CompRoot)
                        break;
                    id = string.Format("{0}.{1}", comp.Name, id);
                }
            }
            return list;
        }


        /// <summary>
        /// Get all the unique names that are registered for this component.
        /// They are ordered by shortest to last.
        /// There will always be at least one item in the returned array
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public string[] GetUniqueNames(CompBase comp)
        {
            List<string> namelist = GetNameList(comp);
            lock (_dictComponents)
            {
                string[] nameArray = namelist.ToArray();
                foreach (string name in nameArray)
                {
                    if (_dictComponents.ContainsKey(name))
                    {
                        object obj = _dictComponents[name];
                        if (obj is List<CompBase>)
                        {
                            List<CompBase> list = obj as List<CompBase>;
                            if (list.Count != 1 || !object.ReferenceEquals(list[0], comp))
                            {
                                // Not unique
                                namelist.Remove(name);
                            }
                        }
                        else if (!object.ReferenceEquals(obj, comp))
                        {
                            namelist.Remove(name);
                        }
                    }
                    else
                    {
                        namelist.Remove(name);
                    }
                }
            }
            return namelist.ToArray();
        }


        /// <summary>
        ///Unregister all the unique ids for this component
        /// </summary>
        /// <param name="ID"></param>
        public void UnregisterComponent(CompBase comp)
        {
            List<string> namelist = GetNameList(comp);
            lock (_dictComponents)
            {
                foreach (string name in namelist)
                {
                    if (_dictComponents.ContainsKey(name))
                    {
                        object obj = _dictComponents[name];
                        if (obj is CompBase)
                        {
                            _dictComponents.Remove(name);
                        }
                        else if (obj is List<CompBase>)
                        {
                            (obj as List<CompBase>).Remove(comp);
                        }
                        _dictComponents.Remove(name);
                    }
                }
            }
        }
        private string _releaseNotes = string.Empty;
        private static string _titleText = string.Empty;
        private static Form _mainForm = null;
        private static object __lockStatus = new object();
        private static string _prevText = string.Empty;
        public static void AppStatus(string status)
        {
            lock (__lockStatus)
            {
                if (status == _prevText)
                {
                    return;
                }
                _prevText = status;
            }
            if (_mainForm.InvokeRequired)
            {
                _mainForm.BeginInvoke(new D.delVoid_String(AppStatusSafe), status);
            }
            else
            {
                AppStatusSafe(status);
            }
        }

        public static void AppStatusSafe(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                _mainForm.Text = _titleText;
            }
            else
            {
                _mainForm.Text = string.Format("{0}               ({1})", _titleText, status);
            }
        }
        public class ThreadTest
        {
            ~ThreadTest()
            {
                U.AddThread("GC Finalizer");
            }
        }       /// <summary>
        /// Initialize the Logger
        /// </summary>
        public void ApplicationSetup(Form mainForm, string rootFolder)
        {
            _mainForm = mainForm;
            U.GetDummyControl();
            U.AddThread("Main GUI");
            {
                ThreadTest test = new ThreadTest();
                test = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            AppStatus("Initialize CPU Counter");
            _cpuCounter = new PerformanceCounter();

            _cpuCounter.CategoryName = "Processor";
            _cpuCounter.CounterName = "% Processor Time";
            _cpuCounter.InstanceName = "_Total";
            string zero = CPUPerc;  // First one is zero
            U.DumpProcessUsage("AppSetup1");  // Dump Processes running
            string rawVer = "Unknown";
            string title = "Unknown Title";
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            Version version = entryAssembly.GetName().Version;  //.Getatt(typeof(AssemblyVersionAttribute), true);
            string ReleaseCandidate = "Official";
            if (version.Revision != 0)
            {
                ReleaseCandidate = string.Format("Release Candidate {0}", version.Revision);
            }
            rawVer = string.Format("{0}.{1} Build {2} ({3})", version.Major, version.Minor, version.Build, ReleaseCandidate);

            object[] attrs = entryAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                title = ((AssemblyTitleAttribute)attrs[0]).Title;
            }

            attrs = entryAssembly.GetCustomAttributes(typeof(AssemblyReleaseNotesAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                _releaseNotes = ((AssemblyReleaseNotesAttribute)attrs[0]).ReleaseNotes;
                mainForm.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelp);

            }
            _titleText = string.Format("{0} - Ver {1}", title, rawVer);

            U.LogInfo("Application Loading : {0}", _titleText);

            AppStatus("Application Setup");


            U.DumpProcessUsage("AppSetup2");  // Dump Processes running

            if (rootFolder.EndsWith("\\"))
            {
                rootFolder = rootFolder.Substring(0, rootFolder.Length - 1);
            }
            RootFolder = rootFolder;
            U.s_mainThreadId = Thread.CurrentThread.ManagedThreadId;

            // Handling UI thread unhandle exceptions.
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ApplicationUI_ThreadException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            // Get all non-system loaded assemblies
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Dictionary<string, Assembly> loadedNames = new Dictionary<string, Assembly>();
            foreach (Assembly assembly in loadedAssemblies)
            {
                AssemblyName assemblyName = new AssemblyName(assembly.FullName);
                if (!assemblyName.Name.StartsWith("System") &&
                    !assemblyName.Name.StartsWith("mscorlib") &&
                    !assemblyName.Name.StartsWith("vshost32") &&
                    !assemblyName.Name.StartsWith("Microsoft"))
                {
                    loadedNames.Add(assemblyName.Name, assembly);
                }
            }
            // Now get all references asswemblies
            AddReferencedAssemblies(entryAssembly, loadedNames);

            List<Type> controlTypes = new List<Type>();

            s_loadedCompTypes.Add(typeof(CompBase));

            foreach (Assembly assembly in loadedNames.Values)
            {
                Type[] assemblyTypes = assembly.GetTypes();
                s_loadedCompTypes.AddRange(assemblyTypes.Where(t => t.IsClass && t.IsSubclassOf(typeof(CompBase))));
                controlTypes.AddRange(assemblyTypes.Where(t => t.IsClass && t.IsSubclassOf(typeof(Control))));
            }

            #region Load all component types from plug in


            U.EnsureDirectory(_rootFolder);
            U.EnsureDirectory(PluginFolder);
            foreach (string dll in Directory.GetFiles(PluginFolder, "*.dll"))
            {
                Assembly assemblyPlugIn = Assembly.LoadFrom(dll);
                Type[] assemblyTypes = null;
                try
                {
                    assemblyTypes = assemblyPlugIn.GetTypes();
                    s_pluginCompTypes.AddRange(assemblyTypes.Where(t => t.IsClass && t.IsSubclassOf(typeof(CompBase))));
                    controlTypes.AddRange(assemblyTypes.Where(t => t.IsClass && t.IsSubclassOf(typeof(Control))));
                    U.LogInfo("PlugIn loaded: {0}", assemblyPlugIn.FullName);
                }
                catch (Exception ex)
                {
                    U.LogPopup("PlugIn Failed: {0}  Reason: {1}", assemblyPlugIn.FullName, ex.Message);
                }
            }

            CreateSimClasses();

            AppStatus("Adding Control Pages");
            AddControlPages(controlTypes);

            #endregion

            U.LogInfo("StopWatch Resolution={0}", Stopwatch.IsHighResolution);
            U.LogInfo("StopWatch Frequency={0}", Stopwatch.Frequency);

            U.DumpProcessUsage("End AppSetup");  // Dump Processes running

        }
        private void AddReferencedAssemblies(Assembly assembly, Dictionary<string, Assembly> loadedNames)
        {

            AssemblyName[] referencedAssembyNames = assembly.GetReferencedAssemblies();
            foreach (AssemblyName assemblyName in referencedAssembyNames)
            {
                // Only pick up assembolies that might have ComponentBase
                if (!assemblyName.Name.StartsWith("System") &&
                    !assemblyName.Name.StartsWith("mscorlib") &&
                    !assemblyName.Name.StartsWith("vshost32") &&
                    !assemblyName.Name.StartsWith("Microsoft") &&
                    !loadedNames.ContainsKey(assemblyName.Name))
                {
                    Assembly loadAssembly = AppDomain.CurrentDomain.Load(assemblyName);
                    loadedNames.Add(assemblyName.Name, loadAssembly);
                    AddReferencedAssemblies(loadAssembly, loadedNames);
                }
            }
        }
        private void AddControlPages(List<Type> controlTypes)
        {
            if (controlTypes.Count > 0)
            {
                foreach (Type tyControl in controlTypes)                
                {
                    Type[] tyI = tyControl.GetInterfaces();


                    Type ty = tyControl.GetInterface(typeof(IComponentBinding<CompBase>).Name);
                    if (ty != null)
                    {
                        PropertyInfo pi = ty.GetProperty("Bind");
                        Type compBaseTy = pi.PropertyType;
                        if (!s_controlTypeList.ContainsKey(compBaseTy))
                        {
                            s_controlTypeList.Add(compBaseTy, new List<Type>());
                        }
                        s_controlTypeList[compBaseTy].Add(tyControl);
                    }
                }
            }
        }
        public static Type[] GetControlPages(Type compType)
        {
            if (s_controlTypeList.ContainsKey(compType))
            {
                return s_controlTypeList[compType].ToArray();
            }
            return null;
        }


        public static event DelLogEntry OnLogEntry;

        private static Queue<LogEntry> _logPostedItems = new Queue<LogEntry>(10);
        private static ReaderWriterLockSlim _lockPostLog = new ReaderWriterLockSlim();
        private Thread _logThread = null;
        public static LogSeverity settingSeverity = LogSeverity.Info;
        /// <summary>
        /// Send the log
        /// </summary>
        /// <param name="logEntry"></param>
        public void Log(LogEntry logEntry)
        {
            _lockPostLog.EnterWriteLock();
            try
            {
                _logPostedItems.Enqueue(logEntry);

                if (_logThread == null)
                {
                    _logThread = new Thread(LogThreadProc);
                    _logThread.Name = "Logging";
                    _logThread.Start();
                }
            }
            finally
            {
                _lockPostLog.ExitWriteLock();
            }
        }

        private static void LogThreadProc()
        {
            U.AddThread("Logging");
            try
            {
                bool aborting = false;
                int logCount = 0;
                do
                {
                    if (OnLogEntry != null)
                    {
                        LogEntry logEntry = null;
                        _lockPostLog.EnterWriteLock();
                        try
                        {
                            logCount = _logPostedItems.Count;
                            if (logCount > 0)
                            {
                                logEntry = _logPostedItems.Dequeue();
                            }
                            else
                            {
                                _logPostedItems.TrimExcess();
                            }
                        }
                        finally
                        {
                            _lockPostLog.ExitWriteLock();
                        }

                        if (logEntry != null)
                        {
                            if (logEntry.severity == LogSeverity.CloseLogger)
                            {
                                aborting = true;
                            }
                            else if (logEntry.severity >= settingSeverity)
                            {
                                OnLogEntry(logEntry);
                            }
                        }
                    }
                    Thread.Sleep(10);
                } while (!aborting || logCount > 0);
            }
            finally
            {
                U.RemoveThread();
            }
        }
        /// <summary>
        /// recursively Export (saveAs) all the settings of the CompBase component
        /// </summary>
        /// <param name="pathName"></param>
        /// <param name="compChild"></param>
        public static void ExportSettingsAutoName(string pathName, CompBase compChild)
        {
            if (compChild != null)
            {
                string filePath = string.Format(@"{0}\{1}.xml", pathName, compChild.ID);
                ExportSettings(filePath, compChild);
            }
        }

        /// <summary>
        /// recursively Export (saveAs) all the settings of the CompBase component
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="compChild"></param>
        public static void ExportSettings(string filePath, CompBase compChild)
        {
            try
            {
                U.EnsureWritable(filePath);
                DefineXmlSerializer(compChild.GetType());
                MCoreXmlSerializer.SaveObjectToFile(filePath, compChild);
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Error Saving '{0}'", compChild.Name);
            }
        }

        public void SaveSettings() 
        {
            foreach (CompBase compChild in ChildArray)
            {
                string filePath = GetXmlPath(compChild.Name);
                // Back up first
                if (File.Exists(filePath))
                {
                    AppStatus(string.Format("Creating backup for {0}", compChild.Name));
                    string backupPath = string.Format(@"{0}\Backup for {1}\{1}-{2}.xml", RootFolder, compChild.Name, DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
                    U.EnsureDirectory(backupPath);
                    File.Copy(filePath, backupPath, true);
                }
                AppStatus(string.Format("Saving {0}", compChild.Name));
                ExportSettings(filePath, compChild);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Unhandled Exception has occured in an non-GUI thread
            // So instead of keep silence and log, we inform user that we are closing.
            object o = e.ExceptionObject;
            if (o is MCoreException)
            {
                // Caller knows what to do.  Info is packed in the exception
                MCoreException mex = o as MCoreException;
                U.Log(mex);
            }
            else if (o is Exception)
            {
                // Caller knows what to do.  Info is packed in the exception
                Exception ex = o as Exception;
                U.LogPopup(ex, "Unhandled Exception");
            }
            else
            {
                U.LogFatal("Unhandled Exception {0}", o.ToString());
                Application.Exit();
            }
        }
        static void ApplicationUI_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                U.LogFatal(e.Exception, "Unhandled Exception");
            }
        }
        private void OnHelp(object sender, HelpEventArgs hlpevent)
        {
            new ReleaseNotesForm(_releaseNotes).ShowDialog();
        }
    }
}
