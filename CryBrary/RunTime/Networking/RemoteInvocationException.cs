using System;

namespace CryEngine.RunTime.Networking
{
	/// <summary>
	/// Represents an exception that is thrown when error happens during remote
	/// invocation.
	/// </summary>
	/// <seealso cref="RemoteInvocationAttribute"></seealso>
	[Serializable]
	public class RemoteInvocationException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="RemoteInvocationException"/> class.
		/// </summary>
		public RemoteInvocationException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RemoteInvocationException"/> class with
		/// specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public RemoteInvocationException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="RemoteInvocationException"/> class with
		/// specified message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public RemoteInvocationException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteInvocationException"/>
		/// class with serialized data.
		/// </summary>
		/// <param name="info">   
		/// The object that holds the serialized object data.
		/// </param>
		/// <param name="context">
		/// The contextual information about the source or destination.
		/// </param>
		protected RemoteInvocationException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}