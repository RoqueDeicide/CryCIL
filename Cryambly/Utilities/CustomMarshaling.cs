using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Utilities
{
	/// <summary>
	/// Defines methods for custom data marshaling.
	/// </summary>
	public static class CustomMarshaling
	{
		/// <summary>
		/// Creates managed string from null-terminated UTF-8 string.
		/// </summary>
		/// <param name="stringHandle">Pointer to UTF-8 string.</param>
		/// <returns>Managed string that is created from given UTF-8 string.</returns>
		[CanBeNull]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetUtf8String(IntPtr stringHandle);
	}
}