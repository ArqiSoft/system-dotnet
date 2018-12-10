using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sds.JSpecView
{
	public class JcampHasTooManySpectraException : Exception
	{
		public JcampHasTooManySpectraException(string message) : base(message)
		{
		}
	}
}
