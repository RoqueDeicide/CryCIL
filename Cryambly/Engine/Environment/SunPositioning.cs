using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Environment
{
	/// <summary>
	/// Encapsulates parameters that describe the interaction between the sun and the TOD system.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SunPositioning
	{
		/// <summary>
		/// Indicates whether sun's position will change as the time of day advances.
		/// </summary>
		public bool LinkedToTimeOfDay;
		/// <summary>
		/// Sun's latitude.
		/// </summary>
		public float Latitude;
		/// <summary>
		/// Sun's longitude.
		/// </summary>
		public float Longitude;
	}
}
