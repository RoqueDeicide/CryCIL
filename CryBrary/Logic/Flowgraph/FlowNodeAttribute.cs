using System;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Applied to classes to mark them as Flowgraph nodes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class FlowNodeAttribute : Attribute
	{
	}
}