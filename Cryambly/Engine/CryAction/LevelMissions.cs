using System;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents a collection of missions that are supported by the level.
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
	/// </remarks>
	public struct LevelMissions
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets number of missions that are supported by this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return Level.GetGameTypeCount(this.handle);
			}
		}
		/// <summary>
		/// Gets information about the mission.
		/// </summary>
		/// <param name="index">Zero-based index of the mission.</param>
		/// <returns>An object that encapsulates details about the mission.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index was out of range.</exception>
		public GameTypeInfo this[int index]
		{
			get
			{
				this.AssertInstance();

				GameTypeInfo info;
				if (!Level.GetGameType(this.handle, index, out info))
				{
					throw new IndexOutOfRangeException("Index was out of range.");
				}
				return info;
			}
		}
		/// <summary>
		/// Gets default mission.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public GameTypeInfo Default
		{
			get
			{
				this.AssertInstance();

				GameTypeInfo info;
				Level.GetDefaultGameType(this.handle, out info);
				return info;
			}
		}
		#endregion
		#region Construction
		internal LevelMissions(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this level supports a mission.
		/// </summary>
		/// <param name="gameType">Name of the mission.</param>
		/// <returns>True, if mission is supported.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Supports(string gameType)
		{
			this.AssertInstance();

			return Level.SupportsGameType(this.handle, gameType);
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
		#endregion
	}
}