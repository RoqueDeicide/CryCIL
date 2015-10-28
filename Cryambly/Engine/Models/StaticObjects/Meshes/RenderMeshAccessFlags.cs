using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// A set of flags that specify how to access one of internal render mesh arrays.
	/// </summary>
	[Flags]
	public enum RenderMeshAccessFlags : uint
	{
		/// <summary>
		/// When set, specifies that data will be read.
		/// </summary>
		Read = 0x01,
		/// <summary>
		/// When set, specifies that data will be written.
		/// </summary>
		Write = 0x02,
		/// <summary>
		/// Unkown.
		/// </summary>
		Dynamic = 0x04,
		/// <summary>
		/// When set, specifies that old data must be discarded.
		/// </summary>
		Discard = 0x08,
		/// <summary>
		/// When set, specifies data in video memory is being accessed.
		/// </summary>
		Video = 0x10,
		/// <summary>
		/// When set, specifies data in system memory is being accessed.
		/// </summary>
		System = 0x20,
		/// <summary>
		/// Unknown.
		/// </summary>
		Instanced = 0x40,
		/// <summary>
		/// When set, specifies that map must not stall for VB/IB locking.
		/// </summary>
		NonStallMap = 0x80,
		/// <summary>
		/// When set, specifies to push down from vram on demand if target architecture supports it, used
		/// internally.
		/// </summary>
		VbIbPushdown = 0x100,
		/// <summary>
		/// When set, specifies that VRAM must be accessed directly if target architecture supports it,
		/// used internally.
		/// </summary>
		Direct = 0x200,
		/// <summary>
		/// Internal use.
		/// </summary>
		Locked = 0x400,
		/// <summary>
		/// Combination of flags that is used to create new data stream in system memory.
		/// </summary>
		SystemCreate = (Write | Discard | System),
		/// <summary>
		/// Combination of flags that is used to update data stream in system memory.
		/// </summary>
		SystemUpdate = (Write | System),
		/// <summary>
		/// Combination of flags that is used to create new data stream in video memory.
		/// </summary>
		VideoCreate = (Write | Discard | Video),
		/// <summary>
		/// Combination of flags that is used to update data stream in video memory.
		/// </summary>
		VideoUpdate = (Write | Video)
	}
}