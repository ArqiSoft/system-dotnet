using FluentAssertions;
using System.IO;
using System.Linq;
using Xunit;

namespace Sds.RdfParser.Tests
{
    public class RxnFileReaderTests
    {
        [Fact]
        public void ReadRxnMethod()
        {
            var records = new RdfParser(new MemoryStream(Resource._10001)).ToList();

            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                record.Data.Should().NotBeNull();
            }

            records.Count().Should().Be(1);
        }
    }
}
