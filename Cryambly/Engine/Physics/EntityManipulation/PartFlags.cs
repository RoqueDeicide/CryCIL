namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information that can be used to: select parts of the physical entity using its flags,
	/// modify the flags that are assigned to the part of the physical entity.
	/// </summary>
	public struct PartFlags
	{
		#region Fields
		internal bool LocalSpace;
		internal uint PartCurrentFlags;
		internal bool HasFlagCond;
		internal uint FlagsCond;
		internal bool HasModFlags;
		internal uint FlagsOr;
		internal uint FlagsAnd;
		internal bool HasColliderModFlags;
		internal uint FlagsColliderOr;
		internal uint FlagsColliderAnd;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the flags that are assigned to the part.
		/// </summary>
		public uint Flags
		{
			get { return this.PartCurrentFlags; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that can be used to specify the parts of the entity that have specific flags
		/// set.
		/// </summary>
		/// <param name="flagCondition">A set of flags that must be set on the part.</param>
		/// <returns>An object of type <see cref="PartFlags"/>.</returns>
		public static PartFlags Condition(uint flagCondition)
		{
			return new PartFlags
			{
				HasFlagCond = true,
				FlagsCond = flagCondition
			};
		}
		/// <summary>
		/// Creates an object that can be used to modify the flags that are assigned to the part of the
		/// entity.
		/// </summary>
		/// <remarks>
		/// The flags are modified like this: <c>partFlags = (partFlags &amp; flagsAnd) | flagsOr);</c>
		/// </remarks>
		/// <param name="flagsOr"> A value that can be used to add flags to the part of the entity.</param>
		/// <param name="flagsAnd">
		/// A value that can be used to remove flags from the part of the entity.
		/// </param>
		/// <returns>An object of type <see cref="PartFlags"/>.</returns>
		public static PartFlags Modifiers(uint flagsOr, uint flagsAnd = uint.MaxValue)
		{
			return new PartFlags
			{
				HasModFlags = true,
				FlagsOr = flagsOr,
				FlagsAnd = flagsAnd
			};
		}
		/// <summary>
		/// Creates an object that can be used to modify the collision-related flags that are assigned to
		/// the part of the entity.
		/// </summary>
		/// <remarks>
		/// The flags are modified like this: <c>partFlags = (partFlags &amp; flagsAnd) | flagsOr);</c>
		/// </remarks>
		/// <param name="flagsColliderOr"> 
		/// A value that can be used to add flags to the part of the entity.
		/// </param>
		/// <param name="flagsColliderAnd">
		/// A value that can be used to remove flags from the part of the entity.
		/// </param>
		/// <returns>An object of type <see cref="PartFlags"/>.</returns>
		public static PartFlags ModifiersExtended(uint flagsColliderOr, uint flagsColliderAnd = uint.MaxValue)
		{
			return new PartFlags
			{
				HasColliderModFlags = true,
				FlagsColliderOr = flagsColliderOr,
				FlagsColliderAnd = flagsColliderAnd
			};
		}
		/// <summary>
		/// Creates an object that can be used to modify the flags that are assigned to the part of the
		/// entity that has specific flags set.
		/// </summary>
		/// <remarks>
		/// The flags are modified like this: <c>partFlags = (partFlags &amp; flagsAnd) | flagsOr);</c>
		/// </remarks>
		/// <param name="flagCondition">A set of flags that must be set on the part.</param>
		/// <param name="flagsOr">      
		/// A value that can be used to add flags to the part of the entity.
		/// </param>
		/// <param name="flagsAnd">     
		/// A value that can be used to remove flags to the part of the entity.
		/// </param>
		/// <returns>An object of type <see cref="PartFlags"/>.</returns>
		public static PartFlags ConditionWithModifiers(uint flagCondition, uint flagsOr, uint flagsAnd = uint.MaxValue)
		{
			return new PartFlags
			{
				HasFlagCond = true,
				FlagsCond = flagCondition,
				HasModFlags = true,
				FlagsOr = flagsOr,
				FlagsAnd = flagsAnd
			};
		}
		/// <summary>
		/// Creates an object that can be used to modify the flags that are assigned to the part of the
		/// entity that has specific flags set.
		/// </summary>
		/// <remarks>
		/// The flags are modified like this: <c>partFlags = (partFlags &amp; flagsAnd) | flagsOr);</c>
		/// </remarks>
		/// <param name="flagCondition">   A set of flags that must be set on the part.</param>
		/// <param name="flagsOr">         
		/// A value that can be used to add flags to the part of the entity.
		/// </param>
		/// <param name="flagsAnd">        
		/// A value that can be used to remove flags from the part of the entity.
		/// </param>
		/// <param name="flagsColliderOr"> 
		/// A value that can be used to add collision-related flags to the part of the entity.
		/// </param>
		/// <param name="flagsColliderAnd">
		/// A value that can be used to remove collision-related flags from the part of the entity.
		/// </param>
		/// <returns>An object of type <see cref="PartFlags"/>.</returns>
		public static PartFlags ConditionWithModifiersExtended(uint flagCondition, uint flagsOr,
															   uint flagsAnd = uint.MaxValue, uint flagsColliderOr = 0,
															   uint flagsColliderAnd = uint.MaxValue)
		{
			return new PartFlags
			{
				HasFlagCond = true,
				FlagsCond = flagCondition,
				HasModFlags = true,
				FlagsOr = flagsOr,
				FlagsAnd = flagsAnd,
				HasColliderModFlags = true,
				FlagsColliderOr = flagsColliderOr,
				FlagsColliderAnd = flagsColliderAnd
			};
		}
		#endregion
	}
}