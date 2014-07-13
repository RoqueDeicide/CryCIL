using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum SkyLightParams
	{
		SunIntensity = 32,
		SunIntensityMultiplier,

		MieScattering,
		RayleighScattering,
		SunAntisotropyFactor,

		WaveLengthR,
		WaveLengthG,
		WaveLengthB,
	}
}