using System;
using System.Runtime.Serialization;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of errors that can cause <see cref="RmiException"/> to be thrown.
	/// </summary>
	public enum RmiError
	{
		/// <summary>
		/// This value is set when creating an instance of type <see cref="RmiException"/> without
		/// specifying the error.
		/// </summary>
		Unspecified = -1,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI to server from the server.
		/// </summary>
		IsNotClient,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI to anywhere other then server
		/// when method is supposed to be directed to the server.
		/// </summary>
		NotDirectedToServer,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI with
		/// <see cref="RmiTarget.NoCall"/> flag set for the target.
		/// </summary>
		NoAllowedCalls,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI to specific client without
		/// specifying which one.
		/// </summary>
		ClientNotSpecified,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI to specific client and own
		/// client at the same time.
		/// </summary>
		SendingToClientAndItself,
		/// <summary>
		/// RMI error of this type is thrown when user attempts to call RMI directed to own client on
		/// object that doesn't have own client.
		/// </summary>
		SendingToItselfWithoutOwnClient
	}
	/// <summary>
	/// Represents an exception that is thrown when an error happens during sending/receiving of RMI calls.
	/// </summary>
	[Serializable]
	public class RmiException : Exception
	{
		/// <summary>
		/// Gets the error that caused this exception.
		/// </summary>
		public RmiError Error { get; private set; }
		/// <summary>
		/// Creates a default instance of <see cref="RmiException"/> class.
		/// </summary>
		public RmiException()
		{
			this.Error = RmiError.Unspecified;
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public RmiException(string message)
			: base(message)
		{
			this.Error = RmiError.Unspecified;
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
			this.Error = RmiError.Unspecified;
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified RMI error.
		/// </summary>
		/// <param name="error">The error that caused this exception.</param>
		public RmiException(RmiError error)
		{
			this.Error = error;
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified message and specified
		/// RMI error.
		/// </summary>
		/// <param name="error">  The error that caused this exception.</param>
		/// <param name="message">Message to supply with exception.</param>
		public RmiException(RmiError error, string message)
			: base(message)
		{
			this.Error = error;
		}
		/// <summary>
		/// Creates a new instance of <see cref="RmiException"/> class with specified message, exception
		/// object that caused new one to be created and specified RMI error.
		/// </summary>
		/// <param name="error">  The error that caused this exception.</param>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public RmiException(RmiError error, string message, Exception inner)
			: base(message, inner)
		{
			this.Error = error;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="RmiException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <exception cref="ArgumentNullException">
		/// The <paramref name="info"/> parameter is null.
		/// </exception>
		/// <exception cref="SerializationException">
		/// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
		/// </exception>
		protected RmiException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}