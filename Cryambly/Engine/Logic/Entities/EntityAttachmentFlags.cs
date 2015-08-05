using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that describe the way the entity can be attached to another.
	/// </summary>
	[Flags]
	public enum EntityAttachmentFlags
	{
		/// <summary>
		/// When set specifies that world transformation of entity should remain the same when attaching or
		/// detaching it.
		/// </summary>
		KeepTransformation = 1,
		/// <summary>
		/// When set specifies that the entity must be attached to a geometry cache node.
		/// </summary>
		GeometryCacheNode = 2,
		/// <summary>
		/// When set specifies that the entity must be attached to a character bone.
		/// </summary>
		CharacterBone = 4,
	}
}