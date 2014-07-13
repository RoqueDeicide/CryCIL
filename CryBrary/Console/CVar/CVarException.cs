using System;
using System.Runtime.Serialization;

namespace CryEngine
{
	/// <summary>
	/// This exception is called when invalid CVar operations are performed.
	/// </summary>
	[Serializable]
	public class CVarException : Exception
	{
		public CVarException()
		{
		}

		public CVarException(string message)
			: base(message)
		{
		}

		public CVarException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected CVarException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}