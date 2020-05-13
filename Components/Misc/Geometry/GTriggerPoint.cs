using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using MDouble;
using MCore;

namespace MCore.Comp.Geometry
{
    public class GTriggerPoint : GDistance, IComparable<GTriggerPoint>
    {

        #region Privates

        private string _triggerID = string.Empty;
        private bool _anchor = false;
        private bool _repeating = false;

        #endregion Privates

        #region Public Constants


        #endregion Public Constants

        #region Public Properties


        /// <summary>
        /// Tigger action or method to call
        /// </summary>
        [Browsable(true)]
        [Category("Trigger")]
        [Description("Trigger action or method to call")]
        public string TriggerID
        {
            get { return _triggerID; }
            set { _triggerID = value; }
        }

        /// <summary>
        /// An anchor and identifies the beginning or ending of a group
        /// </summary>
        [Browsable(true)]
        [Category("Trigger")]
        [Description("An anchor and identifies the beginning or ending of a group")]
        public bool Anchor
        {
            get { return _anchor; }
            set { _anchor = value; }
        }

        /// <summary>
        /// True if multiple triggers are expected at a fixed distance
        /// </summary>
        [Browsable(true)]
        [Category("Trigger")]
        [Description("True if multiple triggers are expected at a fixed distance")]
        public bool Repeating
        {
            get { return _repeating; }
            set { _repeating = value; }
        }

        private Microseconds _pulseWidth = 0.0;
        /// <summary>
        /// For serializing PulseWidth
        /// </summary>
        [Browsable(false)]
        [XmlElement("PulseWidth")]
        public Microseconds SerPulseWidth
        {
            get { return _pulseWidth; }
            set { _pulseWidth = value; }
        }

        /// <summary>
        /// The duration of the pulse (us)
        /// </summary>
        [Browsable(true)]
        [Category("Location")]
        [Description("The duration of the pulse (us)")]
        [XmlIgnore]
        public Microseconds PulseWidth
        {
            get
            {
                if (_pulseWidth.Val == 0.0)
                {
                    return _line.PulseWidth;
                }
                return _pulseWidth;
            }
            set { _pulseWidth.Val = value; }
        }


        #endregion Public Properties

        #region Constructors
        /// <summary>
        /// Default constructor for serialization
        /// </summary>
        public GTriggerPoint()
        {
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public GTriggerPoint(GLine line)
            : base(line)
        {
            AssignTriggerID();
        }
        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="line"></param>
        /// <param name="mmDistance"></param>
        public GTriggerPoint(GLine line, Millimeters mmDistance)
            : base(line)
        {
            Distance = mmDistance;
            AssignTriggerID();
        }

        /// <summary>
        /// Assign the TriggerID
        /// </summary>
        protected virtual void AssignTriggerID()
        {
            TriggerID = GetType().Name;
        }
        /// <summary>
        /// Make a copy
        /// </summary>
        /// <param name="newTriggerPt"></param>
        /// <returns></returns>
        public virtual GTriggerPoint Clone(GTriggerPoint newTriggerPt)
        {
            newTriggerPt.Distance = Distance;
            newTriggerPt.LeadTime = LeadTime;
            newTriggerPt.TriggerID = TriggerID;
            newTriggerPt.Anchor = Anchor;
            newTriggerPt.Repeating = Repeating;
            newTriggerPt._pulseWidth.Val = _pulseWidth;
            return newTriggerPt;
        }
        #endregion Constructors

        /// <summary>
        /// Motion System calls this to setup callback when trigger operation is complete
        /// </summary>
        /// <param name="externalTrigger">Flag to indicate if the triggerring is external or manual</param>
        public virtual void PrepareCallback(bool externalTrigger)
        {
            // Do nothing by default
        }

        /// <summary>
        /// If PSO not setup, then robot is to move to trigger point and call this function.  When completed, can move to the next trigger point (or line)
        /// </summary>
        public virtual void DoStationaryAction()
        {
            // Do nothing by default
        }

        /// <summary>
        /// Simulate the trigger
        /// </summary>
        public virtual void SimulateTrigger()
        {
        }
        /// <summary>
        /// Allow to be compared
        /// </summary>
        /// <param name="otherTriggerPt"></param>
        /// <returns></returns>
        public int CompareTo(GTriggerPoint otherComp)        
        {
            return Distance.Val.CompareTo((otherComp as GTriggerPoint).Distance.Val);
        }

        /// <summary>
        /// Returns the length of the dispsensing based on profile velocities
        /// </summary>
        /// <returns></returns>
        public Millimeters GetPulseLength()
        {
            // Find first Profile Point
            Seconds secRemaining = PulseWidth.ToSeconds;
            return secRemaining * _line.Speed;


        //    Millimeters spentDist = Distance;
        //    Millimeters retLength = 0.0;
        //    MillimetersPerSecond lineSpeed = _line.Speed;



        //    MillimetersPerSecond speed = lineSpeed;
        //    GProfileChange[] profilePts = _line.ProfilePts;
        //    for (int i = 0; i < profilePts.Length && secRemaining > 0.0; i++)
        //    {
        //        GProfileChange profilePt = profilePts[i];
        //        speed = profilePt.Speed;
        //        if (spentDist < profilePt.Distance)
        //        {
        //            if (speed == 0)
        //            {
        //                speed = lineSpeed;
        //            }
        //            if (speed == 0)
        //            {
        //                secRemaining = 0.0;
        //                break;
        //            }

        //            Millimeters distProfile = profilePt.Distance - spentDist;
        //            Seconds timeProfile = distProfile / speed;
        //            if (timeProfile < secRemaining)
        //            {
        //                // Use up all of profile and continue
        //                retLength += distProfile;
        //                spentDist += distProfile;
        //                secRemaining -= timeProfile;
        //            }
        //            else
        //            {
        //                // Use up portion of profile and we are done
        //                break;
        //            }
        //        }
        //    }

        //    retLength += secRemaining * speed;
        //    return retLength;
        }


    }
}
