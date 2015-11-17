using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of the ways how the living entity can jump.
	/// </summary>
	public enum JumpMode
	{
		/// <summary>
		/// Specifies that the entity is not jumping.
		/// </summary>
		NoJump,
		/// <summary>
		/// Specifies that the entity is jumping by having its velocity completely replaced with new one.
		/// </summary>
		AssignVelocity,
		/// <summary>
		/// Specifies that the entity is jumping by having its velocity combined with new one.
		/// </summary>
		AddVelocity
	}
	/// <summary>
	/// Encapsulates description of the action that moves the physical entity that is a living entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionMove
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private Vector3 dir;
		[UsedImplicitly] private JumpMode iJump;
		[UsedImplicitly] private float dt;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new valid object of this type.
		/// </summary>
		/// <param name="velocity">Vector that represents velocity of movement.</param>
		/// <param name="jump">    
		/// A value that indicates whether the entity is supposed to jump (switch to un-grounded movement
		/// mode) and if so, how.
		/// </param>
		/// <param name="time">    
		/// An optional value that represents the time interval for this action.
		/// </param>
		public PhysicsActionMove(Vector3 velocity, JumpMode jump = JumpMode.NoJump, float time = 0)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Move);
			this.dir = velocity;
			this.iJump = jump;
			this.dt = time;
		}
		#endregion
	}
}