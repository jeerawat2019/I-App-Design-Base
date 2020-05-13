namespace MCore.Comp.ScanSystem
{
    partial class SM1000Page
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
            this.btnShowAddresses = new System.Windows.Forms.Button();
            this.listAddresses = new System.Windows.Forms.ListBox();
            this.chkBoxConnected = new System.Windows.Forms.CheckBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.tbResults = new System.Windows.Forms.TextBox();
            this.tbArg = new System.Windows.Forms.TextBox();
            this.gbDataTypes = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbLaserOff = new System.Windows.Forms.RadioButton();
            this.rbJobCopy = new System.Windows.Forms.RadioButton();
            this.rbJobList = new System.Windows.Forms.RadioButton();
            this.rbLaserDisable = new System.Windows.Forms.RadioButton();
            this.rbLaserEnable = new System.Windows.Forms.RadioButton();
            this.rbLaserOn = new System.Windows.Forms.RadioButton();
            this.rbGet = new System.Windows.Forms.RadioButton();
            this.rbCustom2 = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbMarkRel = new System.Windows.Forms.RadioButton();
            this.rbJumpAbs = new System.Windows.Forms.RadioButton();
            this.rbJumpRel = new System.Windows.Forms.RadioButton();
            this.rbList = new System.Windows.Forms.RadioButton();
            this.rbAdmin = new System.Windows.Forms.RadioButton();
            this.rbPerformance = new System.Windows.Forms.RadioButton();
            this.rbUser = new System.Windows.Forms.RadioButton();
            this.rbCorrection = new System.Windows.Forms.RadioButton();
            this.rbLens = new System.Windows.Forms.RadioButton();
            this.rbLaser = new System.Windows.Forms.RadioButton();
            this.rbController = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mdYPos = new MCore.Controls.MDoubleWithUnits();
            this.mdXPos = new MCore.Controls.MDoubleWithUnits();
            this.gbDataTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowAddresses
            // 
            this.btnShowAddresses.Location = new System.Drawing.Point(9, 27);
            this.btnShowAddresses.Name = "btnShowAddresses";
            this.btnShowAddresses.Size = new System.Drawing.Size(94, 23);
            this.btnShowAddresses.TabIndex = 0;
            this.btnShowAddresses.Text = "Show addresses";
            this.btnShowAddresses.UseVisualStyleBackColor = true;
            this.btnShowAddresses.Click += new System.EventHandler(this.btnShowAddresses_Click);
            // 
            // listAddresses
            // 
            this.listAddresses.FormattingEnabled = true;
            this.listAddresses.Location = new System.Drawing.Point(9, 56);
            this.listAddresses.Name = "listAddresses";
            this.listAddresses.Size = new System.Drawing.Size(271, 69);
            this.listAddresses.TabIndex = 1;
            this.listAddresses.SelectedIndexChanged += new System.EventHandler(this.OnSelIndexChanged);
            // 
            // chkBoxConnected
            // 
            this.chkBoxConnected.AutoSize = true;
            this.chkBoxConnected.Location = new System.Drawing.Point(9, 4);
            this.chkBoxConnected.Name = "chkBoxConnected";
            this.chkBoxConnected.Size = new System.Drawing.Size(98, 17);
            this.chkBoxConnected.TabIndex = 2;
            this.chkBoxConnected.Text = "Not Connected";
            this.chkBoxConnected.UseVisualStyleBackColor = true;
            this.chkBoxConnected.CheckedChanged += new System.EventHandler(this.OnConnectedChanged);
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(90, 537);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(104, 23);
            this.btnExecute.TabIndex = 8;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.OnExecute);
            // 
            // tbResults
            // 
            this.tbResults.Location = new System.Drawing.Point(300, 27);
            this.tbResults.Multiline = true;
            this.tbResults.Name = "tbResults";
            this.tbResults.Size = new System.Drawing.Size(331, 543);
            this.tbResults.TabIndex = 9;
            // 
            // tbArg
            // 
            this.tbArg.Location = new System.Drawing.Point(106, 197);
            this.tbArg.Name = "tbArg";
            this.tbArg.Size = new System.Drawing.Size(101, 20);
            this.tbArg.TabIndex = 10;
            // 
            // gbDataTypes
            // 
            this.gbDataTypes.Controls.Add(this.radioButton1);
            this.gbDataTypes.Controls.Add(this.rbLaserOff);
            this.gbDataTypes.Controls.Add(this.rbJobCopy);
            this.gbDataTypes.Controls.Add(this.rbJobList);
            this.gbDataTypes.Controls.Add(this.rbLaserDisable);
            this.gbDataTypes.Controls.Add(this.rbLaserEnable);
            this.gbDataTypes.Controls.Add(this.rbLaserOn);
            this.gbDataTypes.Controls.Add(this.rbGet);
            this.gbDataTypes.Controls.Add(this.rbCustom2);
            this.gbDataTypes.Controls.Add(this.rbCustom);
            this.gbDataTypes.Controls.Add(this.rbMarkRel);
            this.gbDataTypes.Controls.Add(this.rbJumpAbs);
            this.gbDataTypes.Controls.Add(this.rbJumpRel);
            this.gbDataTypes.Controls.Add(this.rbList);
            this.gbDataTypes.Controls.Add(this.rbAdmin);
            this.gbDataTypes.Controls.Add(this.rbPerformance);
            this.gbDataTypes.Controls.Add(this.rbUser);
            this.gbDataTypes.Controls.Add(this.rbCorrection);
            this.gbDataTypes.Controls.Add(this.rbLens);
            this.gbDataTypes.Controls.Add(this.rbLaser);
            this.gbDataTypes.Controls.Add(this.rbController);
            this.gbDataTypes.Location = new System.Drawing.Point(29, 223);
            this.gbDataTypes.Name = "gbDataTypes";
            this.gbDataTypes.Size = new System.Drawing.Size(224, 308);
            this.gbDataTypes.TabIndex = 11;
            this.gbDataTypes.TabStop = false;
            this.gbDataTypes.Text = "Execution Type";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(123, 207);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(82, 17);
            this.radioButton1.TabIndex = 20;
            this.radioButton1.Tag = "LaserOn";
            this.radioButton1.Text = "LaserOn(us)";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbLaserOff
            // 
            this.rbLaserOff.AutoSize = true;
            this.rbLaserOff.Location = new System.Drawing.Point(123, 259);
            this.rbLaserOff.Name = "rbLaserOff";
            this.rbLaserOff.Size = new System.Drawing.Size(80, 17);
            this.rbLaserOff.TabIndex = 19;
            this.rbLaserOff.Tag = "LaserSignalOff";
            this.rbLaserOff.Text = "LaserSigOff";
            this.rbLaserOff.UseVisualStyleBackColor = true;
            // 
            // rbJobCopy
            // 
            this.rbJobCopy.AutoSize = true;
            this.rbJobCopy.Checked = true;
            this.rbJobCopy.Location = new System.Drawing.Point(123, 285);
            this.rbJobCopy.Name = "rbJobCopy";
            this.rbJobCopy.Size = new System.Drawing.Size(66, 17);
            this.rbJobCopy.TabIndex = 18;
            this.rbJobCopy.TabStop = true;
            this.rbJobCopy.Text = "JobCopy";
            this.rbJobCopy.UseVisualStyleBackColor = true;
            // 
            // rbJobList
            // 
            this.rbJobList.AutoSize = true;
            this.rbJobList.Checked = true;
            this.rbJobList.Location = new System.Drawing.Point(13, 285);
            this.rbJobList.Name = "rbJobList";
            this.rbJobList.Size = new System.Drawing.Size(58, 17);
            this.rbJobList.TabIndex = 17;
            this.rbJobList.TabStop = true;
            this.rbJobList.Text = "JobList";
            this.rbJobList.UseVisualStyleBackColor = true;
            // 
            // rbLaserDisable
            // 
            this.rbLaserDisable.AutoSize = true;
            this.rbLaserDisable.Location = new System.Drawing.Point(13, 235);
            this.rbLaserDisable.Name = "rbLaserDisable";
            this.rbLaserDisable.Size = new System.Drawing.Size(86, 17);
            this.rbLaserDisable.TabIndex = 16;
            this.rbLaserDisable.Tag = "LaserEnable";
            this.rbLaserDisable.Text = "LaserDisable";
            this.rbLaserDisable.UseVisualStyleBackColor = true;
            // 
            // rbLaserEnable
            // 
            this.rbLaserEnable.AutoSize = true;
            this.rbLaserEnable.Location = new System.Drawing.Point(13, 206);
            this.rbLaserEnable.Name = "rbLaserEnable";
            this.rbLaserEnable.Size = new System.Drawing.Size(84, 17);
            this.rbLaserEnable.TabIndex = 15;
            this.rbLaserEnable.Tag = "LaserEnable";
            this.rbLaserEnable.Text = "LaserEnable";
            this.rbLaserEnable.UseVisualStyleBackColor = true;
            // 
            // rbLaserOn
            // 
            this.rbLaserOn.AutoSize = true;
            this.rbLaserOn.Location = new System.Drawing.Point(123, 230);
            this.rbLaserOn.Name = "rbLaserOn";
            this.rbLaserOn.Size = new System.Drawing.Size(80, 17);
            this.rbLaserOn.TabIndex = 14;
            this.rbLaserOn.Tag = "LaserSignalOn";
            this.rbLaserOn.Text = "LaserSigOn";
            this.rbLaserOn.UseVisualStyleBackColor = true;
            // 
            // rbGet
            // 
            this.rbGet.AutoSize = true;
            this.rbGet.Location = new System.Drawing.Point(123, 173);
            this.rbGet.Name = "rbGet";
            this.rbGet.Size = new System.Drawing.Size(42, 17);
            this.rbGet.TabIndex = 13;
            this.rbGet.Tag = "Laser";
            this.rbGet.Text = "Get";
            this.rbGet.UseVisualStyleBackColor = true;
            // 
            // rbCustom2
            // 
            this.rbCustom2.AutoSize = true;
            this.rbCustom2.Location = new System.Drawing.Point(15, 173);
            this.rbCustom2.Name = "rbCustom2";
            this.rbCustom2.Size = new System.Drawing.Size(66, 17);
            this.rbCustom2.TabIndex = 12;
            this.rbCustom2.Tag = "Laser";
            this.rbCustom2.Text = "Custom2";
            this.rbCustom2.UseVisualStyleBackColor = true;
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(124, 150);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(67, 17);
            this.rbCustom.TabIndex = 11;
            this.rbCustom.Tag = "MarkAbs";
            this.rbCustom.Text = "MarkAbs";
            this.rbCustom.UseVisualStyleBackColor = true;
            // 
            // rbMarkRel
            // 
            this.rbMarkRel.AutoSize = true;
            this.rbMarkRel.Location = new System.Drawing.Point(124, 130);
            this.rbMarkRel.Name = "rbMarkRel";
            this.rbMarkRel.Size = new System.Drawing.Size(65, 17);
            this.rbMarkRel.TabIndex = 10;
            this.rbMarkRel.Tag = "MarkRel";
            this.rbMarkRel.Text = "MarkRel";
            this.rbMarkRel.UseVisualStyleBackColor = true;
            // 
            // rbJumpAbs
            // 
            this.rbJumpAbs.AutoSize = true;
            this.rbJumpAbs.Location = new System.Drawing.Point(15, 150);
            this.rbJumpAbs.Name = "rbJumpAbs";
            this.rbJumpAbs.Size = new System.Drawing.Size(68, 17);
            this.rbJumpAbs.TabIndex = 9;
            this.rbJumpAbs.Tag = "JumpAbs";
            this.rbJumpAbs.Text = "JumpAbs";
            this.rbJumpAbs.UseVisualStyleBackColor = true;
            // 
            // rbJumpRel
            // 
            this.rbJumpRel.AutoSize = true;
            this.rbJumpRel.Location = new System.Drawing.Point(15, 130);
            this.rbJumpRel.Name = "rbJumpRel";
            this.rbJumpRel.Size = new System.Drawing.Size(66, 17);
            this.rbJumpRel.TabIndex = 8;
            this.rbJumpRel.Tag = "JumpRel";
            this.rbJumpRel.Text = "JumpRel";
            this.rbJumpRel.UseVisualStyleBackColor = true;
            // 
            // rbList
            // 
            this.rbList.AutoSize = true;
            this.rbList.Checked = true;
            this.rbList.Location = new System.Drawing.Point(124, 87);
            this.rbList.Name = "rbList";
            this.rbList.Size = new System.Drawing.Size(41, 17);
            this.rbList.TabIndex = 7;
            this.rbList.TabStop = true;
            this.rbList.Text = "List";
            this.rbList.UseVisualStyleBackColor = true;
            // 
            // rbAdmin
            // 
            this.rbAdmin.AutoSize = true;
            this.rbAdmin.Location = new System.Drawing.Point(124, 65);
            this.rbAdmin.Name = "rbAdmin";
            this.rbAdmin.Size = new System.Drawing.Size(54, 17);
            this.rbAdmin.TabIndex = 6;
            this.rbAdmin.Text = "Admin";
            this.rbAdmin.UseVisualStyleBackColor = true;
            // 
            // rbPerformance
            // 
            this.rbPerformance.AutoSize = true;
            this.rbPerformance.Location = new System.Drawing.Point(124, 42);
            this.rbPerformance.Name = "rbPerformance";
            this.rbPerformance.Size = new System.Drawing.Size(85, 17);
            this.rbPerformance.TabIndex = 5;
            this.rbPerformance.Text = "Performance";
            this.rbPerformance.UseVisualStyleBackColor = true;
            // 
            // rbUser
            // 
            this.rbUser.AutoSize = true;
            this.rbUser.Location = new System.Drawing.Point(124, 19);
            this.rbUser.Name = "rbUser";
            this.rbUser.Size = new System.Drawing.Size(47, 17);
            this.rbUser.TabIndex = 4;
            this.rbUser.Text = "User";
            this.rbUser.UseVisualStyleBackColor = true;
            // 
            // rbCorrection
            // 
            this.rbCorrection.AutoSize = true;
            this.rbCorrection.Location = new System.Drawing.Point(15, 87);
            this.rbCorrection.Name = "rbCorrection";
            this.rbCorrection.Size = new System.Drawing.Size(73, 17);
            this.rbCorrection.TabIndex = 3;
            this.rbCorrection.Text = "Correction";
            this.rbCorrection.UseVisualStyleBackColor = true;
            // 
            // rbLens
            // 
            this.rbLens.AutoSize = true;
            this.rbLens.Location = new System.Drawing.Point(15, 64);
            this.rbLens.Name = "rbLens";
            this.rbLens.Size = new System.Drawing.Size(48, 17);
            this.rbLens.TabIndex = 2;
            this.rbLens.Text = "Lens";
            this.rbLens.UseVisualStyleBackColor = true;
            // 
            // rbLaser
            // 
            this.rbLaser.AutoSize = true;
            this.rbLaser.Location = new System.Drawing.Point(15, 42);
            this.rbLaser.Name = "rbLaser";
            this.rbLaser.Size = new System.Drawing.Size(51, 17);
            this.rbLaser.TabIndex = 1;
            this.rbLaser.Text = "Laser";
            this.rbLaser.UseVisualStyleBackColor = true;
            // 
            // rbController
            // 
            this.rbController.AutoSize = true;
            this.rbController.Location = new System.Drawing.Point(15, 19);
            this.rbController.Name = "rbController";
            this.rbController.Size = new System.Drawing.Size(69, 17);
            this.rbController.TabIndex = 0;
            this.rbController.Text = "Controller";
            this.rbController.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 200);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Arg";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Results";
            // 
            // mdYPos
            // 
            this.mdYPos.Label = "";
            this.mdYPos.Location = new System.Drawing.Point(29, 169);
            this.mdYPos.Name = "mdYPos";
            this.mdYPos.Size = new System.Drawing.Size(170, 22);
            this.mdYPos.TabIndex = 4;
            // 
            // mdXPos
            // 
            this.mdXPos.Label = "";
            this.mdXPos.Location = new System.Drawing.Point(29, 140);
            this.mdXPos.Name = "mdXPos";
            this.mdXPos.Size = new System.Drawing.Size(170, 22);
            this.mdXPos.TabIndex = 3;
            // 
            // SM1000Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbDataTypes);
            this.Controls.Add(this.tbArg);
            this.Controls.Add(this.tbResults);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.mdYPos);
            this.Controls.Add(this.mdXPos);
            this.Controls.Add(this.chkBoxConnected);
            this.Controls.Add(this.listAddresses);
            this.Controls.Add(this.btnShowAddresses);
            this.Name = "SM1000Page";
            this.Size = new System.Drawing.Size(638, 573);
            this.gbDataTypes.ResumeLayout(false);
            this.gbDataTypes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShowAddresses;
        private System.Windows.Forms.ListBox listAddresses;
        private System.Windows.Forms.CheckBox chkBoxConnected;
        private Controls.MDoubleWithUnits mdXPos;
        private Controls.MDoubleWithUnits mdYPos;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox tbResults;
        private System.Windows.Forms.TextBox tbArg;
        private System.Windows.Forms.GroupBox gbDataTypes;
        private System.Windows.Forms.RadioButton rbList;
        private System.Windows.Forms.RadioButton rbAdmin;
        private System.Windows.Forms.RadioButton rbPerformance;
        private System.Windows.Forms.RadioButton rbUser;
        private System.Windows.Forms.RadioButton rbCorrection;
        private System.Windows.Forms.RadioButton rbLens;
        private System.Windows.Forms.RadioButton rbLaser;
        private System.Windows.Forms.RadioButton rbController;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbCustom2;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbMarkRel;
        private System.Windows.Forms.RadioButton rbJumpAbs;
        private System.Windows.Forms.RadioButton rbJumpRel;
        private System.Windows.Forms.RadioButton rbGet;
        private System.Windows.Forms.RadioButton rbLaserOn;
        private System.Windows.Forms.RadioButton rbLaserDisable;
        private System.Windows.Forms.RadioButton rbLaserEnable;
        private System.Windows.Forms.RadioButton rbJobList;
        private System.Windows.Forms.RadioButton rbJobCopy;
        private System.Windows.Forms.RadioButton rbLaserOff;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}
