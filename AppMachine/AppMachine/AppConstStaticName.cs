using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppMachine
{
    public class AppConstStaticName
    {

        #region  Standard Pattern
        public const string DEFAULT_LOGGER = "Default Logger";
        public const string APP_MACHINE = "Application Machine";
        public const string COMMON_PARAMETER = "Common Parameter";
        public const string ALL_STATIONS = "All Stations";
        #endregion

        //Station Name Define
        /* Add Station Name Here (Exmaple in Below)
        public const string FEEDSTATION = "Feeder Station";
        public const string VISIONSTATION = "Vision Station";
        */

        /*Add Vision System Name Here (Example in Below)
        public const string VISIONSYSTEM = "Vision System";
        public const string VISIONCAMERA = "Vision Camera";
        public const string VISIONJOB = "Vision Job";
        public const string VISIONJOBRESULT = "Vision Job Result";
        */


        #region Standrad Pattern
        public const string ALL_IO = "All IO";
        public const string INPUTS = "Inputs";
        public const string OUTPUTS = "Outputs";
        #endregion

        //IO System Name Define
        /*Add IO System Name Here (Example in Below)
        public const string ALLWAGOIO = "All WAGO IO";

        public const string INPUTS = "Inputs";
        public const string STARTPB = "Start_PB";//1
        public const string STOPPB = "Stop_PB";//2
        public const string RESETPB = "Reset_PB";//3
         
      
        public const string OUTPUTS = "Outputs";
        public const string REDLIGHT = "RED_LIGHT";
        public const string AMBERLIGHT = "AMBER_LIGHT";
        public const string GREENLIGHT = "GREEN_LIGHT";
        public const string BUZZER = "BUZZER";
        */


        //Vision System Name Define
        /*Add System Name Here (Example in Below)
        public const string VISIONMOTIONSYSTEM = "Vision Motion System";
        public const string VISIONMOTIONAXES = "Vision Motion Axes";
        public const string VISIONXAXIS = "Vision XAxis";
        */



        //Motion System Name Define
        /*Add Motion System Name Here (Example in Below)
        public const string FEEDMOTIONSYSTEM = "Feeder Motion System";
        public const string FEEDMOTIONAXES= "Feeder Motion Axes";
        public const string FEEDYAXIS = "Feeder YAxis";

        public const string LIFT1RS232 = "Lift1 Motion RS232";
        public const string LIFT1MOTIONSYSTEM = "Lift1 Motion System";
        public const string LIFT1MOTIONAXES = "Lift1 Motion Axes";
        public const string LIFT1ZAXIS = "Lift1 ZAxis";

        public const string LIFT2MOTIONSYSTEM = "Lift2 Motion System";
        public const string LIFT2MOTIONAXES = "Lift2 Motion Axes";
        public const string LIFT2ZAXIS = "Lift2 ZAxis";
        */



        #region Standard Pattern
        public const string ALL_STATE_MACHINE = "All State Machines";
        public const string ALL_SEMI_AUTO_STATE_MACHINE = "All Semi State Machines";

        public const string SM_RESET = "SM Reset";
        public const string SM_MAIN = "SM Main";

        public const string SM_SEMI_RESET = "SM Semi Reset";
        public const string SM_SEMI_MAIN = "SM Semi Main";
        #endregion

        /*Add State Machine Name Here (Example Inbelow)
        public const string SM_FEED = "SM Feed";
        public const string SM_VISION = "SM Vision";
        */
        public const string SM_SEMI_FEED = "Test";

        /*Add Semi State Machine Name Here (Example Inbelow)
        public const string SM_SEMI_FEED = "SM Semi Feed";
        public const string SM_SEMI_VISION = "SM Semi Vision";
        */


        #region Standard Pattern
        public const string ALL_RECIPES = "All Recipes";
        public const string SAMPLE_RECIPE = "Sample Recipe";
        public const string REF_CURRENT_RECIPE = "Ref Current Recipe";
        #endregion


        #region Standard Pattern
        public const string ALL_USER = "All Users";
        public const string ADMIN_USER = "Admin";
        public const string GUEST_USER = "Guest";
        #endregion

    }
}