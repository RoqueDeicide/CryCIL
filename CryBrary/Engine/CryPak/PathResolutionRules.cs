using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[CLSCompliant(false)]
	[Flags]
	public enum PathResolutionRules : uint
	{
		/// <summary>
		/// If used, the source path will be treated as the destination path and no transformations
		/// will be done. Pass this flag when the path is to be the actual path on the disk/in the
		/// packs and doesn't need adjustment (or after it has come through adjustments already) if
		/// this is set, AdjustFileName will not map the input path into the master folder (Ex:
		/// Shaders will not be converted to Game\Shaders)
		/// </summary>
		RealPath = 1 << 16,

		/// <summary>
		/// AdjustFileName will always copy the file path to the destination path: regardless of the
		/// returned value, szDestpath can be used
		/// </summary>
		AlwaysCopyDestination = 1 << 17,

		/// <summary>
		/// Adds trailing slash to the path
		/// </summary>
		AddTrailingSlash = 1 << 18,

		/// <summary>
		/// if this is set, AdjustFileName will not make relative paths into full paths
		/// </summary>
		NoFullPath = 1 << 21,

		/// <summary>
		/// if this is set, AdjustFileName will redirect path to disc
		/// </summary>
		RedirectToDisc = 1 << 22,

		/// <summary>
		/// if this is set, AdjustFileName will not adjust path for writing files
		/// </summary>
		ForWriting = 1 << 23,

		/// <summary>
		/// if this is set, AdjustFileName will not convert the path to low case
		/// </summary>
		NoLowerCase = 1 << 24,

		/// <summary>
		/// if this is set, the pak would be stored in memory
		/// </summary>
		PakInMemory = 1 << 25,

		/// <summary>
		/// Store all file names as crc32 in a flat directory structure.
		/// </summary>
		FileNamesAsCrc32 = 1 << 26,

		/// <summary>
		/// if this is set, AdjustFileName will try to find the file under any mod paths we know about
		/// </summary>
		CheckModPaths = 1 << 27,

		/// <summary>
		/// if this is set, AdjustFileName will always check the filesystem/disk and not check
		/// inside open paks
		/// </summary>
		NeverInPaks = 1 << 28,
	}
}