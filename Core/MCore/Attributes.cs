using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCore
{
    /// <summary>
    /// Attribute to mark a method or property for selection in state machine
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class StateMachineEnabled : Attribute
    {
    }

    /// <summary>
    /// Release notes to be used in AssemblyInfo.cs file
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyReleaseNotesAttribute : Attribute 
    {
        public string ReleaseNotes { get; set; }
        public AssemblyReleaseNotesAttribute() : this(string.Empty) {}
        public AssemblyReleaseNotesAttribute(string txt) { ReleaseNotes = txt; }
    }
}

