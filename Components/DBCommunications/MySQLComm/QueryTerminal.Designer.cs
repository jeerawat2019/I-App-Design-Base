namespace MCore.Comp.DBCommunications
{
    partial class QueryTerminal
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
            this.btnExecute = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dgQueryResult = new System.Windows.Forms.DataGridView();
            this.strQueryCommand = new MCore.Controls.StringCtl();
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryResult)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(743, 18);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(62, 23);
            this.btnExecute.TabIndex = 6;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Command";
            // 
            // dgQueryResult
            // 
            this.dgQueryResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgQueryResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgQueryResult.Location = new System.Drawing.Point(16, 222);
            this.dgQueryResult.Name = "dgQueryResult";
            this.dgQueryResult.Size = new System.Drawing.Size(789, 336);
            this.dgQueryResult.TabIndex = 7;
            // 
            // strQueryCommand
            // 
            this.strQueryCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.strQueryCommand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.strQueryCommand.Location = new System.Drawing.Point(67, 18);
            this.strQueryCommand.LogChanges = true;
            this.strQueryCommand.Multiline = true;
            this.strQueryCommand.Name = "strQueryCommand";
            this.strQueryCommand.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.strQueryCommand.Size = new System.Drawing.Size(670, 198);
            this.strQueryCommand.TabIndex = 8;
            // 
            // QueryTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.strQueryCommand);
            this.Controls.Add(this.dgQueryResult);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.label1);
            this.Name = "QueryTerminal";
            this.Size = new System.Drawing.Size(821, 601);
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgQueryResult;
        private Controls.StringCtl strQueryCommand;
    }
}
