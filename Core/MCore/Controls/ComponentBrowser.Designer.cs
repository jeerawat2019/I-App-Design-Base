using MCore.Comp;

namespace MCore.Controls
{
    partial class ComponentBrowser
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
                CompBase.OnChangedName -= new CompBase.NameChangedEventHandler(OnChangedCompName);
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.treeComponents = new System.Windows.Forms.TreeView();
            this.tabPages = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // treeComponents
            // 
            this.treeComponents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeComponents.Location = new System.Drawing.Point(0, 2);
            this.treeComponents.Name = "treeComponents";
            this.treeComponents.Size = new System.Drawing.Size(255, 558);
            this.treeComponents.TabIndex = 0;
            this.treeComponents.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnNewSelection);
            // 
            // tabPages
            // 
            this.tabPages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPages.Location = new System.Drawing.Point(261, 0);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(541, 561);
            this.tabPages.TabIndex = 1;
            this.tabPages.SizeChanged += new System.EventHandler(this.OnSizedChanged);
            // 
            // ComponentBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabPages);
            this.Controls.Add(this.treeComponents);
            this.Name = "ComponentBrowser";
            this.Size = new System.Drawing.Size(802, 564);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeComponents;
        private System.Windows.Forms.TabControl tabPages;
    }
}
