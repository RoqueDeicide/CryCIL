using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Applied to methods, properties and fields to mark them as Flowgraph node ports.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PortAttribute : Attribute
	{
		/// <summary>
		/// Name of the port.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Description of the port.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Only to be used for string ports
		/// </summary>
		public StringPortType StringPortType { get; set; }
	}
}