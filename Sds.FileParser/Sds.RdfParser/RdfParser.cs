using Sds.FileParser;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using com.epam.indigo;

namespace Sds.RdfParser
{
    class RdfRecordsEnumerator : IEnumerator<Record>
    {
        private Indigo indigo = new Indigo();

        private int ordinal = -1;

        private int index = 0;

        private Record current = null;

        private IndigoObject reader = null;

        public RdfRecordsEnumerator(Stream stream)
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

                reader = indigo.iterateRDF(indigo.loadBuffer(ms.ToArray()));

                index = reader.count();
            }
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

                        return new Record()
                        {
                            Data = indigoObj.rxnfile(),
                            Index = ordinal,
                            Type = RecordType.Reaction,
                            Properties = indigoObj.iterateProperties().Cast<IndigoObject>().Select(p => new PropertyValue() { Name = p.name(), Value = p.rawData(), Type = PropertyType.String }).ToList()
                        };
                    }                    
                    catch(Exception ex)
                    {
                        return new Record()
                        {
                            Index = ordinal,
                            Type = RecordType.Reaction,
                            Error = ex.Message
                        };
                    }
                }
                return current;
            }
        }

        public void Dispose()
        {
            indigo.Dispose();
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
    }

    public class RdfParser : IRecordReader
    {
        private Stream stream;

        public RdfParser(Stream stream)
        {
            this.stream = stream;
        }

        public RdfParser(string path)
        {
            stream = new FileStream(path, FileMode.Open);
        }

        public IEnumerable<string> Extensions()
        {
            return new List<string>() { ".RDF", ".RXN"};
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
            return new RdfRecordsEnumerator(stream);
        }
    }
}
