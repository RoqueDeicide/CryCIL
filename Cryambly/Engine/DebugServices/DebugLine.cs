using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a line that is drawn using <see cref="DebugEngine"/> class.
	/// </summary>
	public class DebugLine : Debug3DObject
	{
		#region Fields
		private float length;
		private Vector3 dir;
		private Vector3 start;
		private Vector3 end;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets direction of this line.
		/// </summary>
		public Vector3 Direction
		{
			get { return this.dir; }
			set
			{
				this.dir = value.Normalized;
				this.end = this.start + this.dir * this.length;
			}
		}
		/// <summary>
		/// Gets or sets length of this line.
		/// </summary>
		public float Length
		{
			get { return this.length; }
			set
			{
				this.length = value;
				this.end = this.start + this.dir * this.length;
			}
		}
		/// <summary>
		/// Gets or sets starting point of this line.
		/// </summary>
		public Vector3 Start
		{
			get { return this.start; }
			set
			{
				this.start = value;
				Vector3 delta = this.end - this.start;
				this.dir = delta.Normalized;
				this.length = delta.Length;
			}
		}
		/// <summary>
		/// Gets or sets starting point of this line.
		/// </summary>
		public Vector3 End
		{
			get { return this.end; }
			set
			{
				this.end = value;
				Vector3 delta = this.end - this.start;
				this.dir = delta.Normalized;
				this.length = delta.Length;
			}
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Initializes new directed object by calculating direction and length out of given two vectors.
		/// </summary>
		/// <param name="start">Start of the line.</param>
		/// <param name="end">  End of the line.</param>
		public DebugLine(Vector3 start, Vector3 end)
		{
			this.start = start;
			this.end = end;
			Vector3 direction = end - start;
			this.length = direction.Length;
			this.dir = direction.Normalized;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this line.
		/// </summary>
		public override void Render()
		{
			AuxiliaryGeometry.Flags = this.RenderingFlags;
			AuxiliaryGeometry.DrawLine(this.start, this.end, this.Color);
		}
		#endregion
		#region Utilities

		#endregion
	}
}