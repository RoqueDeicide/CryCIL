using System;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.Utilities;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Defines signature of methods that can handle <see cref="CryEntityPool.PoolBookmarkCreated"/> event.
	/// </summary>
	/// <param name="id">        Identifier of the entity that has been bookmarked.</param>
	/// <param name="parameters">Parameters that are describe pooled entity.</param>
	/// <param name="entityNode">
	/// An object that provides access to Xml that describes the entity, if it was loaded from Xml before
	/// being bookmarked. This object is disposed immediately after
	/// <see cref="CryEntityPool.PoolBookmarkCreated"/> event handling is complete.
	/// </param>
	public delegate void PoolBookmarkCreationHandler(EntityId id, EntitySpawnParameters parameters, CryXmlNode entityNode);
	/// <summary>
	/// Defines signature of methods that can handle events related to migration of entities to and from
	/// the pool.
	/// </summary>
	/// <param name="id">    Identifier of migrating entity.</param>
	/// <param name="entity">Wrapper that represents migrating entity.</param>
	public delegate void PoolMigrationHandler(EntityId id, CryEntity entity);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="CryEntityPool.BookmarkSyncing"/> event.
	/// </summary>
	/// <param name="sync">  An object that handles synchronization.</param>
	/// <param name="entity">Entity that is being synchronized.</param>
	public delegate void PoolBookmarkSyncHandler(CrySync sync, CryEntity entity);
	/// <summary>
	/// Provides access to CryEngine entity pool API.
	/// </summary>
	/// <remarks>
	/// Entity pool allows entities to avoid being created and destroyed multiple times without need (used
	/// for stuff like bullets).
	/// </remarks>
	public static class CryEntityPool
	{
		#region Fields

		#endregion
		#region Properties

		#endregion
		#region Events
		/// <summary>
		/// Occurs when entity is bookmarked in the pool.
		/// </summary>
		public static event PoolBookmarkCreationHandler PoolBookmarkCreated;
		/// <summary>
		/// Occurs when an entity is prepared (created) from the pool.
		/// </summary>
		public static event PoolMigrationHandler EntityPrepared;
		/// <summary>
		/// Occurs when an entity is about to be saved before returning to the pool.
		/// </summary>
		public static event PoolMigrationHandler EntityReturning;
		/// <summary>
		/// Occurs when an entity is about to return to the pool.
		/// </summary>
		public static event PoolMigrationHandler EntityReturned;
		/// <summary>
		/// Occurs when pool definitions are loaded from the file.
		/// </summary>
		/// <remarks>
		/// Default path to the file that contains those definitions is:
		/// <c>Scripts\Entities\EntityPoolDefinitions.xml</c>.
		/// </remarks>
		public static event Action DefinitionsLoaded;
		/// <summary>
		/// Occurs when an entity is synchronized with its bookmark.
		/// </summary>
		public static event PoolBookmarkSyncHandler BookmarkSyncing;
		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Enables entity pool.
		/// </summary>
		/// <remarks>
		/// In default game sample entity pool is enabled for singleplayer but not multiplayer.
		/// </remarks>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Enable();
		/// <summary>
		/// Disables entity pool.
		/// </summary>
		/// <remarks>
		/// In default game sample entity pool is enabled for singleplayer but not multiplayer.
		/// </remarks>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Disable();
		/// <summary>
		/// Resets all pools and returns all active entities.
		/// </summary>
		/// <param name="saveState">Indicates whether state of all active entities should be saved.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetPools(bool saveState = true);
		/// <summary>
		/// Indicates whether an entity class is set to be bookmarked in the pool.
		/// </summary>
		/// <param name="className">Name of the entity class to check.</param>
		/// <returns>True, if entities of that class will be bookmarked in the pool.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsDefaultBookmarked(string className);
		/// <summary>
		/// Indicates whether the entity is being prepared from the pool.
		/// </summary>
		/// <param name="entityId">
		/// Identifier of the entity that is being prepared, if it's being prepared.
		/// </param>
		/// <returns>True, if the entity is being prepared, otherwise false.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPreparingEntity(out EntityId entityId);
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked to raise the event PoolBookmarkCreated.")]
		private static void OnPoolBookmarkCreated(EntityId id, EntitySpawnParameters parameters, IntPtr entitynode)
		{
			PoolBookmarkCreationHandler handler = PoolBookmarkCreated;
			using (CryXmlNode node = new CryXmlNode(entitynode))
			{
				if (handler != null) handler(id, parameters, node);
			}
		}
		[UnmanagedThunk("Invoked to raise the event EntityPrepared.")]
		private static void OnEntityPrepared(EntityId id, CryEntity entity)
		{
			if (EntityPrepared != null) EntityPrepared(id, entity);
		}
		[UnmanagedThunk("Invoked to raise the event EntityReturning.")]
		private static void OnEntityReturning(EntityId id, CryEntity entity)
		{
			if (EntityReturning != null) EntityReturning(id, entity);
		}
		[UnmanagedThunk("Invoked to raise the event EntityReturned.")]
		private static void OnEntityReturned(EntityId id, CryEntity entity)
		{
			if (EntityReturned != null) EntityReturned(id, entity);
		}
		[UnmanagedThunk("Invoked to raise the event DefinitionsLoaded.")]
		private static void OnDefinitionsLoaded()
		{
			if (DefinitionsLoaded != null) DefinitionsLoaded();
		}
		[UnmanagedThunk("Invoked to raise the event BookmarkSyncing.")]
		private static void OnBookmarkSyncing(CrySync sync, CryEntity entity)
		{
			if (BookmarkSyncing != null) BookmarkSyncing(sync, entity);
		}
		#endregion
	}
}