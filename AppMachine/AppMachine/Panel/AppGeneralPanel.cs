using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;
using MCore.Comp;

using AppMachine.Control;
using AppMachine.Comp;
using AppMachine.Comp.Recipe;

namespace AppMachine.Panel
{
    public partial class AppGeneralPanel : AppUserControlBase
    {
        
        public static AppGeneralPanel This = null;
        private CompBase _allRecipe = null;
        private AppProductRecipe _masterRecipe = null;

        public AppGeneralPanel():base()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;

            _allRecipe = U.GetComponent(AppConstStaticName.ALL_RECIPES);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.CurrentProdRecipeName, CurrentRecipeOnChange);
            #endregion
        }


        #region Standard Pattern
        private void cbCurrentRecipe_DropDown(object sender, EventArgs e)
        {
            cbCurrentRecipe.Items.Clear();
            cbCurrentRecipe.Items.AddRange(_allRecipe.ChildArray);
            AppProductRecipe refCurrentRecipe = U.GetComponent(AppConstStaticName.REF_CURRENT_RECIPE) as AppProductRecipe;
            cbCurrentRecipe.Items.Remove(refCurrentRecipe);
        }

        private void cbCurrentRecipe_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbCurrentRecipe.SelectedItem != null)
            {
                pgRecipe.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Product Recipe"));
                //pgRecipe.BrowsableAttributes = new AttributeCollection(new SubCategoryAttribute("General Recipe"));
                                                                      
                pgRecipe.SelectedObject = cbCurrentRecipe.SelectedItem;
                AppMachine.Comp.AppMachine.This.CurrentProdRecipeName = cbCurrentRecipe.SelectedItem.ToString();
            }
        }

        private void CurrentRecipeOnChange(string recipeName)
        {
            cbCurrentRecipe.Text = recipeName;
            pgRecipe.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Product Recipe"));
            //pgRecipe.BrowsableAttributes = new AttributeCollection(new SubCategoryAttribute("Cofiguration"));

            AppProductRecipe currentRecipe = AppUtility.GetCurrentRecipe();
            if(currentRecipe!=null)
            {
                pgRecipe.SelectedObject = currentRecipe;
            }
        }
       

        private void cbCopyRecipe_DropDown(object sender, EventArgs e)
        {
            cbMasterCopyRecipe.Items.Clear();
            cbMasterCopyRecipe.Items.AddRange(_allRecipe.ChildArray);
            AppProductRecipe refCurrentRecipe = U.GetComponent(AppConstStaticName.REF_CURRENT_RECIPE) as AppProductRecipe;
            cbMasterCopyRecipe.Items.Remove(refCurrentRecipe);
        }

        private void cbCopyRecipe_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbMasterCopyRecipe.SelectedItem != null)
            {
                _masterRecipe = cbMasterCopyRecipe.SelectedItem as AppProductRecipe;
            }
        }

        private void btnCopyRecipe_Click(object sender, EventArgs e)
        {
          if(_masterRecipe == null)
          {
              return;
          }

          DialogResult result = AppUtility.ShowKryptonMessageBox("Confirm copy recipe", "All current recipe data will replace with replace with master copy recipe", "",
                                                                   ComponentFactory.Krypton.Toolkit.TaskDialogButtons.Yes | ComponentFactory.Krypton.Toolkit.TaskDialogButtons.No, MessageBoxIcon.Question, this);
            
          if (result != DialogResult.OK)
          {
              return;
          }

          
          AppProductRecipe masterCopy = _masterRecipe.ShallowClone(typeof(AppProductRecipe)) as AppProductRecipe;
          masterCopy.Name = AppMachine.Comp.AppMachine.This.CurrentRecipe.Name;
          masterCopy.CopyPropertyTo(AppMachine.Comp.AppMachine.This.CurrentRecipe);


           pgRecipe.BrowsableAttributes = new AttributeCollection(new CategoryAttribute("Product Recipe"));
          //pgRecipe.BrowsableAttributes = new AttributeCollection(new SubCategoryAttribute("General Recipe"));

           pgRecipe.SelectedObject = AppMachine.Comp.AppMachine.This.CurrentRecipe;
           AppMachine.Comp.AppMachine.This.CurrentProdRecipeName = "";
           AppMachine.Comp.AppMachine.This.CurrentProdRecipeName = masterCopy.Name;

           _masterRecipe = null;
           cbMasterCopyRecipe.Items.Clear();
           cbMasterCopyRecipe.Text = "";

        }

        public override void RefreshData()
        {
            _masterRecipe = null;
            cbMasterCopyRecipe.Items.Clear();
            cbMasterCopyRecipe.Text = "";

            string currentRecipeNameBackup = AppMachine.Comp.AppMachine.This.CurrentProdRecipeName;
            AppMachine.Comp.AppMachine.This.CurrentProdRecipeName = "";
            AppMachine.Comp.AppMachine.This.CurrentProdRecipeName = currentRecipeNameBackup;
        }

        #endregion

    }
}
