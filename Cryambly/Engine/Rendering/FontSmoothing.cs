using System;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of flags that specify how the font is smoothed.
	/// </summary>
	public enum FontSmoothing : uint
	{
		/// <summary>
		/// Specifies that the font is not smoothed at all.
		/// </summary>
		NoSmoothing = 0x00000000,
		/// <summary>
		/// Specifies that the font is smoothed by blurring.
		/// </summary>
		Blur = 0x00000001,
		/// <summary>
		/// Specifies that the font is smoothed by rendering the characters into a bigger texture, and then
		/// shrinking it to the normal size using bilinear filtering.
		/// </summary>
		SuperSampling = 0x00000002,

		/// <summary>
		/// Specifies double smoothing amount.
		/// </summary>
		Amount2X = 0x00010000,
		/// <summary>
		/// Specifies quadruple smoothing amount.
		/// </summary>
		Amount4X = 0x00020000
	}
}