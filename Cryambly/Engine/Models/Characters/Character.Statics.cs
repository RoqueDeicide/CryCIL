using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters
{
	// Character manager API.
	public unsafe partial struct Character
	{
		#region Properties
		/// <summary>
		/// Gets the object that encapsulates animation system statistics.
		/// </summary>
		public static CharacterManagerStatistics Statistics
		{
			get
			{
				CharacterManagerStatistics statistics;
				GetStatistics(out statistics);
				return statistics;
			}
		}
		/// <summary>
		/// Gets the array of loaded character models.
		/// </summary>
		[CanBeNull]
		public static DefaultSkeleton[] LoadedModels
		{
			get
			{
				uint count;
				GetLoadedModels(null, out count);
				if (count == 0)
				{
					return null;
				}

				DefaultSkeleton[] models = new DefaultSkeleton[count];
				fixed (DefaultSkeleton* modelsPtr = models)
				{
					GetLoadedModels(modelsPtr, out count);
				}

				return models;
			}
		}
		/// <summary>
		/// Gets the number of ticks that were accumulated by animation system.
		/// </summary>
		public static ulong TickCount
		{
			get { return NumFrameTicks(); }
		}
		/// <summary>
		/// Gets the number of ticks that were accumulated by sync functions.
		/// </summary>
		public static ulong SyncTickCount
		{
			get { return NumFrameSyncTicks(); }
		}
		/// <summary>
		/// Gets current number of characters.
		/// </summary>
		public static uint CharacterCount
		{
			get { return NumCharacters(); }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Finds and prefetches resources to make loading of a character model quicker.
		/// </summary>
		/// <param name="file"> Path to the file to prefetch.</param>
		/// <param name="flags">A set of flags that specifies how to prefetch the character.</param>
		/// <returns>True, if successful.</returns>
		public static bool PrefetchResources(string file, CharacterLoadingFlags flags)
		{
			return LoadAndLockResources(file, flags);
		}
		/// <summary>
		/// Changes residence state for render meshes that are used by the character.
		/// </summary>
		/// <param name="file">  Path to the character model to change state for.</param>
		/// <param name="lod">   Index of the LOD to change state for.</param>
		/// <param name="keep">  
		/// If true, then the meshes will be kept resident, otherwise they will be streamed out when not
		/// needed. Any call with <c>true</c> as this argument must be followed with call with <c>false</c>
		/// to prevent memory leaks.
		/// </param>
		/// <param name="urgent">
		/// Indicates whether operation needs to be executed with higher priority.
		/// </param>
		public static void StreamKeepCharacterResourcesResident(string file, int lod, bool keep, bool urgent = false)
		{
			StreamKeepCharacterResourcesResidentInternal(file, lod, keep, urgent);
		}
		/// <summary>
		/// Determines whether character resources are in memory.
		/// </summary>
		/// <param name="file">Path to the character model to check.</param>
		/// <param name="lod"> Index of the LOD to check.</param>
		/// <returns>True, if resources are in memory; false, if resources were streamed out.</returns>
		public static bool StreamHasCharacterResources(string file, int lod)
		{
			return StreamHasCharacterResourcesInternal(file, lod);
		}
		/// <summary>
		/// Removes resources that are currently loaded into the animation system.
		/// </summary>
		/// <param name="deleteEverything">
		/// Indicates whether even resources are currently referenced must be removed.
		/// </param>
		public static void RemoveResources(bool deleteEverything)
		{
			ClearResources(deleteEverything);
		}
		/// <summary>
		/// Reloads all character models.
		/// </summary>
		public static void ReloadAllModels()
		{
			ReloadAllModelsInternal();
		}
		/// <summary>
		/// Reloads all .chrparams files.
		/// </summary>
		public static void ReloadAllCharacterParameters()
		{
			ReloadAllCHRPARAMS();
		}
		/// <summary>
		/// Pre-loads the .dba file.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <param name="priority">Operation priority.</param>
		/// <returns>True, if successful.</returns>
		public static bool PreLoadDbaFile(string filePath, DbaStreamingPriority priority = DbaStreamingPriority.Normal)
		{
			return DBA_PreLoad(filePath, priority);
		}
		/// <summary>
		/// Locks/unlocks the .dba file.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <param name="status">  Indicates whether file should be locked.</param>
		/// <param name="priority">Operation priority.</param>
		/// <returns>True, if successful.</returns>
		public static bool ChangeLockStatusDbaFile(string filePath, bool status,
												   DbaStreamingPriority priority = DbaStreamingPriority.Normal)
		{
			return DBA_LockStatus(filePath, status, priority);
		}
		/// <summary>
		/// Locks the .dba file.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <param name="priority">Operation priority.</param>
		/// <returns>True, if successful.</returns>
		public static bool LockDbaFile(string filePath, DbaStreamingPriority priority = DbaStreamingPriority.Normal)
		{
			return DBA_LockStatus(filePath, true, priority);
		}
		/// <summary>
		/// Unlocks the .dba file.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <param name="priority">Operation priority.</param>
		/// <returns>True, if successful.</returns>
		public static bool UnlockDbaFile(string filePath,
										 DbaStreamingPriority priority = DbaStreamingPriority.Normal)
		{
			return DBA_LockStatus(filePath, false, priority);
		}
		/// <summary>
		/// Unloads the .dba file.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool UnloadDbaFile(string filePath)
		{
			return DBA_Unload(filePath);
		}
		/// <summary>
		/// Unloads all .dba files.
		/// </summary>
		/// <returns>True, if successful.</returns>
		public static bool UnloadAllDbaFiles()
		{
			return DBA_Unload_All();
		}
		/// <summary>
		/// Increments reference count to the .caf file making the system stream it in, if it wasn't loaded
		/// before.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool IncrementRefCountOnCafFile(string filePath)
		{
			return CAF_AddRef(new LowerCaseCrc32(filePath));
		}
		/// <summary>
		/// Increments reference count to the .caf file making the system stream it in, if it wasn't loaded
		/// before.
		/// </summary>
		/// <param name="filePathCrc">CRC32 hash of the path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool IncrementRefCountOnCafFile(LowerCaseCrc32 filePathCrc)
		{
			return CAF_AddRef(filePathCrc);
		}
		/// <summary>
		/// Determines whether a .caf file is currently loaded into memory.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <returns>True, if file is loaded.</returns>
		public static bool IsCafFileLoaded(string filePath)
		{
			return CAF_IsLoaded(new LowerCaseCrc32(filePath));
		}
		/// <summary>
		/// Determines whether a .caf file is currently loaded into memory.
		/// </summary>
		/// <param name="filePathCrc">CRC32 hash of the path to the file.</param>
		/// <returns>True, if file is loaded.</returns>
		public static bool IsCafFileLoaded(LowerCaseCrc32 filePathCrc)
		{
			return CAF_IsLoaded(filePathCrc);
		}
		/// <summary>
		/// Decrements reference count to the .caf file making the system stream it out, if it isn't needed
		/// anymore.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool DecrementRefCountOnCafFile(string filePath)
		{
			return CAF_Release(new LowerCaseCrc32(filePath));
		}
		/// <summary>
		/// Decrements reference count to the .caf file making the system stream it out, if it isn't needed
		/// anymore.
		/// </summary>
		/// <param name="filePathCrc">CRC32 hash of the path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool DecrementRefCountOnCafFile(LowerCaseCrc32 filePathCrc)
		{
			return CAF_Release(filePathCrc);
		}
		/// <summary>
		/// Loads the .caf file and stalls this thread until loading is done.
		/// </summary>
		/// <param name="filePath">Path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool LoadCafFileSynchronously(string filePath)
		{
			return CAF_LoadSynchronously(new LowerCaseCrc32(filePath));
		}
		/// <summary>
		/// Loads the .caf file and stalls this thread until loading is done.
		/// </summary>
		/// <param name="filePathCrc">CRC32 hash of the path to the file.</param>
		/// <returns>True, if successful.</returns>
		public static bool LoadCafFileSynchronously(LowerCaseCrc32 filePathCrc)
		{
			return CAF_LoadSynchronously(filePathCrc);
		}
		/// <summary>
		/// Loads the .lmg file and stalls this thread until loading is done.
		/// </summary>
		/// <param name="filePath">     Path to the file.</param>
		/// <param name="pAnimationSet">Unknown.</param>
		/// <returns>True, if successful.</returns>
		public static bool LoadLmgFileSynchronously(string filePath, AnimationSet pAnimationSet)
		{
			return LMG_LoadSynchronously(new LowerCaseCrc32(filePath), pAnimationSet);
		}
		/// <summary>
		/// Loads the .lmg file and stalls this thread until loading is done.
		/// </summary>
		/// <param name="filePathCrc">  CRC32 hash of the path to the file.</param>
		/// <param name="pAnimationSet">Unknown.</param>
		/// <returns>True, if successful.</returns>
		public static bool LoadLmgFileSynchronously(LowerCaseCrc32 filePathCrc, AnimationSet pAnimationSet)
		{
			return LMG_LoadSynchronously(filePathCrc, pAnimationSet);
		}
		/// <summary>
		/// Reloads given file.
		/// </summary>
		/// <param name="filePath">File to reload.</param>
		/// <returns>Result of reloading.</returns>
		public static ReloadCafResult ReloadCafFile(string filePath)
		{
			return ReloadCAF(filePath);
		}
		/// <summary>
		/// Reloads given file.
		/// </summary>
		/// <param name="filePath">File to reload.</param>
		/// <returns>Result of reloading.</returns>
		public static int ReloadLmg(string filePath)
		{
			return ReloadLMG(filePath);
		}
		/// <summary>
		/// Advances the animation system time by given number of ticks.
		/// </summary>
		/// <param name="ticks">Number of ticks to add.</param>
		public static void AddTicks(ulong ticks)
		{
			AddFrameTicks(ticks);
		}
		/// <summary>
		/// Advances the animation system time for synchronization tasks by given number of ticks.
		/// </summary>
		/// <param name="ticks">Number of ticks to add.</param>
		public static void AddSyncTicks(ulong ticks)
		{
			AddFrameSyncTicks(ticks);
		}
		/// <summary>
		/// Resets the internal animation system clock.
		/// </summary>
		public static void ResetTicks()
		{
			ResetFrameTicks();
		}
		/// <summary>
		/// Gets the number of character instances that use given model.
		/// </summary>
		/// <param name="model">Character model.</param>
		/// <returns>Number of character instances.</returns>
		public static uint GetNumberOfInstancesPerModel(DefaultSkeleton model)
		{
			return GetNumInstancesPerModel(model);
		}
		/// <summary>
		/// Gets a character instance from model(?).
		/// </summary>
		/// <param name="model">Model to get the character instance for.</param>
		/// <param name="num">  Unkown.</param>
		/// <returns>A character instance.</returns>
		public static Character GetCharacterInstanceFromModel(DefaultSkeleton model, uint num)
		{
			return GetICharInstanceFromModel(model, num);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetStatistics(out CharacterManagerStatistics rStats);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateInstance(string szFilename, CharacterLoadingFlags nLoadingFlags = 0);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr LoadModelSKEL(string szFilePath, CharacterLoadingFlags nLoadingFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr LoadModelSKIN(string szFilePath, CharacterLoadingFlags nLoadingFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LoadAndLockResources(string szFilePath, CharacterLoadingFlags nLoadingFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void StreamKeepCharacterResourcesResidentInternal(string szFilePath, int nLod, bool bKeep,
																				bool bUrgent);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool StreamHasCharacterResourcesInternal(string szFilePath, int nLod);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearResources(bool bForceCleanup);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLoadedModels(DefaultSkeleton* pIDefaultSkeletons, out uint nCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReloadAllModelsInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReloadAllCHRPARAMS();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DBA_PreLoad(string filepath, DbaStreamingPriority priority);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DBA_LockStatus(string filepath, bool status, DbaStreamingPriority priority);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DBA_Unload(string filepath);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DBA_Unload_All();

		// Adds a runtime reference to a CAF animation; if not loaded it starts streaming it
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CAF_AddRef(LowerCaseCrc32 filePathCRC);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CAF_IsLoaded(LowerCaseCrc32 filePathCRC);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CAF_Release(LowerCaseCrc32 filePathCRC);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CAF_LoadSynchronously(LowerCaseCrc32 filePathCRC);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool LMG_LoadSynchronously(LowerCaseCrc32 filePathCRC, AnimationSet pAnimationSet);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ReloadCafResult ReloadCAF(string szFilePathCAF);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ReloadLMG(string szFilePathCAF);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddFrameTicks(ulong nTicks);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddFrameSyncTicks(ulong nTicks);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetFrameTicks();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ulong NumFrameTicks();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ulong NumFrameSyncTicks();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint NumCharacters();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetNumInstancesPerModel(DefaultSkeleton rIDefaultSkeleton);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Character GetICharInstanceFromModel(DefaultSkeleton rIDefaultSkeleton, uint num);
		#endregion
	}
}