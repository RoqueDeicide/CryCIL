using System;
using System.Linq;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a collection of attachment proxies.
	/// </summary>
	public struct AttachmentManagerProxies
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the proxy in this collection.
		/// </summary>
		/// <param name="name">Name of the proxy to get.</param>
		/// <returns>A valid <see cref="AttachmentProxy"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentProxy this[string name]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetProxyInterfaceByName(this.handle, name);
			}
		}
		/// <summary>
		/// Gets the proxy in this collection.
		/// </summary>
		/// <param name="hash">
		/// CRC32 hash code of a lower-case version of the name of the proxy to get.
		/// </param>
		/// <returns>A valid <see cref="AttachmentProxy"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentProxy this[LowerCaseCrc32 hash]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetProxyInterfaceByCrc(this.handle, hash);
			}
		}
		/// <summary>
		/// Gets the proxy in this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the proxy to get.</param>
		/// <returns>A valid <see cref="AttachmentProxy"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentProxy this[uint index]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetProxyInterfaceByIndex(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the number of proxies in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetProxyCount(this.handle);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether proxies in this collection must be used.
		/// </summary>
		/// <remarks>Can be set to <c>true</c> for debug purposes.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Draw
		{
			set
			{
				this.AssertInstance();

				AttachmentManager.DrawProxies(this.handle, value ? 1u : 0);
			}
		}
		#endregion
		#region Construction
		internal AttachmentManagerProxies(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates an attachment proxy and adds it to this collection.
		/// </summary>
		/// <param name="name"> Name of the proxy to create.</param>
		/// <param name="joint">Name of the joint to attach the proxy to.</param>
		/// <returns>A valid <see cref="AttachmentProxy"/> object, if it was created successfully.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentProxy Create(string name, string joint)
		{
			this.AssertInstance();

			return AttachmentManager.CreateProxy(this.handle, name, joint);
		}
		/// <summary>
		/// Tries to find an attachment proxy in this collection.
		/// </summary>
		/// <param name="name">Name of the proxy to find.</param>
		/// <returns>
		/// Zero-based index of the proxy, if it was found, otherwise returns a negative value(?).
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(string name)
		{
			this.AssertInstance();

			return AttachmentManager.GetProxyIndexByName(this.handle, name);
		}
		/// <summary>
		/// Removes the attachment proxy from this collection.
		/// </summary>
		/// <param name="attachmentProxy">AttachmentSocket proxy to remove.</param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(AttachmentProxy attachmentProxy)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveProxyByInterface(this.handle, attachmentProxy);
		}
		/// <summary>
		/// Removes the attachment proxy from this collection.
		/// </summary>
		/// <param name="name">Name of the attachment proxy to remove.</param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(string name)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveProxyByName(this.handle, name);
		}
		/// <summary>
		/// Removes the attachment proxy from this collection.
		/// </summary>
		/// <param name="hash">
		/// CRC32 hash code of a lower-case version of the name of the attachment proxy to remove.
		/// </param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(LowerCaseCrc32 hash)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveProxyByNameCrc(this.handle, hash);
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
		#endregion
	}
}