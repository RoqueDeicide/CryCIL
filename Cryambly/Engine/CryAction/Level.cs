using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents standard CryEngine level.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Level
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the collection of game types this level supports.
		/// </summary>
		[FieldOffset(0)] public LevelMissions Missions;
		/// <summary>
		/// Provides access to the collection of game modes this level supports.
		/// </summary>
		[FieldOffset(0)] public LevelGameModes GameModes;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets the name of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Name
		{
			get
			{
				this.AssertInstance();

				return GetName(this.handle);
			}
		}
		/// <summary>
		/// Gets the localized name of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string DisplayName
		{
			get
			{
				this.AssertInstance();

				return GetDisplayName(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Path
		{
			get
			{
				this.AssertInstance();

				return GetPath(this.handle);
			}
		}
		/// <summary>
		/// Gets the wildcard path that can be used to identify the .pak files that a part of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Paks
		{
			get
			{
				this.AssertInstance();

				return GetPaks(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this level is from a CryEngine standard mod.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool IsFromMod
		{
			get
			{
				this.AssertInstance();

				return GetIsModLevel(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the preview image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string PreviewPath
		{
			get
			{
				this.AssertInstance();

				return GetPreviewImagePath(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the background image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string BackgroundPath
		{
			get
			{
				this.AssertInstance();

				return GetBackgroundImagePath(this.handle);
			}
		}
		/// <summary>
		/// Gets the path to the minimap image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string MinimapPath
		{
			get
			{
				this.AssertInstance();

				return GetMinimapImagePath(this.handle);
			}
		}
		/// <summary>
		/// Gets information about level's minimap.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public MinimapInfo Minimap
		{
			get
			{
				this.AssertInstance();

				return GetMinimapInfo(this.handle);
			}
		}
		/// <summary>
		/// Gets the size of height map cells in meters(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int HeightMapSize
		{
			get
			{
				this.AssertInstance();

				return GetHeightmapSize(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether metadata was loaded for this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool MetaDataLoaded
		{
			get
			{
				this.AssertInstance();

				return MetadataLoaded(this.handle);
			}
		}
		/// <summary>
		/// Gets the scan tag of this level(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint ScanTag
		{
			get
			{
				this.AssertInstance();

				return GetScanTag(this.handle);
			}
		}
		/// <summary>
		/// Gets the tag of this level(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public uint LevelTag
		{
			get
			{
				this.AssertInstance();

				return GetLevelTag(this.handle);
			}
		}
		#endregion
		#region Construction
		internal Level(IntPtr handle)
			: this()
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this level is of given type.
		/// </summary>
		/// <param name="typeName">Name of the type.</param>
		/// <returns>True, if the level is of that type.</returns>
		/// <remarks>
		/// Level types are special tags that can be used to categorize levels. For instance, you can
		/// designate the level as "SinglePlayerMenu" and have the game show special menu that is designed
		/// for single-player on that level.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">Name of the level type cannot be null.</exception>
		public bool IsOfType(string typeName)
		{
			this.AssertInstance();
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName", "Name of the level type cannot be null.");
			}

			return IsOfTypeInternal(this.handle, typeName);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsOfTypeInternal(IntPtr handle, string sType);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPaks(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetDisplayName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetPreviewImagePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetBackgroundImagePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetMinimapImagePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetHeightmapSize(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool MetadataLoaded(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsModLevel(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetScanTag(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint GetLevelTag(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetGameTypeCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetGameType(IntPtr handle, int gameType, out GameTypeInfo info);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SupportsGameType(IntPtr handle, string gameTypeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GetDefaultGameType(IntPtr handle, out GameTypeInfo info);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetGameRules(IntPtr handle, int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetGameRulesCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasGameRules(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetDefaultGameRules(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MinimapInfo GetMinimapInfo(IntPtr handle);
		#endregion
	}
}