using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.CodeDom.Compiler;

using CryEngine.Native;

namespace CryEngine.Initialization
{
	public partial class ScriptReloadMessage : Form
	{
		public ScriptReloadMessage(Exception exception, bool canRevert)
		{
			InitializeComponent();

			tryAgainButton.Click += (s, a) =>
				{
					Result = ScriptReloadResult.Retry;
					Close();
				};

			revertButton.Click += (s, a) =>
				{
					Result = ScriptReloadResult.Revert;
					Close();
				};

			exitButton.Click += (s, a) =>
				{
					;
					Result = ScriptReloadResult.Abort;
					Close();
				};

			if (!canRevert)
				revertButton.Enabled = false;

			errorBox.Text = exception.ToString();
		}

		public ScriptReloadResult Result { get; set; }
	}
}