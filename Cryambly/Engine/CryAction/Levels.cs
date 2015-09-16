using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.CryAction
{
	/// <summary>
	/// Represents a collection of level CryEngine level system is aware of.
	/// </summary>
	public class Levels : IEnumerable<Level>
	{
		#region Properties
		/// <summary>
		/// Gets number of levels.
		/// </summary>
		public extern int Count { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the information about a level using its index.
		/// </summary>
		/// <param name="index">Index of the level to get.</param>
		/// <returns>A wrapper object for the level.</returns>
		/// <exception cref="IndexOutOfRangeException">
		/// Index cannot be less then 0 or greater then <see cref="Count"/>.
		/// </exception>
		public extern Level this[int index] { [MethodImpl(MethodImplOptions.InternalCall)] get; }
		/// <summary>
		/// Gets the information about a level using its name.
		/// </summary>
		/// <param name="name">Name of the level to get.</param>
		/// <returns>A wrapper object for the level, if found, otherwise null is returned.</returns>
		public extern Level this[string name] { [MethodImpl(MethodImplOptions.InternalCall)] get; }
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
		#endregion
	}
}