using System;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of preprocessing options that can be assigned to shaders that require it.
	/// </summary>
	[Flags]
	public enum ShaderPreprocessFlags : uint
	{
		/// <summary>
		/// Unknown.
		/// </summary>
		First = 25,
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanCmId = 25,
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanCm = (1 << (int)ScanCmId),
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanTextureWaterId = 26,
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanTextureWater = (1 << (int)ScanTextureWaterId),
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanTextureId = 27,
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanTexture = (1 << (int)ScanTextureId),
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanLcmId = 28,
		/// <summary>
		/// Unknown.
		/// </summary>
		ScanLcm = (1 << (int)ScanLcmId),
		/// <summary>
		/// Unknown.
		/// </summary>
		GenerateSpritesId = 29,
		/// <summary>
		/// Unknown.
		/// </summary>
		GenerateSprites = (1 << (int)GenerateSpritesId),
		/// <summary>
		/// Unknown.
		/// </summary>
		CustomTextureId = 30,
		/// <summary>
		/// Unknown.
		/// </summary>
		CustomTexture = (1 << (int)CustomTextureId),
		/// <summary>
		/// Unknown.
		/// </summary>
		GenerateCloudsId = 31,
		/// <summary>
		/// Unknown.
		/// </summary>
		GenerateClouds = 1u << 31
	}
}
