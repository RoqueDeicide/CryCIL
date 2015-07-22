using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that can be applied to entity classes.
	/// </summary>
	[Flags]
	public enum EntityClassFlags : uint
	{
		/// <summary>
		/// If set this class will not be visible in editor,and entity of this class cannot be placed
		/// manually in editor.
		/// </summary>
		Invisible = 1,
		/// <summary>
		/// If set this is default entity class.
		/// </summary>
		/// <remarks>Not sure what this does.</remarks>
		Default = 2,
		/// <summary>
		/// If set entity of this class can be selected by bounding box in the editor 3D view.
		/// </summary>
		BoundingBoxSelection = 4,
		/// <summary>
		/// If set the entity of this class won't be assigned a static id on creation when it's a part of
		/// the level.
		/// </summary>
		DoNotSpawnAsStatic = 8,
		/// <summary>
		/// If set modify an existing class with the same name.
		/// </summary>
		/// <remarks>Can be used to override entity classes when creating a mod for a game.</remarks>
		ModifyExisting = 16
	}
}