using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of structures that are used to query various information about the physical
	/// entity.
	/// </summary>
	public enum PhysicsStatusTypes
	{
		/// <summary>
		/// Identifier of the structure that is used to query position of the physical entity.
		/// </summary>
		Location = 1,
		/// <summary>
		/// Identifier of the structure that is used to query status of the living physical entity.
		/// </summary>
		Living = 2,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that is a
		/// vehicle.
		/// </summary>
		Vehicle = 4,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that is a
		/// wheel.
		/// </summary>
		Wheel = 5,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that is a
		/// joint.
		/// </summary>
		Joint = 6,
		/// <summary>
		/// Identifier of the structure that is used to query awakeness of the physical entity.
		/// </summary>
		Awake = 7,
		/// <summary>
		/// Identifier of the structure that is used to query status of dynamics of the living physical
		/// entity.
		/// </summary>
		Dynamics = 8,
		/// <summary>
		/// Deprecated.
		/// </summary>
		[Obsolete]
		Collisions = 9,
		/// <summary>
		/// Identifier of the structure that is used to query surface identifier of the physical entity.
		/// </summary>
		SurfaceId = 10,
		/// <summary>
		/// Deprecated.
		/// </summary>
		[Obsolete]
		TimeSlices = 11,
		/// <summary>
		/// Identifier of the structure that is used to query number of parts the physical entity currently
		/// consists of.
		/// </summary>
		PartCount = 12,
		/// <summary>
		/// Identifier of the structure that is used to check whether physical entity contains the point.
		/// </summary>
		ContainsPoint = 13,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that is a rope.
		/// </summary>
		Rope = 14,
		/// <summary>
		/// Identifier of the structure that is used to query current abilities of the physical entity that
		/// is a vehicle.
		/// </summary>
		VehicleAbilities = 15,
		/// <summary>
		/// Identifier of the structure that is used to check whether this physical entity is a
		/// place-holder.
		/// </summary>
		PlaceHolder = 16,
		/// <summary>
		/// Identifier of the structure that is used to query positions vertexes of the physical entity
		/// that is a soft body.
		/// </summary>
		SoftBodyVertices = 17,
		/// <summary>
		/// Identifier of the structure that is used to query status of sensors that are attached to the
		/// physical entity.
		/// </summary>
		Sensors = 18,
		/// <summary>
		/// Identifier of the structure that is used to check whether a point that is projected along a
		/// specific direction will fall onto convex hull of the physical entity.
		/// </summary>
		SampleContactArea = 19,
		/// <summary>
		/// Identifier of the structure that is used to check whether physical entity can change the
		/// orientation that was explicitly set from the outside (by baking into parts/geometries).
		/// </summary>
		CanAlterSetOrientation = 20,
		/// <summary>
		/// Identifier of the structure that is used to check whether new position of the physical entity
		/// will cause collisions.
		/// </summary>
		CheckStance = 21,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that simulates
		/// water.
		/// </summary>
		Water = 22,
		/// <summary>
		/// Identifier of the structure that is used to query status of the physical entity that is an
		/// area.
		/// </summary>
		Area = 23,
		/// <summary>
		/// Identifier of the structure that is used to query extent(?) of the physical entity.
		/// </summary>
		Extent = 24,
		/// <summary>
		/// Identifier of the structure that is used to get random position on the physical entity.
		/// </summary>
		Random = 25,
		/// <summary>
		/// Identifier of the structure that is used to query status of the constraint this physical entity
		/// has attached to it.
		/// </summary>
		Constraint = 26,
		/// <summary>
		/// Identifier of the structure that is used to query position of the physical entity in the
		/// network.
		/// </summary>
		NetworkLocation = 27,
		/// <summary>
		/// Number of identifiers.
		/// </summary>
		Count
	}
}