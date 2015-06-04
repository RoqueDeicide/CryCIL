using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CryCil
{
	/// <summary>
	/// Represents an exception that is thrown to data doesn't fit the buffer.
	/// </summary>
	[Serializable]
	public class BufferOverflowException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="BufferOverflowException"/> class.
		/// </summary>
		public BufferOverflowException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="BufferOverflowException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public BufferOverflowException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="BufferOverflowException"/> class with specified message
		/// and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public BufferOverflowException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="BufferOverflowException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected BufferOverflowException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}