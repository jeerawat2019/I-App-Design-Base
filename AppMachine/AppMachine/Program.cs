using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using ComponentFactory.Krypton.Toolkit;

namespace AppMachine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool createdNew = true;
                using (Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out createdNew))
                {
                    if (createdNew)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new AppMainForm());
                    }
                    else
                    {
                        AppUtility.ShowKryptonMessageBox("Application Already Running", string.Format("{0} is already running!", Assembly.GetExecutingAssembly().GetName().Name), "",
                                                         TaskDialogButtons.OK, MessageBoxIcon.Error, null);
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtility.ShowKryptonMessageBox("Application Exception","",string.Format("{0}\n{1}", ex.Message, ex.StackTrace), TaskDialogButtons.OK, MessageBoxIcon.Error, null);
            }
        }
    }
}
