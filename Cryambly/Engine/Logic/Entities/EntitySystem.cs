using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Provides access to CryEngine entity system API.
	/// </summary>
	public static class EntitySystem
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Spawns an entity that is defined in CryCIL that isn't bound to network.
		/// </summary>
		/// <param name="parameters">
		/// An object that encapsulates all parameters that specify which kind of entity to spawn.
		/// </param>
		/// <returns>An object that represents a spawned entity in CryCIL.</returns>
		/// <exception cref="ArgumentException">Cannot create an entity without a valid class.</exception>
		/// <exception cref="ArgumentException">
		/// EntitySystem.SpawnMonoEntity cannot be used to spawn entities that are not defined in CryCIL.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MonoEntity SpawnMonoEntity(EntitySpawnParameters parameters);
		/// <summary>
		/// Spawns an entity that is defined in CryCIL that is bound to network.
		/// </summary>
		/// <param name="parameters">
		/// An object that encapsulates all parameters that specify which kind of entity to spawn.
		/// </param>
		/// <param name="channelId"> 
		/// Identifier of the network channel the entity is going to use to communicate through network.
		/// </param>
		/// <returns>An object that represents a spawned entity in CryCIL.</returns>
		/// <exception cref="ArgumentException">Cannot create an entity without a valid class.</exception>
		/// <exception cref="ArgumentException">
		/// EntitySystem.SpawnMonoEntity cannot be used to spawn entities that are not defined in CryCIL.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// EntitySystem.SpawnNetEntity cannot be used to spawn entities that are not bound to network.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MonoNetEntity SpawnNetEntity(EntitySpawnParameters parameters, ushort channelId);
		/// <summary>
		/// Spawns a CryEngine that is not necessarily defined in CryCIL.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Bear in mind that the entity will be initialized immediately upon spawn. If spawned entity
		/// requires some customization before initialization then you have to spawn it entirely within C++
		/// code.
		/// </para>
		/// <para>
		/// The reason above behavior is the case is that most customization that can be done before
		/// initialization can only be done in C++.
		/// </para>
		/// </remarks>
		/// <param name="parameters">
		/// An object that encapsulates all parameters that specify which kind of entity to spawn.
		/// </param>
		/// <returns>A handle of a newly spawned entity.</returns>
		/// <exception cref="ArgumentException">Cannot create an entity without a valid class.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern CryEntity SpawnCryEntity(EntitySpawnParameters parameters);
		/// <summary>
		/// Removes the entity from the game.
		/// </summary>
		/// <param name="id"> Identifier of the entity to remove.</param>
		/// <param name="now">
		/// Indicates whether the entity should be removed immediately or at the start of the next frame.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveEntity(EntityId id, bool now);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool RegisterEntityClass(string name, string category, string editorHelper,
														string editorIcon, EntityClassFlags flags,
														EditablePropertyInfo[] properties, bool networked,
														bool dontSyncProperties);
		#endregion
		#region Utilities
		#endregion
	}
}