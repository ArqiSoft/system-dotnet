using System;

namespace Sds.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyCommitAuthorAttribute : Attribute
    {
        public string Author { get; }
        public AssemblyCommitAuthorAttribute() : this(string.Empty) { }
        public AssemblyCommitAuthorAttribute(string value) => Author = value;
    }
}
