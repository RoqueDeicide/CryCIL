using System;

namespace CryEngine.RunTime.Profiling
{
	/// <summary>
	/// A profiler counter with a unique name and data. Multiple <see cref="FrameProfilerSection"/> s can
	/// be executed for this profiler, and will be merged into their parent profiler.
	/// </summary>
	public class FrameProfiler
	{
		private FrameProfiler(IntPtr handle)
		{
			this.Handle = handle;
		}
		/// <summary>
		/// Creates a new frame profiler.
		/// </summary>
		/// <param name="name">Name of the profiler.</param>
		/// <returns>A wrapper object for CFrameProfiler *.</returns>
		public static FrameProfiler Create(string name)
		{
			return new FrameProfiler(Native.DebugInterop.CreateFrameProfiler(name));
		}
		/// <summary>
		/// Creates a new <see cref="FrameProfilerSection"/>.
		/// </summary>
		/// <returns>A new frame profiler section.</returns>
		public FrameProfilerSection CreateSection()
		{
			return new FrameProfilerSection(Native.DebugInterop.CreateFrameProfilerSection(this.Handle), this);
		}
		/// <summary>
		/// Deletes a frame profile section.
		/// </summary>
		/// <param name="profilerSection"><see cref="FrameProfilerSection"/> to delete.</param>
		public void DeleteSection(FrameProfilerSection profilerSection)
		{
			Native.DebugInterop.DeleteFrameProfilerSection(profilerSection.Handle);
		}
		/// <summary>
		/// CFrameProfiler *
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}