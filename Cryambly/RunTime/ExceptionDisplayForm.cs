using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using CryCil.Annotations;

namespace CryCil.RunTime
{
	/// <summary>
	/// Represents a window that opens up when an exception is thrown and not handled anywhere.
	/// </summary>
	public partial class ExceptionDisplayForm : Form
	{
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="ex">The object that represents the exception that has to be displayed.</param>
		public ExceptionDisplayForm([CanBeNull] Exception ex)
		{
			this.InitializeComponent();

			if (ex == null)
			{
				this.ExceptionTypeBox.Text = "Unmanaged Exception";
				this.ExceptionMessageBox.Text = "N/A";
				this.ExceptionTraceBox.Text = new StackTrace().ToString();
			}
			else
			{
				this.ExceptionTypeBox.Text = ex.GetType().FullName;
				this.ExceptionMessageBox.Text = ex.Message;
			}

			Exception parentException = null;
			StringBuilder indent = new StringBuilder(64);
			StringBuilder traceBuilder = new StringBuilder(1000);

			for (Exception currentException = ex;
				 currentException != null;
				 currentException = currentException.InnerException)
			{
				// Print out the name of the type.
				traceBuilder.Append(indent);
				if (parentException != null)
				{
					traceBuilder.Append("caused by ");
				}
				traceBuilder.Append(currentException.GetType().FullName);
				traceBuilder.AppendLine(" with message:");
				// Print out the message.
				traceBuilder.Append(indent);
				traceBuilder.Append('"');
				traceBuilder.Append(currentException.Message);
				traceBuilder.Append('"');
				traceBuilder.AppendLine(" at:");
				// Print out the stack trace. Split default string into lines for indentation.
				string[] stackTraceLines = currentException.StackTrace.Split(new[] {Environment.NewLine},
																			 StringSplitOptions.None);
				foreach (string stackTraceLine in stackTraceLines)
				{
					traceBuilder.Append(indent);
					traceBuilder.AppendLine(stackTraceLine);
				}
				indent.Append("    ");
				parentException = currentException;
			}
			this.ExceptionTraceBox.Text = traceBuilder.ToString();
		}

		private void AttemptToContinue(object sender, EventArgs e)
		{
			this.Close();
		}

		private void Terminate(object sender, EventArgs e)
		{
			Process.GetCurrentProcess().Kill();
		}

		private void ExceptionDisplayForm_Shown(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Arrow;
		}
	}
}