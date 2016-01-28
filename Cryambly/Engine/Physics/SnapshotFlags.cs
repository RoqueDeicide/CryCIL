using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of flags that specify how to synchronize physical entities over network.
	/// </summary>
	[Flags]
	public enum SnapshotFlags
	{
		/// <summary>
		/// When set, specifies that difference between time on client and server must be compensated.
		/// </summary>
		/// <remarks>Set internally, when serious lag is detected.</remarks>
		CompensateTimeDifference = 1,
		/// <summary>
		/// Unknown.
		/// </summary>
		CheckSumOnly = 2,
		/// <summary>
		/// Unknown.
		/// </summary>
		NoUpdate = 4
	}
}