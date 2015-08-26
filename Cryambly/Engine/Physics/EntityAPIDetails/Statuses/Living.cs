using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query the status of the living physical
	/// entity.
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
	public struct PhysicsStatusLiving
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int bFlying;
		[UsedImplicitly] private float timeFlying;
		[UsedImplicitly] private Vector3 camOffset;
		[UsedImplicitly] private Vector3 vel;
		[UsedImplicitly] private Vector3 velUnconstrained;
		[UsedImplicitly] private Vector3 velRequested;
		[UsedImplicitly] private Vector3 velGround;
		[UsedImplicitly] private float groundHeight;
		[UsedImplicitly] private Vector3 groundSlope;
		[UsedImplicitly] private int groundSurfaceIdx;
		[UsedImplicitly] private int groundSurfaceIdxAux;
		[UsedImplicitly] private PhysicalEntity pGroundCollider;
		[UsedImplicitly] private int iGroundColliderPart;
		[UsedImplicitly] private float timeSinceStanceChange;
		[UsedImplicitly] private int bStuck;
		[UsedImplicitly] private int bSquashed;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the value that indicates whether this entity doesn't have contact with the ground.
		/// </summary>
		public bool Flying
		{
			get { return this.bFlying != 0; }
		}
		/// <summary>
		/// Gets the value that represents how long the entity was flying.
		/// </summary>
		public float FlightTime
		{
			get { return this.timeFlying; }
		}
		/// <summary>
		/// Gets the Z-coordinate of the camera position in entity local space.
		/// </summary>
		/// <remarks>
		/// Camera location is influenced by shaking that happens when the entity lands and when its head
		/// 'collides' with anything.
		/// </remarks>
		public float EyeLevel
		{
			get { return this.camOffset.Z; }
		}
		/// <summary>
		/// Gets the actual velocity of the object (calculated from the time step time and difference
		/// between current and last positions).
		/// </summary>
		public Vector3 ActualVelocity
		{
			get { return this.vel; }
		}
		/// <summary>
		/// Gets the velocity the entity is supposed to have according to the simulation state.
		/// </summary>
		public Vector3 SimulatedVelocity
		{
			get { return this.velUnconstrained; }
		}
		/// <summary>
		/// Gets the velocity this entity was requested to have using <see cref="PhysicsActionMove"/>
		/// object.
		/// </summary>
		public Vector3 RequestedVelocity
		{
			get { return this.velRequested; }
		}
		/// <summary>
		/// Gets the velocity of the object this entity is standing on.
		/// </summary>
		public Vector3 GroundVelocity
		{
			get { return this.velGround; }
		}
		/// <summary>
		/// Gets the height of the point of last contact with the ground from 0 plane.
		/// </summary>
		public float GroundHeight
		{
			get { return this.groundHeight; }
		}
		/// <summary>
		/// Gets the normal vector to the ground at the point of last contact with the ground.
		/// </summary>
		public Vector3 GroundNormal
		{
			get { return this.groundSlope; }
		}
		/// <summary>
		/// Gets the object that represents the ground surface.
		/// </summary>
		public SurfaceType GroundSurfaceType
		{
			get { return SurfaceType.Get(this.groundSurfaceIdxAux > 0 ? this.groundSurfaceIdxAux : this.groundSurfaceIdx); }
		}
		/// <summary>
		/// Gets the entity this one stands on.
		/// </summary>
		public PhysicalEntity Ground
		{
			get { return this.pGroundCollider; }
		}
		/// <summary>
		/// Gets the zero-based index of the part of <see cref="Ground"/> entity this one stands on.
		/// </summary>
		public int GroundPartIndex
		{
			get { return this.iGroundColliderPart; }
		}
		/// <summary>
		/// Gets the value that may or may not allow you to determine whether the entity is stuck.
		/// </summary>
		public bool Stuck
		{
			get { return this.bStuck != 0; }
		}
		/// <summary>
		/// Gets the value that indicates whether this entity is in 'squashed' state: being pushed by other
		/// entities in opposite directions.
		/// </summary>
		public bool Squashed
		{
			get { return this.bSquashed != 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsStatusLiving([UsedImplicitly] int notUsed)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Living);
		}
		#endregion
	}
}