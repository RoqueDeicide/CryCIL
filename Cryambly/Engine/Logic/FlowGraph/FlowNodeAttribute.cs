using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Marks types that represent flow graph nodes. Marked class has to inherit from
	/// <see cref="FlowNode"/> class, define the constructor that has the same signature as
	/// <see cref="FlowNode(ushort,IntPtr)"/> otherwise it will be ignored.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class FlowNodeAttribute : Attribute
	{
		/// <summary>
		/// Gets the name of the type of flow graph nodes.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Gets description of this flow node.
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// Gets a set of flags that specify this flow node.
		/// </summary>
		public FlowNodeFlags Flags { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="name">       Name of the type.</param>
		/// <param name="description">Description of the type.</param>
		/// <param name="flags">      Flags that describe this type.</param>
		public FlowNodeAttribute(string name, string description, FlowNodeFlags flags)
		{
			this.Name = name;
			this.Description = description;
			this.Flags = flags;
		}
	}
}