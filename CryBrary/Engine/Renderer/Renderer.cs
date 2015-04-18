using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;
using CryEngine.Native;

namespace CryEngine
{
	public static class Renderer
	{

		public static Vector3 ScreenToWorld(int x, int y)
		{
			return RendererInterop.ScreenToWorld(x, y);
		}

		public static int UnProjectFromScreen(float sx, float sy, float sz, out float px, out float py, out float pz)
		{
			return RendererInterop.UnProjectFromScreen(sx, sy, sz, out px, out py, out pz);
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