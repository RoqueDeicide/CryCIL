using System;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug sphere.
	/// </summary>
	public sealed class DebugSphere : Debug3DObject
	{
		#region Properties
		/// <summary>
		/// Gets or sets coordinates of the center of the sphere.
		/// </summary>
		public Vector3 Center { get; set; }
		/// <summary>
		/// Gets or sets radius of this sphere.
		/// </summary>
		public float Radius { get; set; }
		/// <summary>
		/// Gets or sets boolean value that indicates whether this sphere should be rendered with shading
		/// on.
		/// </summary>
		public bool Shaded { get; set; }
		#endregion
		#region Interface
		/// <summary>
		/// Renders this sphere.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawSphere(this.Center, this.Radius, this.Color, this.Shaded);
		}
		#endregion
	}
}