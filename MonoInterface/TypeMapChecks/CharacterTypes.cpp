#include "stdafx.h"

#include "CheckingBasics.h"
#include <ICryAnimation.h>

TYPE_MIRROR enum CharRenderFlags
{
	CS_FLAG_DRAW_MODEL_check = 1 << 0,
	CS_FLAG_DRAW_NEAR_check = 1 << 1,
	CS_FLAG_UPDATE_check = 1 << 2,
	CS_FLAG_UPDATE_ALWAYS_check = 1 << 3,
	CS_FLAG_COMPOUND_BASE_check = 1 << 4,

	CS_FLAG_DRAW_WIREFRAME_check = 1 << 5, //just for debug
	CS_FLAG_DRAW_TANGENTS_check = 1 << 6, //just for debug
	CS_FLAG_DRAW_BINORMALS_check = 1 << 7, //just for debug
	CS_FLAG_DRAW_NORMALS_check = 1 << 8, //just for debug

	CS_FLAG_DRAW_LOCATOR_check = 1 << 9, //just for debug
	CS_FLAG_DRAW_SKELETON_check = 1 << 10,//just for debug

	CS_FLAG_BIAS_SKIN_SORT_DIST_check = 1 << 11,

	CS_FLAG_STREAM_HIGH_PRIORITY_check = 1 << 12
};

#define CHECK_ENUM(x) static_assert (CharRenderFlags::x ## _check == ECharRenderFlags::x, "ECharRenderFlags enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(CS_FLAG_DRAW_MODEL);
	CHECK_ENUM(CS_FLAG_DRAW_NEAR);
	CHECK_ENUM(CS_FLAG_UPDATE);
	CHECK_ENUM(CS_FLAG_UPDATE_ALWAYS);
	CHECK_ENUM(CS_FLAG_COMPOUND_BASE);

	CHECK_ENUM(CS_FLAG_DRAW_WIREFRAME);
	CHECK_ENUM(CS_FLAG_DRAW_TANGENTS);
	CHECK_ENUM(CS_FLAG_DRAW_BINORMALS);
	CHECK_ENUM(CS_FLAG_DRAW_NORMALS);

	CHECK_ENUM(CS_FLAG_DRAW_LOCATOR);
	CHECK_ENUM(CS_FLAG_DRAW_SKELETON);

	CHECK_ENUM(CS_FLAG_BIAS_SKIN_SORT_DIST);

	CHECK_ENUM(CS_FLAG_STREAM_HIGH_PRIORITY);
}

TYPE_MIRROR enum FileFormatIds
{
	CHR_check = 0x11223344,
	CGA_check = 0x55aa55aa
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FileFormatIds::x ## _check == x, "Animation file format ids were changed.")

inline void CheckFileFormatIds()
{
	CHECK_ENUM(CHR);
	CHECK_ENUM(CGA);
}

TYPE_MIRROR enum StreamingDBAPriority
{
	eStreamingDBAPriority_Normal_check = 0,
	eStreamingDBAPriority_Urgent_check = 1
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (StreamingDBAPriority::x ## _check == ICharacterManager::EStreamingDBAPriority::x, "ICharacterManager::EStreamingDBAPriority enumeration has been changed.")

inline void CheckStreamingDBAPriority()
{
	CHECK_ENUM(eStreamingDBAPriority_Normal);
	CHECK_ENUM(eStreamingDBAPriority_Urgent);
}

TYPE_MIRROR enum ReloadCAFResult
{
	CR_RELOAD_FAILED_check,
	CR_RELOAD_SUCCEED_check,
	CR_RELOAD_GAH_NOT_IN_ARRAY_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (ReloadCAFResult::x ## _check == EReloadCAFResult::x, "ICharacterManager::EStreamingDBAPriority enumeration has been changed.")

inline void CheckReloadCAFResult()
{
	CHECK_ENUM(CR_RELOAD_FAILED);
	CHECK_ENUM(CR_RELOAD_SUCCEED);
	CHECK_ENUM(CR_RELOAD_GAH_NOT_IN_ARRAY);
}

TYPE_MIRROR enum ECHRLOADINGFLAGS
{
	CA_IGNORE_LOD_check = 0x01,
	CA_CharEditModel_check = 0x02,
	CA_PreviewMode_check = 0x04,
	CA_DoNotStreamStaticObjects_check = 0x08,
	CA_SkipSkelRecreation_check = 0x10,
	CA_DisableLogWarnings_check = 0x20
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (ECHRLOADINGFLAGS::x ## _check == CHRLOADINGFLAGS::x, "ICharacterManager::EStreamingDBAPriority enumeration has been changed.")

inline void CheckCHRLOADINGFLAGS()
{
	CHECK_ENUM(CA_IGNORE_LOD);
	CHECK_ENUM(CA_CharEditModel);
	CHECK_ENUM(CA_PreviewMode);
	CHECK_ENUM(CA_DoNotStreamStaticObjects);
	CHECK_ENUM(CA_SkipSkelRecreation);
	CHECK_ENUM(CA_DisableLogWarnings);
}

TYPE_MIRROR struct CharacterManagerStatistics
{
	// Number of character instances
	unsigned numCharacters;
	// Number of character models (CGF)
	unsigned numCharModels;
	// Number of animobjects
	unsigned numAnimObjects;
	// Number of animobject models
	unsigned numAnimObjectModels;

	explicit CharacterManagerStatistics(ICharacterManager::Statistics &other)
	{
		CHECK_TYPES_SIZE(CharacterManagerStatistics, ICharacterManager::Statistics);

		ASSIGN_FIELD(numCharacters);
		ASSIGN_FIELD(numCharModels);
		ASSIGN_FIELD(numAnimObjects);
		ASSIGN_FIELD(numAnimObjectModels);

		CHECK_TYPE(numCharacters);
		CHECK_TYPE(numCharModels);
		CHECK_TYPE(numAnimObjects);
		CHECK_TYPE(numAnimObjectModels);
	}
};

TYPE_MIRROR struct AnimationProcessParams
{
	QuatTS locationAnimation;
	bool bOnRender;
	float zoomAdjustedDistanceFromCamera;
	float overrideDeltaTime;

	explicit AnimationProcessParams(SAnimationProcessParams &other)
	{
		CHECK_TYPE_SIZE(AnimationProcessParams);

		ASSIGN_FIELD(locationAnimation);
		ASSIGN_FIELD(bOnRender);
		ASSIGN_FIELD(zoomAdjustedDistanceFromCamera);
		ASSIGN_FIELD(overrideDeltaTime);

		CHECK_TYPE(locationAnimation);
		CHECK_TYPE(bOnRender);
		CHECK_TYPE(zoomAdjustedDistanceFromCamera);
		CHECK_TYPE(overrideDeltaTime);
	}
};