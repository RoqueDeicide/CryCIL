using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents a collection of game modes supported by the level.
	/// </summary>
	public struct LevelGameModes : IEnumerable<string>
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets name of the supported game mode.
		/// </summary>
		/// <param name="index">Zero-based index of the name of the game mode to get.</param>
		/// <returns>Name of the game mode.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be less then 0 or bigger then number of objects in the collection.
		/// </exception>
		public string this[int index]
		{
			get
			{
				this.AssertInstance();
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException(
						"Index cannot be less then 0 or bigger then number of objects in the collection.");
				}

				return Level.GetGameRules(this.handle, index);
			}
		}
		/// <summary>
		/// Gets the name of the default game mode on this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Default
		{
			get
			{
				this.AssertInstance();

				return Level.GetDefaultGameRules(this.handle);
			}
		}
		/// <summary>
		/// Gets number of game modes that are supported by this level.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return Level.GetGameRulesCount(this.handle);
			}
		}
		#endregion
		#region Construction
		internal LevelGameModes(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Enumerator.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public IEnumerator<string> GetEnumerator()
		{
			this.AssertInstance();

			int count = Level.GetGameRulesCount(this.handle);
			for (int i = 0; i < count; i++)
			{
				yield return Level.GetGameRules(this.handle, i);
			}
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}