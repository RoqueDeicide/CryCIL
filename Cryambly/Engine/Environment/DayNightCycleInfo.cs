using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Encapsulates information about day-night cycle.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DayNightCycleInfo
	{
		/// <summary>
		/// Time of the day when the cycle starts.
		/// </summary>
		public float StartTime;
		/// <summary>
		/// Time of the day when the cycle ends.
		/// </summary>
		public float EndTime;
		/// <summary>
		/// Speed at which the time of the day advances.
		/// </summary>
		public float Speed;
	}
}
