using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get/set foreign data for the physical entity.
	/// Objects of this type are created using factory methods.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersForeignData
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private ForeignData data;
		[UsedImplicitly] private int iForeignFlags;
		[UsedImplicitly] private int iForeignFlagsAND, iForeignFlagsOR;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the foreign data that is assigned to this entity.
		/// </summary>
		public ForeignData Data
		{
			get { return this.data; }
		}
		/// <summary>
		/// Gets any flags the owner would like to store.
		/// </summary>
		public int ForeignDataFlags
		{
			get { return this.iForeignFlags; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object of this type that can be passed to
		/// <see cref="PhysicalEntity.GetParameters"/>.
		/// </summary>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersForeignData Create()
		{
			return new PhysicsParametersForeignData
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.ForeignData),
				data = ForeignData.Unused,
				iForeignFlags = UnusedValue.Int32,
				iForeignFlagsAND = -1,
				iForeignFlagsOR = 0
			};
		}
		/// <summary>
		/// Creates an object of this type that can be passed to
		/// <see cref="PhysicalEntity.SetParameters"/>.
		/// </summary>
		/// <param name="data">A new foreign data to assign to the physical entity.</param>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersForeignData Create(ForeignData data)
		{
			return new PhysicsParametersForeignData
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.ForeignData),
				data = data,
				iForeignFlags = UnusedValue.Int32,
				iForeignFlagsAND = -1,
				iForeignFlagsOR = 0
			};
		}
		/// <summary>
		/// Creates an object of this type that can be passed to
		/// <see cref="PhysicalEntity.SetParameters"/>.
		/// </summary>
		/// <param name="data"> A new foreign data to assign to the physical entity.</param>
		/// <param name="flags">
		/// An object that specifies how to modify the flags that are assigned to the foreign data.
		/// </param>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersForeignData Create(ForeignData data, FlagParameters flags)
		{
			return new PhysicsParametersForeignData
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.ForeignData),
				data = data,
				iForeignFlags = UnusedValue.Int32,
				iForeignFlagsAND = flags.HasModFlags ? flags.FlagsAnd.SignedInt : -1,
				iForeignFlagsOR = flags.HasModFlags ? flags.FlagsOr.SignedInt : 0
			};
		}
		/// <summary>
		/// Creates an object of this type that can be passed to
		/// <see cref="PhysicalEntity.SetParameters"/>.
		/// </summary>
		/// <param name="flags">
		/// An object that specifies how to modify the flags that are assigned to the foreign data.
		/// </param>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersForeignData Create(FlagParameters flags)
		{
			return new PhysicsParametersForeignData
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.ForeignData),
				data = ForeignData.Unused,
				iForeignFlags = UnusedValue.Int32,
				iForeignFlagsAND = flags.HasModFlags ? flags.FlagsAnd.SignedInt : -1,
				iForeignFlagsOR = flags.HasModFlags ? flags.FlagsOr.SignedInt : 0
			};
		}
		#endregion
	}
}