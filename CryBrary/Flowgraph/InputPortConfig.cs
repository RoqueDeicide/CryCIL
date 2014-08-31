namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Encapsulates configuration of the input port.
	/// </summary>
	public struct InputPortConfig
	{
		/// <summary>
		/// Name of the input port.
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
		/// <summary>
		/// Text that contains a mapping of integer values to names, allowing to use drop-down list to choose the value in Sandbox.
		/// </summary>
		public string UiConfig;
		/// <summary>
		/// Default value.
		/// </summary>
		public object DefaultValue;
	}
}