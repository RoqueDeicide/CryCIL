using System;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Union of objects that could represent input data for an input port.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FlowData
	{
		#region Fields
		/// <summary>
		/// 32-bit signed integer value.
		/// </summary>
		[FieldOffset(0)]
		public int Integer32;
		/// <summary>
		/// Single precision floating point value.
		/// </summary>
		[FieldOffset(0)]
		public float Float;
		/// <summary>
		/// Identifier of an entity.
		/// </summary>
		[FieldOffset(0)]
		public uint EntityId;
		/// <summary>
		/// 3 dimensional vector.
		/// </summary>
		[FieldOffset(0)]
		public Vector3 Vector3;
		/// <summary>
		/// Text information.
		/// </summary>
		[FieldOffset(0)]
		public IntPtr text;
		/// <summary>
		/// Boolean value.
		/// </summary>
		[FieldOffset(0)]
		public bool Bool;
		/// <summary>
		/// Type of this input data. Don't use fields or properties within this structure, if its type is
		/// <see cref="FlowDataType.Void"/>. Its type will never be equal to
		/// <see cref="FlowDataType.Any"/>.
		/// </summary>
		[FieldOffset(12)]
		public FlowDataType DataType;
		#endregion
		#region Properties
		/// <summary>
		/// Gets text information.
		/// </summary>
		public string String
		{
			get { return this.text != IntPtr.Zero ? Marshal.PtrToStringAnsi(this.text) : null; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new instance of this type that represents a default value of a certain type.
		/// </summary>
		/// <param name="type">Type of the data that this new object is representing.</param>
		/// <exception cref="ArgumentException">
		/// <see cref="FlowDataType.Any"/> cannot be used to create new instance of this type.
		/// </exception>
		public FlowData(FlowDataType type)
			: this()
		{
			if (type == FlowDataType.Any)
			{
				throw new ArgumentException("FlowDataType.Any cannot be used to create new instance of this type.");
			}
			this.DataType = type;
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="int"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(int value)
			: this()
		{
			this.DataType = FlowDataType.Int;
			this.Integer32 = value;
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="float"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(float value)
			: this()
		{
			this.DataType = FlowDataType.Float;
			this.Float = value;
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="uint"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(uint value)
			: this()
		{
			this.DataType = FlowDataType.EntityId;
			this.EntityId = value;
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="string"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(string value)
			: this()
		{
			this.DataType = FlowDataType.String;
			this.text = Marshal.StringToHGlobalAnsi(value);
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="Vector3"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(Vector3 value)
			: this()
		{
			this.DataType = FlowDataType.Vector3;
			this.Vector3 = value;
		}
		/// <summary>
		/// Initializes a new instance of this type that represents a value of type <see cref="bool"/>.
		/// </summary>
		/// <param name="value">A value itself.</param>
		public FlowData(bool value)
			: this()
		{
			this.DataType = FlowDataType.Bool;
			this.Bool = value;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases memory that was allocate for a text value of this object.
		/// </summary>
		public void Dispose()
		{
			if (this.DataType == FlowDataType.String && this.text != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.text);
				this.text = IntPtr.Zero;
			}
		}
		#endregion
		#region Utilities

		#endregion
	}
}