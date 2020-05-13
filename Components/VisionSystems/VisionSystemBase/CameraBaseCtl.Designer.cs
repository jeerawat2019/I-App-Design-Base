namespace MCore.Comp.VisionSystem
{
    partial class CameraBaseCtl
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
            this.cbCamera = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVideoFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.panelCamera = new MCore.Comp.VisionSystem.CamPanel();
            this.cbLive = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbCamera
            // 
            this.cbCamera.FormattingEnabled = true;
            this.cbCamera.Location = new System.Drawing.Point(70, 9);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(412, 21);
            this.cbCamera.TabIndex = 0;
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.OnChangeCamera);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Camera";
            // 
            // cbVideoFormat
            // 
            this.cbVideoFormat.FormattingEnabled = true;
            this.cbVideoFormat.Location = new System.Drawing.Point(85, 42);
            this.cbVideoFormat.Name = "cbVideoFormat";
            this.cbVideoFormat.Size = new System.Drawing.Size(397, 21);
            this.cbVideoFormat.TabIndex = 2;
            this.cbVideoFormat.SelectedIndexChanged += new System.EventHandler(this.OnChangeVideoFormat);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Video Format";
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(524, 6);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(117, 27);
            this.btnInitialize.TabIndex = 4;
            this.btnInitialize.Text = "Initialize";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // panelCamera
            // 
            this.panelCamera.Location = new System.Drawing.Point(6, 78);
            this.panelCamera.Name = "panelCamera";
            this.panelCamera.Size = new System.Drawing.Size(649, 469);
            this.panelCamera.TabIndex = 5;
            // 
            // cbLive
            // 
            this.cbLive.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbLive.Location = new System.Drawing.Point(524, 39);
            this.cbLive.Name = "cbLive";
            this.cbLive.Size = new System.Drawing.Size(114, 31);
            this.cbLive.TabIndex = 6;
            this.cbLive.Text = "Live";
            this.cbLive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbLive.UseVisualStyleBackColor = true;
            this.cbLive.CheckedChanged += new System.EventHandler(this.OnLiveClicked);
            // 
            // CameraBaseCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbLive);
            this.Controls.Add(this.panelCamera);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbVideoFormat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCamera);
            this.Name = "CameraBaseCtl";
            this.Size = new System.Drawing.Size(662, 553);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCamera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbVideoFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInitialize;
        private MCore.Comp.VisionSystem.CamPanel panelCamera;
        private System.Windows.Forms.CheckBox cbLive;
    }
}
