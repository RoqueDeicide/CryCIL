using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Physics;

namespace CryEngine
{
	[CLSCompliant(false)]
	public struct PhysicalizationParams
	{
		public PhysicalizationParams(PhysicalizationType _type)
			: this()
		{
			type = _type;

			flagsAND = (PhysicalizationFlags)int.MaxValue;
			slot = -1;
			density = -1;
			mass = -1;
			attachToPart = -1;

			switch (type)
			{
				case PhysicalizationType.Living:
					{
						livingDynamics = PlayerDynamicsParameters.Create();
						livingDimensions = PlayerDimensionsParameters.Create();
					}
					break;
				case PhysicalizationType.Particle:
					particleParameters = ParticleParameters.Create();
					break;
			}
		}

		internal PhysicalizationType type;

		public PhysicalizationFlags flagsOR;
		public PhysicalizationFlags flagsAND;

		/// <summary>
		/// Index of object slot, -1 if all slots should be used.
		/// </summary>
		public int slot;

		/// <summary>
		/// Only one either density or mass must be set, parameter set to 0 is ignored.
		/// </summary>
		public float density;
		public float mass;

		/// <summary>
		/// When physicalizing geometry can specify to use physics from different LOD. Used for
		/// characters that have ragdoll physics in Lod1
		/// </summary>
		public int lod;

		/// <summary>
		/// Physical entity to attach this physics object (Only for Soft physical entity).
		/// </summary>
		internal uint attachToEntity;

		/// <summary>
		/// Part ID in entity to attach to (Only for Soft physical entity).
		/// </summary>
		public int attachToPart;

		/// <summary>
		/// Used for character physicalization (Scale of force in character joint's springs).
		/// </summary>
		public float stiffnessScale;

		/// <summary>
		/// Copy joints velocities when converting a character to ragdoll.
		/// </summary>
		public bool copyJointVelocities;

		/// <summary>
		/// Player dimensions, only applicable when type is set to <see
		/// cref="PhysicalizationType.Living" />.
		/// </summary>
		public PlayerDimensionsParameters livingDimensions;
		/// <summary>
		/// Player dynamics, only applicable when type is set to <see
		/// cref="PhysicalizationType.Living" />.
		/// </summary>
		public PlayerDynamicsParameters livingDynamics;

		/// <summary>
		/// Particle parameters, only applicable when type is set to <see
		/// cref="PhysicalizationType.Particle" />.
		/// </summary>
		public ParticleParameters particleParameters;
	}
}