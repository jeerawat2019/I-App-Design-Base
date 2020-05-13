using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore.Comp.SMLib.Flow
{
    public class SMAndCond:SMSubCondBase
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMAndCond()
        {
        }

        /// <summary>
        /// Creation constructor
        /// </summary>
        /// <param name="childName"></param>
        public SMAndCond(string childName)
            : base(childName)
        {
            
        }

        #endregion


        public override bool Validate()
        {
            lock (this)
            {
                foreach (CompBase comp in this.ChildArray)
                {
                    if (comp is SMSubCondBase)
                    {
                        if (!(comp as SMSubCondBase).Validate())
                        {
                            ValidationResult = false;
                            return ValidationResult;
                        }
                    }
                }
            }
            ValidationResult = true;
            return ValidationResult;
        }
    }
}
