using System;
using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates a pair of tangent-space normals.
	/// </summary>
	public struct CryMeshTangents : IEquatable<CryMeshTangents>
	{
		#region Fields
		private Vector4Int16 tangent;
		private Vector4Int16 bitangent;
		#endregion
		#region Properties
		/// <summary>
		/// Extracts normal vector out of this pair of tangents.
		/// </summary>
		public Vector3 Normal
		{
			get
			{
				Vector3 n;
				this.Export(out n);
				return n;
			}
		}
		/// <summary>
		/// Extracts orientation of normal vector relative to this pair of tangents.
		/// </summary>
		public short Sign
		{
			get { return Quantize.Int16ToInt16Reverse(this.tangent.W); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="tangent">  A vector to use to initialize a tangent.</param>
		/// <param name="bitangent">A vector to use to initialize a bitangent.</param>
		public CryMeshTangents(Vector4Int16 tangent, Vector4Int16 bitangent)
		{
			this.tangent = tangent;
			this.bitangent = bitangent;
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="tangent">  A vector to use to initialize a tangent.</param>
		/// <param name="bitangent">A vector to use to initialize a bitangent.</param>
		public CryMeshTangents(Vector4 tangent, Vector4 bitangent)
		{
			this.tangent = Quantize.SingleToInt16(tangent);
			this.bitangent = Quantize.SingleToInt16(bitangent);
		}
		/// <summary>
		/// Packs information about orientation of the point on the surface into a pair of tangents.
		/// </summary>
		/// <param name="tangent">  3D vector that is tangential to the surface.</param>
		/// <param name="bitangent">Another 3D vector that is tangential to the surface.</param>
		/// <param name="normal">   3D vector that is perpendicular to the surface.</param>
		public CryMeshTangents(Vector3 tangent, Vector3 bitangent, Vector3 normal)
		{
			short sign = 1;

			if ((tangent % bitangent * normal) < 0)
			{
				sign = -1;
			}

			this.tangent = new Vector4Int16(Quantize.SingleToInt16(tangent.X), Quantize.SingleToInt16(tangent.Y),
											Quantize.SingleToInt16(tangent.Z), Quantize.Int16ToInt16(sign));
			this.bitangent = new Vector4Int16(Quantize.SingleToInt16(bitangent.X), Quantize.SingleToInt16(bitangent.Y),
											  Quantize.SingleToInt16(bitangent.Z), Quantize.Int16ToInt16(sign));
		}
		/// <summary>
		/// Packs information about orientation of the point on the surface into a pair of tangents.
		/// </summary>
		/// <param name="tangent">  3D vector that is tangential to the surface.</param>
		/// <param name="bitangent">Another 3D vector that is tangential to the surface.</param>
		/// <param name="sign">     
		/// A value that indicates orientation of the normal vector relative to both tangents.
		/// </param>
		public CryMeshTangents(Vector3 tangent, Vector3 bitangent, short sign)
		{
			this.tangent = new Vector4Int16(Quantize.SingleToInt16(tangent.X), Quantize.SingleToInt16(tangent.Y),
											Quantize.SingleToInt16(tangent.Z), Quantize.Int16ToInt16(sign));
			this.bitangent = new Vector4Int16(Quantize.SingleToInt16(bitangent.X), Quantize.SingleToInt16(bitangent.Y),
											  Quantize.SingleToInt16(bitangent.Z), Quantize.Int16ToInt16(sign));
		}
		#endregion
		#region Interface
		/// <summary>
		/// Exports both tangents.
		/// </summary>
		/// <param name="tangent">  First tangent.</param>
		/// <param name="bitangent">Second tangent.</param>
		public void Export(out Vector4Int16 tangent, out Vector4Int16 bitangent)
		{
			tangent = this.tangent;
			bitangent = this.bitangent;
		}
		/// <summary>
		/// Exports both tangents.
		/// </summary>
		/// <param name="tangent">  First tangent.</param>
		/// <param name="bitangent">Second tangent.</param>
		public void Export(out Vector4 tangent, out Vector4 bitangent)
		{
			tangent = Quantize.Int16ToSingle(this.tangent);
			bitangent = Quantize.Int16ToSingle(this.bitangent);
		}
		/// <summary>
		/// Exports both tangents.
		/// </summary>
		/// <param name="tangent">  First tangent.</param>
		/// <param name="bitangent">Second tangent.</param>
		public void Export(out Vector3 tangent, out Vector3 bitangent)
		{
			Vector4 tan = Quantize.Int16ToSingle(this.tangent);
			Vector4 bitan = Quantize.Int16ToSingle(this.bitangent);

			tangent = new Vector3(tan.X, tan.Y, tan.Z);
			bitangent = new Vector3(bitan.X, bitan.Y, bitan.Z);
		}
		/// <summary>
		/// Extracts normal vector out of this pair of tangents.
		/// </summary>
		/// <param name="normal">Extracted normal.</param>
		public void Export(out Vector3 normal)
		{
			Vector3 tangent, bitangent;

			this.Export(out tangent, out bitangent);

			normal = tangent % bitangent * this.tangent.W;
		}
		/// <summary>
		/// Exports both tangents and extracts normal vector.
		/// </summary>
		/// <param name="tangent">  First tangent.</param>
		/// <param name="bitangent">Second tangent.</param>
		/// <param name="normal">   Extracted normal.</param>
		public void Export(out Vector3 tangent, out Vector3 bitangent, out Vector3 normal)
		{
			this.Export(out tangent, out bitangent);
			this.Export(out normal);
		}
		/// <summary>
		/// Exports a sign that indicates orientation of the normal vector relative to the tangents.
		/// </summary>
		/// <param name="sign">Sign.</param>
		public void Export(out short sign)
		{
			sign = Quantize.Int16ToInt16Reverse(this.tangent.W);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(CryMeshTangents other)
		{
			return this.tangent.Equals(other.tangent) && this.bitangent.Equals(other.bitangent);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CryMeshTangents && this.Equals((CryMeshTangents)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.tangent.GetHashCode() * 397) ^ this.bitangent.GetHashCode();
			}
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are equal.</returns>
		public static bool operator ==(CryMeshTangents left, CryMeshTangents right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are not equal.</returns>
		public static bool operator !=(CryMeshTangents left, CryMeshTangents right)
		{
			return !left.Equals(right);
		}
		/// <summary>
		/// Determines whether given information about orientation of the surface at the point can be
		/// described with this set of tangents.
		/// </summary>
		/// <param name="tangent">  First tangent.</param>
		/// <param name="bitangent">Second tangent.</param>
		/// <param name="sign">     
		/// Value that specifies orientation of the normal to the surface relative to tangents.
		/// </param>
		/// <param name="epsilon">  Precision of comparison.</param>
		/// <returns>
		/// True, if given information about orientation of the surface at the point can be described with
		/// this set of tangents.
		/// </returns>
		public bool IsEquivalent(ref Vector3 tangent, ref Vector3 bitangent, ref short sign, float epsilon = 0.01f)
		{
			if (this.tangent.W != sign || this.bitangent.W != sign)
			{
				return false;
			}

			Vector3 tan = new Vector3(this.tangent.X, this.tangent.Y, this.tangent.Z);
			Vector3 bitan = new Vector3(this.bitangent.X, this.bitangent.Y, this.bitangent.Z);

			return tan * tangent >= (1 - epsilon) &&
				   bitan * bitangent >= (1 - epsilon);
		}
		/// <summary>
		/// Applies transformation to this pair of tangents.
		/// </summary>
		/// <param name="matrix">3x3 matrix that represents transformation.</param>
		public void Transform(ref Matrix33 matrix)
		{
			Vector3 tangent, bitangent;

			this.Export(out tangent, out bitangent);

			Transformation.Apply(ref tangent, ref matrix);
			Transformation.Apply(ref bitangent, ref matrix);

			this = new CryMeshTangents(tangent, bitangent, this.Sign);
		}
		/// <summary>
		/// Applies transformation to this pair of tangents and normalizes them.
		/// </summary>
		/// <param name="matrix">3x3 matrix that represents transformation.</param>
		public void TransformNormalize(ref Matrix33 matrix)
		{
			Vector3 tangent, bitangent;

			this.Export(out tangent, out bitangent);

			Transformation.Apply(ref tangent, ref matrix);
			Transformation.Apply(ref bitangent, ref matrix);

			tangent.Normalize();
			bitangent.Normalize();

			this = new CryMeshTangents(tangent, bitangent, this.Sign);
		}
		/// <summary>
		/// Applies transformation to this pair of tangents.
		/// </summary>
		/// <param name="matrix">3x4 matrix that represents transformation.</param>
		public void Transform(ref Matrix34 matrix)
		{
			Vector3 tangent, bitangent;

			this.Export(out tangent, out bitangent);

			Transformation.Apply(ref tangent, ref matrix);
			Transformation.Apply(ref bitangent, ref matrix);

			this = new CryMeshTangents(tangent, bitangent, this.Sign);
		}
		/// <summary>
		/// Applies transformation to this pair of tangents and normalizes them.
		/// </summary>
		/// <param name="matrix">3x4 matrix that represents transformation.</param>
		public void TransformNormalize(ref Matrix34 matrix)
		{
			Vector3 tangent, bitangent;

			this.Export(out tangent, out bitangent);

			Transformation.Apply(ref tangent, ref matrix);
			Transformation.Apply(ref bitangent, ref matrix);

			tangent.Normalize();
			bitangent.Normalize();

			this = new CryMeshTangents(tangent, bitangent, this.Sign);
		}
		/// <summary>
		/// Applies spherical linear interpolation to this pair of tangents.
		/// </summary>
		/// <param name="other">   Pair of tangents that represents "destination" of interpolation.</param>
		/// <param name="normal">  Normal vector to use for the final result.</param>
		/// <param name="position">
		/// Parameter that describes the position between this pair of tangents and another.
		/// </param>
		public void SphericalLinearInterpolation(ref CryMeshTangents other, ref CryMeshNormal normal, float position)
		{
			Vector3 tangent0, bitangent0;
			Vector3 tangent1, bitangent1;

			this.Export(out tangent0, out bitangent0);
			other.Export(out tangent1, out bitangent1);

			Interpolation.SphericalLinear.Apply(out tangent0, tangent0, tangent1, position);
			Interpolation.SphericalLinear.Apply(out bitangent0, bitangent0, bitangent1, position);

			this = new CryMeshTangents(tangent0, bitangent0, normal.Normal);
		}
		#endregion
	}
}