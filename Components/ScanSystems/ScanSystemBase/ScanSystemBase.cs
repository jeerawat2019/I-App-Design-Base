using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

using MCore.Comp;
using MCore.Comp.MotionSystem;
using MDouble;

namespace MCore.Comp.ScanSystem
{
    public partial class ScanSystemBase : MotionSystemBase
    {
        private RealAxis _galvoX = null;
        private RealAxis _galvoY = null;
        private RealAxis _galvoZ = null;

        /// <summary>
        /// Get the reference to the Galvo X Axis
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public RealAxis GalvoX
        {
            get { return _galvoX; }
        }
        /// <summary>
        /// Get the reference to the Galvo Y Axis
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public RealAxis GalvoY
        {
            get { return _galvoY; }
        }

        /// <summary>
        /// Get the reference to the Galvo Z Axis
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public RealAxis GalvoZ
        {
            get { return _galvoZ; }
        }


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScanSystemBase()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public ScanSystemBase(string name) 
            : base (name)
        {
        }
        #endregion Constructors

        #region Overrides
        /// <summary>
        /// Set up the ID references for this object
        /// </summary>
        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            RealAxis[] axes = FilterByType<RealAxis>();
            if (axes.Length > 0)
            {
                try
                {
                    _galvoX = axes.Where(c => c.AxisNo == 0).SingleOrDefault();
                    _galvoY = axes.Where(c => c.AxisNo == 1).SingleOrDefault();
                    _galvoZ = axes.Where(c => c.AxisNo == 2).SingleOrDefault();
                }
                catch { }
            }

        }
        #endregion Overrides

        #region Public Calls to do service

        public virtual void Scan()
        {
            // Simulation
        }

        public virtual void RegisterApiPanel(string apiPanel,Panel parrent)
        {
            // Simulation
        }

        public virtual void UnregisterApiPanel(string apiPanel)
        {
            // Simulation
        }


      
        #endregion Public Calls to do service
    }
}
