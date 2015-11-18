using System;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that represents axis-aligned bounding box.
	/// </summary>
	public class DebugAxisAlignedBoundingBox : Debug3DObject
	{
		/// <summary>
		/// Gets or sets a bounding box to render.
		/// </summary>
		public BoundingBox Box { get; set; }
		/// <summary>
		/// Gets or sets translation vector that is applied to the AABB before rendering.
		/// </summary>
		public Vector3 Translation { get; set; }
		/// <summary>
		/// Renders this box.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			BoundingBox b = new BoundingBox(this.Box.Minimum + this.Translation, this.Box.Maximum + this.Translation);
			AuxiliaryGeometry.DrawAABB(ref b, true, this.Color);
		}
	}
}