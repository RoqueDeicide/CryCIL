using System.Runtime.CompilerServices;

namespace CryCil.Interops
{
	internal static class Initialization
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationStartingBind();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationCompleteBind(bool success);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetSubscribedStagesBind();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnInitializationStageBind(int stageIndex);
	}
}
