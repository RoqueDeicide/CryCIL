using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;
using CryEngine.Utilities;

namespace CryEngine
{
	internal struct PhysicalEntityImpulseAction
	{
		public static PhysicalEntityImpulseAction Create()
		{
			var action = new PhysicalEntityImpulseAction();

			action.type = 2;
			action.iApplyTime = 2;
			action.iSource = 0;
			action.impulse = Vector3.Zero;

			action.point = UnusedMarker.Vector3;
			action.angImpulse = UnusedMarker.Vector3;
			action.partid = UnusedMarker.Integer;
			action.ipart = UnusedMarker.Integer;

			return action;
		}

		public int type;

		public Vector3 impulse;
		public Vector3 angImpulse;    // optional
		public Vector3 point; // point of application, in world CS, optional
		public int partid;    // receiver part identifier
		public int ipart; // alternatively, part index can be used
		public int iApplyTime; // 0-apply immediately, 1-apply before the next time step, 2-apply after the next time step
		public int iSource; // reserved for internal use
	}
}