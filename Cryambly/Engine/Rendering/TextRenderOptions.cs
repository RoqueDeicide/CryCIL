using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of options that specify how the text can be rendered.
	/// </summary>
	[Flags]
	public enum TextRenderOptions
	{
		/// <summary>
		/// No options are specified.
		/// </summary>
		Nothing,
		/// <summary>
		/// When specified, indicates that the text should have center horizontal alignment. Otherwise right
		/// or left alignment will be used.
		/// </summary>
		/// <remarks>
		/// When both <see cref="AlignmentCentered"/> and <see cref="AlignmentRight"/> are not specified
		/// left horizontal alignment is used.
		/// </remarks>
		AlignmentCentered = 1 << 0,
		/// <summary>
		/// When specified, indicates that the text should have right horizontal alignment. Otherwise center
		/// or left alignment will be used.
		/// </summary>
		/// <remarks>
		/// When both <see cref="AlignmentCentered"/> and <see cref="AlignmentRight"/> are not specified
		/// left horizontal alignment is used.
		/// </remarks>
		AlignmentRight = 1 << 1,
		/// <summary>
		/// When specified, indicates that the text should have center vertical alignment. Otherwise bottom
		/// or top alignment will be used.
		/// </summary>
		/// <remarks>
		/// When both <see cref="AlignmentCenteredVertically"/> and <see cref="AlignmentBottom"/> are not
		/// specified top alignment is used.
		/// </remarks>
		AlignmentCenteredVertically = 1 << 2,
		/// <summary>
		/// When specified, indicates that the text should have bottom vertical alignment. Otherwise
		/// vertical center or top alignment will be used.
		/// </summary>
		/// <remarks>
		/// When both <see cref="AlignmentCenteredVertically"/> and <see cref="AlignmentBottom"/> are not
		/// specified top alignment is used.
		/// </remarks>
		AlignmentBottom = 1 << 3,

		/// <summary>
		/// When specified, indicates that <see cref="Vector3.X"/> and <see cref="Vector3.Y"/> coordinates
		/// of provided <see cref="Vector3"/> object are specifying a position on the screen, otherwise that
		/// object specifies a position in 3D world space.
		/// </summary>
		OnScreen = 1 << 4,

		/// <summary>
		/// When specified, indicates that font size is specified in actual pixel resolution.
		/// </summary>
		/// <remarks>
		/// It's not known what happens when both <see cref="FixedSize"/> and <see cref="In800x600"/> are
		/// either set or not.
		/// </remarks>
		FixedSize = 1 << 5,
		/// <summary>
		/// When specified, indicates that font size is specified in virtual 800x600 resolution pixels.
		/// </summary>
		/// <remarks>
		/// It's not known what happens when both <see cref="FixedSize"/> and <see cref="In800x600"/> are
		/// either set or not.
		/// </remarks>
		In800x600 = 1 << 6,

		/// <summary>
		/// When specified, indicates that text should be rendered with monospaced font.
		/// </summary>
		Monospace = 1 << 7,

		/// <summary>
		/// When specified, indicates that text should be rendered with a transparent rectangle behind it to
		/// ease readability independent from the background.
		/// </summary>
		Framed = 1 << 8,

		/// <summary>
		/// When specified, indicates that the text should be occluded by world geometry using the depth
		/// buffer.
		/// </summary>
		/// <remarks>There isn't much sense in using this option with <see cref="OnScreen"/>.</remarks>
		DepthTest = 1 << 9,
		/// <summary>
		/// When specified, indicates that overscan borders should be ignored.
		/// </summary>
		/// <remarks>
		/// This option ensures that the text will be rendered in a very specific location.
		/// </remarks>
		IgnoreOverscan = 1 << 10,
	}
}