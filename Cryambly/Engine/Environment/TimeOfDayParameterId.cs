using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Enumeration of identifiers of all variables.
	/// </summary>
	public enum TimeOfDayParameterId
	{
		/// <summary>
		/// </summary>
		HdrDynamicPowerFactor,
		/// <summary>
		/// </summary>
		TerrainOcclusionMultiplier,
		/// <summary>
		/// </summary>
		GlobalIlluminationMultiplier,

		/// <summary>
		/// </summary>
		SunColor,
		/// <summary>
		/// </summary>
		SunColorMultiplier,
		/// <summary>
		/// </summary>
		SunSpecularMultiplier,

		/// <summary>
		/// </summary>
		FogColor,
		/// <summary>
		/// </summary>
		FogColorMultiplier,
		/// <summary>
		/// </summary>
		VolumetricFogHeight,
		/// <summary>
		/// </summary>
		VolumetricFogDensity,
		/// <summary>
		/// </summary>
		FogColor2,
		/// <summary>
		/// </summary>
		FogColor2Multiplier,
		/// <summary>
		/// </summary>
		VolumetricFogHeight2,
		/// <summary>
		/// </summary>
		VolumetricFogDensity2,
		/// <summary>
		/// </summary>
		VolumetricFogHeightOffset,

		/// <summary>
		/// </summary>
		FogRadialColor,
		/// <summary>
		/// </summary>
		FogRadialColorMultiplier,
		/// <summary>
		/// </summary>
		VolumetricFogRadialSize,
		/// <summary>
		/// </summary>
		VolumetricFogRadialLobe,

		/// <summary>
		/// </summary>
		VolumetricFogFinalDensityClamp,

		/// <summary>
		/// </summary>
		VolumetricFogGlobalDensity,
		/// <summary>
		/// </summary>
		VolumetricFogRampStart,
		/// <summary>
		/// </summary>
		VolumetricFogRampEnd,
		/// <summary>
		/// </summary>
		VolumetricFogRampInfluence,

		/// <summary>
		/// </summary>
		VolumetricFogShadowDarkening,
		/// <summary>
		/// </summary>
		VolumetricFogShadowDarkeningSun,
		/// <summary>
		/// </summary>
		VolumetricFogShadowDarkeningAmbient,
		/// <summary>
		/// </summary>
		VolumetricFogShadowRange,

		/// <summary>
		/// </summary>
		SkyLightSunIntensity,
		/// <summary>
		/// </summary>
		SkyLightSunIntensityMultiplier,

		/// <summary>
		/// </summary>
		SkyLightKm,
		/// <summary>
		/// </summary>
		SkyLightKr,
		/// <summary>
		/// </summary>
		SkyLightG,

		/// <summary>
		/// </summary>
		SkyLightWavelengthR,
		/// <summary>
		/// </summary>
		SkyLightWavelengthG,
		/// <summary>
		/// </summary>
		SkyLightWavelengthB,

		/// <summary>
		/// </summary>
		NightSkyHorizonColor,
		/// <summary>
		/// </summary>
		NightSkyHorizonColorMultiplier,
		/// <summary>
		/// </summary>
		NightSkyZenithColor,
		/// <summary>
		/// </summary>
		NightSkyZenithColorMultiplier,
		/// <summary>
		/// </summary>
		NightSkyZenithShift,

		/// <summary>
		/// </summary>
		NightSkyStartIntensity,

		/// <summary>
		/// </summary>
		NightSkyMoonColor,
		/// <summary>
		/// </summary>
		NightSkyMoonColorMultiplier,
		/// <summary>
		/// </summary>
		NightSkyMoonInnerCoronaColor,
		/// <summary>
		/// </summary>
		NightSkyMoonInnerCoronaColorMultiplier,
		/// <summary>
		/// </summary>
		NightSkyMoonInnerCoronaScale,
		/// <summary>
		/// </summary>
		NightSkyMoonOuterCoronaColor,
		/// <summary>
		/// </summary>
		NightSkyMoonOuterCoronaColorMultiplier,
		/// <summary>
		/// </summary>
		NightSkyMoonOuterCoronaScale,

		/// <summary>
		/// </summary>
		CloudShadingSunLightMultiplier,
		/// <summary>
		/// </summary>
		CloudShadingSkyLightMultiplier,
		/// <summary>
		/// </summary>
		CloudShadingSunLightCustomColor,
		/// <summary>
		/// </summary>
		CloudShadingSunLightCustomColorMultiplier,
		/// <summary>
		/// </summary>
		CloudShadingSunLightCustomColorInfluence,

		/// <summary>
		/// </summary>
		SunShaftsVisibility,
		/// <summary>
		/// </summary>
		SunRaysVisibility,
		/// <summary>
		/// </summary>
		SunRaysAttenuation,
		/// <summary>
		/// </summary>
		SunRaysSunColorInfluence,
		/// <summary>
		/// </summary>
		SunRaysCustomColor,

		/// <summary>
		/// </summary>
		OceanFogColor,
		/// <summary>
		/// </summary>
		OceanFogColorMultiplier,
		/// <summary>
		/// </summary>
		OceanFogDensity,

		/// <summary>
		/// </summary>
		SkyBoxMultiplier,

		/// <summary>
		/// </summary>
		HdrFilmCurveShoulderScale,
		/// <summary>
		/// </summary>
		HdrFilmCurveLinearScale,
		/// <summary>
		/// </summary>
		HdrFilmCurveToeScale,
		/// <summary>
		/// </summary>
		HdrFilmCurveWhitePoint,

		/// <summary>
		/// </summary>
		HdrColorGradingColorSaturation,
		/// <summary>
		/// </summary>
		HdrColorGradingColorBalance,

		/// <summary>
		/// </summary>
		HdrEyeAdaptationSceneKey,
		/// <summary>
		/// </summary>
		HdrEyeAdaptationMinExposure,
		/// <summary>
		/// </summary>
		HdrEyeAdaptationMaxExposure,
		/// <summary>
		/// </summary>
		HdrBloomAmount,

		/// <summary>
		/// </summary>
		ColorGradingFiltersGrain,
		/// <summary>
		/// </summary>
		ColorGradingFiltersPhotoFilterColor,
		/// <summary>
		/// </summary>
		ColorGradingFiltersPhotoFilterDensity,

		/// <summary>
		/// </summary>
		ColorGradingDofFocusRange,
		/// <summary>
		/// </summary>
		ColorGradingDofBlurAmount,

		/// <summary>
		/// </summary>
		ShadowsColor0Bias,
		/// <summary>
		/// </summary>
		ShadowsColor0SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor1Bias,
		/// <summary>
		/// </summary>
		ShadowsColor1SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor2Bias,
		/// <summary>
		/// </summary>
		ShadowsColor2SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor3Bias,
		/// <summary>
		/// </summary>
		ShadowsColor3SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor4Bias,
		/// <summary>
		/// </summary>
		ShadowsColor4SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor5Bias,
		/// <summary>
		/// </summary>
		ShadowsColor5SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor6Bias,
		/// <summary>
		/// </summary>
		ShadowsColor6SlopeBias,
		/// <summary>
		/// </summary>
		ShadowsColor7Bias,
		/// <summary>
		/// </summary>
		ShadowsColor7SlopeBias,

		/// <summary>
		/// </summary>
		ShadowJittering,

		/// <summary>
		/// Total number of parameters.
		/// </summary>
		Total
	}
}