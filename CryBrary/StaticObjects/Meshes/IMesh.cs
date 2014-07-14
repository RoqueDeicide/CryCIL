using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
{
	/// <summary>
	/// Defines common functionality of objects that provide access to meshes in CryEngine.
	/// </summary>
	public interface IMesh
	{
		#region Mesh Editing
		/// <summary>
		/// Gets aggregate of all data about one of the vertices.
		/// </summary>
		/// <param name="index">Index of the vertex to get.</param>
		/// <returns>Object of type <see cref="Vertex" /> that contains all relevant data.</returns>
		Vertex GetVertex(int index);
		/// <summary>
		/// Sets a bunch of data at once for one of the vertices.
		/// </summary>
		/// <param name="index">Index of the vertex to set.</param>
		/// <param name="vertex">Object of type <see cref="Vertex" /> that provides the data.</param>
		void SetVertex(int index, Vertex vertex);
		/// <summary>
		/// Gets a triangular face.
		/// </summary>
		/// <param name="index">Index of the face to get.</param>
		/// <returns>
		/// Object that contains indices of three vertices that comprise the face and index of mesh
		/// subset this face belongs to.
		/// </returns>
		Face GetFace(int index);
		/// <summary>
		/// Updates data about one of the faces.
		/// </summary>
		/// <param name="index">Index of the face to update.</param>
		/// <param name="face">New face data.</param>
		void SetFace(int index, Face face);
		/// <summary>
		/// Gets position of the vertex.
		/// </summary>
		/// <param name="index">Index of the vertex which location we need.</param>
		/// <returns>
		/// Position of the vertex in relation to position of the static object that hosts this mesh.
		/// </returns>
		Vector3 GetVertexPosition(int index);
		/// <summary>
		/// Changes position of one of the vertices.
		/// </summary>
		/// <param name="index">Index of the vertex to move.</param>
		/// <param name="position">
		/// New location of the vertex in relation to position of the static object that hosts this mesh.
		/// </param>
		void SetVertexPosition(int index, Vector3 position);
		/// <summary>
		/// Gets object-space normal vector.
		/// </summary>
		/// <param name="index">Index of the vertex associated with requested normal.</param>
		/// <returns>Object or world-space normal vector.</returns>
		Vector3 GetNormal(int index);
		/// <summary>
		/// Sets object-space normal vector.
		/// </summary>
		/// <param name="index">Index of the vertex associated with requested normal.</param>
		/// <param name="normal">New direction for normal vector.</param>
		void SetNormal(int index, Vector3 normal);
		/// <summary>
		/// Gets position of the vertex on UV map.
		/// </summary>
		/// <param name="index">Index of the vertex.</param>
		/// <returns>Vector that points at location of the vertex on UV map.</returns>
		Vector2 GetTextureCoordinates(int index);
		/// <summary>
		/// Moves vertex on UV map.
		/// </summary>
		/// <param name="index">Index of the vertex to move.</param>
		/// <param name="coordinates">New location of the vertex on UV map.</param>
		void SetTextureCoordinates(int index, Vector2 coordinates);
		/// <summary>
		/// Gets index of the vertex assigned to one of the faces.
		/// </summary>
		/// <remarks>
		/// Index of the face that will be affected by this call is Floor(indexPosition / 3).
		///
		/// Index of vertex within the face that will be returned is indexPosition % 3.
		///
		/// This means that if you need to get index of the vertex that is second vertex of 5th
		/// face, indexPosition should be 4 * 3 + 1 = 13;
		///
		/// It is highly recommended to use <see cref="GetFace" />, if that operation is supported,
		/// so you don't mess up face mappings. Only use this when you are working with render mesh directly.
		/// </remarks>
		/// <param name="indexPosition">Index of the vertex index in vertices array.</param>
		/// <returns>Index of the vertex that is used as one of the vertices of a face.</returns>
		int GetIndex(int indexPosition);
		/// <summary>
		/// Sets index of the vertex assigned to one of the faces.
		/// </summary>
		/// <remarks>
		/// Index of the face that will be affected by this call is Floor(indexPosition / 3).
		///
		/// Index of vertex within the face that will be set is indexPosition % 3.
		///
		/// This means that if you need to change index of the vertex that is second vertex of 5th
		/// face, indexPosition should be 4 * 3 + 1 = 13;
		///
		/// It is highly recommended to use <see cref="SetFace" />, if that operation is supported,
		/// so you don't mess up face mappings. Only use this when you are working with render mesh directly.
		/// </remarks>
		/// <param name="indexPosition">Index of the vertex index in vertices array.</param>
		/// <param name="index">
		/// New index of the vertex that is used as one of the vertices of a face.
		/// </param>
		void SetIndex(int indexPosition, int index);
		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ITangent GetTangent(int index);
		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="tangent"></param>
		void SetTangent(int index, ITangent tangent);
		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IQTangent GetQTangent(int index);
		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="tangent"></param>
		void SetQTangent(int index, IQTangent tangent);
		#endregion
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
		void Validate(List<string> errors, List<string> warnings);
		/// <summary>
		/// Makes CryEngine recognize any changes made to this mesh.
		/// </summary>
		void Export();
		#endregion
	}
}