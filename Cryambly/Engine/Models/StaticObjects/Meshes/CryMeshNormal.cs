using System;
using System.Runtime.InteropServices;
using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a normal vector to the vertex.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct CryMeshNormal : IEquatable<CryMeshNormal>
	{
		#region Fields
		private Vector3 normal;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the normal vector.
		/// </summary>
		public Vector3 Normal
		{
			get { return this.normal; }
			set { this.normal = value; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this normal vector can be considered equal to another.
		/// </summary>
		/// <param name="other">  Another vector.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between 2 vectors is smaller then <paramref name="epsilon"/>
		/// </returns>
		public bool IsEquivalent(ref Vector3 other, float epsilon = 0.00005f)
		{
			return this.normal.IsEquivalent(other, epsilon);
		}
		/// <summary>
		/// Determines whether this normal vector can be considered equal to another.
		/// </summary>
		/// <param name="other">  Another vector.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between 2 vectors is smaller then <paramref name="epsilon"/>
		/// </returns>
		public bool IsEquivalent(ref CryMeshNormal other, float epsilon = 0.00005f)
		{
			return this.normal.IsEquivalent(other.normal, epsilon);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(CryMeshNormal other)
		{
			return this.normal == other.normal;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CryMeshNormal && this.Equals((CryMeshNormal)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			return this.normal.GetHashCode();
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are equal.</returns>
		public static bool operator ==(CryMeshNormal left, CryMeshNormal right)
		{
			return left.normal == right.normal;
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are not equal.</returns>
		public static bool operator !=(CryMeshNormal left, CryMeshNormal right)
		{
			return left.normal != right.normal;
		}
		/// <summary>
		/// Determines whether left operand is less then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is less then the right one.</returns>
		public static bool operator <(CryMeshNormal left, CryMeshNormal right)
		{
			return
				(left.normal.X != right.normal.X)
					? (left.normal.X < right.normal.X)
					: (left.normal.Y != right.normal.Y)
						? (left.normal.Y < right.normal.Y)
						: (left.normal.Z < right.normal.Z);
		}
		/// <summary>
		/// Determines whether left operand is greater then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is greater then the right one.</returns>
		public static bool operator >(CryMeshNormal left, CryMeshNormal right)
		{
			return !(left < right) && left != right;
		}
		/// <summary>
		/// Applies transformation to this normal.
		/// </summary>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public void RotateBy(ref Matrix33 matrix)
		{
			this.normal = matrix * this.normal;
		}
		/// <summary>
		/// Applies transformation to this normal and normalizes it.
		/// </summary>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public void RotateBySafe(ref Matrix33 matrix)
		{
			this.normal = matrix * this.normal;
			this.normal.Normalize();
		}
		/// <summary>
		/// Applies transformation to this normal.
		/// </summary>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public void TransformBy(ref Matrix34 matrix)
		{
			Transformation.Apply(ref this.normal, ref matrix);
		}
		/// <summary>
		/// Applies transformation to this normal and normalizes it.
		/// </summary>
		/// <param name="matrix">Matrix that represents the transformation.</param>
		public void TransformBySafe(ref Matrix34 matrix)
		{
			Transformation.Apply(ref this.normal, ref matrix);
			this.normal.Normalize();
		}
		/// <summary>
		/// Applies spherical linear interpolation to this normal.
		/// </summary>
		/// <param name="other">   Normal that represents "destination" of interpolation.</param>
		/// <param name="position">
		/// Parameter that describes the position between this normal and another.
		/// </param>
		public void SphericalLinearInterpolation(ref CryMeshNormal other, float position)
		{
			Vector3 normalA = this.normal;
			Vector3 normalB = other.normal;

			normalA.Normalize();
			normalB.Normalize();

			Interpolation.SphericalLinear.Apply(out this.normal, normalA, normalB, position);
		}
		#endregion
	}
}