using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for output ports.
	/// </summary>
	public abstract class OutputPort : FlowPort
	{
		#region Properties
		internal override FlowPortConfig Config
		{
			get { return new FlowPortConfig(this.Name, this.DisplayName, this.DisplayName, this.DataType); }
		}
		// Needed for port activation.
		internal IntPtr Graph { get; set; }
		internal ushort NodeId { get; set; }
		#endregion
		#region Events
		/// <summary>
		/// Occurs when this port is connected from another, therefore it activate one more node.
		/// </summary>
		public event Action<OutputPort> Connected;
		/// <summary>
		/// Occurs when this port is disconnected from another, therefore it activate one less node.
		/// </summary>
		public event Action<OutputPort> Disconnected;
		#endregion
		#region Event Riasers
		/// <summary>
		/// Raises the event <see cref="Connected"/>.
		/// </summary>
		internal virtual void OnConnected()
		{
			if (this.Connected != null) this.Connected(this);
		}
		/// <summary>
		/// Raises the event <see cref="Disconnected"/>.
		/// </summary>
		internal virtual void OnDisconnected()
		{
			if (this.Disconnected != null) this.Disconnected(this);
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes common fields of objects that are represented by classes that derive from this one.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		/// <param name="dataType">   Type of the data that can be transfered via this port.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		protected OutputPort(string name, string displayName, string description, FlowDataType dataType)
			: base(name, displayName, description, dataType)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port using given data.
		/// </summary>
		/// <param name="data">Data to transfer out of this port.</param>
		protected void Activate(FlowData data)
		{
			ActivateInternal(this.Graph, this.NodeId, this.PortId, data);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ActivateInternal(IntPtr graph, ushort nodeId, byte portId, FlowData data);
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer any data.
	/// </summary>
	public sealed class OutputPortAny : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortAny(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Any)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port.
		/// </summary>
		public void Activate()
		{
			this.Activate(new FlowData());
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(int value)
		{
			this.Activate(new FlowData(value));
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(float value)
		{
			this.Activate(new FlowData(value));
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(uint value)
		{
			this.Activate(new FlowData(value));
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(Vector3 value)
		{
			this.Activate(new FlowData(value));
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(string value)
		{
			this.Activate(new FlowData(value));
		}
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(bool value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that doesn't transfer any data.
	/// </summary>
	public sealed class OutputPortVoid : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortVoid(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Void)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port.
		/// </summary>
		public void Activate()
		{
			this.Activate(new FlowData());
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="int"/>.
	/// </summary>
	public sealed class OutputPortInt : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortInt(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Int)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(int value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="float"/>.
	/// </summary>
	public sealed class OutputPortFloat : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortFloat(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Float)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(float value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="uint"/>.
	/// </summary>
	public sealed class OutputPortEntityId : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortEntityId(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.EntityId)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(EntityId value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="Vector3"/>.
	/// </summary>
	public sealed class OutputPortVector3 : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortVector3(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Vector3)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(Vector3 value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="String"/>.
	/// </summary>
	public sealed class OutputPortString : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortString(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.String)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(string value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
	/// <summary>
	/// Represents an output port that can transfer data of type <see cref="Boolean"/>.
	/// </summary>
	public sealed class OutputPortBool : OutputPort
	{
		#region Construction
		/// <summary>
		/// Creates new wrapper for an output port.
		/// </summary>
		/// <param name="name">       Internal name of the port.</param>
		/// <param name="displayName">Display name of the port.</param>
		/// <param name="description">Description of the port.</param>
		public OutputPortBool(string name, string displayName, string description)
			: base(name, displayName, description, FlowDataType.Bool)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port and passes a value to the connected input port.
		/// </summary>
		/// <param name="value">Value to pass.</param>
		public void Activate(bool value)
		{
			this.Activate(new FlowData(value));
		}
		#endregion
	}
}