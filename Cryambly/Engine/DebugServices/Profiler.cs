using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

#pragma warning disable 0169

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents an object that can be used to measure execution times of various parts of code.
	/// </summary>
	/// <remarks>
	/// <para>
	/// These profilers are used to measure time to execute parts of code that is a part of the game loop.
	/// </para>
	/// <para>
	/// Results of profiling are stored within the system and can be displayed using various console
	/// commands/variables.
	/// </para>
	/// </remarks>
	/// <example>
	/// <code>
	/// // 2 different profilers for 1 function each.
	/// internal static readonly Profiler profiler1 = new Profiler("Part1 of ProcessingFunction");
	/// internal static readonly Profiler profiler2 = new Profiler("Part2 of ProcessingFunction");
	/// 
	/// // This function is executed every time the entity is updated.
	/// internal static void ProcessingFunction()
	/// {
	///     Part1();
	/// 
	///     Part2();
	/// }
	/// 
	/// // First profiled function.
	/// internal static void Part1()
	/// {
	///     var section = profiler1.Start();
	///     try
	///     {
	///         // Do some work.
	///     }
	///     finally
	///     {
	///         section.Finish();
	///     }
	/// }
	/// 
	/// // Second profiled function.
	/// internal static void Part2()
	/// {
	///     using (profiler2.Start())
	///     {
	///         // Do some work.
	///     }
	/// }
	/// </code>
	/// </example>
	public class Profiler
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates new performance profiler.
		/// </summary>
		/// <param name="name">Name of the profiler.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Profiler([UsedImplicitly] string name);
		#endregion
		#region Interface
		/// <summary>
		/// Starts a profiling section.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProfilingSection Start();
		#endregion
		#region Utilities
		#endregion
	}
	/// <summary>
	/// Represents a profiling section.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ProfilingSection : IDisposable
	{
		[UsedImplicitly]
		private IntPtr handle;
		/// <summary>
		/// Finishes measurement on this profiling section.
		/// </summary>
		public void Finish()
		{
			this.Dispose();
		}
		/// <summary>
		/// Finishes measurement on this profiling section.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Dispose();
	}
}

#pragma warning restore 0169