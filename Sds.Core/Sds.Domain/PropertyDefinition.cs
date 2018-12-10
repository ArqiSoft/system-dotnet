using System;
using System.Collections.Generic;

namespace Sds.Domain
{
    public enum PropertyValueType
    {
        String = 0,
        Boolean,
        Integer,
        Double,
        Choice,
        Complex
    }

    public class PropertyDefinition : ValueObject<PropertyDefinition>
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }
        public PropertyValueType ValueType { get; private set; }
        public string DefaultUnit { get; private set; }

        public PropertyDefinition(string name, string displayName = null, PropertyValueType valueType = PropertyValueType.String, string defaultUnit = null)
        {
            Name = name;
            DisplayName = name;
            ValueType = valueType;
            DefaultUnit = defaultUnit;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Name };
        }
    }
}
