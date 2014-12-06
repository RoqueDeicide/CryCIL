using System;
using System.Linq;
using System.Windows.Forms;

namespace CryEngine.RunTime.Testing.Reports
{
	internal partial class ReportForm : Form
	{
		public ReportForm()
		{
			this.InitializeComponent();
			TestManager.Run += this.OnTestsRun;
			this.uxRerunTests.Click += (sender, args) => TestManager.RunTests();
		}

		public void OnTestsRun(TestReport report)
		{
			this.uxTestTree.Nodes.Clear();
			var root = this.uxTestTree.Nodes.Add("Tests");

			var overallSuccess = true;
			var overallIgnored = false;

			var testCounts = EnumExtas.Values<TestResult>().ToDictionary(key => key, key => 0);

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

					var image = this.GetImageIndex(test.Result);
					var node = new TreeNode(test.Name, image, image) { Tag = test };

					collectionNode.Nodes.Add(node);
				}

				var collectionImage = this.GetImageIndex(failed, ignored);
				collectionNode.ImageIndex = collectionImage;
				collectionNode.SelectedImageIndex = collectionImage;
			}

			var rootImage = this.GetImageIndex(!overallSuccess, overallIgnored);
			root.ImageIndex = rootImage;
			root.SelectedImageIndex = rootImage;

			var totalTestCount = testCounts.Sum(pair => pair.Value);

			this.uxTimeMessage.Text = string.Format("Test run with {0} tests took {1}s to execute.", totalTestCount, report.TimeTaken.TotalSeconds);

			this.uxSuccessCount.Text = this.CreateCountMessage(testCounts[TestResult.Success], "passed");
			this.uxFailureCount.Text = this.CreateCountMessage(testCounts[TestResult.Failure], "failed");
			this.uxIgnoredCount.Text = this.CreateCountMessage(testCounts[TestResult.Ignored], "ignored", true);
		}

		private string CreateCountMessage(int count, string message, bool passive = false)
		{
			var plural = count > 1;
			return string.Format("{0} test{1}{2} {3}.", count, plural ? "s" : string.Empty,
				passive ? (plural ? " were" : " was") : string.Empty, message);
		}

		private int GetImageIndex(bool failed, bool ignored)
		{
			return failed ? this.GetImageIndex(TestResult.Failure) :
				ignored ? this.GetImageIndex(TestResult.Ignored) :
				this.GetImageIndex(TestResult.Success);
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
			this.uxTestResult.Clear();

			var test = e.Node.Tag as TestResultInfo;

			if (test != null)
			{
				var n = Environment.NewLine;
				this.uxTestResult.AppendText(test.Name + n);
				this.uxTestResult.AppendText((test.Description ?? "No description supplied.") + n + n);

				this.uxTestResult.AppendText("Result: " + test.Result.ToString() + n + n);

				if (test.Result == TestResult.Failure)
				{
					this.uxTestResult.AppendText(string.Format("{0} ({1} thrown at line {2} of {3})", test.Exception.Message,
								test.Exception.GetType().Name, test.FirstFrame.GetFileLineNumber(), test.FirstFrame.GetFileName()));

					this.uxTestResult.AppendText(n + n + "Full stacktrace:" + n + n);
					this.uxTestResult.AppendText(test.Stack.ToString());
				}
			}
		}

		private void OnClose(object sender, FormClosingEventArgs e)
		{
			TestManager.Run -= this.OnTestsRun;
		}
	}
}