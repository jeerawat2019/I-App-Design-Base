using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using MCore;

using MCore.Controls;

namespace MCore.Comp.ScanSystem
{
    public partial class SLLaserDeskWrapperCtrl : UserControl
    {

        private SLLaserDesk _laserDesk = null;
       

        public SLLaserDeskWrapperCtrl()
        {
            InitializeComponent();
        }

        


        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SLLaserDesk Bind
        {
            get { return _laserDesk; }
            set
            {
                _laserDesk = value;

                U.EnsureDirectory(_laserDesk.LaserJobFilesRootPath);
                browseForJobFile.OpenDialog.DefaultExt = "sld";
                browseForJobFile.OpenDialog.InitialDirectory = _laserDesk.LaserJobFilesRootPath;
                browseForJobFile.OpenDialog.Filter = "Job File (*.sld)|*.sld";
                browseForJobFile.BindTwoWay(() => _laserDesk.CurrentJobPath);
                this.Dock = DockStyle.Fill;
                SLLaserDeskUtility.ShowWindow(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WindowShowStyle.Hide);
                SLLaserDeskUtility.SetParent(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, apiPanel.Handle);
                SLLaserDeskUtility.SendMessage(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WM_SYSCOMMAND, SLLaserDeskUtility.SC_MAXIMIZE, 0);

                stringSentCmd.BindTwoWay(() => _laserDesk.TCPComm.LastSentMessage);
                stringReceivedCmd.BindTwoWay(() => _laserDesk.TCPComm.LastReceivedMessage);


                mcb_WND_OPEN.BindTwoWay(() => _laserDesk.RM_STATE_WND_OPEN);
                mcb_RTC_INIT.BindTwoWay(() => _laserDesk.RM_STATE_RTC_INIT);
                mcb_LAS_INIT.BindTwoWay(() => _laserDesk.RM_STATE_LAS_INIT);
                mcb_MOT_INIT.BindTwoWay(() => _laserDesk.RM_STATE_MOT_INIT);
                mcb_ALL_INIT.BindTwoWay(() => _laserDesk.RM_STATE_ALL_INIT);
                mcb_READY.BindTwoWay(() => _laserDesk.RM_STATE_READY);
                mcb_AUTOMODE.BindTwoWay(() => _laserDesk.RM_STATE_AUTOMODE);
                mcb_LST_EXEC.BindTwoWay(() => _laserDesk.RM_STATE_LST_EXEC);
                mcb_LST_EXE_ERR.BindTwoWay(() => _laserDesk.RM_STATE_LST_EXE_ERR);
                mcb_RM_MODE.BindTwoWay(() => _laserDesk.RM_STATE_RM_MODE);
                mcb_JOB_LOAD.BindTwoWay(() => _laserDesk.RM_STATE_JOB_LOAD);
                mcb_LST_CALC.BindTwoWay(() => _laserDesk.RM_STATE_LST_CALC);
                mcb_CMD_ERR.BindTwoWay(() => _laserDesk.RM_STATE_CMD_ERR);
                mcb_LAS_ERR.BindTwoWay(() => _laserDesk.RM_STATE_LAS_ERR);
                mcb_LAS_ON.BindTwoWay(() => _laserDesk.RM_STATE_LAS_ON);
                mcb_DEV_ERR.BindTwoWay(() => _laserDesk.RM_STATE_DEV_ERR);
                mcb_HEAD_OK.BindTwoWay(() => _laserDesk.RM_STATE_HEAD_OK);
                mcb_EXEC_DONE.BindTwoWay(() => _laserDesk.RM_STATE_EXEC_DONE);
                mcb_PILOT_MODE.BindTwoWay(() => _laserDesk.RM_STATE_PILOT_MODE);
                mcb_JOB_ABORTED.BindTwoWay(() => _laserDesk.RM_STATE_JOB_ABORTED);
                mcb_SWITCH_AUTOMODE.BindTwoWay(() => _laserDesk.RM_STATE_SWITCH_AUTOMODE);
            }
        }


        


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                _laserDesk.Login();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnOpenJobFile_Click(object sender, EventArgs e)
        {

            try
            {
                _laserDesk.OpenLaserJob(_laserDesk.CurrentJobPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Open Laser Job", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        //private void btnTCPDisConn_Click(object sender, EventArgs e)
        //{
        //    _laserDesk.DisconnectLaserDesk();
        //    btnLaserDeskConnect.BackColor = SystemColors.Control;
        //}


        private void btnReOpenLaserDesk_Click(object sender, EventArgs e)
        {
            _laserDesk.DisconnectLaserDesk();
            
            _laserDesk.OpenLaserDesk();
            SLLaserDeskUtility.ShowWindow(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WindowShowStyle.Hide);
            SLLaserDeskUtility.SetParent(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, apiPanel.Handle);
            SLLaserDeskUtility.SendMessage(_laserDesk.SLLaserDeskProcesss.MainWindowHandle, SLLaserDeskUtility.WM_SYSCOMMAND, SLLaserDeskUtility.SC_MAXIMIZE, 0);

            System.Threading.Thread.Sleep(5000);
            _laserDesk.InitialLaserDesk();
            //_laserDesk.AutoConfirmRemoteGUI();
           
        }


        private bool _firstTimeTrigger = true;
        private void BuildIntimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int GWL_STYLE = -16;
                UInt32 WS_POPUP = 0x80000000;
                UInt32 WS_CHILD = 0x40000000;

                IntPtr pMainWindow = Process.GetProcessesByName("SLLaserDesk")[0].MainWindowHandle;

                UInt32 style = SLLaserDeskUtility.GetWindowLong(pMainWindow, GWL_STYLE);
                style = (style | WS_CHILD) & (~WS_POPUP);
                SLLaserDeskUtility.SetWindowLong(pMainWindow, GWL_STYLE, style);
                SLLaserDeskUtility.SetParent(pMainWindow, apiPanel.Handle);


                //Auto Confirm Remote Dialog

                if (_firstTimeTrigger)
                {
                    _laserDesk.InitialLaserDesk();
                    System.Threading.Thread.Sleep(1000);
                    IntPtr hwndRemote = SLLaserDeskUtility.FindWindow(null, "Remote Control");
                    //UInt32 style2 = SLLaserDeskUtility.GetWindowLong(hwndRemote, GWL_STYLE);
                    //style2 = (style2 | WS_CHILD) & (~WS_POPUP);

                    //SLLaserDeskUtility.SetWindowLong(hwndRemote, GWL_STYLE, style2);
                    SLLaserDeskUtility.ShowWindow(hwndRemote, SLLaserDeskUtility.WindowShowStyle.Show);
                    SLLaserDeskUtility.SetParent(hwndRemote, apiPanel.Handle);
                    SLLaserDeskUtility.SendMessage(hwndRemote, SLLaserDeskUtility.WM_SYSCOMMAND, SLLaserDeskUtility.SC_MAXIMIZE, 0);
                    _firstTimeTrigger = false;
                }

            }
            catch
            {

            }
        }

        private void btnProgState_Click(object sender, EventArgs e)
        {
            //int commandNum = 4;
            // _laserDesk.ExecuteCommand(commandNum, null);
            try
            {
                _laserDesk.QueryStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Remote On", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoteOn_Click(object sender, EventArgs e)
        {
            try
            {
                _laserDesk.InitialLaserDesk();
                //_firstTimeTrigger = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Remote On", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnRemoteOff_Click(object sender, EventArgs e)
        {
            try
            {
                _laserDesk.RemoteOffLaserDesk();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Remote Off", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHideRemote_Click(object sender, EventArgs e)
        {
            _firstTimeTrigger = true;
        }

        private void btnSelUID_Click(object sender, EventArgs e)
        {
            try
            {
                _laserDesk.SelectUID(tbUID.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Sect UID",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnLaserStart_Click(object sender, EventArgs e)
        {
            try
            {
                _laserDesk.LaserStart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Laser Start", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _laserDesk.HideLaserDesk();
        }
    }
}
