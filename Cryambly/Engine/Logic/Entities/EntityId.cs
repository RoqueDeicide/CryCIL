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

		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryEntity GetEntity(uint id);
		#endregion
	}
}