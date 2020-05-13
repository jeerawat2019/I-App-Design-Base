namespace AppMachine.Comp.Vision
{
    partial class AppVisionCameraCtrl
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
            this.cbLive = new System.Windows.Forms.CheckBox();
            this.panelCameraProperty = new System.Windows.Forms.Panel();
            this.camPanel = new MCore.Comp.VisionSystem.CamPanel();
            this.cbCrossHair = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbLive
            // 
            this.cbLive.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cbLive.Location = new System.Drawing.Point(290, 454);
            this.cbLive.Name = "cbLive";
            this.cbLive.Size = new System.Drawing.Size(198, 168);
            this.cbLive.TabIndex = 21;
            this.cbLive.Text = "LIVE VIEW";
            this.cbLive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbLive.UseVisualStyleBackColor = true;
            // 
            // panelCameraProperty
            // 
            this.panelCameraProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCameraProperty.BackColor = System.Drawing.SystemColors.Control;
            this.panelCameraProperty.Location = new System.Drawing.Point(5, 405);
            this.panelCameraProperty.Name = "panelCameraProperty";
            this.panelCameraProperty.Size = new System.Drawing.Size(279, 217);
            this.panelCameraProperty.TabIndex = 22;
            // 
            // camPanel
            // 
            this.camPanel.BackColor = System.Drawing.Color.Navy;
            this.camPanel.Location = new System.Drawing.Point(3, 3);
            this.camPanel.Name = "camPanel";
            this.camPanel.Size = new System.Drawing.Size(485, 399);
            this.camPanel.TabIndex = 15;
            // 
            // cbCrossHair
            // 
            this.cbCrossHair.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbCrossHair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.cbCrossHair.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCrossHair.Location = new System.Drawing.Point(290, 405);
            this.cbCrossHair.Name = "cbCrossHair";
            this.cbCrossHair.Size = new System.Drawing.Size(198, 43);
            this.cbCrossHair.TabIndex = 23;
            this.cbCrossHair.Text = "CROSS HAIR";
            this.cbCrossHair.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbCrossHair.UseVisualStyleBackColor = false;
            this.cbCrossHair.CheckedChanged += new System.EventHandler(this.cbCrossHair_CheckedChanged);
            // 
            // AppVisionCameraCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbCrossHair);
            this.Controls.Add(this.cbLive);
            this.Controls.Add(this.panelCameraProperty);
            this.Controls.Add(this.camPanel);
            this.Name = "AppVisionCameraCtrl";
            this.Size = new System.Drawing.Size(493, 625);
            this.ResumeLayout(false);

        }

        #endregion

        private MCore.Comp.VisionSystem.CamPanel camPanel;
        private System.Windows.Forms.CheckBox cbLive;
        private System.Windows.Forms.Panel panelCameraProperty;
        private System.Windows.Forms.CheckBox cbCrossHair;
    }
}
