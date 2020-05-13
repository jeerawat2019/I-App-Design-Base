namespace MCore.Comp.MotionSystem
{
    partial class AeroTechV_XXPage
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
            this.gbAerotechControler = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbInitialCommands = new MCore.Controls.StringCtl();
            this.buttonAbortMotion = new System.Windows.Forms.Button();
            this.buttonAchnowledgeFault = new System.Windows.Forms.Button();
            this.cbResetControler = new System.Windows.Forms.CheckBox();
            this.cbConnectControler = new System.Windows.Forms.CheckBox();
            this.gbTasks = new System.Windows.Forms.GroupBox();
            this.buttonUpdateTaskStatus = new System.Windows.Forms.Button();
            this.tbStatusTask = new System.Windows.Forms.TextBox();
            this.gbGlobal = new System.Windows.Forms.GroupBox();
            this.buttonUpdateGlobalDouble = new System.Windows.Forms.Button();
            this.lbGlobalDouble = new System.Windows.Forms.ListBox();
            this.gbTask = new System.Windows.Forms.GroupBox();
            this.buttonExecuteCommandOnTask = new System.Windows.Forms.Button();
            this.tbCommandList = new System.Windows.Forms.TextBox();
            this.comboBoxSelectTask = new System.Windows.Forms.ComboBox();
            this.buttonUpdateTaskDouble = new System.Windows.Forms.Button();
            this.lbTaskDouble = new System.Windows.Forms.ListBox();
            this.gbAerotechControler.SuspendLayout();
            this.gbTasks.SuspendLayout();
            this.gbGlobal.SuspendLayout();
            this.gbTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbAerotechControler
            // 
            this.gbAerotechControler.Controls.Add(this.label1);
            this.gbAerotechControler.Controls.Add(this.tbInitialCommands);
            this.gbAerotechControler.Controls.Add(this.buttonAbortMotion);
            this.gbAerotechControler.Controls.Add(this.buttonAchnowledgeFault);
            this.gbAerotechControler.Controls.Add(this.cbResetControler);
            this.gbAerotechControler.Controls.Add(this.cbConnectControler);
            this.gbAerotechControler.Location = new System.Drawing.Point(8, 3);
            this.gbAerotechControler.Name = "gbAerotechControler";
            this.gbAerotechControler.Size = new System.Drawing.Size(399, 439);
            this.gbAerotechControler.TabIndex = 1;
            this.gbAerotechControler.TabStop = false;
            this.gbAerotechControler.Text = "Aerotech Controler";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Initialize Commands";
            // 
            // tbInitialCommands
            // 
            this.tbInitialCommands.AcceptsReturn = true;
            this.tbInitialCommands.Location = new System.Drawing.Point(6, 122);
            this.tbInitialCommands.LogChanges = true;
            this.tbInitialCommands.Multiline = true;
            this.tbInitialCommands.Name = "tbInitialCommands";
            this.tbInitialCommands.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInitialCommands.Size = new System.Drawing.Size(386, 311);
            this.tbInitialCommands.TabIndex = 6;
            this.tbInitialCommands.WordWrap = false;
            // 
            // buttonAbortMotion
            // 
            this.buttonAbortMotion.Location = new System.Drawing.Point(119, 60);
            this.buttonAbortMotion.Name = "buttonAbortMotion";
            this.buttonAbortMotion.Size = new System.Drawing.Size(77, 24);
            this.buttonAbortMotion.TabIndex = 3;
            this.buttonAbortMotion.Text = "Abort Motion";
            this.buttonAbortMotion.UseVisualStyleBackColor = true;
            this.buttonAbortMotion.Click += new System.EventHandler(this.buttonAbortMotion_Click);
            // 
            // buttonAchnowledgeFault
            // 
            this.buttonAchnowledgeFault.Location = new System.Drawing.Point(6, 60);
            this.buttonAchnowledgeFault.Name = "buttonAchnowledgeFault";
            this.buttonAchnowledgeFault.Size = new System.Drawing.Size(107, 24);
            this.buttonAchnowledgeFault.TabIndex = 2;
            this.buttonAchnowledgeFault.Text = "Acknowledge Fault";
            this.buttonAchnowledgeFault.UseVisualStyleBackColor = true;
            this.buttonAchnowledgeFault.Click += new System.EventHandler(this.buttonAcknowledgeFault_Click);
            // 
            // cbResetControler
            // 
            this.cbResetControler.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbResetControler.Location = new System.Drawing.Point(202, 29);
            this.cbResetControler.Name = "cbResetControler";
            this.cbResetControler.Size = new System.Drawing.Size(190, 25);
            this.cbResetControler.TabIndex = 1;
            this.cbResetControler.Text = "Reset Controler";
            this.cbResetControler.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbResetControler.UseVisualStyleBackColor = true;
            this.cbResetControler.CheckedChanged += new System.EventHandler(this.OnReset);
            // 
            // cbConnectControler
            // 
            this.cbConnectControler.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbConnectControler.Location = new System.Drawing.Point(6, 29);
            this.cbConnectControler.Name = "cbConnectControler";
            this.cbConnectControler.Size = new System.Drawing.Size(190, 25);
            this.cbConnectControler.TabIndex = 0;
            this.cbConnectControler.Text = "Connect Controler";
            this.cbConnectControler.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbConnectControler.UseVisualStyleBackColor = true;
            this.cbConnectControler.CheckedChanged += new System.EventHandler(this.OnConnect);
            // 
            // gbTasks
            // 
            this.gbTasks.Controls.Add(this.buttonUpdateTaskStatus);
            this.gbTasks.Controls.Add(this.tbStatusTask);
            this.gbTasks.Location = new System.Drawing.Point(429, 3);
            this.gbTasks.Name = "gbTasks";
            this.gbTasks.Size = new System.Drawing.Size(202, 211);
            this.gbTasks.TabIndex = 2;
            this.gbTasks.TabStop = false;
            this.gbTasks.Text = "Tasks Status";
            // 
            // buttonUpdateTaskStatus
            // 
            this.buttonUpdateTaskStatus.Location = new System.Drawing.Point(6, 29);
            this.buttonUpdateTaskStatus.Name = "buttonUpdateTaskStatus";
            this.buttonUpdateTaskStatus.Size = new System.Drawing.Size(190, 23);
            this.buttonUpdateTaskStatus.TabIndex = 4;
            this.buttonUpdateTaskStatus.Text = "Update Task Status";
            this.buttonUpdateTaskStatus.UseVisualStyleBackColor = true;
            this.buttonUpdateTaskStatus.Click += new System.EventHandler(this.buttonUpdateTaskStatus_Click);
            // 
            // tbStatusTask
            // 
            this.tbStatusTask.Location = new System.Drawing.Point(6, 58);
            this.tbStatusTask.Multiline = true;
            this.tbStatusTask.Name = "tbStatusTask";
            this.tbStatusTask.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbStatusTask.Size = new System.Drawing.Size(190, 147);
            this.tbStatusTask.TabIndex = 0;
            this.tbStatusTask.Text = "Task Status";
            // 
            // gbGlobal
            // 
            this.gbGlobal.Controls.Add(this.buttonUpdateGlobalDouble);
            this.gbGlobal.Controls.Add(this.lbGlobalDouble);
            this.gbGlobal.Location = new System.Drawing.Point(637, 3);
            this.gbGlobal.Name = "gbGlobal";
            this.gbGlobal.Size = new System.Drawing.Size(202, 211);
            this.gbGlobal.TabIndex = 3;
            this.gbGlobal.TabStop = false;
            this.gbGlobal.Text = "Global Double";
            // 
            // buttonUpdateGlobalDouble
            // 
            this.buttonUpdateGlobalDouble.Location = new System.Drawing.Point(7, 29);
            this.buttonUpdateGlobalDouble.Name = "buttonUpdateGlobalDouble";
            this.buttonUpdateGlobalDouble.Size = new System.Drawing.Size(190, 23);
            this.buttonUpdateGlobalDouble.TabIndex = 3;
            this.buttonUpdateGlobalDouble.Text = "Update Global Double";
            this.buttonUpdateGlobalDouble.UseVisualStyleBackColor = true;
            this.buttonUpdateGlobalDouble.Click += new System.EventHandler(this.buttonUpdateGlobalDouble_Click);
            // 
            // lbGlobalDouble
            // 
            this.lbGlobalDouble.FormattingEnabled = true;
            this.lbGlobalDouble.Location = new System.Drawing.Point(6, 58);
            this.lbGlobalDouble.Name = "lbGlobalDouble";
            this.lbGlobalDouble.Size = new System.Drawing.Size(190, 147);
            this.lbGlobalDouble.TabIndex = 2;
            // 
            // gbTask
            // 
            this.gbTask.Controls.Add(this.buttonExecuteCommandOnTask);
            this.gbTask.Controls.Add(this.tbCommandList);
            this.gbTask.Controls.Add(this.comboBoxSelectTask);
            this.gbTask.Controls.Add(this.buttonUpdateTaskDouble);
            this.gbTask.Controls.Add(this.lbTaskDouble);
            this.gbTask.Location = new System.Drawing.Point(429, 220);
            this.gbTask.Name = "gbTask";
            this.gbTask.Size = new System.Drawing.Size(410, 222);
            this.gbTask.TabIndex = 4;
            this.gbTask.TabStop = false;
            this.gbTask.Text = "Task Command";
            // 
            // buttonExecuteCommandOnTask
            // 
            this.buttonExecuteCommandOnTask.Location = new System.Drawing.Point(5, 193);
            this.buttonExecuteCommandOnTask.Name = "buttonExecuteCommandOnTask";
            this.buttonExecuteCommandOnTask.Size = new System.Drawing.Size(191, 23);
            this.buttonExecuteCommandOnTask.TabIndex = 6;
            this.buttonExecuteCommandOnTask.Text = "Execute Command";
            this.buttonExecuteCommandOnTask.UseVisualStyleBackColor = true;
            this.buttonExecuteCommandOnTask.Click += new System.EventHandler(this.buttonExecuteCommandOnTask_Click);
            // 
            // tbCommandList
            // 
            this.tbCommandList.AcceptsReturn = true;
            this.tbCommandList.Location = new System.Drawing.Point(6, 27);
            this.tbCommandList.Multiline = true;
            this.tbCommandList.Name = "tbCommandList";
            this.tbCommandList.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbCommandList.Size = new System.Drawing.Size(190, 160);
            this.tbCommandList.TabIndex = 5;
            this.tbCommandList.Text = "DWELL 1";
            this.tbCommandList.WordWrap = false;
            // 
            // comboBoxSelectTask
            // 
            this.comboBoxSelectTask.FormattingEnabled = true;
            this.comboBoxSelectTask.Location = new System.Drawing.Point(335, 0);
            this.comboBoxSelectTask.Name = "comboBoxSelectTask";
            this.comboBoxSelectTask.Size = new System.Drawing.Size(70, 21);
            this.comboBoxSelectTask.TabIndex = 4;
            this.comboBoxSelectTask.Text = "Task.T02";
            this.comboBoxSelectTask.SelectedIndexChanged += new System.EventHandler(this.OnSelectedTaskChange);
            // 
            // buttonUpdateTaskDouble
            // 
            this.buttonUpdateTaskDouble.Location = new System.Drawing.Point(214, 27);
            this.buttonUpdateTaskDouble.Name = "buttonUpdateTaskDouble";
            this.buttonUpdateTaskDouble.Size = new System.Drawing.Size(191, 23);
            this.buttonUpdateTaskDouble.TabIndex = 3;
            this.buttonUpdateTaskDouble.Text = "Update Task Double";
            this.buttonUpdateTaskDouble.UseVisualStyleBackColor = true;
            this.buttonUpdateTaskDouble.Click += new System.EventHandler(this.buttonUpdateTaskDouble_Click);
            // 
            // lbTaskDouble
            // 
            this.lbTaskDouble.FormattingEnabled = true;
            this.lbTaskDouble.Location = new System.Drawing.Point(214, 56);
            this.lbTaskDouble.Name = "lbTaskDouble";
            this.lbTaskDouble.Size = new System.Drawing.Size(191, 160);
            this.lbTaskDouble.TabIndex = 2;
            // 
            // AeroTechV_XXPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTask);
            this.Controls.Add(this.gbGlobal);
            this.Controls.Add(this.gbTasks);
            this.Controls.Add(this.gbAerotechControler);
            this.Name = "AeroTechV_XXPage";
            this.Size = new System.Drawing.Size(844, 445);
            this.gbAerotechControler.ResumeLayout(false);
            this.gbAerotechControler.PerformLayout();
            this.gbTasks.ResumeLayout(false);
            this.gbTasks.PerformLayout();
            this.gbGlobal.ResumeLayout(false);
            this.gbTask.ResumeLayout(false);
            this.gbTask.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbAerotechControler;
        private System.Windows.Forms.CheckBox cbConnectControler;
        private System.Windows.Forms.GroupBox gbTasks;
        private System.Windows.Forms.TextBox tbStatusTask;
        private System.Windows.Forms.CheckBox cbResetControler;
        private System.Windows.Forms.GroupBox gbGlobal;
        private System.Windows.Forms.ListBox lbGlobalDouble;
        private System.Windows.Forms.Button buttonUpdateGlobalDouble;
        private System.Windows.Forms.GroupBox gbTask;
        private System.Windows.Forms.ComboBox comboBoxSelectTask;
        private System.Windows.Forms.Button buttonUpdateTaskDouble;
        private System.Windows.Forms.ListBox lbTaskDouble;
        private System.Windows.Forms.Button buttonExecuteCommandOnTask;
        private System.Windows.Forms.TextBox tbCommandList;
        private System.Windows.Forms.Button buttonAchnowledgeFault;
        private System.Windows.Forms.Button buttonUpdateTaskStatus;
        private System.Windows.Forms.Button buttonAbortMotion;
        private System.Windows.Forms.Label label1;
        private MCore.Controls.StringCtl tbInitialCommands;
    }
}
