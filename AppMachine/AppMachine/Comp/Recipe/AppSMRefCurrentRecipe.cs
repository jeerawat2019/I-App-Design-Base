using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

using MCore;
using MCore.Comp;
using MDouble;

using AppMachine.Comp;
using AppMachine.Panel;

namespace AppMachine.Comp.Recipe
{
    public class AppSMRefCurrentRecipe:AppRecipeBase
    {

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppSMRefCurrentRecipe()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppSMRefCurrentRecipe(string name)
            : base(name)
        {
            
        }
        #endregion Constructors


        //Recipe Property
        //Put Product Recipe Property in Here (Example in Below)

        /*
        #region General Recipe
        /// <summary>
        /// Product Name
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Name")]
        [SubCategoryAttribute("General Recipe")]
        [XmlIgnore]
        public string ProductName
        {
            [StateMachineEnabled]
            get { return AppMachine.This.CurrentRecipe.ProductName; }
            [StateMachineEnabled]
            set
            { AppMachine.This.CurrentRecipe.ProductName = value; }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Code")]
        [SubCategoryAttribute("General Recipe")]
        [XmlIgnore]
        public string ProductCode
        {
            [StateMachineEnabled]
            get { return  AppMachine.This.CurrentRecipe.ProductCode; }
            [StateMachineEnabled]
            set {  AppMachine.This.CurrentRecipe.ProductCode = value; }
        }

        #endregion
        */


        public override void Initialize()
        {
            base.Initialize();
        }


        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            
        }


    }


    

  
}
