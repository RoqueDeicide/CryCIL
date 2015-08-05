using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of flags that indicate how the X-form transformation of the entity has changed.
	/// </summary>
	[Flags]
	public enum EntityXFormChange
	{
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being moved in
		/// world space.
		/// </summary>
		Position = 1 << 1,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being rotated.
		/// </summary>
		Orientation = 1 << 2,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being scaled.
		/// </summary>
		Scale = 1 << 3,
		/// <summary>
		/// When set indicates that entity's X-Form transformation event should not be propagated to other
		/// game instances in the network.
		/// </summary>
		NoPropagate = 1 << 4,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity's parent being
		/// moved.
		/// </summary>
		FromParent = 1 << 5,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being moved by
		/// the physics engine.
		/// </summary>
		PhysicsStep = 1 << 13,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being moved in
		/// the editor.
		/// </summary>
		Editor = 1 << 14,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being moved in
		/// the TrackView sequence.
		/// </summary>
		TrackView = 1 << 15,
		/// <summary>
		/// When set indicates that entity's X-Form transformation has changed due to entity being moved in
		/// the recorded demo.
		/// </summary>
		TimeDemo = 1 << 16,

		//NOT_REREGISTER = 1 << 17, // An optimization flag, when set object will not be re-registered in 3D engine.
		//NO_EVENT = 1 << 18, // suppresses ENTITY_EVENT_XFORM event
		//NO_SEND_TO_ENTITY_SYSTEM = 1 << 19,
		//USER = 0x1000000,
	}
}