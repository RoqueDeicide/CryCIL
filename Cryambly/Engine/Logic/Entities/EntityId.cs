using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents identifier of a CryEngine entity.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct EntityId
	{
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