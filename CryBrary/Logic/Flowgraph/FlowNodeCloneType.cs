namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Enumeration of flow node types.
	/// </summary>
	public enum FlowNodeCloneType
	{
		/// <summary>
		/// Node has only one instance, never cloned.
		/// </summary>
		Singleton,
		/// <summary>
		/// New instance of node will be created each time it is requested.
		/// </summary>
		Instanced
	}
}