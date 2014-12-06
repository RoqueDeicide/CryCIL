using System;
using System.Collections.Generic;
using System.Linq;
using CryEngine.Mathematics.Graphics;
using CryEngine.StaticObjects;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Base class for all meshes.
	/// </summary>
	public abstract partial class Mesh : IMesh
	{
		#region Abstracts
		/// <summary>
		/// Checks this mesh for problems.
		/// </summary>
		/// <param name="errors">  
		/// List that will contain all problems that make exporting this mesh impossible.
		/// </param>
		/// <param name="warnings">
		/// List that will contain all problems that don't make exporting impossible but can still
		/// cause glitches and may crash the program.
		/// </param>
		/// <returns>
		/// Indication of possibility of this mesh being recognized by CryEngine as a valid one.
		/// </returns>
		public abstract bool Validate(List<string> errors = null, List<string> warnings = null);
		/// <summary>
		/// Makes CryEngine recognize any changes made to this mesh.
		/// </summary>
		/// <param name="staticObject"> Static object that will host the mesh. </param>
		public abstract void Export(StaticObject staticObject);
		/// <summary>
		/// Gets or sets (optionally) a list of locations of vertices that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<Vector3> Positions { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of faces that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<IndexedTriangleFace> Faces { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of indices that form faces that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<uint> Indices { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of locations of texture coordinates that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<Vector2> TextureCoordinates { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of primary colors of vertices that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<Color32> PrimaryColors { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of secondary colors of vertices that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<Color32> SecondaryColors { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of normals of vertices that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<Vector3> Normals { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of tangent space normals that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<ITangent> Tangents { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of tangent space normals that comprise this mesh.
		/// </summary>
		public abstract IMeshDetailsCollection<IQTangent> QTangents { get; set; }
		#endregion
		/// <summary>
		/// Calculates a volume of this mesh using a list of faces.
		/// </summary>
		/// <remarks>
		/// Volume of triangle mesh is a sum of mixed products of vertices of all faces.
		/// </remarks>
		public virtual double Volume
		{
			get
			{
				double volume = 0;

				try
				{
					// Better cash the collections, just in case the mesh doesn't do it. If it does,
					// then we just create more references to the cashes.
					Vector3[] positions = this.Positions.ToArray();
					IndexedTriangleFace[] faces = this.Faces.ToArray();

					volume =
					(
						from face in faces
						select Vector3.Mixed
						(
							positions[face.Indices[0]],
							positions[face.Indices[1]],
							positions[face.Indices[2]]
						) / 6
					).Sum();
				}
				catch (NotSupportedException)
				{
					Debug.LogWarning("Unable to calculate volume of the mesh of type <{0}>: Retrieving a list of faces or vertices is not supported.", this.GetType().Name);
				}
				return volume;
			}
		}
	}
}