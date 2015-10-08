#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR enum StaticObjectLoadingFlags
{
	ELoadingFlagsPreviewMode_check = BIT(0),
	ELoadingFlagsForceBreakable_check = BIT(1),
	ELoadingFlagsIgnoreLoDs_check = BIT(2),
	ELoadingFlagsTessellate_check = BIT(3), // if e_StatObjTessellation enabled
	ELoadingFlagsJustGeometry_check = BIT(4), // for streaming, to avoid parsing all chunks
};

#define CHECK_ENUM(x) static_assert (StaticObjectLoadingFlags::x ## _check == IStatObj::ELoadingFlags::x, "IStatObj::ELoadingFlags enumeration has been changed.")

inline void Check2()
{
	CHECK_ENUM(ELoadingFlagsPreviewMode);
	CHECK_ENUM(ELoadingFlagsForceBreakable);
	CHECK_ENUM(ELoadingFlagsIgnoreLoDs);
	CHECK_ENUM(ELoadingFlagsTessellate);
	CHECK_ENUM(ELoadingFlagsJustGeometry);
}

TYPE_MIRROR enum StaticObjectFlags
{
	STATIC_OBJECT_HIDDEN_check = BIT(0),
	STATIC_OBJECT_CLONE_check = BIT(1),
	STATIC_OBJECT_GENERATED_check = BIT(2),
	STATIC_OBJECT_CANT_BREAK_check = BIT(3),
	STATIC_OBJECT_DEFORMABLE_check = BIT(4),
	STATIC_OBJECT_COMPOUND_check = BIT(5),
	STATIC_OBJECT_MULTIPLE_PARENTS_check = BIT(6),

	// Collisions
	STATIC_OBJECT_NO_PLAYER_COLLIDE_check = BIT(10),

	// Special flags.
	STATIC_OBJECT_SPAWN_ENTITY_check = BIT(20),
	STATIC_OBJECT_PICKABLE_check = BIT(21),
	STATIC_OBJECT_NO_AUTO_HIDEPOINTS_check = BIT(22),
	STATIC_OBJECT_DYNAMIC_check = BIT(23)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (StaticObjectFlags::x ## _check == EStaticObjectFlags::x, "EStaticObjectFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(STATIC_OBJECT_HIDDEN);
	CHECK_ENUM(STATIC_OBJECT_CLONE);
	CHECK_ENUM(STATIC_OBJECT_GENERATED);
	CHECK_ENUM(STATIC_OBJECT_CANT_BREAK);
	CHECK_ENUM(STATIC_OBJECT_DEFORMABLE);
	CHECK_ENUM(STATIC_OBJECT_COMPOUND);
	CHECK_ENUM(STATIC_OBJECT_MULTIPLE_PARENTS);

	CHECK_ENUM(STATIC_OBJECT_NO_PLAYER_COLLIDE);

	CHECK_ENUM(STATIC_OBJECT_SPAWN_ENTITY);
	CHECK_ENUM(STATIC_OBJECT_PICKABLE);
	CHECK_ENUM(STATIC_OBJECT_NO_AUTO_HIDEPOINTS);
	CHECK_ENUM(STATIC_OBJECT_DYNAMIC);
}

TYPE_MIRROR enum StaticSubObjectType
{
	STATIC_SUB_OBJECT_MESH_check,         // This simple geometry part of the multi-sub object geometry.
	STATIC_SUB_OBJECT_HELPER_MESH_check,  // Special helper mesh, not rendered usually, used for broken pieces.
	STATIC_SUB_OBJECT_POINT_check,
	STATIC_SUB_OBJECT_DUMMY_check,
	STATIC_SUB_OBJECT_XREF_check,
	STATIC_SUB_OBJECT_CAMERA_check,
	STATIC_SUB_OBJECT_LIGHT_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (StaticSubObjectType::x ## _check == EStaticSubObjectType::x, "EStaticSubObjectType enumeration has been changed.")

inline void CheckAnother()
{
	CHECK_ENUM(STATIC_SUB_OBJECT_MESH);
	CHECK_ENUM(STATIC_SUB_OBJECT_HELPER_MESH);
	CHECK_ENUM(STATIC_SUB_OBJECT_POINT);
	CHECK_ENUM(STATIC_SUB_OBJECT_DUMMY);
	CHECK_ENUM(STATIC_SUB_OBJECT_XREF);
	CHECK_ENUM(STATIC_SUB_OBJECT_CAMERA);
	CHECK_ENUM(STATIC_SUB_OBJECT_LIGHT);
}


struct SubObject
{

	EStaticSubObjectType nType;
	string name;
	string properties;
	int nParent;          // Index of the parent sub object, if there`s hierarchy between them.
	Matrix34 tm;          // Transformation matrix.
	Matrix34 localTM;     // Local transformation matrix, relative to parent.
	IStatObj *pStatObj;   // Static object for sub part of CGF.
	Vec3 helperSize;      // Size of the helper (if helper).
	struct IRenderMesh *pWeights; // render mesh with a single deformation weights stream
	struct IFoliage *pFoliage;   // for bendable foliage
	unsigned int bIdentityMatrix : 1; // True if sub object matrix is identity.
	unsigned int bHidden : 1; // True if sub object is hidden
	unsigned int bShadowProxy : 1; // Child StatObj has 'shadowproxy' in name
	unsigned int nBreakerJoints : 8; // number of joints that can switch this part to a broken state

	explicit SubObject(IStatObj::SSubObject &other)
	{
		CHECK_TYPES_SIZE(SubObject, IStatObj::SSubObject);

		ASSIGN_FIELD(nType);
		ASSIGN_FIELD(name);
		ASSIGN_FIELD(properties);
		ASSIGN_FIELD(nParent);
		ASSIGN_FIELD(tm);
		ASSIGN_FIELD(localTM);
		ASSIGN_FIELD(pStatObj);
		ASSIGN_FIELD(helperSize);
		ASSIGN_FIELD(pWeights);
		ASSIGN_FIELD(pFoliage);
		ASSIGN_FIELD(bIdentityMatrix);
		ASSIGN_FIELD(bHidden);
		ASSIGN_FIELD(bShadowProxy);
		ASSIGN_FIELD(nBreakerJoints);

		CHECK_TYPE(nType);
		CHECK_TYPE(name);
		CHECK_TYPE(properties);
		CHECK_TYPE(nParent);
		CHECK_TYPE(tm);
		CHECK_TYPE(localTM);
		CHECK_TYPE(pStatObj);
		CHECK_TYPE(helperSize);
		CHECK_TYPE(pWeights);
		CHECK_TYPE(pFoliage);
		CHECK_TYPE(bIdentityMatrix);
		CHECK_TYPE(bHidden);
		CHECK_TYPE(bShadowProxy);
		CHECK_TYPE(nBreakerJoints);
	}
};
struct Statistics
{

	int nVertices;
	int nVerticesPerLod[MAX_STATOBJ_LODS_NUM];
	int nIndices;
	int nIndicesPerLod[MAX_STATOBJ_LODS_NUM];
	int nMeshSize;
	int nMeshSizeLoaded;
	int nPhysProxySize;
	int nPhysProxySizeMax;
	int nPhysPrimitives;
	int nDrawCalls;
	int nLods;
	int nSubMeshCount;
	int nNumRefs;
	bool bSplitLods; // Lods split between files.

	// Optional texture sizer.
	ICrySizer *pTextureSizer;
	ICrySizer *pTextureSizer2;

	explicit Statistics(IStatObj::SStatistics &other)
	{
		static_assert(MAX_STATOBJ_LODS_NUM == 6, "Max number of LOD models has been changed.");

		CHECK_TYPES_SIZE(Statistics, IStatObj::SStatistics);

		ASSIGN_FIELD(nVertices);
		ASSIGN_FIELD(nVerticesPerLod[0]);
		ASSIGN_FIELD(nIndices);
		ASSIGN_FIELD(nIndicesPerLod[0]);
		ASSIGN_FIELD(nMeshSize);
		ASSIGN_FIELD(nMeshSizeLoaded);
		ASSIGN_FIELD(nPhysProxySize);
		ASSIGN_FIELD(nPhysProxySizeMax);
		ASSIGN_FIELD(nPhysPrimitives);
		ASSIGN_FIELD(nDrawCalls);
		ASSIGN_FIELD(nLods);
		ASSIGN_FIELD(nSubMeshCount);
		ASSIGN_FIELD(nNumRefs);
		ASSIGN_FIELD(bSplitLods);

		CHECK_TYPE(nVertices);
		CHECK_TYPE(nVerticesPerLod);
		CHECK_TYPE(nIndices);
		CHECK_TYPE(nIndicesPerLod);
		CHECK_TYPE(nMeshSize);
		CHECK_TYPE(nMeshSizeLoaded);
		CHECK_TYPE(nPhysProxySize);
		CHECK_TYPE(nPhysProxySizeMax);
		CHECK_TYPE(nPhysPrimitives);
		CHECK_TYPE(nDrawCalls);
		CHECK_TYPE(nLods);
		CHECK_TYPE(nSubMeshCount);
		CHECK_TYPE(nNumRefs);
		CHECK_TYPE(bSplitLods);
	}
};