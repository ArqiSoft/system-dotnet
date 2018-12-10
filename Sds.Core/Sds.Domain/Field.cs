using System;
using System.Collections.Generic;

namespace Sds.Domain
{
    public class Field : ValueObject<Field>
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Field(string name, string value)
        {
            Name = name;
            Value = value;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Name, Value };
        }
    }
}
