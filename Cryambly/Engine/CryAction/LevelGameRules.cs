using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents a collection of game rules supported by the level.
	/// </summary>
	public struct LevelGameRules : IEnumerable<string>
	{
		#region Fields
		[UsedImplicitly] private IntPtr levelHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets information about the supported game rules set.
		/// </summary>
		/// <param name="index">Zero-based index of the game rules set name to get.</param>
		/// <returns>Name of the game rules set.</returns>
		/// <exception cref="NullReferenceException">
		/// The level game rules set collection is not valid.
		/// </exception>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be less then 0 or bigger then number of objects in the collection.
		/// </exception>
		public string this[int index]
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level game rules set collection is not valid.");
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
		/// Gets the name of the default game rules set on this level.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// The level game rules set collection is not valid.
		/// </exception>
		public string Default
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level game rules set collection is not valid.");
				}
				return this.GetDefault();
			}
		}
		/// <summary>
		/// Gets number of game rules sets that are supported by this level.
		/// </summary>
		/// <exception cref="NullReferenceException">
		/// The level game rules set collection is not valid.
		/// </exception>
		public int Count
		{
			get
			{
				if (this.levelHandle == IntPtr.Zero)
				{
					throw new NullReferenceException("The level game rules set collection is not valid.");
				}
				return this.GetCount();
			}
		}
		#endregion
		#region Construction
		internal LevelGameRules(IntPtr levelHandle)
		{
			this.levelHandle = levelHandle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates this collection.
		/// </summary>
		/// <returns>Enumerator.</returns>
		/// <exception cref="NullReferenceException">
		/// The level game rules set collection is not valid.
		/// </exception>
		public IEnumerator<string> GetEnumerator()
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
		private extern string GetDefault();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetItem(int index);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetCount();
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}