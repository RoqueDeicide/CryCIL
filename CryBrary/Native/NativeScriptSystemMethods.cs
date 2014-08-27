using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CryEngine.Native
{
	internal static class NativeScriptSystemMethods
	{
		/// <summary>
		/// Revert the last script reload attempt.
		/// </summary>
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void RevertAppDomain();
		/// <summary>
		/// Attempt to reload scripts again
		/// </summary>
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ReloadAppDomain();
	}
}