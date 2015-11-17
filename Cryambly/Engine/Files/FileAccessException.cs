using System;
using System.Runtime.Serialization;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Represents an exception that is thrown when attempt to access a file is failed.
	/// </summary>
	[Serializable]
	public class FileAccessException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="FileAccessException"/> class.
		/// </summary>
		public FileAccessException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="FileAccessException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public FileAccessException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="FileAccessException"/> class with specified message and
		/// exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public FileAccessException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="FileAccessException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <exception cref="ArgumentNullException">
		/// The <paramref name="info"/> parameter is null.
		/// </exception>
		/// <exception cref="SerializationException">
		/// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
		/// </exception>
		protected FileAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}