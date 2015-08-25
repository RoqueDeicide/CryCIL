using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that awakes/puts to sleep the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.</para>
	/// <para>Never use objects of this type that were created using a default constructor (they are not configured properly!).</para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionAwake
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int bAwake;
		[UsedImplicitly] private float minAwakeTime;
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that changes the awake state of the entity.
		/// </summary>
		/// <param name="minAwakeTime">Minimal amount of time the entity has to be awake for after execution of the action.</param>
		/// <param name="awake">Indicates whether entity should be awakened from sleep. If <c>false</c>, then the entity will be put to sleep instead.</param>
		public PhysicsActionAwake(float minAwakeTime, bool awake = true)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Awake);
			this.minAwakeTime = minAwakeTime;
			this.bAwake = awake ? 1 : 0;
		}
		#endregion
	}
}