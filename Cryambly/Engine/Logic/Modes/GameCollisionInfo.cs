using System;
using System.Linq;
using CryCil.Engine.Physics;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Encapsulates information about collision of 2 entities.
	/// </summary>
	public struct GameCollisionInfo
	{
		/// <summary>
		/// Object that provides information about entities which participate in the collision. In the
		/// object <see cref="StereoPhysicsEventData.FirstEntity"/> represents a collider and
		/// <see cref="StereoPhysicsEventData.SecondEntity"/> represents a collidee.
		/// </summary>
		public StereoPhysicsEventData Entities;
		/// <summary>
		/// Object that provides general information about the collision.
		/// </summary>
		public CollisionInfo Collision;
		/// <summary>
		/// Object that provides additional information about the collider.
		/// </summary>
		public CollisionParticipantInfo Collider;
		/// <summary>
		/// If collider is a managed entity than this field will not be <c>null</c>.
		/// </summary>
		public MonoEntity ColliderEntity;
		/// <summary>
		/// A CryEngine entity that hosts the <see cref="StereoPhysicsEventData.FirstEntity"/> that
		/// participates in this collision.
		/// </summary>
		public CryEntity ColliderCryEntity;
		/// <summary>
		/// Object that provides additional information about the collidee.
		/// </summary>
		public CollisionParticipantInfo Collidee;
		/// <summary>
		/// If collidee is a managed entity than this field will not be <c>null</c>.
		/// </summary>
		public MonoEntity CollideeEntity;
		/// <summary>
		/// A CryEngine entity that hosts the <see cref="StereoPhysicsEventData.SecondEntity"/> that
		/// participates in this collision.
		/// </summary>
		public CryEntity CollideeCryEntity;
	}
}