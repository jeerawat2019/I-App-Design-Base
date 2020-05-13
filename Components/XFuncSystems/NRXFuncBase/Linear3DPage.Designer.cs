using System.Windows.Forms;
namespace MCore.Comp.XFunc
{
    partial class Linear3DPage
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
            if (disposing)
            {
                if (_basicLinear != null)
                {
                    _basicLinear.OnUpdateGraph -= new MethodInvoker(RefreshGraph);
                }
            }
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
            this.graphXYZ = new Gigasoft.ProEssentials.Pe3do();
            this.SuspendLayout();
            // 
            // graphXYZ
            // 
            this.graphXYZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphXYZ.Location = new System.Drawing.Point(0, 0);
            this.graphXYZ.Name = "graphXYZ";
            this.graphXYZ.Padding = new System.Windows.Forms.Padding(3);
            this.graphXYZ.Size = new System.Drawing.Size(292, 201);
            this.graphXYZ.TabIndex = 0;
            this.graphXYZ.Text = "XYZ Graph";
            // 
            // Linear3DPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.graphXYZ);
            this.Name = "Linear3DPage";
            this.Size = new System.Drawing.Size(292, 201);
            this.VisibleChanged += new System.EventHandler(this.OnVisibleChanged);
            this.ResumeLayout(false);

        }


        #endregion

        private Gigasoft.ProEssentials.Pe3do graphXYZ;

    }
}
