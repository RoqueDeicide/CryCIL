using System;
using System.Runtime.InteropServices;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering.Lighting
{
	/// <summary>
	/// Encapsulates a set of properties that define a light source.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LightProperties
	{
		/// <summary>
		/// A set of flags that specify the dynamic light source.
		/// </summary>
		public DynamicLightFlags Flags;
		/// <summary>
		/// World-space position of the light source.
		/// </summary>
		public Vector3 Origin;
		/// <summary>
		/// Effective radius of the area that lit up by the light source.
		/// </summary>
		public float Radius;
		/// <summary>
		/// Color of the light. <see cref="ColorSingle.A"/> component is ignored.
		/// </summary>
		public ColorSingle Color;
		/// <summary>
		/// A factor lightness of the light source.
		/// </summary>
		public float SpecularMultiplier;
		/// <summary>
		/// A set of parameters that define the environment probe.
		/// </summary>
		public EnvironmentProbeProperties EnvProbeProperties;
		/// <summary>
		/// A box to project the light into when the flag <see cref="DynamicLightFlags.BoxProjected"/> is
		/// set in <see cref="Flags"/>.
		/// </summary>
		public LightProjectionBox ProjectionBox;
		/// <summary>
		/// Size of bulb that defines the shape of attenuation.
		/// </summary>
		public float AttenuationBulbSize;
		/// <summary>
		/// Name of the light source.
		/// </summary>
		public string Name;
		/// <summary>
		/// Transformation that is applied to the clip volume.
		/// </summary>
		public Matrix34 ClipBoxTransformation;
		/// <summary>
		/// Defines the fading volume.
		/// </summary>
		public BoundingBox FadeBox;
		/// <summary>
		/// Dimensions of the are the light is lighting up.
		/// </summary>
		public AreaLightDimensions AreaDimensions;
	}
	/// <summary>
	/// Encapsulates a set of parameters that define the environment probe.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EnvironmentProbeProperties
	{
		/// <summary>
		/// A cubemap that affects diffuse texture maps.
		/// </summary>
		public Texture DiffuseCubemap;
		/// <summary>
		/// A cubemap that affects specular texture maps.
		/// </summary>
		public Texture SpecularCubemap;
		/// <summary>
		/// Dimensions of the box that is affected by the cubemaps.
		/// </summary>
		public Vector3 Extents;
		/// <summary>
		/// A value from 0 to 1 that defines when influence of the cubemap starts to fall off.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A value of 0 means that the box shape will have hard edges and there is no falloff (cheaper
		/// performance) .
		/// </para>
		/// <para>
		/// A value of 1 means the falloff will begin at the center of the box and blend out to the box
		/// extents (most expensive on performance).
		/// </para>
		/// <para>A value of 0.8 means the falloff begins at 80% of the extents of the box shape.</para>
		/// </remarks>
		public float AttenuationFallOffMax;
		/// <summary>
		/// A value that define a priority that is used when environment probes overlap.
		/// </summary>
		public byte SortPriority;
	}
	/// <summary>
	/// Encapsulates a set of parameters that define the extents of the box the light is projected into.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LightProjectionBox
	{
		/// <summary>
		/// Width of the box.
		/// </summary>
		public float Width;
		/// <summary>
		/// Height of the box.
		/// </summary>
		public float Height;
		/// <summary>
		/// Length of the box.
		/// </summary>
		public float Length;
	}
	/// <summary>
	/// Encapsulates a set of parameters that define a light projector.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct LightProjectorProperties
	{
		/// <summary>
		/// A map that contains custom light attenuation gradient.
		/// </summary>
		public Texture AttenuationMap;
		/// <summary>
		/// A map that contains the image that is projected by the light source.
		/// </summary>
		public Texture Image;
		/// <summary>
		/// A matrix that defines transformation of the projector.
		/// </summary>
		public Matrix34 ObjectMatrix;
	}
	/// <summary>
	/// Encapsulates dimensions of the area the light source lights up.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct AreaLightDimensions
	{
		/// <summary>
		/// Width of the area.
		/// </summary>
		public float Width;
		/// <summary>
		/// Height of the area.
		/// </summary>
		public float Height;
	}
}