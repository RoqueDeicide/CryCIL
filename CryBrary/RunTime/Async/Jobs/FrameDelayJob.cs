namespace CryEngine.RunTime.Async.Jobs
{
	/// <summary>
	/// Delays execution for a number of frames
	/// </summary>
	public class FrameDelayJob : AsyncJob<bool>
	{
		private int _framesWaited;

		/// <summary>
		/// Initializes a new instance of the <see cref="FrameDelayJob" /> class.
		/// </summary>
		/// <param name="numberOfFramesToWait"></param>
		public FrameDelayJob(int numberOfFramesToWait)
		{
			this._framesWaited = 0;
			this.FramesToWait = numberOfFramesToWait;

			if (numberOfFramesToWait <= 0)
			{
				this.source.TrySetResult(false);
				this.IsFinished = true;
			}
		}

		/// <summary>
		/// Gets or sets the number of frames to wait
		/// </summary>
		public int FramesToWait { get; protected set; }

		public override bool Update(float frameTime)
		{
			if (!this.IsFinished)
			{
				this._framesWaited++;
				if (this._framesWaited >= this.FramesToWait)
				{
					this.source.TrySetResult(true);
					this.IsFinished = true;
				}
			}

			return this.IsFinished;
		}
	}
}