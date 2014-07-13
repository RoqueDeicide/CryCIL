using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class NativeNetworkMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static bool IsServer();
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static bool IsClient();
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static bool IsMultiplayer();
	}
}