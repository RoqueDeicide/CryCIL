using System;
using System.Collections.Generic;

using System.Linq;

using CryEngine.Initialization;
using CryEngine.Extensions;
using CryEngine.Native;

namespace CryEngine
{
	public partial class Entity
	{
		/// <summary>
		/// Spawns a new entity
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="type"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <param name="autoInit"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static EntityBase Spawn(string entityName, Type type, Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null, bool autoInit = true, EntityFlags flags = EntityFlags.CastShadow, params object[] args)
		{
			return Spawn(entityName, type.Name, pos, rot, scale, autoInit, flags, args);
		}

		/// <summary>
		/// Spawns a new entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entityName"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <param name="autoInit"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static T Spawn<T>(string entityName, Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null, bool autoInit = true, EntityFlags flags = EntityFlags.CastShadow, params object[] args) where T : Entity, new()
		{
			var entity = Spawn(entityName, typeof(T).Name, pos, rot, scale, autoInit, flags, args);
			if (entity == null)
				return null;

			var entityT = entity as T;
			if (entityT == null)
				Debug.LogAlways("[Entity.Spawn<T>] Spawn was successful, but type could not be casted to type {0}", typeof(T).Name);

			return entityT;
		}

		/// <summary>
		/// Spawns a new entity
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="className"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <param name="autoInit"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static EntityBase Spawn(string entityName, string className = "Default", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null, bool autoInit = true, EntityFlags flags = EntityFlags.CastShadow, params object[] args)
		{
			EntityInitializationParams info;

			var ent = NativeEntityMethods.SpawnEntity(new EntitySpawnParams { Name = entityName, Class = className, Pos = pos ?? new Vector3(1, 1, 1), Rot = rot ?? Quaternion.Identity, Scale = scale ?? new Vector3(1, 1, 1), Flags = flags }, autoInit, out info) as Entity;
			if (ent != null)
			{
				ent.OnPostSpawn(args);

				return ent;
			}
			else if (info.Id != 0)
				return CreateNativeEntity(info.Id, info.IEntityPtr) as Entity;

			Debug.LogAlways("[Entity.Spawn] Failed to spawn entity of class {0} with name {1}", className, entityName);
			return null;
		}

		/// <summary>
		/// Removes the entity with the specified id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="forceRemoveNow"></param>
		public static void Remove(EntityId id, bool forceRemoveNow = false)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (id == 0)
				throw new ArgumentException("entityId cannot be 0!");
#endif

			NativeEntityMethods.RemoveEntity(id, forceRemoveNow);
		}

		internal static bool InternalRemove(EntityId id)
		{
			// The result from OnRemove returns whether the entity should be allowed to be removed.
			// If we return false, the entity won't be removed.
			var result = true;

			ScriptManager.Instance.RemoveInstances(ScriptType.Entity, instance =>
				{
					var entity = instance as EntityBase;
					if (entity != null && entity.Id == id)
					{
						if (entity is Entity)
							result = (entity as Entity).OnRemove();

						return true;
					}

					return false;
				});

			return result;
		}

		/// <summary>
		/// Get an entity by its unique ID.
		/// </summary>
		/// <param name="entityId">The ID as an unsigned integer.</param>
		/// <returns>A reference to the entity.</returns>
		/// <remarks>
		/// If the entity does not exist in the managed space, this function will attempt to find a
		/// C++ entity with the specified ID
		/// </remarks>
		public static T Get<T>(EntityId entityId) where T : EntityBase
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (entityId == 0)
				throw new ArgumentException("entityId cannot be 0!");
#endif

			return ScriptManager.Instance.Find<T>(ScriptType.Entity, x => x.Id == entityId);
		}

		/// <summary>
		/// Get an entity by its unique ID.
		/// </summary>
		/// <param name="entityId">The ID as an unsigned integer.</param>
		/// <returns>A reference to the entity.</returns>
		/// <remarks>
		/// If the entity does not exist in the managed space, this function will attempt to find a
		/// C++ entity with the specified ID&gt;
		/// </remarks>
		public static EntityBase Get(EntityId entityId)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (entityId == 0)
				throw new ArgumentException("entityId cannot be 0!");
#endif

			var ent = Get<EntityBase>(entityId);
			if (ent != null)
				return ent;

			// Couldn't find a CryMono entity, check if a non-managed one exists.
			var entPointer = NativeEntityMethods.GetEntity(entityId);
			if (entPointer != IntPtr.Zero)
				return CreateNativeEntity(entityId, entPointer);

			return null;
		}

		internal static EntityBase Get(IntPtr entityPointer)
		{
			var entity = ScriptManager.Instance.Find<EntityBase>(ScriptType.Entity, x => x.EntityHandle == entityPointer);

			if (entity != null)
				return entity;

			return CreateNativeEntity(NativeEntityMethods.GetEntityId(entityPointer), entityPointer);
		}

		internal static EntityBase CreateNativeEntity(EntityId id, IntPtr entityPointer)
		{
			// check if actor
			var actorInfo = NativeActorMethods.GetActorInfoById((uint)id._value);
			if (actorInfo.Id != 0)
				return Actor.CreateNativeActor(actorInfo);

			var nativeEntity = new NativeEntity(id, entityPointer);
			ScriptManager.Instance.AddScriptInstance(nativeEntity, ScriptType.Entity);

			return nativeEntity;
		}

		/// <summary>
		/// Get an entity by name.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <returns>A reference to the entity.</returns>
		/// <remarks>
		/// If multiple entities have the same name, it will return the first found. Consider using
		/// IDs where necessary.
		/// </remarks>
		public static EntityBase Find(string name)
		{
			var id = NativeEntityMethods.FindEntity(name);
			if (id == 0)
				return null;

			return Get(new EntityId(id));
		}

		/// <summary>
		/// Gets an array of entities that are of a given class.
		/// </summary>
		/// <param name="className">The entity class to search for.</param>
		/// <returns>An array of entities.</returns>
		public static IEnumerable<EntityBase> GetByClass(string className)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (String.IsNullOrEmpty(className))
				throw new ArgumentException("className should not be null or empty", "className");
#endif
			return GetEntitiesCommon<Entity>(NativeEntityMethods.GetEntitiesByClass(className));
		}

		/// <summary>
		/// Gets an array of entities that are of a given class.
		/// </summary>
		/// <typeparam name="T">The entity class to search for.</typeparam>
		/// <returns>An array of entities of type T.</returns>
		public static IEnumerable<T> GetByClass<T>() where T : EntityBase
		{
			return GetEntitiesCommon<T>(NativeEntityMethods.GetEntitiesByClass(typeof(T).Name));
		}

		public static IEnumerable<EntityBase> GetByClasses(string[] classNames)
		{
			return GetEntitiesCommon<Entity>(NativeEntityMethods.GetEntitiesByClasses(classNames.Cast<object>().ToArray()));
		}

		/// <summary>
		/// Gets a list of entities within the specified area.
		/// </summary>
		/// <param name="bbox"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static IEnumerable<EntityBase> GetInBox(BoundingBox bbox, EntityQueryFlags flags = EntityQueryFlags.All)
		{
			return GetEntitiesCommon<EntityBase>(NativeEntityMethods.GetEntitiesInBox(bbox, flags));
		}

		/// <summary>
		/// Gets a list of entities within the specified area.
		/// </summary>
		/// <param name="bbox"></param>
		/// <param name="flags"></param>
		/// <returns></returns>
		public static IEnumerable<T> GetInBox<T>(BoundingBox bbox, EntityQueryFlags flags = EntityQueryFlags.All) where T : EntityBase
		{
			return GetEntitiesCommon<T>(NativeEntityMethods.GetEntitiesInBox(bbox, flags));
		}

		public static IEnumerable<EntityBase> QueryProximity(BoundingBox bbox, string className, EntityFlags flags = 0)
		{
			return GetEntitiesCommon<EntityBase>(NativeEntityMethods.QueryProximity(bbox, className, flags));
		}

		public static IEnumerable<T> QueryProximity<T>(BoundingBox bbox, EntityFlags flags = 0) where T : EntityBase
		{
			return GetEntitiesCommon<T>(NativeEntityMethods.QueryProximity(bbox, typeof(T).Name, flags));
		}

		internal static IEnumerable<T> GetEntitiesCommon<T>(object[] ents) where T : EntityBase
		{
			if (ents == null || ents.Length <= 0)
				yield break;

			foreach (EntityId id in ents)
			{
				var ent = Get<T>(id);
				if (ent != null)
					yield return ent;
			}
		}
	}

	/// <summary>
	/// These flags control entity instance behaviour.
	/// </summary>
	[Flags]
	public enum EntityFlags
	{
		CastShadow = (1 << 1),
		Unremovable = (1 << 2),

		ClientOnly = (1 << 8),
		ServerOnly = (1 << 9),

		/// <summary>
		/// Entity will trigger areas when it enters them.
		/// </summary>
		TriggerAreas = (1 << 14),

		/// <summary>
		/// This entity will not be saved.
		/// </summary>
		NoSave = (1 << 15),

		/// <summary>
		/// Entity was spawned dynamically without a class.
		/// </summary>
		Spawned = (1 << 24),
	}

	/// <summary>
	/// These flags define behaviour for entity classes.
	/// </summary>
	[Flags]
	public enum EntityClassFlags
	{
		/// <summary>
		/// If set this class will not be visible in editor,and entity of this class cannot be
		/// placed manually in editor.
		/// </summary>
		Invisible = 0x0001,
		/// <summary>
		/// If this is default entity class.
		/// </summary>
		Default = 0x0002,
	}
}