namespace MCore.Controls
{
    partial class MDoubleWithUnits
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
            if (disposing)
            {
                UnBind();
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.lblName = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.lblUnits = new System.Windows.Forms.Label();
            this.panelRightControls = new System.Windows.Forms.Panel();
            this.panelRightControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.Location = new System.Drawing.Point(2, 2);
            this.lblName.Margin = new System.Windows.Forms.Padding(2);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(92, 18);
            this.lblName.TabIndex = 0;
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbValue
            // 
            this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbValue.Location = new System.Drawing.Point(0, 0);
            this.tbValue.Multiline = true;
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(50, 18);
            this.tbValue.TabIndex = 1;
            this.tbValue.WordWrap = false;
            this.tbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.tbValue.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidating);
            this.tbValue.Validated += new System.EventHandler(this.OnValidated);
            // 
            // lblUnits
            // 
            this.lblUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnits.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUnits.Location = new System.Drawing.Point(50, 0);
            this.lblUnits.Name = "lblUnits";
            this.lblUnits.Size = new System.Drawing.Size(40, 18);
            this.lblUnits.TabIndex = 2;
            this.lblUnits.Text = "Unit";
            this.lblUnits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelRightControls
            // 
            this.panelRightControls.Controls.Add(this.tbValue);
            this.panelRightControls.Controls.Add(this.lblUnits);
            this.panelRightControls.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightControls.Location = new System.Drawing.Point(94, 2);
            this.panelRightControls.Margin = new System.Windows.Forms.Padding(2);
            this.panelRightControls.Name = "panelRightControls";
            this.panelRightControls.Size = new System.Drawing.Size(94, 18);
            this.panelRightControls.TabIndex = 3;
            // 
            // MDoubleWithUnits
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.panelRightControls);
            this.Name = "MDoubleWithUnits";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(190, 22);
            this.panelRightControls.ResumeLayout(false);
            this.panelRightControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label lblUnits;
        private System.Windows.Forms.Panel panelRightControls;
    }
}
