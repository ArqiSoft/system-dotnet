using System;
using System.Collections.Generic;

namespace Sds.Domain
{
    public class FieldDefinition : ValueObject<FieldDefinition>
    {
        public string Name { get; private set; }
        public string DataType { get; private set; }

        public FieldDefinition(string name, string dataType)
        {
            Name = name;
            DataType = dataType;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Name, DataType };
        }
    }
}
