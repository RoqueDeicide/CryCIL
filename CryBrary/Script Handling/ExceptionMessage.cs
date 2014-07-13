using System;
using System.Diagnostics;
using System.Windows.Forms;
using CryEngine.Extensions;
using CryEngine.Utilities;

namespace CryEngine
{
	internal partial class ExceptionMessage : Form
	{
		public ExceptionMessage(Exception ex, bool fatal)
		{
			InitializeComponent();

			label1.Text = ProjectSettings.ExceptionMessage;

			uxContinueBtn.Click += (s, a) => Close();
			uxReportBtn.Click += (s, a) => Process.Start(ProjectSettings.BugReportLink);
			uxCancelBtn.Click += (s, a) => Process.GetCurrentProcess().Kill();

			var text = "";

			if (fatal)
			{
				text += "Exceptions are currently treated as fatal errors (mono_exceptionsTriggerFatalErrors is true)." + Environment.NewLine;
				text += "The application cannot continue." + Environment.NewLine + Environment.NewLine;
			}

			text += ex.ToString();
			uxStackTextbox.Text = text;

			var selected = ActiveControl;
			ActiveControl = uxStackTextbox;
			uxStackTextbox.ScrollTo(0);
			ActiveControl = selected;

			if (fatal)
				uxContinueBtn.Enabled = false;
		}
	}
}