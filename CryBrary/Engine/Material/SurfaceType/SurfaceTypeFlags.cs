using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum SurfaceTypeFlags
	{
		NoPhysicalize = 1 << 1, // This surface should not be physicalized.
		NoCollision = 1 << 2, // Should only be set for vegetation canopy mats
		VehicleOnlyCollision = 1 << 3,
		CanShatter = 1 << 4, // This surface type can shatter
		BulletPierceable = 1 << 5, // This surface is pierceable by bullets (used by MFX system to spawn front/back FX)
	};
}