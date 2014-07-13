using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CryEngine.Sandbox;
using CryEngine.Testing.Internals;

namespace CryEngine.Testing
{
	public static class TestManager
	{
		public static List<TestCollection> TestCollections { get; private set; }

		public static event Action<TestReport> Run;

		// TODO: Provide a method to selectively run tests
		public static void RunTests()
		{
			RunTests(default(ConsoleCommandArgs));
		}

		private static void RunTests(ConsoleCommandArgs e)
		{
			var timer = Stopwatch.StartNew();

			var testResults = (from testCollection in TestCollections
							   select testCollection.Run()).ToList();

			timer.Stop();

			var report = new TestReport { Collections = testResults, TimeTaken = timer.Elapsed };

			if (Run != null)
				Run(report);
		}

		internal static void Init()
		{
			TestCollections = new List<TestCollection>();

			ConsoleCommand.Register("mono_tests_run", RunTests, "Runs the feature tester");
			FormHelper.RegisterInternal<ReportForm>();
		}

		private static ConsoleTestListener listener = new ConsoleTestListener();
	}
}