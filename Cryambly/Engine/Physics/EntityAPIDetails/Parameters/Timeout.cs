using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get and set the time it takes for the physical
	/// entity to go into sleep mode if it is not receiving any external impulses.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersTimeout
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float timeIdle;
		[UsedImplicitly] private float maxTimeIdle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the time that passed since last time this entity received an impulse (had a contact) from
		/// outside.
		/// </summary>
		/// <exception cref="OverflowException">
		/// Time-span value is less than <see cref="F:System.TimeSpan.MinValue"/> or greater than
		/// <see cref="F:System.TimeSpan.MaxValue"/>. Or it'is
		/// <see cref="F:System.Double.PositiveInfinity"/>. Or it'is
		/// <see cref="F:System.Double.NegativeInfinity"/>.
		/// </exception>
		public TimeSpan TimeSinceLastImpulse
		{
			get { return TimeSpan.FromSeconds(this.timeIdle); }
		}
		/// <summary>
		/// Gets or sets amount of time that must pass since entity's last interaction with physical world
		/// or last reception of external impulse before the entity enters sleep state.
		/// </summary>
		/// <exception cref="OverflowException">
		/// Time-span value is less than <see cref="F:System.TimeSpan.MinValue"/> or greater than
		/// <see cref="F:System.TimeSpan.MaxValue"/>. Or it'is
		/// <see cref="F:System.Double.PositiveInfinity"/>. Or it'is
		/// <see cref="F:System.Double.NegativeInfinity"/>.
		/// </exception>
		public TimeSpan MaxIdleTime
		{
			get { return TimeSpan.FromSeconds(this.maxTimeIdle); }
			set { this.maxTimeIdle = (float)value.TotalSeconds; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="justPassZero">Pass any number to make sure this constructor gets invoked.</param>
		public PhysicsParametersTimeout([UsedImplicitly] int justPassZero)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Timeout);
			this.timeIdle = UnusedValue.Single;
			this.maxTimeIdle = UnusedValue.Single;
		}
		#endregion
	}
}