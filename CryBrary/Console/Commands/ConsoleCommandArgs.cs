using System;

namespace CryEngine.Console.Commands
{
	public class ConsoleCommandArgs : EventArgs
	{
		public ConsoleCommandArgs(string name, string[] args, string fullCommandLine)
		{
			this.Name = name;
			this.Args = args;
			this.FullCommandLine = fullCommandLine;
		}

		public string Name { get; private set; }

		public string[] Args { get; private set; }

		public string FullCommandLine { get; private set; }
	}
}