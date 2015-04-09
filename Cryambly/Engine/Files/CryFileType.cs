using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Files
{
	/// <summary>
	/// Enumeration of types files in CryEngine virtual file system can be treated as.
	/// </summary>
	public enum CryFileType
	{
		/// <summary>
		/// File will be treated as a binary file.
		/// </summary>
		Binary,
		/// <summary>
		/// File will be treated as a text file. This option is not available for files inside .pak files.
		/// </summary>
		Text
	}
}
