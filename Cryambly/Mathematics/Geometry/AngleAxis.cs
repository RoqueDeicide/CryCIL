using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Geometry
{
	/// <summary>
	/// Represents a vector which length is an angle of rotation and its normalized version is an axis of rotation.
	/// </summary>
	public struct AngleAxis
	{
		/// <summary>
		/// <see cref="Vector3"/> which normalized version can be used as axis of rotation and which length represents angle of rotation. Use this field if other two properties are too costly performance-wise.
		/// </summary>
		public Vector3 Vector;
		/// <summary>
		/// Gets or sets axis of rotation.
		/// </summary>
		public Vector3 Axis
		{
			get { return this.vector.Normalized; }
			set { this.vector = value.Normalized * this.vector.Length; }
		}
		/// <summary>
		/// Gets or sets angle of rotation.
		/// </summary>
		public float Angle
		{
			get { return this.vector.Length; }
			set
			{
				this.vector.Normalize();
				Scale.Apply(ref this.vector, value);
			}
		}
	}
}
