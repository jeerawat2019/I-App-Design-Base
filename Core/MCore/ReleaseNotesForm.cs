using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCore
{
    public partial class ReleaseNotesForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int WM_VSCROLL = 0x115;
        private const int SB_BOTTOM = 7;

        public ReleaseNotesForm(string notes)
        {
            InitializeComponent();
            tbNotes.Text = notes;
            tbNotes.SelectionStart = notes.Length;
            SendMessage(tbNotes.Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, IntPtr.Zero);
        }
    }
}
