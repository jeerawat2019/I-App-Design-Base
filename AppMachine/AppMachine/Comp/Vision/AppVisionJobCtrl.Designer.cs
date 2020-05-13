namespace AppMachine.Comp.Vision
{
    partial class AppVisionJobCtrl
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
            this.lbFilePath = new System.Windows.Forms.Label();
            this.panelVisionJob = new System.Windows.Forms.Panel();
            this.strVisionFilePath = new MCore.Controls.StringCtl();
            this.SuspendLayout();
            // 
            // lbFilePath
            // 
            this.lbFilePath.AutoSize = true;
            this.lbFilePath.ForeColor = System.Drawing.Color.Blue;
            this.lbFilePath.Location = new System.Drawing.Point(3, 6);
            this.lbFilePath.Name = "lbFilePath";
            this.lbFilePath.Size = new System.Drawing.Size(79, 13);
            this.lbFilePath.TabIndex = 21;
            this.lbFilePath.Text = "Vision File Path";
            // 
            // panelVisionJob
            // 
            this.panelVisionJob.Location = new System.Drawing.Point(3, 25);
            this.panelVisionJob.Name = "panelVisionJob";
            this.panelVisionJob.Size = new System.Drawing.Size(726, 309);
            this.panelVisionJob.TabIndex = 20;
            // 
            // strVisionFilePath
            // 
            this.strVisionFilePath.Location = new System.Drawing.Point(89, 3);
            this.strVisionFilePath.LogChanges = true;
            this.strVisionFilePath.Name = "strVisionFilePath";
            this.strVisionFilePath.ReadOnly = true;
            this.strVisionFilePath.Size = new System.Drawing.Size(637, 20);
            this.strVisionFilePath.TabIndex = 22;
            // 
            // AppVisionJobCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.strVisionFilePath);
            this.Controls.Add(this.lbFilePath);
            this.Controls.Add(this.panelVisionJob);
            this.Name = "AppVisionJobCtrl";
            this.Size = new System.Drawing.Size(732, 338);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelVisionJob;
        private System.Windows.Forms.Label lbFilePath;
        private MCore.Controls.StringCtl strVisionFilePath;
    }
}
