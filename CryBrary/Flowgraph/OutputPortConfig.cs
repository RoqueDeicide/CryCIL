using System.Collections.Generic;
using System.Linq;

namespace CryEngine.Flowgraph
{
	public struct OutputPortConfig
	{
		public OutputPortConfig(string _name, string _humanName,
			string desc, NodePortType _type)
			: this()
		{
			this.name = _name;
			this.humanName = _humanName;
			this.description = desc;
			this.type = _type;
		}

		public string name;

		public string humanName;

		public string description;

		public NodePortType type;
	}
}