using System.Runtime.CompilerServices;
using CryEngine.Native;

namespace CryEngine
{
	public enum CommandArgumentType
	{
		/// <summary>
		/// Argument was not preceeded by anything
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Argument was preceeded by a minus sign
		/// </summary>
		Pre,

		/// <summary>
		/// Argument was preceeded by a plus sign
		/// </summary>
		Post,

		/// <summary>
		/// Argument is the executable filename
		/// </summary>
		Executable,
	}

	public static class CryConsole
	{
		public static string GetCommandLineArgument(string name, CommandArgumentType type)
		{
			return NativeCVarMethods.GetCmdArg(name, (int)type);
		}
	}
}