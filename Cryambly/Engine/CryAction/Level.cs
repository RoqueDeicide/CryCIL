using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents standard CryEngine level.
	/// </summary>
	public class Level
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the name of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string Name { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the localized name of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string DisplayName { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the path to the level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string Path { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the wildcard path that can be used to identify the .pak files that a part of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string Paks { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Indicates whether this level is from a CryEngine standard mod.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern bool IsFromMod { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the path to the preview image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string PreviewPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the path to the background image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string BackgroundPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the path to the minimap image of this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern string MinimapPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the array of missions defined in this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public LevelMissions Missions
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("This level object is not valid.");
				}
				return new LevelMissions(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this level is from a CryEngine standard mod.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public LevelGameRules GameRules
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new NullReferenceException("This level object is not valid.");
				}
				return new LevelGameRules(this.handle);
			}
		}
		/// <summary>
		/// Gets information about level's minimap.
		/// </summary>
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		public extern MinimapInfo Minimap { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new wrapper for a level.
		/// </summary>
		/// <param name="handle">Handle of the level.</param>
		internal Level(IntPtr handle)
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
		/// <exception cref="NullReferenceException">This level object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Name of the level type cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsOfType(string typeName);
		#endregion
	}
}