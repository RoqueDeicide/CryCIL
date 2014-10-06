using System;

namespace CryEngine.RunTime.Networking
{
	/// <summary>
	/// Marks methods that are invoked remotely.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class RemoteInvocationAttribute : Attribute
	{
	}
	/// <summary>
	/// Flags indicating what a remote invocation targets.
	/// </summary>
	[Flags]
	public enum NetworkTarget
	{
		/// <summary>
		/// Target a specific client channel.
		/// </summary>
		ToClientChannel = 0x01,
		/// <summary>
		/// Target the local client.
		/// </summary>
		ToOwnClient = 0x02,
		/// <summary>
		/// Target all clients but the local one.
		/// </summary>
		ToOtherClients = 0x04,
		/// <summary>
		/// Target all clients.
		/// </summary>
		ToAllClients = 0x08,
		/// <summary>
		/// Target the server.
		/// </summary>
		ToServer = 0x100,
		/// <summary>
		/// No local calls.
		/// </summary>
		NoLocalCalls = 0x10000,
		/// <summary>
		/// No remote calls
		/// </summary>
		NoRemoteCalls = 0x20000,
		/// <summary>
		/// To all remote clients
		/// </summary>
		ToRemoteClients = NoLocalCalls | ToAllClients
	}
}