using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace AppMachine.Comp
{
    public class SubCategoryAttribute : CategoryAttribute
    {
        public SubCategoryAttribute(string name) :
            base(name)
        {

        }
    }
}
