using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Represents an exception that is thrown when there is an attempt to use something that requires a
	/// certain attribute..
	/// </summary>
	[Serializable]
	public class AttributeUsageException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="AttributeUsageException"/> class.
		/// </summary>
		public AttributeUsageException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="AttributeUsageException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public AttributeUsageException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="AttributeUsageException"/> class with specified message
		/// and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public AttributeUsageException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeUsageException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected AttributeUsageException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
	/// <summary>
	/// Represents an exception that is thrown to cancel a certain operation.
	/// </summary>
	[SerializableAttribute]
	public class NullPointerException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="NullPointerException"/> class.
		/// </summary>
		public NullPointerException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="NullPointerException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public NullPointerException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="NullPointerException"/> class with specified message and
		/// exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public NullPointerException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="NullPointerException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected NullPointerException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}