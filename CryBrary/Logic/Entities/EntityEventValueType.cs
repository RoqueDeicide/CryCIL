using System;
using CryEngine.Mathematics;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Enumeration of values of the entity event
	/// </summary>
	public enum EntityEventValueType
	{
		/// <summary>
		/// <see cref="Int32"/> value.
		/// </summary>
		Int,
		/// <summary>
		/// <see cref="Single"/> value.
		/// </summary>
		Float,
		/// <summary>
		/// <see cref="Boolean"/> value.
		/// </summary>
		Bool,
		/// <summary>
		/// <see cref="Vector3"/> value.
		/// </summary>
		Vector,
		/// <summary>
		/// <see cref="Entity"/> value.
		/// </summary>
		Entity,
		/// <summary>
		/// <see cref="String"/> value.
		/// </summary>
		String
	}
}