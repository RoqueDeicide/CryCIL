using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CryEngine.Native
{
	internal static class PlatformMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool AreMeshIndicesInt16();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool AreMeshTangentsSingle();
	}
}