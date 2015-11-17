using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that tricks the physical entity into thinking that it
	/// collided with something.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionRegisterCollisionEvent
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private Vector3 pt;
		[UsedImplicitly] private Vector3 n;
		[UsedImplicitly] private Vector3 v;
		[UsedImplicitly] private Vector3 vSelf;
		[UsedImplicitly] private float collMass;
		[UsedImplicitly] private PhysicalEntity pCollider;
		[UsedImplicitly] private int partid0;
		[UsedImplicitly] private int partid1;
		[UsedImplicitly] private int idmat0;
		[UsedImplicitly] private int idmat1;
		[UsedImplicitly] private short iPrim0;
		[UsedImplicitly] private short iPrim1;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the coordinates of the point of the collision.
		/// </summary>
		public Vector3 CollisionPoint
		{
			get { return this.pt; }
			set { this.pt = value; }
		}
		/// <summary>
		/// Gets or sets the direction of the normal to the plane of collision.
		/// </summary>
		public Vector3 CollisionPlaneNormal
		{
			get { return this.n; }
			set { this.n = value.Normalized; }
		}
		/// <summary>
		/// Gets or sets the vector that represents velocity of the collider entity.
		/// </summary>
		public Vector3 ColliderVelocity
		{
			get { return this.v; }
			set { this.v = value; }
		}
		/// <summary>
		/// Gets or sets the vector that represents velocity of the collidee entity.
		/// </summary>
		public Vector3 CollideeVelocity
		{
			get { return this.vSelf; }
			set { this.vSelf = value; }
		}
		/// <summary>
		/// Gets or sets the mass of the collider entity in kilograms.
		/// </summary>
		public float ColliderMass
		{
			get { return this.collMass; }
			set { this.collMass = value; }
		}
		/// <summary>
		/// Gets or sets the physical entity that is a collider.
		/// </summary>
		public PhysicalEntity Collider
		{
			get { return this.pCollider; }
			set { this.pCollider = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsActionRegisterCollisionEvent([UsedImplicitly] int notUsed)
			: this()
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.RegisterCollisionEvent);
			this.vSelf = UnusedValue.Vector;
		}
		#endregion
	}
}