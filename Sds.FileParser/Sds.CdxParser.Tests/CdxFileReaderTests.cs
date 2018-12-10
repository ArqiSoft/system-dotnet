using FluentAssertions;
using System.IO;
using Xunit;

namespace Sds.CdxParser.Tests
{
    public class CdxFileReaderTests
    {
        [Fact]
        public void ReadCdxMethod()
        {
            var records = new CdxParser(new MemoryStream(Resource._10000_10Mos));

            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                //record.Data.Should().NotBeNull();
            }
        }

        [Fact]
        public void ReadCdxWithWhileIterationMethod()
        {
            var records = new CdxParser(new MemoryStream(Resource._125_11Mos));

            var enumerator = records.GetEnumerator();

            int totalRecords = 0;

            while (enumerator.MoveNext())
            {
                var record = enumerator.Current;

                record.Data.Should().NotBeNull();

                totalRecords++;
            }

            totalRecords.Should().Be(3);
        }

        [Fact]
        public void ReadCdxBeilstein()
        {
            var records = new CdxParser(new MemoryStream(Resource.Scheme_1));

            foreach (var record in records)
            {
                record.Should().BeOfType<FileParser.Record>();
                //record.Data.Should().NotBeNull();
            }
        }
    }
}
