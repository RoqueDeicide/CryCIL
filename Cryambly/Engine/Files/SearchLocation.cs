using System;
using System.Linq;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of locations where files can be searched for in the virtual file system.
	/// </summary>
	public enum SearchLocation
	{
		/// <summary>
		/// Files will be looked for anywhere inside the virtual file system.
		/// </summary>
		Any,
		/// <summary>
		/// Files will be looked for on the HDD only.
		/// </summary>
		Disk,
		/// <summary>
		/// Files will be looked for inside .pak files only.
		/// </summary>
		Pak
	}
}