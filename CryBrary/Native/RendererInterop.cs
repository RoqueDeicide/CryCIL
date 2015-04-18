using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;

namespace CryEngine.Native
{
	public static class RendererInterop
	{

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static Vector3 ScreenToWorld(int x, int y);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int UnProjectFromScreen(float sx, float sy, float sz, out float px, out float py, out float pz);
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