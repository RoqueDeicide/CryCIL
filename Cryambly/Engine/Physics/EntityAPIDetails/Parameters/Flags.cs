using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get/set flags of the physical entity. Objects of
	/// this type are created using the factory methods.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersFlags
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private uint flags;
		[UsedImplicitly] private uint flagsOR;
		[UsedImplicitly] private uint flagsAND;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the flags that are assigned to the entity.
		/// </summary>
		public PhysicalEntityFlags Flags
		{
			get { return (PhysicalEntityFlags)this.flags; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <returns>
		/// A valid object that can be passed to <see cref="PhysicalEntity.GetParameters"/>.
		/// </returns>
		public static PhysicsParametersFlags Create()
		{
			return new PhysicsParametersFlags
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.Flags),
				flags = UnusedValue.UInt32,
				flagsAND = UnusedValue.UInt32,
				flagsOR = UnusedValue.UInt32
			};
		}
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="flagParameters">
		/// An object that specifies how to modify the flags that are assigned to this entity. Use flags
		/// that are defined in <see cref="PhysicalEntityFlags"/> enumeration.
		/// </param>
		/// <returns>
		/// A valid object that can be passed to <see cref="PhysicalEntity.SetParameters"/>.
		/// </returns>
		public static PhysicsParametersFlags Create(FlagParameters flagParameters)
		{
			return new PhysicsParametersFlags
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.Flags),
				flags = UnusedValue.UInt32,
				flagsAND = flagParameters.HasModFlags ? flagParameters.FlagsAnd.UnsignedInt : UnusedValue.UInt32,
				flagsOR = flagParameters.HasModFlags ? flagParameters.FlagsOr.UnsignedInt : UnusedValue.UInt32
			};
		}
		#endregion
	}
}