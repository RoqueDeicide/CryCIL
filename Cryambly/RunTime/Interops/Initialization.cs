using System.Runtime.CompilerServices;

namespace CryCil.RunTime.Interops
{
	internal class Initialization
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationStarting();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnCompilationComplete(bool success);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int[] GetSubscribedStages();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void OnInitializationStage(int stageIndex);
	}
}
