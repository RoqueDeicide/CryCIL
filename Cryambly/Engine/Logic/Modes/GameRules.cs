using System;
using CryCil.Engine.Network;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for object that define the rule set of the game.
	/// </summary>
	public abstract class GameRules : MonoNetEntity
	{
		#region Fields
		
		#endregion
		#region Properties
		/// <summary>
		/// When overridden in derived class, gets or sets the time that remains on the timer.
		/// </summary>
		/// <remarks>
		/// This default implementation returns 0.
		/// </remarks>
		public virtual float RemainingGameTime
		{
			get { return 0; }
			// ReSharper disable once ValueParameterNotUsed
			set { }
		}
		[UnmanagedThunk("Gets RemainingGameTime.")]
		private float GetRemainingGameTimeInternal()
		{
			return this.RemainingGameTime;
		}
		[UnmanagedThunk("Sets RemainingGameTime.")]
		private void SetRemainingGameTimeInternal(float value)
		{
			this.RemainingGameTime = value;
		}
		#endregion
		#region Events
		
		#endregion
		#region Construction
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		protected GameRules(CryEntity handle, EntityId id)
			: base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether a disconnected client should be kept for a while.
		/// </summary>
		/// <remarks>
		/// This default implementation reports <c>false</c> unless <paramref name="cause"/> is equal to <see cref="DisconnectionCause.Timeout"/> or <paramref name="description"/> is equal to "timeout" (comparison is not case-sensitive).
		/// </remarks>
		/// <param name="client">Identifier of the channel that is used to communicate with a client.</param>
		/// <param name="cause">Identifier of the cause of disconnection.</param>
		/// <param name="description">Description of the cause. Can be used to specify a custom cause.</param>
		/// <returns>Indication whether a client-related data should be kept alive.</returns>
		public virtual bool ShouldKeepClient(ChannelId client, DisconnectionCause cause, string description)
		{
			try
			{
				return cause == DisconnectionCause.Timeout ||
					   description.Equals("timeout", StringComparison.InvariantCultureIgnoreCase);
			}
			catch (ArgumentException)
			{
				return true;
			}
		}
		[UnmanagedThunk("Invokes ShouldKeepClient.")]
		private bool ShouldKeepClientInternal(int client, DisconnectionCause cause, string description)
		{
			return this.ShouldKeepClient(client, cause, description);
		}
		/// <summary>
		/// Precaches the resources after loading the level.
		/// </summary>
		/// <remarks>
		/// This default implementation doesn't do anything.
		/// </remarks>
		public virtual void PrecacheLevel()
		{
		}
		[UnmanagedThunk("Invokes PrecacheLevel.")]
		private void PrecacheLevelInternal()
		{
			this.PrecacheLevel();
		}
		/// <summary>
		/// This method is invoked on a client game when it connects to the server.
		/// </summary>
		/// <param name="channel">Network channel that is used to communicate with a server.</param>
		public abstract void OnConnect(CryNetChannel channel);
		[UnmanagedThunk("Invokes OnConnect.")]
		private void OnConnectInternal(CryNetChannel channel)
		{
			this.OnConnect(channel);
		}
		/// <summary>
		/// This method is invoked on a client game when it disconnects from the server.
		/// </summary>
		/// <param name="cause">Identifier of the cause of disconnection.</param>
		/// <param name="description">Description of the cause. Can be used to specify a custom cause.</param>
		public abstract void OnDisconnect(DisconnectionCause cause, string description);
		[UnmanagedThunk("Invokes OnDisconnect.")]
		private void OnDisconnectInternal(DisconnectionCause cause, string description)
		{
			this.OnDisconnect(cause, description);
		}
		/// <summary>
		/// This method is invoked when on a server when a client connects to it.
		/// </summary>
		/// <param name="channel">Identifier of the channel that is used to communicate with the client.</param>
		/// <param name="isReset">Indicates whether client's connection was caused by the game reset.</param>
		/// <returns>Value that indicates whether client was successfully connected.</returns>
		public abstract bool OnClientConnect(ChannelId channel, bool isReset);
		[UnmanagedThunk("Invokes OnClientConnect.")]
		private void OnClientConnectInternal(int client, bool isReset)
		{
			this.OnClientConnect(client, isReset);
		}
		/// <summary>
		/// This method is invoked when on a server when a client disconnects from it.
		/// </summary>
		/// <param name="channel">Identifier of the channel that was used to communicate with the client.</param>
		/// <param name="cause">Identifier of the cause of disconnection.</param>
		/// <param name="description">Description of the cause. Can be used to specify a custom cause.</param>
		/// <param name="keep">Indicates whether information pertaining to the client should be kept alive.</param>
		public abstract void OnClientDisconnect(ChannelId channel, DisconnectionCause cause, string description,
			bool keep);
		[UnmanagedThunk("Invokes OnClientDisconnect.")]
		private void OnClientDisconnectInternal(ChannelId channel, DisconnectionCause cause, string description,
			bool keep)
		{
			this.OnClientDisconnect(channel, cause, description, keep);
		}
		/// <summary>
		/// This method is invoked when a client enters current game.
		/// </summary>
		/// <remarks>
		/// This default implementation returns <c>true</c>.
		/// </remarks>
		/// <param name="client">Identifier of the channel that is used to communicate with a client.</param>
		/// <param name="isReset">Indicates whether client's entrance was caused by the game reset.</param>
		/// <returns>Value that indicates whether client was successfully entered into the game.</returns>
		public virtual bool OnClientEnteredGame(ChannelId client, bool isReset )
		{
			return true;
		}
		[UnmanagedThunk("Invokes OnClientEnteredGame.")]
		private bool OnClientEnteredGameInternal(int channel, bool isReset )
		{
			return this.OnClientEnteredGame(channel, isReset);
		}
		/// <summary>
		/// Notifies this object that the entity has been spawned via a game context.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default implementation doesn't do anything.
		/// </para>
		/// <para>There is no information on when and why this method is invoked.</para>
		/// </remarks>
		/// <param name="entity">An entity that has been spawned.</param>
		public virtual void OnEntitySpawn(CryEntity entity )
		{
		}
		[UnmanagedThunk("Invokes OnEntitySpawn.")]
		private void OnEntitySpawnInternal(CryEntity entity )
		{
			this.OnEntitySpawn(entity);
		}
		/// <summary>
		/// Notifies this object that the entity has been removed via a game context.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default implementation doesn't do anything.
		/// </para>
		/// <para>There is no information on when and why this method is invoked.</para>
		/// </remarks>
		/// <param name="entity">An entity that has been removed.</param>
		public virtual void OnEntityRemoved(CryEntity entity)
		{
		}
		[UnmanagedThunk("Invokes OnEntityRemoved.")]
		private void OnEntityRemovedInternal(CryEntity entity)
		{
			this.OnEntityRemoved(entity);
		}
		/// <summary>
		/// Notifies this object that the entity has been reused via a game context.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default implementation doesn't do anything.
		/// </para>
		/// <para>There is no information on when and why this method is invoked.</para>
		/// </remarks>
		/// <param name="entity">An entity that has been reused.</param>
		/// <param name="parameters">A set of new parameters that are applied to the entity.</param>
		/// <param name="previousId">Previous identifier of the reused entity.</param>
		public virtual void OnEntityReused(CryEntity entity, ref EntitySpawnParameters parameters,
			EntityId previousId)
		{
		}
		[UnmanagedThunk("Invokes OnEntityRemoved.")]
		private void OnEntityReusedInternal(CryEntity entity, ref EntitySpawnParameters parameters,
			EntityId previousId)
		{
			this.OnEntityReused(entity, ref parameters, previousId);
		}
		/// <summary>
		/// Sends a message to other clients or server.
		/// </summary>
		/// <remarks>
		/// This default implementation doesn't do anything.
		/// </remarks>
		/// <param name="type">Type of the message to send.</param>
		/// <param name="message">Text of the message.</param>
		/// <param name="where">A value that specifies where to send the message.</param>
		/// <param name="channel">Identifier of the channel that is used to communicate with a client this message has to be sent to, if <paramref name="where"/> argument has a flag <see cref="RmiTarget.ToClientChannel"/> set.</param>
		public virtual void SendTextMessage(TextMessageType type, string message, RmiTarget where,
			ChannelId channel)
		{
		}
		[UnmanagedThunk("Invokes SendTextMessage.")]
		private void SendTextMessageInternal(TextMessageType type, string message, RmiTarget where, int channel)
		{
			this.SendTextMessage(type, message, where, channel);
		}
		/// <summary>
		/// Sends a chat message to other clients.
		/// </summary>
		/// <remarks>
		/// This default implementation doesn't do anything.
		/// </remarks>
		/// <param name="type">Type of the message to send.</param>
		/// <param name="source">An entity that sent the message.</param>
		/// <param name="message">Text of the message.</param>
		/// <param name="target">An entity that must receive the message. Only needs to be specified when <paramref name="type"/> is equal to <see cref="ChatMessageType.Target"/>.</param>
		public virtual void SendChatMessage(ChatMessageType type, EntityId source, string message,
			EntityId target = new EntityId())
		{
		}
		[UnmanagedThunk("Invokes SendTextMessage.")]
		private void SendChatMessageInternal(ChatMessageType type, EntityId source, EntityId target, string message)
		{
			this.SendChatMessage(type, source, message, target);
		}
		/// <summary>
		/// Notifies about a collision between to physical entities that are hosted by CryEngine entities.
		/// </summary>
		/// <remarks>
		/// This default implementation returns <c>true</c>.
		/// </remarks>
		/// <param name="info">Reference to the object that provides all information about the collision.</param>
		/// <returns>True, if breakage and material effects can be spawned.</returns>
		public virtual bool OnCollision(ref GameCollisionInfo info)
		{
			return true;
		}
		[UnmanagedThunk("Invokes OnCollision.")]
		private bool OnCollisionInternal(ref GameCollisionInfo info)
		{
			return this.OnCollision(ref info);
		}
		/// <summary>
		/// Invoked when "status" console command is used in a game that doesn't utilize CryTek's matchmaking implementation.
		/// </summary>
		/// <remarks>
		/// This default implementation doesn't do anything.
		/// </remarks>
		public virtual void ShowStatus( )
		{
		}
		[UnmanagedThunk("Invokes ShowStatus.")]
		private void ShowStatusInternal( )
		{
			this.ShowStatus();
		}
		/// <summary>
		/// Indicates whether this game mode has a time limit.
		/// </summary>
		/// <remarks>
		/// This default implementation returns <c>false</c>.
		/// </remarks>
		/// <returns>True, if this game mode has a time limit.</returns>
		public virtual bool IsTimeLimited( )
		{
			return false;
		}
		[UnmanagedThunk("Invokes IsTimeLimited.")]
		private bool IsTimeLimitedInternal( )
		{
			return this.IsTimeLimited();
		}
		#endregion
		#region Utilities
		#endregion
	}
}