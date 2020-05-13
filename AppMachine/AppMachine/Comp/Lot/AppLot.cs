using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MCore;
using MCore.Comp;
using MDouble;

namespace AppMachine.Comp.Lot
{
    public class AppLot:CompBase
    {

        /// <summary>
        /// Lot Id
        /// </summary>
        [Category("General"), Browsable(true), Description("Lot Id")]
        public String LotId
        {
            get { return GetPropValue(() => LotId); }
            set { SetPropValue(() => LotId, value); }
        }


        /// <summary>
        /// Lot Size
        /// </summary>
        [Category("General"), Browsable(true), Description("Lot Size")]
        public int LotSize
        {
            get { return GetPropValue(() => LotSize); }
            set { SetPropValue(() => LotSize, value); }
        }

        /// <summary>
        /// Iputs Part
        /// </summary>
        [Category("General"), Browsable(true), Description("Input Parts")]
        public String InputsPart
        {
            get { return GetPropValue(() => InputsPart); }
            set { SetPropValue(() => InputsPart, value); }
        }


        /// <summary>
        /// Failed Parts
        /// </summary>
        [Category("General"), Browsable(true), Description("Failed Parts")]
        public String FailedParts
        {
            get { return GetPropValue(() => FailedParts); }
            set { SetPropValue(() => FailedParts, value); }
        }


        /// <summary>
        /// Yield
        /// </summary>
        [Category("General"), Browsable(true), Description("Yield")]
        public String Yield
        {
            get { return GetPropValue(() => Yield); }
            set { SetPropValue(() => Yield, value); }
        }


        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppLot()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppLot(string name)
            : base(name)
        {

        }

        #endregion Constructors
    }
}
