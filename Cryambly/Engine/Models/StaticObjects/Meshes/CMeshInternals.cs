using System;
using CryCil.Geometry;

namespace CryCil.Engine.Models.StaticObjects
{
	internal unsafe struct CMeshInternals
	{
		internal IntPtr vtable;
		internal CryMeshFace* pFaces; // faces are used in mesh processing/compilation
		internal int* pTopologyIds;

		// geometry data
		internal uint* pIndices; // indices are used for the final render-mesh
		internal Vector3* pPositions;
		internal Vector3Half* pPositionsF16;

		internal CryMeshNormal* pNorms;
		internal CryMeshTangents* pTangents;
		internal CryMeshQTangents* pQTangents;
		internal CryMeshTexturePosition* pTexCoord;
		internal CryMeshColor* pColor0;
		internal CryMeshColor* pColor1;

		internal int* pVertMats;
		internal P3S_C4B_T2S* pP3S_C4B_T2S;

		internal CryMeshBoneMappingUint16* pBoneMapping; //bone-mapping for the final render-mesh
		internal CryMeshBoneMappingByte* pExtraBoneMapping; //bone indices and weights for bones 5 to 8.

		internal int nCoorCount; //number of texture coordinates in pTexCoord array
		internal fixed int streamSize [(int)GeneralMeshStreamId.LastStream];

		// Bounding box.
		internal BoundingBox bbox;

		// Array of mesh subsets.
		internal CryMeshSubset* subsets;

		// Mask that indicate if this stream is using not allocated in Mesh pointer; ex. if
		// (nSharedStreamMask & (1<<NORMALS) -> normals stream is shared
		internal uint nSharedStreamMask;

		// Texture space area divided by geometry area. Zero if cannot compute.
		internal float texMappingDensity;

		// Geometric mean value calculated from the areas of this mesh faces.
		internal float geometricMeanFaceArea;
	}
}