namespace AppMachine.Control
{
    partial class AppFloatableSMOperate
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
            this.linkLblFloat = new System.Windows.Forms.LinkLabel();
            this.lblParamName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkLblFloat
            // 
            this.linkLblFloat.AutoSize = true;
            this.linkLblFloat.Location = new System.Drawing.Point(684, 6);
            this.linkLblFloat.Name = "linkLblFloat";
            this.linkLblFloat.Size = new System.Drawing.Size(30, 13);
            this.linkLblFloat.TabIndex = 13;
            this.linkLblFloat.TabStop = true;
            this.linkLblFloat.Text = "Float";
            this.linkLblFloat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblFloat_LinkClicked);
            // 
            // lblParamName
            // 
            this.lblParamName.AutoSize = true;
            this.lblParamName.Location = new System.Drawing.Point(493, 6);
            this.lblParamName.Name = "lblParamName";
            this.lblParamName.Size = new System.Drawing.Size(0, 13);
            this.lblParamName.TabIndex = 15;
            // 
            // AppFloatableSMOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.lblParamName);
            this.Controls.Add(this.linkLblFloat);
            this.Name = "AppFloatableSMOperate";
            this.Size = new System.Drawing.Size(717, 26);
            this.Controls.SetChildIndex(this.linkLblFloat, 0);
            this.Controls.SetChildIndex(this.lblParamName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLblFloat;
        private System.Windows.Forms.Label lblParamName;
    }
}
