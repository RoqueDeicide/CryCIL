using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CryCil.Engine.DebugServices;

namespace MainTestingAssembly
{
	/// <summary>
	/// Represents an exception that is thrown for testing purposes.
	/// </summary>
	[Serializable]
	public class CryCilTestException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="CryCilTestException"/> class.
		/// </summary>
		public CryCilTestException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CryCilTestException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public CryCilTestException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CryCilTestException"/> class with specified message and
		/// exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public CryCilTestException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CryCilTestException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected CryCilTestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
	static class ExceptionTestingMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ThrowExceptionInternal(Exception ex);
	}
	/// <summary>
	/// Defines static methods that are used by underlying framework to test IMonoExceptions
	/// implementation.
	/// </summary>
	public static class ExceptionTesting
	{
		/// <summary>
		/// Creates and throws a test exception.
		/// </summary>
		/// <param name="message">Message to pass to exception object's constructor.</param>
		public static void MakeAndThrowException(string message)
		{
			throw new CryCilTestException(message);
		}
		/// <summary>
		/// Creates a simple exception with inner one.
		/// </summary>
		/// <param name="inner">An exception object to use as inner one.</param>
		/// <returns>A new exception with given one used as inner.</returns>
		public static Exception GetExceptionWithInnerOne(Exception inner)
		{
			return new Exception("", inner);
		}
		/// <summary>
		/// Tests throwing of exceptions by underlying framework.
		/// </summary>
		public static void TestUnderlyingExceptionThrowing()
		{
			try
			{
				ExceptionTestingMethods.ThrowExceptionInternal(new CryCilTestException("Thrown by CryCIL API."));

				Log.Error("TEST FAILURE: The exception was not raised by CryCIL API.", true);
			}
			catch (Exception)
			{
				Console.WriteLine("TEST SUCCESS: Successfully caught the exception that was raised by CryCIL API.");
			}
		}
	}
}