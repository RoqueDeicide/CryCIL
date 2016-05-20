using System;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Enumeration of flags that can be used to colorize the frame profiling sections.
	/// </summary>
	[Flags]
	public enum ProfileDescription
	{
		/// <summary>
		/// Undefined type - no special coloration, needs to be avoided.
		/// </summary>
		Undefined = 0,
		/// <summary>
		/// The profiler that is used to profile function entry points.
		/// </summary>
		FunctionEntry = 1,
		/// <summary>
		/// The profiler that is used to profile sections of code(?).
		/// </summary>
		Section = 2,
		/// <summary>
		/// The profiler that is used to profile regions of code(?).
		/// </summary>
		Region = 3,
		/// <summary>
		/// Used for sections where we have to wait(?)
		/// </summary>
		Waiting = 1 << 2,
		/// <summary>
		/// No special coloring.
		/// </summary>
		Marker = 1 << 3,
		/// <summary>
		/// No special coloring.
		/// </summary>
		PushPop = 1 << 4
	}
}