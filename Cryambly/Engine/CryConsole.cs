using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

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
	/// Represents static functions that can be invoked to react to changes to the console variable.
	/// </summary>
	/// <param name="var">A wrapper for a console variable that has been changed.</param>
	public delegate void ConsoleVariableCallback(ConsoleVariable var);
	/// <summary>
	/// Provides access to CryEngine Console API.
	/// </summary>
	[SuppressMessage("ReSharper", "ExceptionNotThrown")]
	public static class CryConsole
	{
		#region Fields
		private static readonly SortedList<string, ConsoleCommand> registeredCommands =
			new SortedList<string, ConsoleCommand>();
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
		/// <exception cref="ArgumentNullException">
		/// Name of the command to register must not be null.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// The delegate that represents a command cannot be null.
		/// </exception>
		public static bool RegisterCommand(string name, ConsoleCommand command, string help = null,
										   ConsoleFlags flags = ConsoleFlags.Null, bool overwrite = false)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Name of the command to register must not be null.");
			}
			if (command == null)
			{
				throw new ArgumentNullException("command", "The delegate that represents a command cannot be null.");
			}

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
		/// <exception cref="ArgumentNullException">
		/// Name of the command to unregister must not be null.
		/// </exception>
		public static void UnregisterCommand(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name",
												"Name of the command to unregister must not be null.");
			}

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
		/// <exception cref="ArgumentNullException">
		/// Name of the command to execute must not be null.
		/// </exception>
		/// <exception cref="OverflowException">
		/// The sum is larger than <see cref="F:System.Int32.MaxValue"/>.
		/// </exception>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		public static void ExecuteCommand(string name, string[] arguments, bool silent = true, bool deferExecution = false)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Name of the command to execute must not be null.");
			}

			int argumentsArrayLength;
			int argummentsCombinedLength;

			if (arguments == null)
			{
				argumentsArrayLength = 0;
				argummentsCombinedLength = 0;
			}
			else
			{
				argumentsArrayLength = arguments.Length;
				argummentsCombinedLength = arguments.Sum(x => x.Length);
			}

			var builder = new StringBuilder(name.Length + argummentsCombinedLength + argumentsArrayLength);
			builder.Append(name);
			for (int i = 0; i < argumentsArrayLength; i++)
			{
				builder.Append(' ');
				// NullReferenceException is not possible here since argumentsArrayLength will be equal to
				// 0.

				// ReSharper disable PossibleNullReferenceException
				builder.Append(arguments[i]);
				// ReSharper restore PossibleNullReferenceException
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
		/// <exception cref="ArgumentNullException">
		/// Name of the command to execute must not be null.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ExecuteCommand(string command, bool silent = true, bool deferExecution = false);

		/// <summary>
		/// Registers a console variable.
		/// </summary>
		/// <param name="name">        Name of the console variable.</param>
		/// <param name="var">         
		/// A reference to the integer variable that will be updated by the console system, when console
		/// variable is changed. Don't pass a reference to a simple variable, pass a static field.
		/// </param>
		/// <param name="defaultValue">Default value of the console variable.</param>
		/// <param name="flags">       
		/// A set of flags that describes the variable in technical detail.
		/// </param>
		/// <param name="help">        
		/// Text to display when the user enters the name of the console variable followed by the question
		/// mark into the console.
		/// </param>
		/// <returns>
		/// A wrapper for a newly created console variable. Check <see cref="ConsoleVariable.Valid"/>
		/// property on the returned object to see if registration was successful.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot register a console variable using a null name.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ConsoleVariable RegisterVariable(string name, ref int var, int defaultValue,
															  ConsoleFlags flags, string help = null);
		/// <summary>
		/// Registers a console variable.
		/// </summary>
		/// <param name="name">        Name of the console variable.</param>
		/// <param name="var">         
		/// A reference to the floating-point number variable that will be updated by the console system,
		/// when console variable is changed. Don't pass a reference to a simple variable, pass a static
		/// field.
		/// </param>
		/// <param name="defaultValue">Default value of the console variable.</param>
		/// <param name="flags">       
		/// A set of flags that describes the variable in technical detail.
		/// </param>
		/// <param name="help">        
		/// Text to display when the user enters the name of the console variable followed by the question
		/// mark into the console.
		/// </param>
		/// <returns>
		/// A wrapper for a newly created console variable. Check <see cref="ConsoleVariable.Valid"/>
		/// property on the returned object to see if registration was successful.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot register a console variable using a null name.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ConsoleVariable RegisterVariable(string name, ref float var,
															  float defaultValue, ConsoleFlags flags,
															  string help = null);
		/// <summary>
		/// Registers a new console variable that is a floating-point number.
		/// </summary>
		/// <example>
		/// <para>
		/// It's important that <paramref name="callback"/> catches any unhandled exceptions and doesn't
		/// raise any of its own:
		/// </para>
		/// <code>
		/// internal static void ExampleCallback(ConsoleVariable v)
		/// {
		///     try
		///     {
		///         // Do stuff.
		///     }
		///     catch				// Intercept ALL exceptions.
		///     { }					// And just ignore them. Not doing so is guaranteed to crash the app.
		/// }
		/// </code>
		/// </example>
		/// <param name="name">    Name of the console variable.</param>
		/// <param name="value">   Default value of the console variable.</param>
		/// <param name="flags">   A set of flags that describes the variable in technical detail.</param>
		/// <param name="callback">
		/// Static method that will be invoked, when the value of the console variable changes. Can be
		/// null.
		/// </param>
		/// <param name="help">    
		/// Text to display when the user enters the name of the console variable followed by the question
		/// mark into the console.
		/// </param>
		/// <returns>
		/// A wrapper for a newly created console variable. Check <see cref="ConsoleVariable.Valid"/>
		/// property on the returned object to see if registration was successful.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot register a console variable using a null name.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Unable to use instance method as a console variable callback.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the necessary permission to perform this operation.
		/// </exception>
		/// <exception cref="MemberAccessException">
		/// The caller does not have access to the method represented by the delegate (for example, if the
		/// method is private).
		/// </exception>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		public static ConsoleVariable RegisterVariable(string name, float value, ConsoleFlags flags,
													   ConsoleVariableCallback callback = null,
													   string help = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Cannot register a console variable using a null name.");
			}
			if (callback != null && !callback.Method.IsStatic)
			{
				throw new ArgumentException("Unable to use instance method as a console variable callback.");
			}

			IntPtr fPtr = callback != null
				? callback.Method.MethodHandle.GetFunctionPointer()
				: IntPtr.Zero;

			return RegisterVariableInternal(name, value, flags, fPtr, help);
		}
		/// <summary>
		/// Registers a new console variable that is an integer number.
		/// </summary>
		/// <example>
		/// <para>
		/// It's important that <paramref name="callback"/> catches any unhandled exceptions and doesn't
		/// raise any of its own:
		/// </para>
		/// <code>
		/// internal static void ExampleCallback(ConsoleVariable v)
		/// {
		///     try
		///     {
		///         // Do stuff.
		///     }
		///     catch				// Intercept ALL exceptions.
		///     { }					// And just ignore them. Not doing so is guaranteed to crash the app.
		/// }
		/// </code>
		/// </example>
		/// <param name="name">    Name of the console variable.</param>
		/// <param name="value">   Default value of the console variable.</param>
		/// <param name="flags">   A set of flags that describes the variable in technical detail.</param>
		/// <param name="callback">
		/// Static method that will be invoked, when the value of the console variable changes. Can be
		/// null.
		/// </param>
		/// <param name="help">    
		/// Text to display when the user enters the name of the console variable followed by the question
		/// mark into the console.
		/// </param>
		/// <returns>
		/// A wrapper for a newly created console variable. Check <see cref="ConsoleVariable.Valid"/>
		/// property on the returned object to see if registration was successful.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot register a console variable using a null name.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Unable to use instance method as a console variable callback.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the necessary permission to perform this operation.
		/// </exception>
		/// <exception cref="MemberAccessException">
		/// The caller does not have access to the method represented by the delegate (for example, if the
		/// method is private).
		/// </exception>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		public static ConsoleVariable RegisterVariable(string name, int value, ConsoleFlags flags,
													   ConsoleVariableCallback callback = null,
													   string help = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Cannot register a console variable using a null name.");
			}
			if (callback != null && !callback.Method.IsStatic)
			{
				throw new ArgumentException("Unable to use instance method as a console variable callback.");
			}

			IntPtr fPtr = callback != null
				? callback.Method.MethodHandle.GetFunctionPointer()
				: IntPtr.Zero;

			return RegisterVariableInternal(name, value, flags, fPtr, help);
		}
		/// <summary>
		/// Registers a new console variable that is a text value.
		/// </summary>
		/// <example>
		/// <para>
		/// It's important that <paramref name="callback"/> catches any unhandled exceptions and doesn't
		/// raise any of its own:
		/// </para>
		/// <code>
		/// internal static void ExampleCallback(ConsoleVariable v)
		/// {
		///     try
		///     {
		///         // Do stuff.
		///     }
		///     catch				// Intercept ALL exceptions.
		///     { }					// And just ignore them. Not doing so is guaranteed to crash the app.
		/// }
		/// </code>
		/// </example>
		/// <param name="name">    Name of the console variable.</param>
		/// <param name="value">   Default value of the console variable.</param>
		/// <param name="flags">   A set of flags that describes the variable in technical detail.</param>
		/// <param name="callback">
		/// Static method that will be invoked, when the value of the console variable changes. Can be
		/// null.
		/// </param>
		/// <param name="help">    
		/// Text to display when the user enters the name of the console variable followed by the question
		/// mark into the console.
		/// </param>
		/// <returns>
		/// A wrapper for a newly created console variable. Check <see cref="ConsoleVariable.Valid"/>
		/// property on the returned object to see if registration was successful.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot register a console variable using a null name.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Unable to use instance method as a console variable callback.
		/// </exception>
		/// <exception cref="SecurityException">
		/// The caller does not have the necessary permission to perform this operation.
		/// </exception>
		/// <exception cref="MemberAccessException">
		/// The caller does not have access to the method represented by the delegate (for example, if the
		/// method is private).
		/// </exception>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		public static ConsoleVariable RegisterVariable(string name, string value, ConsoleFlags flags,
													   ConsoleVariableCallback callback = null,
													   string help = null)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name", "Cannot register a console variable using a null name.");
			}
			if (callback != null && !callback.Method.IsStatic)
			{
				throw new ArgumentException("Unable to use instance method as a console variable callback.");
			}

			IntPtr fPtr = callback != null
				? callback.Method.MethodHandle.GetFunctionPointer()
				: IntPtr.Zero;

			return RegisterVariableInternal(name, value, flags, fPtr, help);
		}
		/// <summary>
		/// Unregisters a console variable.
		/// </summary>
		/// <param name="name">  Name of the variable to unregister.</param>
		/// <param name="delete">
		/// Indicates whether there no <see cref="ConsoleVariable"/> objects for this variable hanging
		/// around and it can be deleted.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Name of the console variable to unregister cannot be null.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnregisterVariable(string name, bool delete = true);
		/// <summary>
		/// Gets console variable.
		/// </summary>
		/// <param name="name">Name of the console variable to retreive.</param>
		/// <returns>
		/// A wrapper for a console variable. Check <see cref="ConsoleVariable.Valid"/> property on the
		/// returned object to see if it was registered before.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot get a console variable using a null name.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ConsoleVariable GetVariable(string name);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterCommandInternal(string name, string help, ConsoleFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnregisterCommandInternal(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ConsoleVariable RegisterVariableInternal(string name, float value,
																	   ConsoleFlags flags, IntPtr thunk,
																	   string help = null);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ConsoleVariable RegisterVariableInternal(string name, int value,
																	   ConsoleFlags flags, IntPtr thunk,
																	   string help = null);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ConsoleVariable RegisterVariableInternal(string name, string value,
																	   ConsoleFlags flags, IntPtr thunk,
																	   string help = null);
		[UnmanagedThunk("Invoked by underlying framework to execute appropriate console command.")]
		[SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
		[SuppressMessage("ReSharper", "ExceptionNotDocumented")]
		private static void ExecuteMonoCommand(string commandLine)
		{
			string[] parts = commandLine.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

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