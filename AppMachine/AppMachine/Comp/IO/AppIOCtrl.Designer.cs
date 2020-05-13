namespace AppMachine.Comp.IO
{
    partial class AppIOCtrl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelOutputs = new System.Windows.Forms.Panel();
            this.panelInputs = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(425, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Outputs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inputs";
            // 
            // panelOutputs
            // 
            this.panelOutputs.AutoScroll = true;
            this.panelOutputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOutputs.Location = new System.Drawing.Point(428, 21);
            this.panelOutputs.Name = "panelOutputs";
            this.panelOutputs.Size = new System.Drawing.Size(387, 185);
            this.panelOutputs.TabIndex = 1;
            // 
            // panelInputs
            // 
            this.panelInputs.AutoScroll = true;
            this.panelInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInputs.Location = new System.Drawing.Point(3, 21);
            this.panelInputs.Name = "panelInputs";
            this.panelInputs.Size = new System.Drawing.Size(387, 185);
            this.panelInputs.TabIndex = 0;
            // 
            // IOCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelOutputs);
            this.Controls.Add(this.panelInputs);
            this.Name = "IOCtrl";
            this.Size = new System.Drawing.Size(827, 209);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelInputs;
        private System.Windows.Forms.Panel panelOutputs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
