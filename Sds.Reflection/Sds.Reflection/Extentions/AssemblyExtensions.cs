using System;
using System.Linq;
using System.Reflection;

namespace Sds.Reflection
{
    public static class AssemblyExtensions
    {
        public static T GetCustomAttibute<T>(this Assembly assembly) where T : Attribute
        {
            return (T)assembly.GetCustomAttributes(typeof(T)).Single();
        }

        public static string GetCompany(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
        }

        public static string GetProduct(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        }

        public static string GetDescription(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        }

        public static string GetTitle(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        }

        public static string GetVersion(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;
        }

        public static string GetCommitId(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyCommitIdAttribute>()?.CommitId;
        }

        public static string GetCommitAuthor(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyCommitAuthorAttribute>()?.Author;
        }

        public static string GetBuildTimeStamp(this Assembly assembly)
        {
            return assembly.GetCustomAttribute<AssemblyBuildTimeStampAttribute>()?.BuildTimeStamp;
        }
    }
}
