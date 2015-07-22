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
		public ulong Id
		{
			get { return this.id; }
		}
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

		#endregion
		#region Utilities
		#endregion
	}
}
