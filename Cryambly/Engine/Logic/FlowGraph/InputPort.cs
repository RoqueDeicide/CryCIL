using System;
using System.Collections.Generic;
using System.Text;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for input ports.
	/// </summary>
	public abstract class InputPort : FlowPort
	{
		#region Fields
		internal string UiConfig;
		#endregion
		#region Events
		/// <summary>
		/// Occurs when this port is connected from another, therefore it can be activated by another node.
		/// </summary>
		public event Action<InputPort> Connected;
		/// <summary>
		/// Occurs when this port is disconnected from another, therefore it won't be activated by another
		/// node.
		/// </summary>
		public event Action<InputPort> Disconnected;
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
		protected InputPort(string name, string displayName, string description, FlowDataType dataType)
			: base(name, displayName, description, dataType)
		{
		}
		#endregion
		#region Interface
		// Overridden internally to activate this port.
		internal abstract void Activate();
		// Overridden internally to assign the value before activation of this port.
		internal abstract void Assign(FlowData input);
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortAny : InputPort
	{
		#region Fields
		private FlowData value;
		private readonly Action<FlowData> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public FlowData Value
		{
			get { return this.value; }
		}
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, null, new FlowData(FlowDataType.Any));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">       Name of this input port.</param>
		/// <param name="displayName">Display name of this input port.</param>
		/// <param name="description">Description of this input port.</param>
		/// <param name="action">     Method that will be invoked when this port is activated.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortAny(string name, string displayName, string description, Action<FlowData> action)
			: base(name, displayName, description, FlowDataType.Any)
		{
			this.action = action;
			this.value = new FlowData(FlowDataType.Any);
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.value);
		}
		internal override void Assign(FlowData input)
		{
			this.value = input;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortVoid : InputPort
	{
		#region Fields
		private readonly Action action;
		#endregion
		#region Properties
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, null, new FlowData(FlowDataType.Void));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">       Name of this input port.</param>
		/// <param name="displayName">Display name of this input port.</param>
		/// <param name="description">Description of this input port.</param>
		/// <param name="action">     Method that will be invoked when this port is activated.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortVoid(string name, string displayName, string description, Action action)
			: base(name, displayName, description, FlowDataType.Void)
		{
			this.action = action;
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action();
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.Void)
			{
				throw new Exception("Input port of type Void has been invoked with non-void data.");
			}
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortInt : InputPort
	{
		#region Fields
		private readonly Action<int> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public int Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(this.Value));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">            Name of this input port.</param>
		/// <param name="displayName">     Display name of this input port.</param>
		/// <param name="description">     Description of this input port.</param>
		/// <param name="action">          Method that will be invoked when this port is activated.</param>
		/// <param name="defaultValue">    Default value of this port.</param>
		/// <param name="selectableValues">
		/// A collection of key/value pairs where each key represents a selectable number and value
		/// represents the name of that value.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortInt(string name, string displayName, string description, Action<int> action,
							int defaultValue = 0, SortedList<int, string> selectableValues = null)
			: base(name, displayName, description, FlowDataType.Int)
		{
			this.action = action;
			this.Value = defaultValue;

			if (selectableValues != null && selectableValues.Count > 0)
			{
				StringBuilder b = new StringBuilder();
				b.Append("enum_int:");

				foreach (KeyValuePair<int, string> pair in selectableValues)
				{
					b.Append(pair.Value);
					b.Append("=");
					b.Append(pair.Key);
					b.Append(',');
				}

				// Remove the trailing comma. It's easier to do it this way rather then fiddle with the
				// iteration process.
				b.Remove(b.Length - 1, 1);

				this.UiConfig = b.ToString();
			}
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.Int)
			{
				throw new Exception("Input port of type Int has been invoked with non-integer data.");
			}

			this.Value = input.Integer32;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortFloat : InputPort
	{
		#region Fields
		private readonly Action<float> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public float Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(this.Value));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">            Name of this input port.</param>
		/// <param name="displayName">     Display name of this input port.</param>
		/// <param name="description">     Description of this input port.</param>
		/// <param name="action">          Method that will be invoked when this port is activated.</param>
		/// <param name="defaultValue">    Default value of this port.</param>
		/// <param name="selectableValues">
		/// A collection of key/value pairs where each key represents a selectable number and value
		/// represents the name of that value.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortFloat(string name, string displayName, string description, Action<float> action,
							  float defaultValue = 0, SortedList<float, string> selectableValues = null)
			: base(name, displayName, description, FlowDataType.Float)
		{
			this.action = action;
			this.Value = defaultValue;

			if (selectableValues != null && selectableValues.Count > 0)
			{
				StringBuilder b = new StringBuilder();
				b.Append("enum_float:");

				foreach (KeyValuePair<float, string> pair in selectableValues)
				{
					b.Append(pair.Value);
					b.Append("=");
					b.Append(pair.Key);
					b.Append(',');
				}

				// Remove the trailing comma. It's easier to do it this way rather then fiddle with the
				// iteration process.
				b.Remove(b.Length - 1, 1);

				this.UiConfig = b.ToString();
			}
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.Float)
			{
				throw new Exception("Input port of type Float has been invoked with non-floating-point data.");
			}

			this.Value = input.Float;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortEntityId : InputPort
	{
		#region Fields
		private readonly Action<uint> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public uint Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(FlowDataType.EntityId));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">       Name of this input port.</param>
		/// <param name="displayName">Display name of this input port.</param>
		/// <param name="description">Description of this input port.</param>
		/// <param name="action">     Method that will be invoked when this port is activated.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortEntityId(string name, string displayName, string description, Action<uint> action)
			: base(name, displayName, description, FlowDataType.EntityId)
		{
			this.action = action;
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.EntityId)
			{
				throw new Exception("Input port of type EntityId has been invoked with non-integer data.");
			}

			this.Value = input.EntityId;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortVector3 : InputPort
	{
		#region Fields
		private readonly Action<Vector3> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public Vector3 Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(this.Value));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">        Name of this input port.</param>
		/// <param name="displayName"> Display name of this input port.</param>
		/// <param name="description"> Description of this input port.</param>
		/// <param name="action">      Method that will be invoked when this port is activated.</param>
		/// <param name="defaultValue">Default value of this port.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortVector3(string name, string displayName, string description, Action<Vector3> action,
							  Vector3 defaultValue = new Vector3())
			: base(name, displayName, description, FlowDataType.Vector3)
		{
			this.action = action;
			this.Value = defaultValue;
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.Vector3)
			{
				throw new Exception("Input port of type Vector3 has been invoked with non-vector data.");
			}

			this.Value = input.Vector3;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortString : InputPort
	{
		#region Fields
		private readonly Action<string> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public string Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(this.Value));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">            Name of this input port.</param>
		/// <param name="displayName">     Display name of this input port.</param>
		/// <param name="description">     Description of this input port.</param>
		/// <param name="action">          Method that will be invoked when this port is activated.</param>
		/// <param name="defaultValue">    Default value of this port.</param>
		/// <param name="selectableValues">
		/// A collection of key/value pairs where each key represents a selectable number and value
		/// represents the name of that value.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortString(string name, string displayName, string description, Action<string> action,
							  string defaultValue = "", SortedList<string, string> selectableValues = null)
			: base(name, displayName, description, FlowDataType.String)
		{
			this.action = action;
			this.Value = defaultValue;

			if (selectableValues != null && selectableValues.Count > 0)
			{
				StringBuilder b = new StringBuilder();
				b.Append("enum_string:");

				foreach (KeyValuePair<string, string> pair in selectableValues)
				{
					b.Append(pair.Value);
					b.Append("=");
					b.Append(pair.Key);
					b.Append(',');
				}

				// Remove the trailing comma. It's easier to do it this way rather then fiddle with the
				// iteration process.
				b.Remove(b.Length - 1, 1);

				this.UiConfig = b.ToString();
			}
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.String)
			{
				throw new Exception("Input port of type String has been invoked with non-text data.");
			}

			this.Value = input.String;
		}
		#endregion
	}
	/// <summary>
	/// Represents an input port that is capable of accepting any data.
	/// </summary>
	public sealed class InputPortBool : InputPort
	{
		#region Fields
		private readonly Action<bool> action;
		#endregion
		#region Properties
		/// <summary>
		/// Gets current value of this port.
		/// </summary>
		public bool Value { get; private set; }
		internal override FlowPortConfig Config
		{
			get
			{
				return new FlowPortConfig
					(this.Name, this.DisplayName, this.DisplayName, this.UiConfig, new FlowData(this.Value));
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes this input port.
		/// </summary>
		/// <param name="name">        Name of this input port.</param>
		/// <param name="displayName"> Display name of this input port.</param>
		/// <param name="description"> Description of this input port.</param>
		/// <param name="action">      Method that will be invoked when this port is activated.</param>
		/// <param name="defaultValue">Default value of this port.</param>
		/// <exception cref="ArgumentException">
		/// Given name is not valid for flow node port, because it's not valid for Xml attribute name.
		/// </exception>
		public InputPortBool(string name, string displayName, string description, Action<bool> action,
							  bool defaultValue = false)
			: base(name, displayName, description, FlowDataType.Bool)
		{
			this.action = action;
			this.Value = defaultValue;
		}
		#endregion
		#region Interface
		internal override void Activate()
		{
			if (this.action != null) this.action(this.Value);
		}
		internal override void Assign(FlowData input)
		{
			if (input.DataType != FlowDataType.Bool)
			{
				throw new Exception("Input port of type Bool has been invoked with non-boolean data.");
			}

			this.Value = input.Bool;
		}
		#endregion
	}
}