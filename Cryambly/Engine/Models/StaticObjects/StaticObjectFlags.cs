using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of flags that can be assigned to static objects.
	/// </summary>
	[Flags]
	public enum StaticObjectFlags
	{
		/// <summary>
		/// When set, specifies that this static object must not be rendered.
		/// </summary>
		Hidden = 1 << 0,
		/// <summary>
		/// When set, specifies that this static object was cloned for modification.
		/// </summary>
		Clone = 1 << 1,
		/// <summary>
		/// When set, specifies that this static object was generated procedurally (breakable object, etc).
		/// </summary>
		Generated = 1 << 2,
		/// <summary>
		/// When set, specifies that this static object contains geometry that is not suitable for procedural breaking.
		/// </summary>
		CantBreak = 1 << 3,
		/// <summary>
		/// When set, specifies that this static object is deformable.
		/// </summary>
		Deformable = 1 << 4,
		/// <summary>
		/// When set, specifies that this static object has meshes inside sub-objects.
		/// </summary>
		Compound = 1 << 5,
		/// <summary>
		/// When set, specifies that this static object is referenced by several parents.
		/// </summary>
		MultipleParents = 1 << 6,

		/// <summary>
		/// When set, specifies that this static object must not collide with the player.
		/// </summary>
		NoPlayerCollide = 1 << 10,

		/// <summary>
		/// When set, specifies that this static object spawns an entity when broken.
		/// </summary>
		SpawnEntity = 1 << 20,
		/// <summary>
		/// When set, specifies that this static object can be picked up by players.
		/// </summary>
		Pickable = 1 << 21,
		/// <summary>
		/// When set, specifies that this static object cannot be used as static cover by AI.
		/// </summary>
		NoAutoHidePoints = 1 << 22,
		/// <summary>
		/// When set, specifies that this static object must contain its mesh data in system memory.
		/// </summary>
		Dynamic = 1 << 23
	}
}