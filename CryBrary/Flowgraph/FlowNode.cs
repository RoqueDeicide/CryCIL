﻿using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Extensions;
using CryEngine.Initialization;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Flowgraph.Native;

namespace CryEngine.Flowgraph
{
	/// <summary>
	/// Base class for all flow nodes.
	/// </summary>
	public abstract partial class FlowNode : CryScriptInstance
	{
		#region Fields
		internal IntPtr Handle { get; set; }
		#endregion
		#region Properties
		/// <summary>
		/// Gets entity associated with this node.
		/// </summary>
		public EntityBase TargetEntity { get; private set; }
		/// <summary>
		/// Identifier of the node.
		/// </summary>
		[CLSCompliant(false)]
		public UInt16 NodeId { get; set; }
		/// <summary>
		/// Identifier of the Flow Graph.
		/// </summary>
		[CLSCompliant(false)]
		public UInt32 GraphId { get; set; }
		/// <summary>
		/// Sets the value that indicates whether this node should receive per-frame updates.
		/// </summary>
		public bool ReceiveNodeUpdates
		{
			set { NativeFlowNodeMethods.SetRegularlyUpdated(Handle, value); }
		}
		#endregion
		#region Interface
		// ReSharper disable RedundantOverridenMember

		/// <summary>
		/// Called each frame if script has been set to be regularly updated. (See
		/// <see cref="CryScriptInstance.ReceiveUpdates"/> ) Warning: FlowNode logic such as
		/// <see cref="GetPortValue{T}"/> is not supported within this update loop, see
		/// <see cref="OnNodeUpdate"/> .
		/// </summary>
		public override void OnUpdate() { base.OnUpdate(); }
		// ReSharper restore RedundantOverridenMember

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 29 + ScriptId.GetHashCode();
				hash = hash * 29 + Handle.GetHashCode();
				hash = hash * 29 + NodeId.GetHashCode();
				hash = hash * 29 + GraphId.GetHashCode();

				return hash;
			}
		}
		#endregion
		#region Utilities
		internal override bool InternalInitialize(IScriptInitializationParams initParams)
		{
			var nodeInitParams = (NodeInitializationParams)initParams;

			Handle = nodeInitParams.nodePtr;
			NodeId = nodeInitParams.nodeId;
			GraphId = nodeInitParams.graphId;

			var registrationParams = (IFlowNodeBaseRegistrationParams)Script.RegistrationParams;

			// create instances of OutputPort's.
			for (int i = 0; i < registrationParams.OutputMembers.Length; i++)
			{
				var outputMember = registrationParams.OutputMembers[i];

				Type type = outputMember.MemberType ==
							MemberTypes.Field
					? ((FieldInfo)outputMember).FieldType
					: ((PropertyInfo)outputMember).PropertyType;

				bool isGenericType = type.IsGenericType;
				Type genericType = isGenericType ? type.GetGenericArguments()[0] : typeof(void);

				object[] outputPortConstructorArgs = { Handle, i };
				Type genericOutputPort = typeof(OutputPort<>);
				object outputPort =
					Activator.CreateInstance
					(
						isGenericType
							? genericOutputPort.MakeGenericType(genericType)
							: type,
						outputPortConstructorArgs
					);

				if (outputMember.MemberType == MemberTypes.Field)
					((FieldInfo)outputMember).SetValue(this, outputPort);
				else
					((PropertyInfo)outputMember).SetValue(this, outputPort, null);
			}

			return base.InternalInitialize(initParams);
		}

		internal virtual NodeConfig GetNodeConfig()
		{
			var registrationParams = (FlowNodeRegistrationParams)Script.RegistrationParams;

			return new NodeConfig(registrationParams.filter, registrationParams.description,
								  (registrationParams.hasTargetEntity ? FlowNodeFlags.TargetEntity : 0), registrationParams.type,
								  registrationParams.InputPorts, registrationParams.OutputPorts);
		}

		private int GetInputPortId(MethodInfo method)
		{
			var registrationParams = (IFlowNodeBaseRegistrationParams)Script.RegistrationParams;

			for (int i = 0; i < registrationParams.InputMethods.Length; i++)
			{
				if (registrationParams.InputMethods[i] == method)
					return i;
			}

			throw new ArgumentException("Invalid input method specified");
		}
		#region Callbacks
		/// <summary>
		/// Called if one or more input ports have been activated.
		/// </summary>
		internal void OnPortActivated(int index, object value = null)
		{
			var registrationParams = (IFlowNodeBaseRegistrationParams)Script.RegistrationParams;

			var method = registrationParams.InputMethods[index];

			var parameters = method.GetParameters();
			int parameterCount = parameters.Length;

			if (value != null && parameterCount == 1)
			{
				var paramType = method.GetParameters().ElementAt(0).ParameterType;
				var valueType = value.GetType();

				if (paramType != valueType && paramType != typeof(object))
				{
					var typeConverter = TypeDescriptor.GetConverter(paramType);
					if (!typeConverter.CanConvertFrom(valueType))
						return;

					value = typeConverter.ConvertFrom(value);
				}

				var args = new[] { value };
				method.Invoke(this, args);
			}
			else if (parameterCount == 1 && parameters.ElementAt(0).ParameterType == typeof(object))
				method.Invoke(this, new object[] { null });
			else if (parameterCount == 0)
				method.Invoke(this, null);
		}

		/// <summary>
		/// Called after level has been loaded, is not called on serialization. Note that this is
		/// called prior to GameRules.OnClientConnect and OnClientEnteredGame!
		/// </summary>
		protected virtual void OnInit()
		{
		}

		/// <summary>
		/// Called each frame if node has been set to be regularly updated (See
		/// <see cref="ReceiveNodeUpdates"/> ) Preferred over
		/// <see cref="CryScriptInstance.OnUpdate"/> due to supporting <see cref="GetPortValue{T}"/>
		/// within the update loop.
		/// </summary>
		protected virtual void OnNodeUpdate()
		{
		}

		/// <summary>
		/// Called when the node is removed.
		/// </summary>
		protected virtual void OnRemove()
		{
		}
		#endregion
		#region External methods
		/// <summary>
		/// Indicates whether specified input port is active.
		/// </summary>
		/// <typeparam name="T">Type of the port.</typeparam>
		/// <param name="port">Method that represents a port.</param>
		/// <returns>True, if input port is active.</returns>
		protected bool IsPortActive<T>(Action<T> port)
		{
			return NativeFlowNodeMethods.IsPortActive(Handle, GetInputPortId(port.Method));
		}
		/// <summary>
		/// Gets value that has been passed to input port upon its activation.
		/// </summary>
		/// <typeparam name="T">Type of the port.</typeparam>
		/// <param name="port">Method that represents a port.</param>
		/// <returns>Value that has been passed to input port upon its activation.</returns>
		protected T GetPortValue<T>(Action<T> port)
		{
			var type = typeof(T);

			if (type == typeof(int))
				return (T)(object)NativeFlowNodeMethods.GetPortValueInt(Handle, GetInputPortId(port.Method));
			if (type == typeof(float))
				return (T)(object)NativeFlowNodeMethods.GetPortValueFloat(Handle, GetInputPortId(port.Method));
			if (type == typeof(Vector3) || type == typeof(Color))
				return (T)(object)NativeFlowNodeMethods.GetPortValueVec3(Handle, GetInputPortId(port.Method));
			if (type == typeof(string))
				return (T)(object)NativeFlowNodeMethods.GetPortValueString(Handle, GetInputPortId(port.Method));
			if (type == typeof(bool))
				return (T)(object)NativeFlowNodeMethods.GetPortValueBool(Handle, GetInputPortId(port.Method));
			if (type == typeof(EntityId))
				return (T)(object)NativeFlowNodeMethods.GetPortValueEntityId(Handle, GetInputPortId(port.Method));
			if (type.IsEnum)
				return (T)Enum.ToObject(typeof(T), NativeFlowNodeMethods.GetPortValueInt(Handle, GetInputPortId(port.Method)));

			throw new ArgumentException("Invalid flow node port type specified!");
		}
		#endregion
		internal void InternalSetTargetEntity(IntPtr handle, EntityId entId)
		{
			TargetEntity = Entity.CreateNativeEntity(entId, handle);
		}
		#endregion
	}
}