using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Flowgraph;

namespace CryEngine.Initialization
{
	public struct InputPortConfig
	{
		public InputPortConfig(string _name, NodePortType _type, string desc = "", string _humanName = "", string UIConfig = "")
			: this()
		{
			name = _name;
			humanName = _humanName;

			description = desc;
			uiConfig = UIConfig;

			defaultValue = null;
		}

		public InputPortConfig(string _name, NodePortType _type, object defaultVal = null, string desc = "", string _humanName = "", string UIConfig = "")
			: this(_name, _type, desc, _humanName, UIConfig)
		{
			defaultValue = defaultVal;
		}

		public string name;

		public string humanName;

		public string description;

		public NodePortType type;

		public string uiConfig;

		public object defaultValue;
	}
}