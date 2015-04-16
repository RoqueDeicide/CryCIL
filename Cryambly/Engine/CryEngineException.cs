using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents an exception that is thrown when an error occurs within CryEngine.
	/// </summary>
	[Serializable]
	public class CryEngineException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="CryEngineException"/> class.
		/// </summary>
		public CryEngineException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CryEngineException"/> class with specified message.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		public CryEngineException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="CryEngineException"/> class with specified message and
		/// exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">Message to supply with exception.</param>
		/// <param name="inner">  Exception that caused a new one to be created.</param>
		public CryEngineException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CryEngineException"/> class with serialized data.
		/// </summary>
		/// <param name="info">   The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected CryEngineException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context)
		{
		}
	}
}