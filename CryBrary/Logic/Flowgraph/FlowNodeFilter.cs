namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Enumeration of categories flow nodes can be assigned to to allow them to be
	/// filetered in Sandbox editor.
	/// </summary>
	public enum FlowNodeFilter
	{
		/// <summary>
		/// This node is approved for designers
		/// </summary>
		Approved = 0x0010,
		/// <summary>
		/// This node is slightly advanced and approved.
		/// </summary>
		Advanced = 0x0020,
		/// <summary>
		/// This node is for debug purpose only.
		/// </summary>
		Debug = 0x0040,
		/// <summary>
		/// This node is obsolete and is not available in the editor.
		/// </summary>
		Obsolete = 0x0200,
	}
}