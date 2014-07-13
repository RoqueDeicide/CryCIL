namespace CryEngine.Async.Jobs
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
			_framesWaited = 0;
			FramesToWait = numberOfFramesToWait;

			if (numberOfFramesToWait <= 0)
			{
				source.TrySetResult(false);
				IsFinished = true;
			}
		}

		/// <summary>
		/// Gets or sets the number of frames to wait
		/// </summary>
		public int FramesToWait { get; protected set; }

		public override bool Update(float frameTime)
		{
			if (!IsFinished)
			{
				_framesWaited++;
				if (_framesWaited >= FramesToWait)
				{
					source.TrySetResult(true);
					IsFinished = true;
				}
			}

			return IsFinished;
		}
	}
}