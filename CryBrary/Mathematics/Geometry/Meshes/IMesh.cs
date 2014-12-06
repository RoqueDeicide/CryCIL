using System.Collections.Generic;
using CryEngine.Mathematics.Geometry.Meshes.CSG;
using CryEngine.Mathematics.Graphics;
using CryEngine.StaticObjects;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common functionality of objects that provide access to meshes in CryEngine.
	/// </summary>
	public interface IMesh
	{
		#region General Mesh Operations
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
		bool Validate(List<string> errors = null, List<string> warnings = null);
		/// <summary>
		/// Makes CryEngine recognize any changes made to this mesh.
		/// </summary>
		/// <param name="staticObject">Static object that will host the mesh.</param>
		void Export(StaticObject staticObject);
		/// <summary>
		/// When implemented, converts this mesh to a BSP tree.
		/// </summary>
		/// <returns>BSP tree built from this mesh.</returns>
		BspNode<SplittableTriangle> ToBspTree();
		/// <summary>
		/// When implemented, changes this mesh to be perfect representation of the given BSP tree.
		/// </summary>
		/// <param name="tree">BSP tree to convert to this mesh.</param>
		void SetBsp(BspNode<SplittableTriangle> tree);
		/// <summary>
		/// Gets or sets (optionally) a list of locations of vertices that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<Vector3> Positions { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of faces that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<IndexedTriangleFace> Faces { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of indices that form faces that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<uint> Indices { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of locations of texture coordinates that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<Vector2> TextureCoordinates { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of primary colors of vertices that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<Color32> PrimaryColors { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of secondary colors of vertices that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<Color32> SecondaryColors { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of normals of vertices that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<Vector3> Normals { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of tangent space normals that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<ITangent> Tangents { get; set; }
		/// <summary>
		/// Gets or sets (optionally) a list of tangent space normals that comprise this mesh.
		/// </summary>
		IMeshDetailsCollection<IQTangent> QTangents { get; set; }
		#endregion
	}
}