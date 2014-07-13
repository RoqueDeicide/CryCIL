using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Defines common functionality of types that represent objects
	/// that use unmanaged memory to store the data.
	/// </summary>
	public interface INativeMemoryWrapper : IDisposable
	{
		/// <summary>
		/// When implemented in derived type, gets a pointer to the
		/// first byte of one of arrays of unmanaged memory that are
		/// used by the object.
		/// </summary>
		/// <param name="index">Index of the handle to get.</param>
		/// <returns>
		/// Pointer to the first byte of selected native memory cluster.
		/// </returns>
		IntPtr GetHandle(int index = 0);
		/// <summary>
		/// When implemented in derived type, indicates whether all of
		/// unmanaged memory clusters were freed.
		/// </summary>
		bool Disposed { get; }
		/// <summary>
		/// When implemented in derived type, determines whether
		/// native memory cluster has been freed.
		/// </summary>
		/// <param name="index">
		/// Index of cluster which status needs to be checked.
		/// </param>
		/// <returns>True, if cluster has been freed.</returns>
		bool IsClusterDisposed(int index);
		/// <summary>
		/// When implemented in derived type, gets method of
		/// allocation of memory cluster.
		/// </summary>
		/// <param name="clusterIndex">
		/// Index of cluster which method of allocation we need to know.
		/// </param>
		/// <returns>Method of allocation.</returns>
		Allocators GetAllocationMethod(int clusterIndex);
	}
}