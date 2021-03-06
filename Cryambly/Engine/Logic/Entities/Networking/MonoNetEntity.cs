﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Engine.Network;
using CryCil.Engine.Physics;
using CryCil.RunTime;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Defines signature of methods that can handle events related to initialization of entity clients.
	/// </summary>
	/// <param name="sender">       An entity that raised the event.</param>
	/// <param name="clientChannel">
	/// Identifier of the network channel that can be used to communicate with the client object.
	/// </param>
	public delegate void NetEntityClientEventHandler(MonoNetEntity sender, ChannelId clientChannel);
	/// <summary>
	/// Defines signature of methods that can handle events related to entities gaining/losing authority
	/// over their representation across the network.
	/// </summary>
	/// <param name="sender">         An entity that raised the event.</param>
	/// <param name="gainedAuthority">Indicates whether authority was gained or lost.</param>
	public delegate void NetEntityAuthorizationEventHandler(MonoNetEntity sender, bool gainedAuthority);
	/// <summary>
	/// Base class for all objects that define logic for CryEngine entities that are bound to and
	/// synchronized via network.
	/// </summary>
	public abstract partial class MonoNetEntity : MonoEntity
	{
		#region Fields
		[UsedImplicitly] private ChannelId channelId;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets identifier of the channel this entity uses to interact with other entities in the
		/// network.
		/// </summary>
		public ChannelId ChannelId
		{
			get { return this.channelId; }
			set
			{
				SetChannelId(this.Id, value);
				// The channerlId field will be set by underlying framework.
			}
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when this entity is informed that a mirroring entity is about to be initialized on the
		/// client-side.
		/// </summary>
		public event NetEntityClientEventHandler ClientInitializing;
		/// <summary>
		/// Occurs when this entity is informed that a mirroring entity was initialized on the client-side.
		/// </summary>
		public event NetEntityClientEventHandler ClientInitialized;
		/// <summary>
		/// Occurs when this entity gains authority over representation of itself across the network.
		/// </summary>
		public event NetEntityAuthorizationEventHandler Authorized;
		/// <summary>
		/// Occurs when this entity loses authority over representation of itself across the network.
		/// </summary>
		public event NetEntityAuthorizationEventHandler Deauthorized;
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
		/// Can be overridden to implement custom logic for when the client entity is about to be
		/// initialized.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public virtual void InitializeClient(ChannelId id)
		{
		}
		/// <summary>
		/// Can be overridden to implement custom logic for when the client entity is initialized.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public virtual void PostInitializeClient(ChannelId id)
		{
		}
		/// <summary>
		/// Synchronizes the state of this entity with its representatives on other machines over network.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates the aspect that requires synchronization.</param>
		/// <param name="profile">
		/// A number in range [0; 7] that specifies the data format that has to be used to synchronize the
		/// aspect data.
		/// </param>
		/// <param name="flags">  A set of flags that specify how to write the snapshot.</param>
		/// <returns>True, if synchronization was successful.</returns>
		public abstract bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile,
													SnapshotFlags flags);
		/// <summary>
		/// Informs the networking system that parts of this entity have changed and it must be synchronized
		/// with its proxies on other game instances.
		/// </summary>
		/// <param name="aspects">
		/// A set of flags that indicate which aspects of the entity need to be synchronized.
		/// </param>
		public void ChangeNetworkState(EntityAspects aspects)
		{
			ChangeNetworkStateInternal(this.Id, aspects);
		}
		/// <summary>
		/// Can be overridden in derived class to react to this entity gaining/losing authority over
		/// representation of this entity across the network.
		/// </summary>
		/// <param name="authorityGranted">Indicates whether authority was granted.</param>
		public virtual void ChangeAuthority(bool authorityGranted)
		{
		}
		#endregion
		#region Utilities
		[RawThunk("Invoked from underlying object to raise the event ClientInitializing.")]
		private void OnClientInitializing(ChannelId id)
		{
			try
			{
				this.InitializeClient(id);

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].InitializeClient(id);
				}

				this.ClientInitializing?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying object to raise the event ClientInitialized.")]
		private void OnClientInitialized(ChannelId id)
		{
			try
			{
				this.PostInitializeClient(id);

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].PostInitializeClient(id);
				}

				this.ClientInitialized?.Invoke(this, id);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying object to invoke SynchronizeWithNetwork method.")]
		private bool NetSyncInternal(CrySync sync, EntityAspects aspect, byte profile, SnapshotFlags flags)
		{
			try
			{
				if (this.SynchronizeWithNetwork(sync, aspect, profile, flags))
				{
					for (int i = 0; i < this.Extensions.Count; i++)
					{
						if (!this.Extensions[i].SynchronizeWithNetwork(sync, aspect, profile, flags))
						{
							return false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
			return true;
		}
		[RawThunk("Invoked from underlying object to raise either Authorized or Deauthorized events.")]
		private void OnAuthorized(bool gainedAuthority)
		{
			try
			{
				var handler = gainedAuthority ? this.Authorized : this.Deauthorized;
				handler?.Invoke(this, gainedAuthority);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetChannelId(EntityId entityId, ChannelId channelId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ChangeNetworkStateInternal(EntityId id, EntityAspects aspects);
		#endregion
	}
}