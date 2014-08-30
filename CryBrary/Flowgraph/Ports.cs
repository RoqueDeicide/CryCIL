using System;
using System.Linq;

using CryEngine.Native;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Enumeration of types of input and output ports.
	/// </summary>
	public enum NodePortType
	{
		/// <summary>
		/// Anything can be associated with this port.
		/// </summary>
		Any = -1,
		/// <summary>
		/// The port doesn't work with values.
		/// </summary>
		/// <remarks>
		/// Used for ports, used exclusively for activation of other nodes, without passing data.
		/// </remarks>
		Void,
		/// <summary>
		/// The port uses <see cref="Int32"/> value.
		/// </summary>
		Int,
		/// <summary>
		/// The port uses <see cref="Single"/> value.
		/// </summary>
		Float,
		/// <summary>
		/// The port uses <see cref="EntityId"/> value.
		/// </summary>
		EntityId,
		/// <summary>
		/// The port uses <see cref="Vector3"/> value.
		/// </summary>
		Vector3,
		/// <summary>
		/// The port uses <see cref="String"/> value.
		/// </summary>
		String,
		/// <summary>
		/// The port uses <see cref="Boolean"/> value.
		/// </summary>
		Bool
	}
}