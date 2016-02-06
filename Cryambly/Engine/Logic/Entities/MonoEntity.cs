using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.RunTime;
using CryCil.RunTime.Registration;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Reloading"/> event.
	/// </summary>
	/// <param name="entity">    Entity that is being reloaded.</param>
	/// <param name="parameters">
	/// A reference to the object that contains new spawning parameters of the entity.
	/// </param>
	/// <returns>True, if reloading was successful.</returns>
	public delegate bool EntityReloadEventHandler(MonoEntity entity, ref EntitySpawnParameters parameters);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="MonoEntity.Reloaded"/> event.
	/// </summary>
	/// <param name="entity">    Entity that has been reloaded.</param>
	/// <param name="parameters">
	/// A reference to the object that contains new spawning parameters of the entity.
	/// </param>
	public delegate void EntityPostReloadEventHandler(MonoEntity entity, ref EntitySpawnParameters parameters);
	/// <summary>
	/// Base class for CryEngine entities with custom logic that is defined in CryCIL.
	/// </summary>
	public abstract partial class MonoEntity : IDisposable
	{
		#region Fields
		private readonly List<EntityReloadEventHandler> reloadingEventHandlers;
		/// <summary>
		/// The name of the IEntityClass that is used by this entity.
		/// </summary>
		protected string EntityTypeName;
		private readonly EntityId id;
		private readonly CryEntity entity;
		/// <summary>
		/// Provides access to the collection of objects that extend functionality of this entity.
		/// </summary>
		public EntityExtensions Extensions;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of this entity.
		/// </summary>
		public EntityId Id
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!this.Disposed, "This entity doesn't exist.");
				return this.id;
			}
		}
		/// <summary>
		/// Gets the underlying CryEngine entity object.
		/// </summary>
		public CryEntity Entity
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!this.Disposed, "This entity doesn't exist.");
				return this.entity;
			}
		}
		/// <summary>
		/// Indicates whether this entity has been disposed of.
		/// </summary>
		/// <remarks>
		/// The entity becomes disposed after a call to
		/// <see cref="M:CryCil.Logic.MonoEntity.Dispose(bool)"/> and a call to
		/// <see cref="EntitySystem.RemoveEntity"/>.
		/// </remarks>
		public bool Disposed { get; private set; }
		/// <summary>
		/// Gets the name of entity class that represents this entity.
		/// </summary>
		/// <exception cref="TypeLoadException">The custom attribute type cannot be loaded.</exception>
		public virtual string EntityClassName
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!this.Disposed, "This entity doesn't exist.");
				return this.EntityTypeName ??
					   (this.EntityTypeName = this.GetType().GetAttribute<EntityAttribute>().Name);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this entity receives post updates.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only entities for which this proeprty is <c>true</c> can have their <see cref="PostUpdate"/>
		/// method invoked.
		/// </para>
		/// <para>By default entities do not receive post updates.</para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">This entity doesn't exist.</exception>
		public bool PostUpdatesEnabled
		{
			set
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("this", "This entity doesn't exist.");
				}

				EnablePostUpdates(this.entity, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity receives updates.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only entities for which this proeprty is <c>true</c> can have their <see cref="Update"/> method
		/// invoked.
		/// </para>
		/// <para>By default entities do not receive updates unless they were spawned as active.</para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">This entity doesn't exist.</exception>
		public bool UpdatesEnabled
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("this", "This entity doesn't exist.");
				}

				return AreUpdatesEnabled(this.entity);
			}
			set
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("this", "This entity doesn't exist.");
				}

				EnableUpdates(this.entity, value);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity receives updates prior to update of
		/// the physical world.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only entities for which this proeprty is <c>true</c> can have their
		/// <see cref="MonoEntity.PrePhysicsUpdate"/> method invoked.
		/// </para>
		/// <para>By default entities do not receive pre-physics updates.</para>
		/// </remarks>
		/// <exception cref="ObjectDisposedException">This entity doesn't exist.</exception>
		public bool PrePhysicsUpdatesEnabled
		{
			get
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("this", "This entity doesn't exist.");
				}

				return ArePrePhysicsUpdatesEnabled(this.entity);
			}
			set
			{
				if (this.Disposed)
				{
					throw new ObjectDisposedException("this", "This entity doesn't exist.");
				}

				EnablePrePhysics(this.entity, value);
			}
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the entity is about to be initialized.
		/// </summary>
		public event EventHandler Initializing;
		/// <summary>
		/// Occurs when the entity has been initialized.
		/// </summary>
		public event EventHandler Initialized;
		/// <summary>
		/// Occurs when the entity is reloading.
		/// </summary>
		/// <remarks>Reloading only happens when the entity is returned to the entity pool.</remarks>
		public event EntityReloadEventHandler Reloading
		{
			add { this.reloadingEventHandlers.Add(value); }
			remove { this.reloadingEventHandlers.Remove(value); }
		}
		/// <summary>
		/// Occurs when the entity was successfully reloaded.
		/// </summary>
		/// <remarks>Reloading only happens when the entity is returned to the entity pool.</remarks>
		public event EntityPostReloadEventHandler Reloaded;
		#endregion
		#region Construction
		/// <summary>
		/// Initializes base properties of all objects that serve as abstraction layers between CryEngine
		/// entities and logic defined in CryCIL for them.
		/// </summary>
		/// <param name="handle">Pointer to the entity itself.</param>
		/// <param name="id">    Identifier of the entity.</param>
		protected MonoEntity(CryEntity handle, EntityId id)
		{
			this.entity = handle;
			this.id = id;
			this.reloadingEventHandlers = new List<EntityReloadEventHandler>();
			this.EntityTypeName = null;
			this.Extensions = new EntityExtensions(this);
		}
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, releases this entity.
		/// </summary>
		public void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			this.Extensions.Clear(true);
			this.Dispose(false);
			// Remove the entity from the world: since this layer is going down the entity has no reason to
			// stay up.
			EntitySystem.RemoveEntity(this.Id, true);

			this.Disposed = true;
		}
		/// <summary>
		/// When implemented in derived class, releases resources held by this entity.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether this entity was released from native code.
		/// </param>
		public abstract void Dispose(bool invokedFromNativeCode);
		/// <summary>
		/// When implemented in derived class, performs preliminary initialization of this object.
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// When implemented in derived class, performs final initialization of this object.
		/// </summary>
		public abstract void PostInitialize();
		/// <summary>
		/// Can be overridden in derived class to define custom logic that will be executed when reloading
		/// this entity.
		/// </summary>
		/// <remarks>Reloading only happens when the entity is returned to the entity pool.</remarks>
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
		/// successfully reloading this entity.
		/// </summary>
		/// <remarks>Reloading only happens when the entity is returned to the entity pool.</remarks>
		public virtual void PostReload()
		{
		}
		/// <summary>
		/// Can be overridden in derived class to define the entity's pool signature.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Signature is formed by beginning and ending groups without syncing any actual data.
		/// </para>
		/// <para>Generally there is no reason to return <c>false</c>.</para>
		/// </remarks>
		/// <example>
		/// <code>
		/// public override bool GetEntityPoolSignature(CrySync signature)
		/// {
		///     signature.BeginGroup("SomeEntity");
		///     signature.EndGroup();
		///     return true;
		/// }
		/// </code>
		/// </example>
		/// <param name="signature">Object that defines the signature.</param>
		/// <returns>True, if signature is valid.</returns>
		public virtual bool GetEntityPoolSignature(CrySync signature)
		{
			return true;
		}
		/// <summary>
		/// Synchronizes the state of this entity with its representation in other place (e.g. a save game
		/// file) .
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public abstract void Synchronize(CrySync sync);
		/// <summary>
		/// When implemented in derived class updates logical state of this entity.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public abstract void Update(ref EntityUpdateContext context);
		/// <summary>
		/// When implemented in derived class updates logical state of this entity after most other stuff
		/// is updated.
		/// </summary>
		public abstract void PostUpdate();
		#endregion
		#region Utilities
		[RawThunk("Updates this object.")]
		private void UpdateInternal(ref EntityUpdateContext context)
		{
			try
			{
				this.Update(ref context);

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].Update(ref context);
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Post-updates this object.")]
		private void PostUpdateInternal()
		{
			try
			{
				this.PostUpdate();

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].PostUpdate();
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Releases this object when the entity is removed by CryEngine.")]
		private void DisposeInternal()
		{
			try
			{
				this.Dispose(true);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Creates a managed object that serves as abstraction layer between CryEngine entity and CryCIL.")]
		private static MonoEntity CreateAbstractionLayer(string className, EntityId identifier, CryEntity handle)
		{
			try
			{
				Type entityType;
				if (EntityRegistry.DefinedEntityClasses.TryGetValue(className, out entityType))
				{
					try
					{
						return (MonoEntity)Activator.CreateInstance(entityType, handle, identifier);
					}
					catch (Exception)
					{
						return null;
					}
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
			return null;
		}
		[RawThunk("Invoked from underlying object to raise the event Initializing.")]
		private void OnInitializing()
		{
			try
			{
				this.Initialize();

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].Initialize();
				}

				if (this.Initializing != null) this.Initializing(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying object to raise the event Initialized.")]
		private void OnInitialized()
		{
			try
			{
				this.PostInitialize();

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].PostInitialize();
				}

				if (this.Initialized != null) this.Initialized(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying object to reload the entity.")]
		private bool OnReloading(ref EntitySpawnParameters parameters)
		{
			try
			{
				if (this.Reload(ref parameters))
				{
					for (int i = 0; i < this.Extensions.Count; i++)
					{
						if (!this.Extensions[i].Reload(ref parameters))
						{
							return false;
						}
					}
					for (int i = 0; i < this.reloadingEventHandlers.Count; i++)
					{
						if (!this.reloadingEventHandlers[i](this, ref parameters))
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
		[RawThunk("Invoked from underlying object after reloading the entity.")]
		private void OnReloaded(ref EntitySpawnParameters parameters)
		{
			try
			{
				this.PostReload();

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].PostReload();
				}

				if (this.Reloaded != null) this.Reloaded(this, ref parameters);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying object to acquire entity's pool signature.")]
		private bool GetSignature(CrySync signature)
		{
			try
			{
				return this.GetEntityPoolSignature(signature);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
			return false;
		}
		[RawThunk("Invoked from underlying object to invoke Synchronize method.")]
		private void SyncInternal(CrySync sync)
		{
			try
			{
				this.Synchronize(sync);

				for (int i = 0; i < this.Extensions.Count; i++)
				{
					this.Extensions[i].Synchronize(sync);
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked to set one of the properties.")]
		private void SetEditableProperty(int index, string value)
		{
			try
			{
				EntityRegistry.DefinedProperties[this.EntityClassName][index].Set(this, value);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked to set one of the properties.")]
		private string GetEditableProperty(int index)
		{
			try
			{
				return EntityRegistry.DefinedProperties[this.EntityClassName][index].Get(this);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
			return null;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnablePostUpdates(CryEntity entity, bool receive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableUpdates(CryEntity entity, bool receive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool AreUpdatesEnabled(CryEntity entity);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnablePrePhysics(CryEntity entity, bool receive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ArePrePhysicsUpdatesEnabled(CryEntity entity);
		#endregion
	}
}