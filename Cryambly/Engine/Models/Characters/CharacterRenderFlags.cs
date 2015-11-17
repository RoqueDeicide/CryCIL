using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// A set of flags that are used to specify hot to render animated characters.
	/// </summary>
	[Flags]
	public enum CharacterRenderFlags
	{
		/// <summary>
		/// When set, specifies that a main model geometry must be rendered.
		/// </summary>
		DrawModel = 1 << 0,
		/// <summary>
		/// When set, specifies that the character must be rendered in post render. (used for weapons and
		/// hands in first-person mode(?)).
		/// </summary>
		DrawNear = 1 << 1,
		/// <summary>
		/// When set, specifies that the character instance needs to be updated.
		/// </summary>
		Update = 1 << 2,
		/// <summary>
		/// When set, specifies that the character instance needs to be updated no matter what (used for
		/// player characters).
		/// </summary>
		UpdateAlways = 1 << 3,
		/// <summary>
		/// When set, specifies that (?).
		/// </summary>
		CompoundBase = 1 << 4,

		/// <summary>
		/// When set, specifies that the character must be rendered with its vertices and edges visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawWireframe = 1 << 5,
		/// <summary>
		/// When set, specifies that character must be rendered with its tangent-space normals visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawTangents = 1 << 6,
		/// <summary>
		/// When set, specifies that character must be rendered with its binormals visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawBinormals = 1 << 7,
		/// <summary>
		/// When set, specifies that character must be rendered with its normals visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawNormals = 1 << 8,

		/// <summary>
		/// When set, specifies that character must be rendered with its locator helper visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawLocator = 1 << 9,
		/// <summary>
		/// When set, specifies that character must be rendered with its skeleton visualized.
		/// </summary>
		/// <remarks>Used for debug purposes.</remarks>
		DrawSkeleton = 1 << 10,

		/// <summary>
		/// When set, specifies that (?).
		/// </summary>
		BiasSkinSortDist = 1 << 11,

		/// <summary>
		/// When set, specifies that (?).
		/// </summary>
		StreamHighPriority = 1 << 12
	}
}