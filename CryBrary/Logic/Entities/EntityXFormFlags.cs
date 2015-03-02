namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Enumeration of flags that describe <see cref="EntityEvent.XForm"/> event.
	/// </summary>
	public enum EntityXFormFlags
	{
		/// <summary>
		/// If set, indicates that position of the entity was changed.
		/// </summary>
		Position = 1,
		/// <summary>
		/// If set, indicates that orientation of the entity was changed.
		/// </summary>
		Orientation = 2,
		/// <summary>
		/// If set, indicates that scale of the entity was changed.
		/// </summary>
		Scale = 4,
		/// <summary>
		/// If set, indicates that this event should not be propagated.
		/// </summary>
		NoPropogate = 8,
		/// <summary>
		/// If set, indicates that transformation of the parent entity was changed.
		/// </summary>
		FromParent = 16,
		/// <summary>
		/// If set, indicates that the entity was changed by physics engine.
		/// </summary>
		PhysicsStep = 32,
		/// <summary>
		/// If set, indicates that transformation of the entity was changed in the editor.
		/// </summary>
		Editor = 64,
		/// <summary>
		/// If set, indicates that transformation of the entity was changed in the TrackView.
		/// </summary>
		TrackView = 128,
		/// <summary>
		/// If set, indicates that transformation of the entity was changed in the demo.
		/// </summary>
		TimeDemo = 256,
		/// <summary>
		/// If set, indicates that this entity will not be re-registered in 3D engine.
		/// </summary>
		NotReregister = 512,
		/// <summary>
		/// If set, indicates that <see cref="EntityEvent.XForm"/> event is suppressed.
		/// </summary>
		NoEvent = 1024,
		/// <summary>
		/// If set, indicates that the event won't be sent to the entity system.
		/// </summary>
		NoSendToEntitySystem = 2048,
		/// <summary>
		/// Used by the user for whatever reason.
		/// </summary>
		User = 0x1000000,
	}
}