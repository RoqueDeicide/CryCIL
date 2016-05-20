using System;
using System.Linq;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents a table that can be filled with surface types by
	/// <see cref="Material.FillSurfaceTypesTable"/>.
	/// </summary>
	public unsafe struct SurfaceTypeTable
	{
		/// <summary>
		/// A fixed buffer with surface type ids.
		/// </summary>
		public fixed int Ids [128];
		/// <summary>
		/// Number of valid identifiers in this table.
		/// </summary>
		public int Count;
	}
}