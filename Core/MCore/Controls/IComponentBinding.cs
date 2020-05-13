using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MCore.Controls
{
    /// <summary>
    /// Interface for componentBinding
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComponentBinding<T>
    {
        T Bind { get; set; }
    }
}
