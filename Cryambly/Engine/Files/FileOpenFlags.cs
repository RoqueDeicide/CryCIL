using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of flags that specify how the file is opened when using virtual file system.
	/// </summary>
	public enum FileOpenFlags
	{
		/// <summary>
		/// Nothing special.
		/// </summary>
		Nothing = 0,
		/// <summary>
		/// If possible, will prevent the file from being read from memory.
		/// </summary>
		HintDirectOperation = 1,
		/// <summary>
		/// Will prevent a "missing file" warnings from being created.
		/// </summary>
		HintQuiet = 2,
		/// <summary>
		/// File should be on disk.
		/// </summary>
		OnDisk = 4,
		/// <summary>
		/// Open is done by the streaming thread.
		/// </summary>
		ForStreaming = 8,
		/// <summary>
		/// On supported platforms, file is open in 'locked' mode.
		/// </summary>
		LockedOpen = 16
	}
}
