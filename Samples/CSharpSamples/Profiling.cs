using System;
using System.Linq;
using CryCil.Engine.DebugServices;

namespace CSharpSamples
{
	/// <summary>
	/// In this class there is a function that is invoked to do some work every frame. We want to know how
	/// much time that work takes. The work is divided into 2 parts, so we have 2 frame profilers, 1 for
	/// each part of the work.
	/// </summary>
	public static class Profiling
	{
		/// <summary>
		/// This profiler is used to measure how long it takes to do first part of work.
		/// </summary>
		public static readonly Profiler Part1Profiler = new Profiler("WorkPart1");
		/// <summary>
		/// This profiler is used to measure how long it takes to do second part of work.
		/// </summary>
		public static readonly Profiler Part2Profiler = new Profiler("WorkPart2");

		/// <summary>
		/// This function does some work.
		/// </summary>
		public static void Work()
		{
			WorkPart1();

			WorkPart2();
		}

		/// <summary>
		/// This function does first part of work.
		/// </summary>
		public static void WorkPart1()
		{
			// This part of work we are going to measure by creating a profiling section and releasing it in
			// "finally" block.

			var profilingSection = Part1Profiler.Start();

			try
			{
				// We do some work here.
			}
			finally
			{
				// Here we finish the section. The time it took for program to get to this block since
				// Part1Profiler.Start() is going to be recorded within the profiler.
				profilingSection.Finish();
			}
		}
		/// <summary>
		/// This function does second part of work.
		/// </summary>
		public static void WorkPart2()
		{
			// This part of work we are going to measure by creating a profiling section and releasing it
			// using a "using()" block.

			using (Part2Profiler.Start())
			{
				// We do some work here.
			}

			// This approach is equivalent of try..finally block in the first part.
		}
	}
}