using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics.Graphics;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Encapsulates data that describes a vertex in the triangular mesh.
	/// </summary>
	public struct MeshVertex : IVertex3D, IComparable<MeshVertex>
	{
		#region Fields
		/// <summary>
		/// <see cref="Vector3"/> object that describes Cartesian coordinates of this vertex in 3D space.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// <see cref="Vector3"/> object that describes direction of the normal to hypothetical surface
		/// this vertex is part of.
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// <see cref="Vector2"/> object that describes Cartesian coordinates of this vertex in 2D UV map.
		/// </summary>
		public Vector2 UvMapPosition;
		/// <summary>
		/// Primary color of this vertex in RGBA format.
		/// </summary>
		public Color32 PrimaryColor;
		/// <summary>
		/// Secondary color of this vertex in RGBA format.
		/// </summary>
		public Color32 SecondaryColor;
		#endregion
		#region Properties
		/// <summary>
		/// Creates a deep copy of this vertex. (Just returns a new instance).
		/// </summary>
		public IVertex Clone
		{
			get { return this; }
		}
		/// <summary>
		/// Gets 3-dimensional vector that represents location of this vertex in 3D space.
		/// </summary>
		public Vector3 Location
		{
			get { return this.Position; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates a linear interpolation of this vertex.
		/// </summary>
		/// <param name="anotherVertex">Another vertex used in interpolation.</param>
		/// <param name="position">     Interpolation parameter.</param>
		/// <returns>
		/// A new vertex located on the line between this vertex and another one, with relative position
		/// described by an interpolation parameter.
		/// </returns>
		public IVertex CreateLinearInterpolation(IVertex anotherVertex, float position)
		{
			if (anotherVertex is MeshVertex)
			{
				return this.CreateLinearInterpolation((MeshVertex)anotherVertex, position);
			}
			throw new ArgumentException
			(
				String.Format
				(
					"Cannot interpolate between instances of {0} and {1}.",
					typeof(MeshVertex).FullName, anotherVertex.GetType().FullName
				)
			);
		}
		/// <summary>
		/// Creates a linear interpolation of this vertex.
		/// </summary>
		/// <param name="anotherVertex">Another vertex used in interpolation.</param>
		/// <param name="position">     Interpolation parameter.</param>
		/// <returns>
		/// A new vertex located on the line between this vertex and another one, with relative position
		/// described by an interpolation parameter.
		/// </returns>
		public IVertex CreateLinearInterpolation(MeshVertex anotherVertex, float position)
		{
			return new MeshVertex
			{
				Position = Vector3.CreateLinearInterpolation(this.Position, anotherVertex.Position, position),
				Normal = Vector3.CreateLinearInterpolation(this.Normal, anotherVertex.Normal, position).Normalized,
				UvMapPosition = Vector2.CreateLinearInterpolation(this.UvMapPosition, anotherVertex.UvMapPosition, position),
				PrimaryColor = Color32.CreateLinearInterpolation(this.PrimaryColor, anotherVertex.PrimaryColor, position),
				SecondaryColor = Color32.CreateLinearInterpolation(this.SecondaryColor, anotherVertex.SecondaryColor, position)
			};
		}
		/// <summary>
		/// Creates spherical linear interpolation of this vertex and another.
		/// </summary>
		/// <param name="anotherVertex">Another vertex.</param>
		/// <param name="position">     Interpolation parameter.</param>
		/// <returns>A result of interpolation.</returns>
		/// <seealso cref="CreateSphericalInterpolation(MeshVertex, float)"/>
		public IVertex CreateSphericalInterpolation(IVertex anotherVertex, float position)
		{
			if (anotherVertex is MeshVertex)
			{
				return this.CreateSphericalInterpolation((MeshVertex)anotherVertex, position);
			}
			throw new ArgumentException
			(
				String.Format
				(
					"Cannot interpolate between instances of {0} and {1}.",
					typeof(MeshVertex).FullName, anotherVertex.GetType().FullName
				)
			);
		}
		/// <summary>
		/// Creates spherical linear interpolation of this vertex and another.
		/// </summary>
		/// <remarks>
		/// <para>Terminology</para>
		/// <para>1) Ray1 - a ray from origin of coordinates that contains this vertex.</para>
		/// <para>2) Ray2 - a ray from origin of coordinates that contains another vertex.</para>
		/// <para>3) Ray3 - a ray from origin of coordinates that contains interpolated vertex.</para>
		/// <para></para>
		/// <para>
		/// The angle between Ray1 and Ray3 is a fraction of the angle between Ray1 and Ray2 that is equal
		/// to a <paramref name="position"/> parameter.
		/// </para>
		/// <para></para>
		/// <para>
		/// Interpolation is only applied to <see cref="MeshVertex.Position"/> ,
		/// <see cref="MeshVertex.Normal"/> and <see cref="MeshVertex.UvMapPosition"/> , the rest are
		/// linearly interpolated.
		/// </para>
		/// </remarks>
		/// <param name="anotherVertex">Another vertex.</param>
		/// <param name="position">     Interpolation parameter.</param>
		/// <returns>A result of interpolation.</returns>
		public IVertex CreateSphericalInterpolation(MeshVertex anotherVertex, float position)
		{
			return new MeshVertex
			{
				Position = Vector3.CreateSphericalInterpolation(this.Position, anotherVertex.Position, position),
				Normal = Vector3.CreateSphericalInterpolation(this.Normal, anotherVertex.Normal, position).Normalized,
				UvMapPosition = Vector2.CreateSphericalInterpolation(this.UvMapPosition, anotherVertex.UvMapPosition, position),
				PrimaryColor = Color32.CreateLinearInterpolation(this.PrimaryColor, anotherVertex.PrimaryColor, position),
				SecondaryColor = Color32.CreateLinearInterpolation(this.SecondaryColor, anotherVertex.SecondaryColor, position)
			};
		}
		/// <summary>
		/// Flips normal of this vertex.
		/// </summary>
		public void Flip()
		{
			this.Normal.Flip();
		}
		/// <summary>
		/// Normalizes position of this vertex in 3D space and on UV map.
		/// </summary>
		public void Normalize()
		{
			this.Position.Normalize();
			this.UvMapPosition.Normalize();
		}
		/// <summary>
		/// Determines order of this object in a sorted sequence relative to another.
		/// </summary>
		/// <param name="other">Another vertex.</param>
		/// <returns></returns>
		public int CompareTo(MeshVertex other)
		{
			int pos = this.Position.CompareTo(other.Position);
			if (pos != 0) return pos;
			pos = this.Normal.CompareTo(other.Normal);
			if (pos != 0) return pos;
			pos = this.UvMapPosition.CompareTo(other.UvMapPosition);
			if (pos != 0) return pos;
			pos = this.PrimaryColor.Bytes.SignedInt.CompareTo(other.PrimaryColor.Bytes.SignedInt);
			return
				pos != 0 ? pos : this.SecondaryColor.Bytes.SignedInt.CompareTo(other.SecondaryColor.Bytes.SignedInt);
		}
		#endregion
	}
}