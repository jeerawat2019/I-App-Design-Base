namespace AppMachine.Panel
{
    partial class AppMachineSetupPanel
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
            this.tcMachineSetup = new System.Windows.Forms.TabControl();
            this.tpFeedSetup = new System.Windows.Forms.TabPage();
            this.tpVisionSetup = new System.Windows.Forms.TabPage();
            this.tpSemiAutoOpr = new System.Windows.Forms.TabPage();
            this.tcMachineSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMachineSetup
            // 
            this.tcMachineSetup.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tcMachineSetup.Controls.Add(this.tpFeedSetup);
            this.tcMachineSetup.Controls.Add(this.tpVisionSetup);
            this.tcMachineSetup.Controls.Add(this.tpSemiAutoOpr);
            this.tcMachineSetup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMachineSetup.Location = new System.Drawing.Point(0, 0);
            this.tcMachineSetup.Multiline = true;
            this.tcMachineSetup.Name = "tcMachineSetup";
            this.tcMachineSetup.SelectedIndex = 0;
            this.tcMachineSetup.Size = new System.Drawing.Size(1256, 906);
            this.tcMachineSetup.TabIndex = 0;
            // 
            // tpFeedSetup
            // 
            this.tpFeedSetup.Location = new System.Drawing.Point(23, 4);
            this.tpFeedSetup.Name = "tpFeedSetup";
            this.tpFeedSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tpFeedSetup.Size = new System.Drawing.Size(1229, 898);
            this.tpFeedSetup.TabIndex = 0;
            this.tpFeedSetup.Text = "Feed Setup";
            this.tpFeedSetup.UseVisualStyleBackColor = true;
            // 
            // tpVisionSetup
            // 
            this.tpVisionSetup.Location = new System.Drawing.Point(23, 4);
            this.tpVisionSetup.Name = "tpVisionSetup";
            this.tpVisionSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tpVisionSetup.Size = new System.Drawing.Size(1229, 898);
            this.tpVisionSetup.TabIndex = 1;
            this.tpVisionSetup.Text = "Vision Setup";
            this.tpVisionSetup.UseVisualStyleBackColor = true;
            // 
            // tpSemiAutoOpr
            // 
            this.tpSemiAutoOpr.Location = new System.Drawing.Point(23, 4);
            this.tpSemiAutoOpr.Name = "tpSemiAutoOpr";
            this.tpSemiAutoOpr.Padding = new System.Windows.Forms.Padding(3);
            this.tpSemiAutoOpr.Size = new System.Drawing.Size(1229, 898);
            this.tpSemiAutoOpr.TabIndex = 2;
            this.tpSemiAutoOpr.Text = "Semi Auto Operation";
            this.tpSemiAutoOpr.UseVisualStyleBackColor = true;
            // 
            // AppMachineSetupPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.tcMachineSetup);
            this.Name = "AppMachineSetupPanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.tcMachineSetup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMachineSetup;
        private System.Windows.Forms.TabPage tpFeedSetup;
        private System.Windows.Forms.TabPage tpVisionSetup;
        private System.Windows.Forms.TabPage tpSemiAutoOpr;

    }
}
