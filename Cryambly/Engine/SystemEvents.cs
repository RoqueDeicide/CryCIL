using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.RunTime;

namespace CryCil.Engine
{
	/// <summary>
	/// Defines a signature of methods that are capable of handling any system-wide events.
	/// </summary>
	/// <param name="id">    Identifier of the event.</param>
	/// <param name="wparam">
	/// An instance of <see cref="UIntPtr"/> type that contains first event-specific parameter.
	/// </param>
	/// <param name="lparam">
	/// An instance of <see cref="UIntPtr"/> type that contains second event-specific parameter.
	/// </param>
	public delegate void GeneralSystemEventHandler(SystemEventIds id, UIntPtr wparam, UIntPtr lparam);
	/// <summary>
	/// Defines a signature of methods that are capable of handling system-wide events that represent a
	/// change of an indicator.
	/// </summary>
	/// <param name="isTrue">Indicates whether indicator is now <c>true</c>.</param>
	public delegate void IndicativeSystemEventHandler(bool isTrue);
	/// <summary>
	/// Provides means of listening to CryEngine system-wide events.
	/// </summary>
	[InitializationClass]
	public static class SystemEvents
	{
		#region Fields
		#endregion
		#region Properties
		#endregion
		#region Events
		/// <summary>
		/// Occurs when any system-wide event is raised.
		/// </summary>
		public static event GeneralSystemEventHandler Event;
		/// <summary>
		/// Occurs when the editor switches to in-game mode ( <c>true</c>) or editing mode ( <c>false</c>).
		/// </summary>
		public static event IndicativeSystemEventHandler EditorGameModeChanged;
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities

		[RawThunk("Invokes corresponding events.")]
		private static void OnSystemEvent(SystemEventIds id, UIntPtr wparam, UIntPtr lparam)
		{
			try
			{
				OnEvent(id, wparam, lparam);

				switch (id)
				{
					case SystemEventIds.RandomSeed:
						break;
					case SystemEventIds.RandomEnable:
						break;
					case SystemEventIds.ChangeFocus:
						break;
					case SystemEventIds.Move:
						break;
					case SystemEventIds.Resize:
						break;
					case SystemEventIds.Activate:
						break;
					case SystemEventIds.PositionChanged:
						break;
					case SystemEventIds.StyleChanged:
						break;
					case SystemEventIds.LevelLoadStartPreLoadingScreen:
						break;
					case SystemEventIds.LevelLoadResumeGame:
						break;
					case SystemEventIds.LevelLoadPrepare:
						break;
					case SystemEventIds.LevelLoadStartLoadingScreen:
						break;
					case SystemEventIds.LevelLoadLoadingScreenActive:
						break;
					case SystemEventIds.LevelLoadStart:
						break;
					case SystemEventIds.LevelLoadEnd:
						break;
					case SystemEventIds.LevelLoadError:
						break;
					case SystemEventIds.LevelNotReady:
						break;
					case SystemEventIds.LevelPrecacheStart:
						break;
					case SystemEventIds.LevelPrecacheFirstFrame:
						break;
					case SystemEventIds.LevelGameplayStart:
						break;
					case SystemEventIds.LevelUnload:
						break;
					case SystemEventIds.LevelPostUnload:
						break;
					case SystemEventIds.GamePostInit:
						break;
					case SystemEventIds.GamePostInitDone:
						break;
					case SystemEventIds.FullShutdown:
						break;
					case SystemEventIds.FastShutdown:
						break;
					case SystemEventIds.LanguageChange:
						break;
					case SystemEventIds.ToggleFullscreen:
						break;
					case SystemEventIds.ShareShaderCombinations:
						break;
					case SystemEventIds.Post3DRenderingStart:
						break;
					case SystemEventIds.Post3DRenderingEnd:
						break;
					case SystemEventIds.SwitchingToLevelHeap:
						break;
					case SystemEventIds.SwitchedToLevelHeap:
						break;
					case SystemEventIds.SwitchingToGlobalHeap:
						break;
					case SystemEventIds.SwitchedToGlobalHeap:
						break;
					case SystemEventIds.LevelPrecacheEnd:
						break;
					case SystemEventIds.GameModeSwitchStart:
						break;
					case SystemEventIds.GameModeSwitchEnd:
						break;
					case SystemEventIds.Video:
						break;
					case SystemEventIds.GamePaused:
						break;
					case SystemEventIds.GameResumed:
						break;
					case SystemEventIds.TimeOfDaySet:
						break;
					case SystemEventIds.EditorOnInitialization:
						break;
					case SystemEventIds.FrontendInitialised:
						break;
					case SystemEventIds.EditorGameModeChanged:
						OnEditorGameModeChanged(wparam != UIntPtr.Zero);
						break;
					case SystemEventIds.EditorSimulationModeChanged:
						break;
					case SystemEventIds.FrontendReloaded:
						break;
					case SystemEventIds.SwForceLoadStart:
						break;
					case SystemEventIds.SwForceLoadEnd:
						break;
					case SystemEventIds.SwShiftWorld:
						break;
					case SystemEventIds.StreamingInstallError:
						break;
					case SystemEventIds.OnlineServicesInitialised:
						break;
					case SystemEventIds.AudioImplementationLoaded:
						break;
					case SystemEventIds.User:
						break;
					case SystemEventIds.BeamPlayerToCameraPos:
						break;
					default:
						throw new ArgumentOutOfRangeException("id", id, null);
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}

		private static void OnEvent(SystemEventIds id, UIntPtr wparam, UIntPtr lparam)
		{
			var handler = Event;
			if (handler != null) handler(id, wparam, lparam);
		}
		private static void OnEditorGameModeChanged(bool istrue)
		{
			var handler = EditorGameModeChanged;
			if (handler != null) handler(istrue);
		}
		#endregion
	}
	/// <summary>
	/// Enumeration of identifiers of different system-wide events.
	/// </summary>
	public enum SystemEventIds
	{
		/// <summary>
		/// Occurs when a seed number must be assigned to RNGs. wparam holds a seed value.
		/// </summary>
		RandomSeed = 1,
		/// <summary>
		/// Unknown.
		/// </summary>
		RandomEnable,
		/// <summary>
		/// Unknown.
		/// </summary>
		/// <summary>
		/// Occurs when the main window gains/looses focus. wparam is not equal to 0, if focused.
		/// </summary>
		ChangeFocus = 10,
		/// <summary>
		/// Occurs when the main window changes is position. wparam = X, lparam = Y.
		/// </summary>
		Move = 11,
		/// <summary>
		/// Occurs when the main window changes is size. wparam = width, lparam = height.
		/// </summary>
		Resize = 12,
		/// <summary>
		/// Occurs when the main window (de)activates. wparam == 1 if active, 0 if not active.
		/// </summary>
		Activate = 13,
		/// <summary>
		/// Occurs when the main window's position changes.
		/// </summary>
		PositionChanged = 14,
		/// <summary>
		/// Occurs when the main window's style changes.
		/// </summary>
		StyleChanged = 15,
		/// <summary>
		/// Occurs before the loading cutscene during loading of the level.
		/// </summary>
		LevelLoadStartPreLoadingScreen,
		/// <summary>
		/// Occurs when before loading in the save game.
		/// </summary>
		LevelLoadResumeGame,
		/// <summary>
		/// Occurs before starting the level: before game rules initialization and before
		/// <see cref="LevelLoadStart"/> event occurs.
		/// </summary>
		LevelLoadPrepare,
		/// <summary>
		/// Occurs when rendering of the loading screen starts during loading of the level.
		/// </summary>
		LevelLoadStartLoadingScreen,
		/// <summary>
		/// Occurs when loading screen activates.
		/// </summary>
		LevelLoadLoadingScreenActive,
		/// <summary>
		/// Occurs before loading of the level starts.
		/// </summary>
		LevelLoadStart,
		/// <summary>
		/// Occurs when the loading of the level ends.
		/// </summary>
		LevelLoadEnd,
		/// <summary>
		/// Occurs when loading of the level fails.
		/// </summary>
		LevelLoadError,
		/// <summary>
		/// Occurs when the level cannot be loaded, because it hasn't been downloaded yet.
		/// </summary>
		LevelNotReady,
		/// <summary>
		/// Occurs after precaching of the streaming system has been done.
		/// </summary>
		LevelPrecacheStart,
		/// <summary>
		/// Occurs before object/texture precache stream requests are submitted.
		/// </summary>
		LevelPrecacheFirstFrame,
		/// <summary>
		/// Occurs when level loading is complete and game-play can start.
		/// </summary>
		LevelGameplayStart,
		/// <summary>
		/// Occurs when level is unloading.
		/// </summary>
		LevelUnload,
		/// <summary>
		/// Occurs when level finishes unloading.
		/// </summary>
		LevelPostUnload,
		/// <summary>
		/// Occurs when the game framework initializes.
		/// </summary>
		GamePostInit,
		/// <summary>
		/// Occurs when the game framework initialization is over.
		/// </summary>
		GamePostInitDone,
		/// <summary>
		/// Occurs when the system is going through the full shutdown procedure.
		/// </summary>
		FullShutdown,
		/// <summary>
		/// Occurs when the system is going through the fast shutdown procedure.
		/// </summary>
		FastShutdown,
		/// <summary>
		/// Occurs when keyboard language changes.
		/// </summary>
		LanguageChange,
		/// <summary>
		/// Occurs when full-screen is toggled. wparam equals to 0 when we switch from full-screen.
		/// </summary>
		ToggleFullscreen,
		/// <summary>
		/// Unknown.
		/// </summary>
		ShareShaderCombinations,
		/// <summary>
		/// Occurs at the start of the Post-3D Rendering.
		/// </summary>
		Post3DRenderingStart,
		/// <summary>
		/// Occurs at the end of the Post-3D Rendering.
		/// </summary>
		Post3DRenderingEnd,
		/// <summary>
		/// Occurs before switching to level memory heap.
		/// </summary>
		SwitchingToLevelHeap,
		/// <summary>
		/// Occurs after switching to level memory heap.
		/// </summary>
		SwitchedToLevelHeap,
		/// <summary>
		/// Occurs before switching to global memory heap.
		/// </summary>
		SwitchingToGlobalHeap,
		/// <summary>
		/// Occurs after switching to global memory heap.
		/// </summary>
		SwitchedToGlobalHeap,
		/// <summary>
		/// Occurs when precaching of the streaming system is over.
		/// </summary>
		LevelPrecacheEnd,
		/// <summary>
		/// Occurs at the start of the game mode switch in the editor.
		/// </summary>
		GameModeSwitchStart,
		/// <summary>
		/// Occurs at the end of the game mode switch in the editor.
		/// </summary>
		GameModeSwitchEnd,
		/// <summary>
		/// Occurs when the video playback status changes. wparam = [0/1/2/3] : [stop/play/pause/resume].
		/// </summary>
		Video,
		/// <summary>
		/// Occurs when the game is paused.
		/// </summary>
		GamePaused,
		/// <summary>
		/// Occurs when the game is resumed.
		/// </summary>
		GameResumed,
		/// <summary>
		/// Occurs when the time of day is set.
		/// </summary>
		TimeOfDaySet,
		/// <summary>
		/// Occurs when initialization of the editor is done.
		/// </summary>
		EditorOnInitialization,
		/// <summary>
		/// Occurs when initialization of the front-end is done.
		/// </summary>
		FrontendInitialised,
		/// <summary>
		/// Occurs when the Editor switches between in-game and editing mode. wparam != 0 when switching to
		/// in-game mode.
		/// </summary>
		EditorGameModeChanged,
		/// <summary>
		/// Occurs when the Editor toggles simulation mode (AI/Physics).
		/// </summary>
		EditorSimulationModeChanged,
		/// <summary>
		/// Occurs when front-end is reloaded.
		/// </summary>
		FrontendReloaded,
		/// <summary>
		/// Occurs when segmented world force-loads specified segments.
		/// </summary>
		SwForceLoadStart,
		/// <summary>
		/// Occurs after segmented world force-loads specified segments.
		/// </summary>
		SwForceLoadEnd,
		/// <summary>
		/// Occurs when segmented world manager shifts the world.
		/// </summary>
		SwShiftWorld,
		/// <summary>
		/// Currently durango only. Occurs when installation fails.
		/// </summary>
		StreamingInstallError,
		/// <summary>
		/// Occurs when the online services are initialized.
		/// </summary>
		OnlineServicesInitialised,
		/// <summary>
		/// Occurs after loading of the new audio implementation.
		/// </summary>
		AudioImplementationLoaded,

		/// <summary>
		/// User events are past this id.
		/// </summary>
		User = 0x1000,
		/// <summary>
		/// Unknown.
		/// </summary>
		BeamPlayerToCameraPos
	}
}