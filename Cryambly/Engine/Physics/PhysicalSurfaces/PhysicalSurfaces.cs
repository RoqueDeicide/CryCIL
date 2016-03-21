using System;
using System.Linq;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Provides access to a collection of objects that define physical parameters of surfaces.
	/// </summary>
	public struct PhysicalSurfaces
	{
		/// <summary>
		/// Gets the object that specifies the surface with given index.
		/// </summary>
		/// <param name="index">
		/// Index of the surface to get. This index cannot be less then 0 or greater then 511.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// Index of the surface object to get must be in range [0; 511].
		/// </exception>
		public PhysicalSurface this[int index]
		{
			get
			{
				if (index < 0 || index >= 511)
				{
					throw new IndexOutOfRangeException("Index of the surface object to get must be in range [0; 511].");
				}
				return new PhysicalSurface(index);
			}
		}
	}
}