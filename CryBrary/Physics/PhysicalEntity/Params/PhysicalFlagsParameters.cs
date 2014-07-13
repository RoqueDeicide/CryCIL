using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;

namespace CryEngine
{
	[CLSCompliant(false)]
	public struct PhysicalFlagsParameters
	{
		public static PhysicalFlagsParameters Create()
		{
			var parameters = new PhysicalFlagsParameters();

			parameters.type = 15;

			parameters.flags = (PhysicalizationFlags)UnusedMarker.UnsignedInteger;
			parameters.flagsOR = (PhysicalizationFlags)UnusedMarker.UnsignedInteger;
			parameters.flagsAND = (PhysicalizationFlags)UnusedMarker.UnsignedInteger;

			return parameters;
		}

		internal int type;

		public PhysicalizationFlags flags;
		/// <summary>
		/// when settin
		/// </summary>
		public PhysicalizationFlags flagsOR;
		/// <summary>
		/// when getting
		/// </summary>
		public PhysicalizationFlags flagsAND;
	}
}