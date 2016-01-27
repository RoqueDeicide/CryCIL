using System;
using System.Linq;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Enumeration of types of the area that can be bound to the entity via
	/// <see cref="CryEntityAreaProxy"/>.
	/// </summary>
	public enum EntityAreaType
	{
		/// <summary>
		/// Area is defined as a closed set of points forming a shape.
		/// </summary>
		Shape,
		/// <summary>
		/// Area is defined as an oriented bounding box.
		/// </summary>
		Box,
		/// <summary>
		/// Area is defined as a sphere.
		/// </summary>
		Sphere,
		/// <summary>
		/// Area is defined as a volume around a bezier curve.
		/// </summary>
		Bezier,
		/// <summary>
		/// Area is defined as a solid which can be any geometric figure.
		/// </summary>
		Solid
	}
}