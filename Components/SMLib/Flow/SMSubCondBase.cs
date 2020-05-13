using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MCore.Comp.SMLib.Flow
{
    public class SMSubCondBase:CompBase
    {
        [XmlIgnore]
        public TreeNode RefNode = null;

        private Boolean _validationResult = false;
        [XmlIgnore]
        public Boolean ValidationResult
        {
            get
            {
                return _validationResult;
            }
            set
            {
                _validationResult = value;
                SetNodeGUI(value);
            }
        }

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMSubCondBase()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public SMSubCondBase(string childName)
            : base(childName)
        {

        }

        #endregion

        public virtual Boolean Validate()
        {
            return false;
        }

        protected void SetNodeGUI(Boolean resultOK)
        {
            if (RefNode != null)
            {
                RefNode.ForeColor = resultOK ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
            }
        }

        public virtual void Rebuild()
        {
            lock (this)
            {
                if (!HasChildren)
                {
                    return;
                }
                foreach (CompBase comp in this.ChildArray)
                {
                    if (comp is SMSubCondBase)
                    {
                        (comp as SMSubCondBase).Rebuild();
                    }
                }
            }
        }
    }
}
