using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class NativeLevelMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetCurrentLevel();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr LoadLevel(string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool IsLevelLoaded();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void UnloadLevel();

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetName(IntPtr levelPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetPath(IntPtr levelPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetPaks(IntPtr levelPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetDisplayName(IntPtr levelPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetHeightmapSize(IntPtr levelPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetGameTypeCount(IntPtr levelPtr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetGameType(IntPtr levelPtr, int index);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool SupportsGameType(IntPtr levelPtr, string gameTypeName);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetDefaultGameType(IntPtr levelPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool HasGameRules(IntPtr levelPtr);
	}
}