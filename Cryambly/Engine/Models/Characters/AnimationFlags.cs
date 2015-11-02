using System;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Enumeration of flags that specify the animation.
	/// </summary>
	[Flags]
	public enum AnimationFlags : uint
	{
		/// <summary>
		/// When set, specifies that the animation must not be updated automatically.
		/// </summary>
		/// <remarks>
		/// <para>
		/// User needs to set the time manually for any animations that being played with this flag set.
		/// This is used for steering-wheels and mounted-weapons, where we convert the rotation of an
		/// object into animation key-times.
		/// </para>
		/// <para>
		/// This flag overrides <see cref="LoopAnimation"/> and <see cref="RepeatLastKey"/> if it's set
		/// along with any of them.
		/// </para>
		/// </remarks>
		ManualUpdate = 0x000001,
		/// <summary>
		/// When set, specifies that the animation must play on endless loop until stopped.
		/// </summary>
		/// <remarks>This flag overrides <see cref="RepeatLastKey"/> if it's set along with it.</remarks>
		LoopAnimation = 0x000002,
		/// <summary>
		/// When set, specifies that the animation must play once and remain in last key-frame.
		/// </summary>
		/// <remarks>
		/// The animation that is player with this flag gets removed from animation queue once done.
		/// </remarks>
		RepeatLastKey = 0x000004,
		/// <summary>
		/// When set, specifies that transitions between animations must be handled with linear
		/// time-warping.
		/// </summary>
		TransitionTimeWarping = 0x000008,
		/// <summary>
		/// When set, specifies that the transition between animation must start when previous animation
		/// passed the key-time that is specified by <see cref="CharacterAnimationParameters.KeyTime"/>
		/// </summary>
		StartAtKeyTime = 0x000010,
		/// <summary>
		/// Unknown, but apparently this flag can be simulated with <see cref="StartAtKeyTime"/> and
		/// <see cref="RepeatLastKey"/>.
		/// </summary>
		StartAfter = 0x000020,
		/// <summary>
		/// When set, specifies that the best transition point must be found in order to start locomotion
		/// animation when playing the transition from idle animation to moving one.
		/// </summary>
		Idle2Move = 0x000040,
		/// <summary>
		/// When set, specifies that the best transition point must be found in order to start stopping
		/// animation when playing the transition from moving animation to idle one.
		/// </summary>
		Move2Idle = 0x000080,
		/// <summary>
		/// When set, specifies that the animation can be restarted using previous transition rules.
		/// </summary>
		/// <remarks>Used for weapon recoil animation.</remarks>
		AllowAnimationRestart = 0x000100,
		/// <summary>
		/// When set, specifies that no interpolation must be done between key-frames and 30Hz sampling
		/// must be used instead.
		/// </summary>
		KeyframeSample30Hz = 0x000200,
		/// <summary>
		/// When set in layer 0, specifies that multilayer animations must not be allowed.
		/// </summary>
		DisableMultilayer = 0x000400,
		/// <summary>
		/// When set, specifies that the physical representation of the animated character (skeleton) must
		/// be updated along with animation.
		/// </summary>
		/// <remarks>
		/// Used in cases when physics of the character must be updated no matter whether the object is
		/// visible or not, e.g. when opening the door or when moving physical objects using the animation
		/// (e.g. when lifting a VTOL on a moving platform in multi-player in Crysis).
		/// </remarks>
		ForceSkeletonUpdate = 0x000800,
		/// <summary>
		/// When set, specifies that the animation can only be played in Track-View.
		/// </summary>
		TrackViewExclusive = 0x001000,
		/// <summary>
		/// When set, specifies that when this animation is enqueued, another one must be dequeued using
		/// standard FIFO rules.
		/// </summary>
		/// <remarks>Currently only used for boids.</remarks>
		RemoveFromFifo = 0x002000,
		/// <summary>
		/// Unknown.
		/// </summary>
		FullRootPriority = 0x004000,
		/// <summary>
		/// When set, specifies that the animation must be transitioned to, bypassing any conditions that
		/// can delay the transition (animations in the same queue with flags
		/// <see cref="AnimationFlags.Idle2Move"/>, <see cref="AnimationFlags.Move2Idle"/>,
		/// <see cref="AnimationFlags.StartAfter"/>, <see cref="AnimationFlags.StartAtKeyTime"/>, not in
		/// memory)
		/// </summary>
		ForceTransitionToAnim = 0x008000,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <remarks>
		/// Fadeout works only for animations in higher layers and should be used together with
		/// <see cref="RepeatLastKey"/>.
		/// </remarks>
		FadeOut = 0x40000000
	}
}