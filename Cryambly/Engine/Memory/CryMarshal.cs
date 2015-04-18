using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Memory
{
	internal static class CryMarshal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr AllocateMemory(ulong size);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr ReallocateMemory(IntPtr ptr, ulong size);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void FreeMemory(IntPtr handle);
	}
}