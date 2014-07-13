using System;
using System.Threading.Tasks;
using CryEngine.Async;
using CryEngine.Async.Jobs;

// This class is in the CryEngine namespace, for easy access. If this namespace gets too crowded, it
// might be appropriate to move it to CryEngine.Async
namespace CryEngine
{
	/// <summary>
	/// Utility class that provides async methods for common operations
	/// </summary>
	public class Delay
	{
		/// <summary>
		/// Creates a Task object from an IAsyncJob instance
		/// </summary>
		/// <param name="job"></param>
		public static Task CreateTaskFromJob(IAsyncJob job)
		{
			// job.Task.ConfigureAwait(false).GetAwaiter();
			Awaiter.Instance.Jobs.Add(job);
			return job.Task;
		}

		/// <summary>
		/// Delays execution for a number of frames
		/// </summary>
		/// <param name="numFrames"></param>
		public static Task FrameDelay(int numFrames)
		{
			var frameDelayJob = new FrameDelayJob(numFrames);

			return CreateTaskFromJob(frameDelayJob);
		}

		/// <summary>
		/// Delays execution for a supplied time amount
		/// </summary>
		/// <param name="delay"></param>
		public static Task TimeDelay(TimeSpan delay)
		{
			var timeDelayJob = new TimeDelayJob(delay);

			return CreateTaskFromJob(timeDelayJob);
		}

		/// <summary>
		/// Delays execution for a supplied time amount (in milliseconds)
		/// </summary>
		/// <param name="delayInMilliseconds"></param>
		public static Task TimeDelay(float delayInMilliseconds)
		{
			var timeDelayJob = new TimeDelayJob(delayInMilliseconds);
			return CreateTaskFromJob(timeDelayJob);
		}
	}
}