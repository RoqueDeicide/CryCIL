namespace CryCil
{
	// A Regex pattern for detecting just imported soon-to-be-internal calls:
	//
	//(?<ws>[ \t]+)virtual\s+(?<returnType>[a-zA-Z0-9_(::)\s]+\s*\**\&*\s*\&*\**)\s*\b(?<funcName>[a-zA-Z0-9_]+)\((?<argList>.*)\)\s*(const)*\s*=\s*0;
	//
	// A replacement string for creating internal calls from imported function names:
	//
	//${ws}[MethodImpl(MethodImplOptions.InternalCall)]\r\n${ws}private static extern ${returnType} ${funcName}(IntPtr handle, ${argList});

	/// <summary>
	/// Defines some common constants.
	/// </summary>
	public static class VariousConstants
	{
		/// <summary>
		/// Pattern that allows to find characters that are not valid for XML tag names.
		/// </summary>
		public const string InvalidXmlCharsPattern = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
		/// <summary>
		/// The constant that contains the size of pointers in bytes.
		/// </summary>
		public const int PointerSize = 
#if WIN64
			8
#else
			4
#endif
						 ;
	}
}