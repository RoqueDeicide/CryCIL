using System;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Base class for flow node ports.
	/// </summary>
	public abstract class FlowNodePort
	{
		#region Fields
		private string name;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the name of the port.
		/// </summary>
		public string Name
		{
			get { return this.name; }
			set
			{
				if (!value.IsValidFlowGraphName())
				{
					throw new ArgumentException("Given name is not valid for flow node port.");
				}
				this.name = value;
			}
		}
		/// <summary>
		/// Gets or sets display name of the port.
		/// </summary>
		public string DisplayName { get; set; }
		/// <summary>
		/// Gets or sets the description of the port.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Gets or sets type of the value that is inputed into or outputted from the port.
		/// </summary>
		public abstract NodePortType Type { get; }
		/// <summary>
		/// Indicates the meaning behind a value of a text port.
		/// </summary>
		public StringPortType TextType { get; set; }
		/// <summary>
		/// Gets or sets the pointer to FlowGraph node that contains this port.
		/// </summary>
		public FlowGraphNode NodeHandle { get; private set; }
		/// <summary>
		/// Gets or sets identifier of this port.
		/// </summary>
		public int Identifier { get; private set; }
		#endregion
		#region Construction
		internal void Initialize(FlowGraphNode nodeHandle, int id)
		{
			this.NodeHandle = nodeHandle;
			this.Identifier = id;
		}
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, writes the configuration of the port to the relevant array
		/// pointer to which is provided by a struct pointer.
		/// </summary>
		/// <param name="configPointer">Pointer to array of configuration structures.</param>
		/// <param name="index">        Index of this port in the array.</param>
		public abstract void WriteConfiguration(IntPtr configPointer, int index);
		#endregion
	}
}