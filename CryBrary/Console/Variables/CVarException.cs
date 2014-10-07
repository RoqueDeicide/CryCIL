using System;
using System.Runtime.Serialization;

namespace CryEngine.Console.Variables
{
	/// <summary>
	/// Represents an exception that is thrown when an error occurs during console
	/// variable-related operation.
	/// </summary>
	[Serializable]
	public class CVarException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="CVarException"/> class.
		/// </summary>
		public CVarException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CVarException"/> class with specified
		/// message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public CVarException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CVarException"/> class with specified
		/// message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public CVarException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CVarException"/> class with
		/// serialized data.
		/// </summary>
		/// <param name="info">   
		/// The object that holds the serialized object data.
		/// </param>
		/// <param name="context">
		/// The contextual information about the source or destination.
		/// </param>
		protected CVarException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}