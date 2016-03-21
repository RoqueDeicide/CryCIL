using System;
using System.Linq;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents an arrow that is rendered using <see cref="DebugEngine"/> and
	/// <see cref="AuxiliaryGeometry"/>.
	/// </summary>
	public class DebugArrow : Debug3DObject
	{
		#region Fields
		private float length;
		private Vector3 start;
		private Vector3 end;
		private Vector3 dir;
		private Vector3 center;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets coordinates of the center of the line that represents this arrow.
		/// </summary>
		public Vector3 Center
		{
			get { return this.center; }
			set
			{
				Vector3 delta = value - this.center;
				this.center = value;

				this.start += delta;
				this.end += delta;
			}
		}
		/// <summary>
		/// Gets or sets coordinates of the center of the line that represents this arrow.
		/// </summary>
		public Vector3 Location
		{
			get { return this.Center; }
			set { this.Center = value; }
		}
		/// <summary>
		/// Gets or sets direction this object is pointing at.
		/// </summary>
		/// <remarks>Direction is always normalized upon setting.</remarks>
		public Vector3 Direction
		{
			get { return this.dir; }
			set
			{
				this.dir = value;
				float lengthSquared = this.dir.LengthSquared;
				if (lengthSquared - 1 < MathHelpers.ZeroTolerance)
				{
					this.dir *= 1.0f / (float)Math.Sqrt(lengthSquared);
				}
			}
		}
		/// <summary>
		/// Gets or sets length of this arrow.
		/// </summary>
		public float Length
		{
			get { return this.length; }
			set
			{
				this.length = value;
				// Update the vectors.
				float halfLength = this.length * 0.5f;
				this.start = this.Center - this.dir * halfLength;
				this.end = this.Center + this.dir * halfLength;
			}
		}
		/// <summary>
		/// Gets or sets radius of the cone that represents a tip of this arrow.
		/// </summary>
		public float ArrowTipRadius { get; set; }
		/// <summary>
		/// Gets or sets starting point of this arrow.
		/// </summary>
		public Vector3 Start
		{
			get { return this.start; }
			set
			{
				this.start = value;

				Vector3 delta = this.end - this.start;

				this.Center = this.start + delta * 0.5f;

				this.length = delta.Length;
				this.dir = delta.Normalized;
			}
		}
		/// <summary>
		/// Gets or sets finishing point of this arrow.
		/// </summary>
		public Vector3 End
		{
			get { return this.end; }
			set
			{
				this.end = value;

				Vector3 delta = this.end - this.start;

				this.Center = this.start + delta * 0.5f;

				this.length = delta.Length;
				this.dir = delta.Normalized;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this class.
		/// </summary>
		/// <param name="start"> Coordinates of the start of the arrow.</param>
		/// <param name="end">   Coordinates of the point of the arrow.</param>
		/// <param name="radius">Radius of the arrow's cone.</param>
		public DebugArrow(Vector3 start, Vector3 end, float radius)
		{
			this.start = start;
			this.end = end;

			Vector3 delta = this.end - this.start;

			this.Center = start + delta * 0.5f;

			this.length = delta.Length;
			this.dir = delta.Normalized;

			this.ArrowTipRadius = radius;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this object using <see cref="AuxiliaryGeometry"/> API.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			// Draw a line with Center vector in its center.
			AuxiliaryGeometry.DrawLine(this.start,
									   this.end,
									   this.Color);
			// Draw a cone with its top point at the end of the above line. Its height is .33 of the total
			// length of the arrow.
			AuxiliaryGeometry.DrawCone(this.center + this.dir * this.length * 0.17f,
									   this.dir,
									   this.ArrowTipRadius,
									   this.length * 0.33f,
									   this.Color);
		}
		#endregion
	}
}