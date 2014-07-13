using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Defines the list of supported editor types.
	/// </summary>
	public enum EditorPropertyType
	{
		Bool,
		Int,
		Float,
		Vec3,
		String,
		Entity,
		FolderBegin,
		FolderEnd,
		Object,
		Texture,
		File,
		Sound,
		Dialogue,
		Color,
		Sequence
	}
}