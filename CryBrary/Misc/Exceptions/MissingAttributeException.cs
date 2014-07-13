using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public class AttributeUsageException : Exception
	{
		public AttributeUsageException()
		{
		}

		public AttributeUsageException(string message)
			: base(message)
		{
		}

		public AttributeUsageException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}