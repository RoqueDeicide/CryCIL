using System;
using System.Runtime.Serialization;

namespace CryEngine.Testing
{
	/// <summary>
	/// The exception that is thrown when a false assertion is made.
	/// </summary>
	public sealed class AssertionFailedException : Exception
	{
		public AssertionFailedException()
		{
		}

		public AssertionFailedException(string message)
			: base(message)
		{
		}

		public AssertionFailedException(string format, params object[] args)
			: this(string.Format(format, args))
		{
		}

		public AssertionFailedException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}