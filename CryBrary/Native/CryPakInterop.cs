using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class CryPakInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static string GetGameFolder();
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetGameFolder(string folder);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetAlias(string name, string alias, bool bAdd);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static string GetAlias(string name, bool returnSame = true);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static string AdjustFileName(string source, PathResolutionRules ruleFlags);
	}
}