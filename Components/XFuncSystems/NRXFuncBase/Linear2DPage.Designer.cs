namespace MCore.Comp.XFunc
{
    partial class Linear2DPage
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
            this.graphXY = new Gigasoft.ProEssentials.Pesgo();
            this.SuspendLayout();
            // 
            // graphXY
            // 
            this.graphXY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphXY.Location = new System.Drawing.Point(0, 0);
            this.graphXY.Name = "graphXY";
            this.graphXY.Size = new System.Drawing.Size(292, 201);
            this.graphXY.TabIndex = 0;
            this.graphXY.Text = "XY Graph";
            // 
            // Linear2DPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.graphXY);
            this.Name = "Linear2DPage";
            this.Size = new System.Drawing.Size(292, 201);
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Gigasoft.ProEssentials.Pesgo graphXY;


    }
}
