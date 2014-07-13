using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public class ConsoleCommandArgs : EventArgs
	{
		public ConsoleCommandArgs(string name, string[] args, string fullCommandLine)
		{
			Name = name;
			Args = args;
			FullCommandLine = fullCommandLine;
		}

		public string Name { get; private set; }

		public string[] Args { get; private set; }

		public string FullCommandLine { get; private set; }
	}
}