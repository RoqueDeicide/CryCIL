using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;

namespace CryEngine.Physics
{
	public class PhysicalEntityLiving : PhysicalEntity
	{
		internal PhysicalEntityLiving(IntPtr physEntPtr)
		{
			Handle = physEntPtr;
		}

		public override PhysicalizationType Type
		{
			get { return PhysicalizationType.Living; }
		}
	}
}