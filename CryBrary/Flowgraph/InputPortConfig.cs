namespace CryEngine.Flowgraph
{
	public struct InputPortConfig
	{
		public InputPortConfig(string name, NodePortType type, string desc = "", string humanName = "", string UIConfig = "")
			: this()
		{
			this.name = name;
			this.humanName = humanName;

			this.description = desc;
			this.type = type;
			this.uiConfig = UIConfig;

			this.defaultValue = null;
		}

		public InputPortConfig(string name, NodePortType type, object defaultVal = null, string desc = "", string humanName = "", string UIConfig = "")
			: this(name, type, desc, humanName, UIConfig)
		{
			this.defaultValue = defaultVal;
		}

		public string name;

		public string humanName;

		public string description;

		public NodePortType type;

		public string uiConfig;

		public object defaultValue;
	}
}