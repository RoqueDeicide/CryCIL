using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Native
{
	internal static class MemoryHandlingInterop
	{
		internal static extern IntPtr AllocateMemory(ulong size);
		internal static extern void FreeMemory(IntPtr handle);
	}
}
