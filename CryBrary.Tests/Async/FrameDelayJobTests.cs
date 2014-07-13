using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Async;
using CryEngine.Async.Jobs;
using NUnit.Framework;

namespace CryBrary.Tests.Async
{
	public class FrameDelayJobTests
	{
		[Test]
		public void FrameDelayJob_NegativeNumberOfFrames_InstantExecution()
		{
			// Arrange
			var job = new FrameDelayJob(-20);

			// Act

			// Assert
			Assert.True(job.IsFinished);
			Assert.True(job.Task.IsCompleted);
			Assert.False(job.Task.IsCanceled);
			Assert.False(job.Task.IsFaulted);
			Assert.True(job.Task.Status == TaskStatus.RanToCompletion);
		}

		[Test]
		public void FrameDelayJob_5Frames_DelayedExecution()
		{
			// Arrange
			var job = new FrameDelayJob(5);

			Assert.False(job.IsFinished);
			Assert.False(job.Task.IsCompleted);

			// Act
			Assert.False(job.Update(1));
			Assert.False(job.Update(1));
			Assert.False(job.Update(1));
			Assert.False(job.Update(1));

			// 5th frame should be the final one
			Assert.True(job.Update(1));

			// Assert
			Assert.True(job.IsFinished);
			Assert.True(job.Task.IsCompleted);
		}
	}
}