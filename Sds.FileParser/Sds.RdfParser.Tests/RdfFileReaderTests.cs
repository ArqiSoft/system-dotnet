using FluentAssertions;
using System.IO;
using Xunit;

namespace Sds.RdfParser.Tests
{
    public class RdfFileReaderTests
    {
        [Fact]
        public void ReadRdfMethod()
        {
            var records = new RdfParser(new MemoryStream(Resource.ccr0402));
            var i = 0;
            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                i++;
                if (i == 105)
                {
                    record.Should().BeOfType<FileParser.Record>();
                }
            }
        }
    }
}
