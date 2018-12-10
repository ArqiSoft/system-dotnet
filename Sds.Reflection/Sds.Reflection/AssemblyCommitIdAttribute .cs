using System;

namespace Sds.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCommitIdAttribute : Attribute
    {
        public string CommitId { get; }
        public AssemblyCommitIdAttribute() : this(string.Empty) { }
        public AssemblyCommitIdAttribute(string value) => CommitId = value;
    }
}
