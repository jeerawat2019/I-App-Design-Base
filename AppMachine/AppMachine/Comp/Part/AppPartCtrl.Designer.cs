namespace AppMachine.Comp.Part
{
    partial class AppPartCtrl
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
            this.lblInfo = new MCore.Controls.LabelCtl();
            this.lblPartId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.Yellow;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblInfo.Location = new System.Drawing.Point(2, 14);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(27, 11);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Info";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPartId
            // 
            this.lblPartId.BackColor = System.Drawing.Color.Yellow;
            this.lblPartId.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblPartId.Location = new System.Drawing.Point(2, 1);
            this.lblPartId.Margin = new System.Windows.Forms.Padding(0);
            this.lblPartId.Name = "lblPartId";
            this.lblPartId.Size = new System.Drawing.Size(27, 12);
            this.lblPartId.TabIndex = 0;
            this.lblPartId.Text = "000";
            this.lblPartId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AppPartCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblPartId);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "AppPartCtrl";
            this.Size = new System.Drawing.Size(31, 26);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPartId;
        private MCore.Controls.LabelCtl lblInfo;
    }
}
