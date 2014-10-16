using System.IO;

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
		public static string ToAbsolute(string path, string fallbackFolder)
		{
			return Path.IsPathRooted(path) ? path : Path.GetFullPath(Path.Combine(fallbackFolder, path));
		}
		/// <summary>
		/// Determines whether paths point at the same file or directory.
		/// </summary>
		/// <param name="path1">First path.</param>
		/// <param name="path2">Second path.</param>
		/// <returns>
		/// True, if paths point at the same file or directory, otherwise false.
		/// </returns>
		public static bool Equal(string path1, string path2)
		{
			return Path.GetFullPath(path1) == Path.GetFullPath(path2);
		}
	}
}