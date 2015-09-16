using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Encapsulates description of a mission that is defined for the level.
	/// </summary>
	/// <remarks>
	/// Level missions are sets of scripts and entities that define specific game modes that can be played
	/// on a level.
	/// <para>
	/// Generally, it's a good idea to not create entirely separate level just to play CTF or FFA, instead
	/// common parts of the same level can be reused in multiple so called "missions".
	/// </para>
	/// <para>
	/// Each mission simply defines only entities and scripts that can be used on it, without intersecting
	/// with others.
	/// </para>
	/// <para>TODO: Validate above description.</para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct LevelMission
	{
		/// <summary>
		/// Name of the mission.
		/// </summary>
		public string Name;
		/// <summary>
		/// Path to the xml file that contains detailed description of the mission.
		/// </summary>
		public string XmlFile;
		/// <summary>
		/// Gets number of mission-specific cgf objects.
		/// </summary>
		public int ObjectCount;
	}
	/// <summary>
	/// Represents a collection of missions defined for the level.
	/// </summary>
	/// <remarks>
	/// Never use struct-specific default constructor with this type. Objects created like that are never
	/// valid.
	/// </remarks>
	public struct LevelMissions : IEnumerable<LevelMission>
	{
		#region Fields
		[UsedImplicitly] private IntPtr levelHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets information about the supported mission.
		/// </summary>
		/// <param name="index">Zero-based index of the mission details about which to get.</param>
		/// <returns>Information about the mission.</returns>
		/// <exception cref="NullReferenceException">
		/// The level missions collection is not valid.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be less then 0 or bigger then number of objects in the collection.
		/// </exception>
		public LevelMission this[int index]
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level missions collection is not valid.");
				}
				if (index < 0 || index >= this.GetCount())
				{
					throw new IndexOutOfRangeException(
						"Index cannot be less then 0 or bigger then number of objects in the collection.");
				}
				return this.GetItem(index);
			}
		}
		/// <summary>
		/// Gets information about a default mission on this level.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// The level missions collection is not valid.
		/// </exception>
		public LevelMission Default
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level missions collection is not valid.");
				}
				return this.GetDefault();
			}
		}
		/// <summary>
		/// Gets number of missions that are supported by this level.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// The level missions collection is not valid.
		/// </exception>
		public int Count
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level missions collection is not valid.");
				}
				return this.GetCount();
			}
		}
		#endregion
		#region Construction
		internal LevelMissions(IntPtr levelHandle)
		{
			this.levelHandle = levelHandle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether a mission with a given name is supported by the level.
		/// </summary>
		/// <param name="name">Name of the mission.</param>
		/// <returns>
		/// True, if a mission that uses given name is in the list of supported ones. It will return
		/// <c>false</c>, if provided name is null.
		/// </returns>
		/// <exception cref="NullReferenceException">
		/// The level missions collection is not valid.
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Supports(string name);
		/// <summary>
		/// Enumerates through this collection.
		/// </summary>
		/// <returns>Object that handles enumeration.</returns>
		/// <exception cref="NullReferenceException">
		/// The level missions collection is not valid.
		/// </exception>
		public IEnumerator<LevelMission> GetEnumerator()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				yield return this.GetItem(i);
			}
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern LevelMission GetDefault();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern LevelMission GetItem(int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetCount();
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}