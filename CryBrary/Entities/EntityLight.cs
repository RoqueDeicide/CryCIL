using System;
using CryEngine.Mathematics;

namespace CryEngine.Entities
{
	/// <summary>
	/// Encapsulates parameters that define a light source.
	/// </summary>
	public struct LightParams
	{
		/// <summary>
		/// Path to file that contains a specular cubemap.
		/// </summary>
		public string SpecularCubemapFile;
		/// <summary>
		/// Path to file that contains a diffuse cubemap.
		/// </summary>
		public string DiffuseCubemapFile;
		/// <summary>
		/// Path to file that contains an image that is projected around the light source.
		/// </summary>
		public string LightImageFile;
		/// <summary>
		/// Path to file that contains a light attenuation(?) map.
		/// </summary>
		public string LightAttenuationMap;
		/// <summary>
		/// Color of the light.
		/// </summary>
		public Color Color;
		/// <summary>
		/// Location of the light source.
		/// </summary>
		public Vector3 Origin;
		/// <summary>
		/// Parameter that affects shadow rendering.
		/// </summary>
		public float ShadowBias;
		/// <summary>
		/// Parameter that affects shadow rendering on slopes.
		/// </summary>
		public float ShadowSlopeBias;
		/// <summary>
		/// Radius of the area around the light source that is affected by the light.
		/// </summary>
		public float Radius;
		/// <summary>
		/// Value that defines the brightness of the light source.
		/// </summary>
		public float SpecularMultiplier;
		/// <summary>
		/// Value that defines dynamic HDR.
		/// </summary>
		public float HdrDynamic;
		/// <summary>
		/// Animation speed factor.
		/// </summary>
		public float AnimationSpeed;
		/// <summary>
		/// Scale of the light corona.
		/// </summary>
		public float CoronaScale;
		/// <summary>
		/// Intensity of the light corona.
		/// </summary>
		public float CoronaIntensity;
		/// <summary>
		/// Value that defines how size of the light corona changes with distance from viewer.
		/// </summary>
		public float CoronaDistanceSizeFactor;
		/// <summary>
		/// Value that defines how intensity of the light corona changes with distance from viewer.
		/// </summary>
		public float CoronaDistanceIntensityFactor;
		/// <summary>
		/// Size of the light shaft source.
		/// </summary>
		public float ShaftSourceSize;
		/// <summary>
		/// Length of light shafts.
		/// </summary>
		public float ShaftLength;
		/// <summary>
		/// Brightness of light shafts.
		/// </summary>
		public float ShafeBrightness;
		/// <summary>
		/// Blend factor of light shafts.
		/// </summary>
		public float ShaftBlendFactor;
		/// <summary>
		/// Decay factor of light shafts.
		/// </summary>
		public float ShaftDecayFactor;
		/// <summary>
		/// "Field of light".
		/// </summary>
		public float LightFrustumAngle;
		public float ProjectNearPlane;

		public float ShadowUpdateMinRadius;

		public int LightStyle;
		public int LightPhase;
		public int PostEffect;
		public int ShadowChanMask;
		public Int16 ShadowUpdateRatio;

		public LightFlags Flags;
	}

	[Flags]
	public enum LightFlags : long
	{
		Directional = 2,
		CastShadows = 0x10,
		Point = 0x20,
		Project = 0x40,
		HasCBuffer = 0x80,
		ReflectiveShadowmap = 0x100,
		IgnoreVisAreas = 0x200,
		DeferredCubemaps = 0x400,
		DeferredIndirectLight = 0x800,
		Disabled = 0x1000,
		HasClipBound = 0x4000,
		LightSource = 0x10000,
		Fake = 0x20000,
		Sun = 0x40000,
		Local = 0x100000,
		LM = 0x200000,
		ThisAreaOnly = 0x400000,
		AmbientLight = 0x800000,
		Negative = 0x1000000,
		IndoorOnly = 0x2000000,
		HighSpecOnly = 0x4000000,
		SpecularHighSpecOnly = 0x8000000,
		DeferredLight = 0x10000000,
		IraddianceVolumes = 0x20000000,
		SpecularOcclusion = 0x40000000,
		DiffuseOcclusion = 0x80000000
	}
}