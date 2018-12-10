using FluentAssertions;
using System.IO;
using Xunit;

namespace Sds.JcampParser.Tests
{
    public class JdxFileReaderTests
    {
        [Fact]
        public void JdxFileReaderTest()
        {
            var records = new JcampReader(new MemoryStream(Resource._1567755));

            int i = 0;
            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                record.Data.Should().NotBeNull();
                i++;
            }

            i.Should().Be(1);
        }
    }
}
