using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

using MCore;
using MCore.Comp;
using MDouble;

using AppMachine.Comp;
using AppMachine.Panel;


namespace AppMachine.Comp.Recipe
{
    public class AppProductRecipe:AppRecipeBase
    {

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppProductRecipe()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppProductRecipe(string name)
            : base(name)
        {
            
        }
        #endregion Constructors



        //Recipe Property
        //Put Product Recipe Property in Here (Example in Below)


        /*
        /// <summary>
        /// Product Name
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Name")]
        [SubCategoryAttribute("General Recipe")]
        public string ProductName
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductName, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductName, value); }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Code")]
        [SubCategoryAttribute("General Recipe")]
        public string ProductCode
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductCode, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductCode, value); }
        }
         
      
        /// <summary>
        /// Vision Pass Score
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Vision Pass Score")]
        [SubCategoryAttribute("Specification Recipe")]
        public double VisionPassScore
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => VisionPassScore, 50.0); }
            [StateMachineEnabled]
            set { SetPropValue(() => VisionPassScore, value); }
        }
        */


        /// <summary>
        /// Product Name
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Name")]
        [SubCategoryAttribute("General Recipe")]
        public string ProductName
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductName, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductName, value); }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Code")]
        [SubCategoryAttribute("General Recipe")]
        public string ProductCode
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductCode, "None"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductCode, value); }
        }


        /// <summary>
        /// Product Code
        /// </summary>
        [Category("Product Recipe"), Browsable(true), Description("Product Tab")]
        [SubCategoryAttribute("General Recipe")]
        public string ProductTab
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => ProductTab, "Up"); }
            [StateMachineEnabled]
            set { SetPropValue(() => ProductTab, value);}
        }


        public event PropertyChangedEventHandler PropertyValChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyValChanged != null)
            {
                PropertyValChanged(this, new PropertyChangedEventArgs(caller));
            }
        }


        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            this.PropertyChanged += new PropertyChangedEventHandler(AppMachine.This.RecipePropValue_OnChange);
           
        }


        public void CopyPropertyTo(AppProductRecipe toRecipe)
        {
            CategoryAttribute target_attribute = new CategoryAttribute("Product Recipe");
            PropertyInfo[] toObjectProperties = toRecipe.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                try
                {
                    PropertyInfo propFrom = this.GetType().GetProperty(propTo.Name);
                    AttributeCollection attributes = TypeDescriptor.GetProperties(this)[propTo.Name].Attributes;
                    CategoryAttribute ca = (CategoryAttribute)attributes[typeof(CategoryAttribute)];

                    if (propFrom != null && propFrom.CanWrite && ca.Equals(target_attribute))
                        propTo.SetValue(toRecipe, propFrom.GetValue(this, null), null);
                }
                catch (Exception ex)
                {

                }
            }
        }

    }

  
}
