namespace MCore.Comp.IOSystem
{
    partial class MEDAQLibPage
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
            this.rbAvgMoving = new System.Windows.Forms.RadioButton();
            this.rbAvgMedian = new System.Windows.Forms.RadioButton();
            this.gbAveraging = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.intMovingAverageCount = new MCore.Controls.IntegerCtl();
            this.cbMedianChoices = new MCore.Controls.EnumComboBoxCtl();
            this.label5 = new System.Windows.Forms.Label();
            this.gbSensorInfo = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.tbBootloaderVersion = new System.Windows.Forms.TextBox();
            this.tbSoftwareVersion = new System.Windows.Forms.TextBox();
            this.tbMeasurementRange = new System.Windows.Forms.TextBox();
            this.tbSerialNumber = new System.Windows.Forms.TextBox();
            this.tbOption = new System.Windows.Forms.TextBox();
            this.tbArticleNumber = new System.Windows.Forms.TextBox();
            this.tbSensorType = new System.Windows.Forms.TextBox();
            this.tbSensor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.triggerMode = new MCore.Comp.IOSystem.CompMeasureCtl();
            this.mdReadValue = new MCore.Controls.MDoubleWithUnits();
            this.cbExtInputMode = new MCore.Controls.EnumComboBoxCtl();
            this.cbMeasurementRate = new MCore.Controls.EnumComboBoxCtl();
            this.gbAveraging.SuspendLayout();
            this.gbSensorInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Measurement Rate";
            // 
            // rbAvgMoving
            // 
            this.rbAvgMoving.AutoSize = true;
            this.rbAvgMoving.Location = new System.Drawing.Point(32, 26);
            this.rbAvgMoving.Name = "rbAvgMoving";
            this.rbAvgMoving.Size = new System.Drawing.Size(84, 17);
            this.rbAvgMoving.TabIndex = 2;
            this.rbAvgMoving.TabStop = true;
            this.rbAvgMoving.Text = "Moving over";
            this.rbAvgMoving.UseVisualStyleBackColor = true;
            this.rbAvgMoving.CheckedChanged += new System.EventHandler(this.OnChangedAveragingType);
            // 
            // rbAvgMedian
            // 
            this.rbAvgMedian.AutoSize = true;
            this.rbAvgMedian.Location = new System.Drawing.Point(32, 61);
            this.rbAvgMedian.Name = "rbAvgMedian";
            this.rbAvgMedian.Size = new System.Drawing.Size(72, 17);
            this.rbAvgMedian.TabIndex = 3;
            this.rbAvgMedian.TabStop = true;
            this.rbAvgMedian.Text = "Median of";
            this.rbAvgMedian.UseVisualStyleBackColor = true;
            this.rbAvgMedian.CheckedChanged += new System.EventHandler(this.OnChangedAveragingType);
            // 
            // gbAveraging
            // 
            this.gbAveraging.Controls.Add(this.label3);
            this.gbAveraging.Controls.Add(this.label2);
            this.gbAveraging.Controls.Add(this.intMovingAverageCount);
            this.gbAveraging.Controls.Add(this.cbMedianChoices);
            this.gbAveraging.Controls.Add(this.rbAvgMoving);
            this.gbAveraging.Controls.Add(this.rbAvgMedian);
            this.gbAveraging.Location = new System.Drawing.Point(27, 123);
            this.gbAveraging.Name = "gbAveraging";
            this.gbAveraging.Size = new System.Drawing.Size(300, 100);
            this.gbAveraging.TabIndex = 4;
            this.gbAveraging.TabStop = false;
            this.gbAveraging.Text = "Averaging";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "values";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "values";
            // 
            // intMovingAverageCount
            // 
            this.intMovingAverageCount.Location = new System.Drawing.Point(125, 26);
            this.intMovingAverageCount.Name = "intMovingAverageCount";
            this.intMovingAverageCount.Size = new System.Drawing.Size(88, 20);
            this.intMovingAverageCount.TabIndex = 5;
            // 
            // cbMedianChoices
            // 
            this.cbMedianChoices.FormattingEnabled = true;
            this.cbMedianChoices.Location = new System.Drawing.Point(125, 60);
            this.cbMedianChoices.Name = "cbMedianChoices";
            this.cbMedianChoices.Size = new System.Drawing.Size(88, 21);
            this.cbMedianChoices.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Digital Input";
            // 
            // gbSensorInfo
            // 
            this.gbSensorInfo.Controls.Add(this.label14);
            this.gbSensorInfo.Controls.Add(this.label13);
            this.gbSensorInfo.Controls.Add(this.label12);
            this.gbSensorInfo.Controls.Add(this.label11);
            this.gbSensorInfo.Controls.Add(this.label10);
            this.gbSensorInfo.Controls.Add(this.label9);
            this.gbSensorInfo.Controls.Add(this.label8);
            this.gbSensorInfo.Controls.Add(this.label7);
            this.gbSensorInfo.Controls.Add(this.tbDate);
            this.gbSensorInfo.Controls.Add(this.tbBootloaderVersion);
            this.gbSensorInfo.Controls.Add(this.tbSoftwareVersion);
            this.gbSensorInfo.Controls.Add(this.tbMeasurementRange);
            this.gbSensorInfo.Controls.Add(this.tbSerialNumber);
            this.gbSensorInfo.Controls.Add(this.tbOption);
            this.gbSensorInfo.Controls.Add(this.tbArticleNumber);
            this.gbSensorInfo.Controls.Add(this.tbSensorType);
            this.gbSensorInfo.Controls.Add(this.tbSensor);
            this.gbSensorInfo.Controls.Add(this.label6);
            this.gbSensorInfo.Location = new System.Drawing.Point(347, 15);
            this.gbSensorInfo.Name = "gbSensorInfo";
            this.gbSensorInfo.Size = new System.Drawing.Size(384, 324);
            this.gbSensorInfo.TabIndex = 10;
            this.gbSensorInfo.TabStop = false;
            this.gbSensorInfo.Text = "Sensor Information";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(89, 263);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Date:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(24, 236);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Bootloader version:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 209);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Softwware version:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 182);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Measurement range:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(48, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Serial number:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(81, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Option:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(45, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Article number:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Sensor type:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbDate
            // 
            this.tbDate.Location = new System.Drawing.Point(128, 260);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(234, 20);
            this.tbDate.TabIndex = 9;
            // 
            // tbBootloaderVersion
            // 
            this.tbBootloaderVersion.Location = new System.Drawing.Point(128, 233);
            this.tbBootloaderVersion.Name = "tbBootloaderVersion";
            this.tbBootloaderVersion.Size = new System.Drawing.Size(234, 20);
            this.tbBootloaderVersion.TabIndex = 8;
            // 
            // tbSoftwareVersion
            // 
            this.tbSoftwareVersion.Location = new System.Drawing.Point(128, 206);
            this.tbSoftwareVersion.Name = "tbSoftwareVersion";
            this.tbSoftwareVersion.Size = new System.Drawing.Size(234, 20);
            this.tbSoftwareVersion.TabIndex = 7;
            // 
            // tbMeasurementRange
            // 
            this.tbMeasurementRange.Location = new System.Drawing.Point(128, 179);
            this.tbMeasurementRange.Name = "tbMeasurementRange";
            this.tbMeasurementRange.Size = new System.Drawing.Size(234, 20);
            this.tbMeasurementRange.TabIndex = 6;
            // 
            // tbSerialNumber
            // 
            this.tbSerialNumber.Location = new System.Drawing.Point(128, 152);
            this.tbSerialNumber.Name = "tbSerialNumber";
            this.tbSerialNumber.Size = new System.Drawing.Size(234, 20);
            this.tbSerialNumber.TabIndex = 5;
            // 
            // tbOption
            // 
            this.tbOption.Location = new System.Drawing.Point(128, 125);
            this.tbOption.Name = "tbOption";
            this.tbOption.Size = new System.Drawing.Size(234, 20);
            this.tbOption.TabIndex = 4;
            // 
            // tbArticleNumber
            // 
            this.tbArticleNumber.Location = new System.Drawing.Point(128, 98);
            this.tbArticleNumber.Name = "tbArticleNumber";
            this.tbArticleNumber.Size = new System.Drawing.Size(234, 20);
            this.tbArticleNumber.TabIndex = 3;
            // 
            // tbSensorType
            // 
            this.tbSensorType.Location = new System.Drawing.Point(128, 71);
            this.tbSensorType.Name = "tbSensorType";
            this.tbSensorType.Size = new System.Drawing.Size(234, 20);
            this.tbSensorType.TabIndex = 2;
            // 
            // tbSensor
            // 
            this.tbSensor.Location = new System.Drawing.Point(128, 44);
            this.tbSensor.Name = "tbSensor";
            this.tbSensor.Size = new System.Drawing.Size(234, 20);
            this.tbSensor.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(79, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Sensor:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // triggerMode
            // 
            this.triggerMode.Label = "Trigger mode";
            this.triggerMode.Location = new System.Drawing.Point(27, 229);
            this.triggerMode.Name = "triggerMode";
            this.triggerMode.Size = new System.Drawing.Size(300, 154);
            this.triggerMode.TabIndex = 14;
            // 
            // mdReadValue
            // 
            this.mdReadValue.Label = "Read value";
            this.mdReadValue.Location = new System.Drawing.Point(59, 80);
            this.mdReadValue.Name = "mdReadValue";
            this.mdReadValue.Size = new System.Drawing.Size(170, 22);
            this.mdReadValue.TabIndex = 12;
            // 
            // cbExtInputMode
            // 
            this.cbExtInputMode.FormattingEnabled = true;
            this.cbExtInputMode.Location = new System.Drawing.Point(142, 39);
            this.cbExtInputMode.Name = "cbExtInputMode";
            this.cbExtInputMode.Size = new System.Drawing.Size(121, 21);
            this.cbExtInputMode.TabIndex = 8;
            // 
            // cbMeasurementRate
            // 
            this.cbMeasurementRate.FormattingEnabled = true;
            this.cbMeasurementRate.Location = new System.Drawing.Point(142, 12);
            this.cbMeasurementRate.Name = "cbMeasurementRate";
            this.cbMeasurementRate.Size = new System.Drawing.Size(121, 21);
            this.cbMeasurementRate.TabIndex = 5;
            // 
            // MEDAQLibPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.triggerMode);
            this.Controls.Add(this.mdReadValue);
            this.Controls.Add(this.gbSensorInfo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbExtInputMode);
            this.Controls.Add(this.cbMeasurementRate);
            this.Controls.Add(this.gbAveraging);
            this.Controls.Add(this.label1);
            this.Name = "MEDAQLibPage";
            this.Size = new System.Drawing.Size(751, 538);
            this.gbAveraging.ResumeLayout(false);
            this.gbAveraging.PerformLayout();
            this.gbSensorInfo.ResumeLayout(false);
            this.gbSensorInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbAvgMoving;
        private System.Windows.Forms.RadioButton rbAvgMedian;
        private System.Windows.Forms.GroupBox gbAveraging;
        private Controls.IntegerCtl intMovingAverageCount;
        private Controls.EnumComboBoxCtl cbMedianChoices;
        private Controls.EnumComboBoxCtl cbMeasurementRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Controls.EnumComboBoxCtl cbExtInputMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbSensorInfo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.TextBox tbBootloaderVersion;
        private System.Windows.Forms.TextBox tbSoftwareVersion;
        private System.Windows.Forms.TextBox tbMeasurementRange;
        private System.Windows.Forms.TextBox tbSerialNumber;
        private System.Windows.Forms.TextBox tbOption;
        private System.Windows.Forms.TextBox tbArticleNumber;
        private System.Windows.Forms.TextBox tbSensorType;
        private System.Windows.Forms.TextBox tbSensor;
        private System.Windows.Forms.Label label6;
        private MCore.Controls.MDoubleWithUnits mdReadValue;
        private MCore.Comp.IOSystem.CompMeasureCtl triggerMode;
    }
}
