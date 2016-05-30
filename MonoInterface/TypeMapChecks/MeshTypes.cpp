#include "stdafx.h"

#include "CheckingBasics.h"

#include <Cry3DEngine/IIndexedMesh.h>

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

	explicit MeshTexCoord(SMeshTexCoord &) : s(0), t(0)
	{
		CHECK_TYPE_SIZE(MeshTexCoord);
	}
};

struct MeshColor
{
	uint8 r, g, b, a;

	explicit MeshColor(SMeshColor &) : r(0), g(0), b(0), a(0)
	{
		CHECK_TYPE_SIZE(MeshColor);
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

	explicit MeshNormal(SMeshNormal &) : Normal(ZERO)
	{
		CHECK_TYPE_SIZE(MeshNormal);
	}
};

struct MeshTangents
{
	Vec4sf Tangent;
	Vec4sf Bitangent;

	explicit MeshTangents(SMeshTangents &) : Tangent(ZERO), Bitangent(ZERO)
	{
		CHECK_TYPE_SIZE(MeshTangents);
	}
};

struct MeshQTangents
{
	Vec4sf TangentBitangent;

	explicit MeshQTangents(SMeshQTangents &) : TangentBitangent(ZERO)
	{
		CHECK_TYPE_SIZE(MeshQTangents);
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

TYPE_MIRROR enum VertexFormat
{
	eVF_Unknown_check = 0,

	// Base stream
	eVF_P3F_C4B_T2F_check = 1,
	eVF_P3S_C4B_T2S_check = 2,
	eVF_P3S_N4B_C4B_T2S_check = 3,

	eVF_P3F_C4B_T4B_N3F2_check = 4, // Particles.
	eVF_TP3F_C4B_T2F_check = 5, // Fonts (28 bytes).
	eVF_TP3F_T2F_T3F_check = 6,  // Miscellaneous.
	eVF_P3F_T3F_check = 7,       // Miscellaneous.
	eVF_P3F_T2F_T3F_check = 8,   // Miscellaneous.

	// Additional streams
	eVF_T2F_check = 9,           // Light maps TC (8 bytes).
	eVF_W4B_I4B_check = 10,  // Skinned weights/indices stream.
	eVF_C4B_C4B_check = 11,      // SH coefficients.
	eVF_P3F_P3F_I4B_check = 12,  // Shape deformation stream.
	eVF_P3F_check = 13,       // Velocity stream.

	eVF_C4B_T2S_check = 14,     // General (Position is merged with Tangent stream)

	// Lens effects simulation
	eVF_P2F_T4F_C4F_check = 15,  // primary
	eVF_P2F_T4F_T4F_C4F_check = 16,

	eVF_P2S_N4B_C4B_T1F_check = 17,
	eVF_P3F_C4B_T2S_check = 18,
	eVF_PI_check = 19,

	eVF_Max_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (VertexFormat::x ## _check == EVertexFormat::x, "EVertexFormat enumeration has been changed.")

inline void CheckVertexFormats()
{
	CHECK_ENUM(eVF_Unknown);
	CHECK_ENUM(eVF_P3F_C4B_T2F);
	CHECK_ENUM(eVF_P3S_C4B_T2S);
	CHECK_ENUM(eVF_P3S_N4B_C4B_T2S);
	CHECK_ENUM(eVF_P3F_C4B_T4B_N3F2);
	CHECK_ENUM(eVF_TP3F_C4B_T2F);
	CHECK_ENUM(eVF_TP3F_T2F_T3F);
	CHECK_ENUM(eVF_P3F_T3F);
	CHECK_ENUM(eVF_P3F_T2F_T3F);
	CHECK_ENUM(eVF_T2F);
	CHECK_ENUM(eVF_W4B_I4B);
	CHECK_ENUM(eVF_C4B_C4B);
	CHECK_ENUM(eVF_P3F_P3F_I4B);
	CHECK_ENUM(eVF_P3F);
	CHECK_ENUM(eVF_C4B_T2S);
	CHECK_ENUM(eVF_P2F_T4F_C4F);
	CHECK_ENUM(eVF_P2F_T4F_T4F_C4F);
	CHECK_ENUM(eVF_P2S_N4B_C4B_T1F);
	CHECK_ENUM(eVF_P3F_C4B_T2S);
	CHECK_ENUM(eVF_PI);
	CHECK_ENUM(eVF_Max);
}

TYPE_MIRROR enum RenderMeshType
{
	eRMT_Immmutable_check = 0,
	eRMT_Static_check = 1,
	eRMT_Dynamic_check = 2,
	eRMT_Transient_check = 3
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (RenderMeshType::x ## _check == ERenderMeshType::x, "ERenderMeshType enumeration has been changed.")

inline void CheckRenderMeshType()
{
	CHECK_ENUM(eRMT_Immmutable);
	CHECK_ENUM(eRMT_Static);
	CHECK_ENUM(eRMT_Dynamic);
	CHECK_ENUM(eRMT_Transient);
}

TYPE_MIRROR enum RenderMeshConversionFlags
{
	FSM_VERTEX_VELOCITY_check = 1,
	FSM_NO_TANGENTS_check = 2,
	FSM_CREATE_DEVICE_MESH_check = 4,
	FSM_SETMESH_ASYNC_check = 8,
	FSM_ENABLE_NORMALSTREAM_check = 16,
	FSM_IGNORE_TEXELDENSITY_check = 32
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (RenderMeshConversionFlags::x ## _check == x, "A set of defines for conversion of normal meshes to renderable one has been changed.")

inline void CheckRenderMeshConversionFlags()
{
	CHECK_ENUM(FSM_VERTEX_VELOCITY);
	CHECK_ENUM(FSM_NO_TANGENTS);
	CHECK_ENUM(FSM_CREATE_DEVICE_MESH);
	CHECK_ENUM(FSM_SETMESH_ASYNC);
	CHECK_ENUM(FSM_ENABLE_NORMALSTREAM);
	CHECK_ENUM(FSM_IGNORE_TEXELDENSITY);
}

TYPE_MIRROR enum RenderMeshAccessFlags
{
	FSL_READ_check = 0x01,
	FSL_WRITE_check = 0x02,
	FSL_DYNAMIC_check = 0x04,
	FSL_DISCARD_check = 0x08,
	FSL_VIDEO_check = 0x10,
	FSL_SYSTEM_check = 0x20,
	FSL_INSTANCED_check = 0x40,
	FSL_NONSTALL_MAP_check = 0x80, // Map must not stall for VB/IB locking
	FSL_VBIBPUSHDOWN_check = 0x100, // Push down from vram on demand if target architecture supports it, used internally
	FSL_DIRECT_check = 0x200, // Access VRAM directly if target architecture supports it, used internally
	FSL_LOCKED_check = 0x400, // Internal use
	FSL_SYSTEM_CREATE_check = (FSL_WRITE | FSL_DISCARD | FSL_SYSTEM),
	FSL_SYSTEM_UPDATE_check = (FSL_WRITE | FSL_SYSTEM),
	FSL_VIDEO_CREATE_check = (FSL_WRITE | FSL_DISCARD | FSL_VIDEO),
	FSL_VIDEO_UPDATE_check = (FSL_WRITE | FSL_VIDEO)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (RenderMeshAccessFlags::x ## _check == x, "A set of defines for render mesh access flags has been changed.")

inline void CheckRenderMeshAccessFlags()
{
	CHECK_ENUM(FSL_READ);
	CHECK_ENUM(FSL_WRITE);
	CHECK_ENUM(FSL_DYNAMIC);
	CHECK_ENUM(FSL_DISCARD);
	CHECK_ENUM(FSL_VIDEO);
	CHECK_ENUM(FSL_SYSTEM);
	CHECK_ENUM(FSL_INSTANCED);
	CHECK_ENUM(FSL_NONSTALL_MAP);
	CHECK_ENUM(FSL_VBIBPUSHDOWN);
	CHECK_ENUM(FSL_DIRECT);
	CHECK_ENUM(FSL_LOCKED);
	CHECK_ENUM(FSL_SYSTEM_CREATE);
	CHECK_ENUM(FSL_SYSTEM_UPDATE);
	CHECK_ENUM(FSL_VIDEO_CREATE);
	CHECK_ENUM(FSL_VIDEO_UPDATE);
}

TYPE_MIRROR enum StreamIDs
{
	VSF_GENERAL_check,                 // General vertex buffer
	VSF_TANGENTS_check,                // Tangents buffer
	VSF_QTANGENTS_check,               // Tangents buffer
	VSF_HWSKIN_INFO_check,             // HW skinning buffer
	VSF_VERTEX_VELOCITY_check,                // Velocity buffer
	VSF_NORMALS_check,                 // Normals, used for skinning
	// <- Insert new stream IDs here
	VSF_NUM_check,                     // Number of vertex streams

	VSF_MORPHBUDDY_check = 8,          // Morphing (from m_pMorphBuddy)
	VSF_INSTANCED_check = 9,           // Data is for instance stream
	VSF_MORPHBUDDY_WEIGHTS_check = 15
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (StreamIDs::x ## _check == EStreamIDs::x, "EStreamIDs enumeration has been changed.")

inline void CheckStreamIDs()
{
	CHECK_ENUM(VSF_GENERAL);
	CHECK_ENUM(VSF_TANGENTS);
	CHECK_ENUM(VSF_QTANGENTS);
	CHECK_ENUM(VSF_HWSKIN_INFO);
	CHECK_ENUM(VSF_VERTEX_VELOCITY);
	CHECK_ENUM(VSF_NORMALS);
	CHECK_ENUM(VSF_NUM);
	CHECK_ENUM(VSF_MORPHBUDDY);
	CHECK_ENUM(VSF_INSTANCED);
	CHECK_ENUM(VSF_MORPHBUDDY_WEIGHTS);
}