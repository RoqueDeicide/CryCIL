using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that specify the constraint that binds to physical entities together.
	/// </summary>
	[Flags]
	public enum ConstraintFlags : uint
	{
		/// <summary>
		/// When set, specifies that attachment points and orientations are in respective entities'
		/// coordinate frames.
		/// </summary>
		LocalFrames = 1,
		/// <summary>
		/// When set, specifies that attachment points and orientations are in world coordinate frame.
		/// </summary>
		WorldFrames = 2,
		/// <summary>
		/// When set, specifies that attachment points and orientations are in respective entities parts'
		/// coordinate frames.
		/// </summary>
		LocalFramesPart = 4,
		/// <summary>
		/// When set, specifies that this constraint doesn't do anything except disabling collisions if
		/// <see cref="ConstraintIgnoreBuddy"/> is set.
		/// </summary>
		ConstraintInactive = 0x100,
		/// <summary>
		/// When set, disables collisions between entities bound by the constraint.
		/// </summary>
		ConstraintIgnoreBuddy = 0x200,
		/// <summary>
		/// When set, specifies that relative position of bound entities is constrained to the line.
		/// </summary>
		ConstraintLine = 0x400,
		/// <summary>
		/// When set, specifies that relative position of bound entities is constrained to the plane.
		/// </summary>
		ConstraintPlane = 0x800,
		/// <summary>
		/// When set, specifies that this constraint does not affect relative positions of bound entities.
		/// </summary>
		ConstraintFreePosition = 0x1000,
		/// <summary>
		/// When set, specifies that this constraint doesn't allow any relative rotation of bound entities.
		/// </summary>
		ConstraintNoRotation = 0x2000,
		/// <summary>
		/// When set, specifies that this constraint won't do any positional enforcement during fast
		/// movement. This flag is currently ignored by CryEngine physics subsystem.
		/// </summary>
		ConstraintNoEnforcement = 0x4000,
		/// <summary>
		/// When set, specifies that this constraint won't be deleted when reaching the force limit.
		/// </summary>
		ConstraintNoTears = 0x8000
	}
}