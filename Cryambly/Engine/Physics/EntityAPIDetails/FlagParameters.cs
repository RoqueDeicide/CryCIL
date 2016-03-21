using System;
using System.Linq;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information that can be used to: select parts of the physical entity using its flags,
	/// modify the flags that are assigned to the physical entity or parts of it.
	/// </summary>
	/// <example>
	/// <code source="FlagParameters.cs"></code>
	/// </example>
	public struct FlagParameters
	{
		#region Fields
		internal bool HasFlagBase;
		internal Bytes4 FlagBase;
		internal bool HasFlagCond;
		internal Bytes4 FlagsCond;
		internal bool HasModFlags;
		internal Bytes4 FlagsOr;
		internal Bytes4 FlagsAnd;
		internal bool HasColliderModFlags;
		internal ColliderTypes FlagsColliderOr;
		internal ColliderTypes FlagsColliderAnd;
		#endregion
		#region Properties
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that can be used to flat-out assign a specific set of flags.
		/// </summary>
		/// <param name="flags">A set of flags to set.</param>
		/// <returns>
		/// An object of type <see cref="FlagParameters"/> that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Assign(uint flags)
		{
			return new FlagParameters
			{
				HasFlagBase = true,
				FlagBase = new Bytes4(flags)
			};
		}
		/// <summary>
		/// Creates an object that can be used to flat-out assign a specific set of flags.
		/// </summary>
		/// <param name="flags">A set of flags to set.</param>
		/// <returns>
		/// An object of type <see cref="FlagParameters"/> that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Assign(int flags)
		{
			return new FlagParameters
			{
				HasFlagBase = true,
				FlagBase = new Bytes4(flags)
			};
		}
		/// <summary>
		/// Creates an object that can be used to specify the parts of the entity that have specific flags
		/// set.
		/// </summary>
		/// <param name="flagCondition">A set of flags that must be set on the part.</param>
		/// <returns>
		/// An object of type <see cref="FlagParameters"/> that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Condition(uint flagCondition)
		{
			return new FlagParameters
			{
				HasFlagCond = true,
				FlagsCond = new Bytes4(flagCondition)
			};
		}
		/// <summary>
		/// Creates an object that can be used to specify the parts of the entity that have specific flags
		/// set.
		/// </summary>
		/// <param name="flagCondition">A set of flags that must be set on the part.</param>
		/// <returns>
		/// An object of type <see cref="FlagParameters"/> that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Condition(int flagCondition)
		{
			return new FlagParameters
			{
				HasFlagCond = true,
				FlagsCond = new Bytes4(flagCondition)
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to remove specific flags.
		/// </summary>
		/// <param name="flagsToRemove">A set of flags to remove from the part/entity.</param>
		/// <returns>
		/// A valid object of this type that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Remove(uint flagsToRemove)
		{
			return new FlagParameters
			{
				HasModFlags = true,
				FlagsAnd = new Bytes4(~flagsToRemove)
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to remove specific flags.
		/// </summary>
		/// <param name="flagsToRemove">A set of flags to remove from the part/entity.</param>
		/// <returns>
		/// A valid object of this type that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Remove(ColliderTypes flagsToRemove)
		{
			return new FlagParameters
			{
				HasColliderModFlags = true,
				FlagsColliderAnd = ~flagsToRemove
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to remove specific flags.
		/// </summary>
		/// <param name="flagsToRemove">A set of flags to remove from the part/entity.</param>
		///<returns>A valid object of this type that can be combined with other objects using <see cref="operator|"/>.</returns>
		public static FlagParameters Remove(int flagsToRemove)
		{
			return new FlagParameters
			{
				HasModFlags = true,
				FlagsAnd = new Bytes4(~flagsToRemove)
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to set specific flags.
		/// </summary>
		/// <param name="flagsToSet">A set of flags to set for the part/entity.</param>
		/// <returns>
		/// A valid object of this type that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Set(uint flagsToSet)
		{
			return new FlagParameters
			{
				HasModFlags = true,
				FlagsOr = new Bytes4(flagsToSet)
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to set specific flags.
		/// </summary>
		/// <param name="flagsToSet">A set of flags to set for the part/entity.</param>
		/// <returns>
		/// A valid object of this type that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Set(ColliderTypes flagsToSet)
		{
			return new FlagParameters
			{
				HasColliderModFlags = true,
				FlagsColliderAnd = flagsToSet
			};
		}
		/// <summary>
		/// Creates new object of this type that can be used to set specific flags.
		/// </summary>
		/// <param name="flagsToSet">A set of flags to set for the part/entity.</param>
		/// <returns>
		/// A valid object of this type that can be combined with other objects using
		/// <see cref="operator|"/>.
		/// </returns>
		public static FlagParameters Set(int flagsToSet)
		{
			return new FlagParameters
			{
				HasModFlags = true,
				FlagsOr = new Bytes4(flagsToSet)
			};
		}
		/// <summary>
		/// Combines 2 instances together.
		/// </summary>
		/// <remarks>
		/// <para>Rules of combining the instances:</para>
		/// <para>
		/// When both operands were either created using
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Assign"/> or were combined with objects that
		/// were created using that function, the value from right operand will always be used and left
		/// value will be discarded.
		/// </para>
		/// <para>
		/// When both operands were either created using
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Remove"/> or were combined with objects that
		/// were created using that function, all flags from both operands will be removed.
		/// </para>
		/// <para>
		/// When both operands were either created using
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Set"/> or were combined with objects that were
		/// created using that function, all flags from both operands will be set.
		/// </para>
		/// <para>
		/// When both operands were either created using
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Condition"/> or were combined with objects
		/// that were created using that function, the flags from operands will be combined using bitwise
		/// AND.
		/// </para>
		/// <para/>
		/// <para>Order of application of flags in the combined object:</para>
		/// <para>
		/// Value that was put into object using <see cref="O:CryCil.Engine.Physics.FlagParameters.Assign"/>
		/// is assigned to the flags of the object.
		/// </para>
		/// <para>
		/// All flags that were added to the object with
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Remove"/> are removed.
		/// </para>
		/// <para>
		/// All flags that were added to the object with
		/// <see cref="O:CryCil.Engine.Physics.FlagParameters.Set"/> are set.
		/// </para>
		/// </remarks>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>A new instance of this type that represents 2 operands combined.</returns>
		public static FlagParameters operator |(FlagParameters left, FlagParameters right)
		{
			return new FlagParameters
			{
				HasFlagBase = left.HasFlagBase || right.HasFlagBase,
				FlagBase = right.FlagBase,
				HasModFlags = left.HasModFlags || right.HasModFlags,
				FlagsOr = new Bytes4(left.FlagsOr.UnsignedInt | right.FlagsOr.UnsignedInt),
				FlagsAnd = new Bytes4(left.FlagsAnd.UnsignedInt & right.FlagsAnd.UnsignedInt),
				HasColliderModFlags = left.HasColliderModFlags || right.HasColliderModFlags,
				FlagsColliderOr = left.FlagsColliderOr | right.FlagsColliderOr,
				FlagsColliderAnd = left.FlagsColliderAnd & right.FlagsColliderAnd,
			};
		}
		#endregion
	}
}