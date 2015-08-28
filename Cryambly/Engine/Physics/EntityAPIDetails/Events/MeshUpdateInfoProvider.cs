using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents an object that provides information about updates that were made to physical mesh.
	/// </summary>
	public struct MeshUpdateInfoProvider
	{
		#region Fields
		private readonly PhysicalGeometry geometry;
		private readonly IntPtr lastUpdate;
		#endregion
		#region Properties
		/// <summary>
		/// Gets an array of objects that provide information about changes that were made to geometry.
		/// </summary>
		public object[] MeshUpdates
		{
			get { throw new NotImplementedException(string.Format("{0}{1}", this.geometry, this.lastUpdate)); }
		}
		#endregion
		#region Construction
		internal MeshUpdateInfoProvider(PhysicalGeometry geometry, IntPtr lastUpdate)
		{
			this.geometry = geometry;
			this.lastUpdate = lastUpdate;
		}
		#endregion
	}
}