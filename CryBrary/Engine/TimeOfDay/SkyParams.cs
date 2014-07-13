using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum SkyParams
	{
		SkyBrightening = 1,
		GlobalIlluminationMultiplier = 3,

		SunColor,
		SunColorMultiplier,
		SunSpecularMultiplier,

		SkyColor,
		SkyColorMultiplier,

		AmbientGroundColor,
		AmbientGroundColorMultiplier,

		AmbientMinHeight,
		AmbientMaxHeight,

		SkyboxMultiplier = 70,
	}
}