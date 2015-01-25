using System;
using System.Collections.Generic;
using System.Linq;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Physics;

namespace CryEngine
{
	public class Explosion
	{
		public Explosion()
		{
			explosion = new pe_explosion
			{
				rminOcc = 0.07f,
				explDir = new Vector3(0, 0, 1)
			};
		}

		public void Explode()
		{
			if (Math.Abs(this.explosion.rmax) < 0.0001f)
				explosion.rmax = 0.0001f;
			explosion.nOccRes = explosion.rmax > 50 ? 0 : 16;

			affectedEnts = PhysicsInterop.SimulateExplosion(explosion);
		}

		public Vector3 Epicenter { get { return explosion.epicenter; } set { explosion.epicenter = value; } }
		public Vector3 EpicenterImpulse { get { return explosion.epicenterImp; } set { explosion.epicenterImp = value; } }

		public Vector3 Direction { get { return explosion.explDir; } set { explosion.explDir = value; } }

		public float Radius { get { return explosion.r; } set { explosion.r = value; } }
		public float MinRadius { get { return explosion.rmin; } set { explosion.rmin = value; } }
		public float MaxRadius { get { return explosion.rmax; } set { explosion.rmax = value; } }

		public float ImpulsePressure { get { return explosion.impulsivePressureAtR; } set { explosion.impulsivePressureAtR = value; } }

		public float HoleSize { get { return explosion.holeSize; } set { explosion.holeSize = value; } }
		public int HoleType { get { return explosion.iholeType; } set { explosion.iholeType = value; } }

		public bool ForceEntityDeformation { get { return explosion.forceDeformEntities; } set { explosion.forceDeformEntities = value; } }

		// filled as results
		private object[] affectedEnts { get; set; }
		public IEnumerable<PhysicalEntity> AffectedEntities
		{
			get
			{
				return from IntPtr ptr in this.affectedEnts select PhysicalEntity.TryGet(ptr);
			}
		}

		internal pe_explosion explosion;
	}

	internal struct pe_explosion
	{
		public Vector3 epicenter;    // epicenter for the occlusion computation
		public Vector3 epicenterImp; // epicenter for impulse computation
		// the impulse a surface fragment with area dS and normal n gets is:
		// dS*k*n*max(0,n*dir_to_epicenter)/max(rmin, dist_to_epicenter)^2 k is selected
		// in such way that at impulsivePressureAtR = k/r^2
		public float rmin, rmax, r;
		public float impulsivePressureAtR;
		public int nOccRes; // resolution of the occlusion map (0 disables)
		public int nGrow; // grow occlusion projections by this amount of cells to allow explosion to reach around corners a bit
		public float rminOcc; // ignores geometry closer than this for occlusion computations
		public float holeSize;    // explosion shape for iholeType will be scaled by this holeSize / shape's declared size
		public Vector3 explDir;    // hit direction, for aligning the explosion boolean shape
		public int iholeType; // breakability index for the explosion (<0 disables)
		public bool forceDeformEntities; // force deformation even if breakImpulseScale is zero
		// filled as results
		public IntPtr pAffectedEnts;
		public IntPtr pAffectedEntsExposure;    // 0..1 exposure, computed from the occlusion map
		public int nAffectedEnts;
	}
}