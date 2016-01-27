using System;
using System.Linq;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Represents the type that is used to identify various objects in the audio system.
	/// </summary>
	public struct AudioId
	{
		#region Defaults
		/// <summary>
		/// Default identifier.
		/// </summary>
		public static readonly AudioId Default = 1;
		#endregion
		#region Fields
		private uint id;
		#endregion
		#region Interface
		/// <summary>
		/// Converts the <see cref="AudioId"/> object into <see cref="uint"/>.
		/// </summary>
		/// <param name="id">Identifier to convert.</param>
		/// <returns>Resultant number.</returns>
		public static implicit operator uint(AudioId id)
		{
			return id.id;
		}
		/// <summary>
		/// Converts the <see cref="uint"/> object into <see cref="AudioId"/>.
		/// </summary>
		/// <param name="id">Number to convert.</param>
		/// <returns>Resultant identifier.</returns>
		public static implicit operator AudioId(uint id)
		{
			return new AudioId {id = id};
		}
		#endregion
	}
}