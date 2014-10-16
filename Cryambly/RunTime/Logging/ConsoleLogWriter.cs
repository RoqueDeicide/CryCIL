﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Engine.DebugServices;

namespace CryCil.RunTime.Logging
{
	// Used to redirect output of System.Console class to CryEngine log.
	internal partial class ConsoleLogWriter : TextWriter
	{
		private StringBuilder buffer;
		/// <summary>
		/// Gets a simple encoding.
		/// </summary>
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}
		/// <summary>
		/// Outputs text from current buffer to CryEngine log.
		/// </summary>
		public override void Flush()
		{
			string[] lines =									// Don't remove empty entries.
				buffer.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			if (lines.Length == 1)	// Only happens when there is no new line symbol in the buffer.
			{
				// This code is executed when Flush is called from external class, which
				// means we should post whatever we have in the buffer and empty it
				// afterwards.
				Log.Line(lines[0]);
				buffer = new StringBuilder("", 70);
			}
			else
			{
				// Print all of the lines except the last one.
				for (int i = 0; i < lines.Length - 1; i++)
				{
					Log.Line(lines[i]);
				}
				// Reset the buffer to be the last line. It will be empty if <value> ended
				// with new line symbol.
				buffer = new StringBuilder(lines[lines.Length - 1], 70);
			}
		}
	}
}