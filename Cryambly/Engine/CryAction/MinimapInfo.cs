using System;
using System.Runtime.InteropServices;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Encapsulates information about the minimap.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct MinimapInfo
	{
		/// <summary>
		/// Gets the name of the .dds file that contains the texture that is used as a minimap.
		/// </summary>
		public readonly string FileName;
		/// <summary>
		/// Width of the minimap in pixels.
		/// </summary>
		public int Width;
		/// <summary>
		/// Height of the minimap in pixels.
		/// </summary>
		public int Height;
		/// <summary>
		/// Coordinates of the top left corner of 2D level region where this minimap should be active.
		/// </summary>
		public Vector2 Start;
		/// <summary>
		/// Coordinates of the bottom right corner of 2D level region where this minimap should be active.
		/// </summary>
		public Vector2 End;
		/// <summary>
		/// Width and height of the minimap region.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Width is represented by the <see cref="Vector2.X"/> component and is equal to 1 if <c>End.X -
		/// Start.X &lt; 0;</c>.
		/// </para>
		/// <para>
		/// Height is represented by the <see cref="Vector2.Y"/> component and is equal to 1 if <c>End.Y -
		/// Start.Y &lt; 0;</c>.
		/// </para>
		/// </remarks>
		public Vector2 Dimensions;
	}
}