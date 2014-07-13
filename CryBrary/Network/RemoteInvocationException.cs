using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Thrown when an error related to a remote invocation (see <see
	/// cref="CryEngine.RemoteInvocationAttribute" />) is caught.
	/// </summary>
	public class RemoteInvocationException : Exception
	{
		public RemoteInvocationException()
		{
		}

		public RemoteInvocationException(string message)
			: base(message)
		{
		}

		public RemoteInvocationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}