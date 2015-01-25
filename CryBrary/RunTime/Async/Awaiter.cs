using System.Collections.Generic;
using CryEngine.RunTime.Async.Jobs;

namespace CryEngine.RunTime.Async
{
	/// <summary>
	/// Class that manages all jobs (updating, removing)
	/// </summary>
	public class Awaiter
	{
		private readonly List<IAsyncJob> _jobs;

		static Awaiter()
		{
			Instance = new Awaiter();
		}

		private Awaiter()
		{
			this._jobs = new List<IAsyncJob>();
		}

		/// <summary>
		/// Gets or sets the singleton instance of the awaiter
		/// </summary>
		public static Awaiter Instance { get; set; }

		/// <summary>
		/// Gets a list of all jobs scheduled to be executed on the next OnUpdate call
		/// </summary>
		public List<IAsyncJob> Jobs
		{
			get
			{
				return this._jobs;
			}
		}

		/// <summary>
		/// Updates all scheduled jobs
		/// </summary>
		/// <param name="frameTime"></param>
		public void OnUpdate(float frameTime)
		{
			for (int i = 0; i < this.Jobs.Count; i++)
			{
				var job = this.Jobs[i];

				// Update the job If the job returns true, it means it has finished, and
				// we can remove it from the updatelist
				if (job.Update(frameTime))
				{
					this.Jobs.Remove(job);

					// We need to decrease i since we have removed an element
					i--;
				}
			}
		}
	}
}