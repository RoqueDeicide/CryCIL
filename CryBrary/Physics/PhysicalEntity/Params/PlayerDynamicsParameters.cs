using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.Utilities;

namespace CryEngine
{
	public struct PlayerDynamicsParameters
	{
		public static PlayerDynamicsParameters Create()
		{
			var dyn = new PlayerDynamicsParameters();

			dyn.type = 4;

			dyn.kInertia = UnusedMarker.Float;
			dyn.kInertiaAccel = UnusedMarker.Float;
			dyn.kAirControl = UnusedMarker.Float;
			dyn.gravity = UnusedMarker.Vector3;
			dyn.nodSpeed = UnusedMarker.Float;
			dyn.mass = UnusedMarker.Float;
			dyn.bSwimming = UnusedMarker.Integer;
			dyn.surface_idx = UnusedMarker.Integer;
			dyn.bActive = UnusedMarker.Integer;
			dyn.collTypes = (EntityQueryFlags)UnusedMarker.Integer;
			dyn.livingEntToIgnore = UnusedMarker.IntPtr;
			dyn.minSlideAngle = UnusedMarker.Float;
			dyn.maxClimbAngle = UnusedMarker.Float;
			dyn.maxJumpAngle = UnusedMarker.Float;
			dyn.minFallAngle = UnusedMarker.Float;
			dyn.kAirResistance = UnusedMarker.Float;
			dyn.bNetwork = UnusedMarker.Integer;
			dyn.maxVelGround = UnusedMarker.Float;
			dyn.timeImpulseRecover = UnusedMarker.Float;
			dyn.iRequestedTime = UnusedMarker.Integer;

			return dyn;
		}

		internal int type;

		public float kInertia;    // inertia koefficient, the more it is, the less inertia is; 0 means no inertia
		public float kInertiaAccel; // inertia on acceleration
		public float kAirControl; // air control koefficient 0..1, 1 - special value (total control of movement)
		public float kAirResistance;    // standard air resistance
		public Vector3 gravity; // gravity vector
		public float nodSpeed;    // vertical camera shake speed after landings
		public int bSwimming; // whether entity is swimming (is not bound to ground plane)
		public float mass;    // mass (in kg)
		public int surface_idx; // surface identifier for collisions
		public float minSlideAngle; // if surface slope is more than this angle, player starts sliding (angle is in radians)
		public float maxClimbAngle; // player cannot climb surface which slope is steeper than this angle
		public float maxJumpAngle; // player is not allowed to jump towards ground if this angle is exceeded
		public float minFallAngle;    // player starts falling when slope is steeper than this
		public float maxVelGround; // player cannot stand of surfaces that are moving faster than this
		public float timeImpulseRecover; // forcefully turns on inertia for that duration after receiving an impulse
		public EntityQueryFlags collTypes; // entity types to check collisions against
		private IntPtr livingEntToIgnore;
		private int bNetwork; // uses extended history information (obsolete)
		private int bActive; // 0 disables all simulation for the character, apart from moving along the requested velocity
		private int iRequestedTime; // requests that the player rolls back to that time and re-exucutes pending actions during the next step
	}
}