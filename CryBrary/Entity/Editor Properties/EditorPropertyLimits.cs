using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public struct EditorPropertyLimits
	{
		public EditorPropertyLimits(float min, float max)
			: this()
		{
			this.min = min;
			this.max = max;
		}

		public float min;
		public float max;
	}
}