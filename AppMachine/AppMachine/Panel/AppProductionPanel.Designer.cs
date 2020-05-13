namespace AppMachine.Panel
{
    partial class AppProductionPanel
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.btnResetOutput = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.panelProdOutput = new System.Windows.Forms.Panel();
            this.lblUPH = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.lblYield = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFail = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.panelProductionInput = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.strOperatorName = new MCore.Controls.StringCtl();
            this.btnSetLot = new System.Windows.Forms.Button();
            this.strProductRecipe = new MCore.Controls.StringCtl();
            this.intLotSize = new MCore.Controls.IntegerCtl();
            this.strLotId = new MCore.Controls.StringCtl();
            this.strOperatorEN = new MCore.Controls.StringCtl();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbIntro = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.panelAmberLight = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.panelRedLight = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.panelGreenLight = new System.Windows.Forms.Panel();
            this.panelProdOutput.SuspendLayout();
            this.panelProductionInput.SuspendLayout();
            this.panelAmberLight.SuspendLayout();
            this.panelRedLight.SuspendLayout();
            this.panelGreenLight.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(955, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 10);
            this.button1.TabIndex = 31;
            this.button1.Text = "Reset Output";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(468, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 16);
            this.label1.TabIndex = 30;
            this.label1.Text = "Operation Message";
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(471, 18);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(517, 110);
            this.rtbMessage.TabIndex = 29;
            this.rtbMessage.Text = "";
            // 
            // btnResetOutput
            // 
            this.btnResetOutput.Location = new System.Drawing.Point(1206, 4);
            this.btnResetOutput.Name = "btnResetOutput";
            this.btnResetOutput.Size = new System.Drawing.Size(33, 10);
            this.btnResetOutput.TabIndex = 28;
            this.btnResetOutput.Text = "Reset Output";
            this.btnResetOutput.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label16.Location = new System.Drawing.Point(1000, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(130, 16);
            this.label16.TabIndex = 27;
            this.label16.Text = "Production Output";
            // 
            // panelProdOutput
            // 
            this.panelProdOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProdOutput.Controls.Add(this.lblUPH);
            this.panelProdOutput.Controls.Add(this.label5);
            this.panelProdOutput.Controls.Add(this.label4);
            this.panelProdOutput.Controls.Add(this.lblOutput);
            this.panelProdOutput.Controls.Add(this.lblYield);
            this.panelProdOutput.Controls.Add(this.label2);
            this.panelProdOutput.Controls.Add(this.lblFail);
            this.panelProdOutput.Controls.Add(this.label13);
            this.panelProdOutput.Controls.Add(this.label8);
            this.panelProdOutput.Controls.Add(this.lblInput);
            this.panelProdOutput.Location = new System.Drawing.Point(1003, 19);
            this.panelProdOutput.Name = "panelProdOutput";
            this.panelProdOutput.Size = new System.Drawing.Size(236, 107);
            this.panelProdOutput.TabIndex = 26;
            // 
            // lblUPH
            // 
            this.lblUPH.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblUPH.ForeColor = System.Drawing.Color.Blue;
            this.lblUPH.Location = new System.Drawing.Point(97, 81);
            this.lblUPH.Name = "lblUPH";
            this.lblUPH.Size = new System.Drawing.Size(135, 25);
            this.lblUPH.TabIndex = 11;
            this.lblUPH.Text = "0";
            this.lblUPH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(3, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "UPH";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(1, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "Outputs";
            // 
            // lblOutput
            // 
            this.lblOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblOutput.ForeColor = System.Drawing.Color.Blue;
            this.lblOutput.Location = new System.Drawing.Point(97, 18);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(135, 25);
            this.lblOutput.TabIndex = 9;
            this.lblOutput.Text = "0";
            this.lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblYield
            // 
            this.lblYield.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblYield.ForeColor = System.Drawing.Color.Blue;
            this.lblYield.Location = new System.Drawing.Point(97, 60);
            this.lblYield.Name = "lblYield";
            this.lblYield.Size = new System.Drawing.Size(135, 25);
            this.lblYield.TabIndex = 7;
            this.lblYield.Text = "0";
            this.lblYield.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(3, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Yield";
            // 
            // lblFail
            // 
            this.lblFail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblFail.ForeColor = System.Drawing.Color.Red;
            this.lblFail.Location = new System.Drawing.Point(97, 38);
            this.lblFail.Name = "lblFail";
            this.lblFail.Size = new System.Drawing.Size(135, 25);
            this.lblFail.TabIndex = 5;
            this.lblFail.Text = "0";
            this.lblFail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label13.Location = new System.Drawing.Point(3, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 24);
            this.label13.TabIndex = 4;
            this.label13.Text = "Failed";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(3, -3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 24);
            this.label8.TabIndex = 2;
            this.label8.Text = "Inputs";
            // 
            // lblInput
            // 
            this.lblInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblInput.ForeColor = System.Drawing.Color.Blue;
            this.lblInput.Location = new System.Drawing.Point(97, -2);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(135, 25);
            this.lblInput.TabIndex = 3;
            this.lblInput.Text = "0";
            this.lblInput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gray;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(422, 102);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(15, 25);
            this.panel4.TabIndex = 25;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label15.Location = new System.Drawing.Point(3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(119, 16);
            this.label15.TabIndex = 18;
            this.label15.Text = "Production Input";
            // 
            // panelProductionInput
            // 
            this.panelProductionInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProductionInput.Controls.Add(this.label6);
            this.panelProductionInput.Controls.Add(this.strOperatorName);
            this.panelProductionInput.Controls.Add(this.btnSetLot);
            this.panelProductionInput.Controls.Add(this.strProductRecipe);
            this.panelProductionInput.Controls.Add(this.intLotSize);
            this.panelProductionInput.Controls.Add(this.strLotId);
            this.panelProductionInput.Controls.Add(this.strOperatorEN);
            this.panelProductionInput.Controls.Add(this.label9);
            this.panelProductionInput.Controls.Add(this.label12);
            this.panelProductionInput.Controls.Add(this.label10);
            this.panelProductionInput.Controls.Add(this.label11);
            this.panelProductionInput.Location = new System.Drawing.Point(3, 19);
            this.panelProductionInput.Name = "panelProductionInput";
            this.panelProductionInput.Size = new System.Drawing.Size(394, 108);
            this.panelProductionInput.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(0, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Operator Name";
            // 
            // strOperatorName
            // 
            this.strOperatorName.BackColor = System.Drawing.Color.Black;
            this.strOperatorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strOperatorName.ForeColor = System.Drawing.Color.Lime;
            this.strOperatorName.Location = new System.Drawing.Point(92, 3);
            this.strOperatorName.LogChanges = true;
            this.strOperatorName.Name = "strOperatorName";
            this.strOperatorName.ReadOnly = true;
            this.strOperatorName.Size = new System.Drawing.Size(162, 22);
            this.strOperatorName.TabIndex = 20;
            // 
            // btnSetLot
            // 
            this.btnSetLot.Location = new System.Drawing.Point(205, 76);
            this.btnSetLot.Name = "btnSetLot";
            this.btnSetLot.Size = new System.Drawing.Size(56, 23);
            this.btnSetLot.TabIndex = 19;
            this.btnSetLot.Text = "Set Lot";
            this.btnSetLot.UseVisualStyleBackColor = true;
            // 
            // strProductRecipe
            // 
            this.strProductRecipe.BackColor = System.Drawing.Color.Black;
            this.strProductRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strProductRecipe.ForeColor = System.Drawing.Color.White;
            this.strProductRecipe.Location = new System.Drawing.Point(92, 28);
            this.strProductRecipe.LogChanges = true;
            this.strProductRecipe.Name = "strProductRecipe";
            this.strProductRecipe.ReadOnly = true;
            this.strProductRecipe.Size = new System.Drawing.Size(294, 22);
            this.strProductRecipe.TabIndex = 18;
            // 
            // intLotSize
            // 
            this.intLotSize.BackColor = System.Drawing.Color.Black;
            this.intLotSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.intLotSize.ForeColor = System.Drawing.Color.Lime;
            this.intLotSize.Location = new System.Drawing.Point(92, 76);
            this.intLotSize.LogChanges = true;
            this.intLotSize.Name = "intLotSize";
            this.intLotSize.Size = new System.Drawing.Size(107, 22);
            this.intLotSize.TabIndex = 17;
            // 
            // strLotId
            // 
            this.strLotId.BackColor = System.Drawing.Color.Black;
            this.strLotId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strLotId.ForeColor = System.Drawing.Color.Lime;
            this.strLotId.Location = new System.Drawing.Point(92, 52);
            this.strLotId.LogChanges = true;
            this.strLotId.Name = "strLotId";
            this.strLotId.Size = new System.Drawing.Size(294, 22);
            this.strLotId.TabIndex = 15;
            // 
            // strOperatorEN
            // 
            this.strOperatorEN.BackColor = System.Drawing.Color.Black;
            this.strOperatorEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strOperatorEN.ForeColor = System.Drawing.Color.Lime;
            this.strOperatorEN.Location = new System.Drawing.Point(284, 4);
            this.strOperatorEN.LogChanges = true;
            this.strOperatorEN.Name = "strOperatorEN";
            this.strOperatorEN.ReadOnly = true;
            this.strOperatorEN.Size = new System.Drawing.Size(102, 22);
            this.strOperatorEN.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(260, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "EN";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label12.Location = new System.Drawing.Point(0, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Product";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(1, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Lot Id";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label11.Location = new System.Drawing.Point(1, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Lot Size";
            // 
            // lbIntro
            // 
            this.lbIntro.AutoSize = true;
            this.lbIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbIntro.Location = new System.Drawing.Point(320, 425);
            this.lbIntro.Name = "lbIntro";
            this.lbIntro.Size = new System.Drawing.Size(581, 33);
            this.lbIntro.TabIndex = 8;
            this.lbIntro.Text = "Please put any GUI design up to application";
            // 
            // label20
            // 
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(0, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(50, 27);
            this.label20.TabIndex = 1;
            this.label20.Text = "ALARM";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelAmberLight
            // 
            this.panelAmberLight.BackColor = System.Drawing.Color.Olive;
            this.panelAmberLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAmberLight.Controls.Add(this.label20);
            this.panelAmberLight.Location = new System.Drawing.Point(403, 47);
            this.panelAmberLight.Name = "panelAmberLight";
            this.panelAmberLight.Size = new System.Drawing.Size(52, 29);
            this.panelAmberLight.TabIndex = 23;
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(0, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(50, 27);
            this.label19.TabIndex = 1;
            this.label19.Text = "STOP";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRedLight
            // 
            this.panelRedLight.BackColor = System.Drawing.Color.Maroon;
            this.panelRedLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRedLight.Controls.Add(this.label19);
            this.panelRedLight.Location = new System.Drawing.Point(403, 19);
            this.panelRedLight.Name = "panelRedLight";
            this.panelRedLight.Size = new System.Drawing.Size(52, 29);
            this.panelRedLight.TabIndex = 24;
            // 
            // label17
            // 
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(0, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(50, 27);
            this.label17.TabIndex = 0;
            this.label17.Text = "RUN";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGreenLight
            // 
            this.panelGreenLight.BackColor = System.Drawing.Color.DarkGreen;
            this.panelGreenLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGreenLight.Controls.Add(this.label17);
            this.panelGreenLight.Location = new System.Drawing.Point(403, 75);
            this.panelGreenLight.Name = "panelGreenLight";
            this.panelGreenLight.Size = new System.Drawing.Size(52, 29);
            this.panelGreenLight.TabIndex = 22;
            // 
            // AppProductionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.btnResetOutput);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.panelProdOutput);
            this.Controls.Add(this.panelGreenLight);
            this.Controls.Add(this.panelRedLight);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panelAmberLight);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.panelProductionInput);
            this.Controls.Add(this.lbIntro);
            this.Name = "AppProductionPanel";
            this.Size = new System.Drawing.Size(1256, 892);
            this.panelProdOutput.ResumeLayout(false);
            this.panelProdOutput.PerformLayout();
            this.panelProductionInput.ResumeLayout(false);
            this.panelProductionInput.PerformLayout();
            this.panelAmberLight.ResumeLayout(false);
            this.panelRedLight.ResumeLayout(false);
            this.panelGreenLight.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbIntro;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panelProductionInput;
        private System.Windows.Forms.Label label6;
        private MCore.Controls.StringCtl strOperatorName;
        private System.Windows.Forms.Button btnSetLot;
        private MCore.Controls.StringCtl strProductRecipe;
        private MCore.Controls.IntegerCtl intLotSize;
        private MCore.Controls.StringCtl strLotId;
        private MCore.Controls.StringCtl strOperatorEN;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnResetOutput;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panelProdOutput;
        private System.Windows.Forms.Label lblUPH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Label lblYield;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Panel panelAmberLight;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panelRedLight;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panelGreenLight;
    }
}
