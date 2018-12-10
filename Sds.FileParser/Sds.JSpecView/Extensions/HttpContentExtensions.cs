using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace Sds.JSpecView
{
	public static class HttpContentExtensions
	{
		public static async Task<JSVSpectrum> ReadJcampAsync(this HttpContent file)
		{
			var jcamp = await file.ReadAsStringAsync();

			var spectrum = JcampReader.Read(jcamp);

			//spectrum.FileName = file.Headers.ContentDisposition.FileName.Trim('\"');

			return spectrum;
		}
	}
}
