using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.DebugServices;
using CryCil.RunTime;

namespace CryCil
{
	/// <summary>
	/// Defines some variables that describe this game instance.
	/// </summary>
	[InitializationClass]
	public static class Game
	{
		private static string name;
		private static string longName;
		private static string guidText;
		private static Guid guid;

		/// <summary>
		/// Gets the name of this game.
		/// </summary>
		public static string Name
		{
			get { return name; }
		}
		/// <summary>
		/// Gets the long name of this game.
		/// </summary>
		public static string LongName
		{
			get { return longName; }
		}
		/// <summary>
		/// Gets the Globally Unique Identifier of this game.
		/// </summary>
		public static Guid Guid
		{
			get { return guid; }
		}

		/// <summary>
		/// Indicates whether this game was launched using Sandbox Editor.
		/// </summary>
		public static extern bool IsEditor { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether this game was launched using Sandbox Editor and the user is currently outside
		/// of game mode.
		/// </summary>
		public static extern bool IsEditing { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether we are currently in a multiplayer game mode.
		/// </summary>
		public static extern bool IsMultiplayer { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether our game instance can receive messages from clients.
		/// </summary>
		public static extern bool IsServer { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether our game instance can receive messages from a server.
		/// </summary>
		public static extern bool IsClient { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether our game instance is a dedicated server.
		/// </summary>
		public static extern bool IsDedicatedServer { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether an FMV sequence is currently playing.
		/// </summary>
		public static extern bool IsFullMotionVideoPlaying { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether a cut-scene is currently playing.
		/// </summary>
		public static extern bool IsCutscenePlaying { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[InitializationStage(0)]
		private static void AssignNamesAndGuid(int stageIndex)
		{
			name = "CryCilGame";
			longName = "CryCIL Base Game";
			guidText = "{00000000-1111-2222-3333-444444444444}";
			guid = Guid.Parse(guidText);

			var names = from assembly in MonoInterface.CryCilAssemblies
						from attribute in assembly.GetAttributes<GameNameAttribute>()
						where !attribute.Name.IsNullOrWhiteSpace()
						select attribute;

			var namesArray = names.ToArray();
			if (namesArray.Length == 0)
			{
				Log.Error(true,
						  "No valid short name was found for this game. Default name \"{0}\" will be used.",
						  name);
			}
			if (namesArray.Length != 1)
			{
				Log.Error(false,
						  "Too many short names were found for this game. \"{0}\", the first one, will be used.",
						  namesArray[0]);
			}
			name = namesArray[0].Name;

			var longNames = from assembly in MonoInterface.CryCilAssemblies
							from attribute in assembly.GetAttributes<GameLongNameAttribute>()
							where !attribute.Name.IsNullOrWhiteSpace()
							select attribute;

			var longNamesArray = longNames.ToArray();
			if (longNamesArray.Length == 0)
			{
				Log.Error(true,
						  "No valid long name was found for this game. Default name \"{0}\" will be used.",
						  longName);
			}
			if (longNamesArray.Length != 1)
			{
				Log.Error(false,
						  "Too many long names were found for this game. \"{0}\", the first one, will be used.",
						  longNamesArray[0]);
			}
			longName = longNamesArray[0].Name;

			var guids = from assembly in MonoInterface.CryCilAssemblies
						from attribute in assembly.GetAttributes<GameGuidAttribute>()
						where attribute.Guid != Guid.Empty
						select attribute;

			var guidsArray = guids.ToArray();
			if (guidsArray.Length == 0)
			{
				Log.Error(true,
						  "No valid GUID was found for this game. Default one \"{0}\" will be used.",
						  guid);
			}
			if (guidsArray.Length != 1)
			{
				Log.Error(false,
						  "Too many GUIDs were found for this game. \"{0}\", the first one, will be used.",
						  guidsArray[0]);
			}
			guid = guidsArray[0].Guid;
			guidText = guid.ToString("B");
		}
	}
	/// <summary>
	/// Gives a name to the game.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class GameNameAttribute : Attribute
	{
		/// <summary>
		/// Gets the short name of the game.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="name">Short name of the game.</param>
		public GameNameAttribute(string name)
		{
			this.Name = name;
		}
	}
	/// <summary>
	/// Gives a long name to the game.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class GameLongNameAttribute : Attribute
	{
		/// <summary>
		/// Gets the long name of the game.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="name">Long name of the game.</param>
		public GameLongNameAttribute(string name)
		{
			this.Name = name;
		}
	}
	/// <summary>
	/// Gives a Globally Unique Identifier to the game.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class GameGuidAttribute : Attribute
	{
		/// <summary>
		/// Gets the long name of the game.
		/// </summary>
		public Guid Guid { get; private set; }
		/// <summary>
		/// Creates a new instance of this class.
		/// </summary>
		/// <param name="guid">GUID to assign to the game.</param>
		public GameGuidAttribute(string guid)
		{
			Guid g = new Guid();
			this.Guid = g;
			if (Guid.TryParse(guid, out g))
			{
				this.Guid = g;
			}
		}
	}
}