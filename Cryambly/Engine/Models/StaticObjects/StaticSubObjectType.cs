using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of types of static sub-objects.
	/// </summary>
	public enum StaticSubObjectType
	{
		/// <summary>
		/// Indicates that the sub-object is a simple triangular mesh.
		/// </summary>
		Mesh,
		/// <summary>
		/// Indicates that the sub-object is a helper triangular mesh, used for broken pieces.
		/// </summary>
		HelperMesh,
		/// <summary>
		/// Indicates that the sub-object is a point(?).
		/// </summary>
		Point,
		/// <summary>
		/// Indicates that the sub-object is a dummy(?).
		/// </summary>
		Dummy,
		/// <summary>
		/// Indicates that the sub-object is a something(?).
		/// </summary>
		Xref,
		/// <summary>
		/// Indicates that the sub-object is a camera(?).
		/// </summary>
		Camera,
		/// <summary>
		/// Indicates that the sub-object is a light source(?).
		/// </summary>
		Light
	}
}