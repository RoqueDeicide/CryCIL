using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about physical properties of the surface.
	/// </summary>
	public struct SurfaceTypePhysicalParameters
	{
		[UsedImplicitly]
		private int breakableId;
		[UsedImplicitly]
		private int breakEnergy;
		[UsedImplicitly]
		private float holeSize;
		[UsedImplicitly]
		private float holeSizeExplosion;
		[UsedImplicitly]
		private float hitRadius;
		[UsedImplicitly]
		private float hitPoints;
		[UsedImplicitly]
		private float hitPointsSecondary;
		[UsedImplicitly]
		private float hitMaxDamage;
		[UsedImplicitly]
		private float hitLifeTime;
		[UsedImplicitly]
		private int pierceability;
		[UsedImplicitly]
		private float damageReduction;
		[UsedImplicitly]
		private float ricochetAngle;
		[UsedImplicitly]
		private float ricochetDamageReduction;
		[UsedImplicitly]
		private float ricochetVelocityReduction;
		[UsedImplicitly]
		private float friction;
		[UsedImplicitly]
		private float bouncyness;
		[UsedImplicitly]
		private int breakability;
		[UsedImplicitly]
		private int collType;
		[UsedImplicitly]
		private float soundObstruction;
		/// <summary>
		/// Unknown.
		/// </summary>
		public int BreakableId
		{
			get { return this.breakableId; }
		}
		/// <summary>
		/// Gets the amount of energy that needs to be delivered to the surface of the object to break it.
		/// </summary>
		public int BreakEnergy
		{
			get { return this.breakEnergy; }
		}
		/// <summary>
		/// Gets the size of the hole that can be made in the object with this surface with a simple
		/// impact.
		/// </summary>
		public float HoleSize
		{
			get { return this.holeSize; }
		}
		/// <summary>
		/// Gets the size of the hole that can be made in the object with this surface with an explosion.
		/// </summary>
		public float HoleSizeExplosion
		{
			get { return this.holeSizeExplosion; }
		}
		/// <summary>
		/// Gets the radius of the decal that represents a hit marker on the surface.
		/// </summary>
		public float HitRadius
		{
			get { return this.hitRadius; }
		}
		/// <summary>
		/// Gets the primary number of hit points the object with this surface can have.
		/// </summary>
		public float HitPoints
		{
			get { return this.hitPoints; }
		}
		/// <summary>
		/// Gets the primary number of hit points the object with this surface can have.
		/// </summary>
		/// <remarks>
		/// Can be used to randomize number of hit points different objects with the same surface can have
		/// (?).
		/// </remarks>
		public float HitPointsSecondary
		{
			get { return this.hitPointsSecondary; }
		}
		/// <summary>
		/// Gets maximal amount of damage that can be dealt to the object with this surface with one
		/// impact.
		/// </summary>
		public float HitMaxDamage
		{
			get { return this.hitMaxDamage; }
		}
		/// <summary>
		/// Gets the time span it takes for the hit decal to disappear from the surface.
		/// </summary>
		public float HitLifeTime
		{
			get { return this.hitLifeTime; }
		}
		/// <summary>
		/// Gets the value that determines pierceability of the surface.
		/// </summary>
		public int Pierceability
		{
			get { return this.pierceability; }
		}
		/// <summary>
		/// Gets the value that determines reduction of the damage inflicted by the bullet that pierced
		/// this surface.
		/// </summary>
		public float DamageReduction
		{
			get { return this.damageReduction; }
		}
		/// <summary>
		/// Gets the angle at which the impact object will ricochet off this surface.
		/// </summary>
		public float RicochetAngle
		{
			get { return this.ricochetAngle; }
		}
		/// <summary>
		/// Gets the value that determines reduction of the damage inflicted by the bullet that ricocheted
		/// off this surface.
		/// </summary>
		public float RicochetDamageReduction
		{
			get { return this.ricochetDamageReduction; }
		}
		/// <summary>
		/// Gets the value that determines reduction of the velocity of the bullet that ricocheted off this
		/// surface.
		/// </summary>
		public float RicochetVelocityReduction
		{
			get { return this.ricochetVelocityReduction; }
		}
		/// <summary>
		/// Gets the surface friction.
		/// </summary>
		public float Friction
		{
			get { return this.friction; }
		}
		/// <summary>
		/// Gets the value that indicates how bouncy the surface is.
		/// </summary>
		public float Bouncyness
		{
			get { return this.bouncyness; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int Breakability
		{
			get { return breakability; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int CollisionType
		{
			get { return this.collType; }
		}
		/// <summary>
		/// Gets the value that indicates how easy it is for the sound to penetrate.
		/// </summary>
		public float SoundObstruction
		{
			get { return this.soundObstruction; }
		}
	}
}