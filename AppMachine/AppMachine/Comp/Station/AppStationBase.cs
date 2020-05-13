using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

using MCore;
using MCore.Comp;

namespace AppMachine.Comp.Station
{
    public class AppStationBase:CompBase
    {
        /// <summary>
        /// Is Busy
        /// </summary>
        [XmlIgnore]
        [Category("General"), Browsable(true), Description("Is Busy")]
        public bool IsBusy
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => IsBusy, false); }
            [StateMachineEnabled]
            set { SetPropValue(() => IsBusy, value); }
        }


        [Category("General"), Browsable(true), Description("Station Description")]
        public string  StationDescription
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => StationDescription, ""); }
            [StateMachineEnabled]
            set { SetPropValue(() => StationDescription, value); }
        }
         #region Constructors



        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppStationBase()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppStationBase(string name)
            : base(name)
        {
            
        }
        #endregion Constructors


        public override void Initialize()
        {
            base.Initialize();
            if(this.ChildArray == null)
            {

            }
        }

    }
}
