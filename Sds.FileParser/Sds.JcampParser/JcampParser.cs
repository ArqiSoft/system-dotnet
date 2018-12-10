using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sds.FileParser;

namespace Sds.JcampParser
{
    public class JcampRecordsEnumerator : IEnumerator<Record>
    {
        public const string Title = "Title";
        public const string DataType = "DataType";
        public const string DataClass = "DataClass";
        public const string Mol = "Mol";
        public const string Dx = "Dx";
        public const string Origin = "Origin";
        public const string Owner = "Owner";
        public const string Date = "Date";
        public const string Time = "Time";
        private int index = 0;
        private List<string> jcamps = new List<string>();

        private IEnumerator<string> enumerator = null;

        public JcampRecordsEnumerator(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                string jcamp = sr.ReadToEnd();

                jcamps.Add(jcamp);

                enumerator = jcamps.GetEnumerator();

            }
        }

        public bool MoveNext()
        {
            index++;
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
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
                var spectrum = Sds.JSpecView.JcampReader.Read(enumerator.Current);
                
                var properties = new List<PropertyValue>();
                properties.Add(new PropertyValue() { Name = Title, Value = spectrum.Title });
                properties.Add(new PropertyValue() { Name = DataType, Value = spectrum.DataType });
                properties.Add(new PropertyValue() { Name = DataClass, Value = spectrum.DataClass });
                properties.Add(new PropertyValue() { Name = Dx, Value = spectrum.Dx });
                properties.Add(new PropertyValue() { Name = Date, Value = spectrum.Date });
                properties.Add(new PropertyValue() { Name = Origin, Value = spectrum.Origin });
                properties.Add(new PropertyValue() { Name = Owner, Value = spectrum.Owner });
                properties.Add(new PropertyValue() { Name = Time, Value = spectrum.Time });
                if (!string.IsNullOrEmpty(spectrum.Mol))
                    properties.Add(new PropertyValue() { Name = Mol, Value = spectrum.Mol });

                Record record = new Record()
                {
                    Data = enumerator.Current,
                    Index = 0,
                    Type = RecordType.Spectrum,
                    Properties = properties
                };

                return record;
            }
        }

        void IDisposable.Dispose()
        {
        }
    }

    public class JcampReader : IRecordReader
    {
        private Stream stream;

        public JcampReader(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<string> Extensions()
        {
            return new List<string>() { ".DX", ".JDX" };
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
            return new JcampRecordsEnumerator(stream);
        }
    }
}
