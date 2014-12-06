using System;
using System.Text;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Contains methods useful for tracking down bugs.
	/// </summary>
	public static partial class Debug
	{
		static Debug()
		{
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionOccurred;
		}

		private static void UnhandledExceptionOccurred(object sender, UnhandledExceptionEventArgs e)
		{
			var exception = e.ExceptionObject as Exception;

			// The CLS doesn't force exceptions to derive from System.Exception
			if (exception == null)
				throw new NotSupportedException("An exception that does not derive from System.Exception was thrown.");

			DisplayException(exception);
		}

		/// <summary>
		/// Logs a message to the console
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Log(string format, LogType type = LogType.Message, params object[] args)
		{
#if UNIT_TESTING
            System.Console.WriteLine(format, args);
#else
			DebugInterop.Log(String.Format(format, args), type);
#endif
		}

		/// <summary>
		/// Logs a message to the console
		/// </summary>
		public static void Log(string msg, LogType type = LogType.Message)
		{
#if UNIT_TESTING
            System.Console.WriteLine(msg);
#else
			DebugInterop.Log(msg, type);
#endif
		}

		/// <summary>
		/// Logs a message to the console, regardless of log_verbosity settings
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void LogAlways(string format, params object[] args)
		{
#if UNIT_TESTING
            System.Console.WriteLine(format, args);
#else
			DebugInterop.Log(String.Format(format, args), LogType.Always);
#endif
		}

		/// <summary>
		/// Logs a message to the console, regardless of log_verbosity settings
		/// </summary>
		public static void LogAlways(string msg)
		{
#if UNIT_TESTING
            System.Console.WriteLine(msg);
#else
			DebugInterop.Log(msg, LogType.Always);
#endif
		}

		/// <summary>
		/// Logs an error to console
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void LogError(string format, params object[] args)
		{
#if UNIT_TESTING
            System.Console.WriteLine(format, args);
#else
			DebugInterop.Log(String.Format(format, args), LogType.Error);
#endif
		}

		/// <summary>
		/// Logs a message to the console, regardless of log_verbosity settings
		/// </summary>
		public static void LogError(string msg)
		{
#if UNIT_TESTING
            System.Console.WriteLine(msg);
#else
			DebugInterop.Log(msg, LogType.Error);
#endif
		}

		/// <summary>
		/// Logs an exception message to the console
		/// </summary>
		/// <remarks>
		/// Useful when exceptions are caught and data is still needed from them
		/// </remarks>
		/// <param name="ex"></param>
		public static void LogException(Exception ex)
		{
			LogWarning(ex.ToString());
		}

		/// <summary>
		/// Displays an exception via the CryMono exception form.
		/// </summary>
		/// <param name="ex">The exception that occurred</param>
		/// <param name="fatal">Indicates if this exception is fatal</param>
		public static void DisplayException(Exception ex, bool fatal = false)
		{
			// Log exception as well
			LogException(ex);

			var form = new ExceptionMessage(ex, fatal);
			form.ShowDialog();
		}

		/// <summary>
		/// Outputs a warning message
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void LogWarning(string format, params object[] args)
		{
#if UNIT_TESTING
            System.Console.WriteLine(format, args);
#else
			DebugInterop.Warning(String.Format(format, args));
#endif
		}

		/// <summary>
		/// Outputs a warning message
		/// </summary>
		public static void LogWarning(string msg)
		{
#if UNIT_TESTING
            System.Console.WriteLine("[Warning]" + msg);
#else
			DebugInterop.Warning(msg);
#endif
		}

		public static void LogStackTrace(LogType type = LogType.Message)
		{
			var stackTrace = new System.Diagnostics.StackTrace(true);
			Debug.LogAlways("Stack trace:");
			for (int i = 1; i < stackTrace.FrameCount; i++)
			{
				var frame = stackTrace.GetFrame(i);

				var method = frame.GetMethod();
				var fileName = frame.GetFileName() ?? "<unknown>";

				Debug.Log("  at {0}.{1}.{2} () in {3}:{4}", type, method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name, fileName, frame.GetFileLineNumber());
			}
		}
	}
}