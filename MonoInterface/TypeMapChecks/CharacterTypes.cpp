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

TYPE_MIRROR struct SCryCharAnimationParams
{
	f32 m_fTransTime;
	f32 m_fKeyTime;
	f32	m_fPlaybackSpeed;
	f32	m_fAllowMultilayerAnim;
	int32 m_nLayerID;
	f32 m_fPlaybackWeight;
	uint32 m_nFlags;
	uint32 m_nUserToken;
	f32 m_fUserData[8];

	explicit SCryCharAnimationParams(CryCharAnimationParams &other)
	{
		CHECK_TYPE_SIZE(CryCharAnimationParams);

		ASSIGN_FIELD(m_fTransTime);
		ASSIGN_FIELD(m_fKeyTime);
		ASSIGN_FIELD(m_fPlaybackSpeed);
		ASSIGN_FIELD(m_fAllowMultilayerAnim);
		ASSIGN_FIELD(m_nLayerID);
		ASSIGN_FIELD(m_fPlaybackWeight);
		ASSIGN_FIELD(m_nFlags);
		ASSIGN_FIELD(m_nUserToken);
		ASSIGN_FIELD(m_fUserData[0]);

		CHECK_TYPE(m_fTransTime);
		CHECK_TYPE(m_fKeyTime);
		CHECK_TYPE(m_fPlaybackSpeed);
		CHECK_TYPE(m_fAllowMultilayerAnim);
		CHECK_TYPE(m_nLayerID);
		CHECK_TYPE(m_fPlaybackWeight);
		CHECK_TYPE(m_nFlags);
		CHECK_TYPE(m_nUserToken);
		CHECK_TYPE(m_fUserData);
	}
};

TYPE_MIRROR enum AnimationFlags
{
	CA_MANUAL_UPDATE_check = 0x000001,
	CA_LOOP_ANIMATION_check = 0x000002,
	CA_REPEAT_LAST_KEY_check = 0x000004,
	CA_TRANSITION_TIMEWARPING_check = 0x000008,
	CA_START_AT_KEYTIME_check = 0x000010,
	CA_START_AFTER_check = 0x000020,
	CA_IDLE2MOVE_check = 0x000040,
	CA_MOVE2IDLE_check = 0x000080,
	CA_ALLOW_ANIM_RESTART_check = 0x000100,
	CA_KEYFRAME_SAMPLE_30Hz_check = 0x000200,
	CA_DISABLE_MULTILAYER_check = 0x000400,
	CA_FORCE_SKELETON_UPDATE_check = 0x000800,
	CA_TRACK_VIEW_EXCLUSIVE_check = 0x001000,
	CA_REMOVE_FROM_FIFO_check = 0x002000,
	CA_FULL_ROOT_PRIORITY_check = 0x004000,
	CA_FORCE_TRANSITION_TO_ANIM_check = 0x008000,
	CA_FADEOUT_check = 0x40000000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AnimationFlags::x ## _check == CA_AnimationFlags::x, "CA_AnimationFlags enumeration has been changed.")

inline void CheckAnimationFlags()
{
	CHECK_ENUM(CA_MANUAL_UPDATE);
	CHECK_ENUM(CA_LOOP_ANIMATION);
	CHECK_ENUM(CA_REPEAT_LAST_KEY);
	CHECK_ENUM(CA_TRANSITION_TIMEWARPING);
	CHECK_ENUM(CA_START_AT_KEYTIME);
	CHECK_ENUM(CA_START_AFTER);
	CHECK_ENUM(CA_IDLE2MOVE);
	CHECK_ENUM(CA_MOVE2IDLE);
	CHECK_ENUM(CA_ALLOW_ANIM_RESTART);
	CHECK_ENUM(CA_KEYFRAME_SAMPLE_30Hz);
	CHECK_ENUM(CA_DISABLE_MULTILAYER);
	CHECK_ENUM(CA_FORCE_SKELETON_UPDATE);
	CHECK_ENUM(CA_TRACK_VIEW_EXCLUSIVE);
	CHECK_ENUM(CA_REMOVE_FROM_FIFO);
	CHECK_ENUM(CA_FULL_ROOT_PRIORITY);
	CHECK_ENUM(CA_FORCE_TRANSITION_TO_ANIM);
	CHECK_ENUM(CA_FADEOUT);
}

TYPE_MIRROR struct ParametricSampler
{
	void *vtable;
	uint8	  m_nParametricType;        //Type of Group: i.e. I2M, M2I, MOVE, Idle-Step, Idle-Rot, etc....
	uint8	  m_numDimensions;          //how many dimensions are used in this Parametric Group 
	f32			m_MotionParameter[4];			//we have only 4 dimensions per blend-space
	uint8		m_MotionParameterID[4];		//we have only 4 dimensions per blend-space 
	uint8		m_MotionParameterFlags[4];	//we have only 4 dimensions per blend-space 

	explicit ParametricSampler(SParametricSampler &other)
		: vtable(nullptr)
	{
		CHECK_TYPE_SIZE(ParametricSampler);

		ASSIGN_FIELD(m_nParametricType);
		ASSIGN_FIELD(m_numDimensions);
		ASSIGN_FIELD(m_MotionParameter[0]);
		ASSIGN_FIELD(m_MotionParameterID[0]);
		ASSIGN_FIELD(m_MotionParameterFlags[0]);

		CHECK_TYPE(m_nParametricType);
		CHECK_TYPE(m_numDimensions);
		CHECK_TYPE(m_MotionParameter);
		CHECK_TYPE(m_MotionParameterID);
		CHECK_TYPE(m_MotionParameterFlags);
	}
};

TYPE_MIRROR enum Dimension_Flags
{
	CA_Dim_Initialized_check = 0x001,
	CA_Dim_LockedParameter_check = 0x002,
	CA_Dim_DeltaExtraction_check = 0x004
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (Dimension_Flags::x ## _check == CA_Dimension_Flags::x, "CA_Dimension_Flags enumeration has been changed.")

inline void CheckDimension_Flags()
{
	CHECK_ENUM(CA_Dim_Initialized);
	CHECK_ENUM(CA_Dim_LockedParameter);
	CHECK_ENUM(CA_Dim_DeltaExtraction);
}

TYPE_MIRROR enum MotionParamID
{
	eMotionParamID_TravelSpeed_check = 0,
	eMotionParamID_TurnSpeed_check,
	eMotionParamID_TravelAngle_check,     //forward, backwards and sidestepping  
	eMotionParamID_TravelSlope_check,
	eMotionParamID_TurnAngle_check,       //Idle2Move and idle-rotations
	eMotionParamID_TravelDist_check,      //idle-steps 
	eMotionParamID_StopLeg_check,         //Move2Idle

	eMotionParamID_BlendWeight_check,     //custom parameters
	eMotionParamID_BlendWeight2_check,
	eMotionParamID_BlendWeight3_check,
	eMotionParamID_BlendWeight4_check,
	eMotionParamID_BlendWeight5_check,
	eMotionParamID_BlendWeight6_check,
	eMotionParamID_BlendWeight7_check,
	eMotionParamID_BlendWeight_Last_check = eMotionParamID_BlendWeight7,

	eMotionParamID_COUNT_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (MotionParamID::x ## _check == EMotionParamID::x, "EMotionParamID enumeration has been changed.")

inline void CheckMotionParamID()
{
	CHECK_ENUM(eMotionParamID_TravelSpeed);
	CHECK_ENUM(eMotionParamID_TurnSpeed);
	CHECK_ENUM(eMotionParamID_TravelAngle);
	CHECK_ENUM(eMotionParamID_TravelSlope);
	CHECK_ENUM(eMotionParamID_TurnAngle);
	CHECK_ENUM(eMotionParamID_TravelDist);
	CHECK_ENUM(eMotionParamID_StopLeg);

	CHECK_ENUM(eMotionParamID_BlendWeight);
	CHECK_ENUM(eMotionParamID_BlendWeight2);
	CHECK_ENUM(eMotionParamID_BlendWeight3);
	CHECK_ENUM(eMotionParamID_BlendWeight4);
	CHECK_ENUM(eMotionParamID_BlendWeight5);
	CHECK_ENUM(eMotionParamID_BlendWeight6);
	CHECK_ENUM(eMotionParamID_BlendWeight7);
	CHECK_ENUM(eMotionParamID_BlendWeight_Last);

	CHECK_ENUM(eMotionParamID_COUNT);
}

TYPE_MIRROR enum LayerCountCheck
{
	LayerCount_check = 16
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (LayerCountCheck::x ## _check == ISkeletonAnim::x, "ISkeletonAnim::LayerCount has been changed.")

inline void CheckLayerCount()
{
	CHECK_ENUM(LayerCount);
}

TYPE_MIRROR enum SampleResult
{
	eSR_Success_check,

	eSR_InvalidAnimationId_check,
	eSR_UnsupportedAssetType_check,
	eSR_NotInMemory_check,
	eSR_ControllerNotFound_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (SampleResult::x ## _check == IAnimationSet::ESampleResult::x, "IAnimationSet::ESampleResult enumeration has been changed.")

inline void CheckSampleResult()
{
	CHECK_ENUM(eSR_Success);
	CHECK_ENUM(eSR_InvalidAnimationId);
	CHECK_ENUM(eSR_UnsupportedAssetType);
	CHECK_ENUM(eSR_NotInMemory);
	CHECK_ENUM(eSR_ControllerNotFound);
}

TYPE_MIRROR enum AssetFlags
{
	CA_ASSET_ADDITIVE_check = 0x001,
	CA_ASSET_CYCLE_check = 0x002,
	CA_ASSET_LOADED_check = 0x004,
	CA_ASSET_LMG_check = 0x008,
	CA_ASSET_LMG_VALID_check = 0x020,
	CA_ASSET_CREATED_check = 0x800,
	CA_ASSET_REQUESTED_check = 0x1000,
	CA_ASSET_ONDEMAND_check = 0x2000,
	CA_AIMPOSE_check = 0x4000,
	CA_AIMPOSE_UNLOADED_check = 0x8000,
	CA_ASSET_NOT_FOUND_check = 0x10000,
	CA_ASSET_TCB_check = 0x20000,
	CA_ASSET_INTERNALTYPE_check = 0x40000,
	CA_ASSET_BIG_ENDIAN_check = 0x80000000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AssetFlags::x ## _check == CA_AssetFlags::x, "CA_AssetFlags enumeration has been changed.")

inline void CheckAssetFlags()
{
	CHECK_ENUM(CA_ASSET_ADDITIVE);
	CHECK_ENUM(CA_ASSET_CYCLE);
	CHECK_ENUM(CA_ASSET_LOADED);
	CHECK_ENUM(CA_ASSET_LMG);
	CHECK_ENUM(CA_ASSET_LMG_VALID);
	CHECK_ENUM(CA_ASSET_CREATED);
	CHECK_ENUM(CA_ASSET_REQUESTED);
	CHECK_ENUM(CA_ASSET_ONDEMAND);
	CHECK_ENUM(CA_AIMPOSE);
	CHECK_ENUM(CA_AIMPOSE_UNLOADED);
	CHECK_ENUM(CA_ASSET_NOT_FOUND);
	CHECK_ENUM(CA_ASSET_TCB);
	CHECK_ENUM(CA_ASSET_INTERNALTYPE);
	CHECK_ENUM(CA_ASSET_BIG_ENDIAN);
}