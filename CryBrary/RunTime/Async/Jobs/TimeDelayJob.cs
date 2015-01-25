using System;

namespace CryEngine.RunTime.Async.Jobs
{
	/// <summary>
	/// Delays for a time period
	/// </summary>
	public class TimeDelayJob : AsyncJob<bool>
	{
		private float _timeElapsed;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeDelayJob"/> class.
		/// </summary>
		/// <param name="milliseconds"></param>
		public TimeDelayJob(float milliseconds)
		{
			this.DelayInMilliseconds = milliseconds;
			this._timeElapsed = 0;

			if (milliseconds <= 0)
			{
				this.source.TrySetResult(false);
				this.IsFinished = true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TimeDelayJob"/> class.
		/// </summary>
		/// <param name="delay"></param>
		public TimeDelayJob(TimeSpan delay)
			: this(System.Convert.ToSingle(delay.TotalMilliseconds))
		{
		}

		/// <summary>
		/// Gets or sets the delay in milliseconds
		/// </summary>
		public float DelayInMilliseconds { get; protected set; }

		public override bool Update(float frameTime)
		{
			this._timeElapsed += frameTime;
			if (!this.IsFinished && this._timeElapsed >= this.DelayInMilliseconds)
			{
				this.source.TrySetResult(true);
				this.IsFinished = true;
			}

			return this.IsFinished;
		}
	}
}