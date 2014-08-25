using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.Utilities;

namespace CryEngine.Physics.Status
{
	public struct LivingPhysicsStatus
	{
		public static LivingPhysicsStatus Create()
		{
			var status = new LivingPhysicsStatus { type = 2 };

			return status;
		}

		// ReSharper disable NotAccessedField.Local
		private int type;
		// ReSharper restore NotAccessedField.Local

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

		public Vector3 camOffset; // camera offset
		public Vector3 vel; // actual velocity (as rate of position change)
		public Vector3 velUnconstrained; // 'physical' movement velocity
		public Vector3 velRequested;    // velocity requested in the last action
		public Vector3 velGround;
		/// <summary>
		/// velocity of the object entity is standing on
		/// </summary>
		public Vector3 GroundVelocity { get { return velGround; } }

		private float groundHeight;
		/// <summary>
		/// position where the last contact with the ground occured
		/// </summary>
		public float GroundHeight { get { return groundHeight; } }

		private Vector3 groundSlope;
		public Vector3 GroundNormal { get { return groundSlope; } }

		private int groundSurfaceIdx;
		public int GroundSurfaceId { get { return groundSurfaceIdx; } }

		public SurfaceType GroundSurfaceType { get { return SurfaceType.Get(groundSurfaceIdx); } }

		public int groundSurfaceIdxAux; // contact with the ground that also has default collision flags
		public IntPtr pGroundCollider;    // only returns an actual entity if the ground collider is not static
		public int iGroundColliderPart;
		public float timeSinceStanceChange;
		// int bOnStairs; // tries to detect repeated abrupt ground height changes
		public int bStuck;    // tries to detect cases when the entity cannot move as before because of collisions
		public IntPtr pLockStep; // internal timestepping lock
		public int iCurTime; // quantised time
		public int bSquashed; // entity is being pushed by heavy objects from opposite directions
	}
}