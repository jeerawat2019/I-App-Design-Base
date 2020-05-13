namespace MCore.Comp.SMLib.SMFlowChart.EditForms
{
    partial class GridEditForm
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
            this.gbGridtem = new System.Windows.Forms.GroupBox();
            this.btnInsertRowAfter = new System.Windows.Forms.Button();
            this.btnInsertColumnAfter = new System.Windows.Forms.Button();
            this.btnInsertColumnBefore = new System.Windows.Forms.Button();
            this.btnInsertRowBefore = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbGridtem.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGridtem
            // 
            this.gbGridtem.Controls.Add(this.panel1);
            this.gbGridtem.Controls.Add(this.btnInsertRowAfter);
            this.gbGridtem.Controls.Add(this.btnInsertColumnAfter);
            this.gbGridtem.Controls.Add(this.btnInsertColumnBefore);
            this.gbGridtem.Controls.Add(this.btnInsertRowBefore);
            this.gbGridtem.Location = new System.Drawing.Point(13, 12);
            this.gbGridtem.Name = "gbGridtem";
            this.gbGridtem.Size = new System.Drawing.Size(428, 185);
            this.gbGridtem.TabIndex = 3;
            this.gbGridtem.TabStop = false;
            this.gbGridtem.Text = "Grid Edit";
            // 
            // btnInsertRowAfter
            // 
            this.btnInsertRowAfter.Location = new System.Drawing.Point(144, 134);
            this.btnInsertRowAfter.Name = "btnInsertRowAfter";
            this.btnInsertRowAfter.Size = new System.Drawing.Size(140, 32);
            this.btnInsertRowAfter.TabIndex = 1;
            this.btnInsertRowAfter.Text = "Insert Row After";
            this.btnInsertRowAfter.UseVisualStyleBackColor = true;
            this.btnInsertRowAfter.Click += new System.EventHandler(this.btnInsertRowAfter_Click);
            // 
            // btnInsertColumnAfter
            // 
            this.btnInsertColumnAfter.Location = new System.Drawing.Point(293, 83);
            this.btnInsertColumnAfter.Name = "btnInsertColumnAfter";
            this.btnInsertColumnAfter.Size = new System.Drawing.Size(126, 32);
            this.btnInsertColumnAfter.TabIndex = 3;
            this.btnInsertColumnAfter.Text = "Insert Column After";
            this.btnInsertColumnAfter.UseVisualStyleBackColor = true;
            this.btnInsertColumnAfter.Click += new System.EventHandler(this.btnInsertColumnAfter_Click);
            // 
            // btnInsertColumnBefore
            // 
            this.btnInsertColumnBefore.Location = new System.Drawing.Point(15, 83);
            this.btnInsertColumnBefore.Name = "btnInsertColumnBefore";
            this.btnInsertColumnBefore.Size = new System.Drawing.Size(119, 32);
            this.btnInsertColumnBefore.TabIndex = 2;
            this.btnInsertColumnBefore.Text = "Insert Column Before";
            this.btnInsertColumnBefore.UseVisualStyleBackColor = true;
            this.btnInsertColumnBefore.Click += new System.EventHandler(this.btnInsertColumnBefore_Click);
            // 
            // btnInsertRowBefore
            // 
            this.btnInsertRowBefore.Location = new System.Drawing.Point(144, 26);
            this.btnInsertRowBefore.Name = "btnInsertRowBefore";
            this.btnInsertRowBefore.Size = new System.Drawing.Size(140, 32);
            this.btnInsertRowBefore.TabIndex = 0;
            this.btnInsertRowBefore.Text = "Insert Row Before";
            this.btnInsertRowBefore.UseVisualStyleBackColor = true;
            this.btnInsertRowBefore.Click += new System.EventHandler(this.btnInsertRowBefore_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MCore.Comp.SMLib.SMFlowChart.Properties.Resources.GridBackground;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(144, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 51);
            this.panel1.TabIndex = 4;
            // 
            // GridEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.ClientSize = new System.Drawing.Size(454, 214);
            this.Controls.Add(this.gbGridtem);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GridEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewItemForm";
            this.gbGridtem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGridtem;
        private System.Windows.Forms.Button btnInsertRowAfter;
        private System.Windows.Forms.Button btnInsertRowBefore;
        private System.Windows.Forms.Button btnInsertColumnAfter;
        private System.Windows.Forms.Button btnInsertColumnBefore;
        private System.Windows.Forms.Panel panel1;
    }
}