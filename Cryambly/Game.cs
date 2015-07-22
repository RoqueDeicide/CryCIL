using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil
{
	/// <summary>
	/// Defines some variables that describe this game instance.
	/// </summary>
	public static class Game
	{
		/// <summary>
		/// Indicates whether this game was launched using Sandbox Editor.
		/// </summary>
		public static extern bool IsEditor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether this game was launched using Sandbox Editor and the user is currently outside of game mode.
		/// </summary>
		public static extern bool IsEditing
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether we are currently in a multiplayer game mode.
		/// </summary>
		public static extern bool IsMultiplayer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether our game instance can receive messages from clients.
		/// </summary>
		public static extern bool IsServer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether our game instance can receive messages from a server.
		/// </summary>
		public static extern bool IsClient
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether our game instance is a dedicated server.
		/// </summary>
		public static extern bool IsDedicatedServer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether an FMV sequence is currently playing.
		/// </summary>
		public static extern bool IsFullMotionVideoPlaying
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Indicates whether a cut-scene is currently playing.
		/// </summary>
		public static extern bool IsCutscenePlaying
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
