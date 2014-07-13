using System;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace CryEngine.Native
{
	internal static class NativeDebugMethods
	{
		#region Persistent Debug
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void AddPersistentSphere(Vec3 pos, float radius, Color color, float timeout);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void AddDirection(Vec3 pos, float radius, Vec3 dir, Color color, float timeout);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void AddPersistentText2D(string text, float size, Color color, float timeout);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void AddAABB(Vec3 pos, BoundingBox bbox, Color color, float timeout);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void AddPersistentLine(Vec3 pos, Vec3 end, Color color, float timeout);
		#endregion

		#region Profiling
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr CreateFrameProfiler(string methodName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr CreateFrameProfilerSection(IntPtr profiler);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void DeleteFrameProfilerSection(IntPtr profilerSection);
		#endregion

		#region Logging
		[SuppressUnmanagedCodeSecurity]
		[SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0"), DllImport("CryMono.dll")]
		public extern static void Log(string msg, LogType type);

		[SuppressUnmanagedCodeSecurity]
		[SuppressMessage("Microsoft.Globalization", "CA2101:SpecifyMarshalingForPInvokeStringArguments", MessageId = "0"), DllImport("CryMono.dll")]
		public extern static void Warning(string msg);
		#endregion
	}
}