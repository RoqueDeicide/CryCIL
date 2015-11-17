using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a simple object that is used to simulate physical interactions of objects that are attached to animated characters with characters themselves.
	/// </summary>
	public struct AttachmentProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets the name of this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
		}
		/// <summary>
		/// Gets CRC32 hash code of the lower-case version of the name of this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LowerCaseCrc32 NameHashCode
		{
			get
			{
				this.AssertInstance();

				return GetNameCrc(this.handle);
			}
		}
		/// <summary>
		/// Gets identifier of the joint this object is attached to.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint JointId
		{
			get
			{
				this.AssertInstance();

				return GetJointId(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets location of this proxy in default skeleton pose in model-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec DefaultAbsoluteLocation
		{
			get
			{
				this.AssertInstance();

				return GetProxyAbsoluteDefault(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProxyAbsoluteDefault(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets location of this proxy in default skeleton pose in parent joint or bone-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec DefaultRelativeLocation
		{
			get
			{
				this.AssertInstance();

				return GetProxyRelativeDefault(this.handle);
			}
		}
		/// <summary>
		/// Gets the current location of this proxy in animated skeleton pose in model-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec ModelLocation
		{
			get
			{
				this.AssertInstance();

				return GetProxyModelRelative(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the vector that contains 4 numbers that define the shape of the proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Vector4 Parameters
		{
			get
			{
				this.AssertInstance();

				return GetProxyParams(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProxyParams(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the purpose of this proxy.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public sbyte Purpose
		{
			get
			{
				this.AssertInstance();

				return GetProxyPurpose(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetProxyPurpose(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this proxy is hidden.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Hidden
		{
			set
			{
				this.AssertInstance();
				
				SetHideProxy(this.handle, (byte)(value ? 1 : 0));
			}
		}
		#endregion
		#region Construction
		internal AttachmentProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Changes the name of the joint this proxy is based on.
		/// </summary>
		/// <param name="name">New name of the joint.</param>
		/// <returns>Hash code of the new name(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LowerCaseCrc32 UpdateJointName([CanBeNull]string name)
		{
			this.AssertInstance();

			return SetJointName(this.handle, name);
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint ProjectProxy()
		{
			this.AssertInstance();

			return ProjectProxyInternal(this.handle);
		}
		/// <summary>
		/// Aligns position of this proxy with a joint it's attached to.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AlignWithJoint()
		{
			this.AssertInstance();

			AlignProxyWithJoint(this.handle);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LowerCaseCrc32 GetNameCrc(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint ReName(IntPtr handle, string szSocketName, uint crc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LowerCaseCrc32 SetJointName(IntPtr handle, string szJointName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetJointId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetProxyAbsoluteDefault(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetProxyRelativeDefault(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quatvec GetProxyModelRelative(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProxyAbsoluteDefault(IntPtr handle, ref Quatvec rot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint ProjectProxyInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AlignProxyWithJoint(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector4 GetProxyParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProxyParams(IntPtr handle, ref Vector4 parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern sbyte GetProxyPurpose(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetProxyPurpose(IntPtr handle, sbyte p);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetHideProxy(IntPtr handle, byte nHideProxy);
		#endregion
	}
}