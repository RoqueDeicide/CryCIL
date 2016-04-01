using System;
using System.Linq;
using CryCil;
using CryCil.Engine.Data;
using CryCil.Engine.Environment;
using CryCil.Engine.Logic;
using CryCil.Engine.Network;
using CryCil.Engine.Physics;
using GameLibrary.Entities.Players;

namespace GameLibrary.GameModes
{
	/// <summary>
	/// Represents a set of rules that describes how the single player game goes.
	/// </summary>
	[GameRules("SinglePlayer")]
	[DefaultGameRules]
	public class SinglePlayerRules : GameRules
	{
		#region Construction
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		public SinglePlayerRules(CryEntity handle, EntityId id) : base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="invokedFromNativeCode">
		/// Indicates whether this entity was released from native code.
		/// </param>
		public override void Dispose(bool invokedFromNativeCode)
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		public override void Initialize()
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		public override void PostInitialize()
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="sync">Object that handles synchronization.</param>
		public override void Synchronize(CrySync sync)
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		public override void PostUpdate()
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates the aspect that requires synchronization.</param>
		/// <param name="profile">
		/// A number in range [0; 7] that specifies the data format that has to be used to synchronize the
		/// aspect data.
		/// </param>
		/// <param name="flags">  A set of flags that specify how to write the snapshot.</param>
		/// <returns>True, if synchronization was successful.</returns>
		public override bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile,
													SnapshotFlags flags)
		{
			return true;
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="channel">Network channel that is used to communicate with a server.</param>
		public override void OnConnect(CryNetChannel channel)
		{
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="cause">      Identifier of the cause of disconnection.</param>
		/// <param name="description">
		/// Description of the cause. Can be used to specify a custom cause.
		/// </param>
		public override void OnDisconnect(DisconnectionCause cause, string description)
		{
		}
		/// <summary>
		/// Spawns the player.
		/// </summary>
		/// <param name="channel">
		/// Identifier of the channel that is used to communicate with the client.
		/// </param>
		/// <param name="isReset">
		/// Indicates whether client's connection was caused by the game reset.
		/// </param>
		/// <returns>Value that indicates whether client was successfully connected.</returns>
		public override bool OnClientConnect(ChannelId channel, bool isReset)
		{
			// Spawn location is at the center of the map, 15 meters into the air.
			float terrainSize = Terrain.Size;
			Vector2 centerPoint = new Vector2(terrainSize / 2);
			float terrainElevation = Terrain.Elevation(centerPoint);
			Vector3 spawnLocation = new Vector3(centerPoint) {Z = terrainElevation + 15};

			EntityFlags flags = 0;
			EntityId id = new EntityId();
			if (channel != 0)
			{
				flags |= EntityFlags.NeverNetworkStatic;
				CryNetChannel netChannel = channel.Channel;
				if (netChannel.IsValid && netChannel.IsLocal)
				{
					id = EntityId.LocalPlayerId;
				}
			}

			EntitySpawnParameters parameters = new EntitySpawnParameters(typeof(Player), "Player", flags, id)
			{
				Position = spawnLocation
			};

			EntitySystem.SpawnNetEntity(ref parameters, channel);

			return true;
		}
		/// <summary>
		/// Does nothing special.
		/// </summary>
		/// <param name="channel">    
		/// Identifier of the channel that was used to communicate with the client.
		/// </param>
		/// <param name="cause">      Identifier of the cause of disconnection.</param>
		/// <param name="description">
		/// Description of the cause. Can be used to specify a custom cause.
		/// </param>
		/// <param name="keep">       
		/// Indicates whether information pertaining to the client should be kept alive.
		/// </param>
		public override void OnClientDisconnect(ChannelId channel, DisconnectionCause cause, string description,
												bool keep)
		{
		}
		#endregion
		#region Utilities
		#endregion
	}
}