using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of reason why the physics mesh can change.
	/// </summary>
	public enum PhysicsMeshUpdateReason
	{
		/// <summary>
		/// Specifies that the mesh was changed due to explosion.
		/// </summary>
		Explosion,
		/// <summary>
		/// Specifies that the mesh was changed due to fracture.
		/// </summary>
		Fracture,
		/// Specifies that the mesh was changed per user request.
		Request,
		/// <summary>
		/// Specifies that the mesh was changed by deformation.
		/// </summary>
		Deform
	}
}