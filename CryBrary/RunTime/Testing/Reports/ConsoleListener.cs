using System.Linq;

namespace CryEngine.RunTime.Testing.Reports
{
	/// <summary>
	/// Example report listener, used to log the results of a test run to the console.
	/// </summary>
	internal class ConsoleTestListener
	{
		/// <summary>
		/// Constructs a new ConsoleTestListener and subscribes to the TestManager.Run
		/// event.
		/// </summary>
		public ConsoleTestListener()
		{
			TestManager.Run += this.OnTestsRun;
		}

		/// <summary>
		/// Called when a report is received.
		/// </summary>
		/// <param name="report">The report, containing all test information.</param>
		public void OnTestsRun(TestReport report)
		{
			Debug.LogAlways("Console unit test log. Tests took {0}s overall to execute.", report.TimeTaken.TotalSeconds);

			var testCounts = EnumExtas.Values<TestResult>().ToDictionary(key => key, key => 0);

			foreach (var collection in report.Collections)
			{
				Debug.LogAlways("        Test collection: {0}", collection.Name);

				foreach (var test in collection.Results)
				{
					testCounts[test.Result]++;
					var log = test.Name;

					if (!string.IsNullOrEmpty(test.Description))
						log += string.Format(" ({0})", test.Description);

					Debug.LogAlways("            {0}", log);

					switch (test.Result)
					{
						case TestResult.Success:
							Debug.LogAlways("                Test successful.");
							break;

						case TestResult.Failure:
							Debug.LogAlways("                Test failed: {0} ({1} thrown at line {2} of {3})", test.Exception.Message,
								test.Exception.GetType().Name, test.FirstFrame.GetFileLineNumber(), test.FirstFrame.GetFileName());
							break;

						case TestResult.Ignored:
							Debug.LogAlways("                Test ignored.");
							break;
					}
				}
			}

			var failCount = testCounts[TestResult.Failure];
			var ignoredCount = testCounts[TestResult.Ignored];
			var collectionFailCount = report.Collections.Count(col => col.Results.Any(test => test.Result == TestResult.Failure));

			Debug.LogAlways("Test run overview:");

			if (failCount == 0)
				Debug.LogAlways("        No errors found!");
			else
				Debug.LogAlways("        {0} total error{1} across {2} failing test collection{3}.",
					failCount, failCount > 1 ? "s" : string.Empty,
					collectionFailCount, collectionFailCount > 1 ? "s" : string.Empty);

			if (ignoredCount == 0)
				Debug.LogAlways("        No tests ignored.");
			else
				Debug.LogAlways("        {0} test{1} ignored.", ignoredCount, ignoredCount > 1 ? "s" : string.Empty);
		}
	}
}