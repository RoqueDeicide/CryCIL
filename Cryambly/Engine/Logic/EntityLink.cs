using System;
using System.Diagnostics.Contracts;
using CryCil.Annotations;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents an entity link.
	/// </summary>
	public struct EntityLink
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
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
		/// Gets next link in the chain or invalid <see cref="EntityLink"/> object if this one is last.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance object is not usable.</exception>
		public EntityLink Next
		{
			get
			{
				this.AssertLink();
				Contract.EndContractBlock();

				return CryEntity.GetNextLink(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this link is last in the chain.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance object is not usable.</exception>
		public bool IsLast
		{
			get { return !this.Next.IsValid; }
		}
		/// <summary>
		/// Gets identifier of the linked entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance object is not usable.</exception>
		public EntityId LinkedEntityId
		{
			get
			{
				this.AssertLink();
				Contract.EndContractBlock();

				return CryEntity.GetLinkedEntityId(this.handle);
			}
		}
		/// <summary>
		/// Gets globally unique identifier of the linked entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance object is not usable.</exception>
		public EntityGUID LinkedEntityGuid
		{
			get
			{
				this.AssertLink();
				Contract.EndContractBlock();

				return CryEntity.GetLinkedEntityGuid(this.handle);
			}
		}
		/// <summary>
		/// Gets the name of this link.
		/// </summary>
		/// <remarks>
		/// Be economical with calling this property as each call creates a string and invokes an internal
		/// call.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance object is not usable.</exception>
		public string Name
		{
			get
			{
				this.AssertLink();
				Contract.EndContractBlock();

				return CryEntity.GetLinkName(this.handle);
			}
		}
		#endregion
		#region Construction
		internal EntityLink(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Utilities
		// Assertion method.
		private void AssertLink()
		{
			if (this.handle == IntPtr.Zero)
			{
				throw new NullReferenceException("This instance object is not usable.");
			}
		}
		#endregion
	}
}