using System;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Graphics;

namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates data about a single vertex.
	/// </summary>
	public struct FullVertex : IEquatable<FullVertex>
	{
		#region Fields
		/// <summary>
		/// Position of the vertex in the world.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Vector that is perpendicular to the "surface" this vertex is on.
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// Position of this vertex on the UV map.
		/// </summary>
		public Vector2 UvPosition;
		/// <summary>
		/// Primary color of this vertex.
		/// </summary>
		public ColorByte PrimaryColor;
		/// <summary>
		/// Secondary color of this vertex.
		/// </summary>
		public ColorByte SecondaryColor;
		#endregion
		#region Construction
		/// <summary>
		/// Creates an object that stores information about a vertex that is extracted from CryEngine mesh.
		/// </summary>
		/// <param name="cryMesh">CryEngine mesh to extract information from.</param>
		/// <param name="index">  Zero-based index of the vertex to get.</param>
		public FullVertex(CryMesh cryMesh, int index)
			: this()
		{
			var positions = cryMesh.Vertexes.Positions;
			if (positions.IsValid)
			{
				this.Position = positions[index];
			}
			var normals = cryMesh.Vertexes.Normals;
			if (normals.IsValid)
			{
				this.Normal = normals[index].Normal;
			}
			var primaryColors = cryMesh.Vertexes.PrimaryColors;
			if (primaryColors.IsValid)
			{
				primaryColors[index].Export(out this.PrimaryColor);
			}
			var secondaryColors = cryMesh.Vertexes.SecondaryColors;
			if (secondaryColors.IsValid)
			{
				secondaryColors[index].Export(out this.SecondaryColor);
			}
			var uvPositions = cryMesh.TexturePositions;
			if (uvPositions.IsValid)
			{
				this.UvPosition = uvPositions[index].UV;
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this vertex is equal to another.
		/// </summary>
		/// <param name="other">Another vertex.</param>
		/// <returns>True, if both vertexes can be considered equal.</returns>
		public bool Equals(FullVertex other)
		{
			return this.Position.Equals(other.Position) && this.Normal.Equals(other.Normal) &&
				   this.UvPosition.Equals(other.UvPosition) && this.PrimaryColor.Equals(other.PrimaryColor) &&
				   this.SecondaryColor.Equals(other.SecondaryColor);
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>True, if both objects are of the same type and are equal.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			return obj is FullVertex && this.Equals((FullVertex)obj);
		}
		/// <summary>
		/// Gets the hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = this.Position.GetHashCode();
				hashCode = (hashCode * 397) ^ this.Normal.GetHashCode();
				hashCode = (hashCode * 397) ^ this.UvPosition.GetHashCode();
				hashCode = (hashCode * 397) ^ this.PrimaryColor.GetHashCode();
				hashCode = (hashCode * 397) ^ this.SecondaryColor.GetHashCode();
				return hashCode;
			}
		}
		/// <summary>
		/// Determines whether 2 objects are not equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 operands are not equal.</returns>
		public static bool operator ==(FullVertex left, FullVertex right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines whether 2 objects are equal.
		/// </summary>
		/// <param name="left">Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>True, if 2 operands are equal.</returns>
		public static bool operator !=(FullVertex left, FullVertex right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}