using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query the dynamic status of the living
	/// physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusDynamics
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private Vector3 v;
		[UsedImplicitly] private Vector3 w;
		[UsedImplicitly] private Vector3 a;
		[UsedImplicitly] private Vector3 wa;
		[UsedImplicitly] private Vector3 centerOfMass;
		[UsedImplicitly] private float submergedFraction;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private float energy;
		[UsedImplicitly] private int nContacts;
		[UsedImplicitly] private float time_interval;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current velocity of the part/entity.
		/// </summary>
		public Vector3 Velocity => this.v;
		/// <summary>
		/// Gets current angular velocity of the part/entity.
		/// </summary>
		public Vector3 AngularVelocity => this.w;
		/// <summary>
		/// Gets current acceleration of the part/entity.
		/// </summary>
		public Vector3 Acceleration => this.a;
		/// <summary>
		/// Gets current angular acceleration of the part/entity.
		/// </summary>
		public Vector3 AngularAcceleration => this.wa;
		/// <summary>
		/// Gets current position of the center of mass of the part/entity in world space (probably).
		/// </summary>
		public Vector3 CenterOfMass => this.centerOfMass;
		/// <summary>
		/// Gets the value between 0 and 1 that represents the fraction of the entity that is submerged
		/// underwater.
		/// </summary>
		/// <remarks>Not supported for individual parts.</remarks>
		public float SubmergedFraction => this.submergedFraction;
		/// <summary>
		/// Gets the current mass of the part/entity.
		/// </summary>
		public float Mass => this.mass;
		/// <summary>
		/// Gets the current kinetic energy of this entity.
		/// </summary>
		/// <remarks>Only supported by articulated bodies.</remarks>
		public float KineticEnergy => this.energy;
		/// <summary>
		/// Gets the current number of contacts this entity has with other entities.
		/// </summary>
		public int ContactCount => this.nContacts;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="part">An object that specifies the part of the entity to query.</param>
		public PhysicsStatusDynamics(EntityPartSpec part)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Dynamics);
			this.partid = UnusedValue.Int32;
			this.ipart = UnusedValue.Int32;

			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
		}
		#endregion
	}
}