namespace Sds.FileParser
{
    public enum PropertyType
    {
        String = 0,
        Int,
        Bool,
        Email,
        Url,
        Json,
        Double,
        Select,
        Date,
        Time,
        Datetime,
        Textarea,
    }
    public class PropertyValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public PropertyType Type { get; set; }
    }
}
