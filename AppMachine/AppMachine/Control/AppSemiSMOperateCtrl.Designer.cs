namespace AppMachine.Control
{
    partial class AppSemiSMOperateCtrl
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
            this.flpSemiAutoOpr = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLblFloat = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // flpSemiAutoOpr
            // 
            this.flpSemiAutoOpr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpSemiAutoOpr.AutoScroll = true;
            this.flpSemiAutoOpr.AutoSize = true;
            this.flpSemiAutoOpr.BackColor = System.Drawing.Color.Silver;
            this.flpSemiAutoOpr.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSemiAutoOpr.Location = new System.Drawing.Point(3, 3);
            this.flpSemiAutoOpr.Name = "flpSemiAutoOpr";
            this.flpSemiAutoOpr.Size = new System.Drawing.Size(874, 510);
            this.flpSemiAutoOpr.TabIndex = 5;
            this.flpSemiAutoOpr.WrapContents = false;
            // 
            // linkLblFloat
            // 
            this.linkLblFloat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLblFloat.AutoSize = true;
            this.linkLblFloat.Location = new System.Drawing.Point(881, 3);
            this.linkLblFloat.Name = "linkLblFloat";
            this.linkLblFloat.Size = new System.Drawing.Size(30, 13);
            this.linkLblFloat.TabIndex = 13;
            this.linkLblFloat.TabStop = true;
            this.linkLblFloat.Text = "Float";
            this.linkLblFloat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblFloat_LinkClicked);
            // 
            // AppSemiSMOperateCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.linkLblFloat);
            this.Controls.Add(this.flpSemiAutoOpr);
            this.Name = "AppSemiSMOperateCtrl";
            this.Size = new System.Drawing.Size(915, 516);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpSemiAutoOpr;
        private System.Windows.Forms.LinkLabel linkLblFloat;
    }
}
