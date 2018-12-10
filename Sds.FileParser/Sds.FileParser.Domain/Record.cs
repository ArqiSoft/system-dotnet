using System.Collections.Generic;

namespace Sds.FileParser
{
    public enum RecordType
    {
        Chemical = 0,
        Reaction,
        Spectrum,
        Crystal
    }

    public class Record
    {
        public RecordType Type { get; set; }
        public long Index { get; set; }
        public string Data { get; set; }
        public IEnumerable<PropertyValue> Properties { get; set; }
        public string Error { get; set; }
    }
}
