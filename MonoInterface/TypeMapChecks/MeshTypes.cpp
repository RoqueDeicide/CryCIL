#include "stdafx.h"

#include "CheckingBasics.h"

#include <IIndexedMesh.h>

TYPE_MIRROR enum MeshStreams
{
	POSITIONS_check,
	POSITIONSF16_check,
	NORMALS_check,
	FACES_check,
	TOPOLOGY_IDS_check,
	TEXCOORDS_check,
	COLORS_0_check,
	COLORS_1_check,
	INDICES_check,
	TANGENTS_check,
	BONEMAPPING_check,
	VERT_MATS_check,
	QTANGENTS_check,
	P3S_C4B_T2S_check,

	EXTRABONEMAPPING_check, // Extra stream. Does not have a stream ID in the CGF. Its data is saved at the end of the BONEMAPPING stream.

	LAST_STREAM_check
};

#define CHECK_ENUM(x) static_assert (MeshStreams::x ## _check == CMesh::EStream::x, "EMeshTypes enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(POSITIONS);
	CHECK_ENUM(POSITIONSF16);
	CHECK_ENUM(NORMALS);
	CHECK_ENUM(FACES);
	CHECK_ENUM(TOPOLOGY_IDS);
	CHECK_ENUM(TEXCOORDS);
	CHECK_ENUM(COLORS_0);
	CHECK_ENUM(COLORS_1);
	CHECK_ENUM(INDICES);
	CHECK_ENUM(TANGENTS);
	CHECK_ENUM(BONEMAPPING);
	CHECK_ENUM(VERT_MATS);
	CHECK_ENUM(QTANGENTS);
	CHECK_ENUM(P3S_C4B_T2S);
	CHECK_ENUM(EXTRABONEMAPPING);
	CHECK_ENUM(LAST_STREAM);
}

struct Mesh
{
	SMeshFace* m_pFaces;  // faces are used in mesh processing/compilation
	int32* m_pTopologyIds;

	// geometry data
	vtx_idx* m_pIndices;   // indices are used for the final render-mesh
	Vec3* m_pPositions;
	Vec3f16* m_pPositionsF16;

	SMeshNormal* m_pNorms;
	SMeshTangents* m_pTangents;
	SMeshQTangents* m_pQTangents;
	SMeshTexCoord* m_pTexCoord;
	SMeshColor* m_pColor0;
	SMeshColor* m_pColor1;

	int *m_pVertMats;
	SVF_P3S_C4B_T2S* m_pP3S_C4B_T2S;

	SMeshBoneMapping_uint16* m_pBoneMapping;  //bone-mapping for the final render-mesh
	SMeshBoneMapping_uint16* m_pExtraBoneMapping;	//bone indices and weights for bones 5 to 8.

	int m_nCoorCount;					//number of texture coordinates in m_pTexCoord array
	int m_streamSize[CMesh::LAST_STREAM];

	// Bounding box.
	AABB m_bbox;

	// Array of mesh subsets.
	DynArray<SMeshSubset> m_subsets;

	// Mask that indicate if this stream is using not allocated in Mesh pointer;
	// ex. if (m_nSharedStreamMask & (1<<NORMALS) -> normals stream is shared
	uint32 m_nSharedStreamMask;

	// Texture space area divided by geometry area. Zero if cannot compute.
	float m_texMappingDensity;

	// Geometric mean value calculated from the areas of this mesh faces.
	float m_geometricMeanFaceArea;

	explicit Mesh(CMesh &other)
	{
		CHECK_TYPES_SIZE(Mesh, CMesh);

		ASSIGN_FIELD(m_pFaces);
		ASSIGN_FIELD(m_pTopologyIds);
		ASSIGN_FIELD(m_pIndices);
		ASSIGN_FIELD(m_pPositions);
		ASSIGN_FIELD(m_pPositionsF16);
		ASSIGN_FIELD(m_pNorms);
		ASSIGN_FIELD(m_pTangents);
		ASSIGN_FIELD(m_pQTangents);
		ASSIGN_FIELD(m_pTexCoord);
		ASSIGN_FIELD(m_pColor0);
		ASSIGN_FIELD(m_pColor1);
		ASSIGN_FIELD(m_pVertMats);
		ASSIGN_FIELD(m_pP3S_C4B_T2S);
		ASSIGN_FIELD(m_pBoneMapping);
		ASSIGN_FIELD(m_pExtraBoneMapping);
		ASSIGN_FIELD(m_nCoorCount);
		ASSIGN_FIELD(m_streamSize[0]);
		ASSIGN_FIELD(m_bbox);
		ASSIGN_FIELD(m_subsets);
		ASSIGN_FIELD(m_nSharedStreamMask);
		ASSIGN_FIELD(m_texMappingDensity);
		ASSIGN_FIELD(m_geometricMeanFaceArea);

		CHECK_TYPE(m_pFaces);
		CHECK_TYPE(m_pTopologyIds);
		CHECK_TYPE(m_pIndices);
		CHECK_TYPE(m_pPositions);
		CHECK_TYPE(m_pPositionsF16);
		CHECK_TYPE(m_pNorms);
		CHECK_TYPE(m_pTangents);
		CHECK_TYPE(m_pQTangents);
		CHECK_TYPE(m_pTexCoord);
		CHECK_TYPE(m_pColor0);
		CHECK_TYPE(m_pColor1);
		CHECK_TYPE(m_pVertMats);
		CHECK_TYPE(m_pP3S_C4B_T2S);
		CHECK_TYPE(m_pBoneMapping);
		CHECK_TYPE(m_pExtraBoneMapping);
		CHECK_TYPE(m_nCoorCount);
		CHECK_TYPE(m_streamSize[0]);
		CHECK_TYPE(m_bbox);
		CHECK_TYPE(m_subsets);
		CHECK_TYPE(m_nSharedStreamMask);
		CHECK_TYPE(m_texMappingDensity);
		CHECK_TYPE(m_geometricMeanFaceArea);
	}

	virtual ~Mesh()
	{
	}
};

struct MeshTexCoord
{
	float s, t;

	explicit MeshTexCoord(SMeshTexCoord &other)
	{
		CHECK_TYPE_SIZE(MeshTexCoord);

		ASSIGN_FIELD(s);
		ASSIGN_FIELD(t);

		CHECK_TYPE(s);
		CHECK_TYPE(t);
	}
};

struct MeshColor
{
	uint8 r, g, b, a;

	explicit MeshColor(SMeshColor &other)
	{
		CHECK_TYPE_SIZE(MeshColor);

		ASSIGN_FIELD(r);
		ASSIGN_FIELD(g);
		ASSIGN_FIELD(b);
		ASSIGN_FIELD(a);

		CHECK_TYPE(r);
		CHECK_TYPE(g);
		CHECK_TYPE(b);
		CHECK_TYPE(a);
	}
};

struct MeshFace
{
	int v[3]; // indices to vertex, normals and optionally tangent basis arrays
	unsigned char nSubset; // index to mesh subsets array.

	explicit MeshFace(SMeshFace &other)
	{
		CHECK_TYPE_SIZE(MeshFace);

		ASSIGN_FIELD(v[0]);
		ASSIGN_FIELD(nSubset);

		CHECK_TYPE(v[0]);
		CHECK_TYPE(nSubset);
	}
};

struct MeshNormal
{
	Vec3 Normal;

	explicit MeshNormal(SMeshNormal &other)
	{
		CHECK_TYPE_SIZE(MeshNormal);

		ASSIGN_FIELD(Normal);

		CHECK_TYPE(Normal);
	}
};

struct MeshTangents
{
	Vec4sf Tangent;
	Vec4sf Bitangent;

	explicit MeshTangents(SMeshTangents &other)
	{
		CHECK_TYPE_SIZE(MeshTangents);

		ASSIGN_FIELD(Tangent);
		ASSIGN_FIELD(Bitangent);

		CHECK_TYPE(Tangent);
		CHECK_TYPE(Bitangent);
	}
};

struct MeshQTangents
{
	Vec4sf TangentBitangent;

	explicit MeshQTangents(SMeshQTangents &other)
	{
		CHECK_TYPE_SIZE(MeshQTangents);

		ASSIGN_FIELD(TangentBitangent);

		CHECK_TYPE(TangentBitangent);
	}
};

struct MeshBoneMapping_uint16
{
	typedef uint16 BoneId;
	typedef uint8 Weight;

	BoneId boneIds[4];
	Weight weights[4];

	explicit MeshBoneMapping_uint16(SMeshBoneMapping_uint16 &other)
	{
		CHECK_TYPE_SIZE(MeshBoneMapping_uint16);

		ASSIGN_FIELD(boneIds[0]);
		ASSIGN_FIELD(weights[0]);

		CHECK_TYPE(boneIds);
		CHECK_TYPE(weights);
	}
};

struct MeshBoneMapping_uint8
{
	typedef uint8 BoneId;
	typedef uint8 Weight;

	BoneId boneIds[4];
	Weight weights[4];

	explicit MeshBoneMapping_uint8(SMeshBoneMapping_uint8 &other)
	{
		CHECK_TYPE_SIZE(MeshBoneMapping_uint8);

		ASSIGN_FIELD(boneIds[0]);
		ASSIGN_FIELD(weights[0]);

		CHECK_TYPE(boneIds);
		CHECK_TYPE(weights);
	}
};

struct MeshSubset
{
	Vec3 vCenter;
	float fRadius;
	float fTexelDensity;

	int nFirstIndexId;
	int nNumIndices;

	int nFirstVertId;
	int nNumVerts;

	int nMatID; // Material Sub-object id.
	int nMatFlags; // Special Material flags.
	int nPhysicalizeType; // Type of physicalization for this subset.

	explicit MeshSubset(SMeshSubset &other)
	{
		CHECK_TYPE_SIZE(MeshNormal);

		ASSIGN_FIELD(vCenter);
		ASSIGN_FIELD(fRadius);
		ASSIGN_FIELD(fTexelDensity);
		ASSIGN_FIELD(nFirstIndexId);
		ASSIGN_FIELD(nNumIndices);
		ASSIGN_FIELD(nFirstVertId);
		ASSIGN_FIELD(nNumVerts);
		ASSIGN_FIELD(nMatID);
		ASSIGN_FIELD(nMatFlags);
		ASSIGN_FIELD(nPhysicalizeType);

		CHECK_TYPE(vCenter);
		CHECK_TYPE(fRadius);
		CHECK_TYPE(fTexelDensity);
		CHECK_TYPE(nFirstIndexId);
		CHECK_TYPE(nNumIndices);
		CHECK_TYPE(nFirstVertId);
		CHECK_TYPE(nNumVerts);
		CHECK_TYPE(nMatID);
		CHECK_TYPE(nMatFlags);
		CHECK_TYPE(nPhysicalizeType);
	}
};

struct MeshDescription
{
	const SMeshFace*     m_pFaces;    // pointer to array of faces
	const Vec3*          m_pVerts;    // pointer to array of vertices in f32 format
	const Vec3f16*       m_pVertsF16; // pointer to array of vertices in f16 format
	const SMeshNormal*   m_pNorms;    // pointer to array of normals
	const SMeshColor*    m_pColor;    // pointer to array of vertex colors
	const SMeshTexCoord* m_pTexCoord; // pointer to array of texture coordinates
	const vtx_idx*   m_pIndices;  // pointer to array of indices
	int m_nFaceCount;  // number of elements m_pFaces array
	int m_nVertCount;  // number of elements in m_pVerts, m_pNorms and m_pColor arrays
	int m_nCoorCount;  // number of elements in m_pTexCoord array
	int m_nIndexCount; // number of elements in m_pIndices array

	explicit MeshDescription(IIndexedMesh::SMeshDescription &other)
	{
		CHECK_TYPES_SIZE(MeshDescription, IIndexedMesh::SMeshDescription);

		ASSIGN_FIELD(m_pFaces);
		ASSIGN_FIELD(m_pVerts);
		ASSIGN_FIELD(m_pVertsF16);
		ASSIGN_FIELD(m_pNorms);
		ASSIGN_FIELD(m_pColor);
		ASSIGN_FIELD(m_pTexCoord);
		ASSIGN_FIELD(m_pIndices);
		ASSIGN_FIELD(m_nFaceCount);
		ASSIGN_FIELD(m_nVertCount);
		ASSIGN_FIELD(m_nCoorCount);
		ASSIGN_FIELD(m_nIndexCount);

		CHECK_TYPE(m_pFaces);
		CHECK_TYPE(m_pVerts);
		CHECK_TYPE(m_pVertsF16);
		CHECK_TYPE(m_pNorms);
		CHECK_TYPE(m_pColor);
		CHECK_TYPE(m_pTexCoord);
		CHECK_TYPE(m_pIndices);
		CHECK_TYPE(m_nFaceCount);
		CHECK_TYPE(m_nVertCount);
		CHECK_TYPE(m_nCoorCount);
		CHECK_TYPE(m_nIndexCount);
	}
};