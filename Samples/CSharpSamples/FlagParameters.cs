using System;
using System.Linq;
using CryCil.Engine.Physics;

// ReSharper disable UnusedVariable
namespace CSharpSamples
{
	/// <summary>
	/// Enumeration of sample flags.
	/// </summary>
	[Flags]
	public enum FlagsEnum1 : uint
	{
		/// <summary>
		/// First sample flag.
		/// </summary>
		First = 1 << 0,
		/// <summary>
		/// First sample flag.
		/// </summary>
		Second = 1 << 0,
		/// <summary>
		/// First sample flag.
		/// </summary>
		Third = 1 << 0,
		/// <summary>
		/// First sample flag.
		/// </summary>
		Fourth = 1 << 0
	}
	/// <summary>
	/// Enumeration of sample flags that can be assign as collider flags to the part.
	/// </summary>
	[Flags]
	public enum ColliderFlagsEnum1 : uint
	{
		/// <summary>
		/// First sample collider flag.
		/// </summary>
		First = 1 << 0,
		/// <summary>
		/// First sample collider flag.
		/// </summary>
		Second = 1 << 0,
		/// <summary>
		/// First sample collider flag.
		/// </summary>
		Third = 1 << 0
	}

	/// <summary>
	/// A sample class.
	/// </summary>
	public static class FlagParametersExample
	{
		/// <summary>
		/// A sample function.
		/// </summary>
		public static void Sample()
		{
			// This object will override all flags that were previously assigned to the part/entity.
			const FlagsEnum1 flagsToAssign = FlagsEnum1.First | FlagsEnum1.Fourth;

			// These flags will be removed from the object.
			const FlagsEnum1 flagsToRemove = FlagsEnum1.Second | FlagsEnum1.Third;

			// These flags will be set.
			const FlagsEnum1 flagsToSet = FlagsEnum1.First | FlagsEnum1.Second;

			// This object can be used to just assign the flags.
			FlagParameters params1 = FlagParameters.Assign((uint)flagsToAssign);

			// The following object is used to remove second and third flag and set first and second. Due to
			// setting flags happening after removal the resultant flags won't have third flag set, but
			// first and second will be set.
			FlagParameters params2 = FlagParameters.Remove((uint)flagsToRemove) | FlagParameters.Set((uint)flagsToSet);

			// The following object will assign first and fourth flag and sets the second flag.
			FlagParameters params3 = params1 | params2;

			// This is interesting case:
			//
			// Left operand contains the assignment and right does as well. Due to rules of combination, the
			// assignment information in left operand will be discarded. This means that resultant object
			// will assign the third flag which will be removed immediately and then first and second will
			// be set.
			FlagParameters params4 = params3 | FlagParameters.Assign((uint)FlagsEnum1.Third);

			// Now for parts:

			// This object can be used to designate the parts of physical entity that float, can have their
			// geometry modified and can be broken via code.
			FlagParameters condition = FlagParameters.Condition((uint)PhysicsGeometryFlags.Floats) |
									   FlagParameters.Condition((uint)(PhysicsGeometryFlags.CanModify |
																	   PhysicsGeometryFlags.ManuallyBreakable));

			// This object makes the part not float anymore and makes collidable with any solid object.
			FlagParameters partFlagModification = FlagParameters.Remove((uint)PhysicsGeometryFlags.Floats) |
												  FlagParameters.Set((uint)PhysicsGeometryFlags.CollisionTypeSolid);

			// This object makes the part that float, can have their geometry modified and can be broken via
			// code, not float anymore and makes collidable with any solid object.
			FlagParameters conditionWithModification = condition | partFlagModification;

			// This object changes the part from obstruction into debris.
			FlagParameters colliderModification = FlagParameters.Remove((uint)ColliderTypes.Obstruct) |
												  FlagParameters.Set((uint)ColliderTypes.Debris);

			// This object makes the part that float, can have their geometry modified and can be broken via
			// code, not float anymore and makes collidable with any solid object as will as turns into
			// debris from obstruction.
			FlagParameters massPartFlagModification = colliderModification | conditionWithModification;
		}
	}
}