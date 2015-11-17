using System;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that represents axis-aligned bounding box.
	/// </summary>
	public class DebugAxisAlignedBoundingBox : Debug3DObject
	{
		private BoundingBox box;
		/// <summary>
		/// Gets or sets a bounding box to render.
		/// </summary>
		public BoundingBox Box
		{
			get { return this.box; }
			set { this.box = value; }
		}
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
			BoundingBox b = new BoundingBox(this.box.Minimum + this.Translation,
											this.box.Maximum + this.Translation);
			AuxiliaryGeometry.DrawAABB(ref b, true, this.Color);
		}
	}
}