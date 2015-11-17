using System;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of modes files in CryEngine virtual file system can be opened in.
	/// </summary>
	public enum CryFileMode
	{
		/// <summary>
		/// File will be opened in 'Read' mode.
		/// </summary>
		Read,
		/// <summary>
		/// File will be opened in 'Write' mode and if it had any contents previously they will be wiped.
		/// </summary>
		Write,
		/// <summary>
		/// File will be opened in 'Write' mode and it's stream position will be set to the end of the
		/// file.
		/// </summary>
		Append
	}
}