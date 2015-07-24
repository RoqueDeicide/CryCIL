using System;
using System.Collections.Generic;
using CryCil.Engine.Data;
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
	public abstract class MonoEntity : IDisposable
	{
		#region Fields
		private readonly List<EntityReloadEventHandler> reloadingEventHandlers;
		private string entityTypeName;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of this entity.
		/// </summary>
		public EntityId Id { get; private set; }
		/// <summary>
		/// Gets the underlying CryEngine entity object.
		/// </summary>
		public CryEntity Entity { get; private set; }
		/// <summary>
		/// Gets the name of entity class that represents this entity.
		/// </summary>
		public string EntityClassName
		{
			get
			{
				return this.entityTypeName ?? (this.entityTypeName = this.GetType().GetAttribute<EntityAttribute>().Name);
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
			this.Entity = handle;
			this.Id = id;
			this.reloadingEventHandlers = new List<EntityReloadEventHandler>();
			this.entityTypeName = null;
		}
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, releases this entity.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(false);
			// Remove the entity from the world: since this layer is going down the entity has no reason to
			// stay up.
			EntitySystem.RemoveEntity(this.Id, true);
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
		/// <returns>True, if reloading was a success.</returns>
		public virtual bool Reload()
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
		/// file).
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
		[UnmanagedThunk("Updates this object.")]
		private void UpdateInternal(ref EntityUpdateContext context)
		{
			this.Update(ref context);
		}
		[UnmanagedThunk("Post-updates this object.")]
		private void PostUpdateInternal()
		{
			this.PostUpdate();
		}
		[UnmanagedThunk("Releases this object when the entity is removed by CryEngine.")]
		private void DisposeInternal()
		{
			this.Dispose(true);
		}
		[UnmanagedThunk("Creates a managed object that serves as abstraction layer between CryEngine entity and CryCIL.")]
		private static MonoEntity CreateAbstractionLayer(string className, EntityId identifier, CryEntity handle)
		{
			Type entityType;
			if (EntityRegistry.DefinedEntityClasses.TryGetValue(className, out entityType))
			{
				try
				{
					return (MonoEntity)Activator.CreateInstance(entityType, new object[] { handle, identifier });
				}
				catch (Exception)
				{
					return null;
				}
			}
			return null;
		}
		[UnmanagedThunk("Invoked from underlying object to raise the event Initializing.")]
		private void OnInitializing()
		{
			this.Initialize();
			if (this.Initializing != null) this.Initializing(this, EventArgs.Empty);
		}
		[UnmanagedThunk("Invoked from underlying object to raise the event Initialized.")]
		private void OnInitialized()
		{
			this.PostInitialize();
			if (this.Initialized != null) this.Initialized(this, EventArgs.Empty);
		}
		[UnmanagedThunk("Invoked from underlying object to reload the entity.")]
		private bool OnReloading(ref EntitySpawnParameters parameters)
		{
			if (this.Reload())
			{
				for (int i = 0; i < this.reloadingEventHandlers.Count; i++)
				{
					if (!this.reloadingEventHandlers[i](this, ref parameters))
					{
						return false;
					}
				}
			}
			return true;
		}
		[UnmanagedThunk("Invoked from underlying object after reloading the entity.")]
		private void OnReloaded(ref EntitySpawnParameters parameters)
		{
			this.PostReload();
			if (this.Reloaded != null) this.Reloaded(this, ref parameters);
		}
		[UnmanagedThunk("Invoked from underlying object to acquire entity's pool signature.")]
		private bool GetSignature(CrySync signature)
		{
			return this.GetEntityPoolSignature(signature);
		}
		[UnmanagedThunk("Invoked from underlying object to invoke Synchronize method.")]
		private void SyncInternal(CrySync sync)
		{
			this.Synchronize(sync);
		}
		[UnmanagedThunk("Invoked to set one of the properties.")]
		private void SetEditableProperty(int index, string value)
		{
			EntityRegistry.DefinedProperties[this.EntityClassName][index].Set(this, value);
		}
		[UnmanagedThunk("Invoked to set one of the properties.")]
		private string GetEditableProperty(int index)
		{
			return EntityRegistry.DefinedProperties[this.EntityClassName][index].Get(this);
		}
		#endregion
	}
}