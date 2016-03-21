using System;
using System.Linq;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Enumeration of types of objects that are used to control the audio in the game.
	/// </summary>
	public enum AudioControlType : uint
	{
		/// <summary>
		/// Default value.
		/// </summary>
		None = 0,
		/// <summary>
		/// Object that can make sounds(?).
		/// </summary>
		AudioObject = 1,
		/// <summary>
		/// Audio trigger that, when executed, plays a sound.
		/// </summary>
		Trigger = 2,
		/// <summary>
		/// Real-Time Parameter Control object.
		/// </summary>
		Rtpc = 3,
		/// <summary>
		/// An object that plays different sounds according to its current state.
		/// </summary>
		Switch = 4,
		/// <summary>
		/// An object that represents the state of the switch.
		/// </summary>
		SwitchState = 5,
		/// <summary>
		/// An object that contains information about audio to be preloaded.
		/// </summary>
		Preload = 6,
		/// <summary>
		/// An object that represents the environmental effect.
		/// </summary>
		Environment = 7
	}
}