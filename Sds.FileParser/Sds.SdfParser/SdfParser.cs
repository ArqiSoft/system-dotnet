using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using Sds.FileParser;

namespace Sds.SdfParser
{
	internal class SdfRecordsEnumerator : IEnumerator<Record>
	{
		private SdfReader reader = null;

		private IEnumerator<SdfRecord> enumerator = null;

		private int index = 0;

		private Record current = null;

		public SdfRecordsEnumerator(Stream s)
		{
			reader = new SdfReader(s);
			enumerator = reader.Records.GetEnumerator();
		}
		public bool MoveNext()
		{
			current = null;

			return enumerator.MoveNext();
		}

		public void Reset()
		{
			index = 0;
			current = null;

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
				if (current == null)
				{
                    try
                    {
                        lock (reader)
					    {
						    var record = enumerator.Current as SdfRecord;

						    return new Record()
						    {
                                Type = RecordType.Chemical,
							    Data = record.ToString(),
							    Index = index++,
                                Properties = record.Properties.Select(p => p.Value.Select(v => new PropertyValue() { Name = p.Key, Value = v, Type = PropertyType.String /*v.ToPropertyType()*/ })).SelectMany(f => f).ToList()
                            };
					    }
                    }
                    catch
                    {
                        throw new Exception("Record can not be parsed.");
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

	public class SdfParser : IRecordReader
	{
		private Stream stream;

		public SdfParser(Stream stream)
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
			return new SdfRecordsEnumerator(stream);
		}
	}
}
