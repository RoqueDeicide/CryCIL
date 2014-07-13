using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines methods for working with memory from Mono subsystem.
	/// </summary>
	public static class MonoMemory
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void FreeMonoMemory(IntPtr pointer);
		/// <summary>
		/// Frees memory that has been allocated by Mono subsystem.
		/// </summary>
		/// <param name="pointer"></param>
		public static void FreeMemory(IntPtr pointer)
		{
			FreeMonoMemory(pointer);
		}
	}
}