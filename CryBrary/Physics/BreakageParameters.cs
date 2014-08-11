using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;

namespace CryEngine
{
	public enum BreakageType
	{
		Destroy = 0,
		Freeze_Shatter
	}

	public struct BreakageParameters
	{
		public BreakageType type;                    // Type of the breakage.
		public float fParticleLifeTime;        // Average lifetime of particle pieces.
		public int nGenericCount;                // If not 0, force particle pieces to spawn generically, this many times.
		public bool bForceEntity;                    // Force pieces to spawn as entities.
		public bool bMaterialEffects;            // Automatically create "destroy" and "breakage" material effects on pieces.
		public bool bOnlyHelperPieces;        // Only spawn helper pieces.

		// Impulse params.
		public float fExplodeImpulse;            // Outward impulse to apply.
		public Vector3 vHitImpulse;                    // Hit impulse and center to apply.
		public Vector3 vHitPoint;
	}
}