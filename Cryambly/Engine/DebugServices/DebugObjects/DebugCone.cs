using System;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents persistent debug object that represents a cone.
	/// </summary>
	public class DebugCone : DebugSolidOfRevolution
	{
		#region Construction
		/// <summary>
		/// Creates new cone that stands up-right.
		/// </summary>
		/// <param name="baseCenter">Coordinates of the center of the cone's bottom circle.</param>
		/// <param name="height">    Height of the cone.</param>
		/// <param name="radius">    Radius of the cone's base.</param>
		public DebugCone(Vector3 baseCenter, float height, float radius)
			: base(baseCenter, height, radius, Quaternion.Identity)
		{
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="baseCenter"> Coordinates of the center of the cone's bottom circle.</param>
		/// <param name="height">     Height of the cone.</param>
		/// <param name="radius">     Radius of the cone's base.</param>
		/// <param name="orientation">
		/// <see cref="Quaternion"/> that represents orientation of this cone.
		/// </param>
		public DebugCone(Vector3 baseCenter, float height, float radius, Quaternion orientation)
			: base(baseCenter, height, radius, orientation)
		{
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="baseCenter">Coordinates of the center of the cone's bottom circle.</param>
		/// <param name="top">       Coordinates of the top point of the cone.</param>
		/// <param name="radius">    Radius of the cone's base.</param>
		public DebugCone(Vector3 baseCenter, Vector3 top, float radius)
			: base(baseCenter, top, radius)
		{
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this cone.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawCone(this.Start,
									   this.Orientation.Column2,
									   this.MaxRadius,
									   this.Height,
									   this.Color);
		}
		#endregion
	}
}