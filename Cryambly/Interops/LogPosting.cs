using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Interops
{
	internal static class LogPosting
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Post(int postType, string text);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetVerboxity();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetVerbosity(int level);
	}
}