using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public class NullPointerException : Exception
	{
		public NullPointerException()
		{
		}

		public NullPointerException(string message)
			: base(message)
		{
		}

		public NullPointerException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}