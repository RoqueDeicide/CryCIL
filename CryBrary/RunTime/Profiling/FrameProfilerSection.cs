using System;

namespace CryEngine.RunTime.Profiling
{
	/// <summary>
	/// Frame profiler sections are placed where the code needs to be profiled. Every time this
	/// object is constructed and destructed, the time between the constructor and destructor is
	/// merged into the <see cref="FrameProfiler" /> instance.
	/// </summary>
	public class FrameProfilerSection
	{
		internal FrameProfilerSection(IntPtr handle, FrameProfiler profiler)
		{
			this.Handle = handle;
			this.FrameProfiler = profiler;
		}
		/// <summary>
		/// Ends this section.
		/// </summary>
		public void End()
		{
			this.FrameProfiler.DeleteSection(this);
		}
		/// <summary>
		/// Frame profiler associated with this section.
		/// </summary>
		public FrameProfiler FrameProfiler { get; set; }
		/// <summary>
		/// Handle of this section.
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}