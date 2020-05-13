using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

using AppMachine.Panel;

using MCore;
using MCore.Comp;

using AppMachine.Comp.Recipe;

using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AppMachine
{
    public class AppUtility
    {
        #region Standard Pattern 

        /// <summary>
        /// Application Path
        /// </summary>
        public static string AppFullPath
        {
            get
            {
                string rootFolder = U.RootComp.RootFolder;
                if (string.IsNullOrEmpty(rootFolder))
                {
                    rootFolder = U.RootComp.RootFolder;
                }
                return string.Format(@"{0}", rootFolder);
            }
        }

        public static void CloneAllProperties(object source, object destination)
        {
            //get the list of all properties in the destination object
            var destProps = destination.GetType().GetProperties();

            //get the list of all properties in the source object
            foreach (var sourceProp in source.GetType().GetProperties())
            {
                foreach (var destProperty in destProps)
                {
                    //if we find match between source & destination properties name, set
                    //the value to the destination property
                    if (destProperty.Name == sourceProp.Name &&
                            destProperty.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        destProperty.SetValue(destProps, sourceProp.GetValue(
                            sourceProp, new object[] { }), new object[] { });
                        break;
                    }
                }
            }
        }

        public static AppProductRecipe GetCurrentRecipe()
        {
            if (AppMachine.Comp.AppMachine.This.CurrentProdRecipeName == "")
            {
                return null;
            }
            return U.GetComponent(AppMachine.Comp.AppMachine.This.CurrentProdRecipeName) as AppProductRecipe;
        }

        public static DialogResult ShowKryptonMessageBox(string title,string mainIntrustion,string content,TaskDialogButtons commonButton,MessageBoxIcon icon,IWin32Window owner = null)
        {
                KryptonTaskDialog kUserDialog = new KryptonTaskDialog();
                kUserDialog.WindowTitle = title;
                kUserDialog.MainInstruction = mainIntrustion;
                kUserDialog.Content = content;
                kUserDialog.CommonButtons = commonButton;
                kUserDialog.Icon = icon;
                return kUserDialog.ShowDialog(owner);
        }

        #endregion

        //Add Any Utility Static Method for Service Any Place in Application in below
        

    }
}
