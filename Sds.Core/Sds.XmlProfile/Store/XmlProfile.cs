using System;
using System.Collections.Generic;

namespace Sds.XmlProfile
{
    public class XmlProfile
    {
        public Guid UserKey = Guid.Empty;
        //public string Names = string.Empty;
        //public string ValuesString = string.Empty;
        //public string ValuesBinary = null;
        public DateTime LastUpdated = DateTime.Now;

        public Dictionary<string, string> Values = new Dictionary<string, string>();
    }
}