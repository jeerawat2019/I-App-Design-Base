﻿namespace AppMachine.Panel
{
    partial class AppGeneralPanel
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
            this.btnCopyRecipe = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMasterCopyRecipe = new System.Windows.Forms.ComboBox();
            this.pgRecipe = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCurrentRecipe = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCopyRecipe
            // 
            this.btnCopyRecipe.Location = new System.Drawing.Point(896, 29);
            this.btnCopyRecipe.Name = "btnCopyRecipe";
            this.btnCopyRecipe.Size = new System.Drawing.Size(89, 23);
            this.btnCopyRecipe.TabIndex = 13;
            this.btnCopyRecipe.Text = "COPY RECIPE";
            this.btnCopyRecipe.UseVisualStyleBackColor = true;
            this.btnCopyRecipe.Click += new System.EventHandler(this.btnCopyRecipe_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(643, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "MASTER COPY RECIPE";
            // 
            // cbMasterCopyRecipe
            // 
            this.cbMasterCopyRecipe.FormattingEnabled = true;
            this.cbMasterCopyRecipe.Location = new System.Drawing.Point(646, 29);
            this.cbMasterCopyRecipe.Name = "cbMasterCopyRecipe";
            this.cbMasterCopyRecipe.Size = new System.Drawing.Size(244, 21);
            this.cbMasterCopyRecipe.TabIndex = 11;
            this.cbMasterCopyRecipe.DropDown += new System.EventHandler(this.cbCopyRecipe_DropDown);
            this.cbMasterCopyRecipe.SelectionChangeCommitted += new System.EventHandler(this.cbCopyRecipe_SelectionChangeCommitted);
            // 
            // pgRecipe
            // 
            this.pgRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgRecipe.Location = new System.Drawing.Point(6, 56);
            this.pgRecipe.Name = "pgRecipe";
            this.pgRecipe.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgRecipe.Size = new System.Drawing.Size(1240, 844);
            this.pgRecipe.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "CURRENT RECIPE";
            // 
            // cbCurrentRecipe
            // 
            this.cbCurrentRecipe.FormattingEnabled = true;
            this.cbCurrentRecipe.Location = new System.Drawing.Point(7, 29);
            this.cbCurrentRecipe.Name = "cbCurrentRecipe";
            this.cbCurrentRecipe.Size = new System.Drawing.Size(244, 21);
            this.cbCurrentRecipe.TabIndex = 8;
            this.cbCurrentRecipe.DropDown += new System.EventHandler(this.cbCurrentRecipe_DropDown);
            this.cbCurrentRecipe.SelectionChangeCommitted += new System.EventHandler(this.cbCurrentRecipe_SelectionChangeCommitted);
            // 
            // AppGeneralPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.btnCopyRecipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbMasterCopyRecipe);
            this.Controls.Add(this.pgRecipe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCurrentRecipe);
            this.Name = "AppGeneralPanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCurrentRecipe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PropertyGrid pgRecipe;
        private System.Windows.Forms.ComboBox cbMasterCopyRecipe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCopyRecipe;

    }
}
