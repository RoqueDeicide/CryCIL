using System;
using System.Collections.Generic;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.RunTime.Registration;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Base class for FlowGraph nodes.
	/// </summary>
	/// <remarks>
	/// When you derive from this class, you need to define default constructor.
	/// </remarks>
	public abstract partial class FlowGraphNode
	{
		#region Fields
		private bool? alive;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the native pointer to the flow node.
		/// </summary>
		public IntPtr Handle { get; internal set; }
		/// <summary>
		/// Indicates the filtering category of this node.
		/// </summary>
		public FlowNodeFilter Filter { get; protected set; }
		/// <summary>
		/// Indicates whether this node is singleton or instanced.
		/// </summary>
		public FlowNodeCloneType CloneType { get; protected set; }
		/// <summary>
		/// Gets category of this node.
		/// </summary>
		public string Category { get; protected set; }
		/// <summary>
		/// Gets description of this node.
		/// </summary>
		public string Description { get; protected set; }
		/// <summary>
		/// Gets flags set for this node.
		/// </summary>
		public FlowNodeFlags Flags { get; protected set; }
		/// <summary>
		/// Indicates whether this node is alive.
		/// </summary>
		/// <remarks>
		/// I think, therefore I am. Or this node will receive regular updates from FlowGraph, if
		/// it's alive.
		/// </remarks>
		public bool Alive
		{
			get { return this.alive ?? false; }
			set
			{
				bool changed = this.alive != value;
				bool? old = this.alive;
				Native.FlowNodeInterop.SetRegularlyUpdated(this.Handle, value);
				this.alive = value;
				if (changed)
				{
					this.OnAliveOrDead(new ValueChangedEventArgs { OldValue = old });
				}
			}
		}
		/// <summary>
		/// Indicates whether this node is singleton.
		/// </summary>
		public bool IsSingleton
		{
			get { return this.CloneType == FlowNodeCloneType.Singleton; }
		}
		/// <summary>
		/// Identifier of the node.
		/// </summary>
		public UInt16 NodeId { get; set; }
		/// <summary>
		/// Identifier of the Flow Graph.
		/// </summary>
		public UInt32 GraphId { get; set; }
		/// <summary>
		/// Gets entity associated with this node.
		/// </summary>
		public EntityBase TargetEntity { get; private set; }
		/// <summary>
		/// Gets the list of inputs this node has.
		/// </summary>
		public List<InputPort> Inputs { get; protected set; }
		/// <summary>
		/// Gets the list of outputs this node has.
		/// </summary>
		public List<InputPort> Outputs { get; protected set; }
		#endregion
		#region Construction
		/// <summary>
		/// Invoked from native code to create managed wrapper for a FlowGraph node.
		/// </summary>
		/// <param name="typeName">Name of the type that will represent a wrapper.</param>
		/// <param name="handle">Pointer to FlowGraph node object in native memory.</param>
		/// <param name="nodeId">Identifier of the node within containing graph.</param>
		/// <param name="graphId">Identifier of the FlowGraph that contains the node.</param>
		/// <returns>Managed wrapper for a native object.</returns>
		/// <exception cref="ArgumentException">Attempt to create managed wrapper for flow node using unknown type name.</exception>
		internal static FlowGraphNode Create(string typeName, IntPtr handle, ushort nodeId, uint graphId)
		{
			return (FlowGraphNode)Activator.CreateInstance(FlowNodeRegister.FlowNodeTypes[typeName]);
		}
		internal FlowGraphNode(IntPtr handle, ushort nodeId, uint graphId)
		{
			this.Handle = handle;
			this.NodeId = nodeId;
			this.GraphId = graphId;
		}
		#endregion
		#region Interface
		#region Overridables
		/// <summary>
		/// When implemented in derived class, configures this flow node.
		/// </summary>
		protected abstract void Configure();
		/// <summary>
		/// Overridden in derived class to perform extra initialization for the node.
		/// </summary>
		/// <remarks>
		/// When overriding this method, either put <see cref="OnInitialized"/> call into it or call
		/// base version of this method.
		/// </remarks>
		protected virtual void Initialize()
		{
			this.OnInitialized();
		}
		/// <summary>
		/// Overridden in derived class to perform to let the node react to changes that happen on
		/// timely manner.
		/// </summary>
		/// <remarks>
		/// This method will only be invoked, if this node is <see cref="Alive"/>. Do call this
		/// version of the method, when you override it.
		/// </remarks>
		protected virtual void Think()
		{
			// If we were somehow invoked without Alive being set, then we should update the
			// property to a proper value.
			if (this.alive.HasValue)
			{
				return;
			}
			this.alive = true;
		}
		#endregion
		#region Port Activation
		internal void ActivateInputPort(int portIndex, object value = null)
		{
			if (portIndex < 0 || portIndex >= this.Inputs.Count)
			{
				throw new ArgumentOutOfRangeException("portIndex");
			}
			FlowNodePort port = this.Inputs[portIndex];
			if (value == null && port.Type != NodePortType.Void)
			{
				throw new FlowGraphException("Cannot activate a node that requires a value without one.");
			}
			FlowNodePortActivationEventArgs eventArgs = new FlowNodePortActivationEventArgs
			{
				Name = port.Name,
				DisplayName = port.DisplayName,
				Description = port.Description,
				Type = port.Type,
				IsOutput = false,
				Value = value
			};
			this.OnActivated(eventArgs);
			this.Inputs[portIndex].Activate(value);
			this.OnPostActivated(eventArgs);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex)
		{
			FlowNodePort port = node.Outputs[portIndex];
			FlowNodePortActivationEventArgs eventArgs = new FlowNodePortActivationEventArgs
			{
				Name = port.Name,
				DisplayName = port.DisplayName,
				Description = port.Description,
				Type = port.Type,
				IsOutput = true,
				Value = null
			};
			node.OnActivated(eventArgs);
			Native.FlowNodeInterop.ActivateOutput(node.Handle, portIndex);
			node.OnPostActivated(eventArgs);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, int value)
		{
			node.ActivateOutput(portIndex, value, Native.FlowNodeInterop.ActivateOutputInt);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, float value)
		{
			node.ActivateOutput(portIndex, value, Native.FlowNodeInterop.ActivateOutputFloat);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, EntityId value)
		{
			node.ActivateOutput(portIndex, value.Value, Native.FlowNodeInterop.ActivateOutputEntityId);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, string value)
		{
			node.ActivateOutput(portIndex, value, Native.FlowNodeInterop.ActivateOutputString);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, bool value)
		{
			node.ActivateOutput(portIndex, value, Native.FlowNodeInterop.ActivateOutputBool);
		}
		internal static void ActivateOutput(FlowGraphNode node, int portIndex, Vector3 value)
		{
			node.ActivateOutput(portIndex, value, Native.FlowNodeInterop.ActivateOutputVec3);
		}
		private void ActivateOutput<T>(int portIndex, T value, Action<IntPtr, int, T> activator)
		{
			FlowNodePort port = this.Outputs[portIndex];
			FlowNodePortActivationEventArgs eventArgs = new FlowNodePortActivationEventArgs
			{
				Name = port.Name,
				DisplayName = port.DisplayName,
				Description = port.Description,
				Type = port.Type,
				IsOutput = true,
				Value = value
			};
			this.OnActivated(eventArgs);
			activator(this.Handle, portIndex, value);
			this.OnPostActivated(eventArgs);
		}
		#endregion
		#endregion
		#region Utilities
		internal void SetTargetEntity(IntPtr handle, EntityId entId)
		{
			this.TargetEntity = new NativeEntity(entId, handle);
		}
		internal void Destroy()
		{
			this.OnDestroyed();
		}
		#endregion
	}
}