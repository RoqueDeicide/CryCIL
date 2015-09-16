using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Graphics;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Enumeration of types the shader parameter can be.
	/// </summary>
	public enum ShaderParameterType
	{
		/// <summary>
		/// Unspecified type.
		/// </summary>
		Unknown,
		/// <summary>
		/// 8-bit integer.
		/// </summary>
		Byte,
		/// <summary>
		/// Boolean value.
		/// </summary>
		Bool,
		/// <summary>
		/// 16-bit integer.
		/// </summary>
		Short,
		/// <summary>
		/// 32-bit integer.
		/// </summary>
		Int,
		/// <summary>
		/// Half-precision floating point parameter.
		/// </summary>
		Half,
		/// <summary>
		/// Single-precision floating point parameter.
		/// </summary>
		Float,
		/// <summary>
		/// Text value.
		/// </summary>
		String,
		/// <summary>
		/// Color value.
		/// </summary>
		Color,
		/// <summary>
		/// Vector value.
		/// </summary>
		Vector,
		/// <summary>
		/// An identifier that can be used to access a texture used by the shader.
		/// </summary>
		TextureHandle,
		/// <summary>
		/// A pointer to the <see cref="Graphics.Camera"/> object.
		/// </summary>
		Camera
	}
	/// <summary>
	/// Encapsulates information about one of the parameters that specifies behavior of the shader.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct ShaderParameter
	{
		#region Internal Types
		[StructLayout(LayoutKind.Explicit)]
		private struct ParameterValue
		{
			[FieldOffset(0)] public Byte1 byte1;
			[FieldOffset(0)] public Bytes2 bytes2;
			[FieldOffset(0)] public Bytes4 bytes4;
			[UsedImplicitly] [FieldOffset(0)] public IntPtr ntString;
			[FieldOffset(0)] public ColorSingle color;
			[FieldOffset(0)] public Vector3 vector;
			[FieldOffset(0)] public readonly Camera* camera;
		}
		#endregion
		#region Fields
		[UsedImplicitly] [FieldOffset(0)] private byte nameBuffer;
		[UsedImplicitly] [FieldOffset(32)] private ShaderParameterType currentType;
		[FieldOffset(36)] private ParameterValue value;
		[UsedImplicitly] [FieldOffset(52)] private IntPtr script;
		[UsedImplicitly]
#if WIN32
		[FieldOffset(56)]
#else
		[FieldOffset(60)]
#endif
			private byte semantic;
		#endregion
		#region Properties
		#region General Values
		/// <summary>
		/// Gets or sets boolean value of this parameter. If any other numeric value is stored here, it
		/// will be converted to <see cref="bool"/>. Avoid using it unless this is a parameter of that
		/// type. <c>false</c> will be returned if this shader parameter is not an integer number.
		/// </summary>
		public bool Bool
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return this.value.byte1.UnsignedByte != 0;
					case ShaderParameterType.Bool:
						return this.value.bytes4.Boolean;
					case ShaderParameterType.Short:
						return this.value.bytes2.SignedShort != 0;
					case ShaderParameterType.Int:
						return this.value.bytes4.SignedInt != 0;
					default:
						return false;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = value ? (byte)1 : (byte)0;
						break;
					case ShaderParameterType.Bool:
						this.value.bytes4.Boolean = value;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = value ? (short)1 : (short)0;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = value ? 1 : 0;
						break;
				}
			}
		}
		/// <summary>
		/// Gets or sets 8-bit integer value of this parameter. If any other numeric value is stored here,
		/// it will be converted to <see cref="byte"/>. Avoid using it unless this is a parameter of that
		/// type. 0 will be returned if this shader parameter is not numeric.
		/// </summary>
		public byte Byte
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return this.value.byte1.UnsignedByte;
					case ShaderParameterType.Bool:
						return this.value.bytes4.Boolean ? (byte)1 : (byte)0;
					case ShaderParameterType.Short:
						return (byte)this.value.bytes2.SignedShort;
					case ShaderParameterType.Int:
						return (byte)this.value.bytes4.SignedInt;
					case ShaderParameterType.Half:
						return (byte)this.value.bytes2.HalfFloat;
					case ShaderParameterType.Float:
						return (byte)this.value.bytes4.SingleFloat;
					default:
						return 0;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = value;
						break;
					case ShaderParameterType.Bool:
						this.value.bytes4.Boolean = value != 0;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = value;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = value;
						break;
					case ShaderParameterType.Half:
						this.value.bytes2.HalfFloat = (Half)value;
						break;
					case ShaderParameterType.Float:
						this.value.bytes4.SingleFloat = value;
						break;
				}
			}
		}
		/// <summary>
		/// Gets or sets 16-bit integer value of this parameter. If any other numeric value is stored here,
		/// it will be converted to <see cref="short"/>. Avoid using it unless this is a parameter of that
		/// type. 0 will be returned if this shader parameter is not numeric.
		/// </summary>
		public short Short
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return this.value.byte1.UnsignedByte;
					case ShaderParameterType.Bool:
						return this.value.bytes4.Boolean ? (short)1 : (short)0;
					case ShaderParameterType.Short:
						return this.value.bytes2.SignedShort;
					case ShaderParameterType.Int:
						return (short)this.value.bytes4.SignedInt;
					case ShaderParameterType.Half:
						return (short)this.value.bytes2.HalfFloat;
					case ShaderParameterType.Float:
						return (short)this.value.bytes4.SingleFloat;
					default:
						return 0;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = (byte)value;
						break;
					case ShaderParameterType.Bool:
						this.value.bytes4.Boolean = value != 0;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = value;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = value;
						break;
					case ShaderParameterType.Half:
						this.value.bytes2.HalfFloat = (Half)value;
						break;
					case ShaderParameterType.Float:
						this.value.bytes4.SingleFloat = value;
						break;
				}
			}
		}
		/// <summary>
		/// Gets or sets half-precision floating point value of this parameter. If any other numeric value
		/// is stored here, it will be converted to <see cref="Half"/>. Avoid using it unless this is a
		/// parameter of that type. NaN will be returned if this shader parameter is not numeric.
		/// </summary>
		public Half Half
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return (Half)this.value.byte1.UnsignedByte;
					case ShaderParameterType.Short:
						return (Half)this.value.bytes2.SignedShort;
					case ShaderParameterType.Int:
						return (Half)this.value.bytes4.SignedInt;
					case ShaderParameterType.Half:
						return this.value.bytes2.HalfFloat;
					case ShaderParameterType.Float:
						return (Half)this.value.bytes4.SingleFloat;
					default:
						return Half.NaN;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = (byte)value;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = (short)value;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = (int)value;
						break;
					case ShaderParameterType.Half:
						this.value.bytes2.HalfFloat = value;
						break;
					case ShaderParameterType.Float:
						this.value.bytes4.SingleFloat = (float)value;
						break;
				}
			}
		}
		/// <summary>
		/// Gets or sets 32-bit integer value of this parameter. If any other numeric value is stored here,
		/// it will be converted to <see cref="int"/>. Avoid using it unless this is a parameter of that
		/// type. 0 will be returned if this shader parameter is not numeric.
		/// </summary>
		public int Int
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return this.value.byte1.UnsignedByte;
					case ShaderParameterType.Bool:
						return this.value.bytes4.Boolean ? 1 : 0;
					case ShaderParameterType.Short:
						return this.value.bytes2.SignedShort;
					case ShaderParameterType.Int:
						return this.value.bytes4.SignedInt;
					case ShaderParameterType.Half:
						return (int)this.value.bytes2.HalfFloat;
					case ShaderParameterType.Float:
						return (int)this.value.bytes4.SingleFloat;
					default:
						return 0;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = (byte)value;
						break;
					case ShaderParameterType.Bool:
						this.value.bytes4.Boolean = value != 0;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = (short)value;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = value;
						break;
					case ShaderParameterType.Half:
						this.value.bytes2.HalfFloat = (Half)value;
						break;
					case ShaderParameterType.Float:
						this.value.bytes4.SingleFloat = value;
						break;
				}
			}
		}
		/// <summary>
		/// Gets or sets single-precision floating point number stored in this parameter. If any other
		/// numeric value is stored here, it will be converted to <see cref="float"/>. Avoid using it
		/// unless this is a parameter of that type. NaN will be returned if this shader parameter is not
		/// numeric.
		/// </summary>
		public float Float
		{
			get
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						return this.value.byte1.UnsignedByte;
					case ShaderParameterType.Short:
						return this.value.bytes2.SignedShort;
					case ShaderParameterType.Int:
						return this.value.bytes4.SignedInt;
					case ShaderParameterType.Half:
						return (float)this.value.bytes2.HalfFloat;
					case ShaderParameterType.Float:
						return this.value.bytes4.SingleFloat;
					default:
						return Single.NaN;
				}
			}
			set
			{
				switch (this.currentType)
				{
					case ShaderParameterType.Byte:
						this.value.byte1.UnsignedByte = (byte)value;
						break;
					case ShaderParameterType.Short:
						this.value.bytes2.SignedShort = (short)value;
						break;
					case ShaderParameterType.Int:
						this.value.bytes4.SignedInt = (int)value;
						break;
					case ShaderParameterType.Half:
						this.value.bytes2.HalfFloat = (Half)value;
						break;
					case ShaderParameterType.Float:
						this.value.bytes4.SingleFloat = value;
						break;
				}
			}
		}
		#endregion
		#region Specific Values
		/// <summary>
		/// Specifically gets or sets boolean value of this parameter without any conversions. Avoid using
		/// it unless this is a parameter of that type.
		/// </summary>
		public bool BoolSpecific
		{
			get { return this.value.bytes4.Boolean; }
			set { this.value.bytes4.Boolean = value; }
		}
		/// <summary>
		/// Specifically gets or sets 8-bit integer value of this parameter without any conversions. Avoid
		/// using it unless this is a parameter of that type.
		/// </summary>
		public byte ByteSpecific
		{
			get { return this.value.byte1.UnsignedByte; }
			set { this.value.byte1.UnsignedByte = value; }
		}
		/// <summary>
		/// Specifically gets or sets 16-bit integer value of this parameter without any conversions. Avoid
		/// using it unless this is a parameter of that type.
		/// </summary>
		public short ShortSpecific
		{
			get { return this.value.bytes2.SignedShort; }
			set { this.value.bytes2.SignedShort = value; }
		}
		/// <summary>
		/// Specifically gets or sets half-precision floating point value of this parameter without any
		/// conversions. Avoid using it unless this is a parameter of that type.
		/// </summary>
		public Half HalfSpecific
		{
			get { return this.value.bytes2.HalfFloat; }
			set { this.value.bytes2.HalfFloat = value; }
		}
		/// <summary>
		/// Specifically gets or sets 32-bit integer value of this parameter without any conversions. Avoid
		/// using it unless this is a parameter of that type.
		/// </summary>
		public int IntSpecific
		{
			get { return this.value.bytes4.SignedInt; }
			set { this.value.bytes4.SignedInt = value; }
		}
		/// <summary>
		/// Specifically gets or sets single-precision floating point value of this parameter without any
		/// conversions. Avoid using it unless this is a parameter of that type.
		/// </summary>
		public float FloatSpecific
		{
			get { return this.value.bytes4.SingleFloat; }
			set { this.value.bytes4.SingleFloat = value; }
		}
		#endregion

		// Text parameters are a little finicky to deal with right now.

		// ///
		// <summary>
		// /// Gets or sets byte value of this parameter. Avoid using it unless this is a parameter of that
		// type. ///
		// </summary>
		// ///
		// <remarks>/// This property creates memory leaks when assigned multiple times. ///</remarks>
		// public string Text { get { return this.value.@byte1; } set { this.value.@byte1 = value; } }

		/// <summary>
		/// Gets or sets vector value of this parameter. Avoid using it unless this is a parameter of that
		/// type.
		/// </summary>
		public Vector3 Vector
		{
			get { return this.value.vector; }
			set { this.value.vector = value; }
		}
		/// <summary>
		/// Gets or sets color value of this parameter. Avoid using it unless this is a parameter of that
		/// type.
		/// </summary>
		public ColorSingle Color
		{
			get { return this.value.color; }
			set { this.value.color = value; }
		}
		/// <summary>
		/// Gets Camera pointer value of this parameter. Avoid using it unless this is a parameter of that
		/// type.
		/// </summary>
		public Camera* Camera
		{
			get { return this.value.camera; }
		}
		/// <summary>
		/// Gets or sets internal type of this shader parameter.
		/// </summary>
		public ShaderParameterType InternalType
		{
			get { return this.currentType; }
			set { this.currentType = value; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}