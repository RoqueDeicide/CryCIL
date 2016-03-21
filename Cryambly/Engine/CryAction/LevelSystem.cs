﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Provides access to CryEngine level system.
	/// </summary>
	public static class LevelSystem
	{
		#region Fields
		/// <summary>
		/// Provides access to the list of levels CryEngine is currently aware of.
		/// </summary>
		public static readonly Levels Levels = new Levels();
		#endregion
		#region Properties
		/// <summary>
		/// Gets the wrapper for a currently loaded level. Null will be returned if no level is loaded.
		/// </summary>
		public static extern Level Current { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether a level is currently loaded.
		/// </summary>
		public static extern bool Loaded { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the time it took to load the last (current, if it's not unloaded) level.
		/// </summary>
		public static extern TimeSpan LastLoadTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		#endregion
		#region Events
		/// <summary>
		/// Occurs when someone tries loading a level that could not be found.
		/// </summary>
		/// <remarks>
		/// The first and only parameter of this event is a name of the level attempt to load which was
		/// made.
		/// </remarks>
		public static event Action<string> LevelNotFound;
		/// <summary>
		/// Occurs when loading of the level starts.
		/// </summary>
		/// <remarks>
		/// The first and only parameter of this event is wrapper object that represents the level that is
		/// being loaded.
		/// </remarks>
		public static event Action<Level> LoadingStart;
		/// <summary>
		/// Occurs during loading of the level when the engine starts loading entities.
		/// </summary>
		/// <remarks>
		/// The first and only parameter of this event is wrapper object that represents the level that is
		/// being loaded.
		/// </remarks>
		public static event Action<Level> LoadingEntitiesStart;
		/// <summary>
		/// Occurs when loading of the level ends.
		/// </summary>
		/// <remarks>
		/// The first and only parameter of this event is wrapper object that represents the level that has
		/// been loaded.
		/// </remarks>
		public static event Action<Level> LoadingComplete;
		/// <summary>
		/// Occurs when error is thrown during level load process.
		/// </summary>
		/// <remarks>
		/// <para>Event parameters:</para>
		/// <list type="number">
		/// <item>
		/// <description>
		/// level - a wrapper that represents the level during the load of which the error has occurred.
		/// </description>
		/// </item>
		/// <item>
		/// <description>error - text message that describes the error.</description>
		/// </item>
		/// </list>
		/// </remarks>
		public static event Action<Level, string> LoadingError;
		/// <summary>
		/// Occurs when loading of the level progresses.
		/// </summary>
		/// <remarks>
		/// <para>Event parameters:</para>
		/// <list type="number">
		/// <item>
		/// <description>level - a wrapper that represents the level that is being loaded.</description>
		/// </item>
		/// <item>
		/// <description>
		/// progress - ratio of number of already loaded object to total number of objects (probably).
		/// </description>
		/// </item>
		/// </list>
		/// </remarks>
		public static event Action<Level, int> LoadingProgress;
		/// <summary>
		/// Occurs when level is unloaded.
		/// </summary>
		/// <remarks>
		/// The first and only parameter of this event is wrapper object that represents the level that has
		/// been unloaded.
		/// </remarks>
		public static event Action<Level> UnloadComplete;
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Unloads currently loaded level.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Unload();
		/// <summary>
		/// Loads the level.
		/// </summary>
		/// <param name="name">Name of the level to load.</param>
		/// <returns>Information about the loaded level.</returns>
		/// <exception cref="ArgumentNullException">Name of the level to load cannot be null.</exception>
		public static Level Load(string name)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "Name of the level to load cannot be null.");
			}

			return LoadInternal(name);
		}
		/// <summary>
		/// Prepares loading of the next level.
		/// </summary>
		/// <remarks>Call this before calling <see cref="Load"/>.</remarks>
		/// <param name="name">Name of the next level.</param>
		/// <exception cref="ArgumentNullException">Name of the level to prepare cannot be null.</exception>
		public static void Prepare(string name)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "Name of the level to prepare cannot be null.");
			}

			PrepareInternal(name);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Level LoadInternal(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrepareInternal(string name);

		[RuntimeInvoke("Invoked by underlying framework to raise LevelNotFound event")]
		private static void OnLevelNotFound(string name)
		{
			LevelNotFound?.Invoke(name);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise LoadingStart event")]
		private static void OnLoadingStart(Level levelHandle)
		{
			LoadingStart?.Invoke(levelHandle);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise LoadingEntitiesStart event")]
		private static void OnLoadingEntitiesStart(Level levelHandle)
		{
			LoadingEntitiesStart?.Invoke(levelHandle);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise LoadingComplete event")]
		private static void OnLoadingComplete(Level levelHandle)
		{
			LoadingComplete?.Invoke(levelHandle);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise LevelNotFound event")]
		private static void OnLoadingError(Level levelHandle, string error)
		{
			LoadingError?.Invoke(levelHandle, error);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise LoadingProgress event")]
		private static void OnLoadingProgress(Level levelHandle, int progress)
		{
			LoadingProgress?.Invoke(levelHandle, progress);
		}
		[RuntimeInvoke("Invoked by underlying framework to raise UnloadComplete event")]
		private static void OnUnloadComplete(Level levelHandle)
		{
			UnloadComplete?.Invoke(levelHandle);
		}
		#endregion
	}
}