using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents globally unique identifier of the entity.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EntityGUID
	{
		#region Fields
		private readonly ulong id;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier itself.
		/// </summary>
		public ulong Id => this.id;
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="id">Identifier of an entity.</param>
		public EntityGUID(ulong id)
		{
			this.id = id;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Indicates whether this identifier is equal to another.
		/// </summary>
		/// <param name="other">Another identifier.</param>
		/// <returns>True, if both identifiers are equal to each other.</returns>
		public bool Equals(EntityGUID other)
		{
			return this.id == other.id;
		}
		/// <summary>
		/// Indicates whether this identifier is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if given object is of type <see cref="EntityGUID"/> and is equal to this identifier.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is EntityGUID && this.Equals((EntityGUID)obj);
		}
		/// <summary>
		/// Gets hash code of this identifier.
		/// </summary>
		/// <remarks>Don't use it.</remarks>
		/// <returns>Hash code of this identifier.</returns>
		public override int GetHashCode()
		{
			return (int)this.id;
		}
		/// <summary>
		/// Determines equality of 2 objects of type <see cref="EntityGUID"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are equal.</returns>
		public static bool operator ==(EntityGUID left, EntityGUID right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines inequality of 2 objects of type <see cref="EntityGUID"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are not equal.</returns>
		public static bool operator !=(EntityGUID left, EntityGUID right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Utilities
		#endregion
	}
}