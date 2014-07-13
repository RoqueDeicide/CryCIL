using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum PhysicsForeignIdentifiers
	{
		Terrain = 0,
		Static = 1,
		Entity = 2,
		Foliage = 3,
		Rope = 4,
		SoundObstruction = 5,
		SoundProxyObstruction = 6,
		SoundReverbObstruction = 7,
		WaterVolume = 8,
		BreakableGlass = 9,
		BreakableGlassFragment = 10,
		RigidParticle = 11,

		/// <summary>
		/// All user defined foreign ids should start from this enum.
		/// </summary>
		UserDefined = 100,
	}

	public struct ColliderInfo
	{
		public PhysicsForeignIdentifiers foreignId;
		private IntPtr foreignData;

		public EntityBase Entity
		{
			get
			{
				if (foreignId != PhysicsForeignIdentifiers.Entity)
					return null;

				return CryEngine.Entity.Get(foreignData);
			}
		}

		/// <summary>
		/// Velocity at the contact point
		/// </summary>
		public Vector3 velocity;
		public float mass;
		public int partId;
		public short materialId;
		public short iPrim;
	}
}