using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of targets for RMI calls.
	/// </summary>
	[Flags]
	public enum RmiTarget : uint
	{
		/// <summary>
		/// When set, specifies that RMI call must be directed to a specific client.
		/// </summary>
		/// <remarks>Must be called from a server.</remarks>
		ToClientChannel = 0x01,
		/// <summary>
		/// When set, specifies that RMI call must be directed to the object that called it in the first
		/// place.
		/// </summary>
		/// <remarks>Must be called from a server.</remarks>
		ToOwnClient = 0x02,
		/// <summary>
		/// When set, specifies that RMI call must be directed to all clients connected to the game except
		/// the one that calls the RMI.
		/// </summary>
		/// <remarks>Must be called from a server.</remarks>
		ToOtherClients = 0x04,
		/// <summary>
		/// When set, specifies that RMI call must be directed to all clients connected to the game.
		/// </summary>
		/// <remarks>
		/// Must be called from a server. Currently setting this flag is equivalent of setting
		/// <see cref="ToOtherClients"/>. In order to send RMI to all clients including one on a sender's
		/// game instance, you have to set <see cref="ToOwnClient"/> and either <see cref="ToAllClients"/>
		/// or <see cref="ToOtherClients"/>.
		/// </remarks>
		ToAllClients = 0x08,

		/// <summary>
		/// When set, specifies that RMI call must be directed to a server.
		/// </summary>
		/// <remarks>Must be called from a client.</remarks>
		ToServer = 0x100,

		/// <summary>
		/// When set, specifies that RMI call must not be directed to game instances (clients or server)
		/// that are connected to this one via local (LAN) connection.
		/// </summary>
		NoLocalCalls = 0x10000,
		/// <summary>
		/// When set, specifies that RMI call must not be directed to game instances (clients or server)
		/// that are connected to this one via remote (WAN) connection.
		/// </summary>
		NoRemoteCalls = 0x20000,
		/// <summary>
		/// This is a combination of <see cref="NoLocalCalls"/> and <see cref="NoRemoteCalls"/> flags that
		/// is going to cause RMI to not be sent anywhere.
		/// </summary>
		/// <remarks>
		/// <see cref="RmiException"/> object is thrown by <see cref="o:MonoNetEntity.CallRmi"/> when set.
		/// </remarks>
		NoCall = NoLocalCalls | NoRemoteCalls,

		/// <summary>
		/// When set, specifies that RMI call must be directed to clients that are connected to this game
		/// instance via remote (WAN) connection.
		/// </summary>
		ToRemoteClients = NoLocalCalls | ToAllClients
	}
}