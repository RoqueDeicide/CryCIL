using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.Engine.DebugServices;
using CryCil.Engine.Rendering;
using CryCil.Geometry;
using CryCil.Graphics;

namespace CryCil.Engine.UserInterface
{
	/// <summary>
	/// Provides functionality for 2D rendering that can be used for creating graphical user interface.
	/// </summary>
	public static unsafe class UiRenderer
	{
		#region Fields
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Called when UI rendering needs to start.
		/// </summary>
		/// <remarks>
		/// Always call this method when you are about to start using <see cref="UiRenderer"/>
		/// functionality and always call <see cref="Finish"/> method after you are done.
		/// <para>This function is equivalent of UiDraw::PreRender in CryAction module.</para>
		/// </remarks>
		public static void Start()
		{
			Renderer.CullingMode = CullMode.Disabled;
			Renderer.Enable2DMode(Renderer.NativeWidth, Renderer.NativeHeight);
			Renderer.SetColorOperation(ColorOperation.Modulate, ColorOperation.Modulate,
									   ColorArgument.TexArg0, ColorArgument.TexArg1);
			Renderer.State = RenderState.GS_BLSRC_SRCALPHA | RenderState.GS_BLDST_ONEMINUSSRCALPHA | RenderState.GS_NODEPTHTEST;
		}
		/// <summary>
		/// Called when UI rendering is complete.
		/// </summary>
		/// <remarks>
		/// Always call <see cref="Start"/> when you are about to start using <see cref="UiRenderer"/>
		/// functionality and always call this method after you are done.
		/// <para>This function is equivalent of UiDraw::PostRender in CryAction module.</para>
		/// </remarks>
		public static void Finish()
		{
			Renderer.Disable2DMode();
		}
		/// <summary>
		/// Draws a line on the screen.
		/// </summary>
		/// <param name="start">Coordinates of the first point of the line.</param>
		/// <param name="end">  Coordinates of the second point of the line.</param>
		/// <param name="color">Color of the line.</param>
		public static void DrawLine(Vector2 start, Vector2 end, ColorByte color)
		{
			VertexPosition3FColor4BTex2F* vertices = stackalloc VertexPosition3FColor4BTex2F[2];

			const float offset = -0.5f;

			vertices[0].Color = color;
			vertices[1].Color = color;

			vertices[0].Position = new Vector3(start.X + offset, start.Y + offset, 0);
			vertices[1].Position = new Vector3(end.X + offset, end.Y + offset, 0);

			vertices[0].TextureCoordinates = Vector2.Zero;
			vertices[1].TextureCoordinates = new Vector2(1);

			long indexes = 0x00010000;

			Renderer.DrawDynamicVertexBuffer
			(
				vertices, 2,
				(ushort*)&indexes, 2,
				PublicRenderPrimitiveType.LineList
			);
		}
		/// <summary>
		/// Draws a triangle on the screen.
		/// </summary>
		/// <param name="p0">       Coordinates of the first point of the triangle on the screen.</param>
		/// <param name="p1">       Coordinates of the second point of the triangle on the screen.</param>
		/// <param name="p2">       Coordinates of the third point of the triangle on the screen.</param>
		/// <param name="color">    Color of the triangle.</param>
		/// <param name="tc0">      Coordinates of the first point of the triangle on the texture.</param>
		/// <param name="tc1">      Coordinates of the second point of the triangle on the texture.</param>
		/// <param name="tc2">      Coordinates of the third point of the triangle on the texture.</param>
		/// <param name="textureId">
		/// Identifier of the texture to use. If -1 default white texture will be used.
		/// </param>
		public static void DrawTriangle(Vector2 p0, Vector2 p1, Vector2 p2, ColorByte color,
										Vector2 tc0 = new Vector2(), Vector2 tc1 = new Vector2(),
										Vector2 tc2 = new Vector2(), int textureId = -1)
		{
			VertexPosition3FColor4BTex2F* vertices = stackalloc VertexPosition3FColor4BTex2F[3];

			const float offset = -0.5f;

			vertices[0].Color = color;
			vertices[1].Color = color;
			vertices[2].Color = color;

			vertices[0].Position = new Vector3(p0.X + offset, p0.Y + offset, 0);
			vertices[1].Position = new Vector3(p1.X + offset, p1.Y + offset, 0);
			vertices[2].Position = new Vector3(p2.X + offset, p2.Y + offset, 0);

			vertices[0].TextureCoordinates = tc0;
			vertices[1].TextureCoordinates = tc1;
			vertices[2].TextureCoordinates = tc2;

			SetTexture(textureId);

			long indexes = 0x000200010000;

			Renderer.DrawDynamicVertexBuffer
			(
				vertices, 3,
				(ushort*)&indexes, 3,
				PublicRenderPrimitiveType.TriangleList
			);
		}
		/// <summary>
		/// Draws a quad on the screen.
		/// </summary>
		/// <param name="p0"> Position of the first point of the quad in screen space.</param>
		/// <param name="p1"> Position of the second point of the quad in screen space.</param>
		/// <param name="p2"> Position of the third point of the quad in screen space.</param>
		/// <param name="p3"> Position of the fourth point of the quad in screen space.</param>
		/// <param name="t0"> Position of the first point of the quad in texture space.</param>
		/// <param name="t1"> Position of the second point of the quad in texture space.</param>
		/// <param name="t2"> Position of the third point of the quad in texture space.</param>
		/// <param name="t3"> Position of the fourth point of the quad in texture space.</param>
		/// <param name="c0"> Color of the first point of the quad.</param>
		/// <param name="c1"> Color of the second point of the quad.</param>
		/// <param name="c2"> Color of the third point of the quad.</param>
		/// <param name="c3"> Color of the fourth point of the quad.</param>
		/// <param name="tId">Identifier of the texture to use or -1 for default white.</param>
		public static void DrawQuad(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3,
									Vector2 t0, Vector2 t1, Vector2 t2, Vector2 t3,
									ColorByte c0, ColorByte c1, ColorByte c2, ColorByte c3,
									int tId = -1)
		{
			VertexPosition3FColor4BTex2F* vertices = stackalloc VertexPosition3FColor4BTex2F[4];

			const float offset = -0.5f;

			vertices[0].Color = c0;
			vertices[1].Color = c1;
			vertices[2].Color = c2;
			vertices[3].Color = c3;

			vertices[0].Position = new Vector3(p0.X + offset, p0.Y + offset, 0);
			vertices[1].Position = new Vector3(p1.X + offset, p1.Y + offset, 0);
			vertices[2].Position = new Vector3(p2.X + offset, p2.Y + offset, 0);
			vertices[3].Position = new Vector3(p3.X + offset, p3.Y + offset, 0);

			vertices[0].TextureCoordinates = t0;
			vertices[1].TextureCoordinates = t1;
			vertices[2].TextureCoordinates = t2;
			vertices[3].TextureCoordinates = t3;

			SetTexture(tId);

			long indexes = 0x0003000200010000;

			Renderer.DrawDynamicVertexBuffer
			(
				vertices, 4,
				(ushort *)&indexes, 4,
				PublicRenderPrimitiveType.TriangleStrip
			);
		}
		/// <summary>
		/// Draws a rectangle on the screen.
		/// </summary>
		/// <remarks>
		/// Order of vertices:
		/// <code>
		/// 0--1
		/// |\ |
		/// | \|
		/// 2--3
		/// </code>
		/// </remarks>
		/// <param name="rect2D">
		/// <see cref="RectangleF"/> object that describes location and dimensions of the rectangle in
		/// screen space.
		/// </param>
		/// <param name="color"> Color of the rectangle.</param>
		/// <param name="tId">   Identifier of the texture to use or -1 for default white.</param>
		public static void DrawRectangle(RectangleF rect2D, ColorByte color, int tId = -1)
		{
			Vector2 pos = rect2D.Location;
			float width = rect2D.Width;
			float height = rect2D.Height;

			DrawQuad
			(
				pos,										// Top left.
				new Vector2(pos.X + width, pos.Y),			// Top right.
				new Vector2(pos.X, pos.Y + height),			// Bottom left.
				new Vector2(pos.X + width, pos.Y + height),	// Bottom right.

				Vector2.Zero, new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1),

				color, color, color, color,
				tId
			);
		}
		/// <summary>
		/// Draws a rectangle on the screen.
		/// </summary>
		/// <remarks>
		/// Order of vertices:
		/// <code>
		/// 0--1
		/// |\ |
		/// | \|
		/// 2--3
		/// </code>
		/// </remarks>
		/// <param name="rect2D">
		/// <see cref="RectangleF"/> object that describes location and dimensions of the rectangle in
		/// screen space.
		/// </param>
		/// <param name="rectUV">
		/// <see cref="RectangleF"/> object that describes location and dimensions of the rectangle in
		/// texture space.
		/// </param>
		/// <param name="color"> Color of the rectangle.</param>
		/// <param name="tId">   Identifier of the texture to use or -1 for default white.</param>
		public static void DrawRectangle(RectangleF rect2D, RectangleF rectUV, ColorByte color, int tId = -1)
		{
			Vector2 p2D = rect2D.Location;
			float width2D = rect2D.Width;
			float height2D = rect2D.Height;

			Vector2 pUv = rectUV.Location;
			float widthUv = rectUV.Width;
			float heightUv = rectUV.Height;

			DrawQuad
			(
				p2D,											// Top left.
				new Vector2(p2D.X + width2D, p2D.Y),			// Top right.
				new Vector2(p2D.X, p2D.Y + height2D),			// Bottom left.
				new Vector2(p2D.X + width2D, p2D.Y + height2D),	// Bottom right.

				pUv,											// Top left.
				new Vector2(pUv.X + widthUv, pUv.Y),			// Top right.
				new Vector2(pUv.X, pUv.Y + heightUv),			// Bottom left.
				new Vector2(pUv.X + widthUv, pUv.Y + heightUv),	// Bottom right.

				color, color, color, color,
				tId
			);
		}
		#endregion
		#region Utilities
		private static void SetTexture(int tId = -1)
		{
			if (tId == -1)
			{
				Renderer.SetWhiteTexture();
			}
			else
			{
				Renderer.SetTexture(tId);
			}
		}
		#endregion
	}
}