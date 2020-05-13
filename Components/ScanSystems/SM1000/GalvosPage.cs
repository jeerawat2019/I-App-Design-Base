using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MCore.Controls;


namespace MCore.Comp.ScanSystem
{
    public partial class GalvosPage : UserControl, IComponentBinding<SM1000>
    {
        private SM1000 _sm1000 = null;
        public GalvosPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Bind to the Controller
        /// </summary>
        /// <param name="SM1000"></param>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SM1000 Bind
        {
            get { return _sm1000; }
            set
            {
                _sm1000 = value;
                galvoX.Bind = _sm1000.GalvoX;
                galvoY.Bind = _sm1000.GalvoY;
            }
        }
        #region Overrides
        /// <summary>
        /// Generic Title for the property page
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Scan Galvos";
        }
        #endregion Overrides
    }
}
