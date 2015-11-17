using System;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Enumeration of flags that specify which aspects of sequence of facial animations must be
	/// serialized.
	/// </summary>
	[Flags]
	public enum FacialAnimationSequenceSerializationFlags : uint
	{
		/// <summary>
		/// When set, specifies that sound data must be (de-)serialized.
		/// </summary>
		SoundEntries = 0x00000001,
		/// <summary>
		/// When set, specifies that camera data must be (de-)serialized.
		/// </summary>
		CameraPath = 0x00000002,
		/// <summary>
		/// When set, specifies that animation data must be (de-)serialized.
		/// </summary>
		Animation = 0x00000004,
		/// <summary>
		/// When set, specifies that all data must be (de-)serialized.
		/// </summary>
		All = 0xFFFFFFFF
	}
}