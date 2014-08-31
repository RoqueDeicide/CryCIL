using System.Collections.Generic;
using System.Linq;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Encapsulates configuration of the output port.
	/// </summary>
	public struct OutputPortConfig
	{
		/// <summary>
		/// Name of the output port.
		/// </summary>
		public string Name;
		/// <summary>
		/// Readable name of the port.
		/// </summary>
		public string HumanName;
		/// <summary>
		/// Description of the port.
		/// </summary>
		public string Description;
		/// <summary>
		/// Type of the port.
		/// </summary>
		public NodePortType Type;
	}
}