using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;

namespace CryEngine
{
	[CLSCompliant(false)]
	public struct ParticleParameters
	{
		public static ParticleParameters Create()
		{
			var pparams = new ParticleParameters();

			pparams.type = 3;

			pparams.mass = UnusedMarker.Float;
			pparams.size = UnusedMarker.Float;
			pparams.thickness = UnusedMarker.Float;
			pparams.wspin = UnusedMarker.Vec3;
			pparams.accThrust = UnusedMarker.Float;
			pparams.kAirResistance = UnusedMarker.Float;
			pparams.kWaterResistance = UnusedMarker.Float;
			pparams.velocity = UnusedMarker.Float;
			pparams.heading = UnusedMarker.Vec3;
			pparams.accLift = UnusedMarker.Float;
			pparams.gravity = UnusedMarker.Vec3;
			pparams.waterGravity = UnusedMarker.Vec3;

			pparams.surface_idx = UnusedMarker.Integer;
			pparams.normal = UnusedMarker.Vec3;
			pparams.q0 = UnusedMarker.Quat;
			pparams.minBounceVel = UnusedMarker.Float;
			pparams.rollAxis = UnusedMarker.Vec3;
			pparams.flags = (PhysicalizationFlags)UnusedMarker.UnsignedInteger;
			pparams.pColliderToIgnore = UnusedMarker.IntPtr;
			pparams.iPierceability = UnusedMarker.Integer;
			pparams.areaCheckPeriod = UnusedMarker.Integer;
			pparams.minVel = UnusedMarker.Float;
			pparams.collTypes = UnusedMarker.Integer;

			return pparams;
		}

		internal int type;

		public PhysicalizationFlags flags; // see entity flags
		public float mass;
		public float size; // pseudo-radius
		public float thickness; // thickness when lying on a surface (if left unused, size will be used)
		public Vec3 heading; // direction of movement
		public float velocity;	// velocity along "heading"
		public float kAirResistance; // air resistance koefficient, F = kv
		public float kWaterResistance; // same for water
		public float accThrust; // acceleration along direction of movement
		public float accLift; // acceleration that lifts particle with the current speed
		public int surface_idx;
		public Vec3 wspin; // angular velocity
		public Vec3 gravity;	// stores this gravity and uses it if the current area's gravity is equal to the global gravity
		public Vec3 waterGravity; // gravity when underwater
		public Vec3 normal; // aligns this direction with the surface normal when sliding
		public Vec3 rollAxis; // aligns this directon with the roll axis when rolling (0,0,0 to disable alignment)
		public Quat q0;	// initial orientation (zero means x along direction of movement, z up)
		public float minBounceVel;	// velocity threshold for bouncing->sliding switch
		public float minVel;	// sleep speed threshold
		private IntPtr pColliderToIgnore;	// physical entity to ignore during collisions
		public int iPierceability;	// pierceability for ray tests; pierceble hits slow the particle down, but don't stop it
		public int collTypes; // 'objtype' passed to RayWorldntersection
		public int areaCheckPeriod; // how often (in frames) world area checks are made
	}
}