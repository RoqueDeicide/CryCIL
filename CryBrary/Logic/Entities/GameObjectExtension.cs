using System;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.RunTime.Serialization;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Base class for CryMono objects that define custom logic for CryEngine Game
	/// Objects.
	/// </summary>
	public abstract partial class GameObjectExtension : EntityWrapper
	{
		#region Fields

		#endregion
		#region Properties
		#endregion
		#region (De/Con)struction
		/// <summary>
		/// Initializes base properties of this class.
		/// </summary>
		/// <param name="handle">    Pointer to IEntity object in native memory.</param>
		/// <param name="identifier">Identifier of this entity.</param>
		protected GameObjectExtension(IntPtr handle, EntityId identifier)
			: base(handle, identifier)
		{
		}
		~GameObjectExtension()
		{
			// This method is called from managed code, which means we need to release the
			// native counterpart as well.
			this.Dispose(false);
			Native.EntityInterop.RemoveEntity(this.Identifier, true);
		}
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, completes spawning of this entity.
		/// </summary>
		/// <remarks>
		/// This method is only invoked, when this entity is spawned from CryMono.
		/// </remarks>
		/// <param name="userData">
		/// Optional array of arguments that was passed to
		/// <see cref="EntityManager.Spawn"/>.
		/// </param>
		public abstract void CompleteSpawning(object[] userData);
		/// <summary>
		/// When implemented in derived class, synchronizes this entity with it's
		/// representation somewhere else.
		/// </summary>
		/// <param name="syncronizer">Object that handles the synchronization.</param>
		public abstract void FullSynchronize(CrySerialize syncronizer);
		/// <summary>
		/// When implemented in derived class, synchronizes this entity with it's
		/// representations on remote computers.
		/// </summary>
		/// <param name="syncronizer">Object that handles the synchronization.</param>
		/// <param name="aspect">     
		/// Aspect of the entity that needs synchronization.
		/// </param>
		/// <param name="profile">    Which profile to synchronize.</param>
		/// <param name="flags">      Physics flags to use in syncronization.</param>
		public abstract void NetworkSynchronize(CrySerialize syncronizer, EntityAspects aspect, byte profile, int flags);
		/// <summary>
		/// When implemented in derived class, lets the entity decide how to update its
		/// logical state.
		/// </summary>
		/// <param name="context">   
		/// The object that describes the situation in the game during thinking.
		/// </param>
		/// <param name="updateSlot">
		/// Identifier of the entity slot that requires an update.
		/// </param>
		public abstract void Think(EntityUpdateContext context, int updateSlot);
		/// <summary>
		/// When implemented in derived class, lets the entity decide how to updates its
		/// logical state after all other entities have made their first decision.
		/// </summary>
		/// <param name="deltaTime">Time in seconds that passed since last frame.</param>
		public abstract void AfterThought(float deltaTime);
		/// <summary>
		/// When implemented in derived class, processes an event that is not processed
		/// anywhere else.
		/// </summary>
		/// <remarks>
		/// Documentation for <see cref="EntityEvent"/> contains details about each event
		/// along with descriptions of each parameter.
		/// </remarks>
		/// <param name="event">           Event to process.</param>
		/// <param name="parameter0">      Optional first integer parameter.</param>
		/// <param name="parameter1">      Optional second integer parameter.</param>
		/// <param name="parameter2">      Optional third integer parameter.</param>
		/// <param name="parameter3">      Optional fourth integer parameter.</param>
		/// <param name="singleParameter0">
		/// Optional first floating point parameter.
		/// </param>
		/// <param name="singleParameter1">
		/// Optional second floating point parameter.
		/// </param>
		/// <seealso cref="EntityEvent"/>
		public abstract void ProcessEvent(EntityEvent @event,
										  IntPtr parameter0, IntPtr parameter1,
										  IntPtr parameter2, IntPtr parameter3,
										  float singleParameter0,
										  float singleParameter1);
		/// <summary>
		/// When overridden in derived class, releases all resources that were allocated
		/// by this entity.
		/// </summary>
		/// <param name="releaseManaged">
		/// Indicates whether this entity must release managed resources or just unmanaged
		/// ones.
		/// </param>
		public virtual void Dispose(bool releaseManaged)
		{
			// Invalidate the handle and identifier.
			base.Dispose();
		}
		/// <summary>
		/// Forces this entity to release all of its resources.
		/// </summary>
		public override void Dispose()
		{
			// This method is called from managed code, which means we need to release the
			// native counterpart as well.

			// Lets be safe, validating identifier this time won't hurt.
			this.ExtraSafe = true;
			// Remove the entity from the world. This call will eventually cause
			// invocation of DisposeInternal.
			EntityManager.Remove(this, true);
		}
		#endregion
		#region Utilities
		#endregion
	}
	/// <summary>
	/// Data that describes how entity must be spawned.
	/// </summary>
	public struct EntitySpawnParameters
	{
		/// <summary>
		/// Unique identifier assigned to the entity. If 0 then an ID will be generated
		/// automatically (based on the <see cref="StaticEntityId"/> parameter).
		/// </summary>
		public EntityId IdCurrent;
		/// <summary>
		/// Previous <see cref="EntityId"/> that was used by the entity prior to its
		/// reloading.
		/// </summary>
		public EntityId IdPrevious;
		/// <summary>
		/// Optional entity guid.
		/// </summary>
		public ulong GuidCurrent;
		/// <summary>
		/// Previously used Guid, in the case of reloading.
		/// </summary>
		public ulong GuidPrevious;
		/// <summary>
		/// Pointer to IEntityClass object that describes the entity.
		/// </summary>
		public IntPtr Class;					// IEntityClass *
		/// <summary>
		/// Pointer to an entity preset.
		/// </summary>
		public IntPtr Archetype;				// IEntityArchetype *
		/// <summary>
		/// The name of the layer the entity resides in, when in the Editor.
		/// </summary>
		public IntPtr LayerName;				// const char*
		/// <summary>
		/// The name of the entity, uniqueness not required.
		/// </summary>
		public IntPtr Name;						// const char*
		/// <summary>
		/// Flags assigned to the entity.
		/// </summary>
		public uint Flags;
		/// <summary>
		/// Extended set of flags assigned to the entity.
		/// </summary>
		public uint FlagsExtended;
		/// <summary>
		/// Indicates whether spawn lock should be active.
		/// </summary>
		public bool IgnoreLock;
		/// <summary>
		/// Indicates whether identifier of this entity is static.
		/// </summary>
		/// <remarks>
		/// To support save games compatible with patched levels (patched levels might use
		/// more EntityIDs and save game might conflict with dynamic ones).
		/// </remarks>
		public bool StaticEntityId;
		/// <summary>
		/// Indicates whether this entity was created through entity pool.
		/// </summary>
		public bool CreatedThroughPool;
		/// <summary>
		/// Initial entity position (Local space).
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Initial entity rotation (Local space).
		/// </summary>
		public Quaternion Rotation;
		/// <summary>
		/// Initial entity scale (Local space).
		/// </summary>
		public Vector3 Scale;
		/// <summary>
		/// Any user defined data. It will be available for container when it will be
		/// created.
		/// </summary>
		public IntPtr UserData;					// void *
		/// <summary>
		/// Pointer to the script table with properties.
		/// </summary>
		public IntPtr PropertiesTable;			// IScriptTable *
		/// <summary>
		/// Pointer to the script table with instance properties.
		/// </summary>
		public IntPtr PropertiesInstanceTable;	// IScriptTable *
	}
	/// <summary>
	/// Encapsulates data represents a situation in the game when the entity is updated.
	/// </summary>
	public struct EntityUpdateContext
	{
		/// <summary>
		/// Identifier of the current frame.
		/// </summary>
		public int FrameId;
		/// <summary>
		/// Current active camera handle.
		/// </summary>
		public IntPtr Camera;				// CCamera *
		/// <summary>
		/// Current system time.
		/// </summary>
		public float CurrentTime;
		/// <summary>
		/// Delta frame time (of last frame).
		/// </summary>
		public float FrameTime;
		/// <summary>
		/// Indicates if a profile entity must update the log.
		/// </summary>
		public bool ProfileToLog;
		/// <summary>
		/// Number of updated entities.
		/// </summary>
		public int UpdatedEntitiesCount;
		/// <summary>
		/// Number of visible and updated entities.
		/// </summary>
		public int VisibleEntitiesCount;
		/// <summary>
		/// Maximal view distance.
		/// </summary>
		public float MaxViewDistance;
		/// <summary>
		/// Maximal view distance squared.
		/// </summary>
		public float MaxViewDistanceSquared;
		/// <summary>
		/// Camera source position.
		/// </summary>
		public Vector3 CameraPos;
	}
}