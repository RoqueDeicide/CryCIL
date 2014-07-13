using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	public struct Diagonal33
	{
		public float x, y, z;
		public bool IsValid
		{
			get
			{
				return MathHelpers.IsNumberValid(this.x) && MathHelpers.IsNumberValid(this.y) && MathHelpers.IsNumberValid(this.z);
			}
		}
	}
}