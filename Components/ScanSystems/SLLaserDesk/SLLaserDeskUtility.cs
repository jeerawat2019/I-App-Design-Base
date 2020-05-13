using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MCore.Comp.ScanSystem
{
    public class SLLaserDeskUtility
    {
        public const int DM_IN_BUFFER = 8;
        public const int DM_OUT_BUFFER = 2;
        public const int DM_IN_PROMPT = 4;


        /// <summary>
        /// Variable Call Function
        /// </summary>
        public const int WM_SYSCOMMAND = 274;
        public const int SC_MAXIMIZE = 61488;
        public const int SC_MINIMIZE = 61472;

        public const int WS_EX_APPWINDOW = 0x40000;
        public const int GWL_EXSTYLE = -0x14;
        public const int WS_EX_TOOLWINDOW = 0x0080;

        public const int WM_KEYDOWN = 0x0100;
        public const int VK_RETURN = 0x0D;

        /// <summary>Enumeration of the different ways of showing a window using 
        /// ShowWindow</summary>
        public enum WindowShowStyle : uint
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized 
            /// or maximized, the system restores it to its original size and 
            /// position. An application should specify this flag when displaying 
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position. 
            /// This value is similar to "ShowNormal", except the window is not 
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size 
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next 
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is 
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This 
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is 
            /// minimized or maximized, the system restores it to its original size 
            /// and position. An application should specify this flag when restoring 
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread 
            /// that owns the window is hung. This flag should only be used when 
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        public enum eStatusBit : uint
        {
            RM_STATE_WND_OPEN           = 0x00000001,
            RM_STATE_RTC_INIT            = 0x00000002,
            RM_STATE_LAS_INIT            = 0x00000004,
            RM_STATE_MOT_INIT            = 0x00000008,
            RM_STATE_ALL_INIT            = 0x0000000F,
            RM_STATE_READY              = 0x00000010,
            RM_STATE_AUTOMODE           = 0x00000020,
            RM_STATE_LST_EXEC           = 0x00000040,
            RM_STATE_LST_EXE_ERR        = 0x00000080,
            RM_STATE_RM_MODE            = 0x00000100,
            RM_STATE_JOB_LOAD           = 0x00000200,
            RM_STATE_LST_CALC           = 0x00000400,
            RM_STATE_CMD_ERR            = 0x00000800,
            RM_STATE_LAS_ERR            = 0x00001000,
            RM_STATE_LAS_ON             = 0x00002000,
            RM_STATE_DEV_ERR            = 0x00004000,
            RM_STATE_HEAD_OK            = 0x00008000,
            RM_STATE_EXEC_DONE          = 0x00010000,
            RM_STATE_PILOT_MODE         = 0x00020000,
            RM_STATE_JOB_ABORTED        = 0x00080000,
            RM_STATE_SWITCH_AUTOMODE    = 0x00100000,
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.Dll")]
        public static extern Int32 PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("User32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter, [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName, IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


       
    }
}
