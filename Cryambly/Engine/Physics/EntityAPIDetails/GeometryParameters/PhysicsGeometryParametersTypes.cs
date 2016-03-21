using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of structures that encapsulate parameters that are used to update geometry of
	/// the physical entity.
	/// </summary>
	public enum PhysicsGeometryParametersTypes
	{
		/// <summary>
		/// Identifier of type of structure that encapsulates parameters of that are used update geometry on
		/// general types of physical entities.
		/// </summary>
		General = 0,
		/// <summary>
		/// Identifier of type of structure that encapsulates parameters of that are used update geometry on
		/// vehicles.
		/// </summary>
		Vehicle = 1,
		/// <summary>
		/// Identifier of type of structure that encapsulates parameters of that are used update geometry on
		/// articulated bodies.
		/// </summary>
		ArticulatedBody = 2,
		/// <summary>
		/// Number of identifiers.
		/// </summary>
		Count
	}
}