using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that looks like a cylinder.
	/// </summary>
	public class DebugCylinder : DebugSolidOfRevolution
	{
		#region Construction
		/// <summary>
		/// Creates new cylinder that stands up-right.
		/// </summary>
		/// <param name="baseCenter">Coordinates of the center of the cylinder's bottom circle.</param>
		/// <param name="height">    Height of the cylinder.</param>
		/// <param name="radius">    Radius of the cylinder.</param>
		public DebugCylinder(Vector3 baseCenter, float height, float radius)
			: base(baseCenter, height, radius, Quaternion.Identity)
		{
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="baseCenter"> Coordinates of the center of the cylinder's bottom circle.</param>
		/// <param name="height">     Height of the cylinder.</param>
		/// <param name="radius">     Radius of the cylinder.</param>
		/// <param name="orientation">
		/// <see cref="Quaternion"/> that represents orientation of this cylinder.
		/// </param>
		public DebugCylinder(Vector3 baseCenter, float height, float radius,
							 Quaternion orientation)
			: base(baseCenter, height, radius, orientation)
		{
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="baseCenter">Coordinates of the center of the cylinder's bottom circle.</param>
		/// <param name="top">       Coordinates of the center of the cylinder's top circle.</param>
		/// <param name="radius">    Radius of the cylinder.</param>
		public DebugCylinder(Vector3 baseCenter, Vector3 top, float radius)
			: base(baseCenter, top, radius)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this cylinder.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawCylinder
				(
				 this.Start,
				 this.Orientation.Column2,
				 this.MaxRadius,
				 this.Height,
				 this.Color
				);
		}
		#endregion
	}
}