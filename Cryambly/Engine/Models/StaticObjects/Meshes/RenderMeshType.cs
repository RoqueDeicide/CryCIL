using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of types of render meshes.
	/// </summary>
	public enum RenderMeshType
	{
		/// <summary>
		/// Render mesh is immutable (cannot be changed).
		/// </summary>
		Immmutable = 0,
		/// <summary>
		/// Render mesh is optimized for static (rarely changing) objects.
		/// </summary>
		Static = 1,
		/// <summary>
		/// Render mesh is optimized for dynamic (frequently changing) objects.
		/// </summary>
		Dynamic = 2,
		/// <summary>
		/// Render mesh is optimized for temporary usage.
		/// </summary>
		Transient = 3
	}
}