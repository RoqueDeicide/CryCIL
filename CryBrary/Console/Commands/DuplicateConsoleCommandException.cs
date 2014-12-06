using System;

namespace CryEngine.Console.Commands
{
	/// <summary>
	/// Represents an exception that is thrown to cancel a certain operation.
	/// </summary>
	[Serializable]
	public class DuplicateConsoleCommandException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="DuplicateConsoleCommandException"/>
		/// class.
		/// </summary>
		public DuplicateConsoleCommandException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="DuplicateConsoleCommandException"/> class
		/// with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public DuplicateConsoleCommandException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="DuplicateConsoleCommandException"/> class
		/// with specified message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public DuplicateConsoleCommandException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="DuplicateConsoleCommandException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   
		/// The object that holds the serialized object data.
		/// </param>
		/// <param name="context">
		/// The contextual information about the source or destination.
		/// </param>
		protected DuplicateConsoleCommandException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}