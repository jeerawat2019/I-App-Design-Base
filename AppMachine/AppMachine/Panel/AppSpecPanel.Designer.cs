namespace AppMachine.Panel
{
    partial class AppSpecPanel
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
            this.strProductRecipe = new MCore.Controls.StringCtl();
            this.label12 = new System.Windows.Forms.Label();
            this.pgRecipe = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // strProductRecipe
            // 
            this.strProductRecipe.BackColor = System.Drawing.Color.Black;
            this.strProductRecipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.strProductRecipe.ForeColor = System.Drawing.Color.White;
            this.strProductRecipe.Location = new System.Drawing.Point(121, 14);
            this.strProductRecipe.LogChanges = true;
            this.strProductRecipe.Name = "strProductRecipe";
            this.strProductRecipe.ReadOnly = true;
            this.strProductRecipe.Size = new System.Drawing.Size(231, 26);
            this.strProductRecipe.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label12.Location = new System.Drawing.Point(3, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "CURRENT RECIPE";
            // 
            // pgRecipe
            // 
            this.pgRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgRecipe.Location = new System.Drawing.Point(6, 51);
            this.pgRecipe.Name = "pgRecipe";
            this.pgRecipe.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgRecipe.Size = new System.Drawing.Size(1243, 848);
            this.pgRecipe.TabIndex = 11;
            // 
            // AppSpecPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.strProductRecipe);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pgRecipe);
            this.Name = "AppSpecPanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgRecipe;
        private MCore.Controls.StringCtl strProductRecipe;
        private System.Windows.Forms.Label label12;

    }
}
