using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Contains methods useful for tracking down bugs.
	/// </summary>
	public static partial class Debug
	{
		public static void DrawSphere(Vector3 pos, float radius, ColorSingle color, float timeout)
		{
			DebugInterop.AddPersistentSphere(pos, radius, color, timeout);
		}

		public static void DrawDirection(Vector3 pos, float radius, Vector3 dir, ColorSingle color, float timeout)
		{
			DebugInterop.AddDirection(pos, radius, dir, color, timeout);
		}

		public static void DrawText(string text, float size, ColorSingle color, float timeout)
		{
			DebugInterop.AddPersistentText2D(text, size, color, timeout);
		}

		public static void DrawLine(Vector3 startPos, Vector3 endPos, ColorSingle color, float timeout)
		{
			DebugInterop.AddPersistentLine(startPos, endPos, color, timeout);
		}

		public static void DrawBoundingBox(Vector3 pos, BoundingBox bbox, ColorSingle color, float timeout)
		{
			DebugInterop.AddAABB(pos, bbox, color, timeout);
		}
	}
}