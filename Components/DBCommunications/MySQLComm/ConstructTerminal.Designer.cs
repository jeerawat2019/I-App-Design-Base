namespace MCore.Comp.DBCommunications
{
    partial class ConstructTerminal
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
            this.btnCreateExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConstructExecute = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdateExecute = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.strUpdate = new MCore.Controls.StringCtl();
            this.strConstruct = new MCore.Controls.StringCtl();
            this.strCreate = new MCore.Controls.StringCtl();
            this.SuspendLayout();
            // 
            // btnCreateExecute
            // 
            this.btnCreateExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateExecute.Location = new System.Drawing.Point(743, 18);
            this.btnCreateExecute.Name = "btnCreateExecute";
            this.btnCreateExecute.Size = new System.Drawing.Size(62, 23);
            this.btnCreateExecute.TabIndex = 6;
            this.btnCreateExecute.Text = "Execute";
            this.btnCreateExecute.UseVisualStyleBackColor = true;
            this.btnCreateExecute.Click += new System.EventHandler(this.btnCreateExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Create Command";
            // 
            // btnConstructExecute
            // 
            this.btnConstructExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConstructExecute.Location = new System.Drawing.Point(743, 155);
            this.btnConstructExecute.Name = "btnConstructExecute";
            this.btnConstructExecute.Size = new System.Drawing.Size(62, 23);
            this.btnConstructExecute.TabIndex = 9;
            this.btnConstructExecute.Text = "Execute";
            this.btnConstructExecute.UseVisualStyleBackColor = true;
            this.btnConstructExecute.Click += new System.EventHandler(this.btnConstructExecute_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Construct Command";
            // 
            // btnUpdateExecute
            // 
            this.btnUpdateExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateExecute.Location = new System.Drawing.Point(743, 395);
            this.btnUpdateExecute.Name = "btnUpdateExecute";
            this.btnUpdateExecute.Size = new System.Drawing.Size(62, 23);
            this.btnUpdateExecute.TabIndex = 12;
            this.btnUpdateExecute.Text = "Execute";
            this.btnUpdateExecute.UseVisualStyleBackColor = true;
            this.btnUpdateExecute.Click += new System.EventHandler(this.btnUpdateExecute_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 395);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Update Command";
            // 
            // strUpdate
            // 
            this.strUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.strUpdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.strUpdate.Location = new System.Drawing.Point(133, 397);
            this.strUpdate.LogChanges = true;
            this.strUpdate.Multiline = true;
            this.strUpdate.Name = "strUpdate";
            this.strUpdate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strUpdate.Size = new System.Drawing.Size(604, 227);
            this.strUpdate.TabIndex = 15;
            // 
            // strConstruct
            // 
            this.strConstruct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.strConstruct.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.strConstruct.Location = new System.Drawing.Point(133, 152);
            this.strConstruct.LogChanges = true;
            this.strConstruct.Multiline = true;
            this.strConstruct.Name = "strConstruct";
            this.strConstruct.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strConstruct.Size = new System.Drawing.Size(604, 227);
            this.strConstruct.TabIndex = 14;
            // 
            // strCreate
            // 
            this.strCreate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.strCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.strCreate.Location = new System.Drawing.Point(133, 20);
            this.strCreate.LogChanges = true;
            this.strCreate.Multiline = true;
            this.strCreate.Name = "strCreate";
            this.strCreate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strCreate.Size = new System.Drawing.Size(604, 115);
            this.strCreate.TabIndex = 13;
            // 
            // ConstructTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.strUpdate);
            this.Controls.Add(this.strConstruct);
            this.Controls.Add(this.strCreate);
            this.Controls.Add(this.btnUpdateExecute);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConstructExecute);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCreateExecute);
            this.Controls.Add(this.label1);
            this.Name = "ConstructTerminal";
            this.Size = new System.Drawing.Size(821, 650);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConstructExecute;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdateExecute;
        private System.Windows.Forms.Label label3;
        private Controls.StringCtl strCreate;
        private Controls.StringCtl strConstruct;
        private Controls.StringCtl strUpdate;
    }
}
