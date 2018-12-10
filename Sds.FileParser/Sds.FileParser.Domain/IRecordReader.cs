using System.Collections.Generic;

namespace Sds.FileParser
{
    public interface IRecordReader :  IEnumerable<Record>
    {
        IEnumerable<string> Extensions();
    }
}
