using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates coordinates of a vertex on the texture map.
	/// </summary>
	public struct CryMeshTexturePosition : IEquatable<CryMeshTexturePosition>
	{
		#region Fields
		private float s, t;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the vector that contains the UV coordinates that are stored in this object.
		/// </summary>
		public Vector2 UV => new Vector2(this.s, this.t);
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="x">S-coordinate.</param>
		/// <param name="y">T-coordinate.</param>
		public CryMeshTexturePosition(float x, float y)
		{
			this.s = x;
			this.t = y;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="position">
		/// 2D vector which X and Y coordinates are used to initialize S and T coordinates respectively.
		/// </param>
		public CryMeshTexturePosition(Vector2 position)
		{
			this.s = position.X;
			this.t = position.Y;
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="position">
		/// 4D vector which X and Y coordinates are used to initialize S and T coordinates respectively.
		/// </param>
		public CryMeshTexturePosition(Vector4 position)
		{
			this.s = position.X;
			this.t = position.Y;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Fills given vector with coordinates within this object.
		/// </summary>
		/// <param name="position">
		/// Vector to fill. Its X and Y coordinates will be filled with S and T coordinates respectively.
		/// </param>
		public void Export(out Vector2 position)
		{
			position = new Vector2(this.s, this.t);
		}
		/// <summary>
		/// Fills given vector with coordinates within this object.
		/// </summary>
		/// <param name="position">
		/// Vector to fill. Its X and Y coordinates will be filled with S and T coordinates respectively,
		/// while Z will be equal to 0 and W - to 1.
		/// </param>
		public void Export(out Vector4 position)
		{
			position = new Vector4(this.s, this.t, 0, 1);
		}
		/// <summary>
		/// Provides coordinates that are stored in this object.
		/// </summary>
		/// <param name="x">Object to fill with S-coordinate.</param>
		/// <param name="y">Object to fill with T-coordinate.</param>
		public void Export(out float x, out float y)
		{
			x = this.s;
			y = this.t;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>True, if objects are equal.</returns>
		public bool Equals(CryMeshTexturePosition other)
		{
			return this.s == other.s && this.t == other.t;
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CryMeshTexturePosition && this.Equals((CryMeshTexturePosition)obj);
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return (this.s.GetHashCode() * 397) ^ this.t.GetHashCode();
			}
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are equal.</returns>
		public static bool operator ==(CryMeshTexturePosition left, CryMeshTexturePosition right)
		{
			return left.s == right.s && left.t == right.t;
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if objects are not equal.</returns>
		public static bool operator !=(CryMeshTexturePosition left, CryMeshTexturePosition right)
		{
			return left.s != right.s || left.t != right.t;
		}
		/// <summary>
		/// Determines whether left operand is less then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is less then the right one.</returns>
		public static bool operator <(CryMeshTexturePosition left, CryMeshTexturePosition right)
		{
			return left.s != right.s ? left.s < right.s : left.t < right.t;
		}
		/// <summary>
		/// Determines whether left operand is greater then the right one.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if left operand is greater then the right one.</returns>
		public static bool operator >(CryMeshTexturePosition left, CryMeshTexturePosition right)
		{
			return !(left < right) && left != right;
		}
		/// <summary>
		/// Determines whether this normal vector can be considered equal to another.
		/// </summary>
		/// <param name="other">  Another vector.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between 2 vectors is smaller then <paramref name="epsilon"/>
		/// </returns>
		public bool IsEquivalent(ref Vector2 other, float epsilon = 0.00005f)
		{
			return
				Math.Abs(this.s - other.X) <= epsilon &&
				Math.Abs(this.t - other.Y) <= epsilon;
		}
		/// <summary>
		/// Determines whether this normal vector can be considered equal to another.
		/// </summary>
		/// <param name="other">  Another vector.</param>
		/// <param name="epsilon">Precision of comparison.</param>
		/// <returns>
		/// True, if difference between 2 vectors is smaller then <paramref name="epsilon"/>
		/// </returns>
		public bool IsEquivalent(ref CryMeshTexturePosition other, float epsilon = 0.00005f)
		{
			return
				Math.Abs(this.s - other.s) <= epsilon &&
				Math.Abs(this.t - other.t) <= epsilon;
		}
		/// <summary>
		/// Changes this vector to be a result of linear interpolation between its current position and a
		/// given one.
		/// </summary>
		/// <param name="endPoint">Other vector that represents the interpolation 'end point'.</param>
		/// <param name="position">
		/// Parameter that specifies position of the interpolated value relative to this one and 'end
		/// point'.
		/// </param>
		public void LinearInterpolation(ref CryMeshTexturePosition endPoint, float position)
		{
			Vector2 start, end;

			this.Export(out start);
			endPoint.Export(out end);

			Interpolation.Linear.Apply(out start, start, end, position);

			this.s = start.X;
			this.t = start.Y;
		}
		#endregion
	}
}