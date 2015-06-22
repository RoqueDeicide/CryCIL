using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.RunTime;

namespace CryCil.Engine
{
	/// <summary>
	/// Provides access to CryEngine timing API.
	/// </summary>
	public static class Time
	{
		#region Fields
		private static float timeScale;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the processed absolute time when this frame has started.
		/// </summary>
		/// <remarks>
		/// Possible processes that this time goes through include pausing, smoothing, scaling, clamping.
		/// </remarks>
		public static DateTime FrameStart { get; internal set; }
		/// <summary>
		/// Gets the unprocessed absolute time when this frame has started.
		/// </summary>
		public static DateTime FrameStartUi { get; internal set; }
		/// <summary>
		/// Gets the absolute current time that changes independently from <see cref="FrameStart"/>.
		/// </summary>
		/// <remarks>This timing tends to change more slowly then <see cref="FrameStart"/>.</remarks>
		public static extern DateTime Async { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the absolute current time at the moment of the call.
		/// </summary>
		/// <remarks>Only use this when timing is crucial for the game.</remarks>
		public static extern DateTime AsyncCurrent { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the processed time it took to run previous frame.
		/// </summary>
		/// <remarks>
		/// Possible processes that this time goes through include pausing, smoothing, scaling, clamping.
		/// </remarks>
		public static TimeSpan Frame { get; internal set; }
		/// <summary>
		/// Gets the unprocessed time it took to run previous frame.
		/// </summary>
		public static TimeSpan RealFrame { get; internal set; }
		/// <summary>
		/// Gets or sets time dilation value.
		/// </summary>
		public static float Scale
		{
			get { return timeScale; }
			set
			{
				if (Math.Abs(value - timeScale) < MathHelpers.ZeroTolerance)
				{
					return;
				}
				timeScale = value;
				SetTimeScale(value);
			}
		}
		/// <summary>
		/// Gets the frame rate in frames per second.
		/// </summary>
		public static float FrameRate { get; internal set; }
		#endregion
		#region Events

		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Removes time dilation effect.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearScaling();
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTimeScale(float value);
		[RawThunk("Invoked by underlying framework to update the time values at the start of the frame.")]
		private static void SetTimings(long frameStart, long frameStartUi, long frame,
									   long realFrame, float scale, float frameRate)
		{
			try
			{
				FrameStart = new DateTime(frameStart, DateTimeKind.Unspecified);
				FrameStartUi = new DateTime(frameStartUi, DateTimeKind.Unspecified);
				Frame = new TimeSpan(frame);
				RealFrame = new TimeSpan(realFrame);
				Time.timeScale = scale;
				FrameRate = frameRate;
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}