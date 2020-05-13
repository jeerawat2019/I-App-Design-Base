namespace MCore.Comp.IOSystem.Input
{
    partial class Array2DInputCtl
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
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.tabPasses = new System.Windows.Forms.TabControl();
            this.tpCoarse = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSetCoarse = new System.Windows.Forms.Button();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.hScrollMin = new System.Windows.Forms.HScrollBar();
            this.hScrollMax = new System.Windows.Forms.HScrollBar();
            this.tpFine = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbFineZOff = new System.Windows.Forms.TextBox();
            this.tbFinePrec = new System.Windows.Forms.TextBox();
            this.hScrollZOff = new System.Windows.Forms.HScrollBar();
            this.hScrollPrec = new System.Windows.Forms.HScrollBar();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblRefPoint = new System.Windows.Forms.Label();
            this.lblRefColor = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblCurImage = new System.Windows.Forms.Label();
            this.lblCurrentImageLabel = new System.Windows.Forms.Label();
            this.gbSubImages = new System.Windows.Forms.GroupBox();
            this.btnUploadCoarse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.tabPasses.SuspendLayout();
            this.tpCoarse.SuspendLayout();
            this.tpFine.SuspendLayout();
            this.gbSubImages.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbImage.Location = new System.Drawing.Point(4, 6);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(404, 404);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            this.pbImage.Click += new System.EventHandler(this.OnClickCoarse);
            // 
            // tabPasses
            // 
            this.tabPasses.Controls.Add(this.tpCoarse);
            this.tabPasses.Controls.Add(this.tpFine);
            this.tabPasses.Location = new System.Drawing.Point(414, 6);
            this.tabPasses.Name = "tabPasses";
            this.tabPasses.SelectedIndex = 0;
            this.tabPasses.Size = new System.Drawing.Size(231, 254);
            this.tabPasses.TabIndex = 1;
            this.tabPasses.SelectedIndexChanged += new System.EventHandler(this.OnTabPageChanged);
            // 
            // tpCoarse
            // 
            this.tpCoarse.Controls.Add(this.btnUploadCoarse);
            this.tpCoarse.Controls.Add(this.label4);
            this.tpCoarse.Controls.Add(this.label3);
            this.tpCoarse.Controls.Add(this.btnSetCoarse);
            this.tpCoarse.Controls.Add(this.tbMin);
            this.tpCoarse.Controls.Add(this.tbMax);
            this.tpCoarse.Controls.Add(this.hScrollMin);
            this.tpCoarse.Controls.Add(this.hScrollMax);
            this.tpCoarse.Location = new System.Drawing.Point(4, 22);
            this.tpCoarse.Name = "tpCoarse";
            this.tpCoarse.Padding = new System.Windows.Forms.Padding(3);
            this.tpCoarse.Size = new System.Drawing.Size(223, 228);
            this.tpCoarse.TabIndex = 0;
            this.tpCoarse.Text = "Coarse";
            this.tpCoarse.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Max (Raw)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Min (Raw)";
            // 
            // btnSetCoarse
            // 
            this.btnSetCoarse.Location = new System.Drawing.Point(116, 105);
            this.btnSetCoarse.Name = "btnSetCoarse";
            this.btnSetCoarse.Size = new System.Drawing.Size(99, 34);
            this.btnSetCoarse.TabIndex = 4;
            this.btnSetCoarse.Text = "Download Coarse";
            this.btnSetCoarse.UseVisualStyleBackColor = true;
            this.btnSetCoarse.Click += new System.EventHandler(this.btnSetCoarse_Click);
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(108, 156);
            this.tbMin.Name = "tbMin";
            this.tbMin.Size = new System.Drawing.Size(70, 20);
            this.tbMin.TabIndex = 3;
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(108, 63);
            this.tbMax.Name = "tbMax";
            this.tbMax.Size = new System.Drawing.Size(70, 20);
            this.tbMax.TabIndex = 2;
            // 
            // hScrollMin
            // 
            this.hScrollMin.LargeChange = 2000;
            this.hScrollMin.Location = new System.Drawing.Point(3, 186);
            this.hScrollMin.Maximum = 50000;
            this.hScrollMin.Minimum = -50000;
            this.hScrollMin.Name = "hScrollMin";
            this.hScrollMin.Size = new System.Drawing.Size(217, 32);
            this.hScrollMin.SmallChange = 100;
            this.hScrollMin.TabIndex = 1;
            this.hScrollMin.ValueChanged += new System.EventHandler(this.OnVChanged0);
            // 
            // hScrollMax
            // 
            this.hScrollMax.LargeChange = 2000;
            this.hScrollMax.Location = new System.Drawing.Point(3, 13);
            this.hScrollMax.Maximum = 50000;
            this.hScrollMax.Minimum = -50000;
            this.hScrollMax.Name = "hScrollMax";
            this.hScrollMax.Size = new System.Drawing.Size(217, 32);
            this.hScrollMax.SmallChange = 100;
            this.hScrollMax.TabIndex = 0;
            this.hScrollMax.Value = -50000;
            this.hScrollMax.ValueChanged += new System.EventHandler(this.OnVChanged0);
            // 
            // tpFine
            // 
            this.tpFine.Controls.Add(this.label6);
            this.tpFine.Controls.Add(this.label5);
            this.tpFine.Controls.Add(this.label2);
            this.tpFine.Controls.Add(this.label1);
            this.tpFine.Controls.Add(this.tbFineZOff);
            this.tpFine.Controls.Add(this.tbFinePrec);
            this.tpFine.Controls.Add(this.hScrollZOff);
            this.tpFine.Controls.Add(this.hScrollPrec);
            this.tpFine.Location = new System.Drawing.Point(4, 22);
            this.tpFine.Name = "tpFine";
            this.tpFine.Padding = new System.Windows.Forms.Padding(3);
            this.tpFine.Size = new System.Drawing.Size(223, 228);
            this.tpFine.TabIndex = 1;
            this.tpFine.Text = "Fine";
            this.tpFine.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Z Ref margin";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Precision";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Microns";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Microns";
            // 
            // tbFineZOff
            // 
            this.tbFineZOff.Location = new System.Drawing.Point(84, 147);
            this.tbFineZOff.Name = "tbFineZOff";
            this.tbFineZOff.Size = new System.Drawing.Size(61, 20);
            this.tbFineZOff.TabIndex = 5;
            // 
            // tbFinePrec
            // 
            this.tbFinePrec.Location = new System.Drawing.Point(84, 67);
            this.tbFinePrec.Name = "tbFinePrec";
            this.tbFinePrec.Size = new System.Drawing.Size(61, 20);
            this.tbFinePrec.TabIndex = 4;
            this.tbFinePrec.TextChanged += new System.EventHandler(this.OnPrecChanged);
            // 
            // hScrollZOff
            // 
            this.hScrollZOff.LargeChange = 1000;
            this.hScrollZOff.Location = new System.Drawing.Point(3, 186);
            this.hScrollZOff.Maximum = 5000;
            this.hScrollZOff.Minimum = -5000;
            this.hScrollZOff.Name = "hScrollZOff";
            this.hScrollZOff.Size = new System.Drawing.Size(217, 32);
            this.hScrollZOff.SmallChange = 10;
            this.hScrollZOff.TabIndex = 3;
            this.hScrollZOff.ValueChanged += new System.EventHandler(this.OnVChanged1);
            // 
            // hScrollPrec
            // 
            this.hScrollPrec.LargeChange = 100;
            this.hScrollPrec.Location = new System.Drawing.Point(3, 13);
            this.hScrollPrec.Maximum = 2000;
            this.hScrollPrec.Minimum = 1;
            this.hScrollPrec.Name = "hScrollPrec";
            this.hScrollPrec.Size = new System.Drawing.Size(217, 32);
            this.hScrollPrec.TabIndex = 2;
            this.hScrollPrec.Value = 1;
            this.hScrollPrec.ValueChanged += new System.EventHandler(this.OnVChanged1);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(453, 383);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(78, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblRefPoint
            // 
            this.lblRefPoint.Location = new System.Drawing.Point(450, 268);
            this.lblRefPoint.Name = "lblRefPoint";
            this.lblRefPoint.Size = new System.Drawing.Size(117, 18);
            this.lblRefPoint.TabIndex = 5;
            this.lblRefPoint.Text = "Ref Color(200,200)=";
            // 
            // lblRefColor
            // 
            this.lblRefColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRefColor.Location = new System.Drawing.Point(553, 266);
            this.lblRefColor.Name = "lblRefColor";
            this.lblRefColor.Size = new System.Drawing.Size(40, 18);
            this.lblRefColor.TabIndex = 6;
            this.lblRefColor.Text = "255";
            this.lblRefColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(539, 383);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(23, 53);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(78, 23);
            this.btnPrev.TabIndex = 8;
            this.btnPrev.Text = "Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(108, 53);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 9;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblCurImage
            // 
            this.lblCurImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCurImage.Location = new System.Drawing.Point(119, 27);
            this.lblCurImage.Name = "lblCurImage";
            this.lblCurImage.Size = new System.Drawing.Size(40, 18);
            this.lblCurImage.TabIndex = 11;
            this.lblCurImage.Text = "0";
            this.lblCurImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentImageLabel
            // 
            this.lblCurrentImageLabel.AutoSize = true;
            this.lblCurrentImageLabel.Location = new System.Drawing.Point(37, 29);
            this.lblCurrentImageLabel.Name = "lblCurrentImageLabel";
            this.lblCurrentImageLabel.Size = new System.Drawing.Size(73, 13);
            this.lblCurrentImageLabel.TabIndex = 10;
            this.lblCurrentImageLabel.Text = "Current Image";
            // 
            // gbSubImages
            // 
            this.gbSubImages.Controls.Add(this.lblCurrentImageLabel);
            this.gbSubImages.Controls.Add(this.lblCurImage);
            this.gbSubImages.Controls.Add(this.btnPrev);
            this.gbSubImages.Controls.Add(this.btnNext);
            this.gbSubImages.Location = new System.Drawing.Point(431, 289);
            this.gbSubImages.Name = "gbSubImages";
            this.gbSubImages.Size = new System.Drawing.Size(207, 83);
            this.gbSubImages.TabIndex = 12;
            this.gbSubImages.TabStop = false;
            this.gbSubImages.Text = "Sub Images";
            // 
            // btnUploadCoarse
            // 
            this.btnUploadCoarse.Location = new System.Drawing.Point(9, 105);
            this.btnUploadCoarse.Name = "btnUploadCoarse";
            this.btnUploadCoarse.Size = new System.Drawing.Size(99, 34);
            this.btnUploadCoarse.TabIndex = 7;
            this.btnUploadCoarse.Text = "Upload Coarse";
            this.btnUploadCoarse.UseVisualStyleBackColor = true;
            this.btnUploadCoarse.Click += new System.EventHandler(this.btnUploadCoarse_Click);
            // 
            // Array2DInputCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSubImages);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lblRefColor);
            this.Controls.Add(this.lblRefPoint);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.tabPasses);
            this.Controls.Add(this.pbImage);
            this.Name = "Array2DInputCtl";
            this.Size = new System.Drawing.Size(648, 414);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.tabPasses.ResumeLayout(false);
            this.tpCoarse.ResumeLayout(false);
            this.tpCoarse.PerformLayout();
            this.tpFine.ResumeLayout(false);
            this.tpFine.PerformLayout();
            this.gbSubImages.ResumeLayout(false);
            this.gbSubImages.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.TabControl tabPasses;
        private System.Windows.Forms.TabPage tpCoarse;
        private System.Windows.Forms.HScrollBar hScrollMin;
        private System.Windows.Forms.HScrollBar hScrollMax;
        private System.Windows.Forms.TabPage tpFine;
        private System.Windows.Forms.HScrollBar hScrollZOff;
        private System.Windows.Forms.HScrollBar hScrollPrec;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.Button btnSetCoarse;
        private System.Windows.Forms.Label lblRefPoint;
        private System.Windows.Forms.Label lblRefColor;
        private System.Windows.Forms.TextBox tbFinePrec;
        private System.Windows.Forms.TextBox tbFineZOff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblCurImage;
        private System.Windows.Forms.Label lblCurrentImageLabel;
        private System.Windows.Forms.GroupBox gbSubImages;
        private System.Windows.Forms.Button btnUploadCoarse;
    }
}
