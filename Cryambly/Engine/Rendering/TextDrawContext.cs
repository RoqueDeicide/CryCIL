using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Encapsulates information that specifies how to render text using a specific font.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct TextDrawContext
	{
		#region Fields
		/// <summary>
		/// Index of the section of the Xml definition that defines how the font looks.
		/// </summary>
		public uint EffectIndex;

		/// <summary>
		/// Vector which X coordinate represents width of font and Y - height.
		/// </summary>
		public Vector2 Size;
		/// <summary>
		/// Scale of the width of the font.
		/// </summary>
		public float WidthScale;

		/// <summary>
		/// Rectangle that describes a region the text won't be rendered outside of.
		/// </summary>
		public RectangleF ClippingRegion;

		/// <summary>
		/// Flags that specify how the text is drawn.
		/// </summary>
		public TextRenderOptions DrawTextFlags;

		/// <summary>
		/// Indication whether the text is rendered proportionally.
		/// </summary>
		public bool Proportional;
		/// <summary>
		/// Indication whether the text size is determined using 800x600 virtual resolution.
		/// </summary>
		public bool SizeIn800x600;
		/// <summary>
		/// Indication whether the text clipping is enabled.
		/// </summary>
		public bool ClippingEnabled;
		/// <summary>
		/// Indication whether the text is framed using a rectangle with special background for extra
		/// readability.
		/// </summary>
		public bool Framed;

		/// <summary>
		/// Overriding color of the text.
		/// </summary>
		public ColorByte ColorOverride;
		#endregion
	}
}