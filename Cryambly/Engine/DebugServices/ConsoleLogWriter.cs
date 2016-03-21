using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents an object that can be used to redirect output of <see cref="Console"/> class to CryEngine
	/// <see cref="Log"/>.
	/// </summary>
	public partial class ConsoleLogWriter : TextWriter
	{
		/// <summary>
		/// Type of posts logged by an instance of type <see cref="ConsoleLogWriter"/>.
		/// </summary>
		public static LogPostType PostType = LogPostType.Message;
		private StringBuilder buffer;
		/// <summary>
		/// Gets a simple encoding.
		/// </summary>
		public override Encoding Encoding => Encoding.UTF8;
		/// <summary>
		/// Creates a default instance of this class.
		/// </summary>
		public ConsoleLogWriter()
		{
			this.buffer = new StringBuilder(100);
		}
		/// <summary>
		/// Outputs text from current buffer to CryEngine log.
		/// </summary>
		public override void Flush()
		{
			string[] lines = // Don't remove empty entries.
				this.buffer.ToString().Split(new[] {System.Environment.NewLine}, StringSplitOptions.None);
			if (lines.Length == 1) // Only happens when there is no new line symbol in the buffer.
			{
				// This code is executed when Flush is called from external class, which means we should
				// post whatever we have in the buffer and empty it afterwards.
				Log.Post(PostType, lines[0]);
				this.buffer = new StringBuilder("", 70);
			}
			else
			{
				// Print all of the lines except the last one.
				for (int i = 0; i < lines.Length - 1; i++)
				{
					Log.Post(PostType, lines[i]);
				}
				// Reset the buffer to be the last line. It will be empty if <value> ended with new line
				// symbol.
				try
				{
					this.buffer = new StringBuilder(lines[lines.Length - 1], 70);
				}
				catch (OverflowException)
				{
				}
			}
		}
	}
}