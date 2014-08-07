using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine.StaticObjects.Meshes
{
	// / <summary> /// Defines interface with a collection of positions of vertices. /// </summary>
	// public interface IVertexPositionCollection : IList<Vector3>, IMeshDetailsCollection<Vector3>
	// { } /// <summary> /// Defines interface of mesh faces collection. /// </summary> public
	// interface IFacesCollection : IList<IndexedTriangleFace>,
	// IMeshDetailsCollection<IndexedTriangleFace> { } /// <summary> /// Defines interface of the
	// collection that contains indices that comprise faces of this mesh. /// </summary> public
	// interface IIndicesCollection : IList<uint>, IMeshDetailsCollection<uint> { } /// <summary>
	// /// Defines interface with a collection of normals of vertices. /// </summary> public
	// interface IVertexNormalCollection : IList<Vector3>, IMeshDetailsCollection<Vector3> { } ///
	// <summary> /// Defines interface with a collection of vectors that designate locations of
	// vertices on UV map. /// </summary> public interface IVertexTextureCoordinateCollection :
	// IList<Vector2>, IMeshDetailsCollection<Vector2> { } /// <summary> /// Defines interface with
	// a collection of tangents of vertices. /// </summary> public interface
	// IVertexTangentCollection : IList<ITangent>, IMeshDetailsCollection<ITangent> { } ///
	// <summary> /// Defines interface with a collection of qtangents of vertices. /// </summary>
	// public interface IVertexQTangentCollection : IList<IQTangent>,
	// IMeshDetailsCollection<IQTangent> { } /// <summary> /// Defines interface with a collection
	// of primary colors of vertices. /// </summary> public interface IVertexColor0Collection :
	// IList<Color32>, IMeshDetailsCollection<Color32> { } /// <summary> /// Defines interface with
	// a collection of secondary colors of vertices. /// </summary> public interface
	// IVertexColor1Collection : IList<Color32>, IMeshDetailsCollection<Color32> { }
}