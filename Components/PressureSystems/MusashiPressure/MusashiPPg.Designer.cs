namespace MCore.Comp.PressureSystem
{
    partial class MusashiPPg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUploadResponse = new System.Windows.Forms.Label();
            this.tbUploadCommand = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lblDownloadResponse = new System.Windows.Forms.Label();
            this.tbDownloadCommand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mdPressure = new MCore.Controls.MDoubleWithUnits();
            this.mdDispenseTime = new MCore.Controls.MDoubleWithUnits();
            this.mdVacuum = new MCore.Controls.MDoubleWithUnits();
            this.mchTimedMode = new MCore.Controls.MCheckBox();
            this.strChannel = new MCore.Controls.StringCtl();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetPressure = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "UploadCommand";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(376, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Reponse";
            // 
            // lblUploadResponse
            // 
            this.lblUploadResponse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUploadResponse.Location = new System.Drawing.Point(293, 88);
            this.lblUploadResponse.Name = "lblUploadResponse";
            this.lblUploadResponse.Size = new System.Drawing.Size(291, 27);
            this.lblUploadResponse.TabIndex = 2;
            this.lblUploadResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbUploadCommand
            // 
            this.tbUploadCommand.Location = new System.Drawing.Point(116, 90);
            this.tbUploadCommand.Name = "tbUploadCommand";
            this.tbUploadCommand.Size = new System.Drawing.Size(97, 20);
            this.tbUploadCommand.TabIndex = 3;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(219, 47);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(68, 23);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownloadExecute_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(219, 90);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(68, 23);
            this.btnUpload.TabIndex = 5;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblDownloadResponse
            // 
            this.lblDownloadResponse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDownloadResponse.Location = new System.Drawing.Point(293, 45);
            this.lblDownloadResponse.Name = "lblDownloadResponse";
            this.lblDownloadResponse.Size = new System.Drawing.Size(291, 27);
            this.lblDownloadResponse.TabIndex = 6;
            this.lblDownloadResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbDownloadCommand
            // 
            this.tbDownloadCommand.Location = new System.Drawing.Point(116, 50);
            this.tbDownloadCommand.Name = "tbDownloadCommand";
            this.tbDownloadCommand.Size = new System.Drawing.Size(97, 20);
            this.tbDownloadCommand.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "DownloadCommand";
            // 
            // mdPressure
            // 
            this.mdPressure.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdPressure.DoubleVal = 0D;
            this.mdPressure.Enabled = false;
            this.mdPressure.Label = "Pressure";
            this.mdPressure.Location = new System.Drawing.Point(19, 44);
            this.mdPressure.LogChanges = true;
            this.mdPressure.Name = "mdPressure";
            this.mdPressure.Padding = new System.Windows.Forms.Padding(2);
            this.mdPressure.Size = new System.Drawing.Size(190, 22);
            this.mdPressure.TabIndex = 9;
            this.mdPressure.TextBackColor = System.Drawing.SystemColors.Window;
            this.mdPressure.UnitsLabel = "Unit";
            // 
            // mdDispenseTime
            // 
            this.mdDispenseTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdDispenseTime.DoubleVal = 0D;
            this.mdDispenseTime.Enabled = false;
            this.mdDispenseTime.Label = "Dispense Time";
            this.mdDispenseTime.Location = new System.Drawing.Point(19, 72);
            this.mdDispenseTime.LogChanges = true;
            this.mdDispenseTime.Name = "mdDispenseTime";
            this.mdDispenseTime.Padding = new System.Windows.Forms.Padding(2);
            this.mdDispenseTime.Size = new System.Drawing.Size(190, 22);
            this.mdDispenseTime.TabIndex = 10;
            this.mdDispenseTime.TextBackColor = System.Drawing.SystemColors.Window;
            this.mdDispenseTime.UnitsLabel = "Unit";
            // 
            // mdVacuum
            // 
            this.mdVacuum.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mdVacuum.DoubleVal = 0D;
            this.mdVacuum.Enabled = false;
            this.mdVacuum.Label = "Vacuum";
            this.mdVacuum.Location = new System.Drawing.Point(19, 100);
            this.mdVacuum.LogChanges = true;
            this.mdVacuum.Name = "mdVacuum";
            this.mdVacuum.Padding = new System.Windows.Forms.Padding(2);
            this.mdVacuum.Size = new System.Drawing.Size(190, 22);
            this.mdVacuum.TabIndex = 11;
            this.mdVacuum.TextBackColor = System.Drawing.SystemColors.Window;
            this.mdVacuum.UnitsLabel = "Unit";
            // 
            // mchTimedMode
            // 
            this.mchTimedMode.AutoSize = true;
            this.mchTimedMode.Enabled = false;
            this.mchTimedMode.Location = new System.Drawing.Point(113, 128);
            this.mchTimedMode.LogChanges = true;
            this.mchTimedMode.Name = "mchTimedMode";
            this.mchTimedMode.Size = new System.Drawing.Size(85, 17);
            this.mchTimedMode.TabIndex = 12;
            this.mchTimedMode.Text = "Timed Mode";
            this.mchTimedMode.UseVisualStyleBackColor = true;
            // 
            // strChannel
            // 
            this.strChannel.Enabled = false;
            this.strChannel.Location = new System.Drawing.Point(114, 18);
            this.strChannel.LogChanges = true;
            this.strChannel.Name = "strChannel";
            this.strChannel.Size = new System.Drawing.Size(96, 20);
            this.strChannel.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Channel";
            // 
            // btnSetPressure
            // 
            this.btnSetPressure.Location = new System.Drawing.Point(216, 44);
            this.btnSetPressure.Name = "btnSetPressure";
            this.btnSetPressure.Size = new System.Drawing.Size(106, 23);
            this.btnSetPressure.TabIndex = 15;
            this.btnSetPressure.Text = "Set Pressure";
            this.btnSetPressure.UseVisualStyleBackColor = true;
            this.btnSetPressure.Click += new System.EventHandler(this.btnSetPressure_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateAll);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.mdDispenseTime);
            this.groupBox1.Controls.Add(this.strChannel);
            this.groupBox1.Controls.Add(this.btnSetPressure);
            this.groupBox1.Controls.Add(this.mdPressure);
            this.groupBox1.Controls.Add(this.mdVacuum);
            this.groupBox1.Controls.Add(this.mchTimedMode);
            this.groupBox1.Location = new System.Drawing.Point(27, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 169);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Location = new System.Drawing.Point(216, 15);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(106, 23);
            this.btnUpdateAll.TabIndex = 16;
            this.btnUpdateAll.Text = "Update All";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // MusashiPPg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDownloadCommand);
            this.Controls.Add(this.lblDownloadResponse);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.tbUploadCommand);
            this.Controls.Add(this.lblUploadResponse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MusashiPPg";
            this.Size = new System.Drawing.Size(587, 323);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUploadResponse;
        private System.Windows.Forms.TextBox tbUploadCommand;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Label lblDownloadResponse;
        private System.Windows.Forms.TextBox tbDownloadCommand;
        private System.Windows.Forms.Label label3;
        private Controls.MDoubleWithUnits mdPressure;
        private Controls.MDoubleWithUnits mdDispenseTime;
        private Controls.MDoubleWithUnits mdVacuum;
        private Controls.MCheckBox mchTimedMode;
        private Controls.StringCtl strChannel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetPressure;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpdateAll;
    }
}
