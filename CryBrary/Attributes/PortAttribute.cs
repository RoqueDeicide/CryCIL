using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PortAttribute : Attribute
	{
		public string Name { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// Only to be used for string ports
		/// </summary>
		public StringPortType StringPortType { get; set; }
	}
}