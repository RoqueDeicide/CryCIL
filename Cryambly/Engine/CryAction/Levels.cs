using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents a collection of level CryEngine level system is aware of.
	/// </summary>
	public struct Levels : IEnumerable<Level>
	{
		#region Properties
		/// <summary>
		/// Gets number of levels.
		/// </summary>
		public int Count => GetCount();
		/// <summary>
		/// Gets the information about a level using its index.
		/// </summary>
		/// <param name="index">Index of the level to get.</param>
		/// <returns>A wrapper object for the level.</returns>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be less then 0 or greater then <see cref="Count"/>.
		/// </exception>
		public Level this[int index]
		{
			get
			{
				bool outOfRange;

				Level level = GetItemInt(index, out outOfRange);

				if (outOfRange)
				{
					throw new IndexOutOfRangeException("Index cannot be less then 0 or greater then Count");
				}

				return level;
			}
		}
		/// <summary>
		/// Gets the information about a level using its name.
		/// </summary>
		/// <param name="name">Name of the level to get.</param>
		/// <returns>A valid object, if found, otherwise an invalid one is returned.</returns>
		public Level this[string name] => name.IsNullOrEmpty() ? new Level() : GetItem(name);
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates through this collection.
		/// </summary>
		/// <returns>Object that handles enumeration.</returns>
		public IEnumerator<Level> GetEnumerator()
		{
			int levelCount = this.Count;
			for (int i = 0; i < levelCount; i++)
			{
				yield return this[i];
			}
		}
		#endregion
		#region Utilities
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetCount();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Level GetItemInt(int index, out bool outOfRange);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Level GetItem(string name);
		#endregion
	}
}