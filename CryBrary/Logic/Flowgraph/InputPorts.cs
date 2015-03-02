using System;
using System.Linq;
using System.Text;
using CryEngine.Entities;
using CryEngine.Logic.Entities;
using CryEngine.Mathematics;

namespace CryEngine.Logic.Flowgraph
{
	/// <summary>
	/// Base class for input ports.
	/// </summary>
	public abstract class InputPort : FlowNodePort
	{
		/// <summary>
		/// When implemented in derived class, get default value of this port.
		/// </summary>
		public abstract Native.FlowInputData DefaultValue { get; }
		/// <summary>
		/// When implemented in derived class, gets enumeration mapping for this port.
		/// </summary>
		public abstract string EnumConfig { get; }
		/// <summary>
		/// Writes configuration of this port to native memory.
		/// </summary>
		/// <param name="configPointer">Pointer to array that will contain the configuration.</param>
		/// <param name="index">        Index of this port.</param>
		public override unsafe void WriteConfiguration(IntPtr configPointer, int index)
		{
			((NativeInputPortConfiguration*)configPointer.ToPointer())[index] =
				new NativeInputPortConfiguration
				(
					TextPortPrefixes.List[(int)this.TextType] + this.Name,
					this.DisplayName,
					this.Description,
					this.EnumConfig,
					this.DefaultValue
				);
		}
		/// <summary>
		/// When implemented in derived class, handles activation of this port.
		/// </summary>
		/// <param name="value">Value that has been passed to the port.</param>
		public abstract void Activate(object value);
	}
	/// <summary>
	/// Represents a FlowGraph input port that takes no value.
	/// </summary>
	public class InputPortVoid : InputPort
	{
		#region Fields
		private readonly Action action;
		#endregion
		#region Properties
		/// <summary>
		/// <see cref="NodePortType.Void"/>.
		/// </summary>
		public override NodePortType Type
		{
			get { return NodePortType.Void; }
		}
		/// <summary>
		/// Gets default value of this port.
		/// </summary>
		public override Native.FlowInputData DefaultValue
		{
			get { return Native.FlowInputInterop.CreateVoid(); }
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		public override string EnumConfig
		{
			get { return ""; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new flow node input port.
		/// </summary>
		/// <param name="handler">Method that will handle activation of this port.</param>
		public InputPortVoid(Action handler)
		{
			this.action = handler;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Handles activation of this port.
		/// </summary>
		/// <param name="value">Ignored.</param>
		public override void Activate(object value)
		{
			this.action();
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that takes a value.
	/// </summary>
	/// <typeparam name="T">Type of value this port accepts.</typeparam>
	public class InputPort<T> : InputPort
	{
		#region Fields
		private readonly Action<T> handler;
		private bool invoked;
		private T lastValue;
		private readonly T defaultValue;
		private int typeIndex;
		private string enumConfig;
		#endregion
		#region Properties
		/// <summary>
		/// Returns last value that was passed to this port.
		/// </summary>
		/// <remarks>Returns default value, if this port has not yet been activated.</remarks>
		public T LastValue
		{
			get
			{
				return this.invoked ? this.lastValue : this.defaultValue;
			}
		}
		/// <summary>
		/// Gets default value of this port.
		/// </summary>
		public override Native.FlowInputData DefaultValue
		{
			get
			{
				switch (this.Type)
				{
					case NodePortType.Any:
						return Native.FlowInputInterop.CreateAny();
					case NodePortType.Void:
						return Native.FlowInputInterop.CreateVoid();
					case NodePortType.Int:
						return Native.FlowInputInterop.CreateInt((int)(object)this.defaultValue);
					case NodePortType.Float:
						return Native.FlowInputInterop.CreateFloat((float)(object)this.defaultValue);
					case NodePortType.EntityId:
						return Native.FlowInputInterop.CreateEntityId((EntityId)(object)this.defaultValue);
					case NodePortType.Vector3:
						return Native.FlowInputInterop.CreateVector((Vector3)(object)this.defaultValue);
					case NodePortType.String:
						return Native.FlowInputInterop.CreateString((string)(object)this.defaultValue);
					case NodePortType.Bool:
						return Native.FlowInputInterop.CreateBool((bool)(object)this.defaultValue);
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
		/// <summary>
		/// Gets enumeration mapping for this port.
		/// </summary>
		public override string EnumConfig
		{
			get
			{
				if (this.defaultValue is Enum)
				{
					if (this.enumConfig == null)
					{
						// Create enumeration text.
						int[] enumeratedValues = Enum.GetValues(typeof(T)).Cast<int>().ToArray();
						int lastIndex = enumeratedValues.Length - 1;
						if (enumeratedValues.Length == 0)
						{
							throw new FlowGraphException("Cannot use enumeration with no values for a FlowGraph node.");
						}
						StringBuilder builder = new StringBuilder(enumeratedValues.Length * 11);
						builder.Append("enum_int:");								// Beginning.
						for (int i = 0; i < enumeratedValues.Length; i++)
						{
							builder.Append(Enum.GetName(typeof(T), enumeratedValues[i]));	// Name...
							builder.Append('=');											// Equals...
							builder.Append(enumeratedValues[i]);							// Value...
							if (i != lastIndex)
							{
								builder.Append(',');	// Add comma if this is not the last value.
							}
						}
						this.enumConfig = builder.ToString();
					}
					return this.enumConfig;
				}
				return "";
			}
		}
		/// <summary>
		/// Gets the type of this port.
		/// </summary>
		public override NodePortType Type
		{
			get
			{
				if (this.typeIndex == -2)
				{
					if (this.defaultValue is int || this.defaultValue is Enum)
					{
						this.typeIndex = (int)NodePortType.Int;
					}
					if (this.defaultValue is float)
					{
						this.typeIndex = (int)NodePortType.Float;
					}
					if (this.defaultValue is bool)
					{
						this.typeIndex = (int)NodePortType.Bool;
					}
					if (this.defaultValue is string)
					{
						this.typeIndex = (int)NodePortType.String;
					}
					if (this.defaultValue is Vector3)
					{
						this.typeIndex = (int)NodePortType.Vector3;
					}
					if (this.defaultValue is EntityId)
					{
						this.typeIndex = (int)NodePortType.EntityId;
					}
					this.typeIndex = (int)NodePortType.Any;
				}
				return (NodePortType)this.typeIndex;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this part.
		/// </summary>
		/// <param name="handler">     Method that handles activation of this port.</param>
		/// <param name="defaultValue">
		/// Default value to pass to this port if no other port is connected to it.
		/// </param>
		public InputPort(Action<T> handler, object defaultValue)
		{
			this.handler = handler;
			this.defaultValue = (T)defaultValue;
			this.typeIndex = -2;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Handles activation of this port.
		/// </summary>
		/// <param name="value">Value that is passed to this port.</param>
		public override void Activate(object value)
		{
			this.lastValue = (T)value;
			this.handler((T)value);
		}
		#endregion
		#region Utilities

		#endregion
	}
}