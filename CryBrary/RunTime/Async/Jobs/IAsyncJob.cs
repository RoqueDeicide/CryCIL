using System.Threading.Tasks;

namespace CryEngine.RunTime.Async.Jobs
{
	/// <summary>
	/// An asynchronous job
	/// </summary>
	public interface IAsyncJob
	{
		/// <summary>
		/// Gets the internal Task object associated with the job
		/// </summary>
		Task Task { get; }

		/// <summary>
		/// Gets a value indicating whether the job has finished processing
		/// </summary>
		bool IsFinished { get; }

		/// <summary>
		/// Updates the job
		/// </summary>
		/// <param name="frameTime"></param>
		bool Update(float frameTime);
	}
}