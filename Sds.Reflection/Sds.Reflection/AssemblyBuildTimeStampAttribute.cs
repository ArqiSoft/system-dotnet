using System;

namespace Sds.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyBuildTimeStampAttribute : Attribute
    {
        public string BuildTimeStamp { get; }
        public AssemblyBuildTimeStampAttribute() : this(string.Empty) { }
        public AssemblyBuildTimeStampAttribute(string value) => BuildTimeStamp = value;
    }
}
