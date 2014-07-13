using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	internal static class NativeTimeMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetTimeScale(float scale);
	}
}