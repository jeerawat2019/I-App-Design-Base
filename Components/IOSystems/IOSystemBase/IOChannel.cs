using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading;

using MDouble;

namespace MCore.Comp.IOSystem
{
    public class IOChannel : CompMeasure
    {
        protected IOSystemBase _ioSystem = null;

        /// <summary>
        /// The parent IOSystem reference
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public IOSystemBase IOSystem
        {
            get { return _ioSystem; }
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
        public IOChannel()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public IOChannel(string name)
            : base(name)
        {
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
            _ioSystem = GetParent<IOSystemBase>();
            if (_ioSystem == null)
            {
                throw new MCoreExceptionPopup("The '{0}' input must have a IOSystemBase parent", Name);
            }

        }
    }
}
