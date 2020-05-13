using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;

namespace MCore.Comp.VisionSystem
{
    public class Cognex9 : VisionSystemBase
    {

        private CogFrameGrabbers _framegrabberCams = null;

        /// <summary>
        /// Get the list of FrameGrabbers
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]        
        public CogFrameGrabbers FramegrabberCams
        {
            get { return _framegrabberCams; }
        }


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Cognex9()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public Cognex9(string name)
            : base(name)
        {
        }
        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Initialize this Component
        /// </summary>
        public override void Initialize()
        {
            // Initialize 
            if (Simulate == eSimulate.SimulateDontAsk)
            {
                throw new ForceSimulateException("Always Simulate");
            }

            try
            {
                U.SleepWithEvents(100);
                _framegrabberCams = new CogFrameGrabbers();

                if (_framegrabberCams.Count < 6)
                {
                    int jjb = 0;
                }
                Simulate = eSimulate.None;

                // Continue with children
                base.Initialize();
            }
            catch (ForceSimulateException fsex)
            {
                throw fsex;
            }
            catch (Exception ex)
            {
                throw new ForceSimulateException(ex);
            }

        }

        #endregion Overrides
    }
}
