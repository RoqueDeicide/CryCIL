using System;

namespace CryEngine.Flowgraph
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class FlowNodeAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the name of the node, if not set will use the node class name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the category in which the node will appear when right-clicking in the
		/// Flowgraph Editor.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// Gets or sets the Sandbox filtering category
		/// </summary>
		public FlowNodeFilter Filter { get; set; }

		/// <summary>
		/// Gets or sets how the node is cloned.
		/// </summary>
		public FlowNodeType Type { get; set; }

		/// <summary>
		/// Gets or sets the node description
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets whether this node targets an entity. See <see
		/// cref="CryEngine.Flowgraph.FlowNode.TargetEntity" />
		/// </summary>
		public bool TargetsEntity { get; set; }
	}
}