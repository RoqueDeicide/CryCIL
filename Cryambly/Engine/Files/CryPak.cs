using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Provides access to CryEngine virtual file system API.
	/// </summary>
	/// <remarks>
	/// <para>
	/// WARNING: All internal path handling code uses only ASCII characters, which means, you should avoid
	///          using absolute paths for anything, since it is entirely possible for a person with
	///          national alphabet to have folders with Unicode characters in their names. Also warn any
	///          mod-makers to only use ASCII characters in folder names within CryEngine installation
	///          directory.
	/// </para>
	/// </remarks>
	public static class CryPak
	{
		#region Fields
		/// <summary>
		/// Max length of the buffer in bytes that can be used to store the file path.
		/// </summary>
		public const int MaxFilePathLength = 260;
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Determines whether the file can be accessed at the specified location using specified path.
		/// </summary>
		/// <param name="path">    Path to the file to look for.</param>
		/// <param name="location">Region of the virtual file system where to look for the file.</param>
		/// <returns>
		/// True, if the file exists inside the specified locale and can be accessed using the given path,
		/// otherwise false.
		/// </returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Exists(string path, SearchLocation location = SearchLocation.Any);
		/// <summary>
		/// Determines whether specified path is a path to the folder.
		/// </summary>
		/// <param name="path">Path to check.</param>
		/// <returns>True, if a folder can be accessed using a given path.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsFolder(string path);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Open(string path, ref uint modeSymbols, FileOpenFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Close(IntPtr file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetSize(IntPtr file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCurrentPosition(IntPtr file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int Seek(IntPtr file, int offset, SeekOrigin origin);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Flush(IntPtr file);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int ReadBytes(IntPtr file, byte[] bytes, int offset, int count);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int WriteBytes(IntPtr file, byte[] bytes, int offset, int count);
		#endregion
	}
}