using System;
using System.Collections.Generic;

namespace Sds.Domain
{
    public class Issue : ValueObject<Issue>
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public string AuxInfo { get; private set; }

        public Issue(string code, string message = null, string aux = null)
        {
            Code = code;
            Message = message;
            AuxInfo = aux;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Code, Message };
        }
    }
}
