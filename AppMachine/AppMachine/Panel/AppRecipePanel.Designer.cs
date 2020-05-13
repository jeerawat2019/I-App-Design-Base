namespace AppMachine.Panel
{
    partial class AppRecipePanel
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
            this.btnImportRecipe = new System.Windows.Forms.Button();
            this.btnExportRecipe = new System.Windows.Forms.Button();
            this.cbDelRecipe = new System.Windows.Forms.ComboBox();
            this.tbNewRecipe = new System.Windows.Forms.TextBox();
            this.btnDelRecipe = new System.Windows.Forms.Button();
            this.btnAddRecipe = new System.Windows.Forms.Button();
            this.componentBrowser = new MCore.Controls.ComponentBrowser();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(266, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "RECIPE PROPERTIES";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ALL RECIPES";
            // 
            // btnImportRecipe
            // 
            this.btnImportRecipe.BackColor = System.Drawing.SystemColors.Control;
            this.btnImportRecipe.Location = new System.Drawing.Point(346, 4);
            this.btnImportRecipe.Name = "btnImportRecipe";
            this.btnImportRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnImportRecipe.TabIndex = 9;
            this.btnImportRecipe.Text = "IMPORT";
            this.btnImportRecipe.UseVisualStyleBackColor = false;
            this.btnImportRecipe.Click += new System.EventHandler(this.btnImportRecipe_Click);
            // 
            // btnExportRecipe
            // 
            this.btnExportRecipe.BackColor = System.Drawing.SystemColors.Control;
            this.btnExportRecipe.Location = new System.Drawing.Point(839, 5);
            this.btnExportRecipe.Name = "btnExportRecipe";
            this.btnExportRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnExportRecipe.TabIndex = 8;
            this.btnExportRecipe.Text = "EXPORT";
            this.btnExportRecipe.UseVisualStyleBackColor = false;
            this.btnExportRecipe.Click += new System.EventHandler(this.btnExportRecipe_Click);
            // 
            // cbDelRecipe
            // 
            this.cbDelRecipe.FormattingEnabled = true;
            this.cbDelRecipe.Location = new System.Drawing.Point(510, 6);
            this.cbDelRecipe.Name = "cbDelRecipe";
            this.cbDelRecipe.Size = new System.Drawing.Size(244, 21);
            this.cbDelRecipe.TabIndex = 7;
            this.cbDelRecipe.DropDown += new System.EventHandler(this.cbDelRecipe_DropDown);
            // 
            // tbNewRecipe
            // 
            this.tbNewRecipe.Location = new System.Drawing.Point(7, 6);
            this.tbNewRecipe.Name = "tbNewRecipe";
            this.tbNewRecipe.Size = new System.Drawing.Size(253, 20);
            this.tbNewRecipe.TabIndex = 6;
            // 
            // btnDelRecipe
            // 
            this.btnDelRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelRecipe.Location = new System.Drawing.Point(758, 5);
            this.btnDelRecipe.Name = "btnDelRecipe";
            this.btnDelRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnDelRecipe.TabIndex = 5;
            this.btnDelRecipe.Text = "REMOVE";
            this.btnDelRecipe.UseVisualStyleBackColor = false;
            this.btnDelRecipe.Click += new System.EventHandler(this.btnDelRecipe_Click);
            // 
            // btnAddRecipe
            // 
            this.btnAddRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddRecipe.Location = new System.Drawing.Point(265, 4);
            this.btnAddRecipe.Name = "btnAddRecipe";
            this.btnAddRecipe.Size = new System.Drawing.Size(75, 23);
            this.btnAddRecipe.TabIndex = 4;
            this.btnAddRecipe.Text = "NEW";
            this.btnAddRecipe.UseVisualStyleBackColor = false;
            this.btnAddRecipe.Click += new System.EventHandler(this.btnAddRecipe_Click);
            // 
            // componentBrowser
            // 
            this.componentBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.componentBrowser.Location = new System.Drawing.Point(6, 55);
            this.componentBrowser.Name = "componentBrowser";
            this.componentBrowser.Size = new System.Drawing.Size(1243, 844);
            this.componentBrowser.TabIndex = 2;
            // 
            // AppRecipePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnImportRecipe);
            this.Controls.Add(this.btnExportRecipe);
            this.Controls.Add(this.cbDelRecipe);
            this.Controls.Add(this.tbNewRecipe);
            this.Controls.Add(this.btnDelRecipe);
            this.Controls.Add(this.btnAddRecipe);
            this.Controls.Add(this.componentBrowser);
            this.Name = "AppRecipePanel";
            this.Size = new System.Drawing.Size(1256, 906);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MCore.Controls.ComponentBrowser componentBrowser;
        private System.Windows.Forms.Button btnAddRecipe;
        private System.Windows.Forms.Button btnDelRecipe;
        private System.Windows.Forms.TextBox tbNewRecipe;
        private System.Windows.Forms.ComboBox cbDelRecipe;
        private System.Windows.Forms.Button btnExportRecipe;
        private System.Windows.Forms.Button btnImportRecipe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;


    }
}
