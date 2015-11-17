using System;
using System.Collections.Generic;

namespace CryCil
{
	public partial struct Vector3d
	{
		#region IVector
		/// <summary>
		/// Creates deep copy of this vector.
		/// </summary>
		Vector3d IVector<double, Vector3d>.DeepCopy
		{
			get { return this; }
		}
		/// <summary>
		/// Calculates the dot product of this vector and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Dot product of the 2 vectors.</returns>
		double IVector<double, Vector3d>.Dot(Vector3d other)
		{
			return this * other;
		}
		/// <summary>
		/// Adds components from another vector to respective components of this one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		void IVector<double, Vector3d>.Add(Vector3d other)
		{
			this.X += other.X;
			this.Y += other.Y;
			this.Z += other.Z;
		}
		/// <summary>
		/// Multiplies components of this vector by the given factor.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		void IVector<double, Vector3d>.Scale(double factor)
		{
			this.X *= factor;
			this.Y *= factor;
			this.Z *= factor;
		}
		/// <summary>
		/// Creates a new vector that is a sum of this one and another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>Sum of two vectors.</returns>
		Vector3d IVector<double, Vector3d>.Added(Vector3d other)
		{
			return this + other;
		}
		/// <summary>
		/// Creates a new vector that is a scaled version of this one.
		/// </summary>
		/// <param name="factor">Scaling factor.</param>
		/// <returns>Scaled vector.</returns>
		Vector3d IVector<double, Vector3d>.Scaled(double factor)
		{
			return this * factor;
		}
		#endregion
		#region IEnumerable<double>
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		public IEnumerator<double> GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
		}
		/// <summary>
		/// Enumerates this vector.
		/// </summary>
		/// <returns>Yields components of this vector.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			yield return this.X;
			yield return this.Y;
			yield return this.Z;
		}
		#endregion
		#region IEquatable<Vector3d>
		/// <summary>
		/// Determines whether this vector is equal to another one.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>True, if vectors are equal.</returns>
		public bool Equals(Vector3d other)
		{
			return this.IsEquivalent(other, MathHelpers.ZeroTolerance);
		}
		#endregion
		#region IComparable<Vector3d>
		/// <summary>
		/// Determines position of this vector relative to another in a sorted sequence.
		/// </summary>
		/// <param name="other">Another vector.</param>
		/// <returns>
		/// <para><c>1</c> - Means that this vector should succeed another.</para>
		/// <para><c>0</c> - Means that both vectors are equal.</para>
		/// <para><c>-1</c> - Means that this vector should preceed another.</para>
		/// </returns>
		public int CompareTo(Vector3d other)
		{
			int pos = this.X.CompareTo(other.X);
			if (pos != 0) return pos;
			pos = this.Y.CompareTo(other.Y);
			return pos == 0 ? this.Z.CompareTo(other.Z) : pos;
		}
		#endregion
	}
}