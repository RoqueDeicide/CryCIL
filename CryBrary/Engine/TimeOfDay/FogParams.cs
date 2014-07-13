using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum FogParams
	{
		ColorBottom = 14,
		ColorBottomMultiplier,
		HeightBottom,
		DensityBottom,

		ColorTop,
		ColorTopMultiplier,
		HeightTop,
		DensityTop,

		ColorHeightOffset,

		ColorRadial,
		ColorRadialMultiplier,
		RadialSize,
		RadialLobe,

		FinalDensityClamp,

		GlobalDensity,
		RampStart,
		RampEnd,
		RampInfluence,

		OceanFogColor = 67,
		OceanFogColorMultiplier,
		OceanFogDesnity,
	}
}