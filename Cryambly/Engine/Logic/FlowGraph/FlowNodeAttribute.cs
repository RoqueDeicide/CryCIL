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
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="name">Name of the type.</param>
		public FlowNodeAttribute(string name)
		{
			this.Name = name;
		}
	}
}