using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public static VerbosityLevels VerbosityLevel
		{
			get { return (VerbosityLevels)Interops.LogPosting.GetVerboxity(); }
			set { Interops.LogPosting.SetVerbosity((int)value); }
		}
		/// <summary>
		/// Writes a line into the log.
		/// </summary>
		/// <param name="value">Text to write.</param>
		public static void Line(string value)
		{
			Log.Post(LogPostType.Message, value);
		}
		/// <summary>
		/// Writes a line into the log that will be visible on any log verbosity level.
		/// </summary>
		/// <param name="value">Text to post.</param>
		public static void Always(string value)
		{
			Log.Post(LogPostType.Always, value);
		}
		/// <summary>
		/// Writes a warning into the log. Displayed with yellow font.
		/// </summary>
		/// <param name="value">    Text to post.</param>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		public static void Warning(string value, bool important)
		{
			Log.Post(important ? LogPostType.WarningAlways : LogPostType.Warning, value);
		}
		/// <summary>
		/// Writes an error message into the log. Displayed with red font.
		/// </summary>
		/// <param name="value">    Text to post.</param>
		/// <param name="important">Indicates whether the message is very important to post.</param>
		public static void Error(string value, bool important)
		{
			Log.Post(important ? LogPostType.ErrorAlways : LogPostType.Error, value);
		}
		/// <summary>
		/// Posts a comment into the log.
		/// </summary>
		/// <param name="value">Text to post.</param>
		public static void Comment(string value)
		{
			Log.Post(LogPostType.Comment, value);
		}
		/// <summary>
		/// Posts a line of text to the log.
		/// </summary>
		/// <param name="postType">Type of post to send.</param>
		/// <param name="text">    Text to post.</param>
		public static void Post(LogPostType postType, string text)
		{
			Interops.LogPosting.Post((int)postType, text);
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