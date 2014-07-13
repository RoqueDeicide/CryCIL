using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	public static class NativeRendererMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetWidth();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetHeight();

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static Vector3 ScreenToWorld(int x, int y);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int UnProjectFromScreen(float sx, float sy, float sz, out float px, out float py, out float pz);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void DrawTextToScreen(float x, float y, float fontSize, Color color, bool center, string text);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int LoadTexture(string path);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void DrawTextureToScreen(float xpos, float ypos, float width, float height, int textureId, float s0 = 0, float t0 = 0, float s1 = 1, float t1 = 1, float angle = 0, float r = 1, float g = 1, float b = 1, float a = 1, float z = 1);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int CreateRenderTarget(int width, int height, int flags);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void DestroyRenderTarget(int id);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetRenderTarget(int id);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetViewCamera();

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetCameraMatrix(IntPtr cameraPtr, Matrix34 matrix);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static Matrix34 GetCameraMatrix(IntPtr cameraPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetCameraPosition(IntPtr cameraPtr, Vector3 pos);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static Vector3 GetCameraPosition(IntPtr cameraPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static float GetCameraFieldOfView(IntPtr cameraPtr);
	}
}