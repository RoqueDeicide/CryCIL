using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
	/// <code source="Profiling.cs"/>
	/// </example>
	public class Profiler
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Construction
		/// <summary>
		/// Creates new performance profiler.
		/// </summary>
		/// <param name="name">Name of the profiler.</param>
		/// <param name="description">A set of flags that specify how to colorize the frame profiler.</param>
		/// <param name="file">Name of the file that contains the profiling sections.</param>
		/// <param name="line">Index of the line this profiler is associated with.</param>
		public Profiler(string name, ProfileDescription description, string file, uint line)
		{
			this.handle = CreateProfiler(description, name, file, line);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Starts a profiling section.
		/// </summary>
		/// <returns>An object that will close the profiling section, once it's disposed of.</returns>
		/// <exception cref="NullReferenceException">
		/// This profiler wasn't properly initialized. This exception is only thrown in debug builds.
		/// </exception>
		public ProfilingSection Start()
		{
#if DEBUG
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("This profiler wasn't properly initialized.");
			}
#endif

			return StartSection(this.handle);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateProfiler(ProfileDescription description, string name,
													string file, uint line);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ProfilingSection StartSection(IntPtr handle);
		#endregion
	}
	/// <summary>
	/// Represents a profiling section.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ProfilingSection : IDisposable
	{
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
		public void Dispose()
		{
			if (this.handle == IntPtr.Zero)
			{
				return;
			}

			FinishSection(this.handle);
			this.handle = IntPtr.Zero;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FinishSection(IntPtr handle);
	}
}
#pragma warning restore 0169