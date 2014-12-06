using System;
using System.Runtime.Serialization;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Represents an exception that is thrown during erroneous action in flow graph.
	/// </summary>
	[Serializable]
	public class FlowGraphException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="FlowGraphException" /> class.
		/// </summary>
		public FlowGraphException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="FlowGraphException" /> class with specified message.
		/// </summary>
		/// <param name="message">
		/// Message to supply with exception.
		/// </param>
		public FlowGraphException(string message) : base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="FlowGraphException" /> class with specified
		/// message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message">
		/// Message to supply with exception.
		/// </param>
		/// <param name="inner">
		/// Exception that caused a new one to be created.
		/// </param>
		public FlowGraphException(string message, Exception inner) : base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="FlowGraphException" /> class with serialized data.
		/// </summary>
		/// <param name="info">
		/// The object that holds the serialized object data.
		/// </param>
		/// <param name="context">
		/// The contextual information about the source or destination.
		/// </param>
		protected FlowGraphException(
			SerializationInfo info,
			StreamingContext context) : base(info, context)
		{
		}
	}
}