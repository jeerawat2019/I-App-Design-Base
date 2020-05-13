using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using MCore;
using MCore.Comp;
using MCore.Comp.VisionSystem;
using MDouble;

using AppMachine.Comp.Station;

namespace AppMachine.Comp.Vision
{
    public class AppVisionInspResult : AppVisionResultBase
    {

        #region Result Exchange
        public Spec<int> CountKey
        {
            get { return GetPropValue(() => CountKey, new Spec<int>()); }
            set { SetPropValue(() => CountKey, value); }
        }

        public int Count
        {
            get { return GetPropValue(() => Count); }
            set { SetPropValue(() => Count, value); }
        }

        public Spec<double> ScoreKey
        {
            get { return GetPropValue(() => ScoreKey, new Spec<double>()); }
            set { SetPropValue(() => ScoreKey, value); }
        }

        public double Score
        {
            get { return GetPropValue(() => Score); }
            set { SetPropValue(() => Score, value); }
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppVisionInspResult()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppVisionInspResult(string name)
            : base(name)
        {
            
        }
        #endregion Constructors


        public override void Initialize()
        {
            base.Initialize();
            
        }


        /// <summary>
        /// Process the results, set any data values, and determine if success
        /// </summary>
        public override void ProcessResults()
        {
            base.ProcessResults();

          
        }
    }

}