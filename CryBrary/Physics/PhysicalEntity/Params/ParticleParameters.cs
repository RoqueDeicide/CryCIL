using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.RunTime;
using CryEngine.Utilities;

namespace CryEngine
{
	[CLSCompliant(false)]
	public struct ParticleParameters
	{
		public static ParticleParameters Create()
		{
			var pparams = new ParticleParameters
			{
				type = 3,
				mass = UnusedMarker.Float,
				size = UnusedMarker.Float,
				thickness = UnusedMarker.Float,
				wspin = UnusedMarker.Vector3,
				accThrust = UnusedMarker.Float,
				kAirResistance = UnusedMarker.Float,
				kWaterResistance = UnusedMarker.Float,
				velocity = UnusedMarker.Float,
				heading = UnusedMarker.Vector3,
				accLift = UnusedMarker.Float,
				gravity = UnusedMarker.Vector3,
				waterGravity = UnusedMarker.Vector3,
				surface_idx = UnusedMarker.Integer,
				normal = UnusedMarker.Vector3,
				q0 = UnusedMarker.Quaternion,
				minBounceVel = UnusedMarker.Float,
				rollAxis = UnusedMarker.Vector3,
				flags = (PhysicalizationFlags)UnusedMarker.UnsignedInteger,
				pColliderToIgnore = UnusedMarker.IntPtr,
				iPierceability = UnusedMarker.Integer,
				areaCheckPeriod = UnusedMarker.Integer,
				minVel = UnusedMarker.Float,
				collTypes = UnusedMarker.Integer
			};

			return pparams;
		}

		internal int type;

		public PhysicalizationFlags flags; // see entity flags
		public float mass;
		public float size; // pseudo-radius
		public float thickness; // thickness when lying on a surface (if left unused, size will be used)
		public Vector3 heading; // direction of movement
		public float velocity;	// velocity along "heading"
		public float kAirResistance; // air resistance koefficient, F = kv
		public float kWaterResistance; // same for water
		public float accThrust; // acceleration along direction of movement
		public float accLift; // acceleration that lifts particle with the current speed
		public int surface_idx;
		public Vector3 wspin; // angular velocity
		public Vector3 gravity;	// stores this gravity and uses it if the current area's gravity is equal to the global gravity
		public Vector3 waterGravity; // gravity when underwater
		public Vector3 normal; // aligns this direction with the surface normal when sliding
		public Vector3 rollAxis; // aligns this directon with the roll axis when rolling (0,0,0 to disable alignment)
		public Quaternion q0;	// initial orientation (zero means x along direction of movement, z up)
		public float minBounceVel;	// velocity threshold for bouncing->sliding switch
		public float minVel;	// sleep speed threshold
		public IntPtr pColliderToIgnore;	// physical entity to ignore during collisions
		public int iPierceability;	// pierceability for ray tests; pierceble hits slow the particle down, but don't stop it
		public int collTypes; // 'objtype' passed to RayWorldntersection
		public int areaCheckPeriod; // how often (in frames) world area checks are made
	}
}