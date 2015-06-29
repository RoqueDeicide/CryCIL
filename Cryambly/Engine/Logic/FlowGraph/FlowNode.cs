using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Engine.DebugServices;
using CryCil.RunTime;
using CryCil.RunTime.Registration;
using CryCil.Utilities;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for all FlowGraph nodes.
	/// </summary>
	public abstract partial class FlowNode : IDisposable
	{
		#region Nested Types
		// A set of flags that describe which properties of this flow node have been initialized.
		[Flags]
		private enum InitializationDetails
		{
			Flags = 1,			// When set indicates that flags are already assigned.
			Description = 2,	// When set indicates that description is already provided.
			Inputs = 4,			// When set indicates that input ports are already defined.
			Outputs = 8,		// When set indicates that output ports are already defined.
			All = Flags | Description | Inputs | Outputs
		}
		#endregion
		#region Fields
		private InitializationDetails initData;
		private FlowNodeFlags flags;
		private string description;
		private InputPort[] inputs;
		private OutputPort[] outputs;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a sets of flags that describes this flow node.
		/// </summary>
		/// <exception cref="Exception">
		/// Flags that describe this flow node can only be set once.
		/// </exception>
		public FlowNodeFlags Flags
		{
			get { return this.flags; }
			private set
			{
				if (this.initData.HasFlag(InitializationDetails.Flags))
				{
					throw new Exception("Flags that describe this flow node can only be set once.");
				}
				this.flags = value;
				this.initData |= InitializationDetails.Flags;
			}
		}
		/// <summary>
		/// Gets a sets of description this flow node.
		/// </summary>
		/// <exception cref="Exception">Description of this flow node can only be set once.</exception>
		public string Description
		{
			get { return this.description; }
			private set
			{
				if (this.initData.HasFlag(InitializationDetails.Description))
				{
					throw new Exception("Description of this flow node can only be set once.");
				}
				this.description = value;
				this.initData |= InitializationDetails.Description;
			}
		}
		/// <summary>
		/// Gets a sets an array of objects that represent input ports of this node.
		/// </summary>
		/// <exception cref="Exception">An array of input ports can only be set once.</exception>
		public InputPort[] Inputs
		{
			get { return this.inputs; }
			protected set
			{
				if (this.initData.HasFlag(InitializationDetails.Inputs))
				{
					throw new Exception("An array of input ports can only be set once.");
				}
				this.inputs = value;
				this.initData |= InitializationDetails.Inputs;
			}
		}
		/// <summary>
		/// Gets a sets an array of objects that represent output ports of this node.
		/// </summary>
		/// <exception cref="Exception">An array of output ports can only be set once.</exception>
		public OutputPort[] Outputs
		{
			get { return this.outputs; }
			protected set
			{
				if (this.initData.HasFlag(InitializationDetails.Outputs))
				{
					throw new Exception("An array of output ports can only be set once.");
				}
				this.outputs = value;
				this.initData |= InitializationDetails.Outputs;
			}
		}

		/// <summary>
		/// Gets the identifier of this node.
		/// </summary>
		public ushort Id { get; private set; }

		internal IntPtr GraphHandle { get; private set; }
		/// <summary>
		/// Gets identifier of the entity this node targets.
		/// </summary>
		public EntityId TargetEntityId { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Initializes base properties of the new flow node wrapper object.
		/// </summary>
		/// <param name="id">   Identifier of the node.</param>
		/// <param name="graph">
		/// Pointer to the native object that represents the FlowGraph where this node is located.
		/// </param>
		protected FlowNode(ushort id, IntPtr graph)
		{
			this.Id = id;
			this.GraphHandle = graph;
		}
		/// <summary>
		/// Releases this wrapper.
		/// </summary>
		~FlowNode()
		{
			this.Dispose();
			Deactivate(this.GraphHandle, this.Id);
			this.PostDispose();
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Deactivate(IntPtr graphHandle, ushort nodeId);
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, defines a list of specific properties of this flow node.
		/// </summary>
		public abstract void Define();
		/// <summary>
		/// Can be overridden in derived class to release resources held by this wrapper object.
		/// </summary>
		/// <remarks>
		/// Do not resurrect this object when this method is called. The node this object is supposed to
		/// represent is already deleted at this point.
		/// </remarks>
		public virtual void Dispose()
		{
		}
		/// <summary>
		/// Can be overridden in derived class to save various data into Xml node.
		/// </summary>
		/// <remarks>Used when saving and loading (like in the editor). Probably.</remarks>
		/// <param name="node">Object that represents the root Xml node for this flow node.</param>
		/// <returns>True, if successful or false, if something went wrong.</returns>
		public virtual bool Save(CryXmlNode node)
		{
			return true;
		}
		/// <summary>
		/// Can be overridden in derived class to load various data from Xml node.
		/// </summary>
		/// <remarks>Used when saving and loading (like in the editor). Probably.</remarks>
		/// <param name="node">Object that represents the root Xml node for this flow node.</param>
		/// <returns>True, if successful or false, if something went wrong.</returns>
		public virtual bool Load(CryXmlNode node)
		{
			return true;
		}
		/// <summary>
		/// Can be overridden in derived class to update logical state of this node.
		/// </summary>
		public virtual void Think()
		{
		}
		/// <summary>
		/// Can be overridden in derived class to react to activation of multiple ports at once.
		/// </summary>
		/// <param name="activatedPorts">
		/// A collection of objects that represent input ports that were activated (with new values already assigned to them).
		/// </param>
		public virtual void MultiActivate(ActivationSet activatedPorts)
		{
		}
		/// <summary>
		/// Can be overridden in derived class to precache resources that will be used by this node.
		/// </summary>
		public virtual void PrecacheResources()
		{
		}
		/// <summary>
		/// Can be overridden in derived class to synchronize the state of this node.
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public virtual void Synchronizing(CrySync sync)
		{
		}
		/// <summary>
		/// Can be overridden in derived class to update state of this node after synchronization.
		/// </summary>
		public virtual void Synchronized()
		{
		}
		#endregion
		#region Utilities
		private void PostDispose()
		{
			this.Id = 0;
		}
		[UnmanagedThunk("Invoked from underlying framework to create a wrapper for a new node.")]
		private static object Create(IntPtr grapHandle, ushort typeId, ushort nodeId)
		{
			Type type = FlowNodeTypeRegistry.GetFlowNodeType(typeId);
			return
				type == null
					? null
					: Activator.CreateInstance(type, new object[] { nodeId, grapHandle });
		}
		[RawThunk("Invoked from underlying framework when this node is removed completely from the game.")]
		private void Release()
		{
			try
			{
				this.Dispose();
				GC.SuppressFinalize(this);
				this.PostDispose();
			}
			catch (Exception ex)
			{
				// Catch everything, since this will be invoked through raw thunk.
				MonoInterface.DisplayException(ex);
			}
		}
		[UnmanagedThunk("Invoked from underlying framework to inform the flow system about how this node works.")]
		private void GetConfiguration(out FlowNodeConfig config)
		{
			if (this.initData != InitializationDetails.All)
			{
				this.Define();

				FlowNodeAttribute attribute = this.GetType().GetAttribute<FlowNodeAttribute>();
				this.Flags = attribute.Flags;
				this.Description = attribute.Description;

				if (this.initData != InitializationDetails.All)
				{
					string errorText = string.Format("The node {0} is not fully initialized.", attribute.Name);
#if DEBUG
					throw new Exception(errorText);
#else
				Log.Error(errorText, true);
#endif
				}

				for (byte i = 0; i < this.outputs.Length; i++)
				{
					this.outputs[i].Graph = this.GraphHandle;
					this.outputs[i].NodeId = this.Id;
					this.outputs[i].PortId = i;
				}
				for (byte i = 0; i < this.inputs.Length; i++)
				{
					this.inputs[i].PortId = i;
				}
			}

			config = new FlowNodeConfig
			{
				Description = StringPool.Get(this.description),
				Flags = this.flags,
				Inputs = this.inputs.Select(input => input.Config).ToArray(),
				Outputs = this.outputs.Select(output => output.Config).ToArray()
			};
		}
		[RawThunk("Invoked from underlying framework to save some data to Xml.")]
		private bool SaveData(IntPtr xmlHandle)
		{
			try
			{
				return this.Save(new CryXmlNode(xmlHandle));
			}
			catch (Exception)
			{
				return false;
			}
		}
		[RawThunk("Invoked from underlying framework to load some data to Xml.")]
		private bool LoadData(IntPtr xmlHandle)
		{
			try
			{
				return this.Load(new CryXmlNode(xmlHandle));
			}
			catch (Exception)
			{
				return false;
			}
		}
		[RawThunk("Invoked from underlying framework to synchronize this node.")]
		private void Serialize(CrySync sync)
		{
			try
			{
				this.Synchronizing(sync);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to synchronize this node.")]
		private void PostSerialize()
		{
			try
			{
				this.Synchronized();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to update this node.")]
		private void Update()
		{
			try
			{
				this.Think();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to inform this node about input ports being activated.")]
		private void Activate(byte[] portIds, FlowData[] values)
		{
			try
			{
				SortedList<InputPort, bool> activatedPorts =
					new SortedList<InputPort, bool>(portIds.ToDictionary(id => this.inputs[id], id => false));

				for (int i = 0; i < activatedPorts.Keys.Count; i++)
				{
					activatedPorts.Keys[i].Assign(values[portIds[i]]);
				}

				this.MultiActivate(new ActivationSet(activatedPorts));

				var portsToActivate =
					from activatedPort in activatedPorts
					where !activatedPort.Value
					select activatedPort.Key;

				foreach (InputPort port in portsToActivate)
				{
					port.Activate();
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to trigger precaching of resources.")]
		private void PrecacheResourcesInternal()
		{
			try
			{
				this.PrecacheResources();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to initialize this node.")]
		private void Initialize()
		{
			try
			{
				this.OnInitializing();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework after initializing all nodes.")]
		private void PostInitialize()
		{
			try
			{
				this.OnInitialized();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to set the entity identifier.")]
		private void SetEntityId(EntityId id)
		{
			try
			{
				this.TargetEntityId = id;
				this.OnEntityTargeted(id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework when this node is suspended.")]
		private void Suspend()
		{
			try
			{
				this.OnSuspended();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework when this node is resumed.")]
		private void Resume()
		{
			try
			{
				this.OnResumed();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}

		// I accidentally made a small reversal of events here, shouldn't matter unless you are familiar
		// with source code of the flow graph system.
		[RawThunk("Invoked from underlying framework when an input port is connected to this node.")]
		private void ConnectInputPort(byte portId)
		{
			try
			{
				this.outputs[portId].OnConnected();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework when an input port is disconnected from this node.")]
		private void DisconnectInputPort(byte portId)
		{
			try
			{
				this.outputs[portId].OnDisconnected();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework when an output port is connected to this node.")]
		private void ConnectOutputPort(byte portId)
		{
			try
			{
				this.inputs[portId].OnConnected();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework when an output port is disconnected from this node.")]
		private void DisconnectOutputPort(byte portId)
		{
			try
			{
				this.inputs[portId].OnDisconnected();
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}