using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
	public struct OutputPortConfig
	{
		public OutputPortConfig(string _name, string _humanName, string desc, NodePortType _type, InputPortConfig[] inputPorts, OutputPortConfig[] outputPorts)
			: this()
		{
			name = _name;
			humanName = _humanName;
			description = desc;
			type = _type;

			inputs = inputPorts.Cast<object>().ToArray();
			outputs = outputPorts.Cast<object>().ToArray();
		}

		public string name;

		public string humanName;

		public string description;

		public NodePortType type;

		public object[] inputs;
		public object[] outputs;
	}
}