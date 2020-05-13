namespace MCore.Comp.MotionSystem.Axis
{
    partial class SetupPSOPage
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
            this.gbFirstPSO = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTriggerCommandFirstPSO = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTriggerTypeFirstPSO = new System.Windows.Forms.ComboBox();
            this.buttonApplySetupPSO = new System.Windows.Forms.Button();
            this.gbSetupPSO = new System.Windows.Forms.GroupBox();
            this.gbPSOEncoderTracking = new System.Windows.Forms.GroupBox();
            this.cbTertiaryEncoderPSO = new System.Windows.Forms.ComboBox();
            this.cbSecondaryEncoderPSO = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbPrimaryEncoderPSO = new System.Windows.Forms.ComboBox();
            this.gbSecondPSO = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTriggerCommandSecondPSO = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbTriggerTypeSecondPSO = new System.Windows.Forms.ComboBox();
            this.buttonRefreshSetupPSO = new System.Windows.Forms.Button();
            this.gbFirstPSO.SuspendLayout();
            this.gbSetupPSO.SuspendLayout();
            this.gbPSOEncoderTracking.SuspendLayout();
            this.gbSecondPSO.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFirstPSO
            // 
            this.gbFirstPSO.Controls.Add(this.label2);
            this.gbFirstPSO.Controls.Add(this.tbTriggerCommandFirstPSO);
            this.gbFirstPSO.Controls.Add(this.label1);
            this.gbFirstPSO.Controls.Add(this.cbTriggerTypeFirstPSO);
            this.gbFirstPSO.Location = new System.Drawing.Point(146, 26);
            this.gbFirstPSO.Name = "gbFirstPSO";
            this.gbFirstPSO.Size = new System.Drawing.Size(394, 119);
            this.gbFirstPSO.TabIndex = 0;
            this.gbFirstPSO.TabStop = false;
            this.gbFirstPSO.Text = "First PSO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enter Trigger Command";
            // 
            // tbTriggerCommandFirstPSO
            // 
            this.tbTriggerCommandFirstPSO.Location = new System.Drawing.Point(123, 40);
            this.tbTriggerCommandFirstPSO.Multiline = true;
            this.tbTriggerCommandFirstPSO.Name = "tbTriggerCommandFirstPSO";
            this.tbTriggerCommandFirstPSO.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbTriggerCommandFirstPSO.Size = new System.Drawing.Size(265, 68);
            this.tbTriggerCommandFirstPSO.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Trigger Type";
            // 
            // cbTriggerTypeFirstPSO
            // 
            this.cbTriggerTypeFirstPSO.FormattingEnabled = true;
            this.cbTriggerTypeFirstPSO.Location = new System.Drawing.Point(9, 40);
            this.cbTriggerTypeFirstPSO.Name = "cbTriggerTypeFirstPSO";
            this.cbTriggerTypeFirstPSO.Size = new System.Drawing.Size(108, 21);
            this.cbTriggerTypeFirstPSO.TabIndex = 0;
            // 
            // buttonApplySetupPSO
            // 
            this.buttonApplySetupPSO.Location = new System.Drawing.Point(885, 0);
            this.buttonApplySetupPSO.Name = "buttonApplySetupPSO";
            this.buttonApplySetupPSO.Size = new System.Drawing.Size(55, 20);
            this.buttonApplySetupPSO.TabIndex = 2;
            this.buttonApplySetupPSO.Text = "Apply";
            this.buttonApplySetupPSO.UseVisualStyleBackColor = true;
            this.buttonApplySetupPSO.Click += new System.EventHandler(this.buttonApplySetupPSO_Click);
            // 
            // gbSetupPSO
            // 
            this.gbSetupPSO.Controls.Add(this.buttonRefreshSetupPSO);
            this.gbSetupPSO.Controls.Add(this.gbPSOEncoderTracking);
            this.gbSetupPSO.Controls.Add(this.gbSecondPSO);
            this.gbSetupPSO.Controls.Add(this.gbFirstPSO);
            this.gbSetupPSO.Controls.Add(this.buttonApplySetupPSO);
            this.gbSetupPSO.Location = new System.Drawing.Point(3, 17);
            this.gbSetupPSO.Name = "gbSetupPSO";
            this.gbSetupPSO.Size = new System.Drawing.Size(950, 152);
            this.gbSetupPSO.TabIndex = 1;
            this.gbSetupPSO.TabStop = false;
            this.gbSetupPSO.Text = "Setup PSO";
            // 
            // gbPSOEncoderTracking
            // 
            this.gbPSOEncoderTracking.Controls.Add(this.cbTertiaryEncoderPSO);
            this.gbPSOEncoderTracking.Controls.Add(this.cbSecondaryEncoderPSO);
            this.gbPSOEncoderTracking.Controls.Add(this.label5);
            this.gbPSOEncoderTracking.Controls.Add(this.cbPrimaryEncoderPSO);
            this.gbPSOEncoderTracking.Location = new System.Drawing.Point(6, 26);
            this.gbPSOEncoderTracking.Name = "gbPSOEncoderTracking";
            this.gbPSOEncoderTracking.Size = new System.Drawing.Size(134, 119);
            this.gbPSOEncoderTracking.TabIndex = 4;
            this.gbPSOEncoderTracking.TabStop = false;
            this.gbPSOEncoderTracking.Text = "PSO Encoder Tracking";
            // 
            // cbTertiaryEncoderPSO
            // 
            this.cbTertiaryEncoderPSO.FormattingEnabled = true;
            this.cbTertiaryEncoderPSO.Location = new System.Drawing.Point(6, 87);
            this.cbTertiaryEncoderPSO.Name = "cbTertiaryEncoderPSO";
            this.cbTertiaryEncoderPSO.Size = new System.Drawing.Size(108, 21);
            this.cbTertiaryEncoderPSO.TabIndex = 5;
            // 
            // cbSecondaryEncoderPSO
            // 
            this.cbSecondaryEncoderPSO.FormattingEnabled = true;
            this.cbSecondaryEncoderPSO.Location = new System.Drawing.Point(6, 64);
            this.cbSecondaryEncoderPSO.Name = "cbSecondaryEncoderPSO";
            this.cbSecondaryEncoderPSO.Size = new System.Drawing.Size(108, 21);
            this.cbSecondaryEncoderPSO.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Select Encoder Track";
            // 
            // cbPrimaryEncoderPSO
            // 
            this.cbPrimaryEncoderPSO.FormattingEnabled = true;
            this.cbPrimaryEncoderPSO.Location = new System.Drawing.Point(6, 41);
            this.cbPrimaryEncoderPSO.Name = "cbPrimaryEncoderPSO";
            this.cbPrimaryEncoderPSO.Size = new System.Drawing.Size(108, 21);
            this.cbPrimaryEncoderPSO.TabIndex = 2;
            // 
            // gbSecondPSO
            // 
            this.gbSecondPSO.Controls.Add(this.label3);
            this.gbSecondPSO.Controls.Add(this.tbTriggerCommandSecondPSO);
            this.gbSecondPSO.Controls.Add(this.label4);
            this.gbSecondPSO.Controls.Add(this.cbTriggerTypeSecondPSO);
            this.gbSecondPSO.Location = new System.Drawing.Point(546, 26);
            this.gbSecondPSO.Name = "gbSecondPSO";
            this.gbSecondPSO.Size = new System.Drawing.Size(394, 119);
            this.gbSecondPSO.TabIndex = 3;
            this.gbSecondPSO.TabStop = false;
            this.gbSecondPSO.Text = "Second PSO";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter Trigger Command";
            // 
            // tbTriggerCommandSecondPSO
            // 
            this.tbTriggerCommandSecondPSO.Location = new System.Drawing.Point(123, 40);
            this.tbTriggerCommandSecondPSO.Multiline = true;
            this.tbTriggerCommandSecondPSO.Name = "tbTriggerCommandSecondPSO";
            this.tbTriggerCommandSecondPSO.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbTriggerCommandSecondPSO.Size = new System.Drawing.Size(265, 68);
            this.tbTriggerCommandSecondPSO.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Select Trigger Type";
            // 
            // cbTriggerTypeSecondPSO
            // 
            this.cbTriggerTypeSecondPSO.FormattingEnabled = true;
            this.cbTriggerTypeSecondPSO.Location = new System.Drawing.Point(9, 40);
            this.cbTriggerTypeSecondPSO.Name = "cbTriggerTypeSecondPSO";
            this.cbTriggerTypeSecondPSO.Size = new System.Drawing.Size(108, 21);
            this.cbTriggerTypeSecondPSO.TabIndex = 0;
            // 
            // buttonRefreshSetupPSO
            // 
            this.buttonRefreshSetupPSO.Location = new System.Drawing.Point(814, 0);
            this.buttonRefreshSetupPSO.Name = "buttonRefreshSetupPSO";
            this.buttonRefreshSetupPSO.Size = new System.Drawing.Size(55, 20);
            this.buttonRefreshSetupPSO.TabIndex = 5;
            this.buttonRefreshSetupPSO.Text = "Refresh";
            this.buttonRefreshSetupPSO.UseVisualStyleBackColor = true;
            this.buttonRefreshSetupPSO.Click += new System.EventHandler(this.buttonRefreshSetupPSO_Click);
            // 
            // SetupPSOPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSetupPSO);
            this.Name = "SetupPSOPage";
            this.Size = new System.Drawing.Size(961, 185);
            this.gbFirstPSO.ResumeLayout(false);
            this.gbFirstPSO.PerformLayout();
            this.gbSetupPSO.ResumeLayout(false);
            this.gbPSOEncoderTracking.ResumeLayout(false);
            this.gbPSOEncoderTracking.PerformLayout();
            this.gbSecondPSO.ResumeLayout(false);
            this.gbSecondPSO.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFirstPSO;
        private System.Windows.Forms.ComboBox cbTriggerTypeFirstPSO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonApplySetupPSO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTriggerCommandFirstPSO;
        private System.Windows.Forms.GroupBox gbSetupPSO;
        private System.Windows.Forms.GroupBox gbSecondPSO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTriggerCommandSecondPSO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbTriggerTypeSecondPSO;
        private System.Windows.Forms.GroupBox gbPSOEncoderTracking;
        private System.Windows.Forms.ComboBox cbTertiaryEncoderPSO;
        private System.Windows.Forms.ComboBox cbSecondaryEncoderPSO;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbPrimaryEncoderPSO;
        private System.Windows.Forms.Button buttonRefreshSetupPSO;

    }
}
