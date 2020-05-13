using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;

namespace MCore.Comp.Communications
{
    public partial class Terminal : UserControl,IComponentBinding<CommunicationBase>
    {
        private CommunicationBase _commBase = null;
        private DataRecievedEventHandler _delDataReceived = null;
        public Terminal()
        { 
            InitializeComponent();
            _delDataReceived = new DataRecievedEventHandler(OnDataReceived);

        }

        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CommunicationBase Bind
        {
            get { return _commBase; }
            set
            {
                _commBase = value;
                _commBase.OnDataReceived += _delDataReceived;
            }
        }

        private void OnDataReceived(string text)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(_delDataReceived, new object[] { text });
                return;
            }
            if (cbRun.Checked)
            {
                tbTerminal.Text += text + "\r\n";
                tbTerminal.SelectionStart = tbTerminal.TextLength;
                tbTerminal.ScrollToCaret();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
           
            tbTerminal.Text += tbCommand.Text + "\r\n";
            cbRun.Checked = true;
            _commBase.WriteLine(tbCommand.Text);
            tbCommand.Text = string.Empty;
        }

        public override string ToString()
        {
            return "Terminal Window";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbTerminal.Text = string.Empty;
        }

    }
}
