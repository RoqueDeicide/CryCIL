using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Defines functions that can be used add and remove boolean carving shapes that are used by
	/// procedurally breakable objects.
	/// </summary>
	public static class ExplosionShapes
	{
		/// <summary>
		/// Registers a boolean carving for procedurally breakable objects.
		/// </summary>
		/// <param name="shape">      Represents the shape of the carving.</param>
		/// <param name="size">       
		/// 'Characteristic size of the shape. When breakage occurs, desired size is specified and the
		/// shape is scaled by desired_size / this_size.
		/// </param>
		/// <param name="index">      
		/// Breakability index to assign this shape to. It is possible to have multiple shapes assigned to
		/// the same index.
		/// </param>
		/// <param name="probability">
		/// The value that defines relative probability of this shape getting used when there are multiple
		/// shapes in the same index that are close to the desired size.
		/// </param>
		/// <returns>Assigned breakability index.</returns>
		/// <exception cref="ArgumentNullException">
		/// Cannot use null geometry for boolean carvings.
		/// </exception>
		public static int Add(PhysicalGeometry shape, float size, int index, float probability = 1.0f)
		{
			if (!shape.IsValid)
			{
				throw new ArgumentNullException("shape", "Cannot use null geometry for boolean carvings.");
			}
			return PhysicalWorld.AddExplosionShape(shape, size, index, probability);
		}
		/// <summary>
		/// Clears all boolean carvings that were assigned to the specified index using <see cref="Add"/>.
		/// </summary>
		/// <param name="index">Breakability index to clear.</param>
		public static void Remove(int index)
		{
			PhysicalWorld.RemoveExplosionShape(index);
		}
		/// <summary>
		/// Removes all registered breakability indexes.
		/// </summary>
		public static void RemoveAll()
		{
			PhysicalWorld.RemoveAllExplosionShapes();
		}
	}
}