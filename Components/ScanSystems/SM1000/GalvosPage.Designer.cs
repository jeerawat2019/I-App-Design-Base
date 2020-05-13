namespace MCore.Comp.ScanSystem
{
    partial class GalvosPage
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
            this.galvoX = new MCore.Comp.MotionSystem.Axis.RealAxisPage();
            this.galvoY = new MCore.Comp.MotionSystem.Axis.RealAxisPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // galvoX
            // 
            this.galvoX.Location = new System.Drawing.Point(79, 44);
            this.galvoX.Name = "galvoX";
            this.galvoX.Size = new System.Drawing.Size(381, 179);
            this.galvoX.TabIndex = 0;
            // 
            // galvoY
            // 
            this.galvoY.Location = new System.Drawing.Point(79, 244);
            this.galvoY.Name = "galvoY";
            this.galvoY.Size = new System.Drawing.Size(381, 179);
            this.galvoY.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Galvo X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Galvo Y";
            // 
            // GalvosPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.galvoY);
            this.Controls.Add(this.galvoX);
            this.Name = "GalvosPage";
            this.Size = new System.Drawing.Size(473, 444);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MotionSystem.Axis.RealAxisPage galvoX;
        private MotionSystem.Axis.RealAxisPage galvoY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
