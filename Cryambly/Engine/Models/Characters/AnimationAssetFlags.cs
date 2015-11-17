using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of flags that specify animation assets.
	/// </summary>
	[Flags]
	public enum AnimationAssetFlags : uint
	{
		/// <summary>
		/// When set, specifies that this animation asset is an additive animation.
		/// </summary>
		Additive = 0x001,
		/// <summary>
		/// When set, specifies that this animation asset can be used with a loop flag.
		/// </summary>
		Cycle = 0x002,
		/// <summary>
		/// When set, specifies that this animation asset has valid information (has been loaded at least
		/// once) .
		/// </summary>
		Loaded = 0x004,

		/// <summary>
		/// When set, specifies that this animation asset is a locomotion group (LMG).
		/// </summary>
		Lmg = 0x008,
		/// <summary>
		/// When set, specifies that this animation asset is a locomotion group (LMG) that has been
		/// processed at loading time.
		/// </summary>
		LmgValid = 0x020,

		/// <summary>
		/// When set, specifies that this animation asset was created at run-time rather then loaded.
		/// </summary>
		Created = 0x800,
		/// <summary>
		/// When set, specifies that this animation asset has already been requested for loading.
		/// </summary>
		Requested = 0x1000,
		/// <summary>
		/// When set, specifies that this animation asset has on-demand loading.
		/// </summary>
		OnDemand = 0x2000,

		/// <summary>
		/// When set, specifies that this animation asset is an Aim-Pose.
		/// </summary>
		AimPose = 0x4000,
		/// <summary>
		/// When set, specifies that this animation asset is an unloaded Aim-Pose.
		/// </summary>
		AimPoseUnloaded = 0x8000,

		/// <summary>
		/// Unknown.
		/// </summary>
		NotFound = 0x10000,
		/// <summary>
		/// Unknown.
		/// </summary>
		Tcb = 0x20000,
		/// <summary>
		/// Unknown.
		/// </summary>
		InternalType = 0x40000,
		/// <summary>
		/// Unknown.
		/// </summary>
		BigEndian = 0x80000000
	}
}