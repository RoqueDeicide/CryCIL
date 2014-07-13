using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum HDRParams
	{
		DynamicPowerFactor = 0,

		FilmCurve_ShoulderScale = 71,
		FilmCurve_LinearScale,
		FilmCurve_ToeScale,
		FilmCurve_WhitePoint,

		BlueShift,
		BlueShiftThreshold,
	}
}