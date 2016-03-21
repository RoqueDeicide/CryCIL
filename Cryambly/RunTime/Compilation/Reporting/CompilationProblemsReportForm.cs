using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CryCil.RunTime.Compilation.Reporting
{
	/// <summary>
	/// Represents a form that is shown when at least one project fails to build.
	/// </summary>
	public partial class CompilationProblemsReportForm : Form
	{
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="failures">Map of failed projects.</param>
		public CompilationProblemsReportForm(Dictionary<string, string> failures)
		{
			this.InitializeComponent();

			foreach (ListViewItem item in from failure in failures
										  select new ListViewItem(new[]
										  {
											  failure.Key, failure.Value
										  }))
			{
				this.ProblemsList.Items.Add(item);
			}
		}
	}
}