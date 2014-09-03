using System;
using System.IO;
using System.Reflection;

using CryEngine.Serialization;
using CryEngine.Extensions;
using CryEngine.Native;

namespace CryEngine
{
	public static class Game
	{
		/// <summary>
		/// Initializes static variables which never change, i.e. IsEditor.
		/// </summary>
		/// <param name="isEditor"></param>
		/// <param name="isDedicated"></param>
		internal static void InitializeGameStatics(bool isEditor, bool isDedicated)
		{
			IsEditor = isEditor;
			IsDedicated = isDedicated;
		}

		#region Properties
		/// <summary>
		/// Gets a boolean indicating whether we're currently running a Multiplayer game.
		/// </summary>
		public static bool IsMultiplayer { get { return NetworkInterop.IsMultiplayer(); } }
		/// <summary>
		/// Gets a boolean indicating whether we're currently running the server.
		/// </summary>
		/// <remarks>
		/// <see cref="IsClient" /> can be set at th same time as <see cref="IsServer" />, if
		/// hosting via the Launcher or Editor.
		/// </remarks>
		public static bool IsServer { get { return NetworkInterop.IsServer(); } }
		/// <summary>
		/// Gets a boolean indicating whether we're currently running the server.
		/// </summary>
		/// <remarks>
		/// <see cref="IsServer" /> can be set at th same time as <see cref="IsClient" />, if
		/// hosting via the Launcher or Editor.
		/// </remarks>
		public static bool IsClient { get { return NetworkInterop.IsClient(); } }
		/// <summary>
		/// Gets a boolean indicating whether we're a client on a remote server.
		/// </summary>
		public static bool IsPureClient { get { return NetworkInterop.IsClient() && !NetworkInterop.IsServer(); } }

		/// <summary>
		/// Gets a boolean indicating whether we're currently running via the Editor.
		/// </summary>
		public static bool IsEditor { private set; get; }
		/// <summary>
		/// Gets a boolean indicating whether we're currently running via the Dedicated Server.
		/// </summary>
		public static bool IsDedicated { private set; get; }
		#endregion
	}
}