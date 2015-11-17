using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Physics;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents an object that manages objects that are attached to the character.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct AttachmentManager
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the collection of attachments.
		/// </summary>
		[FieldOffset(0)] public readonly AttachmentManagerSockets Sockets;
		/// <summary>
		/// Provides access to the collection of attachment proxies.
		/// </summary>
		[FieldOffset(0)] public readonly AttachmentManagerProxies Proxies;
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
		/// Gets the character this object manages attachments and attachment proxies for.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Character Character
		{
			get
			{
				this.AssertInstance();

				return GetSkelInstance(this.handle);
			}
		}
		#endregion
		#region Construction
		internal AttachmentManager(IntPtr handle)
			: this()
		{
			this.handle = handle;
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
		internal static extern uint LoadAttachmentList(IntPtr handle, string pathname);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentSocket CreateAttachment(IntPtr handle, string szName, AttachmentTypes type,
																 string szJointName, bool bCallProject);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveAttachmentByInterface(IntPtr handle, AttachmentSocket ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveAttachmentByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveAttachmentByNameCrc(IntPtr handle, LowerCaseCrc32 nameCrc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentSocket GetInterfaceByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentSocket GetInterfaceByIndex(IntPtr handle, uint c);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentSocket GetInterfaceByNameCrc(IntPtr handle, LowerCaseCrc32 nameCrc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetAttachmentCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetIndexByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetIndexByNameCrc(IntPtr handle, LowerCaseCrc32 nameCrc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint ProjectAllAttachment(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void PhysicalizeAttachment(IntPtr handle, int idx, PhysicalEntity pent, int nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DephysicalizeAttachment(IntPtr handle, int idx, PhysicalEntity pent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Character GetSkelInstance(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetProxyCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentProxy CreateProxy(IntPtr handle, string szName, string szJointName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentProxy GetProxyInterfaceByIndex(IntPtr handle, uint c);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentProxy GetProxyInterfaceByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern AttachmentProxy GetProxyInterfaceByCrc(IntPtr handle, LowerCaseCrc32 nameCrc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetProxyIndexByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveProxyByInterface(IntPtr handle, AttachmentProxy ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveProxyByName(IntPtr handle, string szName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int RemoveProxyByNameCrc(IntPtr handle, LowerCaseCrc32 nameCrc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DrawProxies(IntPtr handle, uint enable);
		#endregion
	}
}