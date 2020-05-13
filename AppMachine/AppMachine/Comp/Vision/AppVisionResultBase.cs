using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MCore.Comp.VisionSystem;
using MCore;

namespace AppMachine.Comp.Vision
{
    public class AppVisionResultBase : ResultsExchange
    {

        #region Result Exchange
        //public Spec<double> ImageHeightKey
        //{
        //    get { return GetPropValue(() => ImageHeightKey, new Spec<double>()); }
        //    set { SetPropValue(() => ImageHeightKey, value); }
        //}

        //public double ImageHeight
        //{
        //    get { return GetPropValue(() => ImageHeight); }
        //    set { SetPropValue(() => ImageHeight, value); }
        //}

        //public Spec<double> ImageWidthKey
        //{
        //    get { return GetPropValue(() => ImageWidthKey, new Spec<double>()); }
        //    set { SetPropValue(() => ImageWidthKey, value); }
        //}

        //public double ImageWidth
        //{
        //    get { return GetPropValue(() => ImageWidth); }
        //    set { SetPropValue(() => ImageWidth, value); }
        //}


        public Spec<int> FoundCountKey
        {
            get { return GetPropValue(() => FoundCountKey, new Spec<int>()); }
            set { SetPropValue(() => FoundCountKey, value); }
        }

        public int FoundCount
        {
            get { return GetPropValue(() => FoundCount); }
            set { SetPropValue(() => FoundCount, value); }
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

        public AppVisionResultBase()
        {

        }

        public AppVisionResultBase(String name):
            base(name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            dataMap = new ExchangeDataMapBase[] 
            {
                //new ExchangeDataMap<double>(() => ImageHeightKey, () => ImageHeight),
                //new ExchangeDataMap<double>(() => ImageWidthKey, () => ImageWidth),

                new ExchangeDataMap<int>(() => FoundCountKey, () => FoundCount),
                new ExchangeDataMap<double>(() => ScoreKey, () => Score),


            };
        }

    }
}
