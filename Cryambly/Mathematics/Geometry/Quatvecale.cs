using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates <see cref="Quaternion"/> that describes orientation, <see cref="Vector3"/> that
	/// describes position and <see cref="float"/> value that represents a scaling factor.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Quatvecale
	{
		#region Static Fields
		/// <summary>
		/// An object of this type that when applied to an object as a transformation doesn't change
		/// anything about it.
		/// </summary>
		public static readonly Quatvecale Identity = new Quatvecale(Quaternion.Identity, new Vector3(), 1);
		#endregion
		#region Fields
		/// <summary>
		/// <see cref="Orientation"/> object that describes orientation of an arbitrary object.
		/// </summary>
		public Quaternion Orientation;
		/// <summary>
		/// <see cref="Vector3"/> object that describes position of an arbitrary object.
		/// </summary>
		public Vector3 Translation;
		/// <summary>
		/// <see cref="float"/> value that describes scale of an arbitrary object.
		/// </summary>
		public float Scale;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the object that represents the transformation that cancels the one represented by this
		/// object.
		/// </summary>
		public Quatvecale Inverted
		{
			get
			{
				Quatvecale i = this;
				i.Invert();
				return i;
			}
		}
		/// <summary>
		/// Gets the first column of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Column0 => this.Orientation.Column0;
		/// <summary>
		/// Gets the second column of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Column1 => this.Orientation.Column1;
		/// <summary>
		/// Gets the third column of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Column2 => this.Orientation.Column2;
		/// <summary>
		/// Gets the fourth column of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Column3 => this.Translation;
		/// <summary>
		/// Gets the first row of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Row0 => this.Orientation.Row0;
		/// <summary>
		/// Gets the second row of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Row1 => this.Orientation.Row1;
		/// <summary>
		/// Gets the third row of the 3x4 matrix that represents the same transformation as this object.
		/// </summary>
		public Vector3 Row2 => this.Orientation.Row2;
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="orientation">
		/// <see cref="Quaternion"/> object that describes orientation of an arbitrary object.
		/// </param>
		/// <param name="translation">
		/// <see cref="Vector3"/> object that describes position of an arbitrary object.
		/// </param>
		/// <param name="scale">      
		/// <see cref="float"/> value that describes scale of an arbitrary object.
		/// </param>
		public Quatvecale(Quaternion orientation, Vector3 translation, float scale)
		{
			this.Orientation = orientation;
			this.Translation = translation;
			this.Scale = scale;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="quatvec">
		/// <see cref="Quatvec"/> object that describes orientation and translation of an arbitrary object.
		/// </param>
		public Quatvecale(Quatvec quatvec)
		{
			this.Orientation = quatvec.Quaternion;
			this.Translation = quatvec.Vector;
			this.Scale = 1;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="matrix">
		/// <see cref="Matrix34"/> object that describes orientation and translation of an arbitrary object.
		/// </param>
		public Quatvecale(Matrix34 matrix)
		{
			this.Translation = matrix.Translation;

			// The determinant of a matrix is the volume spanned by its base vectors. We need an approximate
			// length scale, so we calculate the cube root of the determinant.
			this.Scale = (float)Math.Pow(matrix.Determinant, 1.0 / 3.0);

			// Orthonormalize using X and Z as anchors.
			Vector3 r0 = new Vector3(matrix.M00, matrix.M01, matrix.M02);
			Vector3 r2 = new Vector3(matrix.M20, matrix.M21, matrix.M22);

			Vector3 v0 = r0.Normalized;
			Vector3 v1 = (r2 % r0).Normalized;
			Vector3 v2 = v0 % v1;

			Matrix33 m33 = new Matrix33 {Row0 = v0, Row1 = v1, Row2 = v2};

			this.Orientation = new Quaternion(m33);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes any transformations that were stored in this object.
		/// </summary>
		public void SetIdentity()
		{
			this.Orientation = Quaternion.Identity;
			this.Translation = new Vector3();
			this.Scale = 1;
		}
		/// <summary>
		/// Inverts the transformation represented by this object.
		/// </summary>
		public void Invert()
		{
			this.Scale = 1 / this.Scale;
			this.Orientation.Invert();
			Transformation.Apply(ref this.Translation, ref this.Orientation);
			this.Translation *= -this.Scale;
		}
		/// <summary>
		/// Determines whether this object is equivalent of another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <param name="qe">   Precision of quaternion comparison.</param>
		/// <param name="ve">   Precision of vector comparison.</param>
		/// <returns>True, if two objects can be considered equivalents.</returns>
		public bool IsEquivalent(Quatvecale other, float qe = 0.01f, float ve = 0.05f)
		{
			double rad = Math.Acos(Math.Min(1.0, Math.Abs(this.Orientation | other.Orientation)));
			return
				rad <= qe &&
				Math.Abs(this.Translation.X - other.Translation.X) <= ve &&
				Math.Abs(this.Translation.Y - other.Translation.Y) <= ve &&
				Math.Abs(this.Translation.Z - other.Translation.Z) <= ve &&
				Math.Abs(this.Scale - other.Scale) <= ve;
		}
		#endregion
		#region Utilities
		#endregion
	}
}