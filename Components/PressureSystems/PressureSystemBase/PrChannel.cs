using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MDouble;

using MCore.Comp;

namespace MCore.Comp.PressureSystem
{
    public class PrChannel : CompBase
    {
        protected PressureSystemBase _prSystem = null;

        /// <summary>
        /// The parent PressureSystem reference
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public PressureSystemBase PrSystem
        {
            get { return _prSystem; }
        }

        /// <summary>
        /// Get/Set the channel
        /// </summary>
        [Browsable(true)]
        [Category("Channel")]
        [XmlIgnore]
        public int Channel
        {
            get 
            {
                try
                {
                    return Convert.ToInt32(ChannelID);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                ChannelID = value.ToString();
            }
        }


        /// <summary>
        /// Get/Set the channel Id
        /// </summary>
        [Browsable(true)]
        [Category("Channel")]
        [XmlElement("Channel")]
        public string ChannelID
        {
            get { return GetPropValue(() => ChannelID, "0"); }
            set { SetPropValue(() => ChannelID, value); }
        }

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public PrChannel()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public PrChannel(string name)
            : base(name)
        {
        }


        /// <summary>
        /// Return if this is equal to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PrChannel))
                return false;            
            return (obj as PrChannel).ChannelID == this.ChannelID;
        }
        /// <summary>
        /// Return the hascode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        #endregion Constructors

        public override void InitializeIDReferences()
        {
            base.InitializeIDReferences();
            _prSystem = GetParent<PressureSystemBase>();
            if (_prSystem == null)
            {
                throw new MCoreExceptionPopup("The '{0}' input must have a PressureSystemBase parent", Name);
            }

        }
    }    
}
