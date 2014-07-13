using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public struct SurfaceTypeParams
	{
		public int BreakableId { get; set; }
		public int BreakEnergy { get; set; }
		public float HoleSize { get; set; }
		public float HoleSize_Explosion { get; set; }
		public float HitRadius { get; set; }
		public float HitPoints { get; set; }
		public float HitPoints_Secondary { get; set; }
		public float HitMaxDamage { get; set; }
		public float HitLifetime { get; set; }
		public int Pierceability { get; set; }
		public float DamageReduction { get; set; }
		public float RicochetAngle { get; set; }
		public float RicochetDamageReduction { get; set; }
		public float RicochetVelocityReduction { get; set; }
		public float Friction { get; set; }
		public float Bouncyness { get; set; }
		public int iBreakability { get; set; }
	}
}