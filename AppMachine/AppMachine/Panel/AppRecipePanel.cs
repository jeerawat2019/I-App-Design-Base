using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using MCore;
using MCore.Comp;


using AppMachine.Control;
using AppMachine.Comp.Recipe;
using MCore.Controls;


namespace AppMachine.Panel
{
    public partial class AppRecipePanel : AppUserControlBase
    {
        public static AppRecipePanel This = null;
        private CompBase _allRecipe = null;

        public AppRecipePanel()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;

            _allRecipe = U.GetComponent(AppConstStaticName.ALL_RECIPES);
            componentBrowser.Rebuild(_allRecipe);
            componentBrowser.TabPages.ControlAdded += new ControlEventHandler(SetPropertyGridFilter);
            this.Update();
            #endregion


        }


        #region Standard Pattern
        private void cbDelRecipe_DropDown(object sender, EventArgs e)
        {
            cbDelRecipe.Items.Clear();
            cbDelRecipe.Items.AddRange(_allRecipe.ChildArray);
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            if(tbNewRecipe.Text =="")
            {
                AppUtility.ShowKryptonMessageBox("No Recipe Name", "Please input recipe name", "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                return;
            }

            if(_allRecipe.ChildExists(tbNewRecipe.Text))
            {
                AppUtility.ShowKryptonMessageBox("Recipe Name Exist", String.Format("Recipe name \"{0}\" already exist.", tbNewRecipe.Text), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                return;
            }

            try
            {
                AppProductRecipe newRecipe = new AppProductRecipe(tbNewRecipe.Text);
                newRecipe.PropertyValChanged += new PropertyChangedEventHandler(AppMachine.Comp.AppMachine.This.RecipePropValue_OnChange);
                _allRecipe.Add(newRecipe);
                componentBrowser.Rebuild(_allRecipe);
                AppUtility.ShowKryptonMessageBox("Add New Product Completed", String.Format("Add New Product \"{0}\" Completed", tbNewRecipe.Text), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);
                tbNewRecipe.Clear();
                
            }
            finally
            {

            }
        }

        private void btnDelRecipe_Click(object sender, EventArgs e)
        {
            if (cbDelRecipe.SelectedItem != null &&
               _allRecipe.ChildExists(cbDelRecipe.SelectedItem.ToString()))
            {

                DialogResult result = AppUtility.ShowKryptonMessageBox("Remove Recipe", String.Format("Confirm to remove recipe \"{0}\".", cbDelRecipe.SelectedItem.ToString()), "",
                                                                        ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK | ComponentFactory.Krypton.Toolkit.TaskDialogButtons.Cancel, MessageBoxIcon.Question, this);

                if (result != DialogResult.OK)
                {
                    return;
                }

                if (cbDelRecipe.SelectedItem.ToString() == AppMachine.Comp.AppMachine.This.CurrentProdRecipeName ||
                    cbDelRecipe.SelectedItem.ToString() == AppConstStaticName.SAMPLE_RECIPE ||
                    cbDelRecipe.SelectedItem.ToString() == AppConstStaticName.REF_CURRENT_RECIPE)
                {
                    AppUtility.ShowKryptonMessageBox("Remove Recipe", String.Format("Unable to remove active recipe \"{0}\" ", cbDelRecipe.SelectedItem.ToString()), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                    return;
                }


                try
                {
                    (cbDelRecipe.SelectedItem as AppProductRecipe).PropertyValChanged -= new PropertyChangedEventHandler(AppMachine.Comp.AppMachine.This.RecipePropValue_OnChange);
                    _allRecipe.Remove(cbDelRecipe.SelectedItem as CompBase);
                    componentBrowser.Rebuild(_allRecipe);
                    AppUtility.ShowKryptonMessageBox("Remove Recipe", String.Format("Recipe \"{0}\" Removed", cbDelRecipe.SelectedItem.ToString()), "",ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information,this);
                    cbDelRecipe.Items.Clear();
                    cbDelRecipe.Text = "";
                }
                finally
                {

                }
            }
        }


        private void SetPropertyGridFilter(object sender, ControlEventArgs e)
        {
        
            foreach (TabPage tp in componentBrowser.TabPages.TabPages)
            {
                if (tp.Text == "Properties")
                {
                    
                    CompBasePropCtl compPropCtrl = tp.Controls[0] as CompBasePropCtl;
                    AttributeCollection attributesCollection = new AttributeCollection(new CategoryAttribute("Product Recipe"));
                    compPropCtrl.BrowsableAttributes = attributesCollection;
                }
            }
        }


        public override void RefreshData()
        {
            componentBrowser.Rebuild(_allRecipe);
        }


        private void btnImportRecipe_Click(object sender, EventArgs e)
        {
            string recipeDumpPath = AppUtility.AppFullPath + "\\Dump Recipes\\";
            U.EnsureDirectory(recipeDumpPath);
            OpenFileDialog od = new OpenFileDialog();
            od.DefaultExt = ".xml";
            od.Filter = "Product File|*.xml";
            od.InitialDirectory = recipeDumpPath;
            od.CheckFileExists = true;
            od.Multiselect = false;
            od.ShowDialog();
            if(String.IsNullOrEmpty(od.FileName))
            {
                return;
            }

            string importFilePath = od.FileName;
            AppProductRecipe importRecipe = null;
            try
            {
                importRecipe = CompRoot.ImportFile(typeof(AppProductRecipe), importFilePath) as AppProductRecipe;
            }
            catch(Exception ex)
            {
                AppUtility.ShowKryptonMessageBox("Import Recipe Error", String.Format("Import Recipe Error"), ex.ToString(), ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Error, this);
                return;
            }


            if (_allRecipe.ChildExists(importRecipe.Name))
            {

                DialogResult result = AppUtility.ShowKryptonMessageBox("Recipe Name Exist", String.Format("Recipe name \"{0}\" already exist.", importRecipe.Name),
                                                                       "Do you want to auto generate unique name?", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.Yes | ComponentFactory.Krypton.Toolkit.TaskDialogButtons.No, MessageBoxIcon.Question, this);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                int count = 1;
                string autoGenRecipeName ="";
                do
                {
                    autoGenRecipeName = importRecipe.Name + count.ToString("_00#");
                } while (_allRecipe.ChildExists(autoGenRecipeName));
                importRecipe.Name = autoGenRecipeName;
            }

            try
            {
                
                importRecipe.PropertyValChanged += new PropertyChangedEventHandler(AppMachine.Comp.AppMachine.This.RecipePropValue_OnChange);
                _allRecipe.Add(importRecipe);
                componentBrowser.Rebuild(_allRecipe);
                AppUtility.ShowKryptonMessageBox("Import Recipe Completed", String.Format("Import Recipe \"{0}\" Completed", importRecipe.Name), "", ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);
                tbNewRecipe.Clear();
            }
            finally
            {

            }
        }

        private void btnExportRecipe_Click(object sender, EventArgs e)
        {
            if (cbDelRecipe.SelectedItem != null &&
              _allRecipe.ChildExists(cbDelRecipe.SelectedItem.ToString()))
            {
                string recipeDumpPath = AppUtility.AppFullPath + "\\Dump Recipes\\";
                U.EnsureDirectory(recipeDumpPath);
                AppProductRecipe exportRecipe = cbDelRecipe.SelectedItem as AppProductRecipe;
                string exportFilePath = String.Format("{0}DumpRecipe[ {1} ]_{2}.xml",recipeDumpPath,exportRecipe.Name, DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
                CompRoot.ExportSettings(exportFilePath, exportRecipe);

                AppUtility.ShowKryptonMessageBox("Export Recipe Completed", String.Format("Export Recipe \"{0}\" Completed", exportRecipe.Name), String.Format("Export Recipe {0} to {1}", exportRecipe.Name, exportFilePath),
                                                 ComponentFactory.Krypton.Toolkit.TaskDialogButtons.OK, MessageBoxIcon.Information, this);
            }
        }

        #endregion

        

       


    }
}
