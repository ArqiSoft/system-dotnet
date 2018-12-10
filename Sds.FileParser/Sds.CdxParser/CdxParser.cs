using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Sds.FileParser;
using com.epam.indigo;
using System.Linq;

namespace Sds.CdxParser
{
    public class CdxRecordsEnumerator : IEnumerator<Record>
    {
        private Indigo indigo = new Indigo();

        private IEnumerable enumerator = null;

        private int index = -1;

        private IndigoObject reader = null;

        private Record current = null;

        private IndigoObject _nextIndigoObject = null;

        public CdxRecordsEnumerator(Stream stream)
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

                reader = indigo.iterateCDX(indigo.loadBuffer(ms.ToArray()));
            }
        }

        public bool MoveNext()
        {
            current = null;

            if (reader.hasNext())
            {
                _nextIndigoObject = reader.next();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            index = 0;
            current = null;
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
                        var properties = _nextIndigoObject.iterateProperties().Cast<IndigoObject>().Select(p => new PropertyValue() { Name = p.name(), Value = p.rawData(), Type = PropertyType.String }).ToList();

                        return new Record()
                        {
                            Data = _nextIndigoObject.molfile(),
                            Index = _nextIndigoObject.index(),
                            Type = RecordType.Chemical,
                            Properties = properties
                        };
                    }
                    catch (Exception ex)
                    {
                        return new Record()
                        {
                            Index = _nextIndigoObject.index(),
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
        }
    }

    public class CdxParser : IRecordReader
    {
        private Stream stream;

        public CdxParser(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<string> Extensions()
        {
            return new List<string>() { ".CDX" };
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
            return new CdxRecordsEnumerator(stream);
        }
    }
}
