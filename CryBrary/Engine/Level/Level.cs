﻿using System;
using System.Collections.Generic;

using System.Linq;

using System.Runtime.CompilerServices;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Represents a CryENGINE level.
	/// </summary>
	public class Level
	{
		#region Statics
		private static readonly List<Level> Levels = new List<Level>();

		internal static Level TryGet(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var level = Levels.FirstOrDefault(x => x.Handle == ptr);
			if (level != null)
				return level;

			level = new Level(ptr);
			Levels.Add(level);

			return level;
		}

		/// <summary>
		/// Gets the currently loaded level
		/// </summary>
		public static Level Current { get { return TryGet(LevelInterop.GetCurrentLevel()); } }

		/// <summary>
		/// Loads a new level and returns its level info
		/// </summary>
		/// <param name="name"></param>
		/// <returns>The loaded level</returns>
		public static Level Load(string name)
		{
			return TryGet(LevelInterop.LoadLevel(name));
		}

		/// <summary>
		/// Unloads the currently loaded level.
		/// </summary>
		public static void Unload()
		{
			LevelInterop.UnloadLevel();
		}

		/// <summary>
		/// Gets a value indicating whether a level is currently loaded.
		/// </summary>
		public static bool Loaded { get { return LevelInterop.IsLevelLoaded(); } }
		#endregion

		internal Level(IntPtr ptr)
		{
			Handle = ptr;
		}

		/// <summary>
		/// Gets the level name.
		/// </summary>
		public string Name { get { return LevelInterop.GetName(Handle); } }

		/// <summary>
		/// Gets the level display name.
		/// </summary>
		public string DisplayName { get { return LevelInterop.GetDisplayName(Handle); } }

		/// <summary>
		/// Gets the full path to the directory this level resides in.
		/// </summary>
		public string Path { get { return LevelInterop.GetName(Handle); } }

		/// <summary>
		/// Gets the height map size for this level.
		/// </summary>
		public int HeightmapSize { get { return LevelInterop.GetHeightmapSize(Handle); } }

		/// <summary>
		/// Gets the number of supported game rules for this level.
		/// </summary>
		public int SupportedGamerules { get { return LevelInterop.GetGameTypeCount(Handle); } }

		/// <summary>
		/// Gets the default game mode for this level.
		/// </summary>
		public string DefaultGameRules { get { return LevelInterop.GetDefaultGameType(Handle); } }

		/// <summary>
		/// Gets a value indicating whether the level is configured to support any game
		/// rules.
		/// </summary>
		public bool HasGameRules { get { return LevelInterop.HasGameRules(Handle); } }

		internal IntPtr Handle { get; set; }

		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj is Level)
				return this == obj;

			return false;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 29 + Handle.GetHashCode();

				return hash;
			}
		}
		#endregion

		/// <summary>
		/// Gets the supported game rules at the index; see SupportedGamerules.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Name of the supported gamemode</returns>
		public string GetSupportedGameRules(int index)
		{
			return LevelInterop.GetGameType(Handle, index);
		}

		/// <summary>
		/// Returns true if this level supports the specific game rules.
		/// </summary>
		/// <param name="gamemodeName"></param>
		/// <returns>
		/// A boolean indicating whether the specified gamemode is supported.
		/// </returns>
		public bool SupportsGameRules(string gamemodeName)
		{
			return LevelInterop.SupportsGameType(Handle, gamemodeName);
		}
	}
}