using System.Collections.Generic;

namespace Sds.Domain
{
    public static class IssuesCollectionExtensions
    {
        public static void Add(this ICollection<Issue> issues, string code, string message = null, string aux = null)
        {
            issues.Add(new Issue(code: code, message: message, aux: aux));
        }
    }
}
