using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum ColorGradingParams
	{
		HDR_ColorSaturation = 77,
		HDR_ColorContrast,
		HDR_ColorBalance,

		ColorSaturation,
		ColorContrast,
		ColorBrightness,

		MinInput,
		Gamme,
		MaxInput,
		MinOutput,
		MaxOutput,

		SelectiveColor_Color,
		SelectiveColor_Cyans,
		SelectiveColor_Magnetas,
		SelectiveColor_Yellows,
		SelectiveColor_Blacks,

		Filters_Grain,
		Filters_Sharpening,
		Filters_PhotofilterColor,
		Filters_PhotofilterDensity,

		DepthOfField_FocusRange,
		DepthOfField_BlurAmount
	}
}