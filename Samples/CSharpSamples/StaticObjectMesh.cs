using System;
using System.Linq;
using CryCil;
using CryCil.Engine.Logic;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering;

namespace CSharpSamples
{
	public static class StaticObjectMesh
	{
		public static void CreateTriangle(CryEntity entity)
		{
			// Create a new static object.
			StaticObject staticObject = new StaticObject(true);
			staticObject.IncrementReferenceCount();

			// Set materials.
			Material material = Material.Load("EngineAssets/TextureMsg/DefaultSolids");

			entity.Material = material;
			staticObject.Material = material;

			// Acquire the indexed mesh object.
			CryIndexedMesh indexedMesh = staticObject.GetIndexedMesh();

			if (!indexedMesh.IsValid)
			{
				return;
			}

			// Allocate data.
			const int vertexCount = 3;
			const int faceCount = 1;

			indexedMesh.VertexCount = vertexCount;
			indexedMesh.FaceCount = faceCount;
			indexedMesh.TextureCoordinatesCount = vertexCount;

			// Acquire editable mesh object.
			CryMesh mesh = indexedMesh.Mesh;

			// Acquire collections of mesh elements.
			var vertexes = mesh.Vertexes;

			var positions = vertexes.Positions;
			var normals = vertexes.Normals;

			var uvPositions = mesh.TexturePositions;
			var faces = mesh.Faces;

			// Assign coordinates and form the triangle.

			// Coordinates in 3D world.
			positions[0] = new Vector3();
			positions[1] = new Vector3(0, 10, 0);
			positions[2] = new Vector3(10, 0, 0);

			// Coordinates on UV map.
			uvPositions[0] = new CryMeshTexturePosition();
			uvPositions[1] = new CryMeshTexturePosition(0, 1);
			uvPositions[2] = new CryMeshTexturePosition(1, 0);

			// Indexes that form the face.
			faces[0] = new CryMeshFace
			{
				First = 2,
				Second = 1,
				Third = 0
			};

			// All normals point upwards.
			CryMeshNormal up = new CryMeshNormal {Normal = Vector3.Up};
			normals[0] = up;
			normals[1] = up;
			normals[2] = up;

			// Assign the material to the only subset.
			var subsets = indexedMesh.Subsets;
			subsets.Count = 1;
			subsets.SetSubsetMaterialId(0, 0);

			// Calculate the bounding box.
			indexedMesh.CalculateBoundingBox();

			// Make the static object create the render mesh from our indexed mesh.
			staticObject.Invalidate(true);

			// Activate the entity just in case.
			entity.Active = true;

			// Add static object to the entity.
			entity.Slots.Add(staticObject, true);

			// Decrement reference count of the static object since now its in the entity.
			staticObject.DecrementReferenceCount();
		}
	}
}