using System;
using CryCil.Annotations;
using CryCil.Engine.Physics;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a collection of attachments.
	/// </summary>
	public struct AttachmentManagerSockets
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
		/// Gets the socket in this collection.
		/// </summary>
		/// <param name="name">Name of the socket to get.</param>
		/// <returns>A valid <see cref="AttachmentSocket"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSocket this[string name]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetInterfaceByName(this.handle, name);
			}
		}
		/// <summary>
		/// Gets the socket in this collection.
		/// </summary>
		/// <param name="hash">
		/// CRC32 hash code of a lower-case version of the name of the socket to get.
		/// </param>
		/// <returns>A valid <see cref="AttachmentSocket"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSocket this[LowerCaseCrc32 hash]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetInterfaceByNameCrc(this.handle, hash);
			}
		}
		/// <summary>
		/// Gets the socket in this collection.
		/// </summary>
		/// <param name="index">Zero-based index of the socket to get.</param>
		/// <returns>A valid <see cref="AttachmentSocket"/> object, if it was found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSocket this[uint index]
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetInterfaceByIndex(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the number of attachments in this collection.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return AttachmentManager.GetAttachmentCount(this.handle);
			}
		}
		#endregion
		#region Construction
		internal AttachmentManagerSockets(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Loads information about a list of attachments from the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>Number of loaded attachments(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint Load([PathReference] string file)
		{
			this.AssertInstance();

			return
				file.IsNullOrWhiteSpace()
					? 0
					: AttachmentManager.LoadAttachmentList(this.handle, file);
		}
		/// <summary>
		/// Creates a brand new socket and adds it to this collection.
		/// </summary>
		/// <param name="name">   Name of the socket to create.</param>
		/// <param name="type">   Type of the socket to create.</param>
		/// <param name="joint">  
		/// Optional name of the joint to use as a base for the socket, if it connects to the joint.
		/// </param>
		/// <param name="project">
		/// Optional value that indicates whether thee socket needs to be projected(?) in order to be
		/// placed on faces.
		/// </param>
		/// <returns>A valid <see cref="AttachmentSocket"/> object, if successful.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSocket Create(string name, AttachmentTypes type, string joint = null, bool project = true)
		{
			this.AssertInstance();

			return AttachmentManager.CreateAttachment(this.handle, name, type, joint, project);
		}
		/// <summary>
		/// Removes the socket from this collection.
		/// </summary>
		/// <param name="attachmentSocket">Socket to remove.</param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(AttachmentSocket attachmentSocket)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveAttachmentByInterface(this.handle, attachmentSocket);
		}
		/// <summary>
		/// Removes the socket from this collection.
		/// </summary>
		/// <param name="name">Name of the socket to remove.</param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(string name)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveAttachmentByName(this.handle, name);
		}
		/// <summary>
		/// Removes the socket from this collection.
		/// </summary>
		/// <param name="hash">
		/// CRC32 hash code of a lower-case version of the name of the socket to remove.
		/// </param>
		/// <returns>Unknown.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Remove(LowerCaseCrc32 hash)
		{
			this.AssertInstance();

			return AttachmentManager.RemoveAttachmentByNameCrc(this.handle, hash);
		}
		/// <summary>
		/// Looks through this collection to find an socket.
		/// </summary>
		/// <param name="name">Name of the socket to find.</param>
		/// <returns>
		/// Zero-based index of the socket, if it was found. Otherwise a negative value will be returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(string name)
		{
			this.AssertInstance();

			return AttachmentManager.GetIndexByName(this.handle, name);
		}
		/// <summary>
		/// Looks through this collection to find an socket.
		/// </summary>
		/// <param name="hash">
		/// CRC32 hash code of a lower-case version of the name of the socket to find.
		/// </param>
		/// <returns>
		/// Zero-based index of the socket, if it was found. Otherwise a negative value will be returned.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(LowerCaseCrc32 hash)
		{
			this.AssertInstance();

			return AttachmentManager.GetIndexByNameCrc(this.handle, hash);
		}
		/// <summary>
		/// Projects all face attachments in this collection.
		/// </summary>
		/// <returns>Number of projected attachments(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint ProjectAll()
		{
			this.AssertInstance();

			return AttachmentManager.ProjectAllAttachment(this.handle);
		}
		/// <summary>
		/// Physicalizes socket(s) in this collection.
		/// </summary>
		/// <param name="index"> 
		/// Zero-based index of the socket to physicalize. If equal to <c>-1</c> then all attachments will
		/// physicalized(?).
		/// </param>
		/// <param name="entity">
		/// Optional object that represents a physical entity that shall host physical representations of
		/// attachments.
		/// </param>
		/// <param name="lod">   Zero-based index of the LOD model of the socket(s) to physicalize.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Physicalize(int index, PhysicalEntity entity = new PhysicalEntity(), int lod = 0)
		{
			this.AssertInstance();

			AttachmentManager.PhysicalizeAttachment(this.handle, index, entity, lod);
		}
		/// <summary>
		/// Removes physical representation of the socket(s).
		/// </summary>
		/// <param name="index"> 
		/// Zero-based index of the socket to dephysicalize. If equal to <c>-1</c> then all attachments
		/// will dephysicalized(?).
		/// </param>
		/// <param name="entity">
		/// Optional object that represents a physical entity that was used as a host for physical
		/// representation(s) of socket(s) when calling <see cref="Physicalize"/>.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Dephysicalize(int index, PhysicalEntity entity = new PhysicalEntity())
		{
			this.AssertInstance();

			AttachmentManager.DephysicalizeAttachment(this.handle, index, entity);
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