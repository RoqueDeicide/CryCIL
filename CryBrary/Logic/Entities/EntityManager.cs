using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Provides functionality for working with entities.
	/// </summary>
	public static class EntityManager
	{
		/// <summary>
		/// Spawns an entity.
		/// </summary>
		/// <param name="name">       Name to assign to the new entity.</param>
		/// <param name="type">       Name of the type of the entity to spawn.</param>
		/// <param name="position">   
		/// If not null, determines initial position of the entity, otherwise entity is spawned at the
		/// origin of coordinates.
		/// </param>
		/// <param name="orientation">
		/// If not null, determines initial rotation of the entity from default orientation, otherwise no
		/// rotation is applied.
		/// </param>
		/// <param name="scale">      
		/// If not null, determines initial scale of the entity, otherwise entity's size is kept at
		/// default.
		/// </param>
		/// <param name="initialize"> If true, the entity will be initialized immediately.</param>
		/// <param name="flags">      A set of initial flags to assign to the new entity.</param>
		/// <param name="userData">   
		/// Optional array of objects that will be passed to
		/// <see cref="GameObjectExtension.CompleteSpawning"/> method, if spawned entity is derived from
		/// <see cref="GameObjectExtension"/>.
		/// </param>
		/// <returns>A wrapper for the entity.</returns>
		public static EntityWrapper Spawn
		(
			string name,
			string type,
			Vector3? position = null,
			Quaternion? orientation = null,
			Vector3? scale = null,
			bool initialize = true,
			EntityFlags flags = EntityFlags.NoFlags,
			params object[] userData
		)
		{
			IntPtr spawnedEntityHandle;
			EntityId spawnedEntityId;
			// Spawn the entity.
			GameObjectExtension spawnedManagedEntity =
				Native.EntityInterop.SpawnEntity
					(
						new EntitySpawnParams
						{
							Name = name,
							Class = type,
							Position = position ?? new Vector3(0),
							Rotation = orientation ?? Quaternion.Identity,
							Scale = scale ?? new Vector3(1),
							Flags = flags
						},
						initialize,
						out spawnedEntityHandle,
						out spawnedEntityId
					) as GameObjectExtension;
			// Let the entity complete its own initialization by giving it user data.
			if (spawnedManagedEntity != null)// If this is a CryMono entity, of course.
			{
				spawnedManagedEntity.CompleteSpawning(userData);
				return spawnedManagedEntity;
			}
			// Create a simple wrapper, if we spawned a native entity.
			if (spawnedEntityId != 0)
			{
				return new EntityWrapper(spawnedEntityHandle);
			}

			Debug.LogError("Unable to spawn entity named {0} of type {1}", name, type);

			return null;
		}
		/// <summary>
		/// Removes the entity from the world.
		/// </summary>
		/// <param name="wrapper">  Wrapper object for the entity to remove.</param>
		/// <param name="deleteNow">
		/// Indicates, whether deletion should happen now, or on the next frame.
		/// </param>
		public static void Remove(EntityWrapper wrapper, bool deleteNow = false)
		{
			if (wrapper.IsDisposed)
			{
				return;
			}
			Native.EntityInterop.RemoveEntity(wrapper.Identifier, deleteNow);
		}
		/// <summary>
		/// Returns managed wrapper for the entity.
		/// </summary>
		/// <typeparam name="T">Type of the wrapper to get.</typeparam>
		/// <param name="id">Identifier of the entity.</param>
		/// <returns>
		/// Object of type <typeparamref name="T"/> if this entity is managed by that type, otherwise null.
		/// </returns>
		public static T Get<T>(EntityId id) where T : EntityWrapper
		{
			if (id == 0)
			{
				return null;
			}
			return new EntityWrapper(id).ManagedExtension as T;
		}
	}
}