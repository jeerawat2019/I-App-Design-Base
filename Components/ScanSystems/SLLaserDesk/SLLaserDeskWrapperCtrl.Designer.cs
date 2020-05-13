namespace MCore.Comp.ScanSystem
{
    public partial class SLLaserDeskWrapperCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //_laserDesk.UnregisterApiPanel();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.apiPanel = new System.Windows.Forms.Panel();
            this.btnReOpenLaserDesk = new System.Windows.Forms.Button();
            this.BuildIntimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnOpenJobFile = new System.Windows.Forms.Button();
            this.btnProgState = new System.Windows.Forms.Button();
            this.btnRemoteOn = new System.Windows.Forms.Button();
            this.btnRemoteOff = new System.Windows.Forms.Button();
            this.btnHideRemote = new System.Windows.Forms.Button();
            this.browseForJobFile = new MCore.Controls.BrowseForFile();
            this.stringSentCmd = new MCore.Controls.StringCtl();
            this.stringReceivedCmd = new MCore.Controls.StringCtl();
            this.mcb_WND_OPEN = new MCore.Controls.MCheckBox();
            this.mcb_RTC_INIT = new MCore.Controls.MCheckBox();
            this.mcb_LAS_INIT = new MCore.Controls.MCheckBox();
            this.mcb_MOT_INIT = new MCore.Controls.MCheckBox();
            this.mcb_ALL_INIT = new MCore.Controls.MCheckBox();
            this.mcb_READY = new MCore.Controls.MCheckBox();
            this.mcb_AUTOMODE = new MCore.Controls.MCheckBox();
            this.mcb_LST_EXEC = new MCore.Controls.MCheckBox();
            this.mcb_LST_EXE_ERR = new MCore.Controls.MCheckBox();
            this.mcb_RM_MODE = new MCore.Controls.MCheckBox();
            this.mcb_JOB_LOAD = new MCore.Controls.MCheckBox();
            this.mcb_LST_CALC = new MCore.Controls.MCheckBox();
            this.mcb_CMD_ERR = new MCore.Controls.MCheckBox();
            this.mcb_LAS_ERR = new MCore.Controls.MCheckBox();
            this.mcb_LAS_ON = new MCore.Controls.MCheckBox();
            this.mcb_DEV_ERR = new MCore.Controls.MCheckBox();
            this.mcb_HEAD_OK = new MCore.Controls.MCheckBox();
            this.mcb_EXEC_DONE = new MCore.Controls.MCheckBox();
            this.mcb_PILOT_MODE = new MCore.Controls.MCheckBox();
            this.mcb_JOB_ABORTED = new MCore.Controls.MCheckBox();
            this.mcb_SWITCH_AUTOMODE = new MCore.Controls.MCheckBox();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.btnSelUID = new System.Windows.Forms.Button();
            this.btnLaserStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // apiPanel
            // 
            this.apiPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.apiPanel.AutoScroll = true;
            this.apiPanel.BackColor = System.Drawing.Color.Silver;
            this.apiPanel.Location = new System.Drawing.Point(4, 4);
            this.apiPanel.Name = "apiPanel";
            this.apiPanel.Size = new System.Drawing.Size(549, 387);
            this.apiPanel.TabIndex = 1;
            // 
            // btnReOpenLaserDesk
            // 
            this.btnReOpenLaserDesk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReOpenLaserDesk.Location = new System.Drawing.Point(558, 366);
            this.btnReOpenLaserDesk.Name = "btnReOpenLaserDesk";
            this.btnReOpenLaserDesk.Size = new System.Drawing.Size(63, 23);
            this.btnReOpenLaserDesk.TabIndex = 8;
            this.btnReOpenLaserDesk.Text = "Re-Open Laser Desk";
            this.btnReOpenLaserDesk.UseVisualStyleBackColor = true;
            this.btnReOpenLaserDesk.Visible = false;
            this.btnReOpenLaserDesk.Click += new System.EventHandler(this.btnReOpenLaserDesk_Click);
            // 
            // BuildIntimer
            // 
            this.BuildIntimer.Enabled = true;
            this.BuildIntimer.Interval = 1000;
            this.BuildIntimer.Tick += new System.EventHandler(this.BuildIntimer_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 453);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sent Cmd";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 476);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Return Cmd";
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLogin.Location = new System.Drawing.Point(387, 422);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(49, 23);
            this.btnLogin.TabIndex = 12;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Visible = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnOpenJobFile
            // 
            this.btnOpenJobFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenJobFile.Location = new System.Drawing.Point(611, 395);
            this.btnOpenJobFile.Name = "btnOpenJobFile";
            this.btnOpenJobFile.Size = new System.Drawing.Size(78, 23);
            this.btnOpenJobFile.TabIndex = 14;
            this.btnOpenJobFile.Text = "Open Job";
            this.btnOpenJobFile.UseVisualStyleBackColor = true;
            this.btnOpenJobFile.Click += new System.EventHandler(this.btnOpenJobFile_Click);
            // 
            // btnProgState
            // 
            this.btnProgState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnProgState.Location = new System.Drawing.Point(4, 422);
            this.btnProgState.Name = "btnProgState";
            this.btnProgState.Size = new System.Drawing.Size(66, 23);
            this.btnProgState.TabIndex = 15;
            this.btnProgState.Text = "Prog State";
            this.btnProgState.UseVisualStyleBackColor = true;
            this.btnProgState.Click += new System.EventHandler(this.btnProgState_Click);
            // 
            // btnRemoteOn
            // 
            this.btnRemoteOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoteOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnRemoteOn.Location = new System.Drawing.Point(442, 422);
            this.btnRemoteOn.Name = "btnRemoteOn";
            this.btnRemoteOn.Size = new System.Drawing.Size(78, 23);
            this.btnRemoteOn.TabIndex = 16;
            this.btnRemoteOn.Text = "Remote On";
            this.btnRemoteOn.UseVisualStyleBackColor = false;
            this.btnRemoteOn.Click += new System.EventHandler(this.btnRemoteOn_Click);
            // 
            // btnRemoteOff
            // 
            this.btnRemoteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoteOff.BackColor = System.Drawing.Color.Black;
            this.btnRemoteOff.ForeColor = System.Drawing.Color.White;
            this.btnRemoteOff.Location = new System.Drawing.Point(611, 422);
            this.btnRemoteOff.Name = "btnRemoteOff";
            this.btnRemoteOff.Size = new System.Drawing.Size(78, 23);
            this.btnRemoteOff.TabIndex = 17;
            this.btnRemoteOff.Text = "Remote Off";
            this.btnRemoteOff.UseVisualStyleBackColor = false;
            this.btnRemoteOff.Click += new System.EventHandler(this.btnRemoteOff_Click);
            // 
            // btnHideRemote
            // 
            this.btnHideRemote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHideRemote.Location = new System.Drawing.Point(526, 422);
            this.btnHideRemote.Name = "btnHideRemote";
            this.btnHideRemote.Size = new System.Drawing.Size(78, 23);
            this.btnHideRemote.TabIndex = 18;
            this.btnHideRemote.Text = "Hide Remote";
            this.btnHideRemote.UseVisualStyleBackColor = true;
            this.btnHideRemote.Click += new System.EventHandler(this.btnHideRemote_Click);
            // 
            // browseForJobFile
            // 
            this.browseForJobFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.browseForJobFile.Location = new System.Drawing.Point(4, 397);
            this.browseForJobFile.LogChanges = true;
            this.browseForJobFile.Name = "browseForJobFile";
            this.browseForJobFile.Size = new System.Drawing.Size(582, 20);
            this.browseForJobFile.TabIndex = 13;
            // 
            // stringSentCmd
            // 
            this.stringSentCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stringSentCmd.Location = new System.Drawing.Point(82, 450);
            this.stringSentCmd.LogChanges = true;
            this.stringSentCmd.Name = "stringSentCmd";
            this.stringSentCmd.ReadOnly = true;
            this.stringSentCmd.Size = new System.Drawing.Size(607, 20);
            this.stringSentCmd.TabIndex = 19;
            // 
            // stringReceivedCmd
            // 
            this.stringReceivedCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stringReceivedCmd.Location = new System.Drawing.Point(82, 473);
            this.stringReceivedCmd.LogChanges = true;
            this.stringReceivedCmd.Name = "stringReceivedCmd";
            this.stringReceivedCmd.ReadOnly = true;
            this.stringReceivedCmd.Size = new System.Drawing.Size(607, 20);
            this.stringReceivedCmd.TabIndex = 20;
            // 
            // mcb_WND_OPEN
            // 
            this.mcb_WND_OPEN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_WND_OPEN.AutoSize = true;
            this.mcb_WND_OPEN.Location = new System.Drawing.Point(558, 3);
            this.mcb_WND_OPEN.LogChanges = true;
            this.mcb_WND_OPEN.Name = "mcb_WND_OPEN";
            this.mcb_WND_OPEN.Size = new System.Drawing.Size(89, 17);
            this.mcb_WND_OPEN.TabIndex = 21;
            this.mcb_WND_OPEN.Text = "WND_OPEN";
            this.mcb_WND_OPEN.UseVisualStyleBackColor = true;
            // 
            // mcb_RTC_INIT
            // 
            this.mcb_RTC_INIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_RTC_INIT.AutoSize = true;
            this.mcb_RTC_INIT.Location = new System.Drawing.Point(558, 20);
            this.mcb_RTC_INIT.LogChanges = true;
            this.mcb_RTC_INIT.Name = "mcb_RTC_INIT";
            this.mcb_RTC_INIT.Size = new System.Drawing.Size(75, 17);
            this.mcb_RTC_INIT.TabIndex = 22;
            this.mcb_RTC_INIT.Text = "RTC_INIT";
            this.mcb_RTC_INIT.UseVisualStyleBackColor = true;
            // 
            // mcb_LAS_INIT
            // 
            this.mcb_LAS_INIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LAS_INIT.AutoSize = true;
            this.mcb_LAS_INIT.Location = new System.Drawing.Point(558, 37);
            this.mcb_LAS_INIT.LogChanges = true;
            this.mcb_LAS_INIT.Name = "mcb_LAS_INIT";
            this.mcb_LAS_INIT.Size = new System.Drawing.Size(73, 17);
            this.mcb_LAS_INIT.TabIndex = 23;
            this.mcb_LAS_INIT.Text = "LAS_INIT";
            this.mcb_LAS_INIT.UseVisualStyleBackColor = true;
            // 
            // mcb_MOT_INIT
            // 
            this.mcb_MOT_INIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_MOT_INIT.AutoSize = true;
            this.mcb_MOT_INIT.Location = new System.Drawing.Point(558, 54);
            this.mcb_MOT_INIT.LogChanges = true;
            this.mcb_MOT_INIT.Name = "mcb_MOT_INIT";
            this.mcb_MOT_INIT.Size = new System.Drawing.Size(77, 17);
            this.mcb_MOT_INIT.TabIndex = 24;
            this.mcb_MOT_INIT.Text = "MOT_INIT";
            this.mcb_MOT_INIT.UseVisualStyleBackColor = true;
            // 
            // mcb_ALL_INIT
            // 
            this.mcb_ALL_INIT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_ALL_INIT.AutoSize = true;
            this.mcb_ALL_INIT.Location = new System.Drawing.Point(558, 71);
            this.mcb_ALL_INIT.LogChanges = true;
            this.mcb_ALL_INIT.Name = "mcb_ALL_INIT";
            this.mcb_ALL_INIT.Size = new System.Drawing.Size(72, 17);
            this.mcb_ALL_INIT.TabIndex = 25;
            this.mcb_ALL_INIT.Text = "ALL_INIT";
            this.mcb_ALL_INIT.UseVisualStyleBackColor = true;
            // 
            // mcb_READY
            // 
            this.mcb_READY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_READY.AutoSize = true;
            this.mcb_READY.Location = new System.Drawing.Point(558, 88);
            this.mcb_READY.LogChanges = true;
            this.mcb_READY.Name = "mcb_READY";
            this.mcb_READY.Size = new System.Drawing.Size(63, 17);
            this.mcb_READY.TabIndex = 26;
            this.mcb_READY.Text = "READY";
            this.mcb_READY.UseVisualStyleBackColor = true;
            // 
            // mcb_AUTOMODE
            // 
            this.mcb_AUTOMODE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_AUTOMODE.AutoSize = true;
            this.mcb_AUTOMODE.Location = new System.Drawing.Point(558, 105);
            this.mcb_AUTOMODE.LogChanges = true;
            this.mcb_AUTOMODE.Name = "mcb_AUTOMODE";
            this.mcb_AUTOMODE.Size = new System.Drawing.Size(88, 17);
            this.mcb_AUTOMODE.TabIndex = 27;
            this.mcb_AUTOMODE.Text = "AUTOMODE";
            this.mcb_AUTOMODE.UseVisualStyleBackColor = true;
            // 
            // mcb_LST_EXEC
            // 
            this.mcb_LST_EXEC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LST_EXEC.AutoSize = true;
            this.mcb_LST_EXEC.Location = new System.Drawing.Point(558, 122);
            this.mcb_LST_EXEC.LogChanges = true;
            this.mcb_LST_EXEC.Name = "mcb_LST_EXEC";
            this.mcb_LST_EXEC.Size = new System.Drawing.Size(80, 17);
            this.mcb_LST_EXEC.TabIndex = 28;
            this.mcb_LST_EXEC.Text = "LST_EXEC";
            this.mcb_LST_EXEC.UseVisualStyleBackColor = true;
            // 
            // mcb_LST_EXE_ERR
            // 
            this.mcb_LST_EXE_ERR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LST_EXE_ERR.AutoSize = true;
            this.mcb_LST_EXE_ERR.Location = new System.Drawing.Point(558, 139);
            this.mcb_LST_EXE_ERR.LogChanges = true;
            this.mcb_LST_EXE_ERR.Name = "mcb_LST_EXE_ERR";
            this.mcb_LST_EXE_ERR.Size = new System.Drawing.Size(102, 17);
            this.mcb_LST_EXE_ERR.TabIndex = 29;
            this.mcb_LST_EXE_ERR.Text = "LST_EXE_ERR";
            this.mcb_LST_EXE_ERR.UseVisualStyleBackColor = true;
            // 
            // mcb_RM_MODE
            // 
            this.mcb_RM_MODE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_RM_MODE.AutoSize = true;
            this.mcb_RM_MODE.Location = new System.Drawing.Point(558, 156);
            this.mcb_RM_MODE.LogChanges = true;
            this.mcb_RM_MODE.Name = "mcb_RM_MODE";
            this.mcb_RM_MODE.Size = new System.Drawing.Size(81, 17);
            this.mcb_RM_MODE.TabIndex = 30;
            this.mcb_RM_MODE.Text = "RM_MODE";
            this.mcb_RM_MODE.UseVisualStyleBackColor = true;
            // 
            // mcb_JOB_LOAD
            // 
            this.mcb_JOB_LOAD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_JOB_LOAD.AutoSize = true;
            this.mcb_JOB_LOAD.Location = new System.Drawing.Point(558, 173);
            this.mcb_JOB_LOAD.LogChanges = true;
            this.mcb_JOB_LOAD.Name = "mcb_JOB_LOAD";
            this.mcb_JOB_LOAD.Size = new System.Drawing.Size(81, 17);
            this.mcb_JOB_LOAD.TabIndex = 31;
            this.mcb_JOB_LOAD.Text = "JOB_LOAD";
            this.mcb_JOB_LOAD.UseVisualStyleBackColor = true;
            // 
            // mcb_LST_CALC
            // 
            this.mcb_LST_CALC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LST_CALC.AutoSize = true;
            this.mcb_LST_CALC.Location = new System.Drawing.Point(558, 190);
            this.mcb_LST_CALC.LogChanges = true;
            this.mcb_LST_CALC.Name = "mcb_LST_CALC";
            this.mcb_LST_CALC.Size = new System.Drawing.Size(79, 17);
            this.mcb_LST_CALC.TabIndex = 32;
            this.mcb_LST_CALC.Text = "LST_CALC";
            this.mcb_LST_CALC.UseVisualStyleBackColor = true;
            // 
            // mcb_CMD_ERR
            // 
            this.mcb_CMD_ERR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_CMD_ERR.AutoSize = true;
            this.mcb_CMD_ERR.Location = new System.Drawing.Point(558, 207);
            this.mcb_CMD_ERR.LogChanges = true;
            this.mcb_CMD_ERR.Name = "mcb_CMD_ERR";
            this.mcb_CMD_ERR.Size = new System.Drawing.Size(79, 17);
            this.mcb_CMD_ERR.TabIndex = 33;
            this.mcb_CMD_ERR.Text = "CMD_ERR";
            this.mcb_CMD_ERR.UseVisualStyleBackColor = true;
            // 
            // mcb_LAS_ERR
            // 
            this.mcb_LAS_ERR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LAS_ERR.AutoSize = true;
            this.mcb_LAS_ERR.Location = new System.Drawing.Point(558, 224);
            this.mcb_LAS_ERR.LogChanges = true;
            this.mcb_LAS_ERR.Name = "mcb_LAS_ERR";
            this.mcb_LAS_ERR.Size = new System.Drawing.Size(75, 17);
            this.mcb_LAS_ERR.TabIndex = 34;
            this.mcb_LAS_ERR.Text = "LAS_ERR";
            this.mcb_LAS_ERR.UseVisualStyleBackColor = true;
            // 
            // mcb_LAS_ON
            // 
            this.mcb_LAS_ON.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_LAS_ON.AutoSize = true;
            this.mcb_LAS_ON.Location = new System.Drawing.Point(558, 241);
            this.mcb_LAS_ON.LogChanges = true;
            this.mcb_LAS_ON.Name = "mcb_LAS_ON";
            this.mcb_LAS_ON.Size = new System.Drawing.Size(68, 17);
            this.mcb_LAS_ON.TabIndex = 35;
            this.mcb_LAS_ON.Text = "LAS_ON";
            this.mcb_LAS_ON.UseVisualStyleBackColor = true;
            // 
            // mcb_DEV_ERR
            // 
            this.mcb_DEV_ERR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_DEV_ERR.AutoSize = true;
            this.mcb_DEV_ERR.Location = new System.Drawing.Point(558, 258);
            this.mcb_DEV_ERR.LogChanges = true;
            this.mcb_DEV_ERR.Name = "mcb_DEV_ERR";
            this.mcb_DEV_ERR.Size = new System.Drawing.Size(77, 17);
            this.mcb_DEV_ERR.TabIndex = 36;
            this.mcb_DEV_ERR.Text = "DEV_ERR";
            this.mcb_DEV_ERR.UseVisualStyleBackColor = true;
            // 
            // mcb_HEAD_OK
            // 
            this.mcb_HEAD_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_HEAD_OK.AutoSize = true;
            this.mcb_HEAD_OK.Location = new System.Drawing.Point(558, 275);
            this.mcb_HEAD_OK.LogChanges = true;
            this.mcb_HEAD_OK.Name = "mcb_HEAD_OK";
            this.mcb_HEAD_OK.Size = new System.Drawing.Size(77, 17);
            this.mcb_HEAD_OK.TabIndex = 37;
            this.mcb_HEAD_OK.Text = "HEAD_OK";
            this.mcb_HEAD_OK.UseVisualStyleBackColor = true;
            // 
            // mcb_EXEC_DONE
            // 
            this.mcb_EXEC_DONE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_EXEC_DONE.AutoSize = true;
            this.mcb_EXEC_DONE.Location = new System.Drawing.Point(558, 292);
            this.mcb_EXEC_DONE.LogChanges = true;
            this.mcb_EXEC_DONE.Name = "mcb_EXEC_DONE";
            this.mcb_EXEC_DONE.Size = new System.Drawing.Size(91, 17);
            this.mcb_EXEC_DONE.TabIndex = 38;
            this.mcb_EXEC_DONE.Text = "EXEC_DONE";
            this.mcb_EXEC_DONE.UseVisualStyleBackColor = true;
            // 
            // mcb_PILOT_MODE
            // 
            this.mcb_PILOT_MODE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_PILOT_MODE.AutoSize = true;
            this.mcb_PILOT_MODE.Location = new System.Drawing.Point(558, 309);
            this.mcb_PILOT_MODE.LogChanges = true;
            this.mcb_PILOT_MODE.Name = "mcb_PILOT_MODE";
            this.mcb_PILOT_MODE.Size = new System.Drawing.Size(95, 17);
            this.mcb_PILOT_MODE.TabIndex = 39;
            this.mcb_PILOT_MODE.Text = "PILOT_MODE";
            this.mcb_PILOT_MODE.UseVisualStyleBackColor = true;
            // 
            // mcb_JOB_ABORTED
            // 
            this.mcb_JOB_ABORTED.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_JOB_ABORTED.AutoSize = true;
            this.mcb_JOB_ABORTED.Location = new System.Drawing.Point(558, 326);
            this.mcb_JOB_ABORTED.LogChanges = true;
            this.mcb_JOB_ABORTED.Name = "mcb_JOB_ABORTED";
            this.mcb_JOB_ABORTED.Size = new System.Drawing.Size(104, 17);
            this.mcb_JOB_ABORTED.TabIndex = 40;
            this.mcb_JOB_ABORTED.Text = "JOB_ABORTED";
            this.mcb_JOB_ABORTED.UseVisualStyleBackColor = true;
            // 
            // mcb_SWITCH_AUTOMODE
            // 
            this.mcb_SWITCH_AUTOMODE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mcb_SWITCH_AUTOMODE.AutoSize = true;
            this.mcb_SWITCH_AUTOMODE.Location = new System.Drawing.Point(558, 343);
            this.mcb_SWITCH_AUTOMODE.LogChanges = true;
            this.mcb_SWITCH_AUTOMODE.Name = "mcb_SWITCH_AUTOMODE";
            this.mcb_SWITCH_AUTOMODE.Size = new System.Drawing.Size(105, 17);
            this.mcb_SWITCH_AUTOMODE.TabIndex = 41;
            this.mcb_SWITCH_AUTOMODE.Text = "SWITCH_AUTO";
            this.mcb_SWITCH_AUTOMODE.UseVisualStyleBackColor = true;
            // 
            // tbUID
            // 
            this.tbUID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbUID.Location = new System.Drawing.Point(73, 423);
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(74, 20);
            this.tbUID.TabIndex = 42;
            // 
            // btnSelUID
            // 
            this.btnSelUID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelUID.Location = new System.Drawing.Point(153, 421);
            this.btnSelUID.Name = "btnSelUID";
            this.btnSelUID.Size = new System.Drawing.Size(58, 23);
            this.btnSelUID.TabIndex = 43;
            this.btnSelUID.Text = "Sel UID";
            this.btnSelUID.UseVisualStyleBackColor = true;
            this.btnSelUID.Click += new System.EventHandler(this.btnSelUID_Click);
            // 
            // btnLaserStart
            // 
            this.btnLaserStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLaserStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLaserStart.Location = new System.Drawing.Point(217, 421);
            this.btnLaserStart.Name = "btnLaserStart";
            this.btnLaserStart.Size = new System.Drawing.Size(66, 23);
            this.btnLaserStart.TabIndex = 44;
            this.btnLaserStart.Text = "Laser Start";
            this.btnLaserStart.UseVisualStyleBackColor = false;
            this.btnLaserStart.Click += new System.EventHandler(this.btnLaserStart_Click);
            // 
            // SLLaserDeskWrapperCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLaserStart);
            this.Controls.Add(this.btnSelUID);
            this.Controls.Add(this.tbUID);
            this.Controls.Add(this.mcb_SWITCH_AUTOMODE);
            this.Controls.Add(this.mcb_JOB_ABORTED);
            this.Controls.Add(this.mcb_PILOT_MODE);
            this.Controls.Add(this.mcb_EXEC_DONE);
            this.Controls.Add(this.mcb_HEAD_OK);
            this.Controls.Add(this.mcb_DEV_ERR);
            this.Controls.Add(this.mcb_LAS_ON);
            this.Controls.Add(this.mcb_LAS_ERR);
            this.Controls.Add(this.mcb_CMD_ERR);
            this.Controls.Add(this.mcb_LST_CALC);
            this.Controls.Add(this.mcb_JOB_LOAD);
            this.Controls.Add(this.mcb_RM_MODE);
            this.Controls.Add(this.mcb_LST_EXE_ERR);
            this.Controls.Add(this.mcb_LST_EXEC);
            this.Controls.Add(this.mcb_AUTOMODE);
            this.Controls.Add(this.mcb_READY);
            this.Controls.Add(this.mcb_ALL_INIT);
            this.Controls.Add(this.mcb_MOT_INIT);
            this.Controls.Add(this.mcb_LAS_INIT);
            this.Controls.Add(this.mcb_RTC_INIT);
            this.Controls.Add(this.mcb_WND_OPEN);
            this.Controls.Add(this.stringReceivedCmd);
            this.Controls.Add(this.stringSentCmd);
            this.Controls.Add(this.btnHideRemote);
            this.Controls.Add(this.btnRemoteOff);
            this.Controls.Add(this.btnRemoteOn);
            this.Controls.Add(this.btnProgState);
            this.Controls.Add(this.btnOpenJobFile);
            this.Controls.Add(this.browseForJobFile);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReOpenLaserDesk);
            this.Controls.Add(this.apiPanel);
            this.Name = "SLLaserDeskWrapperCtrl";
            this.Size = new System.Drawing.Size(692, 499);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel apiPanel;
        private System.Windows.Forms.Button btnReOpenLaserDesk;
        private System.Windows.Forms.Timer BuildIntimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnOpenJobFile;
        private MCore.Controls.BrowseForFile browseForJobFile;
        private System.Windows.Forms.Button btnProgState;
        private System.Windows.Forms.Button btnRemoteOn;
        private System.Windows.Forms.Button btnRemoteOff;
        private System.Windows.Forms.Button btnHideRemote;
        private Controls.StringCtl stringSentCmd;
        private Controls.StringCtl stringReceivedCmd;
        private Controls.MCheckBox mcb_WND_OPEN;
        private Controls.MCheckBox mcb_RTC_INIT;
        private Controls.MCheckBox mcb_LAS_INIT;
        private Controls.MCheckBox mcb_MOT_INIT;
        private Controls.MCheckBox mcb_ALL_INIT;
        private Controls.MCheckBox mcb_READY;
        private Controls.MCheckBox mcb_AUTOMODE;
        private Controls.MCheckBox mcb_LST_EXEC;
        private Controls.MCheckBox mcb_LST_EXE_ERR;
        private Controls.MCheckBox mcb_RM_MODE;
        private Controls.MCheckBox mcb_JOB_LOAD;
        private Controls.MCheckBox mcb_LST_CALC;
        private Controls.MCheckBox mcb_CMD_ERR;
        private Controls.MCheckBox mcb_LAS_ERR;
        private Controls.MCheckBox mcb_LAS_ON;
        private Controls.MCheckBox mcb_DEV_ERR;
        private Controls.MCheckBox mcb_HEAD_OK;
        private Controls.MCheckBox mcb_EXEC_DONE;
        private Controls.MCheckBox mcb_PILOT_MODE;
        private Controls.MCheckBox mcb_JOB_ABORTED;
        private Controls.MCheckBox mcb_SWITCH_AUTOMODE;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.Button btnSelUID;
        private System.Windows.Forms.Button btnLaserStart;
    }
}
