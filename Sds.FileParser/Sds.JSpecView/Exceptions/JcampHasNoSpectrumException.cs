using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.JSpecView
{
	public class JcampHasNoSpectrumException : Exception
	{
		public JcampHasNoSpectrumException()
		{
		}

		public JcampHasNoSpectrumException(string message) : base(message)
		{
		}
	}
}
