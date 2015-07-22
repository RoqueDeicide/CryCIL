using System;
using System.Runtime.Serialization;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents an exception that is thrown when an error happens during sending/receiving of RMI calls.
	/// </summary>
	[Serializable]
	public class RmiException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="RmiException"/> class.
		/// </summary>
		public RmiException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public RmiException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified message and exception
		/// object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public RmiException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RmiException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected RmiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}