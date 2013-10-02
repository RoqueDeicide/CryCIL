using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.Profiling
{
	/// <summary>
	/// A profiler counter with a unique name and data.
	/// Multiple <see cref="FrameProfilerSection"/>s can be executed for this profiler,
	/// and will be merged into their parent profiler.
	/// </summary>
	public class FrameProfiler
	{
		private FrameProfiler(IntPtr handle)
		{
			Handle = handle;
		}
		
		public static FrameProfiler Create(string name)
		{
			return new FrameProfiler(NativeDebugMethods.CreateFrameProfiler(name));
		}

		public FrameProfilerSection CreateSection()
		{
			return new FrameProfilerSection(NativeDebugMethods.CreateFrameProfilerSection(Handle), this);
		}

		public void DeleteSection(FrameProfilerSection profilerSection)
		{
			NativeDebugMethods.DeleteFrameProfilerSection(profilerSection.Handle);
		}

		/// <summary>
		/// CFrameProfiler *
		/// </summary>
		public IntPtr Handle { get; set; }
	}
}
