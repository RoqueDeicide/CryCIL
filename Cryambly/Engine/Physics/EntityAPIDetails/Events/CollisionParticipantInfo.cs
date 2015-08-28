using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about one of the entities that participates in the collision.
	/// </summary>
	public struct CollisionParticipantInfo
	{
		#region Fields
		private readonly Vector3 velocity;
		private readonly float mass;
		private readonly int partId;
		private readonly short matId;
		[UsedImplicitly] private readonly short iPrim;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the velocity of the participant at the moment of contact.
		/// </summary>
		public Vector3 Velocity
		{
			get { return this.velocity; }
		}
		/// <summary>
		/// Gets the mass of the participant at the moment of contact.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
		}
		/// <summary>
		/// Gets the identifier of the part of the entity that represents a participant.
		/// </summary>
		public int PartIdentifier
		{
			get { return this.partId; }
		}
		/// <summary>
		/// Gets the identifier of the material the participant used at the moment of contact.
		/// </summary>
		public short MaterialId
		{
			get { return this.matId; }
		}
		#endregion
		#region Construction
		internal CollisionParticipantInfo(Vector3 velocity, int partId, float mass, short matId, short iPrim)
		{
			this.velocity = velocity;
			this.partId = partId;
			this.mass = mass;
			this.matId = matId;
			this.iPrim = iPrim;
		}
		#endregion
	}
}