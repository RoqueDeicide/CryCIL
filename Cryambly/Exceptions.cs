using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CryCil
{
	/// <summary>
	/// Represents an exception that is thrown when data doesn't fit the buffer.
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
	/// <summary>
	/// Represents an exception that is thrown when attempt is made to use a metadata object that is
	/// supposed to have some sort of an attribute but it doesn't have it.
	/// </summary>
	[Serializable]
	public class MissingAttributeException : Exception
	{
		/// <summary>
		/// Gets the reflection object that represents the attribute that was missing.
		/// </summary>
		public Type AttributeType { get; private set; }
		/// <summary>
		/// Creates a default instance of <see cref="MissingAttributeException"/> class.
		/// </summary>
		public MissingAttributeException()
		{
			this.AttributeType = typeof(Attribute);
		}
		/// <summary>
		/// Creates a new instance of <see cref="MissingAttributeException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public MissingAttributeException(string message)
			: base(message)
		{
			this.AttributeType = typeof(Attribute);
		}
		/// <summary>
		/// Creates a new instance of <see cref="MissingAttributeException"/> class with specified message
		/// and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public MissingAttributeException(string message, Exception inner)
			: base(message, inner)
		{
			this.AttributeType = typeof(Attribute);
		}
		/// <summary>
		/// Creates an instance of <see cref="MissingAttributeException"/> class that provides information
		/// about an attribute that must be present.
		/// </summary>
		/// <param name="attribute">
		/// A reflection object that represents the type attribute that is missing.
		/// </param>
		public MissingAttributeException(Type attribute)
		{
			this.AttributeType = attribute;
		}
		/// <summary>
		/// Creates a new instance of <see cref="MissingAttributeException"/> class with specified message
		/// and information about an attribute that must be present.
		/// </summary>
		/// <param name="attribute">
		/// A reflection object that represents the type attribute that is missing.
		/// </param>
		/// <param name="message">  Message to supply with exception.</param>
		public MissingAttributeException(Type attribute, string message)
			: base(message)
		{
			this.AttributeType = attribute;
		}
		/// <summary>
		/// Creates a new instance of <see cref="MissingAttributeException"/> class with specified message,
		/// exception object that caused new one to be created and information about an attribute that must
		/// be present.
		/// </summary>
		/// <param name="attribute">
		/// A reflection object that represents the type attribute that is missing.
		/// </param>
		/// <param name="message">  Message to supply with exception.</param>
		/// <param name="inner">    Exception that caused a new one to be created.</param>
		public MissingAttributeException(Type attribute, string message, Exception inner)
			: base(message, inner)
		{
			this.AttributeType = attribute;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MissingAttributeException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected MissingAttributeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
	/// <summary>
	/// Represents an exception that is thrown when an error occurs during registration of one of the
	/// elements defined using CryCIL.
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
		/// Creates a new instance of <see cref="RegistrationException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public RegistrationException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RegistrationException"/> class with specified message and
		/// exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public RegistrationException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RegistrationException"/> class with serialized
		/// data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected RegistrationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}