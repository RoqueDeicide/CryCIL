using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to modify the collision class of the physical entity.
	/// Objects of this type are created using factory methods.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersCollisionClass
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private CollisionClass or;
		[UsedImplicitly] private CollisionClass and;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current collision of the entity.
		/// </summary>
		public CollisionClass Class => this.or;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type that can be used to query the collision class of the entity.
		/// </summary>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersCollisionClass Create()
		{
			PhysicsParametersCollisionClass parameters = new PhysicsParametersCollisionClass
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.CollisionClass),
				or = new CollisionClass(),
				and = new CollisionClass
				{
					Type = (ColliderTypes)uint.MaxValue,
					Ignore = (ColliderTypes)uint.MaxValue
				}
			};
			return parameters;
		}
		/// <summary>
		/// Creates a new instance of this type that can be used to modify the collision class of the
		/// entity.
		/// </summary>
		/// <param name="flagsToSet">   
		/// The bit flags that are set in this class will be set for flags that are set in the collision
		/// class of the entity.
		/// </param>
		/// <param name="flagsToRemove">
		/// The bit flags that are set in this class will be removed from flags that are set in the
		/// collision class of the entity.
		/// </param>
		/// <returns>A valid object of this type.</returns>
		public static PhysicsParametersCollisionClass Create(CollisionClass flagsToSet,
															 CollisionClass flagsToRemove = new CollisionClass())
		{
			PhysicsParametersCollisionClass parameters = new PhysicsParametersCollisionClass
			{
				Base = new PhysicsParameters(PhysicsParametersTypes.CollisionClass),
				or = flagsToSet,
				and = new CollisionClass
				{
					Type = ~flagsToRemove.Type,
					Ignore = ~flagsToRemove.Ignore
				}
			};
			return parameters;
		}
		#endregion
	}
}