using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.RunTime;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents an interface between CryCIL and CryEngine debug log functionality.
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// Gets or sets log verbosity level.
		/// </summary>
		public static extern VerbosityLevels VerbosityLevel { [MethodImpl(MethodImplOptions.InternalCall)] get;
			[MethodImpl(MethodImplOptions.InternalCall)] set; }
		/// <summary>
		/// Writes a line into the log.
		/// </summary>
		/// <param name="value">Text to write.</param>
		public static void Line(string value)
		{
			Post(LogPostType.Message, value);
		}
		/// <summary>
		/// Writes a line into the log.
		/// </summary>
		/// <param name="format">String that specifies how to build the final text to post.</param>
		/// <param name="args">  
		/// An array of object which string representations must be inserted into final text according to
		/// <paramref name="format"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="format"/> or <paramref name="args"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		/// <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or
		/// greater than or equal to the length of the <paramref name="args"/> array.
		/// </exception>
		[StringFormatMethod("format")]
		public static void Line(string format, params object[] args)
		{
			Post(LogPostType.Message, string.Format(format, args));
		}
		/// <summary>
		/// Writes a line into the log that will be visible on any log verbosity level.
		/// </summary>
		/// <param name="value">Text to post.</param>
		public static void Always(string value)
		{
			Post(LogPostType.Always, value);
		}
		/// <summary>
		/// Writes a line into the log that will be visible on any log verbosity level.
		/// </summary>
		/// <param name="format">String that specifies how to build the final text to post.</param>
		/// <param name="args">  
		/// An array of object which string representations must be inserted into final text according to
		/// <paramref name="format"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="format"/> or <paramref name="args"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		/// <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or
		/// greater than or equal to the length of the <paramref name="args"/> array.
		/// </exception>
		[StringFormatMethod("format")]
		public static void Always(string format, params object[] args)
		{
			Post(LogPostType.Always, string.Format(format, args));
		}
		/// <summary>
		/// Writes a warning into the log. Displayed with yellow font.
		/// </summary>
		/// <param name="value">    Text to post.</param>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		public static void Warning(string value, bool important)
		{
			Post(important ? LogPostType.WarningAlways : LogPostType.Warning, value);
		}
		/// <summary>
		/// Writes a warning into the log. Displayed with yellow font.
		/// </summary>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		/// <param name="format">   String that specifies how to build the final text to post.</param>
		/// <param name="args">     
		/// An array of object which string representations must be inserted into final text according to
		/// <paramref name="format"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="format"/> or <paramref name="args"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		/// <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or
		/// greater than or equal to the length of the <paramref name="args"/> array.
		/// </exception>
		[StringFormatMethod("format")]
		public static void Warning(bool important, string format, params object[] args)
		{
			Post(important ? LogPostType.WarningAlways : LogPostType.Warning, string.Format(format, args));
		}
		/// <summary>
		/// Writes an error message into the log. Displayed with red font.
		/// </summary>
		/// <param name="value">    Text to post.</param>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		public static void Error(string value, bool important)
		{
			Post(important ? LogPostType.ErrorAlways : LogPostType.Error, value);
		}
		/// <summary>
		/// Writes an error message into the log. Displayed with red font.
		/// </summary>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		/// <param name="format">   String that specifies how to build the final text to post.</param>
		/// <param name="args">     
		/// An array of object which string representations must be inserted into final text according to
		/// <paramref name="format"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="format"/> or <paramref name="args"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		/// <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or
		/// greater than or equal to the length of the <paramref name="args"/> array.
		/// </exception>
		[StringFormatMethod("format")]
		public static void Error(bool important, string format, params object[] args)
		{
			Post(important ? LogPostType.ErrorAlways : LogPostType.Error, string.Format(format, args));
		}
		/// <summary>
		/// Posts a comment into the log.
		/// </summary>
		/// <param name="value">Text to post.</param>
		public static void Comment(string value)
		{
			Post(LogPostType.Comment, value);
		}
		/// <summary>
		/// Posts a comment into the log.
		/// </summary>
		/// <param name="format">String that specifies how to build the final text to post.</param>
		/// <param name="args">  
		/// An array of object which string representations must be inserted into final text according to
		/// <paramref name="format"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="format"/> or <paramref name="args"/> is null.
		/// </exception>
		/// <exception cref="FormatException">
		/// <paramref name="format"/> is invalid.-or- The index of a format item is less than zero, or
		/// greater than or equal to the length of the <paramref name="args"/> array.
		/// </exception>
		[StringFormatMethod("format")]
		public static void Comment(string format, params object[] args)
		{
			Post(LogPostType.Comment, string.Format(format, args));
		}
		/// <summary>
		/// Posts a line of text to the log.
		/// </summary>
		/// <param name="postType">Type of post to send.</param>
		/// <param name="text">    Text to post.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Post(LogPostType postType, string text);

		[RawThunk("Invoked to redirect the standard output to the ConsoleLogWriter.")]
		private static void RedirectStdOutput()
		{
			try
			{
				// Redirect Console output.
				Console.SetOut(new ConsoleLogWriter());

				// A simple test for redirected console output.
				Console.WriteLine("Standard output has been redirected.");
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
	}
	/// <summary>
	/// Enumeration of types of posts that can be sent to the log.
	/// </summary>
	public enum LogPostType
	{
		/// <summary>
		/// A simple message. Minimal log verbosity required for display is
		/// <see cref="VerbosityLevels.High"/>.
		/// </summary>
		Message,
		/// <summary>
		/// A warning message. Minimal log verbosity required for display is
		/// <see cref="VerbosityLevels.High"/>.
		/// </summary>
		Warning,
		/// <summary>
		/// An error message. Minimal log verbosity required for display is
		/// <see cref="VerbosityLevels.High"/>.
		/// </summary>
		Error,
		/// <summary>
		/// A simple message. Displayed on an log verbosity level.
		/// </summary>
		Always,
		/// <summary>
		/// A warning message. Displayed on an log verbosity level.
		/// </summary>
		WarningAlways,
		/// <summary>
		/// An error message. Displayed on an log verbosity level.
		/// </summary>
		ErrorAlways,
		/// <summary>
		/// For internal use.
		/// </summary>
		Input,
		/// <summary>
		/// For internal use.
		/// </summary>
		InputResponse,
		/// <summary>
		/// A simple message. Minimal log verbosity required for display is
		/// <see cref="VerbosityLevels.Full"/>.
		/// </summary>
		Comment
	}
	/// <summary>
	/// Enumeration of different verbosity levels.
	/// </summary>
	public enum VerbosityLevels
	{
		/// <summary>
		/// Only very important messages can be posted.
		/// </summary>
		Minimal,
		/// <summary>
		/// Error messages can also be posted.
		/// </summary>
		Low,
		/// <summary>
		/// Warnings can also be posted.
		/// </summary>
		Medium,
		/// <summary>
		/// Simple messages can also be posted.
		/// </summary>
		High,
		/// <summary>
		/// Everything can be posted.
		/// </summary>
		Full
	}
}