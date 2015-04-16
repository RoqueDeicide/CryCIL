using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Encapsulates information about a vertex using SVF_P3F_C4B_T2F format.
	/// </summary>
	public struct VertexPosition3FColor4BTex2F
	{
		/// <summary>
		/// Coordinates that describe location of this vertex in 3D world space.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Color of the vertex.
		/// </summary>
		public ColorByte Color;
		/// <summary>
		/// Coordinates that describe location of this vertex on the UV map.
		/// </summary>
		public Vector2 TextureCoordinates;
	}
}
