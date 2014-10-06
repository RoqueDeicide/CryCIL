using System;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Logic.Entities;
using CryEngine.Mathematics;

namespace CryEngine.RunTime.Serialization
{
	public struct CrySerialize : ICrySerialize
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void BeginGroup(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EndGroup(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueString(IntPtr handle, string name, ref string obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueInt(IntPtr handle, string name, ref int obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueUInt(IntPtr handle, string name, ref uint obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueBool(IntPtr handle, string name, ref bool obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueEntityId(IntPtr handle, string name, ref uint obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueFloat(IntPtr handle, string name, ref float obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueVec3(IntPtr handle, string name, ref Vector3 obj, string policy);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ValueQuat(IntPtr handle, string name, ref Quaternion obj, string policy);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnumValue(IntPtr handle, string name, ref int obj, int first, int last);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnsignedEnumValue(IntPtr handle, string name, ref uint obj, uint first, uint last);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _IsReading(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FlagPartialRead(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SerializationTarget GetSerializationTarget(IntPtr handle);

		/// <summary>
		/// Begins a serialization group - must be matched by an <see cref="EndGroup()" /> call.
		/// </summary>
		/// <param name="name">
		/// Preferably as short as possible for performance reasons, cannot contain spaces.
		/// </param>
		public void BeginGroup(string name)
		{
			BeginGroup(this.Handle, name);
		}

		public void EndGroup()
		{
			EndGroup(this.Handle);
		}

		public void Value(string name, ref string obj, string policy = null)
		{
			ValueString(this.Handle, name, ref obj, policy);
		}

		public void Value(string name, ref int obj, string policy = null)
		{
			ValueInt(this.Handle, name, ref obj, policy);
		}

		[CLSCompliant(false)]
		public void Value(string name, ref uint obj, string policy = null)
		{
			ValueUInt(this.Handle, name, ref obj, policy);
		}

		public void Value(string name, ref bool obj, string policy = null)
		{
			ValueBool(this.Handle, name, ref obj, policy);
		}

		public void Value(string name, ref EntityId obj, string policy = null)
		{
			ValueEntityId(this.Handle, name, ref obj.Value, policy);
		}

		public void Value(string name, ref float obj, string policy = null)
		{
			ValueFloat(this.Handle, name, ref obj, policy);
		}

		public void Value(string name, ref Vector3 obj, string policy = null)
		{
			ValueVec3(this.Handle, name, ref obj, policy);
		}

		public void Value(string name, ref Quaternion obj, string policy = null)
		{
			ValueQuat(this.Handle, name, ref obj, policy);
		}

		public void EnumValue(string name, ref int obj, int first, int last)
		{
			EnumValue(this.Handle, name, ref obj, first, last);
		}

		[CLSCompliant(false)]
		public void EnumValue(string name, ref uint obj, uint first, uint last)
		{
			UnsignedEnumValue(this.Handle, name, ref obj, first, last);
		}

		/// <summary>
		/// For network updates: Notify the network engine that this value was only partially read
		/// and we should re-request an update from the server soon.
		/// </summary>
		public void FlagPartialRead()
		{
			FlagPartialRead(this.Handle);
		}

		public bool IsReading
		{
			get { return _IsReading(this.Handle); }
		}
		public bool IsWriting
		{
			get { return !this.IsReading; }
		}

		public SerializationTarget Target
		{
			get { return GetSerializationTarget(this.Handle); }
		}

		internal IntPtr Handle { get; set; }
	}
}