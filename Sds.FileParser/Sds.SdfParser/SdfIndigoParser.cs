using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using Sds.FileParser;
using com.epam.indigo;

namespace Sds.SdfParser
{
	internal class SdfIndigoRecordsEnumerator : IEnumerator<Record>
	{
        private Indigo indigo = new Indigo();

        private int ordinal = -1;

        private int index = 0;

        private Record current = null;

        private IndigoObject reader = null;

        public SdfIndigoRecordsEnumerator(Stream stream)
        {
            indigo.setOption("ignore-stereochemistry-errors", "true");
            indigo.setOption("unique-dearomatization", "false");
            indigo.setOption("ignore-noncritical-query-features", "true");
            indigo.setOption("timeout", "600000");

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                reader = indigo.iterateSDF(indigo.loadBuffer(ms.ToArray()));

                index = reader.count();
            }
        }
        public bool MoveNext()
        {
            current = null;

            return ++ordinal < index;
        }


        public void Reset()
        {
            current = null;
            ordinal = -1;
        }

        object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

        public Record Current
        {
            get
            {
                if (current == null)
                {
                    try
                    {
                        var indigoObj = reader.at(ordinal);

                        var Data = indigoObj.molfile();

                        var recordName = indigoObj.name();

                        var properties = indigoObj.iterateProperties().Cast<IndigoObject>().Select(p => new PropertyValue() { Name = p.name(), Value = p.rawData(), Type = PropertyType.String }).ToList();

                        if(!recordName.Equals(" ") && !recordName.Equals("") && !recordName.Equals("  "))
                        {
                            properties.Add(new PropertyValue { Name = "__NAME", Value = recordName, Type = PropertyType.String });
                        }
                        return new Record()
                        {
                            Data = indigoObj.molfile(),
                            Index = ordinal,
                            Type = RecordType.Chemical,
                            Properties = properties
                        };
                    }
                    catch(Exception ex)
                    {
                        return new Record()
                        {
                            Index = ordinal,
                            Type = RecordType.Chemical,
                            Error = ex.Message
                        };
                    }
                }
                return current;
            }
        }

        void IDisposable.Dispose()
		{
			if (reader != null)
				reader.Dispose();
		}
	}

	public class SdfIndigoParser : IRecordReader
	{
		private Stream stream;

		public SdfIndigoParser(Stream stream)
		{
			this.stream = stream;
		}

		public IEnumerable<string> Extensions()
		{
			return new List<string>() { ".SDF", ".MOL" };
		}

		public IEnumerator<Record> GetEnumerator()
		{
			return getEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return getEnumerator();
		}

		private IEnumerator<Record> getEnumerator()
		{
			return new SdfIndigoRecordsEnumerator(stream);
		}
	}
}
