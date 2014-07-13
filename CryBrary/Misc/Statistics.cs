using System;

namespace CryEngine.Utilities
{
	/// <summary>
	/// CryStats enables tracking of commonly used library statistics such as memory usage.
	/// </summary>
	public static class CryStats
	{
		/// <summary>
		/// The current approximate memory usage in megabytes.
		/// </summary>
		public static long MemoryUsageMB
		{
			get { return MemoryUsageKB / 1024; }
		}

		/// <summary>
		/// The current approximate memory usage in kilobytes.
		/// </summary>
		public static long MemoryUsageKB
		{
			get { return MemoryUsage / 1024; }
		}

		/// <summary>
		/// The current approximate memory usage in bytes.
		/// </summary>
		public static long MemoryUsage
		{
			get { return GC.GetTotalMemory(false); }
		}
	}
}