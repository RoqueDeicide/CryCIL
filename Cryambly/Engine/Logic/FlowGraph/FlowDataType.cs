using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of types of data that can be transfered via flow node ports.
	/// </summary>
	public enum FlowDataType
	{
		/// <summary>
		/// Any supported type.
		/// </summary>
		/// <remarks>
		/// Used when data is being transfered, but its type doesn't matter. For instance in the flow node
		/// "All".
		/// </remarks>
		Any = -1,
		/// <summary>
		/// No data is transfered.
		/// </summary>
		Void = 0,
		/// <summary>
		/// Signed 32-bit integer data.
		/// </summary>
		Int,
		/// <summary>
		/// Single precision floating point number.
		/// </summary>
		Float,
		/// <summary>
		/// Identifier of the entity.
		/// </summary>
		EntityId,
		/// <summary>
		/// 3 dimensional vector.
		/// </summary>
		Vector3,
		/// <summary>
		/// Text information.
		/// </summary>
		String,
		/// <summary>
		/// Boolean value.
		/// </summary>
		Bool
	}
}