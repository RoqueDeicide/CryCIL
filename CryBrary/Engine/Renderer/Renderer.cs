using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;
using CryEngine.Native;

namespace CryEngine
{
	public static class Renderer
	{
		/// <summary>
		/// Gets the width of the screen in pixels.
		/// </summary>
		public static int ScreenHeight { get { return RendererInterop.GetHeight(); } }

		/// <summary>
		/// Gets the height of the screen in pixels.
		/// </summary>
		public static int ScreenWidth { get { return RendererInterop.GetWidth(); } }

		public static Vector3 ScreenToWorld(int x, int y)
		{
			return RendererInterop.ScreenToWorld(x, y);
		}

		public static int UnProjectFromScreen(float sx, float sy, float sz, out float px, out float py, out float pz)
		{
			return RendererInterop.UnProjectFromScreen(sx, sy, sz, out px, out py, out pz);
		}

		public static void DrawTextToScreen(float x, float y, float fontSize, ColorSingle color, string text, params object[] args)
		{
			DrawTextToScreen(x, y, fontSize, color, false, text, args);
		}

		public static void DrawTextToScreen(float x, float y, float fontSize, ColorSingle color, bool centered, string text, params object[] args)
		{
			RendererInterop.DrawTextToScreen(x, y, fontSize, color, centered, string.Format(text, args));
		}

		/// <summary>
		/// Loads a texture into memory and generates its ID.
		/// </summary>
		/// <param name="texturePath">The path to the texture.</param>
		/// <returns>Texture id if successful, otherwise -1.</returns>
		public static int LoadTexture(string texturePath)
		{
			return RendererInterop.LoadTexture(texturePath);
		}

		/// <summary>
		/// Draws a texture to the screen for one frame.
		/// </summary>
		/// <param name="x">The x position on the screen, relative to the top-left corner.</param>
		/// <param name="y">The y position on the screen, relative to the top-left corner.</param>
		/// <param name="width">The width to render the texture at.</param>
		/// <param name="height">The height to render the texture at.</param>
		/// <param name="textureId">
		/// The ID of the texture (obtained using Renderer.LoadTexture(path)).
		/// </param>
		public static void DrawTexture(int x, int y, int width, int height, int textureId)
		{
			RendererInterop.DrawTextureToScreen(x, y, width, height, textureId);
		}

		public static int CreateRenderTarget(int width, int height, int flags = 0)
		{
			return RendererInterop.CreateRenderTarget(width, height, flags);
		}

		public static void SetRenderTarget(int id)
		{
			RendererInterop.SetRenderTarget(id);
		}

		public static void DestroyRenderTarget(int id)
		{
			RendererInterop.DestroyRenderTarget(id);
		}
	}
}