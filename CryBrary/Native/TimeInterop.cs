using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class TimeInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeScale(float scale);
	}
}