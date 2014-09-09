using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Annotations;
using CryEngine.Entities;
using CryEngine.Mathematics;

namespace CryEngine.Flowgraph
{
	internal static class PortActivators
	{
		internal static readonly Delegate[] Activators;
		static PortActivators()
		{
			Activators = new Delegate[7];
			// Indices of delegates here are mapped to indices of port types in NodePortType enumeration.
			Activators[0] = new Action<FlowGraphNode, int>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, int>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, float>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, EntityId>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, Vector3>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, string>(FlowGraphNode.ActivateOutput);
			Activators[1] = new Action<FlowGraphNode, int, bool>(FlowGraphNode.ActivateOutput);
		}
	}
	/// <summary>
	/// Base class for output ports.
	/// </summary>
	public abstract class OutputPort : FlowNodePort
	{
		/// <summary>
		/// Writes configuration of this port to native memory.
		/// </summary>
		/// <param name="configPointer">Pointer to array that will contain the configuration.</param>
		/// <param name="index">        Index of this port.</param>
		public override unsafe void WriteConfiguration(IntPtr configPointer, int index)
		{
			((NativeOutputPortConfiguration*)configPointer.ToPointer())[index] =
				new NativeOutputPortConfiguration
				(
					TextPortPrefixes.List[(int)this.TextType] + this.Name,
					this.DisplayName,
					this.Description,
					(int)this.Type
				);
		}
	}
	/// <summary>
	/// Represents a flow node port that does not output a value.
	/// </summary>
	public class OutputPortVoid : OutputPort
	{
		#region Properties
		/// <summary>
		/// <see cref="NodePortType.Void"/>
		/// </summary>
		public override NodePortType Type
		{
			get { return NodePortType.Void; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port.
		/// </summary>
		public void Activate()
		{
			((Action<FlowGraphNode, int>)PortActivators.Activators[0])(this.NodeHandle, this.Identifier);
		}
		#endregion
	}
	/// <summary>
	/// Represents a flow node port that does not output a value.
	/// </summary>
	public class OutputPort<T> : OutputPort
	{
		#region Statics
		#endregion
		#region Fields
		private int typeIndex = -2;
		#endregion
		#region Properties
		/// <summary>
		/// Gets type of this port.
		/// </summary>
		public override NodePortType Type
		{
			get
			{
				if (this.typeIndex == -2)
				{
					if (typeof(T) == typeof(int) || typeof(T).IsEnum)
					{
						this.typeIndex = (int)NodePortType.Int;
					}
					if (typeof(T) == typeof(float))
					{
						this.typeIndex = (int)NodePortType.Float;
					}
					if (typeof(T) == typeof(bool))
					{
						this.typeIndex = (int)NodePortType.Bool;
					}
					if (typeof(T) == typeof(string))
					{
						this.typeIndex = (int)NodePortType.String;
					}
					if (typeof(T) == typeof(Vector3))
					{
						this.typeIndex = (int)NodePortType.Vector3;
					}
					if (typeof(T) == typeof(EntityId))
					{
						this.typeIndex = (int)NodePortType.EntityId;
					}
					this.typeIndex = (int)NodePortType.Any;
				}
				return (NodePortType)this.typeIndex;
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value">Value that is outputted by this port.</param>
		public void Activate(T value)
		{
			((Action<FlowGraphNode, int, T>)PortActivators.Activators[(int)this.Type])(this.NodeHandle, this.Identifier, value);
		}
		#endregion
	}
}