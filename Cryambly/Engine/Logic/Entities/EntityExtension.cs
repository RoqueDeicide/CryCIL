using System;
using System.Linq;
using CryCil.Engine.Data;
using CryCil.Engine.Network;
using CryCil.Engine.Physics;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for objects that can be used to extend functionality of managed entities.
	/// </summary>
	public abstract class EntityExtension
	{
		#region Fields
		#endregion
		#region Properties
		/// <summary>
		/// Gets the hosting entity.
		/// </summary>
		public MonoEntity Host { get; internal set; }
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, releases resources held by this extension.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether a hosting entity was released from native code.
		/// </param>
		public abstract void Dispose(bool invokedFromNativeCode);
		/// <summary>
		/// When implemented in derived class, performs preliminary initialization of this extension.
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// When implemented in derived class, performs final initialization of this extension.
		/// </summary>
		public abstract void PostInitialize();
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
		/// Synchronizes the state of this extension with its representation in other place (e.g. a save
		/// game
		/// file) .
		/// </summary>
		/// <remarks>
		/// All extensions are synchronized after the hosting entity in the order they were added to the
		/// entity.
		/// </remarks>
		/// <param name="sync">Object that handles synchronization.</param>
		public abstract void Synchronize(CrySync sync);
		/// <summary>
		/// When implemented in derived class updates logical state of this extension.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public abstract void Update(ref EntityUpdateContext context);
		/// <summary>
		/// When implemented in derived class updates logical state of this extension after most other
		/// stuff is updated.
		/// </summary>
		public abstract void PostUpdate();
		/// <summary>
		/// Can be overridden in derived class to define custom logic that will be executed when reloading
		/// this extension.
		/// </summary>
		/// <remarks>Reloading only happens when the host entity is returned to the entity pool.</remarks>
		/// <param name="parameters">
		/// Reference to the object that contains the parameters that were used to respawn this entity.
		/// </param>
		/// <returns>True, if reloading was a success.</returns>
		public virtual bool Reload(ref EntitySpawnParameters parameters)
		{
			return true;
		}
		/// <summary>
		/// Can be overridden in derived class to define custom logic that will be executed after
		/// successfully reloading this extension.
		/// </summary>
		/// <remarks>Reloading only happens when the host entity is returned to the entity pool.</remarks>
		public virtual void PostReload()
		{
		}
		/// <summary>
		/// Can be overridden in derived class to synchronize the state of this extension with its
		/// representatives on other machines over network.
		/// </summary>
		/// <remarks>
		/// All extensions are synchronized after the hosting entity in the order they were added to the
		/// entity.
		/// </remarks>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates the aspect that requires synchronization.</param>
		/// <param name="profile">
		/// A number in range [0; 7] that specifies the data format that has to be used to synchronize the
		/// aspect data.
		/// </param>
		/// <param name="flags">  A set of flags that specify how to write the snapshot.</param>
		/// <returns>True, if synchronization was successful.</returns>
		public virtual bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile,
												   SnapshotFlags flags)
		{
			return true;
		}
		/// <summary>
		/// When overridden in derived class, performs operations that must be done when this extension is
		/// removed from the entity.
		/// </summary>
		/// <remarks>
		/// <see cref="EntityExtension.Host"/> property still returns the hosting entity during this
		/// method.
		/// </remarks>
		/// <param name="disposing">
		/// Indicates whether release was caused by the entity getting disposed of.
		/// </param>
		public abstract void Release(bool disposing);
		/// <summary>
		/// When overridden in derived class, performs operations that must be done when this extension is
		/// added to the entity.
		/// </summary>
		/// <remarks>
		/// <see cref="EntityExtension.Host"/> property already returns the hosting entity during this
		/// method.
		/// </remarks>
		public abstract void Bind();
		/// <summary>
		/// Can be overridden in derived class to allow the extension to request changes to the entity's
		/// positioning in physical world, before physics engine updates it.
		/// </summary>
		/// <param name="frameTime">Length of the last frame.</param>
		public virtual void PrePhysicsUpdate(TimeSpan frameTime)
		{
		}
		#endregion
	}
}