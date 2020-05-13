namespace MCore.Comp.VisionSystem
{
    partial class CognexJobEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelExtraCommands = new System.Windows.Forms.Panel();
            this.btnImportFromFile = new System.Windows.Forms.Button();
            this.panelToolGroupEdit = new System.Windows.Forms.Panel();
            this.btnNextHelp = new System.Windows.Forms.Button();
            this.btnPrevHelp = new System.Windows.Forms.Button();
            this.panelExtraCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelExtraCommands
            // 
            this.panelExtraCommands.Controls.Add(this.btnPrevHelp);
            this.panelExtraCommands.Controls.Add(this.btnNextHelp);
            this.panelExtraCommands.Controls.Add(this.btnImportFromFile);
            this.panelExtraCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelExtraCommands.Location = new System.Drawing.Point(0, 294);
            this.panelExtraCommands.Name = "panelExtraCommands";
            this.panelExtraCommands.Size = new System.Drawing.Size(656, 32);
            this.panelExtraCommands.TabIndex = 0;
            // 
            // btnImportFromFile
            // 
            this.btnImportFromFile.Location = new System.Drawing.Point(12, 6);
            this.btnImportFromFile.Name = "btnImportFromFile";
            this.btnImportFromFile.Size = new System.Drawing.Size(189, 23);
            this.btnImportFromFile.TabIndex = 0;
            this.btnImportFromFile.Text = "Import Tools From vpp File";
            this.btnImportFromFile.UseVisualStyleBackColor = true;
            this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
            // 
            // panelToolGroupEdit
            // 
            this.panelToolGroupEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolGroupEdit.Location = new System.Drawing.Point(0, 0);
            this.panelToolGroupEdit.Name = "panelToolGroupEdit";
            this.panelToolGroupEdit.Size = new System.Drawing.Size(656, 294);
            this.panelToolGroupEdit.TabIndex = 1;
            // 
            // btnNextHelp
            // 
            this.btnNextHelp.Location = new System.Drawing.Point(404, 6);
            this.btnNextHelp.Name = "btnNextHelp";
            this.btnNextHelp.Size = new System.Drawing.Size(75, 23);
            this.btnNextHelp.TabIndex = 1;
            this.btnNextHelp.Text = "Next Help";
            this.btnNextHelp.UseVisualStyleBackColor = true;
            this.btnNextHelp.Visible = false;
            this.btnNextHelp.Click += new System.EventHandler(this.btnNextHelp_Click);
            // 
            // btnPrevHelp
            // 
            this.btnPrevHelp.Location = new System.Drawing.Point(291, 5);
            this.btnPrevHelp.Name = "btnPrevHelp";
            this.btnPrevHelp.Size = new System.Drawing.Size(75, 23);
            this.btnPrevHelp.TabIndex = 2;
            this.btnPrevHelp.Text = "Prev Help";
            this.btnPrevHelp.UseVisualStyleBackColor = true;
            this.btnPrevHelp.Visible = false;
            this.btnPrevHelp.Click += new System.EventHandler(this.btnPrevHelp_Click);
            // 
            // CognexJobEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 326);
            this.Controls.Add(this.panelToolGroupEdit);
            this.Controls.Add(this.panelExtraCommands);
            this.Name = "CognexJobEditForm";
            this.Text = "CognexJobEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CognexJobEditForm_FormClosing);
            this.Load += new System.EventHandler(this.CognexJobEditForm_Load);
            this.panelExtraCommands.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelExtraCommands;
        private System.Windows.Forms.Panel panelToolGroupEdit;
        private System.Windows.Forms.Button btnImportFromFile;
        private System.Windows.Forms.Button btnPrevHelp;
        private System.Windows.Forms.Button btnNextHelp;
    }
}