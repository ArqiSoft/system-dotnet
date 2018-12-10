using Sds.FileParser;
using System.IO;
using System.Linq;
using Xunit;

namespace Sds.SdfParser.Tests
{
    public class MolFileReaderTests
    {
        [Fact]
        public void ReadMolMethod()
        {
            var records = new SdfIndigoParser(new MemoryStream(Resource._S__Glutamic_Acid)).ToList();

            foreach (var record in records)
            {
                //Assert.InstanceOfType(record, typeof(Record));
                Assert.NotNull(record.Data);
            }

            //Assert.Equal(records.Count(), 1);
        }
    }
}
