using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MCore.Comp;
using MDouble;
namespace MCore
{
    /// <summary>
    /// Static helper functions intended to enhance MCore
    /// </summary>
    public static class U
    {
        [DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        public static extern Int32 GetCurrentWin32ThreadId();
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool QueryServiceStatusEx(SafeHandle serviceHandle, int infoLevel, IntPtr buffer, int bufferSize, out int bytesNeeded);
        /// Key Down message index
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;
        /// <summary>
        /// Key Down message index
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// Control key down when click
        /// </summary>
        public const int MK_CONTROL = 0x08;
        /// <summary>
        /// Shift key down when click
        /// </summary>
        public const int MK_SHIFT = 0x04;

        /// <summary>
        /// Char messageIndex
        /// </summary>
        public const int WM_CHAR = 0x0102;
        /// <summary>
        /// Paint messageIndex
        /// </summary>
        public const int WM_PAINT = 0x000f;
        /// <summary>
        /// The start date/time
        /// </summary>
        public static DateTime StartDateTime = DateTime.Now;
        private static Stopwatch _runningStopwatch = Stopwatch.StartNew();

        /// <summary>
        /// Get the current date Time
        /// </summary>
        public static long DateTimeNow
        {
            get { return StartDateTime.Ticks + _runningStopwatch.Elapsed.Ticks; }
        }

        /// <summary>
        /// Convert ticks to milliseconds seconds
        /// </summary>
        public static double TicksToMS(long ticks)
        {
            return (double)ticks / (double)TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Convert ticks to seconds
        /// </summary>
        public static double TicksToSec(long ticks)
        {
            return TicksToMS(ticks) / 1000.0;
        }

        /// <summary>
        /// Convert milliseconds to ticks
        /// </summary>
        public static long MSToTicks(double ms)
        {
            return (long)Math.Ceiling(ms * TimeSpan.TicksPerMillisecond);
        }

        /// <summary>
        /// Convert seconds to ticks
        /// </summary>
        public static long SecToTicks(double sec)
        {
            return MSToTicks(sec * 1000.0);
        }

        public const float SQRTHALF = 0.7071067811865475244f;
        /// <summary>
        /// Ensure that directory exists
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool EnsureDirectory(string path)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (dir == null || dir.Length == 0)
                    return false;
                if (Directory.Exists(dir))
                    return true;
                Directory.CreateDirectory(dir);
                return true;
            }
            catch
            {
            }
            return false;
        }


        /// <summary>
        /// Make sure the file is writable
        /// </summary>
        /// <param name="filePath"></param>
        public static void EnsureWritable(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileAttributes fa = File.GetAttributes(filePath);
                    if ((fa & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        fa &= ~FileAttributes.ReadOnly;
                        File.SetAttributes(filePath, fa);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex, "Could not make {0} writable", filePath);
            }
        }
        public static int s_mainThreadId = 0;
        public static volatile bool _doingEvents = false;
        /// <summary>
        /// If on the GUI thread, calls DoEvents and sleeps, otherwise sleeps only
        /// </summary>
        /// <param name="ms"></param>
        public static long SleepWithEvents(int ms)
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (IsMainGUIThread)
            {
                DateTime stop = DateTime.Now.AddTicks(ms*10000);                
                do
                {
                    if (!_doingEvents)
                    {
                        _doingEvents = true;
                        Application.DoEvents();
                        _doingEvents = false;
                    }
                    Thread.Sleep(1);
                } while (ms != 1 && DateTime.Now < stop);
            }
            else
            {
                Thread.Sleep(ms);
            }
            return sw.ElapsedMilliseconds;
        }
        /// <summary>
        /// If on the GUI thread, calls DoEvents, otherwise, blocks the thread until event signals
        /// </summary>
        /// <param name="waitMoveDone"></param>
        /// <param name="timeout"></param>
        public static void BlockOrDoEvents(ManualResetEvent waitEvent, int timeout)
        {
            if (IsMainGUIThread)
            {
                DateTime dtTimeout = DateTime.Now.AddMilliseconds(timeout);
                Cursor origCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                while (!waitEvent.WaitOne(1))
                {
                    if (DateTime.Now > dtTimeout)
                    {
                        throw new Exception("Timeout waiting for event.");
                    }
                    if (!_doingEvents)
                    {
                        _doingEvents = true;
                        Application.DoEvents();
                        _doingEvents = false;
                    }
                }
                Cursor.Current = origCursor;
            }
            else
            {
                if (!waitEvent.WaitOne(timeout))
                {
                    throw new Exception("Timeout waiting for event.");
                }
            }            
        }

        /// <summary>
        /// Delay for a number of millisecond
        /// </summary>
        /// <param name="msecTimeout"></param>
        public static void Delay(int msecTimeout)
        {
            if (IsMainGUIThread)
            {
                Stopwatch sw = Stopwatch.StartNew();
                Cursor origCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                while (sw.ElapsedMilliseconds < msecTimeout)
                {
                    if (!_doingEvents)
                    {
                        _doingEvents = true;
                        Application.DoEvents();
                        _doingEvents = false;
                    }
                    Thread.Sleep(10);
                }
                Cursor.Current = origCursor;
            }
            else
            {
               Thread.Sleep(msecTimeout);
            }
        }
        /// <summary>
        /// Returns true if the thread is the Main GUI thread
        /// </summary>
        public static bool IsMainGUIThread
        {
            get { return s_mainThreadId == Thread.CurrentThread.ManagedThreadId; }
        }

        private static CompRoot _compRoot = new CompRoot();
        /// <summary>
        /// Get the root component
        /// </summary>
        public static CompRoot RootComp
        {
            get { return _compRoot; }
        }

        class ThreadInfo
        {
            public string Name { get; set; }
            public TimeSpan PrevTime { get; set; }
        };
        private static Dictionary<int, ThreadInfo> _threads = new Dictionary<int, ThreadInfo>();
        /// <summary>
        /// Add this thread to the list of running threads
        /// </summary>
        /// <param name="name">Name of the thread</param>
        /// <param name="args"></param>
        public static void AddThread(string name, params object[] args)
        {
            try
            {
                name = string.Format(name, args);
            }
            catch (Exception exFormat)
            {
                name += string.Format(" Unable to apply args to formated msg.  Reason: {0}", exFormat.Message);
            }
            lock (_threads)
            {
                int procThreadId = GetCurrentWin32ThreadId();
                ThreadInfo threadInfo = new ThreadInfo() { Name = name };
                if (!_threads.ContainsKey(procThreadId))
                {
                    _threads.Add(procThreadId, threadInfo);
                }
                else
                {
                    _threads[procThreadId] = threadInfo;
                }
            }
        }

        public static void RemoveThread()
        {
            lock (_threads)
            {
                int procThreadId = GetCurrentWin32ThreadId();
                if (_threads.ContainsKey(procThreadId))
                {
                    _threads.Remove(procThreadId);
                }
            }
        }
        class ProcessInfo
        {
            public string procName { get; set; }
            public TimeSpan totalTime { get; set; }
            public TimeSpan timeSinceLast { get; set; }
            public string type { get; set; }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class SERVICE_STATUS_PROCESS
        {
            public int serviceType;
            public int currentState;
            public int controlsAccepted;
            public int win32ExitCode;
            public int serviceSpecificExitCode;
            public int checkPoint;
            public int waitHint;
            public int processID;
            public int serviceFlags;
        }

        public static int GetServiceProcessId(this ServiceController sc)
        {
            if (sc == null)
                throw new ArgumentNullException("sc");

            IntPtr zero = IntPtr.Zero;

            try
            {
                int dwBytesNeeded;
                const int SC_STATUS_PROCESS_INFO = 0;
                const int ERROR_INSUFFICIENT_BUFFER = 0x7A;
                // Call once to figure the size of the output buffer.
                QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, zero, 0, out dwBytesNeeded);
                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    // Allocate required buffer and call again.
                    zero = Marshal.AllocHGlobal((int)dwBytesNeeded);

                    if (QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, zero, dwBytesNeeded, out dwBytesNeeded))
                    {
                        var ssp = new SERVICE_STATUS_PROCESS();
                        Marshal.PtrToStructure(zero, ssp);
                        return (int)ssp.processID;
                    }
                }
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return -1;
        }        private static Dictionary<int, ProcessInfo> _prevProcesslist = new Dictionary<int, ProcessInfo>();

        private static long _lastProcessDump = U.DateTimeNow;
        private static List<string> _svcExceptions = new List<string>();
        /// <summary>
        /// Dumps CPU Usage to try and find causes of delays
        /// </summary>
        public static void DumpProcessUsage(string tag)
        {
            long enterTick = U.DateTimeNow;
            double msSinceLast = U.TicksToMS(enterTick - _lastProcessDump);
            U.LogInfo("Dumping Processes at {0}.  Current total CPU Usage=,{1}, time since last=,{2}", tag, RootComp.CPUPerc, msSinceLast.ToString("####0.#"));

            for (int i = 0; i < _prevProcesslist.Count; i++ )
            {
                KeyValuePair<int, ProcessInfo> keyVal = _prevProcesslist.ElementAt(i);
                keyVal.Value.type = string.Empty;  // Not there anymore
            }
            double totalTimeSinceLast = 0.0;
            Process[] curProcesslist = Process.GetProcesses();
            // First pass, compute delta times
            foreach (Process process in curProcesslist)
            {
                if (process.Id != 0 && process.ProcessName != "System")
                {
                    try
                    {
                        // Not Idle process
                        // Find Prev proc
                        if (_prevProcesslist.ContainsKey(process.Id))
                        {
                            ProcessInfo pi = _prevProcesslist[process.Id];
                            if (pi.procName == process.ProcessName)
                            {
                                // Existing process.  Compute processor time since last
                                TimeSpan tsSinceLast = process.TotalProcessorTime - pi.totalTime;
                                totalTimeSinceLast += tsSinceLast.TotalSeconds;
                                pi.timeSinceLast = tsSinceLast;
                                pi.type = "Existing";
                            }
                            else
                            {

                                totalTimeSinceLast += process.TotalProcessorTime.TotalSeconds;
                                pi.procName = process.ProcessName;
                                pi.timeSinceLast = process.TotalProcessorTime;
                                pi.type = "Replaced";
                            }
                            pi.totalTime = process.TotalProcessorTime;
                        }
                        else
                        {
                            totalTimeSinceLast += process.TotalProcessorTime.TotalSeconds;
                            // Newly Added, use TotalProcessorTime 
                            _prevProcesslist.Add(process.Id, new ProcessInfo()
                            {
                                procName = process.ProcessName,
                                totalTime = process.TotalProcessorTime,
                                timeSinceLast = process.TotalProcessorTime,
                                type = "Added"
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    //Console.WriteLine("Process: {0} ID: {1} Info: {2}  ProcTime={3}", process.ProcessName, process.Id, process.StartTime.ToString(), process.TotalProcessorTime);
                }
            }
            long time1 = U.DateTimeNow;
            double dElapsed = U.TicksToMS(time1 - enterTick);
            U.LogInfo("Time to getProceses = {0} mS", dElapsed.ToString("###,###.##"));
            ServiceController[] services = ServiceController.GetServices();
            Dictionary<int, List<string>> servicesByPid = new Dictionary<int, List<string>>();
            foreach (ServiceController service in services)
            {

                if (service.Status == ServiceControllerStatus.Running && !_svcExceptions.Contains(service.ServiceName))
                {
                    try
                    {
                        int pid = GetServiceProcessId(service);
                        if (!servicesByPid.ContainsKey(pid))
                        {
                            servicesByPid.Add(pid, new List<string>());
                        }
                        servicesByPid[pid].Add(service.DisplayName);
                    }
                    catch
                    {
                        _svcExceptions.Add(service.ServiceName);
                        U.LogInfo("** Service threw exception: '{0}'", service.DisplayName);
                    }
                }
            }
            dElapsed = U.TicksToMS(U.DateTimeNow - time1);
            U.LogInfo("Time to extract services = {0} mS", dElapsed.ToString("###,###.##"));
            Process curProcess = Process.GetCurrentProcess();
            // Log
            for (int i = _prevProcesslist.Count-1; i >= 0; i--)
            {
                KeyValuePair<int, ProcessInfo> keyVal = _prevProcesslist.ElementAt(i);
                if (string.IsNullOrEmpty(keyVal.Value.type))
                {
                    // Not there anymore
                    U.LogInfo("Removed Id={0}, Name={1}", keyVal.Key, keyVal.Value.procName);
                    _prevProcesslist.Remove(keyVal.Key);
                }
                else
                {
                    double secSinceLast = keyVal.Value.timeSinceLast.TotalSeconds;
                    double perc = secSinceLast * 100.0 / totalTimeSinceLast;
                    if (perc > 1.0 || keyVal.Value.type == "Added" || keyVal.Key == curProcess.Id)
                    {
                        U.LogInfo("{0} Id={1}, Name={2}, {3},(%),{4},(Sec)", keyVal.Value.type, keyVal.Key, keyVal.Value.procName, perc.ToString("###0.##"), secSinceLast.ToString("###0.##"));
                        if (servicesByPid.ContainsKey(keyVal.Key))
                        {
                            foreach (string serviceName in servicesByPid[keyVal.Key])
                            {
                                U.LogInfo("** Service'{0}'", serviceName);
                            }
                        }
                        else if (keyVal.Key == curProcess.Id)
                        {
                            ProcessThreadCollection threads = curProcess.Threads;
                            foreach (ProcessThread procThread in threads)
                            {
                                try
                                {
                                    TimeSpan ts = procThread.TotalProcessorTime;
                                    string name = "Unknown";
                                    double ms = 0.0;
                                    lock (_threads)
                                    {
                                        if (_threads.ContainsKey(procThread.Id))
                                        {
                                            name = _threads[procThread.Id].Name;
                                            ms = ts.TotalMilliseconds - _threads[procThread.Id].PrevTime.TotalMilliseconds;
                                            _threads[procThread.Id].PrevTime = ts;
                                        }
                                    }
                                    if (ms > 0.1)
                                    {
                                        U.LogInfo("** Thread {0}({1}) = {2} mS", name, procThread.Id, ms.ToString("###,##0.#"));
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            dElapsed = U.TicksToMS(U.DateTimeNow - enterTick);
            U.LogInfo("Total time to dump = {0} mS", dElapsed.ToString("###,###.##"));
            _lastProcessDump = enterTick;

        }
        /// <summary>
        /// Returns true if the component exists
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool ComponentExists(string ID)
        {
            return _compRoot.ComponentExists(ID);

        }
        /// <summary>
        /// Retrieve the Component based on the ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static CompBase GetComponent(string ID)
        {
            return _compRoot.GetComponent(ID);
        }

        /// <summary>
        /// Asynchronously make a call to a void method with no parameters
        /// </summary>
        /// <param name="del"></param>
        public static void AsyncCall(MethodInvoker del)
        {
            del.BeginInvoke(null, null);
        }
        /// <summary>
        /// Asynchronously make a call to a void method with int parameter
        /// </summary>
        /// <param name="del"></param>
        /// <param name="iVal"></param>
        public static void AsyncCall(D.delVoid_Int del, int iVal)
        {
            del.BeginInvoke(iVal, null, null);
        }

        /// <summary>
        /// Asynchronously make a call to a void method with double parameter
        /// </summary>
        /// <param name="del"></param>
        /// <param name="dVal"></param>
        public static void AsyncCall(D.delVoid_Double del, double dVal)
        {
            del.BeginInvoke(dVal, null, null);
        }

        private static Control _sDummyControl = null;
        /// <summary>
        /// Get Dummy Control to invoke for those who do not have access to UI thread
        /// </summary>
        /// <returns></returns>
        public static Control GetDummyControl()
        {
            if (_sDummyControl == null)
            {
                _sDummyControl = new Control();
                _sDummyControl.Handle.ToString(); // Force create handle
            }
            return _sDummyControl;
        }

        /// <summary>
        /// remove all child controls and dispose each; 
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearAndDisposeChildren(Control parent)
        {
            if (parent.Controls != null)
            {
                int count = parent.Controls.Count;
                if (count > 0)
                {
                    Control[] ctls = new Control[count];
                    parent.Controls.CopyTo(ctls, 0);
                    parent.Controls.Clear();
                    foreach (Control ctl in ctls)
                    {
                        ctl.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Safely dispose of control
        /// </summary>
        /// <param name="control"></param>
        public static void SafeDisposeControl(Control control)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new D.ControlEventHandler(SafeDisposeControl), control);
                return;
            }
            control.Dispose();
        }


        public static void MoveTabPages(TabControl tabControl)
        {
            Size tabPagesSize = new Size(tabControl.Width - 8, tabControl.Height - 26);
            foreach (TabPage tabPage in tabControl.Controls)
            {
                tabPage.Size = tabPagesSize;
                if (tabPage.Controls.Count > 0)
                {
                    System.Windows.Forms.Control control = tabPage.Controls[0];
                    control.Size = tabPagesSize;
                }
            }
        }

        /// <summary>
        /// Make the child equal to the parent
        /// </summary>
        /// <param name="parent"></param>
        public static void EqualSizeChild(Control parent)
        {

            if (parent.Controls != null && parent.Controls.Count == 1)
            {
                parent.Controls[0].Size = parent.Size;
            }
        }

        /// <summary>
        /// This returns true if the specified class is derived from a specified base class.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public static bool IsDerivedFrom(Type t, Type baseType)
        {
            if (t == null || t == typeof(Object))
                return false;
            if (t == baseType)
                return true;

            return IsDerivedFrom(t.BaseType, baseType);
        }

        /// <summary>
        /// Returns true if the string is nothing but numbers
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsDigit(str[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Snap value to the nearest grid position
        /// </summary>
        /// <param name="val"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static Millimeters Snap(Millimeters val, Millimeters gridSize)
        {
            if (gridSize.Val != 0.0)
            {
                int iSnapVal = (int)Math.Round(val.Val / gridSize.Val);
                return iSnapVal * gridSize.Val;
            }
            return val;
        }
        /// <summary>
        /// Snap value to the nearest angfle
        /// </summary>
        /// <param name="val"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        public static Radians Snap(Radians val, Radians gridSize)
        {

            if (gridSize.Val != 0.0)
            {
                int iSnapVal = (int)Math.Round(val.Val / gridSize.Val);
                return iSnapVal * gridSize.Val;
            }
            return val;
        }
        /// <summary>
        /// Get the assembly version
        /// </summary>
        /// <param name="assy"></param>
        public static string GetAssemblyVersion(Assembly assy)
        {
            object[] attrs = assy.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                return ((AssemblyFileVersionAttribute)attrs[0]).Version;
            }
            return "Version Unknown!";
        }

        /// <summary>
        /// Extract the ComponentBase pointer from the provided Lambda Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static CompBase GetComponentFromPropExpression<T>(Expression<Func<T>> property)
        {
            MemberExpression memberExpression = property.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new Exception("Must provide a Lamda Expression to identify the property");
            }
            Expression exp = memberExpression.Expression;
            if (exp == null)
            {
                throw new Exception("Must provide a Lamda Expression to identify the property");
            }
            // Extract the object base pointer
            var lambda = Expression.Lambda<Func<CompBase>>(exp);
            Func<CompBase> func = lambda.Compile();
            return func();
        }
        
        /// <summary>
        /// Return the checksum for all the bits in this bitmap
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public static int GetChecksum(Bitmap bm)
        {
            try
            {
                BitmapData bmpData = bm.LockBits(new Rectangle(Point.Empty, bm.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, bm.PixelFormat);
                IntPtr iptr = bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * bm.Height;
                byte[] rgbValues = new byte[bytes];
                Marshal.Copy(iptr, rgbValues, 0, bytes);
                int checksum = 0;
                for (int counter = 0; counter < rgbValues.Length; counter++)
                {
                    checksum += rgbValues[counter];
                }

                bm.UnlockBits(bmpData);
                return checksum;
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Problem finding the checksum for Bitmap");
            }
            return 0;
        }


        /// <summary>
        /// Register a property to notify to a specific callback.
        /// Only fires if the prperty has changed value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public static void RegisterOnChanged<T>(Expression<Func<T>> property, CompBase.NotifyItem<T>.DelPropNotify del)
        {
            CompBase comp = GetComponentFromPropExpression(property);
            if (comp == null)
            {
                throw new Exception("Must provide a valid ComponentBase pointer");
            }
            comp.RegisterOnChanged(property, del);
        }
        /// <summary>
        /// Register a property to notify to a specific callback.
        /// Only fires if the prperty has changed value
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public static void RegisterOnChanged<S, T>(Expression<Func<T>> property, CompBase.NotifyItemSender<S,T>.DelPropSenderNotify del)
        {
            CompBase comp = GetComponentFromPropExpression(property);
            if (comp == null)
            {
                throw new Exception("Must provide a valid ComponentBase pointer");
            }
            comp.RegisterOnChanged(property, del);
        }
        /// <summary>
        /// UnRegister a previously registered notification
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public static void UnRegisterOnChanged<T>(Expression<Func<T>> property, CompBase.NotifyItem<T>.DelPropNotify del)
        {
            CompBase comp = GetComponentFromPropExpression(property);
            if (comp == null)
            {
                throw new Exception("Must provide a valid ComponentBase pointer");
            }

            comp.UnRegisterOnChanged(property, del);
        }
        /// <summary>
        /// UnRegister a previously registered notification
        /// </summary>
        /// <typeparam name="S">Sender type</typeparam>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="property"></param>
        /// <param name="del"></param>
        public static void UnRegisterOnChanged<S, T>(Expression<Func<T>> property, CompBase.NotifyItemSender<S,T>.DelPropSenderNotify del)
        {
            CompBase comp = GetComponentFromPropExpression(property);
            if (comp == null)
            {
                throw new Exception("Must provide a valid ComponentBase pointer");
            }

            comp.UnRegisterOnChanged(property, del);
        }


        public static Point Center(Rectangle rc)
        {
            return new Point(rc.Left + rc.Width / 2, rc.Top + rc.Height / 2);
        }

        /// <summary>
        /// Get a list of Methods and Properties that match the return type and arguments
        /// </summary>
        /// <param name="comp">The class object containing the methods</param>
        /// <param name="includeOnlyAttribute"></param>
        /// <param name="retType">The return type of the property or method.
        /// If the retType is void, then arguments are allowed.</param>
        /// <returns></returns>
        public static string[] GetMethodList(CompBase comp, Type includeOnlyAttribute, Type retType)
        {
            MethodInfo[] methods = comp.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            List<string> list = new List<string>();

            foreach (MethodInfo method in methods)
            {
                if (includeOnlyAttribute != null)
                {
                    object[] ca = method.GetCustomAttributes(false);
                    if (ca.Count((c) => c.GetType().Equals(includeOnlyAttribute)) == 0)
                    {
                        continue;
                    }
                    
                }
                if (method.ReturnType == retType || retType.Equals(typeof(Object)))
                {
                    string name = method.Name;
                    if (retType.Equals(typeof(void)))
                    {
                        // Add parameters
                        name += "(";
                        ParameterInfo[] args = method.GetParameters();
                        if (args.Length > 0)
                        {
                            string prefix = "";
                            foreach (ParameterInfo pi in args)
                            {
                                string tyName = pi.ParameterType.FullName;
                                if (tyName.StartsWith("System."))
                                {
                                    tyName = tyName.Substring(7);
                                }
                                name += prefix + string.Format("({0})", tyName);
                                prefix = ",";
                            }
                        }
                        name += ")";
                        list.Add(name);
                    }
                    else
                    {
                        // Getting properties only
                        if (name.StartsWith("get_"))
                        {
                            // Property, shouldn't be any arguments
                            list.Add(name.Substring(4));
                        }
                    }
                }
            }

            return list.ToArray();
        }

        public static string GetDefaultText(Type type)
        {
            object obj = GetDefault(type);
            if (obj != null)
                return obj.ToString();
            return string.Empty;
        }
        public static object GetDefault(Type type)
        {
            try
            {
                if (type.Name.Contains('['))
                {
                    return null;
                }
                if (type.Equals(typeof(string)))
                {
                    return string.Empty;
                }
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }                
                if (type.IsClass)
                {
                    if (type.GetConstructor(Type.EmptyTypes) != null)
                    {
                        return Activator.CreateInstance(type);
                    }
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not get default for type '{0}'", type.Name); 
            }
            return null;
        }

        public static Type TryGetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        public static string SplitPropertyID(string id, out string propPath)
        {
            propPath = string.Empty;
            int lastPeriod = id.LastIndexOf('.');
            if (lastPeriod < 0)
            {
                // No target to be found
                return string.Empty;
            }
            propPath = id.Substring(0, lastPeriod);
            return id.Substring(lastPeriod + 1);

        }
        /// <summary>
        /// Get the PropertyInfo for a given property name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Object obj, string propertyName, Type expectedType)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                if (expectedType == null || expectedType.Name == pi.PropertyType.Name)
                {
                    return pi;
                }
            }
            return null;
        }

        //private static object _lockLOHOp = new object();
        //public static double MaxLOHMBytes = 50.0;
        //private static double _totalMBytes = 0;
        //public static void LOHAdded(double bytes)
        //{
        //    double collect = 0.0;
        //    lock (_lockLOHOp)
        //    {                
        //        _totalMBytes += Math.Ceiling(bytes/1024)/1024;
        //        if (_totalMBytes >= MaxLOHMBytes)
        //        {
        //            collect = _totalMBytes;
        //            _totalMBytes = 0;
        //        }
        //    }
        //    if (collect != 0.0)
        //    {
        //        Stopwatch sw = Stopwatch.StartNew();
        //        long totalMem = GC.GetTotalMemory(true)/1024/1024;
        //        System.Diagnostics.Debug.WriteLine(string.Format("GC.TotalMemory= {0} MB LOH Collected {1} MB in {2} ms", totalMem, collect, sw.ElapsedMilliseconds));
        //    }
        //}
        /// <summary>
        ///  Get the PropertyInfo for a given ID.  Must match the type expected
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(string id)
        {
            return GetPropertyInfo(id, null);
        }
        /// <summary>
        ///  Get the PropertyInfo for a given ID.  Must match the type expected
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(string id, Type expectedType)
        {
            string propPath = string.Empty;
            string propertyName = SplitPropertyID(id, out propPath);
            if (!ComponentExists(propPath))
            {
                // No target to be found
                return null;
            }

            CompBase comp = GetComponent(propPath);
            return GetPropertyInfo(comp, propertyName, expectedType);

        }

        /// <summary>
        /// Split up the Method ID into Method Path and string array of argument IDs
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="methodPath"></param>
        /// <returns></returns>
        public static string[] SplitMethodID(string ID, out string methodPath)
        {
            string[] sArgs = null;
            // Extract args
            int nFirstArg = ID.IndexOf('(');
            if (nFirstArg >= 0)
            {
                methodPath = ID.Substring(0, nFirstArg);
                string args = ID.Substring(nFirstArg + 1);
                args = args.Trim();
                if (args[args.Length-1] == ')')
                {
                    args = args.Substring(0, args.Length-1);
                }
                args = args.Trim();
                if (args.Length != 0)
                {
                    sArgs = args.Split(',');
                }
            }
            else
            {
                methodPath = ID;
            }
            return sArgs;
        }

        /// <summary>
        /// Invoke a method according to the method ID
        /// No argumentsare expected
        /// </summary>
        /// <param name="methodID">In the form 'compBase.compBase.method'</param>
        public static void InvokeNoArgMethod(string methodID)
        {
            // Extract Target and Method name
            int lastPeriod = methodID.LastIndexOf('.');
            if (lastPeriod < 0)
            {
                U.LogPopup("Expected a target in string: '{0}'", methodID);
                return;
            }
            string compID = methodID.Substring(0, lastPeriod);
            if (!ComponentExists(compID))
            {
                U.LogPopup("Not a valid component ID ('{0}') in ID string: '{1}'", compID, methodID);
                return;
            }
            CompBase target = U.GetComponent(compID);
            string methodName = methodID.Substring(lastPeriod + 1);
            MethodInfo mi = null;
            Exception exGetMethod = null;
            try
            {
                mi = target.GetType().GetMethod(methodName, new Type[0] );
            }
            catch (Exception ex)
            {
                exGetMethod = ex;
            }
            if (mi == null)
            {
                U.LogPopup(exGetMethod, "Could not find a method named '{0}.{1}' in ID string: '{2}'", methodName, compID, methodID);
                return;            
            }
            try
            {
                MethodInvoker del = Delegate.CreateDelegate(typeof(MethodInvoker), target, mi) as MethodInvoker;
                if (del != null)
                {
                    AsyncCall(del);
                }
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Could not invoke the call '{0}'", methodID);
            }
            
            //mi.Invoke(target, null);
            
        }

        /// <summary>
        /// Get the angle of the point to origin
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static double GetAngle(PointF pt)
        {
            return Math.Atan2(pt.Y, pt.X);
        }

        /// <summary>
        /// Get the distance for this point from the origin
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static double GetDistance(PointF pt)
        {
            return Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
        }

        public static PointF Add(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }
        public static PointF Subtract(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        /// <summary>
        /// Return the Average (Mean) of the elements in the array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double Average(double[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                return double.NaN;
            }
            double dValTotal = 0.0;
            foreach (double dVal in arr)
            {
                dValTotal += dVal;
            }
            return dValTotal / (double)arr.Length;
        }
        /// <summary>
        /// Return the Variance of the elements in the array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double Variance(double[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                return double.NaN;
            }
            double mean = Average(arr);
            double dValTotal = 0.0;
            foreach (double dVal in arr)
            {

                double diff = dVal - mean;
                dValTotal += diff * diff;
            }
            return dValTotal / ((double)arr.Length-1);
        }
        /// <summary>
        /// Return the Standard deviation of the elements in the array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double StdDev(double[] arr)
        {
            double var = Variance(arr);
            if (double.IsNaN(var))
            {
                return double.NaN;
            }
            return Math.Sqrt(var);
        }

        /// <summary>
        /// Return a point rotated about the origin (0,0)
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="rd"></param>
        /// <returns></returns>
        public static PointF RotateAboutOrigin(PointF pt, double radians)
        {
            double len = GetDistance(pt);
            double radDraw = Math.Atan2(-pt.Y, pt.X);
            double radRotation = radDraw + radians;
            float X = (float)(len * Math.Cos(radRotation));
            float Y = (float)(len * Math.Sin(radRotation));
            return new PointF(X, -Y);
        }

        /// <summary>
        /// Rotate the bitmap by 0, 90, 180, or 270 degrees
        /// </summary>
        /// <param name="imageSource"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Bitmap RotateImage(Bitmap imageSource, double degrees)
        {
            if (imageSource == null)
            {
                return null;
            }
            Bitmap bmRet = null;
            try
            {
                lock (imageSource)
                {
                    bmRet = new Bitmap(imageSource);
                }
                RotateFlipType rotType = RotateFlipType.RotateNoneFlipNone;
                switch ((int)Math.Round(degrees))
                {
                    case 270:
                    case -90:
                        rotType = RotateFlipType.Rotate270FlipNone;
                        break;
                    case -270:
                    case 90:
                        rotType = RotateFlipType.Rotate90FlipNone;
                        break;
                    case 180:
                    case -180:
                        rotType = RotateFlipType.Rotate180FlipNone;
                        break;
                }
                bmRet.RotateFlip(rotType);
            }
            catch (Exception ex)
            {
                U.LogError(ex, "Error to RotateImage");
            }
            return bmRet;
        }

        /// <summary>
        /// Rotate an image about a point
        /// </summary>
        /// <param name="image"></param>
        /// <param name="point"></param>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static Bitmap RotateImageAboutPt(Bitmap image, PointF point, float degrees)
        {
            if (image == null)
            {
                return null;
            }
            Bitmap newImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.TranslateTransform(point.X, point.Y);
                g.RotateTransform(degrees);
                g.TranslateTransform(-point.X, -point.Y);
                g.DrawImage(image, Point.Empty);
            }
            return newImage;
        }

        /// <summary>
        /// Convert a string to array
        /// The string separates values with ','.  Example: "1,4,13"
        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
        public static int[] ArrayFromString(string sArray)
        {
            if (string.IsNullOrEmpty(sArray))
            {
                return null;
            }
            try
            {
                string[] split = sArray.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
                int[] array = new int[split.Length];
                for (int i=0; i<split.Length; i++)
                {
                    try
                    {
                        array[i] = Convert.ToInt32(split[i]);
                    }
                    catch
                    {
                        array[i] = 0;
                    }
                }
                return array;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert a refernce array from one type to another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T[] ConvertArray<T>(Array source)
        {
            Array arr = Array.CreateInstance(typeof(T), source.Length);
            Array.Copy(source, arr, source.Length);
            return arr as T[];
        }

        /// <summary>
        /// Convert a string to a double.  Remove any trailing text after space
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double SafeToDouble(string text)
        {
            try
            {
                if (text.Contains(' '))
                {
                    text = text.Split(' ')[0];
                }
                return Convert.ToDouble(text);
            }
            catch { }
            return 0.0;
        }

        /// <summary>
        /// Parse and execute formulae
        /// </summary>
        /// <param name="formulae">"1.0 + 2.0*c + 1.2*r"</param>
        /// <param name="assignments">"c=2", "r=3"</param>
        /// <returns></returns>
        public static double Formulae(string formulae, params string[] assignments)
        {
            string[] split = null;
            if (assignments != null)
            {
                foreach (string assignment in assignments)
                {
                    split = assignment.Split('=');
                    if (split.Length == 2)
                    {
                        if (formulae.Contains(split[0]))
                        {
                            formulae = formulae.Replace(split[0], split[1]);
                        }
                    }
                    else
                    {
                        U.LogPopup("Unexpected formulae assignment {0}", assignment);
                        return 0.0;
                    }
                }
            }
            try
            {
                var calc = new System.Data.DataTable();
                object oAns = calc.Compute(formulae, string.Empty);

                return Convert.ToDouble(oAns);
            }
            catch
            {
            }
            return 0.0;
        }

        /// <summary>
        /// Get the array of class hierarchy starting with the root base class
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="upToClass"></param>
        /// <returns></returns>
        public static Type[] GetClassHierarchy(Type ty, Type upToClass)
        {
            List<Type> list = new List<Type>();
            list.Add(ty);
            if (!Type.Equals(ty, upToClass))
            {
                while (ty.BaseType != null && !Type.Equals(ty, upToClass))
                {
                    ty = ty.BaseType;
                    list.Insert(0, ty);
                }
            }
            return list.ToArray();
        }

        #region Mean and Sigma utils
        /// <summary>
        /// Get population STDEV of an ICollection of double
        /// </summary>
        /// <param name="doubleCollection"></param>
        /// <returns>STDEV of the population</returns>
        /// <remarks>Exception will be thrown if the collection does not contain valid double numbers</remarks>
        public static double StDev(ICollection doubleCollection)
        {
            double average = GetMean(doubleCollection);
            double sumOfDerivation = 0;
            foreach (double value in doubleCollection)
            {
                sumOfDerivation += Math.Pow(value - average, 2);
            }
            double sumOfDerivationAverage = SafeDivide(sumOfDerivation, doubleCollection.Count);
            return Math.Sqrt(sumOfDerivationAverage);
        }

        /// <summary>
        /// Get mean of an ICollection of double
        /// </summary>
        /// <param name="doubleCollection"></param>
        /// <returns>Mean of the population</returns>
        /// <remarks>Exception will be thrown if the collection does not contain valid double numbers</remarks>
        public static double GetMean(ICollection doubleCollection)
        {
            double sum = 0;
            foreach (double value in doubleCollection)
            {
                sum += value;
            }
            return SafeDivide(sum, doubleCollection.Count);
        }


        /// <summary>
        /// Safe divide 2 double
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static double SafeDivide(double value1, double value2)
        {
            double ret = 0;
            try
            {
                if ((value1 == 0) || (value2 == 0)) { return ret; }
                ret = value1 / value2;
            }
            catch { }
            return ret;
        }
        #endregion


        #region Logging

        public static void DeleteLogFile(string filename)
        {
            try
            {
                File.Delete(GetLogFilePath(filename));
            }
            catch (Exception ex)
            {
                U.LogPopup(ex, "Unable to delete log file '{0}'", filename);
            }
        }

        public static string GetLogFilePath(string filename)
        {
            return string.Format(@"{0}\Log\{1}", U.RootComp.RootFolder, filename);
        }

        public static void LogCustom(string filename, string msg)
        {
            LogCustom(filename, msg, string.Empty);
        }

        public static void LogCustom(string filename, string msg, string header)
        {
            string strFullLogFileName = GetLogFilePath(filename);
            U.EnsureDirectory(strFullLogFileName);
            bool needsHeader =  !string.IsNullOrEmpty(header) && !File.Exists(strFullLogFileName);

            using (StreamWriter logWriter = new StreamWriter(strFullLogFileName, true))
            {
                if (needsHeader)
                {
                    logWriter.WriteLine(header);
                }
                logWriter.WriteLine(msg);
                logWriter.Close();
            }
        }

        /// <summary>
        /// Write an Exception entry to the log file
        /// </summary>
        /// <param name="ex">The generic exception</param>
        public static void Log(Exception ex)
        {
            if (ex is MCoreException)
            {
                MCoreException mex = ex as MCoreException;
                Log(mex.Severity, mex.Procedure, GetRecursiveExceptionMessage(mex), null);
            }
            else
            {
                LogError(ex, ex.Message);
            }
        }


        #region LogPopup
        /// <summary>
        /// Write an Log entry to the log file and Pop it up.  Also log with alarm on
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogAlarmPopup(string msg, params object[] args)
        {
            LogAlarmPopup(null, msg, args);
        }

        /// <summary>
        /// Write an Log entry to the log file and Pop it up.  Also log with alarm on
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogAlarmPopup(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Popup | LogSeverity.AlarmOn, msg, args);
        }

        /// <summary>
        /// Write an Log entry to the log file and Pop it up
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogPopup(string msg, params object[] args)
        {
            Log(null, LogSeverity.Popup, msg, args);
        }

        /// <summary>
        /// Write an exception entry to the log file and Pop it up
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogPopup(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Popup, msg, args);
        }
        #endregion LogPopup


        private static string GetRecursiveExceptionMessage(Exception ex)
        {
            string msg = string.Empty;
            while (ex != null)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    msg += string.Format(" \nMore Info: {0}", ex.Message);
                }
                ex = ex.InnerException;
            }
            return msg;
        }
        #region LogClose
        /// <summary>
        /// Command to close the logger
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogClose()
        {
            Log(null, LogSeverity.CloseLogger, "Close Log Thread");
        }

        #endregion LogClose

        #region LogUserChange
        /// <summary>
        /// Write an Log entry to the log file
        /// </summary>
        /// <param name="propDel">The source for the change</param>
        /// <param name="fromVal"></param>
        public static void LogChange(PropDelBase propDel, string fromVal)
        {
            if (propDel.CompTarget == null)
            {
                LogChange(propDel.Name, fromVal, propDel.SValue);
            }
            else
            {
                LogChange(string.Format("{0}.{1}", propDel.CompTarget.Nickname, propDel.Name), fromVal, propDel.SValue);
            }
        }
        /// <summary>
        /// Write an Log entry to the log file
        /// </summary>
        /// <param name="id">The source for the change</param>
        /// <param name="fromVal"></param>
        /// <param name="toVal"></param>
        public static void LogChange(string id, string fromVal, string toVal)
        {
            LogChange(id + ", was changed from, " + fromVal + ", to, " + toVal);
        }
        /// <summary>
        /// Write an Log entry to the log file
        /// </summary>
        /// <param name="itemAdded">The item added</param>
        public static void LogChangeAdded(string itemAdded)
        {
            LogChange(string.Format("{0}, was added", itemAdded));
        }
        /// <summary>
        /// Write an Log entry to the log file
        /// </summary>
        /// <param name="itemRemoved">The item removed</param>
        public static void LogChangeRemoved(string itemRemoved)
        {
            LogChange(string.Format("{0}, was removed", itemRemoved));
        }
        /// <summary>
        /// Write an Log entry to the log file
        /// </summary>
        /// <param name="msg">The source for the change</param>
        public static void LogChange(string msg)
        {
            Log(null, LogSeverity.UserChange, msg);
        }
        #endregion LogUserChange


        #region LogAlert
        /// <summary>
        /// Write an Log entry to the log file and send to Alert window
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogAlert(string msg, params object[] args)
        {
            Log(null, LogSeverity.Alert, msg, args);
        }

        /// <summary>
        /// Write an exception entry to the log file and send to Alert window
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogAlert(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Alert, msg, args);
        }
        #endregion LogAlert

        #region LogError
        /// <summary>
        /// Write an Error entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogError(string msg, params object[] args)
        {
            Log(null, LogSeverity.Error, msg, args);
        }

        /// <summary>
        /// Write an exception Error entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogError(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Error, msg, args);
        }
        #endregion LogError

        #region LogFatal
        /// <summary>
        /// Write an Fatal entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogFatal(string msg, params object[] args)
        {
            Log(null, LogSeverity.Fatal, msg, args);
        }

        /// <summary>
        /// Write an exception Fatal entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogFatal(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Fatal, msg, args);
        }
        #endregion LogFatal

        #region LogWarning
        /// <summary>
        /// Write a warning Log entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogWarning(string msg, params object[] args)
        {
            Log(null, LogSeverity.Warning, msg, args);
        }

        /// <summary>
        /// Write an exception warning Log entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogWarning(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Warning, msg, args);
        }
        #endregion LogWarning

        #region LogDebug
        /// <summary>
        /// Write a Debug Log entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogDebug(string msg, params object[] args)
        {
            Log(null, LogSeverity.Debug, msg, args);
        }

        /// <summary>
        /// Write an exception Debug Log entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogDebug(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Debug, msg, args);
        }
        #endregion LogDebug


        #region LogInfo
        /// <summary>
        /// Write a Info Log entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogInfo(string msg, params object[] args)
        {
            Log(null, LogSeverity.Info, msg, args);
        }

        /// <summary>
        /// Write an exception Info Log entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogInfo(Exception ex, string msg, params object[] args)
        {
            Log(ex, LogSeverity.Info, msg, args);
        }
        /// <summary>
        /// Write an AlarmOff Log entry to the log file
        /// </summary>
        /// <param name="msg">The actual msg to log</param>
        /// <param name="args"></param>
        public static void LogAlarmOff(LogEntry logEntry)
        {
            RootComp.Log(new LogEntry(LogSeverity.AlarmOff, logEntry.procedureName, logEntry.text, logEntry.customFileDumps));
        }
        #endregion LogInfo


        private static Queue<LogEntry> _logPostedItems = new Queue<LogEntry>(10);
        private static ReaderWriterLockSlim _lockPostLog = new ReaderWriterLockSlim();

         /// <summary>
        /// Write an exception entry to the log file
        /// </summary>
        /// <param name="ex">The generic excption</param>
        /// <param name="severity"></param>
        /// <param name="msg">The actual msg to log</param>
        private static void Log(Exception ex, LogSeverity severity, string msg, params object[] args)
        {
            string procedureName = "Unknown Source";
            if (args != null && args.Length > 0)
            {
                try
                {
                    msg = string.Format(msg, args);
                }
                catch (Exception exFormat)
                {
                    msg += string.Format(" Unable to apply args to formated msg.  Reason: {0}", exFormat.Message);
                }
            }
            if (ex != null)
            {
                procedureName = ex.Source;
                msg += GetRecursiveExceptionMessage(ex);
            }
            else
            {
                StackTrace st = new StackTrace();
                foreach (StackFrame frame in st.GetFrames())
                {
                    MethodBase methodBase = frame.GetMethod();
                    if (!methodBase.Name.StartsWith("Log"))
                    {
                        procedureName = string.Format("{0}.{1}", methodBase.DeclaringType.Name, methodBase.Name);
                        break;
                    }
                }
            }
            Log(severity, procedureName, msg, null);
        }
       /// <summary>
        /// Write a log with custom dump files
        /// </summary>
        /// <remarks>
        /// Custom dump files are assembled into a string array.
        /// </remarks>
        /// <example>This code shows how write a log and send attachments to Email notifiers
        /// <code>
        ///    U.WriteLog(U.LogSeverity.Error, "Testing", "Dump Log File",
        ///               U.LogAttachment.ScreenShot | U.LogAttachment.ProcessDump,
        ///               new string[] { @"C:\MPT\USN\SPLBackEnd\SA Adjust\ScanLines.xls"} );
        /// </code>
        /// </example>
        /// <param name="severity">The type of log submitted ordered by importance</param>
        /// <param name="procedureName">Description of the entry or source of problem</param>
        /// <param name="text">The actual msg to log</param>
        /// <param name="customFileDumps">Array of filepaths to be dumped as well.</param>
        private static void Log(LogSeverity severity, string procedureName, string msg, string[] customFileDumps)
        {
            if (severity == LogSeverity.Warning
                || severity == LogSeverity.Error
                || severity == LogSeverity.Popup
                || severity == LogSeverity.Fatal)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} ({1}) : {2}", procedureName, severity.ToString(), msg));
            }
            RootComp.Log(new LogEntry(severity, procedureName, msg, customFileDumps));
        }
    }

    /// <summary>
    /// Class to store the Logging info
    /// </summary>
    public class LogEntry
    {
        public LogSeverity severity;
        public string procedureName;
        public string text;
        public string[] customFileDumps = null;
        public DateTime dateTime;
        public int threadId;
        public LogEntry(LogSeverity severity,
            string procedureName,
            string text,
            string[] customFileDumps)
        {
            this.dateTime = DateTime.Now;
            this.threadId = Thread.CurrentThread.ManagedThreadId;
            this.severity = severity;
            this.procedureName = procedureName;
            this.text = text;
            if (customFileDumps != null)
            {
                this.customFileDumps = new string[customFileDumps.Length];
                for (int i = 0; i < customFileDumps.Length; i++)
                {
                    this.customFileDumps[i] = customFileDumps[i];
                }
            }
        }
    }
    /// <summary>
    /// Severity for log
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// Zero means no flags
        /// </summary>
        NO_FLAGS = 0,
        /// <summary>
        /// Lowest severity, for debugging
        /// </summary>
        Debug = 0x00000004,
        /// <summary>
        /// Info
        /// </summary>
        Info = 0x00000010,
        /// <summary>
        /// Warning
        /// </summary>
        Warning = 0x00000040,
        /// <summary>
        /// A value has changed by the user
        /// </summary>
        UserChange = 0x00000100,
        /// <summary>
        /// Error
        /// </summary>
        Error = 0x00000400,
        /// <summary>
        /// Add entry in Alert window. Entry is always logged
        /// </summary>
        Alert = 0x00001000,
        /// <summary>
        /// Pop up a MessageBox.  User must Click Ok. Entry is always logged
        /// </summary>
        Popup = 0x00004000,
        /// <summary>
        /// Command to kill the Logging thread
        /// </summary>
        Fatal = 0x00010000,
        /// <summary>
        /// Alarm On
        /// </summary>
        AlarmOn = 0x00020000,
        /// <summary>
        /// Alarm Off
        /// </summary>
        AlarmOff = 0x00040000,
        /// <summary>
        /// Command to kill the Logging thread
        /// </summary>
        CloseLogger = -2,
        /// <summary>
        /// Mask for all
        /// </summary>
        MASK_ALL = -1
    };

    #endregion Logging
    public class WindowWrapper : IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }
        public IntPtr Handle
        {
            get { return _hwnd; }
        }
        private IntPtr _hwnd;
    }
}
