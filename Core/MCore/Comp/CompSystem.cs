using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MCore.Comp
{
    public class CompSystem : CompBase
    {
        /// <summary>
        /// Write the basic controller script list of commands
        /// </summary>
        [Browsable(true)]
        [Category("CompSystem")]
        [Description("Write the basic controller script list of commands")]
        public bool LogCommandScript
        {
            get { return GetPropValue(() => LogCommandScript); }
            set { SetPropValue(() => LogCommandScript, value); }
        }
        /// <summary>
        /// Connected
        /// </summary>
        [Browsable(true)]
        [Category("CompSystem")]
        [XmlIgnore]
        public bool Connected
        {
            get { return GetPropValue(() => Connected, false); }
            set { SetPropValue(() => Connected, value); }
        }
        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CompSystem()
        {
        }

        /// <summary>
        /// Manual creation constructor
        /// </summary>
        /// <param name="name"></param>
        public CompSystem(string name) 
            : base (name)
        {
        }
        #endregion Constructors

        #region Virtual Functions
        
        
        /// <summary>
        /// Execuate a string command to te controller
        /// </summary>
        /// <param name="command"></param>
        public virtual void ExecuteCommand(string command)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public virtual object QueryCommand(string command)
        {
            return null;
        }

        /// <summary>
        /// Execuate a command and parameter to the controller
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteCommand(object command, object parameters)
        {
            
        }


        /// <summary>
        /// Query a command and parameter to the controller
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual object QueryCommand(object command, object parameter)
        {
            return null;
        }

        #endregion Virtual Functions
    }
}
