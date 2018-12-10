using System.IO;

namespace Sds.Core
{
	public static class StreamExtensions
	{
		public static byte[] ReadAll(this Stream input)
		{
			byte[] buffer = new byte[16 * 1024];

			using (MemoryStream outStream = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					outStream.Write(buffer, 0, read);
				}
				return outStream.ToArray();
			}
		}
	}
}
