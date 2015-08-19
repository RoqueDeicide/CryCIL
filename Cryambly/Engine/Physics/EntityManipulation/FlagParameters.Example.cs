﻿using System;

namespace CryCil.Engine.Physics
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
			FlagsEnum1 flagsToAssign = FlagsEnum1.First | FlagsEnum1.Fourth;

			// These flags will be removed from the object.
			FlagsEnum1 flagsToRemove = FlagsEnum1.Second | FlagsEnum1.Third;

			// These flags will be set.
			FlagsEnum1 flagsToSet = FlagsEnum1.First | FlagsEnum1.Second;

			// This object can be used to just assign the flags.
			FlagParameters params1 = FlagParameters.Assign((uint)flagsToAssign);

			// The following object is used to remove second and third flag and set first and second. Due
			// to setting flags happening after removal the resultant flags won't have third flag set, but
			// first and second will be set.
			FlagParameters params2 = FlagParameters.Remove((uint)flagsToRemove) | FlagParameters.Set((uint)flagsToSet);

			// The following object will assign first and fourth flag and sets the second flag.
			FlagParameters params3 = params1 | params2;

			// This is interesting case:
			// 
			// Left operand contains the assignment and right does as well. Due to rules of combination,
			// the assignment information in left operand will be discarded. This means that resultant
			// object will assign the third flag which will be removed immediately and then first and
			// second will be set.
			FlagParameters params4 = params3 | FlagParameters.Assign((uint)FlagsEnum1.Third);

			// Now for parts:

			// This object can be used to designate the parts of physical entity that have all but second
			// flag set.
			FlagParameters condition = FlagParameters.Condition((uint)FlagsEnum1.First) |
									   FlagParameters.Condition((uint)(FlagsEnum1.Third | FlagsEnum1.Fourth));

			// This object removes fourth flag and sets the second one.
			FlagParameters partFlagModification = FlagParameters.Remove((uint)FlagsEnum1.Fourth) |
												  FlagParameters.Set((uint)FlagsEnum1.Second);

			// This object removes fourth flag and sets the second one on every part of the entity that has
			// all but second flag set.
			FlagParameters conditionWithModification = condition | partFlagModification;

			// This object removes third collider flag and sets the second one.
			FlagParameters colliderModification = FlagParameters.Remove((uint)ColliderFlagsEnum1.Third) |
												  FlagParameters.Set((uint)ColliderFlagsEnum1.Second);

			// This object removes fourth flag and sets the second one as well as removes third collider
			// flag and sets the second one on every part of the entity that has all but second flag set.
			FlagParameters massPartFlagModification = colliderModification | conditionWithModification;
		}
	}
}