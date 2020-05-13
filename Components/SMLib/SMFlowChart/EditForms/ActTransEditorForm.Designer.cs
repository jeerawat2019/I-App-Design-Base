namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class ActTransEditorForm
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
            this.gbMethods = new System.Windows.Forms.GroupBox();
            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.panelActions = new System.Windows.Forms.Panel();
            this.tcTransitions = new System.Windows.Forms.TabControl();
            this.btnAddTransition = new System.Windows.Forms.Button();
            this.gbMethods.SuspendLayout();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(778, 692);
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
            this.btnCancel.Location = new System.Drawing.Point(970, 692);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 40);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbText
            // 
            this.tbText.Location = new System.Drawing.Point(74, 12);
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(191, 20);
            this.tbText.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Label text";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(874, 692);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 40);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // gbMethods
            // 
            this.gbMethods.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMethods.Controls.Add(this.btnRemoveItem);
            this.gbMethods.Location = new System.Drawing.Point(9, 3);
            this.gbMethods.Name = "gbMethods";
            this.gbMethods.Size = new System.Drawing.Size(1046, 31);
            this.gbMethods.TabIndex = 5;
            this.gbMethods.TabStop = false;
            this.gbMethods.Text = "Asynchronous Methods";
            // 
            // btnRemoveItem
            // 
            this.btnRemoveItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveItem.Location = new System.Drawing.Point(1020, 10);
            this.btnRemoveItem.Name = "btnRemoveItem";
            this.btnRemoveItem.Size = new System.Drawing.Size(20, 21);
            this.btnRemoveItem.TabIndex = 1;
            this.btnRemoveItem.Text = "X";
            this.btnRemoveItem.UseVisualStyleBackColor = true;
            this.btnRemoveItem.Visible = false;
            this.btnRemoveItem.Click += new System.EventHandler(this.OnRemoveItem);
            // 
            // panelActions
            // 
            this.panelActions.AutoScroll = true;
            this.panelActions.Controls.Add(this.gbMethods);
            this.panelActions.Location = new System.Drawing.Point(3, 38);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(1061, 156);
            this.panelActions.TabIndex = 6;
            // 
            // tcTransitions
            // 
            this.tcTransitions.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcTransitions.ItemSize = new System.Drawing.Size(60, 18);
            this.tcTransitions.Location = new System.Drawing.Point(3, 200);
            this.tcTransitions.Name = "tcTransitions";
            this.tcTransitions.SelectedIndex = 0;
            this.tcTransitions.Size = new System.Drawing.Size(673, 539);
            this.tcTransitions.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcTransitions.TabIndex = 7;
            this.tcTransitions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tcTransitions_DrawItem);
            // 
            // btnAddTransition
            // 
            this.btnAddTransition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnAddTransition.Location = new System.Drawing.Point(682, 203);
            this.btnAddTransition.Name = "btnAddTransition";
            this.btnAddTransition.Size = new System.Drawing.Size(99, 34);
            this.btnAddTransition.TabIndex = 8;
            this.btnAddTransition.Text = "AddTransition";
            this.btnAddTransition.UseVisualStyleBackColor = false;
            this.btnAddTransition.Click += new System.EventHandler(this.btnAddTransition_Click);
            // 
            // ActTransEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 742);
            this.Controls.Add(this.btnAddTransition);
            this.Controls.Add(this.tcTransitions);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActTransEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Action Transition Edit Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.gbMethods.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox gbMethods;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.TabControl tcTransitions;
        private System.Windows.Forms.Button btnAddTransition;
    }
}