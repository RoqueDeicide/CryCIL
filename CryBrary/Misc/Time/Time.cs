using System.Runtime.CompilerServices;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Contains useful functions related to game timing.
	/// </summary>
	public static class Time
	{
		internal static void Set(float frameTime, float frameStartTime, float asyncTime, float frameRate, float timeScale)
		{
			DeltaTime = frameTime;

			FrameStartTime = frameStartTime;
			AsyncTime = asyncTime;
			Framerate = frameRate;
			_timeScale = timeScale;
		}

		/// <summary>
		/// Returns the absolute time of the last frame start in milliseconds.
		/// </summary>
		public static float FrameStartTime { get; private set; }

		/// <summary>
		/// Returns the absolute current time.
		/// Note: The value continuously changes, slower than GetFrameStartTime().
		/// </summary>
		public static float AsyncTime { get; private set; }

		/// <summary>
		/// The time used to render the current frame. Useful for creating framerate independent operations.
		/// </summary>
		/// <example>
		/// public override void OnUpdate() { this.Position.X += 10 * Time.DeltaTime; }
		/// </example>
		public static float DeltaTime { get; private set; }

		private static float _timeScale;
		/// <summary>
		/// Sets / gets the time scale applied to time values.
		/// </summary>
		public static float TimeScale { get { return _timeScale; } set { NativeTimeMethods.SetTimeScale(value); _timeScale = value; } }

		/// <summary>
		/// Returns the current framerate in frames/second.
		/// </summary>
		public static float Framerate { get; private set; }
	}
}