using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore;

using AppMachine.Control;
using AppMachine.Comp;
using AppMachine.Comp.Recipe;


namespace AppMachine.Panel
{
    public partial class AppSpecPanel : AppUserControlBase
    {
        public static AppSpecPanel This = null;
        public AppSpecPanel()
        {
            
        }

        protected override void Initializing()
        {
            #region Standard Pattern
            InitializeComponent();
            This = this;

            strProductRecipe.BindTwoWay(() => AppMachine.Comp.AppMachine.This.CurrentProdRecipeName);
            U.RegisterOnChanged(() => AppMachine.Comp.AppMachine.This.CurrentProdRecipeName, CurrentRecipeOnChange);
            this.Update();
            #endregion
        }


        #region Standard Pattern
        private void CurrentRecipeOnChange(string recipeName)
        {
            pgRecipe.BrowsableAttributes = new AttributeCollection(new SubCategoryAttribute("Specification Recipe"));
            AppProductRecipe currentRecipe = U.GetComponent(recipeName) as AppProductRecipe;
            if (currentRecipe != null)
            {
                pgRecipe.SelectedObject = currentRecipe;
            }
        }

        public override void RefreshData()
        {
            pgRecipe.BrowsableAttributes = new AttributeCollection(new SubCategoryAttribute("Specification Recipe"));
            AppProductRecipe currentRecipe = AppUtility.GetCurrentRecipe();
            if (currentRecipe != null)
            {
                pgRecipe.SelectedObject = currentRecipe;
            }
        }
        #endregion

    }
}
