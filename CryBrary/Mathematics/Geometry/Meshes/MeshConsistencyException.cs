using System;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents an exception that is thrown when program attempts to create or change a
	/// triangular mesh using inconsistent data.
	/// </summary>
	[Serializable]
	public class MeshConsistencyException : Exception
	{
		/// <summary>
		/// Creates a default instance of <see cref="MeshConsistencyException" /> class.
		/// </summary>
		public MeshConsistencyException()
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="MeshConsistencyException" /> class with specified message.
		/// </summary>
		/// <param name="message"> Message to supply with exception. </param>
		public MeshConsistencyException(string message)
			: base(message)
		{
		}
		/// <summary>
		/// Creates a new instance of <see cref="MeshConsistencyException" /> class with specified
		/// message and exception object that caused new one to be created.
		/// </summary>
		/// <param name="message"> Message to supply with exception. </param>
		/// <param name="inner">   Exception that caused a new one to be created. </param>
		public MeshConsistencyException(string message, Exception inner)
			: base(message, inner)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="MeshConsistencyException" /> class with
		/// serialized data.
		/// </summary>
		/// <param name="info">    The object that holds the serialized object data. </param>
		/// <param name="context"> The contextual information about the source or destination. </param>
		protected MeshConsistencyException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}