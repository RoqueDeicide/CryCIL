using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents identifier of a CryEngine entity.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EntityId : IEquatable<EntityId>
	{
		#region Static Fields
		/// <summary>
		/// Identifier that should be used for the entity that represents local player.
		/// </summary>
		public static readonly EntityId LocalPlayerId = new EntityId(0x7777u);
		#endregion
		#region Fields
		private readonly uint id;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a wrapper of a CryEngine entity this identifier represents.
		/// </summary>
		public CryEntity Entity
		{
			get { return GetEntity(this.id); }
		}

		/// <summary>
		/// Indicates whether this entity is bookmarked in the entity pool.
		/// </summary>
		public bool IsBookmarked
		{
			get { return IsEntityBookmarkedInternal(this); }
		}
		/// <summary>
		/// Gets the name of the class that represents this entity if this entity is bookmarked in the
		/// pool.
		/// </summary>
		public string BookmarkedClassName
		{
			get { return GetBookmarkedClassNameInternal(this); }
		}
		/// <summary>
		/// Gets the name of the entity bookmark if this entity is bookmarked in the pool.
		/// </summary>
		public string BookmarkedName
		{
			get { return GetBookmarkedEntityNameInternal(this); }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="id">Identifier of an entity.</param>
		public EntityId(uint id)
		{
			this.id = id;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Prepares an entity with this identifier using the pool.
		/// </summary>
		/// <param name="now">
		/// Indicates whether the entity should be prepared now ( <c>true</c>) or at the start of the next
		/// frame ( <c>false</c>).
		/// </param>
		/// <returns>True, if preparation was successful, otherwise false.</returns>
		public bool Prepare(bool now = false)
		{
			return PrepareInternal(this, now);
		}
		/// <summary>
		/// Returns an entity with this identifier to the pool.
		/// </summary>
		/// <param name="saveState">Indicates whether state of the entity should be saved.</param>
		/// <returns>True, if successful, otherwise false.</returns>
		public bool Return(bool saveState = true)
		{
			return ReturnInternal(this, saveState);
		}
		/// <summary>
		/// Resets pool bookmark of this entity, if it's set to be created through the pool.
		/// </summary>
		/// <remarks>
		/// Can be used to make sure that, next time the entity is prepared, it won't use state that was
		/// saved when returning an entity with <c>saveState</c> parameter set to <c>true</c>.
		/// </remarks>
		public void ResetBookmark()
		{
			ResetBookmarkInternal(this);
		}
		/// <summary>
		/// Indicates whether this identifier is equal to another.
		/// </summary>
		/// <param name="other">Another identifier.</param>
		/// <returns>True, if both identifiers are equal to each other.</returns>
		public bool Equals(EntityId other)
		{
			return this.id == other.id;
		}
		/// <summary>
		/// Indicates whether this identifier is equal to another object.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if given object is of type <see cref="EntityId"/> and is equal to this identifier.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is EntityId && this.Equals((EntityId)obj);
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
		/// Determines equality of 2 objects of type <see cref="EntityId"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are equal.</returns>
		public static bool operator ==(EntityId left, EntityId right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines inequality of 2 objects of type <see cref="EntityId"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 objects are not equal.</returns>
		public static bool operator !=(EntityId left, EntityId right)
		{
			return !left.Equals(right);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntity GetEntity(uint id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool PrepareInternal(EntityId id, bool prepareNow);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ReturnInternal(EntityId id, bool saveState = true);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ResetBookmarkInternal(EntityId id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEntityBookmarkedInternal(EntityId entityId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetBookmarkedClassNameInternal(EntityId entityId);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetBookmarkedEntityNameInternal(EntityId entityId);
		#endregion
	}
}