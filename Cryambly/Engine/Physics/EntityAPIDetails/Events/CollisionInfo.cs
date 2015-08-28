namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about the collision.
	/// </summary>
	public struct CollisionInfo
	{
		#region Fields
		private readonly int idCollider;
		private readonly Vector3 point;
		private readonly Vector3 normal;
		private readonly float penetration;
		private readonly float impulse;
		private readonly float size;
		private readonly float decalSize;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the value that indicates whether the collider entity is a terrain object.
		/// </summary>
		public bool IsColliderTerrain
		{
			get { return this.idCollider == -1; }
		}
		/// <summary>
		/// Gets the value that indicates whether the collider entity is a water object.
		/// </summary>
		public bool IsColliderWater
		{
			get { return this.idCollider == -2; }
		}
		/// <summary>
		/// Gets the identifier of the collider entity.
		/// </summary>
		public int ColliderId
		{
			get { return this.idCollider; }
		}
		/// <summary>
		/// Gets the coordinates of the point of contact.
		/// </summary>
		public Vector3 PointOfContact
		{
			get { return this.point; }
		}
		/// <summary>
		/// Gets the normal vector to the point of contact.
		/// </summary>
		public Vector3 NormalToPointOfContact
		{
			get { return this.normal; }
		}
		/// <summary>
		/// Gets the penetration depth of the contact.
		/// </summary>
		public float PenetrationDepth
		{
			get { return this.penetration; }
		}
		/// <summary>
		/// Gets the magnitude of the impulse that is applied at the point of contact along the
		/// <see cref="NormalToPointOfContact"/> by the solver to resolve the collision.
		/// </summary>
		public float NormalImpulse
		{
			get { return this.impulse; }
		}
		/// <summary>
		/// Gets the approximate radius of the sphere that represents the contact area.
		/// </summary>
		/// <remarks>
		/// Can be used to approximate the pressure that is applied at the point of contact.
		/// </remarks>
		public float ContactAreaRadius
		{
			get { return this.size; }
		}
		/// <summary>
		/// Gets the maximal allowed size of decals that were caused by this explosion.
		/// </summary>
		public float MaxDecalSize
		{
			get { return this.decalSize; }
		}
		#endregion
		#region Construction
		internal CollisionInfo(int idCollider, Vector3 point, Vector3 normal, float penetration, float impulse,
							   float size, float decalSize)
		{
			this.idCollider = idCollider;
			this.point = point;
			this.normal = normal;
			this.penetration = penetration;
			this.impulse = impulse;
			this.size = size;
			this.decalSize = decalSize;
		}
		#endregion
	}
}