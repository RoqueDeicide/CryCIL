using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Data;
using CryCil.Engine.DebugServices;
using CryCil.Engine.Network;
using CryCil.RunTime;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Base class for all objects that define logic for CryEngine entities that are bound to and
	/// synchronized via network.
	/// </summary>
	public abstract class MonoNetEntity : MonoEntity
	{
		#region Fields
		[UsedImplicitly]
		private ChannelId channelId;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets identifier of the channel this entity uses to interact with other entities in the
		/// network.
		/// </summary>
		public ChannelId ChannelId
		{
			get { return channelId; }
			set
			{
				SetChannelId(this.Id, value);
				// The channerlId field will be set by underlying framework.
			}
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when the entity is informed that a mirroring entity is about to be initialized on the
		/// client-side.
		/// </summary>
		/// <remarks>
		/// First parameter is an entity object for which the event was raised and second parameter is
		/// identifier of channel that can be used to communicate with the client.
		/// </remarks>
		public event Action<object, ChannelId> ClientInitializing;
		/// <summary>
		/// Occurs when the entity is informed that a mirroring entity was initialized on the client-side.
		/// </summary>
		/// <remarks>
		/// First parameter is an entity object for which the event was raised and second parameter is
		/// identifier of channel that can be used to communicate with the client.
		/// </remarks>
		public event Action<object, ChannelId> ClientInitialized;
		#endregion
		#region Construction
		/// <summary>
		/// Delegates initialization to the constructor of the base class.
		/// </summary>
		/// <param name="handle">Entity handle that is passed to the base constructor.</param>
		/// <param name="id">    Entity id that is passed to the base constructor.</param>
		protected MonoNetEntity(CryEntity handle, EntityId id)
			: base(handle, id)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Reacts to initialization of the client.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public abstract void InitializeClient(ChannelId id);
		/// <summary>
		/// Reacts to end of initialization of the client.
		/// </summary>
		/// <param name="id">
		/// Identifier of the network channel that is used to communicate with the client.
		/// </param>
		public abstract void PostInitializeClient(ChannelId id);
		/// <summary>
		/// Synchronizes the state of this entity with its representatives on other machines over network.
		/// </summary>
		/// <param name="sync">   Object that handles synchronization.</param>
		/// <param name="aspect"> Designates synchronized aspect.</param>
		/// <param name="profile">Unknown.</param>
		/// <param name="pflags"> Physics flags(?).</param>
		/// <returns>True, if synchronization was successful.</returns>
		public abstract bool SynchronizeWithNetwork(CrySync sync, EntityAspects aspect, byte profile, int pflags);
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked from underlying object to raise the event ClientInitializing.")]
		private void OnClientInitializing(ChannelId id)
		{
			this.InitializeClient(id);
			if (this.ClientInitializing != null) this.ClientInitializing(this, id);
		}
		[UnmanagedThunk("Invoked from underlying object to raise the event ClientInitialized.")]
		private void OnClientInitialized(ChannelId id)
		{
			this.PostInitializeClient(id);
			if (this.ClientInitialized != null) this.ClientInitialized(this, id);
		}
		[UnmanagedThunk("Invoked from underlying object to invoke SynchronizeWithNetwork method.")]
		private bool NetSyncInternal(CrySync sync, EntityAspects aspect, byte profile, int pflags)
		{
			return this.SynchronizeWithNetwork(sync, aspect, profile, pflags);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetChannelId(EntityId entityId, ChannelId channelId);
		[UnmanagedThunk("Invoked from underlying object when it receives a request to invoke RMI method from somewhere " +
						"else.")]
		private bool ReceiveRmi(string methodName, RmiParameters parameters)
		{
			Type type = this.GetType();
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null)
			{
				Log.Error(string.Format("A request to invoke the method {0} of type {1} has been received from remote " +
										  "location, but the method is not defined.", methodName, type.FullName), true);
				return false;
			}
			RMIAttribute attribute = method.GetAttribute<RMIAttribute>();
			if (attribute == null)
			{
				Log.Warning(string.Format("A request to invoke the method {0} of type {1} has been received from remote " +
										  "location, but the method is not allowed to be invoked remotely.",
										  method.Name, type.FullName), false);
				return false;
			}
			if (attribute.ToServer && !Game.IsServer)
			{
				Log.Warning(string.Format("A request to invoke the method {0} of type {1} has been received from remote " +
										  "location, but the method is supposed to be invoked from the client to the server " +
										  "and this game instance is not a server.",
										  method.Name, type.FullName), false);
				return false;
			}
			if (!attribute.ToServer && !Game.IsClient)
			{
				Log.Warning(string.Format("A request to invoke the method {0} of type {1} has been received from remote " +
										  "location, but the method is supposed to be invoked from the server to the client " +
										  "and this game instance is not a client.",
										  method.Name, type.FullName), false);
				return false;
			}
			try
			{
				return (bool)method.Invoke(this, parameters != null ? new object[] {parameters} : null);
			}
			catch (ArgumentException ex)
			{
				if (attribute.Reliable)
				{
					var pars = method.GetParameters();
					string message;
					if (parameters == null)
					{
						message =
						string.Format("Method {0} of type {1} accepts parameters, but it was invoked with no arguments.",
									  method.Name, type.FullName);
					}
					else if (pars.Length == 0)
					{
						message =
						string.Format("Method {0} of type {1} doesn't accept any parameters, but was invoked with one.",
									  method.Name, type.FullName);
					}
					else
					{
						message =
						string.Format("Method {0} of type {1} must only accept one parameter of type {2}, " +
									  "but it actually accepts {3} parameters of types {4}.",
									  method.Name, type.FullName, parameters.GetType().FullName, pars.Length,
									  pars.Select(t => t.ParameterType.FullName).ContentsToString(", "));
					}
					RmiException rex = new RmiException(message, ex);
					MonoInterface.DisplayException(rex);
				}
				return false;
			}
			catch (Exception ex)
			{
				if (attribute.Reliable)
				{
					string message =
						string.Format("An error has occurred when invoking method {0} of type {1} via RMI mechanism.",
									  method.Name, type.FullName);
					RmiException rex = new RmiException(message, ex);
					MonoInterface.DisplayException(rex);
				}
				return false;
			}
		}
		#endregion
	}
}