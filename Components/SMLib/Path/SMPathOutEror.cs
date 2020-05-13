using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MCore.Comp.SMLib.Path
{
    public class SMPathOutError : SMPathOut
    {


        private List<Exception> _exceptions = new List<Exception>();

        [XmlIgnore]
        [Browsable(false)]
        public List<Exception> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMPathOutError()
        {
        }
        /// <summary>
        /// Manual creation Constructor
        /// </summary>
        /// <param name="initialGridDistance"></param>
        public SMPathOutError(float initialGridDistance)
        {
            // Starts out to the left
            GridDistance = initialGridDistance;
        }
        #endregion Constructors

        /// <summary>
        /// Process any errors contained
        /// </summary>
        public void ProcessErrors()
        {
            if (Exceptions.Count > 0)
            {
                // Process the exceptions
                foreach (Exception ex in Exceptions)
                {
                    try
                    {
                        U.Log(ex);
                    }
                    catch (Exception exex)
                    {
                        MessageBox.Show(exex.Message);
                    }
                }
            }
        }
    }
}
