using System;

namespace CryEngine.Flowgraph
{
	public partial class FlowGraphNode
	{
		#region Events
		/// <summary>
		/// Occurs before one of this node's ports is activated.
		/// </summary>
		public event EventHandler<FlowNodePortActivationEventArgs> Activated;
		/// <summary>
		/// Occurs after one of this node's ports is activated.
		/// </summary>
		public event EventHandler<FlowNodePortActivationEventArgs> PostActivated;
		/// <summary>
		/// Occurs when this node is initialized.
		/// </summary>
		public event EventHandler Initialized;
		/// <summary>
		/// Occurs when <see cref="Alive"/> property changes its value.
		/// </summary>
		public event EventHandler<ValueChangedEventArgs> AliveOrDead;
		/// <summary>
		/// Occurs when this node is removed.
		/// </summary>
		public event EventHandler Destroyed;
		#endregion
		#region Event Raisers
		/// <summary>
		/// Raises event <see cref="Activated"/>.
		/// </summary>
		/// <param name="e">Details about the event.</param>
		protected virtual void OnActivated(FlowNodePortActivationEventArgs e)
		{
			if (this.Activated != null) this.Activated(this, e);
		}
		/// <summary>
		/// Raises event <see cref="PostActivated"/>.
		/// </summary>
		/// <param name="e">Details about the event.</param>
		protected virtual void OnPostActivated(FlowNodePortActivationEventArgs e)
		{
			if (this.PostActivated != null) this.PostActivated(this, e);
		}
		/// <summary>
		/// Raises event <see cref="Initialized"/>.
		/// </summary>
		protected virtual void OnInitialized()
		{
			if (this.Initialized != null) this.Initialized(this, EventArgs.Empty);
		}
		/// <summary>
		/// Raises event <see cref="AliveOrDead"/>.
		/// </summary>
		/// <param name="e">Details about the event.</param>
		protected virtual void OnAliveOrDead(ValueChangedEventArgs e)
		{
			if (this.AliveOrDead != null) this.AliveOrDead(this, e);
		}
		/// <summary>
		/// Raises event <see cref="Destroyed"/>.
		/// </summary>
		protected virtual void OnDestroyed()
		{
			if (this.Destroyed != null) this.Destroyed(this, EventArgs.Empty);
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates details about activation of the port on one of the FlowGraph nodes.
	/// </summary>
	public class FlowNodePortActivationEventArgs : EventArgs
	{
		/// <summary>
		/// Indicates whether the <see cref="Value"/> was sent into output.
		/// </summary>
		public bool IsOutput { get; set; }
		/// <summary>
		/// Name of the port.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Display name of the port.
		/// </summary>
		public string DisplayName { get; set; }
		/// <summary>
		/// Description of the port.
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// Type of the port.
		/// </summary>
		public NodePortType Type { get; set; }
		/// <summary>
		/// Value that went through the port, null, if port is typeless (
		/// <see cref="FlowNodePortActivationEventArgs.Type"/> equals <see cref="NodePortType.Void"/>).
		/// </summary>
		public object Value { get; set; }
	}
}