using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of entity property types.
	/// </summary>
	public enum EditablePropertyType
	{
		/// <summary>
		/// Boolean property.
		/// </summary>
		Bool,
		/// <summary>
		/// 32-bit integer property.
		/// </summary>
		Int,
		/// <summary>
		/// Single-precision floating point number property.
		/// </summary>
		Float,
		/// <summary>
		/// 3D vector property.
		/// </summary>
		Vector,
		/// <summary>
		/// Text property.
		/// </summary>
		String,
		/// <summary>
		/// <see cref="EntityId"/> property.
		/// </summary>
		Entity,
		/// <summary>
		/// Property that starts a property folder.
		/// </summary>
		FolderBegin,
		/// <summary>
		/// Property that ends a property folder.
		/// </summary>
		FolderEnd
	}
}