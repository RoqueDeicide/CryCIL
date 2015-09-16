using System;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of flags that can be used to specify how internal path resolution system should process
	/// paths.
	/// </summary>
	[Flags]
	public enum PathResolutionRules
	{
		/// <summary>
		/// When set, the source path will not be transformed. Use it when the transformation already
		/// happened before.
		/// </summary>
		/// <remarks>
		/// <para>Original CryEngine documentation:</para>
		/// <para>
		/// If used, the source path will be treated as the destination path and no transformations will be
		/// done. Pass this flag when the path is to be the actual path on the disk/in the packs and
		/// doesn't need adjustment (or after it has come through adjustments already) if this is set,
		/// AdjustFileName will not map the input path into the master folder (Ex: Shaders will not be
		/// converted to Game\Shaders)
		/// </para>
		/// </remarks>
		PathReal = 1 << 16,
		/// <summary>
		/// This flag makes no sense in C#.
		/// </summary>
		/// <remarks>
		/// <para>Original CryEngine documentation:</para>
		/// <para>
		/// AdjustFileName will always copy the file path to the destination path: regardless of the
		/// returned value, szDestpath can be used
		/// </para>
		/// </remarks>
		AlwaysCopyToDestination = 1 << 17,
		/// <summary>
		/// When set, instructs path resolution system to make sure that resultant path always ends with a
		/// slash '/'.
		/// </summary>
		AddTrailingSlash = 1 << 18,
		/// <summary>
		/// When set, instructs path resolution system to not make relative paths into full paths.
		/// </summary>
		NoFullPath = 1 << 21,
		/// <summary>
		/// When set, instructs path resolution system to redirect path to disc.
		/// </summary>
		RedirectToDisc = 1 << 22,
		/// <summary>
		/// When set, instructs path resolution system to not adjust path for writing files.
		/// </summary>
		ForWriting = 1 << 23,
		/// <summary>
		/// When set, instructs path resolution system to not convert the path to lower case.
		/// </summary>
		NoLowcase = 1 << 24,
		/// <summary>
		/// When set, indicates that a .pak file should be stored in the memory (GPU).
		/// </summary>
		/// <remarks>This flag is used when opening packs.</remarks>
		PakInMemory = 1 << 25,
		/// <summary>
		/// When set, instructs pack loading system to store all file names as crc32 in a flat directory
		/// structure.
		/// </summary>
		/// <remarks>This flag is used when opening packs.</remarks>
		FilenamesAsCrc32 = 1 << 26,
		/// <summary>
		/// When set, instructs path resolution system to check any known mod folder paths.
		/// </summary>
		CheckModPaths = 1 << 27,
		/// <summary>
		/// When set, instructs path resolution system to not check opened packs.
		/// </summary>
		NeverInPak = 1 << 28,
		/// <summary>
		/// When set, instructs path resolution system to use cached file names.
		/// </summary>
		/// <remarks>Used by resource compiler.</remarks>
		ResolveToCache = 1 << 29,
		/// <summary>
		/// When set, indicates that the pack should be stored in CPU cache memory.
		/// </summary>
		/// <remarks>This flag is used when opening packs.</remarks>
		PakInMemoryCpu = 1 << 30
	}
}