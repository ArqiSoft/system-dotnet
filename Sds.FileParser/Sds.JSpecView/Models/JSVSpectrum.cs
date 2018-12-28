using System.Runtime.Serialization;
using Sds.Core;

namespace Sds.JSpecView
{
	[DataContract]
	public class JSVSpectrum
	{
		[DataMember]
		public string Title { get; set; }
		[DataMember]
		public string Jcamp { get; set; }
		[DataMember]
		public string Mol { get; set; }
		[DataMember]
		public byte[] Md5 { get { return string.IsNullOrEmpty(Jcamp) ? null : Jcamp.GetMD5Hash(); } }
		[DataMember]
		public string Dx { get; set; }
		[DataMember]
		public string DataType { get; set; }
		[DataMember]
		public string DataClass { get; set; }
		[DataMember]
		public string Origin { get; set; }
		[DataMember]
		public string Owner { get; set; }
		[DataMember]
		public string Date { get; set; }
		[DataMember]
		public string Time { get; set; }
		[DataMember]
		public string FileName { get; set; }
	}
}
