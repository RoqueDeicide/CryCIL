using System;
using System.Collections;
using System.Collections.Generic;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents a collection of all loaded particle effects.
	/// </summary>
	public struct ParticleEffectDatabase : IEnumerable<ParticleEffect>
	{
		#region Interface
		/// <summary>
		/// Enumerates the database.
		/// </summary>
		/// <returns>An object that will do the enumeration.</returns>
		public IEnumerator<ParticleEffect> GetEnumerator()
		{
			return new ParticleEffectEnumerator();
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