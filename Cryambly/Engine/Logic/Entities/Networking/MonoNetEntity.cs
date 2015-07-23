using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Engine.Network;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for all objects that define logic for CryEngine entities that are bound to and
	/// synchronized via network.
	/// </summary>
	public abstract partial class MonoNetEntity : MonoEntity
	{
		#region Fields
		[UsedImplicitly]
		private ChannelId channelId;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets identifier of the channel this entity uses to interact with other entities in the
		/// network.
		/// </summary>
		public ChannelId ChannelId
		{
			get { return channelId; }
			set
			{
				SetChannelId(this.Id, value);
				// The channerlId field will be set by underlying framework.
			}
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the entity is informed that a mirroring entity is about to be initialized on the
		/// client-side.
		/// </summary>
		/// <remarks>
		/// First parameter is an entity object for which the event was raised and second parameter is
		/// identifier of channel that can be used to communicate with the client.
		/// </remarks>
		public event Action<object, ChannelId> ClientInitializing;
		/// <summary>
		/// Occurs when the entity is informed that a mirroring entity was initialized on the client-side.
		/// </summary>
		/// <remarks>
		/// First parameter is an entity object for which the event was raised and second parameter is
		/// identifier of channel that can be used to communicate with the client.
		/// </remarks>
		public event Action<object, ChannelId> ClientInitialized;
		#endregion
		#region Construction
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		protected MonoNetEntity(CryEntity handle, EntityId id)
			: base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Reacts to initialization of the client.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public abstract void InitializeClient(ChannelId id);
		/// <summary>
		/// Reacts to end of initialization of the client.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public abstract void PostInitializeClient(ChannelId id);
		/// <summary>
		/// Synchronizes the state of this entity with its representatives on other machines over network.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates synchronized aspect.</param>
		/// <param name="profile">Unknown.</param>
		/// <param name="pflags"> Physics flags(?).</param>
		/// <returns>True, if synchronization was successful.</returns>
		public abstract bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile, int pflags);
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked from underlying object to raise the event ClientInitializing.")]
		private void OnClientInitializing(ChannelId id)
		{
			this.InitializeClient(id);
			if (this.ClientInitializing != null) this.ClientInitializing(this, id);
		}
		[UnmanagedThunk("Invoked from underlying object to raise the event ClientInitialized.")]
		private void OnClientInitialized(ChannelId id)
		{
			this.PostInitializeClient(id);
			if (this.ClientInitialized != null) this.ClientInitialized(this, id);
		}
		[UnmanagedThunk("Invoked from underlying object to invoke SynchronizeWithNetwork method.")]
		private bool NetSyncInternal(CrySync sync, EntityAspects aspect, byte profile, int pflags)
		{
			return this.SynchronizeWithNetwork(sync, aspect, profile, pflags);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetChannelId(EntityId entityId, ChannelId channelId);
		#endregion
	}
}