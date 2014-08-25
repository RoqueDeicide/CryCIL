using System.Collections.Generic;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common functionality of collections of mesh details.
	/// </summary>
	/// <typeparam name="ElementType"> Type of elements in the collection. </typeparam>
	public interface IMeshDetailsCollection<ElementType> : IList<ElementType>
	{
		/// <summary>
		/// When implemented in derived class, gets or sets size of collection of mesh details.
		/// </summary>
		int Capacity { get; set; }
		/// <summary>
		/// When implemented in derived class, copies all elements from the collection to the array.
		/// </summary>
		/// <returns> Array that contains copy of all data inside this collection. </returns>
		ElementType[] ToArray();
	}
}