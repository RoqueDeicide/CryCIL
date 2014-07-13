using System.Threading.Tasks;

namespace CryEngine.Async.Jobs
{
	/// <summary>
	/// Base class for any async job
	/// </summary>
	public abstract class AsyncJob<T> : IAsyncJob
	{
		protected TaskCompletionSource<T> source = new TaskCompletionSource<T>();

		/// <summary>
		/// Gets the underlying System.Threading.Tasks.Task object
		/// </summary>
		public Task Task
		{
			get
			{
				return source.Task;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether if the job has finished executing
		/// </summary>
		public bool IsFinished { get; set; }

		/// <summary>
		/// Updates the job
		/// </summary>
		/// <param name="frameTime"></param>
		public abstract bool Update(float frameTime);
	}
}