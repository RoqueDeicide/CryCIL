using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Geometry;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Base class for persistent debug objects that represent solids of revolution.
	/// </summary>
	/// <remarks>
	/// <para>
	/// A solid of revolution is a solid figure obtained by rotating a plane curve around some straight
	/// line (the axis) that lies on the same plane.
	/// </para>
	/// <para>Axis of revolution represents an "Up" axis of orientation of the solid.</para>
	/// </remarks>
	public abstract class PersistentDebugSolidOfRevolution : PersistentDebug3DObject
	{
		#region Properties
		/// <summary>
		/// Gets or sets location of the starting point of the axis of revolution.
		/// </summary>
		public Vector3 Start { get; set; }
		/// <summary>
		/// Gets or sets maximal width of the curve which revolution formed this solid.
		/// </summary>
		public float MaxRadius { get; set; }
		/// <summary>
		/// Gets or sets length of the axis or revolution.
		/// </summary>
		public float Height { get; set; }
		/// <summary>
		/// Gets or sets quaternion that represents orientation of this body.
		/// </summary>
		/// <remarks>
		/// 3rd column of the quaternion represents direction of line parallel to the axis of revolution.
		/// </remarks>
		public Quaternion Orientation { get; set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <remarks>This constructor creates an up-right standing body.</remarks>
		/// <param name="start"> Coordinates of the start of axis of revolution.</param>
		/// <param name="height">Length of the axis of revolution.</param>
		/// <param name="radius">Maximal radius of the body.</param>
		protected PersistentDebugSolidOfRevolution(Vector3 start, float height, float radius)
			: this(start, height, radius, Quaternion.Identity)
		{
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="start">      Coordinates of the start of axis of revolution.</param>
		/// <param name="height">     Length of the axis of revolution.</param>
		/// <param name="radius">     Maximal radius of the body.</param>
		/// <param name="orientation">
		/// <see cref="Quaternion"/> that represents orientation of this body.
		/// </param>
		protected PersistentDebugSolidOfRevolution(Vector3 start, float height, float radius, Quaternion orientation)
		{
			this.Start = start;
			this.Height = height;
			this.MaxRadius = radius;
			this.Orientation = orientation;
		}
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="start"> Coordinates of the start of axis of revolution.</param>
		/// <param name="top">   Coordinates of the start of axis of revolution.</param>
		/// <param name="radius">Maximal radius of the body.</param>
		protected PersistentDebugSolidOfRevolution(Vector3 start, Vector3 top, float radius)
		{
			this.Start = start;
			this.Height = (top - start).Length;
			this.MaxRadius = radius;
			var angleAxis = Rotation.ArcBetween2Vectors(Vector3.Up, top);
			this.Orientation = new Quaternion(angleAxis.Axis, angleAxis.Angle);
		}
		#endregion
	}
}