using System;
using System.Runtime.Serialization;

namespace CryEngine.Console.Variables
{
	/// <summary>
	/// Represents an exception that is thrown when an error occurs during console variable-related
	/// operation.
	/// </summary>
	[Serializable]
	public class ConsoleVariableException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="ConsoleVariableException"/> class.
		/// </summary>
		public ConsoleVariableException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="ConsoleVariableException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public ConsoleVariableException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="ConsoleVariableException"/> class with specified message
		/// and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public ConsoleVariableException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleVariableException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected ConsoleVariableException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}