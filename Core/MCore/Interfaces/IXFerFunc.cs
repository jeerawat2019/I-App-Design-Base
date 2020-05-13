using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MDouble;

namespace MCore.Interfaces
{
    public interface IXFerFunc
    {
        void ClearTraining();
        double Calculate();

        MDoubleBase Result
        {
            get; // 
            set;  // Teach
        }

    }
}
