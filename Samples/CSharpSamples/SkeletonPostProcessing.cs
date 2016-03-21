using System;
using System.Linq;
using CryCil.Engine.Models.Characters;

namespace CSharpSamples
{
	public static class SkeletonPostProcessing
	{
		/// <summary>
		/// This field Has to be static.
		/// </summary>
		public static CharacterBonesUpdateEventHandler Callback;
		public static void Test(Character character)
		{
			if (!character.IsValid)
			{
				return;
			}

			var pose = character.SkeletonPose;

			if (!pose.IsValid)
			{
				return;
			}

			// This call will transfer an address to the field to the native code. Since native code is not
			// managed, any references like this one Must point at memory that is pinned to prevent GC from
			// touching it. All static fields are perma-pinned and therefore - safe.
			character.SkeletonPose.SetBoneUpdateHandler(ref Callback);
		}
	}
}