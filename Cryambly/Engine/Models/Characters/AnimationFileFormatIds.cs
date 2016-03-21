using System;
using System.Linq;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of identifiers of file formats that contain animation info.
	/// </summary>
	public enum AnimationFileFormatIds
	{
		/// <summary>
		/// Identifier of .chr files that are used for characters and complicated animation sets.
		/// </summary>
		CHR = 0x11223344,
		/// <summary>
		/// Identifier of .cga files that are used for simple animation sets.
		/// </summary>
		CGA = 0x55aa55aa
	}
}