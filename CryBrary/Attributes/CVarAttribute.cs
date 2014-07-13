using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class CVarAttribute : Attribute
	{
		public object DefaultValue;
		public CVarFlags Flags;
		public string Help;
		public string Name;
	}
}