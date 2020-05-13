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
    public class AppVisionCorrResult : AppVisionResultBase
    {
        #region Result Exchange
        //public Spec<int> FoundCountKey
        //{
        //    get { return GetPropValue(() => FoundCountKey, new Spec<int>()); }
        //    set { SetPropValue(() => FoundCountKey, value); }
        //}

        //public int FoundCount
        //{
        //    get { return GetPropValue(() => FoundCount); }
        //    set { SetPropValue(() => FoundCount, value); }
        //}
       
        //public Spec<double> ScoreKey
        //{
        //    get { return GetPropValue(() => ScoreKey, new Spec<double>()); }
        //    set { SetPropValue(() => ScoreKey, value); }
        //}

        //public double Score
        //{
        //    get { return GetPropValue(() => Score); }
        //    set { SetPropValue(() => Score, value); }
        //}


        public Spec<double> TransXKey
        {
            get { return GetPropValue(() => TransXKey, new Spec<double>()); }
            set { SetPropValue(() => TransXKey, value); }
        }

        public double TransX
        {
            get { return GetPropValue(() => TransX); }
            set { SetPropValue(() => TransX, value); }
        }

        public Spec<double> TransYKey
        {
            get { return GetPropValue(() => TransYKey, new Spec<double>()); }
            set { SetPropValue(() => TransYKey, value); }
        }

        public double TransY
        {
            get { return GetPropValue(() => TransY); }
            set { SetPropValue(() => TransY, value); }
        }

        public Spec<double> TransRKey
        {
            get { return GetPropValue(() => TransRKey, new Spec<double>()); }
            set { SetPropValue(() => TransRKey, value); }
        }

        public double TransR
        {
            get { return GetPropValue(() => TransR); }
            set { SetPropValue(() => TransR, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AppVisionCorrResult()
        {

        }

        /// <summary>
        /// Manual Creation Constructor
        /// </summary>
        /// <param name="name"></param>
        public AppVisionCorrResult(string name)
            : base(name)
        {
            
        }
        #endregion Constructors


        public override void Initialize()
        {
            base.Initialize();


            dataMap = new ExchangeDataMapBase[] 
            {
                new ExchangeDataMap<int>(() => FoundCountKey, () => FoundCount),
                new ExchangeDataMap<double>(() => ScoreKey, () => Score),
                new ExchangeDataMap<double>(() => TransXKey, () => TransX),
                new ExchangeDataMap<double>(() => TransYKey, () => TransY),
                new ExchangeDataMap<double>(() => TransRKey, () => TransR),
            };

            
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