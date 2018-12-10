using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.JSpecView
{
	public class JcampReadException : Exception
	{
		public JcampReadException()
		{
		}

		public JcampReadException(string message)
			: base(message)
		{
		}
	}
}
