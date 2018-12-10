using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Sds.Core
{
    public static class HashUtils
    {
        private static MD5 _md5 = MD5.Create();

        public static byte[] GetMD5Hash(this string str)
        {
            lock ( _md5 ) {
                return _md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            }
        }

        public static string GetMD5String(this string str)
        {
            byte[] hash = GetMD5Hash(str);
            return BytesToX2String(hash);
        }

        public static string BytesToX2String(byte[] hash)
        {
            StringBuilder sb = new StringBuilder();
            for ( int i = 0; i < hash.Length; i++ )
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        public static byte[] GetMD5Hash(this byte[] buf)
        {
            lock ( _md5 ) {
                return _md5.ComputeHash(buf);
            }
        }

        public static string GetMD5String(this byte[] buf)
        {
            byte[] hash = GetMD5Hash(buf);
            return BytesToX2String(hash);
        }

        public static string Hex2String(byte[] data)
        {
            return String.Concat(Array.ConvertAll(data, x => x.ToString("X2")));
        }

        public static byte[] String2Hex(string data)
        {
            return Enumerable.Range(0, data.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                             .ToArray();
        }
		//public static int HashIntegers(params int[] args)
		//{
		//	int sum = 0;
		//	args.ForAll(i => sum ^= i);
		//	return sum;
		//}

        public static byte[] GetFileMD5Hash(string file)
        {
            lock ( _md5 ) {
                using ( var ifs = File.Open(file, FileMode.Open, FileAccess.Read) )
                    return _md5.ComputeHash(ifs);
            }
        }
    }
}
