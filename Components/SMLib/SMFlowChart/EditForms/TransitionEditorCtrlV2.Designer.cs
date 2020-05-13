namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class TransitionEditorCtrlV2
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
            mCbLoopTransition.UnBind();
            mDoubleTimeOut.UnBind();
            strTimeOutCaption.UnBind();
            strTimeOutMsg.UnBind();

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
            this.btnDeleteTrans = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.smValueID = new MCore.Controls.MPIDCtl();
            this.cbOperator = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.smPropID = new MCore.Controls.MPIDCtl();
            this.treeComponents = new System.Windows.Forms.TreeView();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddCond = new System.Windows.Forms.Button();
            this.btnAddAND = new System.Windows.Forms.Button();
            this.btnAddOR = new System.Windows.Forms.Button();
            this.btnDelCond = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.cmbTransitionID = new System.Windows.Forms.ComboBox();
            this.btnUpdateCond = new System.Windows.Forms.Button();
            this.btnMorePrioruty = new System.Windows.Forms.Button();
            this.btnLessPriority = new System.Windows.Forms.Button();
            this.btnNewTrans = new System.Windows.Forms.Button();
            this.mCbLoopTransition = new MCore.Controls.MCheckBox();
            this.strTimeOutMsg = new MCore.Controls.StringCtl();
            this.lblTimeOutMsg = new System.Windows.Forms.Label();
            this.lblTimeOutCaption = new System.Windows.Forms.Label();
            this.strTimeOutCaption = new MCore.Controls.StringCtl();
            this.mDoubleTimeOut = new MCore.Controls.MDoubleWithUnits();
            this.mDoubleLoopTime = new MCore.Controls.MDoubleWithUnits();
            this.mcbTimeoutToStopPath = new MCore.Controls.MCheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDryRunTransitionID = new System.Windows.Forms.ComboBox();
            this.mcbUseDryRunTrans = new MCore.Controls.MCheckBox();
            this.panelTransitionEdit = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panelConditionEdit = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panelTransitionEdit.SuspendLayout();
            this.panelConditionEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDeleteTrans
            // 
            this.btnDeleteTrans.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDeleteTrans.Location = new System.Drawing.Point(3, 35);
            this.btnDeleteTrans.Name = "btnDeleteTrans";
            this.btnDeleteTrans.Size = new System.Drawing.Size(90, 26);
            this.btnDeleteTrans.TabIndex = 2;
            this.btnDeleteTrans.Text = "Delete Trans";
            this.btnDeleteTrans.UseVisualStyleBackColor = false;
            this.btnDeleteTrans.Click += new System.EventHandler(this.btnDeleteTrans_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.smValueID);
            this.groupBox1.Controls.Add(this.cbOperator);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.smPropID);
            this.groupBox1.Location = new System.Drawing.Point(2, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(609, 76);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conditional Select";
            // 
            // smValueID
            // 
            this.smValueID.AutoLastType = false;
            this.smValueID.ID = "";
            this.smValueID.Location = new System.Drawing.Point(143, 49);
            this.smValueID.LogChanges = true;
            this.smValueID.Name = "smValueID";
            this.smValueID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            this.smValueID.ScopeID = "";
            this.smValueID.Size = new System.Drawing.Size(456, 21);
            this.smValueID.TabIndex = 26;
            this.smValueID.XPos = 0;
            // 
            // cbOperator
            // 
            this.cbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOperator.FormattingEnabled = true;
            this.cbOperator.Items.AddRange(new object[] {
            "==",
            "!=",
            ">",
            ">=",
            "<",
            "<="});
            this.cbOperator.Location = new System.Drawing.Point(93, 48);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(44, 21);
            this.cbOperator.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Condition Value";
            // 
            // smPropID
            // 
            this.smPropID.AutoLastType = false;
            this.smPropID.ID = "";
            this.smPropID.Location = new System.Drawing.Point(11, 19);
            this.smPropID.LogChanges = true;
            this.smPropID.Name = "smPropID";
            this.smPropID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            this.smPropID.ScopeID = "";
            this.smPropID.Size = new System.Drawing.Size(594, 21);
            this.smPropID.TabIndex = 22;
            this.smPropID.XPos = 0;
            // 
            // treeComponents
            // 
            this.treeComponents.Location = new System.Drawing.Point(26, 59);
            this.treeComponents.Name = "treeComponents";
            this.treeComponents.Size = new System.Drawing.Size(505, 292);
            this.treeComponents.TabIndex = 15;
            this.treeComponents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeComponents_NodeMouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Path Out";
            // 
            // btnAddCond
            // 
            this.btnAddCond.BackColor = System.Drawing.Color.White;
            this.btnAddCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCond.Location = new System.Drawing.Point(17, 3);
            this.btnAddCond.Name = "btnAddCond";
            this.btnAddCond.Size = new System.Drawing.Size(72, 26);
            this.btnAddCond.TabIndex = 17;
            this.btnAddCond.Text = "Add Cond";
            this.btnAddCond.UseVisualStyleBackColor = false;
            this.btnAddCond.Click += new System.EventHandler(this.btnAddCond_Click);
            // 
            // btnAddAND
            // 
            this.btnAddAND.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddAND.Location = new System.Drawing.Point(320, 3);
            this.btnAddAND.Name = "btnAddAND";
            this.btnAddAND.Size = new System.Drawing.Size(84, 26);
            this.btnAddAND.TabIndex = 18;
            this.btnAddAND.Text = "Add AND";
            this.btnAddAND.UseVisualStyleBackColor = true;
            this.btnAddAND.Click += new System.EventHandler(this.btnAddAND_Click);
            // 
            // btnAddOR
            // 
            this.btnAddOR.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddOR.Location = new System.Drawing.Point(410, 3);
            this.btnAddOR.Name = "btnAddOR";
            this.btnAddOR.Size = new System.Drawing.Size(84, 26);
            this.btnAddOR.TabIndex = 19;
            this.btnAddOR.Text = "Add OR";
            this.btnAddOR.UseVisualStyleBackColor = true;
            this.btnAddOR.Click += new System.EventHandler(this.btnAddOR_Click);
            // 
            // btnDelCond
            // 
            this.btnDelCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnDelCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelCond.Location = new System.Drawing.Point(187, 3);
            this.btnDelCond.Name = "btnDelCond";
            this.btnDelCond.Size = new System.Drawing.Size(83, 26);
            this.btnDelCond.TabIndex = 21;
            this.btnDelCond.Text = "Delete Cond";
            this.btnDelCond.UseVisualStyleBackColor = false;
            this.btnDelCond.Click += new System.EventHandler(this.btnDelCond_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnValidate.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(3, 244);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(96, 26);
            this.btnValidate.TabIndex = 22;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // cmbTransitionID
            // 
            this.cmbTransitionID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransitionID.FormattingEnabled = true;
            this.cmbTransitionID.Location = new System.Drawing.Point(61, 31);
            this.cmbTransitionID.Name = "cmbTransitionID";
            this.cmbTransitionID.Size = new System.Drawing.Size(550, 21);
            this.cmbTransitionID.TabIndex = 26;
            this.cmbTransitionID.DropDown += new System.EventHandler(this.cmbTransitionID_DropDown);
            this.cmbTransitionID.SelectionChangeCommitted += new System.EventHandler(this.cmbTransitionID_SelectionChangeCommitted);
            // 
            // btnUpdateCond
            // 
            this.btnUpdateCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnUpdateCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnUpdateCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCond.Location = new System.Drawing.Point(95, 3);
            this.btnUpdateCond.Name = "btnUpdateCond";
            this.btnUpdateCond.Size = new System.Drawing.Size(86, 26);
            this.btnUpdateCond.TabIndex = 27;
            this.btnUpdateCond.Text = "Update Cond";
            this.btnUpdateCond.UseVisualStyleBackColor = false;
            this.btnUpdateCond.Click += new System.EventHandler(this.btnUpdateCond_Click);
            // 
            // btnMorePrioruty
            // 
            this.btnMorePrioruty.BackColor = System.Drawing.Color.White;
            this.btnMorePrioruty.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnMorePrioruty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMorePrioruty.Location = new System.Drawing.Point(3, 96);
            this.btnMorePrioruty.Name = "btnMorePrioruty";
            this.btnMorePrioruty.Size = new System.Drawing.Size(84, 26);
            this.btnMorePrioruty.TabIndex = 29;
            this.btnMorePrioruty.Text = "More Priority";
            this.btnMorePrioruty.UseVisualStyleBackColor = false;
            this.btnMorePrioruty.Click += new System.EventHandler(this.btnMorePrioruty_Click);
            // 
            // btnLessPriority
            // 
            this.btnLessPriority.BackColor = System.Drawing.Color.Black;
            this.btnLessPriority.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnLessPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLessPriority.ForeColor = System.Drawing.Color.White;
            this.btnLessPriority.Location = new System.Drawing.Point(3, 128);
            this.btnLessPriority.Name = "btnLessPriority";
            this.btnLessPriority.Size = new System.Drawing.Size(84, 26);
            this.btnLessPriority.TabIndex = 30;
            this.btnLessPriority.Text = "Less Priority";
            this.btnLessPriority.UseVisualStyleBackColor = false;
            this.btnLessPriority.Click += new System.EventHandler(this.btnLessPriority_Click);
            // 
            // btnNewTrans
            // 
            this.btnNewTrans.Location = new System.Drawing.Point(3, 3);
            this.btnNewTrans.Name = "btnNewTrans";
            this.btnNewTrans.Size = new System.Drawing.Size(90, 26);
            this.btnNewTrans.TabIndex = 31;
            this.btnNewTrans.Text = "New Trans";
            this.btnNewTrans.UseVisualStyleBackColor = true;
            this.btnNewTrans.Click += new System.EventHandler(this.btnNewTrans_Click);
            // 
            // mCbLoopTransition
            // 
            this.mCbLoopTransition.AutoSize = true;
            this.mCbLoopTransition.Location = new System.Drawing.Point(1, 176);
            this.mCbLoopTransition.LogChanges = true;
            this.mCbLoopTransition.Name = "mCbLoopTransition";
            this.mCbLoopTransition.Size = new System.Drawing.Size(104, 17);
            this.mCbLoopTransition.TabIndex = 32;
            this.mCbLoopTransition.Text = "Loop Transitions";
            this.mCbLoopTransition.UseVisualStyleBackColor = true;
            // 
            // strTimeOutMsg
            // 
            this.strTimeOutMsg.Location = new System.Drawing.Point(125, 33);
            this.strTimeOutMsg.LogChanges = true;
            this.strTimeOutMsg.Name = "strTimeOutMsg";
            this.strTimeOutMsg.Size = new System.Drawing.Size(369, 20);
            this.strTimeOutMsg.TabIndex = 35;
            // 
            // lblTimeOutMsg
            // 
            this.lblTimeOutMsg.AutoSize = true;
            this.lblTimeOutMsg.Location = new System.Drawing.Point(26, 36);
            this.lblTimeOutMsg.Name = "lblTimeOutMsg";
            this.lblTimeOutMsg.Size = new System.Drawing.Size(96, 13);
            this.lblTimeOutMsg.TabIndex = 36;
            this.lblTimeOutMsg.Text = "Time Out Message";
            // 
            // lblTimeOutCaption
            // 
            this.lblTimeOutCaption.AutoSize = true;
            this.lblTimeOutCaption.Location = new System.Drawing.Point(26, 11);
            this.lblTimeOutCaption.Name = "lblTimeOutCaption";
            this.lblTimeOutCaption.Size = new System.Drawing.Size(89, 13);
            this.lblTimeOutCaption.TabIndex = 37;
            this.lblTimeOutCaption.Text = "Time Out Caption";
            // 
            // strTimeOutCaption
            // 
            this.strTimeOutCaption.Location = new System.Drawing.Point(125, 7);
            this.strTimeOutCaption.LogChanges = true;
            this.strTimeOutCaption.Name = "strTimeOutCaption";
            this.strTimeOutCaption.Size = new System.Drawing.Size(301, 20);
            this.strTimeOutCaption.TabIndex = 38;
            // 
            // mDoubleTimeOut
            // 
            this.mDoubleTimeOut.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDoubleTimeOut.DoubleVal = 0D;
            this.mDoubleTimeOut.Enabled = false;
            this.mDoubleTimeOut.Label = "";
            this.mDoubleTimeOut.Location = new System.Drawing.Point(433, 6);
            this.mDoubleTimeOut.LogChanges = true;
            this.mDoubleTimeOut.Name = "mDoubleTimeOut";
            this.mDoubleTimeOut.Padding = new System.Windows.Forms.Padding(2);
            this.mDoubleTimeOut.Size = new System.Drawing.Size(215, 22);
            this.mDoubleTimeOut.TabIndex = 39;
            this.mDoubleTimeOut.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDoubleTimeOut.UnitsLabel = "Unit";
            // 
            // mDoubleLoopTime
            // 
            this.mDoubleLoopTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDoubleLoopTime.DoubleVal = 0D;
            this.mDoubleLoopTime.Enabled = false;
            this.mDoubleLoopTime.Label = "";
            this.mDoubleLoopTime.Location = new System.Drawing.Point(-2, 199);
            this.mDoubleLoopTime.LogChanges = true;
            this.mDoubleLoopTime.Name = "mDoubleLoopTime";
            this.mDoubleLoopTime.Padding = new System.Windows.Forms.Padding(2);
            this.mDoubleLoopTime.Size = new System.Drawing.Size(101, 22);
            this.mDoubleLoopTime.TabIndex = 42;
            this.mDoubleLoopTime.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDoubleLoopTime.UnitsLabel = "Unit";
            // 
            // mcbTimeoutToStopPath
            // 
            this.mcbTimeoutToStopPath.AutoSize = true;
            this.mcbTimeoutToStopPath.Location = new System.Drawing.Point(510, 36);
            this.mcbTimeoutToStopPath.LogChanges = true;
            this.mcbTimeoutToStopPath.Name = "mcbTimeoutToStopPath";
            this.mcbTimeoutToStopPath.Size = new System.Drawing.Size(131, 17);
            this.mcbTimeoutToStopPath.TabIndex = 33;
            this.mcbTimeoutToStopPath.Text = "Time Out to Stop Path";
            this.mcbTimeoutToStopPath.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 534);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Path Out";
            // 
            // cmbDryRunTransitionID
            // 
            this.cmbDryRunTransitionID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbDryRunTransitionID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDryRunTransitionID.FormattingEnabled = true;
            this.cmbDryRunTransitionID.Location = new System.Drawing.Point(79, 531);
            this.cmbDryRunTransitionID.Name = "cmbDryRunTransitionID";
            this.cmbDryRunTransitionID.Size = new System.Drawing.Size(566, 21);
            this.cmbDryRunTransitionID.TabIndex = 44;
            this.cmbDryRunTransitionID.DropDown += new System.EventHandler(this.cmbDryRunTransitionID_DropDown);
            this.cmbDryRunTransitionID.SelectionChangeCommitted += new System.EventHandler(this.cmbDryRunTransitionID_SelectionChangeCommitted);
            // 
            // mcbUseDryRunTrans
            // 
            this.mcbUseDryRunTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mcbUseDryRunTrans.AutoSize = true;
            this.mcbUseDryRunTrans.Location = new System.Drawing.Point(25, 508);
            this.mcbUseDryRunTrans.LogChanges = true;
            this.mcbUseDryRunTrans.Name = "mcbUseDryRunTrans";
            this.mcbUseDryRunTrans.Size = new System.Drawing.Size(136, 17);
            this.mcbUseDryRunTrans.TabIndex = 45;
            this.mcbUseDryRunTrans.Text = "Use Dry Run Transition";
            this.mcbUseDryRunTrans.UseVisualStyleBackColor = true;
            // 
            // panelTransitionEdit
            // 
            this.panelTransitionEdit.BackColor = System.Drawing.Color.Gray;
            this.panelTransitionEdit.Controls.Add(this.mDoubleLoopTime);
            this.panelTransitionEdit.Controls.Add(this.btnNewTrans);
            this.panelTransitionEdit.Controls.Add(this.btnValidate);
            this.panelTransitionEdit.Controls.Add(this.btnDeleteTrans);
            this.panelTransitionEdit.Controls.Add(this.mCbLoopTransition);
            this.panelTransitionEdit.Controls.Add(this.btnMorePrioruty);
            this.panelTransitionEdit.Controls.Add(this.btnLessPriority);
            this.panelTransitionEdit.Location = new System.Drawing.Point(535, 72);
            this.panelTransitionEdit.Name = "panelTransitionEdit";
            this.panelTransitionEdit.Size = new System.Drawing.Size(106, 279);
            this.panelTransitionEdit.TabIndex = 46;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(535, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Edit Transition";
            // 
            // panelConditionEdit
            // 
            this.panelConditionEdit.BackColor = System.Drawing.Color.Gray;
            this.panelConditionEdit.Controls.Add(this.btnAddOR);
            this.panelConditionEdit.Controls.Add(this.btnAddCond);
            this.panelConditionEdit.Controls.Add(this.cmbTransitionID);
            this.panelConditionEdit.Controls.Add(this.btnAddAND);
            this.panelConditionEdit.Controls.Add(this.groupBox1);
            this.panelConditionEdit.Controls.Add(this.btnDelCond);
            this.panelConditionEdit.Controls.Add(this.label5);
            this.panelConditionEdit.Controls.Add(this.btnUpdateCond);
            this.panelConditionEdit.Location = new System.Drawing.Point(27, 367);
            this.panelConditionEdit.Name = "panelConditionEdit";
            this.panelConditionEdit.Size = new System.Drawing.Size(614, 135);
            this.panelConditionEdit.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(23, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Edit Condition";
            // 
            // TransitionEditorCtrlV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panelConditionEdit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelTransitionEdit);
            this.Controls.Add(this.mcbUseDryRunTrans);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDryRunTransitionID);
            this.Controls.Add(this.mcbTimeoutToStopPath);
            this.Controls.Add(this.mDoubleTimeOut);
            this.Controls.Add(this.strTimeOutCaption);
            this.Controls.Add(this.lblTimeOutCaption);
            this.Controls.Add(this.lblTimeOutMsg);
            this.Controls.Add(this.strTimeOutMsg);
            this.Controls.Add(this.treeComponents);
            this.Name = "TransitionEditorCtrlV2";
            this.Size = new System.Drawing.Size(648, 561);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelTransitionEdit.ResumeLayout(false);
            this.panelTransitionEdit.PerformLayout();
            this.panelConditionEdit.ResumeLayout(false);
            this.panelConditionEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDeleteTrans;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeComponents;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddCond;
        private System.Windows.Forms.Button btnAddAND;
        private System.Windows.Forms.Button btnAddOR;
        private System.Windows.Forms.Button btnDelCond;
        private MCore.Controls.MPIDCtl smPropID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbOperator;
        private MCore.Controls.MPIDCtl smValueID;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.ComboBox cmbTransitionID;
        private System.Windows.Forms.Button btnUpdateCond;
        private System.Windows.Forms.Button btnMorePrioruty;
        private System.Windows.Forms.Button btnLessPriority;
        private System.Windows.Forms.Button btnNewTrans;
        private MCore.Controls.MCheckBox mCbLoopTransition;
        private MCore.Controls.StringCtl strTimeOutMsg;
        private System.Windows.Forms.Label lblTimeOutMsg;
        private System.Windows.Forms.Label lblTimeOutCaption;
        private MCore.Controls.StringCtl strTimeOutCaption;
        private MCore.Controls.MDoubleWithUnits mDoubleTimeOut;
        private MCore.Controls.MCheckBox mcbTimeoutToStopPath;
        private MCore.Controls.MDoubleWithUnits mDoubleLoopTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDryRunTransitionID;
        private MCore.Controls.MCheckBox mcbUseDryRunTrans;
        private System.Windows.Forms.Panel panelTransitionEdit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelConditionEdit;
        private System.Windows.Forms.Label label4;
    }
}