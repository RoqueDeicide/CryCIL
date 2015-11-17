using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of physics parameter structures.
	/// </summary>
	public enum PhysicsParametersTypes
	{
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change position of the physical
		/// entity.
		/// </summary>
		Position = 0,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change dimensions of the living
		/// physical entity.
		/// </summary>
		PlayerDimensions = 1,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the vehicle.
		/// </summary>
		Vehicle = 2,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change particle-related
		/// parameters of the physical entity.
		/// </summary>
		Particle = 3,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change dynamics of the living
		/// physical entity.
		/// </summary>
		PlayerDynamics = 4,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the joint.
		/// </summary>
		Joint = 5,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the part of
		/// the articulated entity.
		/// </summary>
		Part = 6,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to attach sensors to the living
		/// physical entity.
		/// </summary>
		Sensors = 7,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the
		/// articulated body.
		/// </summary>
		ArticulatedBody = 8,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change the outer entity of the
		/// physical entity.
		/// </summary>
		OuterEntity = 9,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change simulation parameters of
		/// the living physical entity.
		/// </summary>
		Simulation = 10,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change foreign data pointers for
		/// the physical entity.
		/// </summary>
		ForeignData = 11,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change buoyancy of the physical
		/// entity.
		/// </summary>
		Buoyancy = 12,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the rope.
		/// </summary>
		Rope = 13,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change the bounding box of the
		/// physical entity.
		/// </summary>
		BoundingBox = 14,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change a set of flags that
		/// specifies the physical entity.
		/// </summary>
		Flags = 15,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change the parameters of the
		/// wheel.
		/// </summary>
		Wheel = 16,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the soft
		/// body.
		/// </summary>
		SoftBody = 17,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the area.
		/// </summary>
		Area = 18,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change the shape of the
		/// tetra-lattice.
		/// </summary>
		TetraLattice = 19,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change position of the ground
		/// plane for the physical entity.
		/// </summary>
		GroundPlane = 20,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change parameters of the
		/// structural joint.
		/// </summary>
		StructuralJoint = 21,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change water simulation
		/// parameters.
		/// </summary>
		WaterMananger = 22,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change timeout parameters for the
		/// physical entity.
		/// </summary>
		Timeout = 23,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change the configuration of the
		/// skeleton of the physical entity.
		/// </summary>
		Skeleton = 24,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change initial velocity of the
		/// structure.
		/// </summary>
		StructuralInitialVelocity = 25,
		/// <summary>
		/// Identifier of the type of physics parameters that are used to change collision class of the
		/// physical entity.
		/// </summary>
		CollisionClass = 26,
		/// <summary>
		/// Total number of type of parameter structures.
		/// </summary>
		Count
	}
}