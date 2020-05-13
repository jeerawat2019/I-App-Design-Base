using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;
using MDouble;

namespace MCore.Comp.MotionSystem.Axis
{
    public partial class MultiPosPage : UserControl, IComponentBinding<MultiPosAxis>
    {
        private MultiPosAxis _axis = null;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MultiPosPage()
        {
            InitializeComponent();
        }

        #region Overrides
        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Multi-Pt Axis";
        }
        #endregion Overrides

        /// <summary>
        /// Bind to the THreePosAxis
        /// </summary>
        /// <param name="axis"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MultiPosAxis Bind
        {
            get { return _axis; }
            set
            {
                _axis = value;
                axisPage.Bind = _axis;

                tbPositionA.BindTwoWay(() => _axis.PositionA);
                tbPositionB.BindTwoWay(() => _axis.PositionB);
                tbPositionC.BindTwoWay(() => _axis.PositionC);
                tbPositionD.BindTwoWay(() => _axis.PositionD);
                tbPositionE.BindTwoWay(() => _axis.PositionE);
                tbPositionF.BindTwoWay(() => _axis.PositionF);

                btnMoveA.BindTwoWay(() => _axis.PositionA);
                btnMoveB.BindTwoWay(() => _axis.PositionB);
                btnMoveC.BindTwoWay(() => _axis.PositionC);
                btnMoveD.BindTwoWay(() => _axis.PositionD);
                btnMoveE.BindTwoWay(() => _axis.PositionE);
                btnMoveF.BindTwoWay(() => _axis.PositionF);

                btnTeachA.BindTwoWay(() => _axis.PositionA);
                btnTeachB.BindTwoWay(() => _axis.PositionB);
                btnTeachC.BindTwoWay(() => _axis.PositionC);
                btnTeachD.BindTwoWay(() => _axis.PositionD);
                btnTeachE.BindTwoWay(() => _axis.PositionE);
                btnTeachF.BindTwoWay(() => _axis.PositionF);

            }
        }


    }
}
