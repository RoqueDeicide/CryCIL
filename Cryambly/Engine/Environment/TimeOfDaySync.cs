using System;
using System.Linq;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Enumeration of flags that specify how to synchronize the Time-Of-Day system with other games within
	/// multiplayer.
	/// </summary>
	[Flags]
	public enum TimeOfDaySync : uint
	{
		/// <summary>
		/// When set, specifies that all variables should be assigned when synchronizing(?).
		/// </summary>
		/// <remarks>
		/// In default GameSDK implementation this flag is used when synchronization happens on a receiving
		/// end and it is used only once.
		/// </remarks>
		ForceSet = 1,
		/// <summary>
		/// When set, specifies that the lag value should affect the current time of the day.
		/// </summary>
		/// <remarks>
		/// In default GameSDK implementation this flag is used when synchronization happens on a receiving
		/// end.
		/// </remarks>
		CompensateLag = 2,
		/// <summary>
		/// When set, specifies that only static properties (those that don't change over time) should be
		/// synchronized.
		/// </summary>
		/// <remarks>
		/// Used when the Time-Of-Day settings are completely static, and time doesn't advance.
		/// </remarks>
		StaticProperties = 4
	}
}