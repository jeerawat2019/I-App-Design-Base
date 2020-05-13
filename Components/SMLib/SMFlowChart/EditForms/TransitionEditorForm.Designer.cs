namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class TransitionEditorForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
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
            this.btnClearSelect = new System.Windows.Forms.Button();
            this.lblTimeOutCaption = new System.Windows.Forms.Label();
            this.lblTimeOutMsg = new System.Windows.Forms.Label();
            this.mcbTimeoutToStopPath = new MCore.Controls.MCheckBox();
            this.mCbLoopTransition = new MCore.Controls.MCheckBox();
            this.mDoubleTimeOut = new MCore.Controls.MDoubleWithUnits();
            this.strTimeOutCaption = new MCore.Controls.StringCtl();
            this.strTimeOutMsg = new MCore.Controls.StringCtl();
            this.mDoubleLoopTime = new MCore.Controls.MDoubleWithUnits();
            this.mcbUseDryRunTrans = new MCore.Controls.MCheckBox();
            this.cmbDryRunTransitionID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(633, 582);
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
            this.btnCancel.Location = new System.Drawing.Point(825, 582);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(120, 12);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(793, 20);
            this.tbText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Label text";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(729, 582);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.smValueID);
            this.groupBox1.Controls.Add(this.cbOperator);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.smPropID);
            this.groupBox1.Location = new System.Drawing.Point(15, 483);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(898, 93);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conditional Select";
            // 
            // smValueID
            // 
            this.smValueID.AutoLastType = false;
            this.smValueID.ID = "";
            this.smValueID.Location = new System.Drawing.Point(143, 64);
            this.smValueID.LogChanges = true;
            this.smValueID.Name = "smValueID";
            this.smValueID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            this.smValueID.ScopeID = "";
            this.smValueID.Size = new System.Drawing.Size(749, 21);
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
            this.cbOperator.Location = new System.Drawing.Point(93, 64);
            this.cbOperator.Name = "cbOperator";
            this.cbOperator.Size = new System.Drawing.Size(44, 21);
            this.cbOperator.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Condition Value";
            // 
            // smPropID
            // 
            this.smPropID.AutoLastType = false;
            this.smPropID.ID = "";
            this.smPropID.Location = new System.Drawing.Point(11, 24);
            this.smPropID.LogChanges = true;
            this.smPropID.Name = "smPropID";
            this.smPropID.ReturnType = MCore.Controls.MPIDCtl.eTypes.Object;
            this.smPropID.ScopeID = "";
            this.smPropID.Size = new System.Drawing.Size(881, 21);
            this.smPropID.TabIndex = 22;
            this.smPropID.XPos = 0;
            // 
            // treeComponents
            // 
            this.treeComponents.Location = new System.Drawing.Point(120, 89);
            this.treeComponents.Name = "treeComponents";
            this.treeComponents.Size = new System.Drawing.Size(790, 328);
            this.treeComponents.TabIndex = 15;
            this.treeComponents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeComponents_NodeMouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 459);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Path Out";
            // 
            // btnAddCond
            // 
            this.btnAddCond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCond.BackColor = System.Drawing.Color.White;
            this.btnAddCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCond.Location = new System.Drawing.Point(120, 421);
            this.btnAddCond.Name = "btnAddCond";
            this.btnAddCond.Size = new System.Drawing.Size(94, 26);
            this.btnAddCond.TabIndex = 17;
            this.btnAddCond.Text = "Add";
            this.btnAddCond.UseVisualStyleBackColor = false;
            this.btnAddCond.Click += new System.EventHandler(this.btnAddCond_Click);
            // 
            // btnAddAND
            // 
            this.btnAddAND.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAND.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddAND.Location = new System.Drawing.Point(646, 421);
            this.btnAddAND.Name = "btnAddAND";
            this.btnAddAND.Size = new System.Drawing.Size(70, 26);
            this.btnAddAND.TabIndex = 18;
            this.btnAddAND.Text = "Add AND";
            this.btnAddAND.UseVisualStyleBackColor = true;
            this.btnAddAND.Click += new System.EventHandler(this.btnAddAND_Click);
            // 
            // btnAddOR
            // 
            this.btnAddOR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOR.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddOR.Location = new System.Drawing.Point(722, 421);
            this.btnAddOR.Name = "btnAddOR";
            this.btnAddOR.Size = new System.Drawing.Size(70, 26);
            this.btnAddOR.TabIndex = 19;
            this.btnAddOR.Text = "Add OR";
            this.btnAddOR.UseVisualStyleBackColor = true;
            this.btnAddOR.Click += new System.EventHandler(this.btnAddOR_Click);
            // 
            // btnDelCond
            // 
            this.btnDelCond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnDelCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelCond.Location = new System.Drawing.Point(320, 421);
            this.btnDelCond.Name = "btnDelCond";
            this.btnDelCond.Size = new System.Drawing.Size(70, 26);
            this.btnDelCond.TabIndex = 21;
            this.btnDelCond.Text = "Delete";
            this.btnDelCond.UseVisualStyleBackColor = false;
            this.btnDelCond.Click += new System.EventHandler(this.btnDelCond_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnValidate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnValidate.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(816, 421);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(94, 26);
            this.btnValidate.TabIndex = 22;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = false;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // cmbTransitionID
            // 
            this.cmbTransitionID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTransitionID.FormattingEnabled = true;
            this.cmbTransitionID.Items.AddRange(new object[] {
            "==",
            "!=",
            ">",
            ">=",
            "<",
            "<="});
            this.cmbTransitionID.Location = new System.Drawing.Point(76, 456);
            this.cmbTransitionID.Name = "cmbTransitionID";
            this.cmbTransitionID.Size = new System.Drawing.Size(837, 21);
            this.cmbTransitionID.TabIndex = 26;
            this.cmbTransitionID.DropDown += new System.EventHandler(this.cmbTransitionID_DropDown);
            this.cmbTransitionID.SelectionChangeCommitted += new System.EventHandler(this.cmbTransitionID_SelectionChangeCommitted);
            // 
            // btnUpdateCond
            // 
            this.btnUpdateCond.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnUpdateCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnUpdateCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCond.Location = new System.Drawing.Point(220, 421);
            this.btnUpdateCond.Name = "btnUpdateCond";
            this.btnUpdateCond.Size = new System.Drawing.Size(94, 26);
            this.btnUpdateCond.TabIndex = 27;
            this.btnUpdateCond.Text = "Update";
            this.btnUpdateCond.UseVisualStyleBackColor = false;
            this.btnUpdateCond.Click += new System.EventHandler(this.btnUpdateCond_Click);
            // 
            // btnClearSelect
            // 
            this.btnClearSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearSelect.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnClearSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSelect.Location = new System.Drawing.Point(158, 576);
            this.btnClearSelect.Name = "btnClearSelect";
            this.btnClearSelect.Size = new System.Drawing.Size(137, 26);
            this.btnClearSelect.TabIndex = 28;
            this.btnClearSelect.Text = "Clear";
            this.btnClearSelect.UseVisualStyleBackColor = false;
            this.btnClearSelect.Click += new System.EventHandler(this.btnClearSelect_Click);
            // 
            // lblTimeOutCaption
            // 
            this.lblTimeOutCaption.AutoSize = true;
            this.lblTimeOutCaption.Location = new System.Drawing.Point(21, 44);
            this.lblTimeOutCaption.Name = "lblTimeOutCaption";
            this.lblTimeOutCaption.Size = new System.Drawing.Size(89, 13);
            this.lblTimeOutCaption.TabIndex = 42;
            this.lblTimeOutCaption.Text = "Time Out Caption";
            // 
            // lblTimeOutMsg
            // 
            this.lblTimeOutMsg.AutoSize = true;
            this.lblTimeOutMsg.Location = new System.Drawing.Point(21, 66);
            this.lblTimeOutMsg.Name = "lblTimeOutMsg";
            this.lblTimeOutMsg.Size = new System.Drawing.Size(96, 13);
            this.lblTimeOutMsg.TabIndex = 41;
            this.lblTimeOutMsg.Text = "Time Out Message";
            // 
            // mcbTimeoutToStopPath
            // 
            this.mcbTimeoutToStopPath.AutoSize = true;
            this.mcbTimeoutToStopPath.Location = new System.Drawing.Point(646, 66);
            this.mcbTimeoutToStopPath.LogChanges = true;
            this.mcbTimeoutToStopPath.Name = "mcbTimeoutToStopPath";
            this.mcbTimeoutToStopPath.Size = new System.Drawing.Size(131, 17);
            this.mcbTimeoutToStopPath.TabIndex = 46;
            this.mcbTimeoutToStopPath.Text = "Time Out to Stop Path";
            this.mcbTimeoutToStopPath.UseVisualStyleBackColor = true;
            // 
            // mCbLoopTransition
            // 
            this.mCbLoopTransition.AutoSize = true;
            this.mCbLoopTransition.Location = new System.Drawing.Point(807, 66);
            this.mCbLoopTransition.LogChanges = true;
            this.mCbLoopTransition.Name = "mCbLoopTransition";
            this.mCbLoopTransition.Size = new System.Drawing.Size(99, 17);
            this.mCbLoopTransition.TabIndex = 45;
            this.mCbLoopTransition.Text = "Loop Transition";
            this.mCbLoopTransition.UseVisualStyleBackColor = true;
            // 
            // mDoubleTimeOut
            // 
            this.mDoubleTimeOut.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDoubleTimeOut.DoubleVal = 0D;
            this.mDoubleTimeOut.Enabled = false;
            this.mDoubleTimeOut.Label = "";
            this.mDoubleTimeOut.Location = new System.Drawing.Point(428, 36);
            this.mDoubleTimeOut.LogChanges = true;
            this.mDoubleTimeOut.Name = "mDoubleTimeOut";
            this.mDoubleTimeOut.Padding = new System.Windows.Forms.Padding(2);
            this.mDoubleTimeOut.Size = new System.Drawing.Size(215, 22);
            this.mDoubleTimeOut.TabIndex = 44;
            this.mDoubleTimeOut.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDoubleTimeOut.UnitsLabel = "Unit";
            // 
            // strTimeOutCaption
            // 
            this.strTimeOutCaption.Location = new System.Drawing.Point(120, 37);
            this.strTimeOutCaption.LogChanges = true;
            this.strTimeOutCaption.Name = "strTimeOutCaption";
            this.strTimeOutCaption.Size = new System.Drawing.Size(301, 20);
            this.strTimeOutCaption.TabIndex = 43;
            // 
            // strTimeOutMsg
            // 
            this.strTimeOutMsg.Location = new System.Drawing.Point(120, 63);
            this.strTimeOutMsg.LogChanges = true;
            this.strTimeOutMsg.Name = "strTimeOutMsg";
            this.strTimeOutMsg.Size = new System.Drawing.Size(518, 20);
            this.strTimeOutMsg.TabIndex = 40;
            // 
            // mDoubleLoopTime
            // 
            this.mDoubleLoopTime.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mDoubleLoopTime.DoubleVal = 0D;
            this.mDoubleLoopTime.Enabled = false;
            this.mDoubleLoopTime.Label = "";
            this.mDoubleLoopTime.Location = new System.Drawing.Point(802, 38);
            this.mDoubleLoopTime.LogChanges = true;
            this.mDoubleLoopTime.Name = "mDoubleLoopTime";
            this.mDoubleLoopTime.Padding = new System.Windows.Forms.Padding(2);
            this.mDoubleLoopTime.Size = new System.Drawing.Size(101, 22);
            this.mDoubleLoopTime.TabIndex = 47;
            this.mDoubleLoopTime.TextBackColor = System.Drawing.SystemColors.Window;
            this.mDoubleLoopTime.UnitsLabel = "Unit";
            // 
            // mcbUseDryRunTrans
            // 
            this.mcbUseDryRunTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mcbUseDryRunTrans.AutoSize = true;
            this.mcbUseDryRunTrans.Location = new System.Drawing.Point(16, 582);
            this.mcbUseDryRunTrans.LogChanges = true;
            this.mcbUseDryRunTrans.Name = "mcbUseDryRunTrans";
            this.mcbUseDryRunTrans.Size = new System.Drawing.Size(136, 17);
            this.mcbUseDryRunTrans.TabIndex = 48;
            this.mcbUseDryRunTrans.Text = "Use Dry Run Transition";
            this.mcbUseDryRunTrans.UseVisualStyleBackColor = true;
            // 
            // cmbDryRunTransitionID
            // 
            this.cmbDryRunTransitionID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbDryRunTransitionID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDryRunTransitionID.FormattingEnabled = true;
            this.cmbDryRunTransitionID.Items.AddRange(new object[] {
            "==",
            "!=",
            ">",
            ">=",
            "<",
            "<="});
            this.cmbDryRunTransitionID.Location = new System.Drawing.Point(69, 605);
            this.cmbDryRunTransitionID.Name = "cmbDryRunTransitionID";
            this.cmbDryRunTransitionID.Size = new System.Drawing.Size(558, 21);
            this.cmbDryRunTransitionID.TabIndex = 50;
            this.cmbDryRunTransitionID.DropDown += new System.EventHandler(this.cmbDryRunTransitionID_DropDown);
            this.cmbDryRunTransitionID.SelectionChangeCommitted += new System.EventHandler(this.cmbDryRunTransitionID_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 608);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Path Out";
            // 
            // TransitionEditorForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(922, 632);
            this.Controls.Add(this.cmbDryRunTransitionID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mcbUseDryRunTrans);
            this.Controls.Add(this.mDoubleLoopTime);
            this.Controls.Add(this.mcbTimeoutToStopPath);
            this.Controls.Add(this.mCbLoopTransition);
            this.Controls.Add(this.mDoubleTimeOut);
            this.Controls.Add(this.strTimeOutCaption);
            this.Controls.Add(this.lblTimeOutCaption);
            this.Controls.Add(this.cmbTransitionID);
            this.Controls.Add(this.lblTimeOutMsg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.strTimeOutMsg);
            this.Controls.Add(this.btnClearSelect);
            this.Controls.Add(this.btnUpdateCond);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnDelCond);
            this.Controls.Add(this.btnAddOR);
            this.Controls.Add(this.btnAddAND);
            this.Controls.Add(this.btnAddCond);
            this.Controls.Add(this.treeComponents);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransitionEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decision Flow Element Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TransitionEditorForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDelete;
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
        private System.Windows.Forms.Button btnClearSelect;
        private MCore.Controls.MDoubleWithUnits mDoubleTimeOut;
        private MCore.Controls.StringCtl strTimeOutCaption;
        private System.Windows.Forms.Label lblTimeOutCaption;
        private System.Windows.Forms.Label lblTimeOutMsg;
        private MCore.Controls.StringCtl strTimeOutMsg;
        private MCore.Controls.MCheckBox mCbLoopTransition;
        private MCore.Controls.MCheckBox mcbTimeoutToStopPath;
        private MCore.Controls.MDoubleWithUnits mDoubleLoopTime;
        private MCore.Controls.MCheckBox mcbUseDryRunTrans;
        private System.Windows.Forms.ComboBox cmbDryRunTransitionID;
        private System.Windows.Forms.Label label3;
    }
}