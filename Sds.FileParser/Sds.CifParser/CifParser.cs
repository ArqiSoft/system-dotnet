using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Sds.FileParser;

namespace Sds.CifParser
{
    public class CifRecordsEnumerator : IEnumerator<Record>
    {
        public const string Alpha = "Alpha";
        public const string Beta = "Beta";
        public const string Gamma = "Gamma";
        public const string ChemicalFormula = "ChemicalFormula";
        public const string ChemicalName = "ChemicalName";
        public const string LengthA = "LengthA";
        public const string LengthB = "LengthB";
        public const string LengthC = "LengthC";

        private List<string> pathes = new List<string>();

        private IEnumerator<string> enumerator = null;

        public CifRecordsEnumerator(string path)
        {
            pathes.Add(path);

            enumerator = pathes.GetEnumerator();
        }

        public bool MoveNext()
        {
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
                var crystal = Sds.Jmol.CifReader.Read(enumerator.Current);

                var properties = new List<PropertyValue>();
                properties.Add(new PropertyValue() { Name = ChemicalFormula, Value = crystal.ChemicalFormula });
                if (!string.IsNullOrEmpty(crystal.ChemicalName))
                    properties.Add(new PropertyValue() { Name = ChemicalName, Value = crystal.ChemicalName });
                properties.Add(new PropertyValue() { Name = Alpha, Value = crystal.Alpha.ToString() });
                properties.Add(new PropertyValue() { Name = Beta, Value = crystal.Beta.ToString() });
                properties.Add(new PropertyValue() { Name = Gamma, Value = crystal.Gamma.ToString() });
                properties.Add(new PropertyValue() { Name = LengthA, Value = crystal.LengthA.ToString() });
                properties.Add(new PropertyValue() { Name = LengthB, Value = crystal.LengthB.ToString() });
                properties.Add(new PropertyValue() { Name = LengthC, Value = crystal.LengthC.ToString() });
                foreach (var aux in crystal.AuxInfo)
                {
                    properties.Add(new PropertyValue() { Name = aux.Key, Value = aux.Value });
                }

                return new Record()
                {
                    Data = crystal.Cif,
                    Index = 0,
                    Type = RecordType.Crystal,
                    Properties = properties
                };
            }
        }

        void IDisposable.Dispose()
        {
        }
    }

    public class CifParser : IRecordReader, IDisposable
    {
        private string path;

        public CifParser(Stream s)
        {
            path = Path.GetTempFileName();

			using (FileStream fs = File.OpenWrite(path))
			{
				s.CopyTo(fs);
			}
		}

		public void Dispose()
		{
			if (File.Exists(path))
				File.Delete(path);
		}

		public IEnumerable<string> Extensions()
        {
            return new List<string>() { ".CIF" };
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
            return new CifRecordsEnumerator(path);
        }
    }
}
