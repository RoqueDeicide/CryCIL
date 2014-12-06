namespace CryEngine.Console
{
	/// <summary>
	/// Enumeration of types of command argument.
	/// </summary>
	public enum CommandArgumentType
	{
		/// <summary>
		/// Argument was not preceded by anything
		/// </summary>
		Normal = 0,
		/// <summary>
		/// Argument was preceded by a minus sign
		/// </summary>
		Pre,
		/// <summary>
		/// Argument was preceded by a plus sign
		/// </summary>
		Post,
		/// <summary>
		/// Argument is the executable filename
		/// </summary>
		Executable
	}
	/// <summary>
	/// Defines methods for working with console.
	/// </summary>
	public static class CryConsole
	{
		public static string GetCommandLineArgument(string name, CommandArgumentType type)
		{
			return Native.ConsoleInterop.GetCmdArg(name, (int)type);
		}
	}
}