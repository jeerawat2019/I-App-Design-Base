using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

using MCore;
using MCore.Comp;

namespace AppMachine.Comp.Part
{
    public class AppPartCarrier : CompBase
    {

        #region Standard Pattern
        /// <summary>
        /// Carrier Id
        /// </summary>
        [Category("General"), Browsable(true), Description("Carrier Id")]
        public int CarrierId
        {
            get { return GetPropValue(() => CarrierId); }
            set { SetPropValue(() => CarrierId, value); }
        }


        /// <summary>
        /// MainIndexPos
        /// </summary>
        [Category("Index Position"), Browsable(true), Description("Main Index Pos")]
        public int MainIndexPos
        {
            get { return GetPropValue(() => MainIndexPos); }
            set { SetPropValue(() => MainIndexPos, value); }
        }

        /// <summary>
        /// SubIndexPos
        /// </summary>
        [Category("Index Position"), Browsable(true), Description("Sub Index Pos")]
        public int SubIndexPos
        {
            get { return GetPropValue(() => SubIndexPos); }
            set { SetPropValue(() => SubIndexPos, value); }
        }


        /// <summary>
        /// Number Of Column
        /// </summary>
        [Category("General"), Browsable(true), Description("Number of Column")]
        public int NumOfColumn
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => NumOfColumn, 0); }
            [StateMachineEnabled]
            set { SetPropValue(() => NumOfColumn, value); }
        }

        /// <summary>
        /// Number Of Row
        /// </summary>
        [Category("General"), Browsable(true), Description("Number of Row")]
        public int NumOfRow
        {
            [StateMachineEnabled]
            get { return GetPropValue(() => NumOfRow, 0); }
            [StateMachineEnabled]
            set { SetPropValue(() => NumOfRow, value); }
        }
        #endregion

        //Add More Application Requirement in Below

        #region Constructors


        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppPartCarrier()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppPartCarrier(string name)
            : base(name)
        {
            
        }


        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppPartCarrier(string name,int numOfPart)
            : base(name)
        {
            #region Standard Pattern
            for (int i = 0; i < numOfPart; i++)
            {
                Add(new AppPart(String.Format("Part{0}", i), i));
            }
            #endregion

            //Add More Application Requirement in Below

        }
        #endregion Constructors

        //Add More Application Requirement in Below
       
    }
}
