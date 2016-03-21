using System;
using System.Linq;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex that is defined by <see cref="VertexFormat.P3F_P3F_I4B"/>.
	/// </summary>
	public struct P3F_P3F_I4B
	{
		/// <summary>
		/// Coordinates of thin part of the bone.
		/// </summary>
		public Vector3 Thin;
		/// <summary>
		/// Coordinates of fat part of the bone.
		/// </summary>
		public Vector3 Fat;
		/// <summary>
		/// Indices of bones this one connects to(?).
		/// </summary>
		public ColorByte Indices;
	}
}