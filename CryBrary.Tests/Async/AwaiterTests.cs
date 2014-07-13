using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryEngine;
using CryEngine.Async;
using CryEngine.Async.Jobs;
using Moq;
using NUnit.Framework;

namespace CryBrary.Tests.Async
{
	public class AwaiterTests
	{
		public AwaiterTests()
		{
			Awaiter.Instance.Jobs.Clear();
		}

		[Test]
		public void OnUpdate_FrameDelayJob_ExpiredJobRemovedCorrectly()
		{
			// Arrange
			var task = Delay.FrameDelay(1);

			// Act
			Awaiter.Instance.OnUpdate(1);

			// Assert
			Assert.AreEqual(0, Awaiter.Instance.Jobs.Count);
		}

		[Test]
		public void OnUpdate_FrameDelayJob_DoNotRemoveJobsTooEarly()
		{
			// Arrange
			var task = Delay.FrameDelay(5);
			var task2 = Delay.FrameDelay(20);
			var task3 = Delay.FrameDelay(10);
			var task4 = Delay.FrameDelay(3);
			var task5 = Delay.FrameDelay(4);

			// Act
			Awaiter.Instance.OnUpdate(1);
			Awaiter.Instance.OnUpdate(1);
			Awaiter.Instance.OnUpdate(1);
			Awaiter.Instance.OnUpdate(1);

			// Assert
			Assert.AreEqual(3, Awaiter.Instance.Jobs.Count);
		}

		[Test]
		public void OnUpdate_MockJob_IsUpdated()
		{
			// Arrange
			bool updateCalled = false;
			Mock<IAsyncJob> job = new Mock<IAsyncJob>();
			job.Setup(j => j.Update(It.IsAny<float>())).Callback(() => updateCalled = true).Returns(true);

			// Act
			Awaiter.Instance.Jobs.Add(job.Object);
			Awaiter.Instance.OnUpdate(1);

			// Assert
			Assert.True(updateCalled);
		}
	}
}