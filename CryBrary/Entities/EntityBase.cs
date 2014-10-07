using System;
using System.Collections.Generic;
using CryEngine.Logic.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Physics;
using CryEngine.StaticObjects;

namespace CryEngine.Entities
{
	/// <summary>
	/// Represents a CryENGINE entity
	/// </summary>
	public abstract class EntityBase
	{
		internal PhysicalEntity _physics;
		/// <summary>
		/// Gets the globally unique identifier of this entity, assigned to it by the
		/// Entity System. EntityGUID's are guaranteed to be the same when saving /
		/// loading, and are the same in both editor and pure game mode.
		/// </summary>
		public EntityGuid GUID
		{
			get { return EntityInterop.GetEntityGUID(this.EntityHandle); }
		}
		/// <summary>
		/// Gets the physical entity, contains essential functions for modifying the
		/// entitys existing physical state.
		/// </summary>
		public PhysicalEntity Physics
		{
			get { return this._physics; }
		}
		/// <summary>
		/// Removes the entity from the CryEngine world.
		/// </summary>
		/// <param name="forceRemoveNow">
		/// If true, the entity will be removed immediately.
		/// </param>
		public virtual void Remove(bool forceRemoveNow = false)
		{
			Entity.Remove(this.Id, forceRemoveNow);
		}
		/// <summary>
		/// Loads a light source to the specified slot, or to the next available slot.
		/// </summary>
		/// <param name="parameters">
		/// New params of the light source we wish to load
		/// </param>
		/// <param name="slot">      
		/// Slot we want to load the light into, if -1 chooses the next available slot.
		/// </param>
		/// <returns>
		/// The slot where the light source was loaded, or -1 if loading failed.
		/// </returns>
		public int LoadLight(LightParams parameters, int slot = 1)
		{
			return EntityInterop.LoadLight(this.EntityHandle, slot, parameters);
		}
		/// <summary>
		/// Loads a mesh for this entity. Can optionally load multiple meshes using entity
		/// slots.
		/// </summary>
		/// <param name="name">Path to the object (Relative to the game directory)</param>
		/// <param name="slot"></param>
		/// <returns>true if successful, otherwise false.</returns>
		public bool LoadObject(string name, int slot = 0)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			if (name.EndsWith("cgf"))
				EntityInterop.LoadObject(this.EntityHandle, name, slot);
			else if (name.EndsWith("cdf") || name.EndsWith("cga") || name.EndsWith("chr"))
				EntityInterop.LoadCharacter(this.EntityHandle, name, slot);
			else
				return false;

			return true;
		}
		/// <summary>
		/// Gets the path to the currently loaded object.
		/// </summary>
		/// <param name="slot">
		/// Slot containing the object we want to know the path of.
		/// </param>
		/// <returns>Path to the currently loaded object at the specified slot.</returns>
		public string GetObjectFilePath(int slot = 0)
		{
			return EntityInterop.GetStaticObjectFilePath(this.EntityHandle, slot);
		}
		/// <summary>
		/// Frees the specified slot of all objects.
		/// </summary>
		/// <param name="slot">Zero-based index of the slot to free.</param>
		public void FreeSlot(int slot)
		{
			EntityInterop.FreeSlot(this.EntityHandle, slot);
		}
		/// <summary>
		/// Requests movement at the specified slot, providing an animated character is
		/// currently loaded.
		/// </summary>
		/// <param name="request">Object that describes the movement.</param>
		public void AddMovement(ref EntityMovementRequest request)
		{
			EntityInterop.AddMovement(this.AnimatedCharacterHandle, ref request);
		}
		/// <summary>
		/// Loads an emitter for a particle effect.
		/// </summary>
		/// <param name="particleEffect">
		/// <see cref="ParticleEffect"/> to load emitter for.
		/// </param>
		/// <param name="spawnParams">   A set of parameters to use for spawning.</param>
		/// <param name="slot">          Slot to assign the emitter to.</param>
		/// <returns>A new particle emitter.</returns>
		public ParticleEmitter LoadParticleEmitter(ParticleEffect particleEffect,
												   ref ParticleSpawnParameters spawnParams,
												   int slot = -1)
		{
			return
				ParticleEmitter.TryGet(EntityInterop.LoadParticleEmitter(this.EntityHandle, slot, particleEffect.Handle,
																			   ref spawnParams));
		}
	}
}