using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.Profiling
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
			Handle = handle;
			FrameProfiler = profiler;
		}

		public void End()
		{
			FrameProfiler.DeleteSection(this);
		}

		public FrameProfiler FrameProfiler { get; set; }

		public IntPtr Handle { get; set; }
	}
}