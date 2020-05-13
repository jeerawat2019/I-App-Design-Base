namespace MCore.Comp.VisionSystem
{
    partial class CameraPropertiesCtl
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
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.tbExposure = new MCore.Controls.MDoubleCtl();
            this.tbContrast = new MCore.Controls.MDoubleCtl();
            this.tbBrightness = new MCore.Controls.MDoubleCtl();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPortDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSerialNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCameraName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCamera.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.tbExposure);
            this.gbCamera.Controls.Add(this.tbContrast);
            this.gbCamera.Controls.Add(this.tbBrightness);
            this.gbCamera.Controls.Add(this.tbID);
            this.gbCamera.Controls.Add(this.label4);
            this.gbCamera.Controls.Add(this.tbPortDescription);
            this.gbCamera.Controls.Add(this.label3);
            this.gbCamera.Controls.Add(this.tbSerialNumber);
            this.gbCamera.Controls.Add(this.label2);
            this.gbCamera.Controls.Add(this.tbCameraName);
            this.gbCamera.Controls.Add(this.label1);
            this.gbCamera.Location = new System.Drawing.Point(0, 0);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(274, 210);
            this.gbCamera.TabIndex = 1;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "Camera Name";
            // 
            // tbExposure
            // 
            this.tbExposure.Label = "";
            this.tbExposure.Location = new System.Drawing.Point(9, 182);
            this.tbExposure.LogChanges = true;
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(175, 22);
            this.tbExposure.TabIndex = 16;
            // 
            // tbContrast
            // 
            this.tbContrast.Label = "";
            this.tbContrast.Location = new System.Drawing.Point(9, 156);
            this.tbContrast.LogChanges = true;
            this.tbContrast.Name = "tbContrast";
            this.tbContrast.Size = new System.Drawing.Size(175, 22);
            this.tbContrast.TabIndex = 15;
            // 
            // tbBrightness
            // 
            this.tbBrightness.Label = "";
            this.tbBrightness.Location = new System.Drawing.Point(9, 128);
            this.tbBrightness.LogChanges = true;
            this.tbBrightness.Name = "tbBrightness";
            this.tbBrightness.Size = new System.Drawing.Size(175, 22);
            this.tbBrightness.TabIndex = 14;
            // 
            // tbID
            // 
            this.tbID.Location = new System.Drawing.Point(91, 102);
            this.tbID.Name = "tbID";
            this.tbID.ReadOnly = true;
            this.tbID.Size = new System.Drawing.Size(177, 20);
            this.tbID.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "ID";
            // 
            // tbPortDescription
            // 
            this.tbPortDescription.Location = new System.Drawing.Point(91, 76);
            this.tbPortDescription.Name = "tbPortDescription";
            this.tbPortDescription.ReadOnly = true;
            this.tbPortDescription.Size = new System.Drawing.Size(177, 20);
            this.tbPortDescription.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port info";
            // 
            // tbSerialNumber
            // 
            this.tbSerialNumber.Location = new System.Drawing.Point(91, 50);
            this.tbSerialNumber.Name = "tbSerialNumber";
            this.tbSerialNumber.ReadOnly = true;
            this.tbSerialNumber.Size = new System.Drawing.Size(177, 20);
            this.tbSerialNumber.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Serial Number";
            // 
            // tbCameraName
            // 
            this.tbCameraName.Location = new System.Drawing.Point(55, 24);
            this.tbCameraName.Name = "tbCameraName";
            this.tbCameraName.ReadOnly = true;
            this.tbCameraName.Size = new System.Drawing.Size(213, 20);
            this.tbCameraName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera";
            // 
            // CameraPropertiesCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbCamera);
            this.Name = "CameraPropertiesCtl";
            this.Size = new System.Drawing.Size(274, 210);
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCamera;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPortDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSerialNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCameraName;
        private System.Windows.Forms.Label label1;
        private Controls.MDoubleCtl tbExposure;
        private Controls.MDoubleCtl tbContrast;
        private Controls.MDoubleCtl tbBrightness;
    }
}
