using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using AppMachine.Comp.CommonParam;
using AppMachine.Comp;

using MCore;
using MCore.Comp;

namespace AppMachine.Comp.Users
{
    public class AppUserInfo : CompBase
    {

       
        #region User Info
        /// <summary>
        /// Username
        /// </summary>
        [Category("User Info"), Browsable(true), Description("Username"), ReadOnly(true)]
        public String UserName
        {
            get { return GetPropValue(() => UserName, ""); }
            set { SetPropValue(() => UserName, value); }
        }


        /// <summary>
        /// UserEN
        /// </summary>
        [Category("User Info"), Browsable(true), Description("UserEN")]
        public String UserEN
        {
            get { return GetPropValue(() => UserEN, ""); }
            set { SetPropValue(() => UserEN, value); }
        }
        
        /// <summary>
        /// Usercode
        /// </summary>
        [Category("User Info"), Browsable(true), Description("Usercode")]
        public String UserCode
        {
            get { return GetPropValue(() => UserCode, ""); }
            set { SetPropValue(() => UserCode, value); }
        }


        

        /// <summary>
        /// UserLevel
        /// </summary>
        [Category("User Info"), Browsable(true), Description("UserLevel")]
        public AppEnums.eAccessLevel UserLevel
        {
            get { return GetPropValue(() => UserLevel, AppEnums.eAccessLevel.Operator); }
            set { SetPropValue(() => UserLevel, value); }
        }

        #endregion


         #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppUserInfo()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppUserInfo(string name)
            : base(name)
        {
            UserName = name;
        }
        #endregion Constructors

    }
}
