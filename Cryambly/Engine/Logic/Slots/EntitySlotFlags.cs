using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that describe an entity slot.
	/// </summary>
	[Flags]
	public enum EntitySlotFlags
	{
		/// <summary>
		/// When set, specifies that this slot should rendered as normal.
		/// </summary>
		Render = 0x0001,
		/// <summary>
		/// When set, specifies that this slot should rendered in camera space (can be used to render a
		/// weapon and hands in First-Person Shooter).
		/// </summary>
		RenderNearest = 0x0002,
		/// <summary>
		/// When set, specifies that this slot should rendered using a custom camera that can be set via a
		/// shader parameter.
		/// </summary>
		RenderWithCustomCamera = 0x0004,
		/// <summary>
		/// When set, specifies that this slot should ignore physics events that are sent to it.
		/// </summary>
		IgnorePhysics = 0x0008,
		/// <summary>
		/// When set, specifies that this slot should break when the entity
		/// </summary>
		BreakAsEntity = 0x0010,
		/// <summary>
		/// When set, specifies that this slot should be rendered after post-processing is applied to the
		/// scene.
		/// </summary>
		RenderAfterPostProcessing = 0x0020,
		/// <summary>
		/// When set, specifies that this slot should be broken without raising an event in multiplayer.
		/// </summary>
		BreakAsEntityMp = 0x0040,
	}
}