using System;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Used to declare flownode output ports that can output any or no value.
	/// </summary>
	public sealed class OutputPort
	{
		public OutputPort()
		{
		}

		public OutputPort(IntPtr nodePtr, int portId)
		{
			ParentNodePointer = nodePtr;
			PortId = portId;
		}

		public void Activate()
		{
			NativeFlowNodeMethods.ActivateOutput(ParentNodePointer, PortId);
		}

		public void Activate(int value)
		{
			NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, value);
		}

		public void Activate(float value)
		{
			NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, value);
		}

		public void Activate(EntityId value)
		{
			NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, value);
		}

		public void Activate(string value)
		{
			NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, value);
		}

		public void Activate(bool value)
		{
			NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, value);
		}

		public void Activate(Vector3 value)
		{
			NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, value);
		}

		public void Activate(object value)
		{
			if (value is int)
				NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, System.Convert.ToInt32(value));
			else if (value is float || value is double)
				NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, System.Convert.ToSingle(value));
			else if (value is EntityId)
				NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, (uint)((EntityId)(object)value).Value);
			else if (value is string)
				NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, System.Convert.ToString(value));
			else if (value is bool)
				NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, System.Convert.ToBoolean(value));
			else if (value is Vector3)
				NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, (Vector3)(object)value);
			else
				throw new ArgumentException("Attempted to activate output with invalid value type!");
		}

		public bool IsActive()
		{
			return NativeFlowNodeMethods.IsOutputConnected(ParentNodePointer, PortId);
		}

		private IntPtr ParentNodePointer { get; set; }

		private int PortId { get; set; }
	}

	/// <summary>
	/// Used to declare flownode output ports of a specified type.
	/// </summary>
	/// <typeparam name="T">Int, Float, EntityId, String, Bool or Vec3.</typeparam>
	public sealed class OutputPort<T>
	{
		public OutputPort()
		{
		}

		public OutputPort(IntPtr nodePtr, int portId)
		{
			ParentNodePointer = nodePtr;
			PortId = portId;
		}

		public void Activate(T value)
		{
			if (value is int)
				NativeFlowNodeMethods.ActivateOutputInt(ParentNodePointer, PortId, System.Convert.ToInt32(value));
			else if (value is float || value is double)
				NativeFlowNodeMethods.ActivateOutputFloat(ParentNodePointer, PortId, System.Convert.ToSingle(value));
			else if (value is EntityId)
				NativeFlowNodeMethods.ActivateOutputEntityId(ParentNodePointer, PortId, (uint)((EntityId)(object)value).Value);
			else if (value is string)
				NativeFlowNodeMethods.ActivateOutputString(ParentNodePointer, PortId, System.Convert.ToString(value));
			else if (value is bool)
				NativeFlowNodeMethods.ActivateOutputBool(ParentNodePointer, PortId, System.Convert.ToBoolean(value));
			else if (value is Vector3)
				NativeFlowNodeMethods.ActivateOutputVec3(ParentNodePointer, PortId, (Vector3)(object)value);
			else
				throw new ArgumentException("Attempted to activate output with invalid value type!");
		}

		public bool IsActive()
		{
			return NativeFlowNodeMethods.IsOutputConnected(ParentNodePointer, PortId);
		}

		private IntPtr ParentNodePointer { get; set; }

		private int PortId { get; set; }
	}
}