using System;
using CryCil.Geometry;

namespace CryCil.Engine.Rendering.Lighting
{
	/// <summary>
	/// Enumeration of flags that describe the dynamic light source.
	/// </summary>
	[Flags]
	public enum DynamicLightFlags
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		AreaSpecTex = 1 << 0,
		/// <summary>
		/// When set, specifies that the light source is a directional light source.
		/// </summary>
		Directional = 1 << 1,
		/// <summary>
		/// When set, specifies that the light source projects itself onto a box with specified dimensions.
		/// </summary>
		BoxProjected = 1 << 2,
		/// <summary>
		/// When set, specifies that the light source has a <see cref="BoundingBox"/> defined for it that
		/// defines the dimensions along which this light will fade.
		/// </summary>
		LightBoxFalloff = 1 << 3,
		/// <summary>
		/// When set, specifies that the light source is rendered after everything else (can be used to
		/// light up first-person UI, like menus).
		/// </summary>
		Post3DRenderer = 1 << 4,
		/// <summary>
		/// When set, specifies that the light source casts shadows.
		/// </summary>
		CastShadowMaps = 1 << 5,
		/// <summary>
		/// When set, specifies that the light source is a point light source.
		/// </summary>
		Point = 1 << 6,
		/// <summary>
		/// When set, specifies that the light source has a projected beam (can be used for smth like
		/// vehicle headlights).
		/// </summary>
		Project = 1 << 7,
		/// <summary>
		/// Unknown.
		/// </summary>
		LightBeam = 1 << 8,
		/// <summary>
		/// When set, specifies that the light source casts reflective shadows.
		/// </summary>
		ReflectiveShadowMap = 1 << 9,
		/// <summary>
		/// When set, specifies that the light source lights up everything regardless of whether it's in
		/// VisArea or not.
		/// </summary>
		IgnoresVisAreas = 1 << 10,
		/// <summary>
		/// When set, specifies that the light source uses deferred cubemap rendering.
		/// </summary>
		DeferredCubemaps = 1 << 11,
		/// <summary>
		/// When set, specifies that the light source has a set of planes that limit its extent.
		/// </summary>
		HasClipVolume = 1 << 12,
		/// <summary>
		/// When set, specifies that the light source is disabled.
		/// </summary>
		Disabled = 1 << 13,
		/// <summary>
		/// When set, specifies that the light source is an area light.
		/// </summary>
		AreaLight = 1 << 14,
		/// <summary>
		/// When set, specifies that the light source has defined clipping bounds.
		/// </summary>
		HasClipBound = 1 << 15,
		/// <summary>
		/// When set, specifies that the light source has defined clipping geometry.
		/// </summary>
		HasClipGeometry = 1 << 16,
		/// <summary>
		/// When set, specifies that the light source is fake (used for Flares, beams and such).
		/// </summary>
		Fake = 1 << 17,
		/// <summary>
		/// When set, specifies that the light source is a sun.
		/// </summary>
		Sun = 1 << 18,
		/// <summary>
		/// Unknown.
		/// </summary>
		Lm = 1 << 19,
		/// <summary>
		/// When set, specifies that the light source only affects the VisArea it's currently inside.
		/// </summary>
		ThisAreaOnly = 1 << 20,
		/// <summary>
		/// When set, specifies that the light source emits ambient light.
		/// </summary>
		Ambient = 1 << 21,
		/// <summary>
		/// When set, specifies that the light source doesn't affect the terrain.
		/// </summary>
		IndoorOnly = 1 << 22,
		/// <summary>
		/// When set, specifies that the light source affects volumetric fog.
		/// </summary>
		VolumetricFog = 1 << 23,
		/// <summary>
		/// When set, specifies that the light source allows Light Propagation Volume to be defined for it.
		/// </summary>
		AllowLightPropagationVolume = 1 << 24,
		/// <summary>
		/// When set, specifies that the light source is attached to the Sun.
		/// </summary>
		AttachToSun = 1 << 25,
		/// <summary>
		/// Unknown.
		/// </summary>
		TrackViewTimeScrubbing = 1 << 26,
		/// <summary>
		/// When set, specifies that the light source only affects volumetric fog.
		/// </summary>
		VolumetricFogOnly = 1 << 27,

		/// <summary>
		/// Deprecated.
		/// </summary>
		[Obsolete]
		DeferredLight = 1 << 29,
		/// <summary>
		/// Deprecated.
		/// </summary>
		[Obsolete]
		SpecularOcclusion = 1 << 30,
		/// <summary>
		/// Deprecated.
		/// </summary>
		[Obsolete]
		DiffuseOcclusion = 1 << 31,

		/// <summary>
		/// A mask that allows to check whether light source has a defined type.
		/// </summary>
		LightTypeMask = (Directional | Point | Project | AreaLight)
	}
}