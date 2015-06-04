using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that describe the surface type.
	/// </summary>
	[Flags]
	public enum SurfaceTypeFlags
	{
		/// <summary>
		/// When set, specifies that this surface should not be physicalized.
		/// </summary>
		NoPhysicalize = 1,
		/// <summary>
		/// When set, specifies that this surface should not collide with anything.
		/// </summary>
		/// <remarks>Normally used for surfaces that represents vegetation canopies.</remarks>
		NoCollide = 2,
		/// <summary>
		/// When set, specifies that this surface should only collide with vehicles.
		/// </summary>
		VehicleOnlyCollision = 4,
		/// <summary>
		/// When set, specifies that an object with this surface can shatter.
		/// </summary>
		CanShatter = 8,
		/// <summary>
		/// When set, specifies that an object with this surface can be pierced by objects.
		/// </summary>
		/// <remarks>Used by Material Effects system to spawn front/back effects.</remarks>
		BulletPierceable = 16
	}
}