namespace AppMachine.Panel.SubPanel
{
    partial class AppSubPanelBase
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
            this.panelIOStatus = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelIOStatus
            // 
            this.panelIOStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIOStatus.Location = new System.Drawing.Point(3, 3);
            this.panelIOStatus.Name = "panelIOStatus";
            this.panelIOStatus.Size = new System.Drawing.Size(831, 212);
            this.panelIOStatus.TabIndex = 0;
            // 
            // AppSubPanelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelIOStatus);
            this.Name = "AppSubPanelBase";
            this.Size = new System.Drawing.Size(1229, 898);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelIOStatus;


    }
}
