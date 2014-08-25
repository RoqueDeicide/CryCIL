using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CryEngine.Mathematics.Geometry.Meshes.CSG;
using CryEngine.Native;
using CryEngine.StaticObjects;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Represents a wrapper object that allows to edit CryEngine meshes located in native memory.
	/// </summary>
	/// <remarks>
	/// This particular class uses instance of CMesh class in native memory for its operations.
	/// </remarks>
	public class NativeMesh : Mesh
	{
		#region Fields
		private readonly NativeFaceCollection faces;
		private readonly NativeIndicesCollection indices;
		private readonly NativeVertexPositionCollection positions;
		private readonly NativeVertexNormalCollection normals;
		private readonly NativeVertexTextureCoordinatesCollection texCoords;
		private readonly NativeVertexColor0Collection colors0;
		private readonly NativeVertexColor1Collection colors1;
		private readonly NativeVertexTangentCollection tangents;
		private readonly NativeVertexQTangentCollection qTangents;
		#endregion
		#region Properties
		/// <summary>
		/// Gets static object that hosts this mesh.
		/// </summary>
		public StaticObject StaticObject { get; private set; }
		/// <summary>
		/// Gets pointer to CMesh structure in native memory.
		/// </summary>
		public IntPtr CMeshHandle { get; private set; }
		/// <summary>
		/// Gets pointer to IndexedMesh structure in native memory.
		/// </summary>
		public IntPtr IndexedMeshHandle { get; private set; }
		/// <summary>
		/// Gets an object that provides access to positions of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<Vector3> Positions
		{
			get { return this.positions; }
			set { this.positions.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to faces in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<IndexedTriangleFace> Faces
		{
			get { return this.faces; }
			set { this.faces.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to indices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<uint> Indices
		{
			get { return this.indices; }
			set { this.indices.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to positions of vertices on UV map in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<Vector2> TextureCoordinates
		{
			get { return this.texCoords; }
			set { this.texCoords.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to primary colors of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<Color32> PrimaryColors
		{
			get { return this.colors0; }
			set { this.colors0.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to secondary colors of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<Color32> SecondaryColors
		{
			get { return this.colors1; }
			set { this.colors1.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to normals of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<Vector3> Normals
		{
			get { return this.normals; }
			set { this.normals.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to tangent space normals of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<ITangent> Tangents
		{
			get { return this.tangents; }
			set { this.tangents.CopyFrom(value); }
		}
		/// <summary>
		/// Gets an object that provides access to tangent space normals of vertices in native memory.
		/// </summary>
		/// <remarks>When setting data is copied from 'value' to this collection.</remarks>
		public override IMeshDetailsCollection<IQTangent> QTangents
		{
			get { return this.qTangents; }
			set { this.qTangents.CopyFrom(value); }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when vertex collection is reallocated.
		/// </summary>
		internal event EventHandler<VertexCollectionEventArgs> VerticesReallocated;
		#endregion
		#region Construction
		/// <summary>
		/// Initializes new wrapper object for native mesh.
		/// </summary>
		/// <param name="obj">Static object that hosts the mesh.</param>
		public NativeMesh(StaticObject obj)
		{
			if (obj.Disposed)
			{
				throw new ObjectDisposedException("Cannot acquire mesh data from static" +
												  " object that has been disposed of.");
			}
			MeshHandles handles = NativeStaticObjectMethods.GetMeshHandles(obj.Handle);
			if (handles.IndexedMeshHandle == IntPtr.Zero || handles.MeshHandle == IntPtr.Zero)
			{
				throw new Exception("Unable to acquire mesh handles for the static object.");
			}
			this.CMeshHandle = handles.MeshHandle;
			this.IndexedMeshHandle = handles.IndexedMeshHandle;
			this.StaticObject = obj;

			this.colors0 = new NativeVertexColor0Collection(this);
			this.colors1 = new NativeVertexColor1Collection(this);
			this.faces = new NativeFaceCollection(this.CMeshHandle);
			this.indices = new NativeIndicesCollection(this);
			this.positions = new NativeVertexPositionCollection(this);
			this.normals = new NativeVertexNormalCollection(this);
			this.tangents = new NativeVertexTangentCollection(this);
			this.qTangents = new NativeVertexQTangentCollection(this);
			this.texCoords = new NativeVertexTextureCoordinatesCollection(this);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Validates this mesh.
		/// </summary>
		/// <remarks>
		/// See <see cref="Validate(bool, List{string}, List{string})"/> for the list of possible problems.
		/// </remarks>
		/// <param name="errors">  
		/// A list that will contain a list of problems that will prevent calling
		/// <see cref="Export"/> method from being successful.
		/// </param>
		/// <param name="warnings">
		/// A list that will contain a list of problems that may cause problems but won't prevent
		/// CryEngine from recognizing this mesh.
		/// </param>
		/// <returns>True if a list of errors has no entries, otherwise false.</returns>
		public override bool Validate(List<string> errors = null, List<string> warnings = null)
		{
			return this.Validate(true, errors, warnings);
		}
		/// <summary>
		/// Validates this mesh.
		/// </summary>
		/// <remarks>
		/// Possible errors:
		///
		/// Error 1)
		///
		/// Possible warnings:
		///
		/// Warning 1)
		/// </remarks>
		/// <param name="cache">
		/// Indicates whether all collections must be transferred to Mono memory before validation.
		/// If true, can increase performance but along with memory consumption.
		/// </param>
		/// <param name="errors">
		/// A list that will contain a list of problems that will prevent calling <see cref="Export"
		/// /> method from being successful.
		/// </param>
		/// <param name="warnings">
		/// A list that will contain a list of problems that may cause problems but won't prevent
		/// CryEngine from recognizing this mesh.
		/// </param>
		/// <returns>True if a list of errors has no entries, otherwise false.</returns>
		public bool Validate(bool cache = false, List<string> errors = null, List<string> warnings = null)
		{
			if (errors == null)
			{
				errors = new List<string>();
			}
			if (warnings == null)
			{
				warnings = new List<string>();
			}

			if (this.Faces.Count <= 0 && this.Indices.Count < 3 && this.Positions.Count <= 0)
			{
				errors.Add("This mesh is completely empty.");
				return false;									// No point in continuing validation.
			}

			if (this.Faces.Count <= 0 && this.Indices.Count < 3 && this.Positions.Count > 0)
			{
				errors.Add("No valid faces, despite existence of vertices.");
			}
			// Currently all meshes use UInt16 for indices, and it is not possible to change it
			// without access to engine source. CryMono uses UInt32 for indices, so, if there are
			// too many vertices, all extras will be discarded along with faces during export. When
			// that happens, a lot of data movements will have to happen, so it's best to avoid
			// breaching a limit before exporting.
			if (Platform.MeshIndexIs16Bit && this.Positions.Count > UInt16.MaxValue)
			{
				warnings.Add("Too many vertices. Maximal number of vertices is " +
					UInt16.MaxValue.ToString(CultureInfo.InvariantCulture));
			}

			if (this.Faces.Count > 0 || this.Indices.Count > 2 && this.Positions.Count <= 0)
			{
				errors.Add("No valid vertices, despite existence of faces or indices.");
			}

			IList<IndexedTriangleFace> facesList;
			IList<uint> indicesList;
			IList<Vector3> vertices;

			if (cache)
			{
				facesList = this.Faces.ToArray();
				indicesList = this.Indices.ToArray();
				vertices = this.Positions.ToArray();
			}
			else
			{
				facesList = this.Faces;
				indicesList = this.Indices;
				vertices = this.Positions;
			}

			if (facesList.Any(x => indicesList.Any(y => y >= vertices.Count)))
			{
				errors.Add("One of the faces has vertex index that points out of bounds of vertex list.");
			}

			if (indicesList.Any(x => x >= vertices.Count))
			{
				errors.Add("One of the face indices points out of bounds of vertex list.");
			}

			return errors.Count != 0;
		}
		/// <summary>
		/// Signals underlying static object to create a new render mesh.
		/// </summary>
		/// <param name="staticObject">Ignored.</param>
		public override void Export(StaticObject staticObject)
		{
			NativeMeshMethods.Export(this.StaticObject.Handle);
		}
		/// <summary>
		/// Sets this mesh to value based on given BSP tree.
		/// </summary>
		/// <param name="tree">Root node of the BSP tree.</param>
		public override void SetBsp(BspNode<SplittableTriangle> tree)
		{
			// Calculate capacities.

			// Create vertex pool.
			List<SplittableTriangle> triangles = tree.AllElements;
			List<MeshVertex> vertexes = new List<MeshVertex>(triangles.Count * 3);
			Action<MeshVertex> registerVertex = delegate(MeshVertex vertex)
			{
				int index = vertexes.BinarySearch(vertex);
				if (index < 0)
				{
					vertexes.Insert(~index, vertex);
				}
			};
			for (int i = 0; i < triangles.Count; i++)
			{
				registerVertex(triangles[i].First);
				registerVertex(triangles[i].Second);
				registerVertex(triangles[i].Third);
			}
			// Assign faces.
			this.faces.Capacity = triangles.Count;
			for (int i = 0; i < triangles.Count; i++)
			{
				this.faces[i] = new IndexedTriangleFace
				{
					Indices = new Int32Vector3
					(
						vertexes.BinarySearch(triangles[i].First),
						vertexes.BinarySearch(triangles[i].Second),
						vertexes.BinarySearch(triangles[i].Third)
					)
				};
			}
			// Assign vertex-related data.
			this.positions.Capacity = vertexes.Count;
			for (int i = 0; i < vertexes.Count; i++)
			{
				MeshVertex vertex = vertexes[i];
				this.positions[i] = vertex.Position;
				this.normals[i] = vertex.Normal;
				this.texCoords[i] = vertex.UvMapPosition;
				this.colors0[i] = vertex.PrimaryColor;
				this.colors1[i] = vertex.SecondaryColor;
			}
		}
		#endregion
		#region Utilities
		#region Event Invokators
		internal virtual void OnVerticesReallocated(int old, int @new)
		{
			if (this.VerticesReallocated != null)
			{
				this.VerticesReallocated
				(
					this,
					new VertexCollectionEventArgs
					{
						OldCount = old,
						NewCount = @new
					}
				);
			}
		}

		#endregion
		#endregion
	}
	/// <summary>
	/// Enumeration of sections of memory occupied by CMesh object.
	/// </summary>
	public enum NativeMeshMemoryRegion// : int
	{
		/// <summary>
		/// Array of locations of vertices.
		/// </summary>
		Positions,
		/// <summary>
		/// Array of locations of vertices in <see cref="Half"/> format.
		/// </summary>
		Positionsf16,
		/// <summary>
		/// Array of normals.
		/// </summary>
		Normals,
		/// <summary>
		/// Array of faces.
		/// </summary>
		Faces,
		/// <summary>
		/// Array of topology identifiers.
		/// </summary>
		TopologyIds,
		/// <summary>
		/// Array of texture coordinates.
		/// </summary>
		TextureCoordinates,
		/// <summary>
		/// Array of primary colors.
		/// </summary>
		Colors0,
		/// <summary>
		/// Array of secondary colors.
		/// </summary>
		Colors1,
		/// <summary>
		/// Array of indices.
		/// </summary>
		Indices,
		/// <summary>
		/// Array of tangent-space normals.
		/// </summary>
		Tangents,
		/// <summary>
		/// Array of bones.
		/// </summary>
		Bonemapping,
		/// <summary>
		/// Array of vertex mats.
		/// </summary>
		VertMats,
		/// <summary>
		/// Array of q-tangents.
		/// </summary>
		Qtangents,
		/// <summary>
		/// Array of PS3 edges.
		/// </summary>
		Ps3Edgedata,
		/// <summary>
		/// Array of something related to PS3.
		/// </summary>
		P3Sc4Bt2S,
		/// <summary>
		/// Extra bone mapping data.
		/// </summary>
		/// <remarks>
		/// Does not have a stream ID in the CGF. Its data is saved at the end of the
		/// <see cref="Bonemapping"/> stream.
		/// </remarks>
		Extrabonemapping,
		/// <summary>
		/// Last stream.
		/// </summary>
		LastStream,
	}
	/// <summary>
	/// Encapsulates details about changes to size of vertex collection.
	/// </summary>
	public class VertexCollectionEventArgs : EventArgs
	{
		/// <summary>
		/// Previous number of vertices in the collection.
		/// </summary>
		public int OldCount { get; set; }
		/// <summary>
		/// New number of vertices in the collection.
		/// </summary>
		public int NewCount { get; set; }
	}
}