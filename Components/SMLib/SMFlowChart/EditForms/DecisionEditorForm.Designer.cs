namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class DecisionEditorForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cbNested = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.booleanID = new MCore.Controls.MPIDCtl();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTrueDelay = new System.Windows.Forms.TextBox();
            this.tbFalseDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWaitTimeOut = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbFlowTimeouttoStop = new System.Windows.Forms.CheckBox();
            this.cbUseDryRunLogic = new System.Windows.Forms.CheckBox();
            this.bcbDryRunLogic = new MCore.Controls.BoolComboBoxCtl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(383, 206);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 40);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(575, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(117, 70);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(543, 20);
            this.tbText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Label text";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(479, 206);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cbNested
            // 
            this.cbNested.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbNested.AutoSize = true;
            this.cbNested.Location = new System.Drawing.Point(12, 185);
            this.cbNested.Name = "cbNested";
            this.cbNested.Size = new System.Drawing.Size(90, 17);
            this.cbNested.TabIndex = 5;
            this.cbNested.Text = "Make Nested";
            this.cbNested.UseVisualStyleBackColor = true;
            this.cbNested.CheckedChanged += new System.EventHandler(this.cbNested_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.booleanID);
            this.groupBox1.Location = new System.Drawing.Point(12, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(648, 66);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conditional";
            // 
            // booleanID
            // 
            this.booleanID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.booleanID.AutoLastType = true;
            this.booleanID.ID = "";
            this.booleanID.Location = new System.Drawing.Point(15, 29);
            this.booleanID.LogChanges = true;
            this.booleanID.Name = "booleanID";
            this.booleanID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Boolean;
            this.booleanID.ScopeID = "";
            this.booleanID.Size = new System.Drawing.Size(627, 21);
            this.booleanID.TabIndex = 6;
            this.booleanID.XPos = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "True Delay (mS)";
            // 
            // tbTrueDelay
            // 
            this.tbTrueDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbTrueDelay.Location = new System.Drawing.Point(238, 208);
            this.tbTrueDelay.Name = "tbTrueDelay";
            this.tbTrueDelay.Size = new System.Drawing.Size(73, 20);
            this.tbTrueDelay.TabIndex = 9;
            // 
            // tbFalseDelay
            // 
            this.tbFalseDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbFalseDelay.Location = new System.Drawing.Point(239, 231);
            this.tbFalseDelay.Name = "tbFalseDelay";
            this.tbFalseDelay.Size = new System.Drawing.Size(73, 20);
            this.tbFalseDelay.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "False Delay (mS)";
            // 
            // tbWaitTimeOut
            // 
            this.tbWaitTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbWaitTimeOut.Location = new System.Drawing.Point(238, 185);
            this.tbWaitTimeOut.Name = "tbWaitTimeOut";
            this.tbWaitTimeOut.Size = new System.Drawing.Size(73, 20);
            this.tbWaitTimeOut.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Wait Time Out (mS)";
            // 
            // cbFlowTimeouttoStop
            // 
            this.cbFlowTimeouttoStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbFlowTimeouttoStop.AutoSize = true;
            this.cbFlowTimeouttoStop.Location = new System.Drawing.Point(317, 187);
            this.cbFlowTimeouttoStop.Name = "cbFlowTimeouttoStop";
            this.cbFlowTimeouttoStop.Size = new System.Drawing.Size(126, 17);
            this.cbFlowTimeouttoStop.TabIndex = 14;
            this.cbFlowTimeouttoStop.Text = "Timeout to Stop Path";
            this.cbFlowTimeouttoStop.UseVisualStyleBackColor = true;
            this.cbFlowTimeouttoStop.CheckedChanged += new System.EventHandler(this.cbFlowTimeouttoStop_CheckedChanged);
            // 
            // cbUseDryRunLogic
            // 
            this.cbUseDryRunLogic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbUseDryRunLogic.AutoSize = true;
            this.cbUseDryRunLogic.Location = new System.Drawing.Point(12, 209);
            this.cbUseDryRunLogic.Name = "cbUseDryRunLogic";
            this.cbUseDryRunLogic.Size = new System.Drawing.Size(116, 17);
            this.cbUseDryRunLogic.TabIndex = 15;
            this.cbUseDryRunLogic.Text = "Use Dry Run Logic";
            this.cbUseDryRunLogic.UseVisualStyleBackColor = true;
            this.cbUseDryRunLogic.CheckedChanged += new System.EventHandler(this.cbUseDryRunLogic_CheckedChanged);
            // 
            // bcbDryRunLogic
            // 
            this.bcbDryRunLogic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bcbDryRunLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bcbDryRunLogic.FormattingEnabled = true;
            this.bcbDryRunLogic.Items.AddRange(new object[] {
            "False",
            "True"});
            this.bcbDryRunLogic.Location = new System.Drawing.Point(12, 227);
            this.bcbDryRunLogic.LogChanges = true;
            this.bcbDryRunLogic.Name = "bcbDryRunLogic";
            this.bcbDryRunLogic.Size = new System.Drawing.Size(116, 21);
            this.bcbDryRunLogic.TabIndex = 16;
            this.bcbDryRunLogic.SelectedIndexChanged += new System.EventHandler(this.bcbDryRunLogic_SelectedIndexChanged);
            // 
            // DecisionEditorForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(672, 256);
            this.Controls.Add(this.bcbDryRunLogic);
            this.Controls.Add(this.cbUseDryRunLogic);
            this.Controls.Add(this.cbFlowTimeouttoStop);
            this.Controls.Add(this.tbWaitTimeOut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbFalseDelay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbTrueDelay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbNested);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DecisionEditorForm";
            this.Text = "Decision Flow Element Editor";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox cbNested;
        private MCore.Controls.MPIDCtl booleanID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTrueDelay;
        private System.Windows.Forms.TextBox tbFalseDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWaitTimeOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbFlowTimeouttoStop;
        private System.Windows.Forms.CheckBox cbUseDryRunLogic;
        private MCore.Controls.BoolComboBoxCtl bcbDryRunLogic;
    }
}