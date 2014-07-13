using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public struct LightParams
	{
		public string specularCubemap;
		public string diffuseCubemap;
		public string lightImage;
		public string lightAttenMap;

		public Color color;
		public Vector3 origin;

		public float shadowBias;
		public float shadowSlopeBias;

		public float radius;
		public float specularMultiplier;

		public float hdrDynamic;

		public float animSpeed;
		public float coronaScale;
		public float coronaIntensity;
		public float coronaDistSizeFactor;
		public float coronaDistIntensityFactor;

		public float shaftSrcSize;
		public float shaftLength;
		public float shafeBrightness;
		public float shaftBlendFactor;
		public float shaftDecayFactor;

		public float lightFrustumAngle;
		public float projectNearPlane;

		public float shadowUpdateMinRadius;

		public int lightStyle;
		public int lightPhase;
		public int postEffect;
		public int shadowChanMask;
		public Int16 shadowUpdateRatio;

		public LightFlags flags;
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