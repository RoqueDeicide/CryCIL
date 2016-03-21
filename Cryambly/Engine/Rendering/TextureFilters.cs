using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of texture filters.
	/// </summary>
	public enum TextureFilters
	{
		/// <summary>
		/// No filtering.
		/// </summary>
		None = -1,
		/// <summary>
		/// Point filtering.
		/// </summary>
		Point = 0,
		/// <summary>
		/// Linear filtering.
		/// </summary>
		Linear = 1,
		/// <summary>
		/// Bilinear filtering.
		/// </summary>
		Bilinear = 2,
		/// <summary>
		/// Trilinear filtering.
		/// </summary>
		Trilinear = 3,
		/// <summary>
		/// Anisotropic 2X filtering.
		/// </summary>
		Anisotropic2X = 4,
		/// <summary>
		/// Anisotropic 4X filtering.
		/// </summary>
		Anisotropic4X = 5,
		/// <summary>
		/// Anisotropic 8X filtering.
		/// </summary>
		Anisotropic8X = 6,
		/// <summary>
		/// Anisotropic 16X filtering.
		/// </summary>
		Anisotropic16X = 7
	}
}