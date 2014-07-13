using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;

namespace CryEngine.Physics.Status
{
	public struct LivingPhysicsStatus
	{
		public static LivingPhysicsStatus Create()
		{
			var status = new LivingPhysicsStatus();

			status.type = 2;

			return status;
		}

		private int type;

		/// <summary>
		/// whether entity has no contact with ground
		/// </summary>
		private int bFlying;
		public bool IsFlying { get { return bFlying == 1; } }

		private float timeFlying;
		/// <summary>
		/// for how long the entity was flying
		/// </summary>
		public float FlyTime { get { return timeFlying; } }

		private Vec3 camOffset; // camera offset
		private Vec3 vel; // actual velocity (as rate of position change)
		private Vec3 velUnconstrained; // 'physical' movement velocity
		private Vec3 velRequested;    // velocity requested in the last action
		private Vec3 velGround;
		/// <summary>
		/// velocity of the object entity is standing on
		/// </summary>
		public Vec3 GroundVelocity { get { return velGround; } }

		private float groundHeight;
		/// <summary>
		/// position where the last contact with the ground occured
		/// </summary>
		public float GroundHeight { get { return groundHeight; } }

		private Vec3 groundSlope;
		public Vec3 GroundNormal { get { return groundSlope; } }

		private int groundSurfaceIdx;
		public int GroundSurfaceId { get { return groundSurfaceIdx; } }

		public SurfaceType GroundSurfaceType { get { return SurfaceType.Get(groundSurfaceIdx); } }

		private int groundSurfaceIdxAux; // contact with the ground that also has default collision flags
		private IntPtr pGroundCollider;    // only returns an actual entity if the ground collider is not static
		private int iGroundColliderPart;
		private float timeSinceStanceChange;
		// int bOnStairs; // tries to detect repeated abrupt ground height changes
		private int bStuck;    // tries to detect cases when the entity cannot move as before because of collisions
		private IntPtr pLockStep; // internal timestepping lock
		private int iCurTime; // quantised time
		private int bSquashed; // entity is being pushed by heavy objects from opposite directions
	}
}