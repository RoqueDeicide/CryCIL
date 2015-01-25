using System;
using System.Runtime.Serialization;

namespace CryEngine.RunTime.Registration
{
	/// <summary>
	/// Represents an exception that is thrown when registration of the CryEngine-related
	/// type fails.
	/// </summary>
	[Serializable]
	public class RegistrationException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="RegistrationException"/> class.
		/// </summary>
		public RegistrationException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RegistrationException"/> class with
		/// specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public RegistrationException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RegistrationException"/> class with
		/// specified message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public RegistrationException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationException"/> class
		/// with serialized data.
		/// </summary>
		/// <param name="info">   
		/// The object that holds the serialized object data.
		/// </param>
		/// <param name="context">
		/// The contextual information about the source or destination.
		/// </param>
		protected RegistrationException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}