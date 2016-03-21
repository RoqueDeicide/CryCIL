using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of reasons that can cause creation a new physics part.
	/// </summary>
	public enum PhysicsPartCreationReason
	{
		/// <summary>
		/// Specifies that the part was created by splitting the mesh using boolean operation.
		/// </summary>
		MeshSplit,
		/// <summary>
		/// Specifies that the part was created by breaking one of the joints.
		/// </summary>
		JointBroken
	}
}