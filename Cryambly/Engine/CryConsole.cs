using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents functions that can be used for execution of console commands.
	/// </summary>
	/// <param name="name">       Name of the command.</param>
	/// <param name="args">       An array of arguments.</param>
	/// <param name="fullCommand">A space-delimited list of arguments, preceeded by the name.</param>
	public delegate void ConsoleCommand(string name, string[] args, string fullCommand);
	/// <summary>
	/// Provides access to CryEngine Console API.
	/// </summary>
	public static class CryConsole
	{
		#region Fields
		private static readonly SortedList<string, ConsoleCommand> registeredCommands =
			new SortedList<string, ConsoleCommand>();
		#endregion
		#region Properties

		#endregion
		#region Events

		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Registers a console command.
		/// </summary>
		/// <param name="name">     Name of the command to register.</param>
		/// <param name="command">  
		/// Delegate that represents a method to invoke when the command is executed.
		/// </param>
		/// <param name="help">     
		/// Text to display when the user enters the name of the command followed by the question mark into
		/// the console.
		/// </param>
		/// <param name="flags">    A set of flags to assign to the command.</param>
		/// <param name="overwrite">
		/// If true and there is already a command with a given name, then the existing one will be
		/// unregistered. Such detection is only possible with commands that were registered using CryCIL
		/// API.
		/// </param>
		/// <returns>
		/// True, if the command was successfully registered. It is possible for <c>true</c> to be returned
		/// and have registration fail, if there was already a command with this name that was registered
		/// through native code.
		/// </returns>
		public static bool RegisterCommand(string name, ConsoleCommand command, string help = "", ConsoleFlags flags = ConsoleFlags.Null, bool overwrite = false)
		{
			bool registered = registeredCommands.ContainsKey(name);
			if (registered && !overwrite)
			{
				return false;
			}
			if (registered)
			{
				UnregisterCommand(name);
			}

			RegisterCommandInternal(name, help, flags);

			registeredCommands.Add(name, command);

			return true;
		}
		/// <summary>
		/// Unregisters the console command with a given name.
		/// </summary>
		/// <param name="name">Name of the console command to unregister.</param>
		public static void UnregisterCommand(string name)
		{
			registeredCommands.Remove(name);

			UnregisterCommandInternal(name);
		}
		/// <summary>
		/// Executes a console command.
		/// </summary>
		/// <remarks>
		/// When executing on the console commands that was registered using <see cref="RegisterCommand"/>
		/// function with <paramref name="silent"/> set to <c>true</c> and
		/// <paramref name="deferExecution"/> set to <c>false</c> normal execution procedure is bypassed
		/// and actual delegate is invoked immediately.
		/// </remarks>
		/// <param name="name">          Name of the command to execute.</param>
		/// <param name="arguments">     A list of arguments to pass to the command.</param>
		/// <param name="silent">        
		/// If true, suppresses error output when failing to execute the command and suppresses message
		/// that would normally be printed containing the full command line.
		/// </param>
		/// <param name="deferExecution">
		/// If true, the command is stored in special FIFO collection that allows delayed execution by
		/// using wait_seconds and wait_frames commands.
		/// </param>
		public static void ExecuteCommand(string name, string[] arguments, bool silent = true, bool deferExecution = false)
		{
			var builder = new StringBuilder(name.Length + arguments.Sum(x => x.Length) + arguments.Length);
			builder.Append(name);
			foreach (string argument in arguments)
			{
				builder.Append(' ');
				builder.Append(argument);
			}

			string commandLine = builder.ToString();

			if (silent && !deferExecution)
			{
				ConsoleCommand command;
				if (registeredCommands.TryGetValue(name, out command))
				{
					command(name, arguments, commandLine);
				}
			}
			else
			{
				ExecuteCommand(commandLine, silent, deferExecution);
			}
		}
		/// <summary>
		/// Executes a console command.
		/// </summary>
		/// <param name="command">       A command line (e.g. "map testy", no leading slash).</param>
		/// <param name="silent">        
		/// If true, suppresses error output when failing to execute the command and suppresses message
		/// that would normally be printed containing the full command line.
		/// </param>
		/// <param name="deferExecution">
		/// If true, the command is stored in special FIFO collection that allows delayed execution by
		/// using wait_seconds and wait_frames commands.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCommand(string command, bool silent = true, bool deferExecution = false);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterCommandInternal(string name, string help, ConsoleFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnregisterCommandInternal(string name);
		[PublicAPI("Invoked by underlying framework to execute appropriate console command.")]
		private static void ExecuteMonoCommand(string commandLine)
		{
			string[] parts = commandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			string[] args = new string[parts.Length - 1];
			string name = parts[0];

			ConsoleCommand command;
			if (registeredCommands.TryGetValue(name, out command))
			{
				Array.Copy(parts, 1, args, 0, args.Length);

				command(name, args, commandLine);
			}
		}
		#endregion
	}
}