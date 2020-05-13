using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;
using MCore.Comp;
using MCore.Comp.SMLib.Flow;
namespace MCore.Comp.SMLib.Path
{
    public class SMPathOutBool : SMPathOut
    {

        #region Serialized Properties

        /// <summary>
        /// Is the output True or false?
        /// </summary>
        public bool True { get; set; }

        /// <summary>
        /// The duration to delay the path out
        /// </summary>
        public int PathOutDelayMS { get; set; }

        /// <summary>
        /// Either 'A' or 'B'
        /// Used to identify between the two SMPathOutBool outputs
        /// We cannot use True because it can change
        /// </summary>
        public string ID { get; set; }

        #endregion Serialized Properties

        [XmlIgnore]
        public long TimeToEnableTargetID { get; set; }

        /// <summary>
        /// Returns either 'T' of 'F'
        /// </summary>
        public string Text
        {
            get { return True ? "T" : "F"; }
        }

        /// <summary>
        /// Returns true if we have reached a target
        /// </summary>
        public override bool HasTargetID
        {
            get 
            {
                if (TimeToEnableTargetID != 0)
                {
                    if (U.DateTimeNow < TimeToEnableTargetID)
                    {
                        return false;
                    }
                    double timeOver = U.TicksToMS(U.DateTimeNow - TimeToEnableTargetID);
                    if (timeOver < 200)
                    {
                        U.LogWarning("Decision ({0}) Timed out at {1} mS", Owner.BestDisplayText, PathOutDelayMS);
                    }
                    TimeToEnableTargetID = 0;
                }
                return base.HasTargetID;
            }
        }


        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPathOutBool()
        {
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="initialGridDistance"></param>
        /// <param name="bTrue"></param>
        public SMPathOutBool(float initialGridDistance, bool bTrue)
        {
            True = bTrue;
            ID = bTrue ? "A" : "B";
            GridDistance = initialGridDistance;
        }
        #endregion Constructors
        
        /// <summary>
        /// Recursivley make a clone of this item 
        /// </summary>
        /// <returns>New cloned instance</returns>
        public override SMPath Clone()
        {
            SMPathOutBool newPathOut = base.Clone() as SMPathOutBool;
            newPathOut.True = True;
            newPathOut.ID = ID;
            return newPathOut;
        }

        public void ApplyPathDelay(SMFlowContainer parentContainer)
        {
            if (TimeToEnableTargetID == 0 && PathOutDelayMS > 0)
            {
                parentContainer.EnterPathDelay(PathOutDelayMS);
                TimeToEnableTargetID = U.DateTimeNow + U.MSToTicks(PathOutDelayMS);
            }
        }

        public void SwitchLogic()
        {
            True = !True;
        }
    }
}

