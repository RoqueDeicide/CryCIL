using System;
using System.Collections.Generic;

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
				explDir = new Vec3(0, 0, 1)
			};
		}

		public void Explode()
		{
			if (explosion.rmax == 0)
				explosion.rmax = 0.0001f;
			explosion.nOccRes = explosion.rmax > 50 ? 0 : 16;

			affectedEnts = NativePhysicsMethods.SimulateExplosion(explosion);
		}

		public Vec3 Epicenter { get { return explosion.epicenter; } set { explosion.epicenter = value; } }
		public Vec3 EpicenterImpulse { get { return explosion.epicenterImp; } set { explosion.epicenterImp = value; } }

		public Vec3 Direction { get { return explosion.explDir; } set { explosion.explDir = value; } }

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
				foreach (IntPtr ptr in affectedEnts)
					yield return PhysicalEntity.TryGet(ptr);
			}
		}

		internal pe_explosion explosion;
	}

	internal struct pe_explosion
	{
		public Vec3 epicenter;    // epicenter for the occlusion computation
		public Vec3 epicenterImp; // epicenter for impulse computation
		// the impulse a surface fragment with area dS and normal n gets is:
		// dS*k*n*max(0,n*dir_to_epicenter)/max(rmin, dist_to_epicenter)^2 k is selected in such way
		// that at impulsivePressureAtR = k/r^2
		public float rmin, rmax, r;
		public float impulsivePressureAtR;
		public int nOccRes; // resolution of the occlusion map (0 disables)
		public int nGrow; // grow occlusion projections by this amount of cells to allow explosion to reach around corners a bit
		public float rminOcc; // ignores geometry closer than this for occlusion computations
		public float holeSize;    // explosion shape for iholeType will be scaled by this holeSize / shape's declared size
		public Vec3 explDir;    // hit direction, for aligning the explosion boolean shape
		public int iholeType; // breakability index for the explosion (<0 disables)
		public bool forceDeformEntities; // force deformation even if breakImpulseScale is zero
		// filled as results
		IntPtr pAffectedEnts { get; set; }
		IntPtr pAffectedEntsExposure { get; set; }    // 0..1 exposure, computed from the occlusion map
		public int nAffectedEnts;
	}
}