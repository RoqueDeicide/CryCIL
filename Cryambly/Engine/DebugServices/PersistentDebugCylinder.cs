using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a persistent debug object that looks like a cylinder.
	/// </summary>
	public class PersistentDebugCylinder : PersistentDebug3DObject
	{
		#region Properties
		/// <summary>
		/// Gets or sets location of the center to the base circle of the cylinder.
		/// </summary>
		public Vector3 BaseCenter { get; set; }
		/// <summary>
		/// Gets or sets radius of the base circle of the cylinder.
		/// </summary>
		public float BaseRadius { get; set; }
		/// <summary>
		/// Gets or sets height of the cylinder.
		/// </summary>
		public float Height { get; set; }
		/// <summary>
		/// Gets or sets quaternion that represents orientation of this cylinder.
		/// </summary>
		/// <remarks>
		/// 3rd column of the quaternion represents direction of line parallel to the line between top and
		/// a bottom of the cylinder.
		/// </remarks>
		public Quaternion Orientation { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="baseCenter">Coordinates of the center of the cylinder's base.</param>
		/// <param name="top">       Coordinates of the top point of the cylinder.</param>
		/// <param name="radius">    Radius of the cylinder's base.</param>
		public PersistentDebugCylinder(Vector3 baseCenter, Vector3 top, float radius)
		{
			this.BaseCenter = baseCenter;
			this.Height = (top - baseCenter).Length;
			this.BaseRadius = radius;
			var angleAxis = Rotation.ArcBetween2Vectors(Vector3.Up, top);
			this.Orientation = new Quaternion(angleAxis.Axis, angleAxis.Angle);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this cylinder.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawCylinder(this.BaseCenter, this.Orientation.Column2, this.BaseRadius, this.Height, this.Color);
		}
		#endregion
	}
}