using System.Collections.Generic;

namespace Sds.FileParser
{
    public static class RecordExtensions
    {
        public static void AddJsonProperty(this Record record, string name, string value)
        {

            record.AddProperty(new PropertyValue()
            {
                Name = name,
                Value = value,
                Type = PropertyType.Json
            });
        }

        public static void AddProperty(this Record record, string name, string value, PropertyType type = PropertyType.String)
        {
            record.AddProperty(new PropertyValue()
            {
                Name = name,
                Value = value,
                Type = type
            });
        }

        public static void AddProperty(this Record record, string categoryUri, string name, int value)
        {
            record.AddProperty(name, value.ToString(), PropertyType.Int);
        }

        public static void AddProperty(this Record record, string categoryUri, string name, double value)
        {
            record.AddProperty(name, value.ToString(), PropertyType.Double);
        }

        public static void AddProperty(this Record record, PropertyValue property)
        {
            if (record.Properties == null)
                record.Properties = new List<PropertyValue>();

            (record.Properties as List<PropertyValue>).Add(property);
        }
    }
}
