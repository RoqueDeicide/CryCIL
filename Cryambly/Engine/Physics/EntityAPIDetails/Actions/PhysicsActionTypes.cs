namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of types of actions that can be applied to physical entities.
	/// </summary>
	public enum PhysicsActionTypes
	{
		/// <summary>
		/// Identifier of the action that moves the physical entity.
		/// </summary>
		Move = 1,
		/// <summary>
		/// Identifier of the action that applies impulse to the physical entity.
		/// </summary>
		Impulse = 2,
		/// <summary>
		/// Identifier of the action that drives the physical entity that is a vehicle.
		/// </summary>
		Drive = 3,
		/// <summary>
		/// Identifier of the action that resets the physical entity.
		/// </summary>
		Reset = 4,
		/// <summary>
		/// Identifier of the action that adds constraint to the physical entity.
		/// </summary>
		AddConstraint = 5,
		/// <summary>
		/// Identifier of the action that updates constraint in the physical entity.
		/// </summary>
		UpdateConstraint = 6,
		/// <summary>
		/// Identifier of the action that applies a fake collision to the physical entity.
		/// </summary>
		RegisterCollisionEvent = 7,
		/// <summary>
		/// Identifier of the action that awakes the physical entity.
		/// </summary>
		Awake = 8,
		/// <summary>
		/// Identifier of the action that removes all articulated parts from the physical entity.
		/// </summary>
		RemoveAllParts = 9,
		/// <summary>
		/// Identifier of the action that sets velocity of the physical entity.
		/// </summary>
		SetVelocity = 10,
		/// <summary>
		/// Identifier of the action that attaches points to the physical entity that is a soft body.
		/// </summary>
		AttachPoints = 11,
		/// <summary>
		/// Identifier of the action that sets the target pose for the physical entity that is a rope.
		/// </summary>
		SetRopeTargetPose = 12,
		/// <summary>
		/// Identifier of the action that resets the transformation matrix of one of the parts of the
		/// physical entity.
		/// </summary>
		ResetPartMatrix = 13,
		/// <summary>
		/// Identifier of the action that notifies the physical entity that is a rope that an entity it was
		/// attached to was moved.
		/// </summary>
		Notify = 14,
		/// <summary>
		/// Identifier of the action that specifies the conditions for automatic detachment of parts of the
		/// physical entity that is an articulated body with pre-baked physics simulation.
		/// </summary>
		AutoPartDetachment = 15,
		/// <summary>
		/// Identifier of the action that that transfers a part from one physical entity to another.
		/// </summary>
		MoveParts = 16,
		/// <summary>
		/// Identifier of the action that updates transformations of a number of parts of the physical
		/// entity.
		/// </summary>
		BatchPartsUpdate = 17,
		/// <summary>
		/// Identifier of the action that slices the physical entity with a plane.
		/// </summary>
		Slice = 18,
		/// <summary>
		/// Number of types of actions.
		/// </summary>
		Count
	}
}