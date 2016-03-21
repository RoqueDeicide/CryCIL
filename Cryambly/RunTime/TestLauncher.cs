using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.RunTime
{
	internal static class TestLauncher
	{
		internal static void StartTesting()
		{
			try
			{
				Test();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
				Process.GetCurrentProcess().Kill();
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Test();
	}
}