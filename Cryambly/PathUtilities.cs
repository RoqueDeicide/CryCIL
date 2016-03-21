using System;
using System.IO;
using System.Linq;
using System.Security;

namespace CryCil
{
	/// <summary>
	/// Defines functions for working with paths.
	/// </summary>
	public static class PathUtilities
	{
		/// <summary>
		/// Creates absolute path based on given one.
		/// </summary>
		/// <param name="path">          Path to use.</param>
		/// <param name="fallbackFolder">
		/// Folder path to attach to <paramref name="path"/> if it's not absolute.
		/// </param>
		/// <returns>Absolute path.</returns>
		/// <exception cref="ArgumentException">
		/// <paramref name="path"/> is a zero-length string, contains only white space, or contains one or
		/// more of the invalid characters defined in
		/// <see cref="M:System.IO.Path.GetInvalidPathChars"/>.-or- The system could not retrieve the
		/// absolute path.
		/// </exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
		/// <exception cref="NotSupportedException">
		/// <paramref name="path"/> contains a colon (":") that is not part of a volume identifier (for
		/// example, "c:\").
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length. For example, on
		/// Windows-based platforms, paths must be less than 248 characters, and file names must be less
		/// than 260 characters.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the required permissions.
		/// </exception>
		public static string ToAbsolute(string path, string fallbackFolder)
		{
			return Path.IsPathRooted(path) ? path : Path.GetFullPath(Path.Combine(fallbackFolder, path));
		}
		/// <summary>
		/// Determines whether paths point at the same file or directory.
		/// </summary>
		/// <param name="path1">First path.</param>
		/// <param name="path2">Second path.</param>
		/// <returns>True, if paths point at the same file or directory, otherwise false.</returns>
		/// <exception cref="ArgumentException">
		/// One of the paths is a zero-length string, contains only white space, or contains one or more of
		/// the invalid characters defined in <see cref="M:System.IO.Path.GetInvalidPathChars"/>.-or- The
		/// system could not retrieve the absolute path.
		/// </exception>
		/// <exception cref="ArgumentNullException">One of the paths is null.</exception>
		/// <exception cref="NotSupportedException">
		/// One of the paths contains a colon (":") that is not part of a volume identifier (for example,
		/// "c:\").
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// The specified path, file name, or both exceed the system-defined maximum length. For example, on
		/// Windows-based platforms, paths must be less than 248 characters, and file names must be less
		/// than 260 characters.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the required permissions.
		/// </exception>
		public static bool Equal(string path1, string path2)
		{
			return Path.GetFullPath(path1) == Path.GetFullPath(path2);
		}
	}
}