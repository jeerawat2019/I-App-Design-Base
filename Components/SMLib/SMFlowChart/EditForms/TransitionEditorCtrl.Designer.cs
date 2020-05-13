namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class TransitionEditorCtrl
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(120, 12);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(523, 20);
            this.tbText.TabIndex = 0;
            this.tbText.TextChanged += new System.EventHandler(this.tbText_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Label text";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(553, 474);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 26);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.smValueID);
            this.groupBox1.Controls.Add(this.cbOperator);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.smPropID);
            this.groupBox1.Location = new System.Drawing.Point(15, 396);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 76);
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
            this.smValueID.Size = new System.Drawing.Size(479, 21);
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
            this.smPropID.Size = new System.Drawing.Size(611, 21);
            this.smPropID.TabIndex = 22;
            this.smPropID.XPos = 0;
            // 
            // treeComponents
            // 
            this.treeComponents.Location = new System.Drawing.Point(30, 65);
            this.treeComponents.Name = "treeComponents";
            this.treeComponents.Size = new System.Drawing.Size(505, 297);
            this.treeComponents.TabIndex = 15;
            this.treeComponents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeComponents_NodeMouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 43);
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
            this.btnAddCond.Location = new System.Drawing.Point(30, 364);
            this.btnAddCond.Name = "btnAddCond";
            this.btnAddCond.Size = new System.Drawing.Size(72, 26);
            this.btnAddCond.TabIndex = 17;
            this.btnAddCond.Text = "Add";
            this.btnAddCond.UseVisualStyleBackColor = false;
            this.btnAddCond.Click += new System.EventHandler(this.btnAddCond_Click);
            // 
            // btnAddAND
            // 
            this.btnAddAND.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAddAND.Location = new System.Drawing.Point(361, 364);
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
            this.btnAddOR.Location = new System.Drawing.Point(451, 364);
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
            this.btnDelCond.Location = new System.Drawing.Point(186, 364);
            this.btnDelCond.Name = "btnDelCond";
            this.btnDelCond.Size = new System.Drawing.Size(70, 26);
            this.btnDelCond.TabIndex = 21;
            this.btnDelCond.Text = "Delete";
            this.btnDelCond.UseVisualStyleBackColor = false;
            this.btnDelCond.Click += new System.EventHandler(this.btnDelCond_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnValidate.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(541, 65);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(84, 26);
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
            this.cmbTransitionID.Location = new System.Drawing.Point(120, 38);
            this.cmbTransitionID.Name = "cmbTransitionID";
            this.cmbTransitionID.Size = new System.Drawing.Size(523, 21);
            this.cmbTransitionID.TabIndex = 26;
            this.cmbTransitionID.DropDown += new System.EventHandler(this.cmbTransitionID_DropDown);
            this.cmbTransitionID.SelectionChangeCommitted += new System.EventHandler(this.cmbTransitionID_SelectionChangeCommitted);
            // 
            // btnUpdateCond
            // 
            this.btnUpdateCond.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnUpdateCond.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnUpdateCond.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCond.Location = new System.Drawing.Point(108, 364);
            this.btnUpdateCond.Name = "btnUpdateCond";
            this.btnUpdateCond.Size = new System.Drawing.Size(72, 26);
            this.btnUpdateCond.TabIndex = 27;
            this.btnUpdateCond.Text = "Update";
            this.btnUpdateCond.UseVisualStyleBackColor = false;
            this.btnUpdateCond.Click += new System.EventHandler(this.btnUpdateCond_Click);
            // 
            // btnClearSelect
            // 
            this.btnClearSelect.BackColor = System.Drawing.SystemColors.Control;
            this.btnClearSelect.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnClearSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSelect.Location = new System.Drawing.Point(15, 474);
            this.btnClearSelect.Name = "btnClearSelect";
            this.btnClearSelect.Size = new System.Drawing.Size(137, 26);
            this.btnClearSelect.TabIndex = 28;
            this.btnClearSelect.Text = "Clear";
            this.btnClearSelect.UseVisualStyleBackColor = false;
            this.btnClearSelect.Click += new System.EventHandler(this.btnClearSelect_Click);
            // 
            // TransitionEditorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClearSelect);
            this.Controls.Add(this.btnUpdateCond);
            this.Controls.Add(this.cmbTransitionID);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnDelCond);
            this.Controls.Add(this.btnAddOR);
            this.Controls.Add(this.btnAddAND);
            this.Controls.Add(this.btnAddCond);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeComponents);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.label1);
            this.Name = "TransitionEditorCtrl";
            this.Size = new System.Drawing.Size(648, 505);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
    }
}