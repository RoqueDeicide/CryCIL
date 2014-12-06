using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CryEngine.RunTime.Testing
{
	/// <summary>
	/// Contains all the test data for a single run.
	/// </summary>
	public sealed class TestReport
	{
		/// <summary>
		/// The test collections which were run.
		/// </summary>
		public List<TestCollectionResult> Collections { get; internal set; }

		/// <summary>
		/// The time the entire unit test set took to execute.
		/// </summary>
		public TimeSpan TimeTaken { get; internal set; }
	}

	/// <summary>
	/// Indicates the actual result of a given single test.
	/// </summary>
	public enum TestResult
	{
		Success,
		Failure,
		Ignored
	}

	/// <summary>
	/// Contains detailed information on the result of a test.
	/// </summary>
	public sealed class TestResultInfo
	{
		/// <summary>
		/// The name of the test.
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// The optional test description.
		/// </summary>
		public string Description { get; internal set; }

		/// <summary>
		/// The result of the test.
		/// </summary>
		public TestResult Result { get; internal set; }

		/// <summary>
		/// The full stracktrace for this test, if it failed.
		/// </summary>
		public StackTrace Stack { get; internal set; }

		/// <summary>
		/// For convenience, the first desired frame from the stack. If the frame is inside CryBrary
		/// due to an assertion, we drop down one frame.
		/// </summary>
		public StackFrame FirstFrame
		{
			get
			{
				var firstFrame = this.Stack.GetFrame(0);
				return firstFrame.GetFileName().Contains(Path.Combine("CryMono", "CryBrary")) ? this.Stack.GetFrame(1) : firstFrame;
			}
		}

		/// <summary>
		/// The exception that caused this test to fail, if it did.
		/// </summary>
		public Exception Exception { get; internal set; }
	}

	/// <summary>
	/// Contains the results for a set of tests.
	/// </summary>
	public sealed class TestCollectionResult
	{
		/// <summary>
		/// The name of this collection.
		/// </summary>
		public string Name { get; internal set; }

		/// <summary>
		/// The optional collection description.
		/// </summary>
		public string Description { get; internal set; }

		/// <summary>
		/// The individual results for the tests in this collection.
		/// </summary>
		public List<TestResultInfo> Results { get; internal set; }
	}
}