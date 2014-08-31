using System.Collections.Generic;
using System.Linq;

namespace CryEngine.Flowgraph
{
	public struct OutputPortConfig
	{
		public OutputPortConfig(string _name, string _humanName,
			string desc, NodePortType _type, IEnumerable<InputPortConfig> inputPorts,
			IEnumerable<OutputPortConfig> outputPorts)
			: this()
		{
			this.name = _name;
			this.humanName = _humanName;
			this.description = desc;
			this.type = _type;

			this.inputs = inputPorts.Cast<object>().ToArray();
			this.outputs = outputPorts.Cast<object>().ToArray();
		}

		public string name;

		public string humanName;

		public string description;

		public NodePortType type;

		public object[] inputs;
		public object[] outputs;
	}
}