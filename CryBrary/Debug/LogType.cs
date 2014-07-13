using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum LogType
	{
		Message = 0,
		Warning,
		Error,
		Always,
		WarningAlways,
		ErrorAlways,
		/// <summary>
		/// e.g. "e_CaptureFolder ?" or all printouts from history or auto completion
		/// </summary>
		Input,
		/// <summary>
		/// e.g. "Set output folder for video capturing" in response to "e_CaptureFolder ?"
		/// </summary>
		InputResponse,
		Comment
	}
}