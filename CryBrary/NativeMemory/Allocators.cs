using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.NativeMemory
{
	/// <summary>
	/// Enumeration of ways, how native memory can be allocated. Each
	/// requires a specific way of releasing.
	/// </summary>
	public enum Allocators
	{
		/// <summary>
		/// Memory has been allocated by <see cref="CryMarshal" /> class.
		/// </summary>
		/// <remarks>
		/// Memory allocated by calling <see
		/// cref="CryMarshal.AllocateMemory" /> is tracked by <see
		/// cref="CryMarshal" /> class, releasing memory by means
		/// other then calling <see cref="CryMarshal.FreeMemory" />
		/// will cause <see cref="CryMarshal" /> to believe that
		/// memory is still available, which may cause errors.
		/// </remarks>
		CryMarshal,
		/// <summary>
		/// Memory has been allocated by <see cref="CryModule" />.
		/// </summary>
		/// <remarks>
		/// Memory that has been allocated by calling <see
		/// cref="CryModule.AllocateMemory" /> is not tracked by
		/// anything by default. You must release it manually either
		/// by calling <see cref="CryModule.FreeMemory" /> or by
		/// releasing it from C++ code.
		/// </remarks>
		CryModule,
		/// <summary>
		/// Memory has been allocated within Mono subsystem.
		/// </summary>
		/// <remarks>
		/// This memory must be freed by <see
		/// cref="MonoMemory.FreeMemory" />.
		/// </remarks>
		Mono
	}
}