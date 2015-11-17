using System;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of culling modes that can be used by the renderer.
	/// </summary>
	public enum CullMode
	{
		/// <summary>
		/// Always draw all triangles.
		/// </summary>
		Disabled,
		/// <summary>
		/// Do not draw triangles that are front-facing.
		/// </summary>
		Front,
		/// <summary>
		/// Do not draw triangles that are back-facing.
		/// </summary>
		Back
	}
}