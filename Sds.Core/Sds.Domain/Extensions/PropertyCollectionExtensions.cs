using System.Collections.Generic;

namespace Sds.Domain
{
    public static class CollectionExtensions
    {
        public static void Add(this IList<Property> properties, string name, object value, double? error = null)
        {
            if (value != null)
            {
                properties.Add(new Property(name, value, error));
            }
        }
    }
}
