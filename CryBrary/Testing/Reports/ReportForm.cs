using System;
using System.Linq;
using System.Windows.Forms;
using CryEngine.Extensions;
using CryEngine.Sandbox;

namespace CryEngine.Testing.Internals
{
	[SandboxExtension("Unit Testing", "Provides automated testing for your .NET game project.", AuthorName = "Ink Studios Ltd.", AuthorContact = "http:// inkdev.net")]
	internal partial class ReportForm : Form
	{
		public ReportForm()
		{
			InitializeComponent();
			TestManager.Run += OnTestsRun;
			uxRerunTests.Click += (sender, args) => TestManager.RunTests();
		}

		public void OnTestsRun(TestReport report)
		{
			uxTestTree.Nodes.Clear();
			var root = uxTestTree.Nodes.Add("Tests");

			var overallSuccess = true;
			var overallIgnored = false;

			var testCounts = EnumExtensions.GetMembers<TestResult>().ToDictionary(key => key, key => 0);

			foreach (var collection in report.Collections)
			{
				var collectionNode = root.Nodes.Add(collection.Name);
				collectionNode.Tag = collection;

				var ignored = false;
				var failed = false;

				foreach (var test in collection.Results)
				{
					testCounts[test.Result]++;

					if (test.Result == TestResult.Failure)
					{
						overallSuccess = false;
						failed = true;
					}
					else if (test.Result == TestResult.Ignored && !failed)
					{
						if (overallSuccess)
							overallIgnored = true;

						ignored = true;
					}

					var image = GetImageIndex(test.Result);
					var node = new TreeNode(test.Name, image, image) { Tag = test };

					collectionNode.Nodes.Add(node);
				}

				var collectionImage = GetImageIndex(failed, ignored);
				collectionNode.ImageIndex = collectionImage;
				collectionNode.SelectedImageIndex = collectionImage;
			}

			var rootImage = GetImageIndex(!overallSuccess, overallIgnored);
			root.ImageIndex = rootImage;
			root.SelectedImageIndex = rootImage;

			var totalTestCount = testCounts.Sum(pair => pair.Value);

			uxTimeMessage.Text = string.Format("Test run with {0} tests took {1}s to execute.", totalTestCount, report.TimeTaken.TotalSeconds);

			uxSuccessCount.Text = CreateCountMessage(testCounts[TestResult.Success], "passed");
			uxFailureCount.Text = CreateCountMessage(testCounts[TestResult.Failure], "failed");
			uxIgnoredCount.Text = CreateCountMessage(testCounts[TestResult.Ignored], "ignored", true);
		}

		private string CreateCountMessage(int count, string message, bool passive = false)
		{
			var plural = count > 1;
			return string.Format("{0} test{1}{2} {3}.", count, plural ? "s" : string.Empty,
				passive ? (plural ? " were" : " was") : string.Empty, message);
		}

		private int GetImageIndex(bool failed, bool ignored)
		{
			return failed ? GetImageIndex(TestResult.Failure) :
				ignored ? GetImageIndex(TestResult.Ignored) :
				GetImageIndex(TestResult.Success);
		}

		private int GetImageIndex(TestResult result)
		{
			switch (result)
			{
				case TestResult.Success:
					return 0;
				case TestResult.Failure:
					return 1;
				case TestResult.Ignored:
					return 2;
			}

			throw new ArgumentException("The supplied TestResult value is not supported by the report UI.", "result");
		}

		private void OnTreeSelect(object sender, TreeViewEventArgs e)
		{
			uxTestResult.Clear();

			var test = e.Node.Tag as TestResultInfo;

			if (test != null)
			{
				var n = Environment.NewLine;
				uxTestResult.AppendText(test.Name + n);
				uxTestResult.AppendText((test.Description ?? "No description supplied.") + n + n);

				uxTestResult.AppendText("Result: " + test.Result.ToString() + n + n);

				if (test.Result == TestResult.Failure)
				{
					uxTestResult.AppendText(string.Format("{0} ({1} thrown at line {2} of {3})", test.Exception.Message,
								test.Exception.GetType().Name, test.FirstFrame.GetFileLineNumber(), test.FirstFrame.GetFileName()));

					uxTestResult.AppendText(n + n + "Full stacktrace:" + n + n);
					uxTestResult.AppendText(test.Stack.ToString());
				}
			}
		}

		private void OnClose(object sender, FormClosingEventArgs e)
		{
			TestManager.Run -= OnTestsRun;
		}
	}
}