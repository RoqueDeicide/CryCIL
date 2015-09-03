using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about a part that was created by having it break off the entity.
	/// </summary>
	public unsafe struct CreatedPartInfo
	{
		#region Fields
		private readonly PhysicalEntity newEntity;
		private readonly int oldPartId;
		private readonly int newPartId;
		private readonly int totalPartCount;
		private readonly bool invalid;
		private readonly PhysicsPartCreationReason reason;
		private readonly Vector3 breakageImpulse;
		private readonly EulerAngles breakageAngularImpulse;
		private readonly Vector3 ejectionVelocity;
		private readonly EulerAngles ejectionAngularVelocity;
		private readonly float breakageSize;
		private readonly float cutRadius;
		private readonly Vector3 cutSourcePosition;
		private readonly Vector3 cutPartPosition;
		private readonly Vector3 cutSourceNormal;
		private readonly Vector3 cutPartNormal;
		private readonly GeometryShape newMesh;
		private readonly MeshUpdate* lastUpdate;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the entity that was created to house the part that was broken off.
		/// </summary>
		public PhysicalEntity NewPartEntity
		{
			get { return this.newEntity; }
		}
		/// <summary>
		/// Gets the identifier that was used by the part when it was within original entity.
		/// </summary>
		public int OldPartId
		{
			get { return this.oldPartId; }
		}
		/// <summary>
		/// Gets the identifier that is used by the part within new entity.
		/// </summary>
		public int NewPartId
		{
			get { return this.newPartId; }
		}
		/// <summary>
		/// Gets the total number of parts that were broken off the entity during the update, for each part
		/// there is a separate event.
		/// </summary>
		public int TotalPartCount
		{
			get { return this.totalPartCount; }
		}
		/// <summary>
		/// Gets the value that indicates whether the new mesh that is used by the part is invalid
		/// (degenerated, flipped).
		/// </summary>
		public bool IsInvalidMesh
		{
			get { return this.invalid; }
		}
		/// <summary>
		/// Gets the value that indicates why a new part was created.
		/// </summary>
		public PhysicsPartCreationReason Reason
		{
			get { return this.reason; }
		}
		/// <summary>
		/// Gets the vector that represents the impulse that caused the breakage.
		/// </summary>
		public Vector3 BreakageImpulse
		{
			get { return this.breakageImpulse; }
		}
		/// <summary>
		/// Gets the object that represents the angular impulse that caused the breakage.
		/// </summary>
		public EulerAngles BreakageAngularImpulse
		{
			get { return this.breakageAngularImpulse; }
		}
		/// <summary>
		/// Gets the velocity that was assigned to the new entity upon creation.
		/// </summary>
		public Vector3 EjectionVelocity
		{
			get { return this.ejectionVelocity; }
		}
		/// <summary>
		/// Gets the angular velocity that was assigned to the new entity upon creation.
		/// </summary>
		public EulerAngles EjectionAngularVelocity
		{
			get { return this.ejectionAngularVelocity; }
		}
		/// <summary>
		/// Gets the size of the explosion if the breakage was caused by explosion.
		/// </summary>
		public float BreakageSize
		{
			get { return this.breakageSize; }
		}
		/// <summary>
		/// Gets the value that represents the radius of a cross section of a capsule, if a mesh of the new
		/// part was successfully approximated with capsules.
		/// </summary>
		public float CutRadius
		{
			get { return this.cutRadius; }
		}
		/// <summary>
		/// Gets the position of the cut in local space of the source entity, if a mesh of the new part was
		/// successfully approximated with capsules.
		/// </summary>
		public Vector3 CutSourcePosition
		{
			get { return this.cutSourcePosition; }
		}
		/// <summary>
		/// Gets the position of the cut in local space of the source entity, if a mesh of the new part was
		/// successfully approximated with capsules.
		/// </summary>
		public Vector3 CutPartPosition
		{
			get { return this.cutPartPosition; }
		}
		/// <summary>
		/// Gets the normal to plane of the cut in local space of the new part entity, if a mesh of the new
		/// part was successfully approximated with capsules.
		/// </summary>
		public Vector3 CutSourceNormal
		{
			get { return this.cutSourceNormal; }
		}
		/// <summary>
		/// Gets the normal to plane of the cut in local space of the new part entity, if a mesh of the new
		/// part was successfully approximated with capsules.
		/// </summary>
		public Vector3 CutPartNormal
		{
			get { return this.cutPartNormal; }
		}
		/// <summary>
		/// Gets the object that represents the new mesh that is used by the part that has broken off.
		/// </summary>
		public GeometryShape NewMesh
		{
			get { return this.newMesh; }
		}
		/// <summary>
		/// Gets the pointer to the last mesh update that was applied to the geometry at the moment of this
		/// event.
		/// </summary>
		/// <remarks>
		/// Pass this pointer to <see cref="MeshUpdate.GetNext"/> if you want to traverse the linked list
		/// of updates up-to this point.
		/// </remarks>
		public MeshUpdate* LastMeshUpdate
		{
			get { return this.lastUpdate; }
		}
		#endregion
		#region Construction
		internal CreatedPartInfo(PhysicalEntity newEntity, int oldPartId, int newPartId, int totalPartCount,
								 EulerAngles breakageAngularImpulse, EulerAngles ejectionAngularVelocity,
								 Vector3 breakageImpulse, bool invalid, PhysicsPartCreationReason reason,
								 Vector3 ejectionVelocity, float breakageSize, float cutRadius,
								 Vector3 cutSourcePosition, Vector3 cutPartPosition, Vector3 cutSourceNormal,
								 Vector3 cutPartNormal, GeometryShape newMesh,
								 MeshUpdate* lastUpdate)
		{
			this.newEntity = newEntity;
			this.oldPartId = oldPartId;
			this.newPartId = newPartId;
			this.totalPartCount = totalPartCount;
			this.breakageAngularImpulse = breakageAngularImpulse;
			this.ejectionAngularVelocity = ejectionAngularVelocity;
			this.breakageImpulse = breakageImpulse;
			this.invalid = invalid;
			this.reason = reason;
			this.ejectionVelocity = ejectionVelocity;
			this.breakageSize = breakageSize;
			this.cutRadius = cutRadius;
			this.cutSourcePosition = cutSourcePosition;
			this.cutPartPosition = cutPartPosition;
			this.cutSourceNormal = cutSourceNormal;
			this.cutPartNormal = cutPartNormal;
			this.newMesh = newMesh;
			this.lastUpdate = lastUpdate;
		}
		#endregion
	}
}