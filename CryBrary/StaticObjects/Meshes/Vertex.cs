using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Encapsulates data about one vertex in the mesh.
	/// </summary>
	public struct Vertex
	{
		private Vector3 position;
		private Vector3 normal;
		private Color32 color0;
		private Color32 color1;
		private int vertexMat;
		private Vector2 texCoords;
		/// <summary>
		/// Location of the vertex relative to location of static object that hosts the mesh.
		/// </summary>
		public Vector3 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}
		/// <summary>
		/// Normal vector associated with this vertex.
		/// </summary>
		public Vector3 Normal
		{
			get { return this.normal; }
			set { this.normal = value; }
		}
		/// <summary>
		/// First color associated with this vertex.
		/// </summary>
		public Color32 Color0
		{
			get { return this.color0; }
			set { this.color0 = value; }
		}
		/// <summary>
		/// Second color associated with this vertex.
		/// </summary>
		public Color32 Color1
		{
			get { return this.color1; }
			set { this.color1 = value; }
		}
		/// <summary>
		/// Currently unknown data associated with this vertex.
		///
		/// Presumably this is index of the material.
		/// </summary>
		public int VertexMat
		{
			get { return this.vertexMat; }
			set { this.vertexMat = value; }
		}
		/// <summary>
		/// Position of this vertex on UV map.
		/// </summary>
		public Vector2 TextureCoordinates
		{
			get { return this.texCoords; }
			set { this.texCoords = value; }
		}
	}
}