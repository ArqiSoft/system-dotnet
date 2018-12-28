using FluentAssertions;
using System.IO;
using System.Linq;
using Xunit;

namespace Sds.SdfParser.Tests
{
    public class SdfFileReaderTests
    {
        [Fact]
        public void ReadSdfMethod()
        {
            var records = new SdfIndigoParser(new MemoryStream(Resource.AChemo_extract_of_dictionary_names_and_CAS_numbers_For_dictionary_for_deposition)).ToList();

            foreach (var record in records)
            {
                Assert.IsType<FileParser.Record>(record);
                //Assert.NotNull(record.Data);
            }

            records.Count().Should().Be(2117);
        }


        [Fact]
        public void ReadSdfUsingIndigoMethod()
        {
            var records = new SdfIndigoParser(new MemoryStream(Resource.HMDB));

            foreach (var record in records)
            {
                Assert.IsType<FileParser.Record>(record);
                Assert.NotNull(record.Data);
            }
        }

        [Fact]
        public void ReadSdfUsingIndigoMethod2()
        {
            var records = new SdfIndigoParser(new MemoryStream(Resource.AChemo_extract_of_dictionary_names_and_CAS_numbers_For_dictionary_for_deposition)).ToList();

            var nullRecords = records.Where(r => r == null).ToList();

            foreach (var record in records)
            {
                Assert.IsType<FileParser.Record>(record);
                //Assert.NotNull(record.Data);
            }
        }

        [Fact]
        public void ReadSdfUsingIndigoMethodByBoris()
        {
            var records = new SdfIndigoParser(new MemoryStream(Resource.nr_ahr)).ToList();

            foreach (var record in records)
            {
                Assert.IsType<FileParser.Record>(record);
                Assert.NotNull(record.Data);
            }
        }

        //[Fact]
        //public void ReadSdfUsingIndigoMethod1MRecords()
        //{
        //    var records = new SdfIndigoParser(File.Open("Resources\\chembl_23.sdf", FileMode.Open));

        //    foreach (var record in records)
        //    {
        //        Assert.IsType<FileParser.Record>(record);
        //        Assert.NotNull(record.Data);
        //    }
        //}
    }
}
