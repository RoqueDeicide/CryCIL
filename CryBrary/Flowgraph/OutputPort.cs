using System;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Used to declare flow node output ports that can output any or no value.
	/// </summary>
	public sealed class OutputPort
	{
		#region Properties
		/// <summary>
		/// Handle of the node that contains this output port.
		/// </summary>
		private IntPtr ParentNodePointer { get; set; }
		/// <summary>
		/// Identifier of this output port.
		/// </summary>
		private int PortId { get; set; }
		/// <summary>
		/// Indicates whether this node is active.
		/// </summary>
		public bool IsActive
		{
			get { return NativeFlowNodeMethods.IsOutputConnected(ParentNodePointer, PortId); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes default instance of this type.
		/// </summary>
		public OutputPort()
		{
		}
		/// <summary>
		/// Initializes new instance of type <see cref="OutputPort"/> .
		/// </summary>
		/// <param name="parentHandle">Parent node handle.</param>
		/// <param name="portId">      Identifier of this port.</param>
		public OutputPort(IntPtr parentHandle, int portId)
		{
			ParentNodePointer = parentHandle;
			PortId = portId;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this port without any data.
		/// </summary>
		public void Activate()
		{
			NativeFlowNodeMethods.ActivateOutput(ParentNodePointer, PortId);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> number to output upon activation.</param>
		public void Activate(int value)
		{
			NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="Single"/> value to output upon activation.</param>
		public void Activate(float value)
		{
			NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="EntityId"/> value to output upon activation.</param>
		public void Activate(EntityId value)
		{
			NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="String"/> value to output upon activation.</param>
		public void Activate(string value)
		{
			NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> value to output upon activation.</param>
		public void Activate(bool value)
		{
			NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="Vector3"/> value to output upon activation.</param>
		public void Activate(Vector3 value)
		{
			NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, value);
		}
		/// <summary>
		/// Activates this port.
		/// </summary>
		/// <param name="value"><see cref="Object"/> value to output upon activation.</param>
		public void Activate(object value)
		{
			if (value is int)
				NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, System.Convert.ToInt32(value));
			else if (value is float || value is double)
				NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, System.Convert.ToSingle(value));
			else if (value is EntityId)
				NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, ((EntityId)value).Value);
			else if (value is string)
				NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, System.Convert.ToString(value));
			else if (value is bool)
				NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, System.Convert.ToBoolean(value));
			else if (value is Vector3)
				NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, (Vector3)value);
			else
				throw new ArgumentException("Attempted to activate output with invalid value type!");
		}
		#endregion
	}
	/// <summary>
	/// Used to declare flow node output ports of a specified type.
	/// </summary>
	/// <typeparam name="T">
	/// <see cref="Int32"/> , <see cref="Single"/> , <see cref="EntityId"/> , <see cref="String"/> ,
	/// <see cref="Boolean"/> or <see cref="Vector3"/> .
	/// </typeparam>
	public sealed class OutputPort<T>
	{
		#region Properties
		/// <summary>
		/// Handle of the node that contains this output port.
		/// </summary>
		private IntPtr ParentNodePointer { get; set; }
		/// <summary>
		/// Identifier of this output port.
		/// </summary>
		private int PortId { get; set; }
		/// <summary>
		/// Indicates whether this node is active.
		/// </summary>
		public bool IsActive
		{
			get { return NativeFlowNodeMethods.IsOutputConnected(ParentNodePointer, PortId); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes default instance of this type.
		/// </summary>
		public OutputPort()
		{
		}
		/// <summary>
		/// Initializes new instance of type <see cref="OutputPort"/> .
		/// </summary>
		/// <param name="parentHandle">Parent node handle.</param>
		/// <param name="portId">      Identifier of this port.</param>
		public OutputPort(IntPtr parentHandle, int portId)
		{
			ParentNodePointer = parentHandle;
			PortId = portId;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this node.
		/// </summary>
		/// <param name="value">Value to output upon activation.</param>
		public void Activate(T value)
		{
			if (value is int)
				NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, Convert.ToInt32(value));
			else if (value is float || value is double)
				NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, Convert.ToSingle(value));
			else if (value is EntityId)
				NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, ((EntityId)(object)value).Value);
			else if (value is string)
				NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, Convert.ToString(value));
			else if (value is bool)
				NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, Convert.ToBoolean(value));
			else if (value is Vector3)
				NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, (Vector3)(object)value);
			else
				throw new ArgumentException("Attempted to activate output with invalid value type!");
		}
		#endregion
	}
}