using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of identifiers of various types of textures.
	/// </summary>
	public enum ResourceTextureTypes
	{
		/// <summary>
		/// Diffuse color map.
		/// </summary>
		Diffuse = 0,
		/// <summary>
		/// Normal map.
		/// </summary>
		Normals,
		/// <summary>
		/// Specular map.
		/// </summary>
		Specular,
		/// <summary>
		/// Environment map.
		/// </summary>
		Environment,
		/// <summary>
		/// Detail overlay map.
		/// </summary>
		DetailOverlay,
		/// <summary>
		/// Translucency map.
		/// </summary>
		Translucency,
		/// <summary>
		/// Height map.
		/// </summary>
		Height,
		/// <summary>
		/// Decal overlay map.
		/// </summary>
		DecalOverlay,
		/// <summary>
		/// Sub-surface scattering map.
		/// </summary>
		SubSurface,
		/// <summary>
		/// Primary custom map.
		/// </summary>
		Custom,
		/// <summary>
		/// Secondary custom map.
		/// </summary>
		CustomSecondary,
		/// <summary>
		/// Opacity map.
		/// </summary>
		Opacity,
		/// <summary>
		/// Smoothness map.
		/// </summary>
		Smoothness,

		/// <summary>
		/// This value serves a number of known types of textures.
		/// </summary>
		Max,
		/// <summary>
		/// Unknown map.
		/// </summary>
		Unknown = Max,

		// NOTE: currently aliases, to get it's own ID and assigned dynamically to a texture slot later

		/// <summary>
		/// Temporary identifier for emission maps.
		/// </summary>
		Emissive = DecalOverlay,
		/// <summary>
		/// Temporary identifier for glow maps.
		/// </summary>
		Glow = DecalOverlay
	}
}