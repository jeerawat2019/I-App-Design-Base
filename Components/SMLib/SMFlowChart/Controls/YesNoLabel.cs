using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using MCore.Comp.SMLib.Path;
using MCore.Comp.SMLib.Flow;

namespace MCore.Comp.SMLib.SMFlowChart.Controls
{
    public class YesNoLabel : Label, ISelectable
    {
        public SMPathOutBool PathSegmentBool { get; set; }
        private SMContainerPanel _containerPanel = null;
        private SMCtlBase _smCrlBase = null;
        private Point _locOfstFromCtrlBase;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathSegmentBool"></param>
        public YesNoLabel(SMCtlBase smCrlBase, SMPathOutBool pathSegmentBool)
        {
            _smCrlBase = smCrlBase;
            PathSegmentBool = pathSegmentBool;
            _smCrlBase.LocationChanged += new EventHandler(smCrlBase_OnLocationChanged);
        }

        bool ISelectable.SMSelected 
        { 
            get
            {
                return false;
            }
            set
            {
                if (value)
                {
                    BackColor = System.Drawing.Color.LightBlue;
                }
                else
                {
                    BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        public void SetLocOfstfromCtrlBase()
        {
            _locOfstFromCtrlBase = new Point(_smCrlBase.Location.X - this.Location.X, _smCrlBase.Location.Y - this.Location.Y);
        }

        private void smCrlBase_OnLocationChanged(object sender, EventArgs e)
        {
            this.Location = new Point(_smCrlBase.Location.X - _locOfstFromCtrlBase.X, _smCrlBase.Location.Y - _locOfstFromCtrlBase.Y);
        }

    }
}
