using System;
using System.Runtime.InteropServices;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about one physical entity that was affected by the explosion.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ExplodedEntity
	{
		/// <summary>
		/// Represents a physical entity itself.
		/// </summary>
		public PhysicalEntity Entity;
		/// <summary>
		/// The value from 0 to 1 that represents the degree of exposure of the entity to the explosion.
		/// </summary>
		public float Exposure;
	}
	/// <summary>
	/// Encapsulates result of the explosion.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ExplosionResult
	{
		/// <summary>
		/// An array of entities that were affected by the explosion.
		/// </summary>
		public ExplodedEntity[] AffectedEntities;
	}
}