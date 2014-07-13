using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ConsoleCommandAttribute : Attribute
	{
		public string Name;
		public string Comment;
		public CVarFlags Flags;
	}
}