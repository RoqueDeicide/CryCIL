using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Represents a wrapper around a standard null-terminated string in native memory.
	/// </summary>
	public class NativeString
	{
		#region ScriptBinds
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetMonoString(IntPtr value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetMonoWideString(IntPtr value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetNativeString(string value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetNativeWideString(string value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void FreeStringMemory(IntPtr pointer);
		#endregion
		/// <summary>
		/// Converts a C++ null terminated wchar_t string to its .NET counter part.
		/// </summary>
		/// <param name="pointer">Pointer to the first symbol.</param>
		/// <param name="length">Length of the string excluding terminal.</param>
		/// <param name="symbolSize">Size of one symbol in bytes.</param>
		/// <returns>New string.</returns>
		public static string FromWideString(IntPtr pointer, int length, int symbolSize)
		{
			Contract.Requires(pointer != null && length >= 0 && symbolSize > 0);
			if (length == 0)
			{
				return "";
			}
			NativeArray nativeStringBytes = new NativeArray(pointer, (uint)(length * symbolSize), false, false);
			switch (symbolSize)
			{
				case 1:
					return Encoding.ASCII.GetString(nativeStringBytes.ToMonoArray());
				case 2:
					return Encoding.Unicode.GetString(nativeStringBytes.ToMonoArray());
				case 4:
					return Encoding.UTF32.GetString(nativeStringBytes.ToMonoArray());
				default:
					Debug.LogWarning(String.Format("NativeString.FromWideString: Attempt to use an unknown encoding string. Symbol size is {0}", symbolSize.ToString()));
					break;
			}
			return null;
		}
		/// <summary>
		/// Writes given string as a null terminated wchar_t sequence.
		/// </summary>
		/// <param name="text">Text to write.</param>
		/// <returns>Pointer to the first symbol.</returns>
		public static NativeArray ToWideString(string text)
		{
			Contract.Requires(text != null);
			// We use UTF-16, so length of byte array is number of symbols * 2 + 2 bytes for
			// terminating symbol.
			byte[] bytes = new byte[text.Length * 2 + 2];
			// Copy bytes of string into byte buffer.
			Encoding.Unicode.GetBytes(text).CopyTo(bytes, 0);
			// Initialize terminator just in case.
			bytes[bytes.Length - 2] = 0;
			bytes[bytes.Length - 1] = 0;
			// Write to native memory.
			return new NativeArray(bytes);
		}
	}
}