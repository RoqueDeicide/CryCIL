using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public class DuplicateConsoleCommandException : Exception
	{
		public DuplicateConsoleCommandException()
		{
		}

		public DuplicateConsoleCommandException(string message)
			: base(message)
		{
		}

		public DuplicateConsoleCommandException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}