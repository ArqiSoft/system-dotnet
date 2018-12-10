using FluentAssertions;
using System.IO;
using System.Linq;
using Xunit;

namespace Sds.CifParser.Tests
{
    public class CifFileReaderTests
    {
        [Fact]
        public void ReadCifMethod()
        {
            var records = new CifParser(new MemoryStream(Resource._1100110));

            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                record.Data.Should().NotBeNull();
            }

            records.Count().Should().Be(1);
        }
    }
}
