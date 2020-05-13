namespace MCore.Comp.VisionSystem
{
    partial class CognexCamPage
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
                _camera.UnregisterCameraWindow(panelCamera);

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
            this.panelCamera = new System.Windows.Forms.Panel();
            this.mDoubleUnitsAquireTime = new MCore.Controls.MDoubleWithUnits();
            this.mDoubleBrightness = new MCore.Controls.MDoubleCtl();
            this.mDoubleContrast = new MCore.Controls.MDoubleCtl();
            this.mDoubleUnitsExposure = new MCore.Controls.MDoubleWithUnits();
            this.triggerMode = new MCore.Comp.IOSystem.CompMeasureCtl();
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
            this.btnInitialize.Location = new System.Drawing.Point(520, 9);
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
            this.panelCamera.Size = new System.Drawing.Size(476, 319);
            this.panelCamera.TabIndex = 5;
            // 
            // mDoubleUnitsAquireTime
            // 
            this.mDoubleUnitsAquireTime.Label = "";
            this.mDoubleUnitsAquireTime.Location = new System.Drawing.Point(494, 332);
            this.mDoubleUnitsAquireTime.Name = "mDoubleUnitsAquireTime";
            this.mDoubleUnitsAquireTime.Size = new System.Drawing.Size(170, 22);
            this.mDoubleUnitsAquireTime.TabIndex = 10;
            // 
            // mDoubleBrightness
            // 
            this.mDoubleBrightness.Label = "";
            this.mDoubleBrightness.Location = new System.Drawing.Point(494, 248);
            this.mDoubleBrightness.Name = "mDoubleBrightness";
            this.mDoubleBrightness.Size = new System.Drawing.Size(170, 22);
            this.mDoubleBrightness.TabIndex = 7;
            // 
            // mDoubleContrast
            // 
            this.mDoubleContrast.Label = "";
            this.mDoubleContrast.Location = new System.Drawing.Point(494, 304);
            this.mDoubleContrast.Name = "mDoubleContrast";
            this.mDoubleContrast.Size = new System.Drawing.Size(170, 22);
            this.mDoubleContrast.TabIndex = 8;
            // 
            // mDoubleUnitsExposure
            // 
            this.mDoubleUnitsExposure.Label = "";
            this.mDoubleUnitsExposure.Location = new System.Drawing.Point(494, 276);
            this.mDoubleUnitsExposure.Name = "mDoubleUnitsExposure";
            this.mDoubleUnitsExposure.Size = new System.Drawing.Size(170, 22);
            this.mDoubleUnitsExposure.TabIndex = 12;
            // 
            // triggerMode
            // 
            this.triggerMode.Location = new System.Drawing.Point(494, 78);
            this.triggerMode.Name = "triggerMode";
            this.triggerMode.Size = new System.Drawing.Size(182, 154);
            this.triggerMode.TabIndex = 13;
            // 
            // CognexCamPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.triggerMode);
            this.Controls.Add(this.mDoubleUnitsExposure);
            this.Controls.Add(this.mDoubleUnitsAquireTime);
            this.Controls.Add(this.mDoubleContrast);
            this.Controls.Add(this.mDoubleBrightness);
            this.Controls.Add(this.panelCamera);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbVideoFormat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCamera);
            this.Name = "CognexCamPage";
            this.Size = new System.Drawing.Size(679, 418);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCamera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbVideoFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Panel panelCamera;
        private Controls.MDoubleCtl mDoubleBrightness;
        private Controls.MDoubleWithUnits mDoubleUnitsAquireTime;
        private Controls.MDoubleCtl mDoubleContrast;
        private Controls.MDoubleWithUnits mDoubleUnitsExposure;
        private MCore.Comp.IOSystem.CompMeasureCtl triggerMode;
    }
}
