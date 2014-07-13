using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Serialization
{
	public struct CrySerialize : ICrySerialize
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void BeginGroup(IntPtr handle, string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void EndGroup(IntPtr handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueString(IntPtr handle, string name, ref string obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueInt(IntPtr handle, string name, ref int obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueUInt(IntPtr handle, string name, ref uint obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueBool(IntPtr handle, string name, ref bool obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueEntityId(IntPtr handle, string name, ref uint obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueFloat(IntPtr handle, string name, ref float obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueVec3(IntPtr handle, string name, ref Vector3 obj, string policy);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void ValueQuat(IntPtr handle, string name, ref Quaternion obj, string policy);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void EnumValue(IntPtr handle, string name, ref int obj, int first, int last);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void UnsignedEnumValue(IntPtr handle, string name, ref uint obj, uint first, uint last);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern bool _IsReading(IntPtr handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void FlagPartialRead(IntPtr handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern SerializationTarget GetSerializationTarget(IntPtr handle);

		/// <summary>
		/// Begins a serialization group - must be matched by an <see cref="EndGroup()" /> call.
		/// </summary>
		/// <param name="name">
		/// Preferably as short as possible for performance reasons, cannot contain spaces.
		/// </param>
		public void BeginGroup(string name)
		{
			BeginGroup(Handle, name);
		}

		public void EndGroup()
		{
			EndGroup(Handle);
		}

		public void Value(string name, ref string obj, string policy = null)
		{
			ValueString(Handle, name, ref obj, policy);
		}

		public void Value(string name, ref int obj, string policy = null)
		{
			ValueInt(Handle, name, ref obj, policy);
		}

		[CLSCompliant(false)]
		public void Value(string name, ref uint obj, string policy = null)
		{
			ValueUInt(Handle, name, ref obj, policy);
		}

		public void Value(string name, ref bool obj, string policy = null)
		{
			ValueBool(Handle, name, ref obj, policy);
		}

		public void Value(string name, ref EntityId obj, string policy = null)
		{
			ValueEntityId(Handle, name, ref obj._value, policy);
		}

		public void Value(string name, ref float obj, string policy = null)
		{
			ValueFloat(Handle, name, ref obj, policy);
		}

		public void Value(string name, ref Vector3 obj, string policy = null)
		{
			ValueVec3(Handle, name, ref obj, policy);
		}

		public void Value(string name, ref Quaternion obj, string policy = null)
		{
			ValueQuat(Handle, name, ref obj, policy);
		}

		public void EnumValue(string name, ref int obj, int first, int last)
		{
			EnumValue(Handle, name, ref obj, first, last);
		}

		[CLSCompliant(false)]
		public void EnumValue(string name, ref uint obj, uint first, uint last)
		{
			UnsignedEnumValue(Handle, name, ref obj, first, last);
		}

		/// <summary>
		/// For network updates: Notify the network engine that this value was only partially read
		/// and we should re-request an update from the server soon.
		/// </summary>
		public void FlagPartialRead()
		{
			FlagPartialRead(Handle);
		}

		public bool IsReading { get { return _IsReading(Handle); } }
		public bool IsWriting { get { return !IsReading; } }

		public SerializationTarget Target { get { return GetSerializationTarget(Handle); } }

		internal IntPtr Handle { get; set; }
	}
}