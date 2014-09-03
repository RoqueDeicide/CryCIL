using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.Profiling
{
	/// <summary>
	/// A profiler counter with a unique name and data. Multiple <see cref="FrameProfilerSection"
	/// />s can be executed for this profiler, and will be merged into their parent profiler.
	/// </summary>
	public class FrameProfiler
	{
		private FrameProfiler(IntPtr handle)
		{
			Handle = handle;
		}
		/// <summary>
		/// Creates a new frame profiler.
		/// </summary>
		/// <param name="name">Name of the profiler.</param>
		/// <returns>A wrapper object for CFrameProfiler *.</returns>
		public static FrameProfiler Create(string name)
		{
			return new FrameProfiler(DebugInterop.CreateFrameProfiler(name));
		}
		/// <summary>
		/// Creates a new <see cref="FrameProfilerSection"/>.
		/// </summary>
		/// <returns>A new frame profiler section.</returns>
		public FrameProfilerSection CreateSection()
		{
			return new FrameProfilerSection(DebugInterop.CreateFrameProfilerSection(Handle), this);
		}
		/// <summary>
		/// Deletes a frame profile section.
		/// </summary>
		/// <param name="profilerSection"><see cref="FrameProfilerSection"/> to delete.</param>
		public void DeleteSection(FrameProfilerSection profilerSection)
		{
			DebugInterop.DeleteFrameProfilerSection(profilerSection.Handle);
		}
		/// <summary>
		/// CFrameProfiler *
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}