using System;
using System.Linq;
using System.Collections.Generic;

using CryEngine.Native;

namespace CryEngine
{
	public delegate void ConsoleCommandDelegate(ConsoleCommandArgs e);

	public static class ConsoleCommand
	{
		internal static Dictionary<string, ConsoleCommandDelegate> Commands = new Dictionary<string, ConsoleCommandDelegate>();

		/// <summary>
		/// Executes a string in the console
		/// </summary>
		/// <param name="command">console command e.g. "map testy" - no leading slash</param>
		/// <param name="silent">
		/// suppresses log in error case and logging the command to the console
		/// </param>
		public static void Execute(string command, bool silent = false)
		{
			NativeCVarMethods.Execute(command, silent);
		}

		/// <summary>
		/// Register a new console command.
		/// </summary>
		/// <param name="name">Command name.</param>
		/// <param name="func">
		/// Delegate to the console command function to be called when command is invoked.
		/// </param>
		/// <param name="comment">
		/// Help string, will be displayed when typing in console "command ?".
		/// </param>
		/// <param name="flags">Bitfield consist of VF_ flags (e.g. VF_CHEAT)</param>
		public static void Register(string name, ConsoleCommandDelegate func, string comment = "", CVarFlags flags = CVarFlags.None, bool overwrite = false)
		{
			bool containsKey = Commands.ContainsKey(name);
			if (containsKey && overwrite)
			{
				Unregister(name);
				containsKey = false;
			}

			if (!containsKey)
			{
				NativeCVarMethods.RegisterCommand(name, comment, flags);

				Commands.Add(name, func);
			}
			else
				throw new DuplicateConsoleCommandException(string.Format("The console command {0} was already registered", name));
		}

		public static void Unregister(string name)
		{
			NativeCVarMethods.UnregisterCCommand(name);

			if (Commands.ContainsKey(name))
				Commands.Remove(name);
		}

		internal static void OnCommand(string fullCommandLine)
		{
			var argsWithName = fullCommandLine.Split(' ');
			var name = argsWithName[0];

			var args = new string[argsWithName.Length - 1];
			for (int i = 1; i < argsWithName.Length; i++)
				args[i - 1] = argsWithName[i];

			Commands[name](new ConsoleCommandArgs(name, args, fullCommandLine));
		}
	}
}