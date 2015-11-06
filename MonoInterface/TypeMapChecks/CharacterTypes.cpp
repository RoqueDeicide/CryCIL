#include "stdafx.h"

#include "CheckingBasics.h"
#include <ICryAnimation.h>
#include <IFacialAnimation.h>

TYPE_MIRROR enum CharRenderFlags
{
	CS_FLAG_DRAW_MODEL_check = 1 << 0,
	CS_FLAG_DRAW_NEAR_check = 1 << 1,
	CS_FLAG_UPDATE_check = 1 << 2,
	CS_FLAG_UPDATE_ALWAYS_check = 1 << 3,
	CS_FLAG_COMPOUND_BASE_check = 1 << 4,

	CS_FLAG_DRAW_WIREFRAME_check = 1 << 5,
	CS_FLAG_DRAW_TANGENTS_check = 1 << 6,
	CS_FLAG_DRAW_BINORMALS_check = 1 << 7,
	CS_FLAG_DRAW_NORMALS_check = 1 << 8,

	CS_FLAG_DRAW_LOCATOR_check = 1 << 9,
	CS_FLAG_DRAW_SKELETON_check = 1 << 10,

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
	unsigned numCharacters;
	unsigned numCharModels;
	unsigned numAnimObjects;
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
	uint8	  m_nParametricType;
	uint8	  m_numDimensions;
	f32			m_MotionParameter[4];
	uint8		m_MotionParameterID[4];
	uint8		m_MotionParameterFlags[4];

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
	eMotionParamID_TravelAngle_check,
	eMotionParamID_TravelSlope_check,
	eMotionParamID_TurnAngle_check,
	eMotionParamID_TravelDist_check,
	eMotionParamID_StopLeg_check,

	eMotionParamID_BlendWeight_check,
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

TYPE_MIRROR enum FacialSequenceLayer
{
	eFacialSequenceLayer_Preview_check,
	eFacialSequenceLayer_Dialogue_check,
	eFacialSequenceLayer_Trackview_check,
	eFacialSequenceLayer_AGStateAndAIAlertness_check,
	eFacialSequenceLayer_Mannequin_check,
	eFacialSequenceLayer_AIExpression_check,
	eFacialSequenceLayer_FlowGraph_check,

	eFacialSequenceLayer_COUNT_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialSequenceLayer::x ## _check == EFacialSequenceLayer::x, "EFacialSequenceLayer enumeration has been changed.")

inline void CheckFacialSequenceLayer()
{
	CHECK_ENUM(eFacialSequenceLayer_Preview);
	CHECK_ENUM(eFacialSequenceLayer_Dialogue);
	CHECK_ENUM(eFacialSequenceLayer_Trackview);
	CHECK_ENUM(eFacialSequenceLayer_AGStateAndAIAlertness);
	CHECK_ENUM(eFacialSequenceLayer_Mannequin);
	CHECK_ENUM(eFacialSequenceLayer_AIExpression);
	CHECK_ENUM(eFacialSequenceLayer_FlowGraph);

	CHECK_ENUM(eFacialSequenceLayer_COUNT);
}

TYPE_MIRROR enum FacialEffectorType
{
	EFE_TYPE_GROUP_check = 0x00,
	EFE_TYPE_EXPRESSION_check = 0x01,
	EFE_TYPE_MORPH_TARGET_check = 0x02,
	EFE_TYPE_BONE_check = 0x03,
	EFE_TYPE_MATERIAL_check = 0x04,
	EFE_TYPE_ATTACHMENT_check = 0x05
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialEffectorType::x ## _check == EFacialEffectorType::x, "EFacialEffectorType enumeration has been changed.")

inline void CheckFacialEffectorType()
{
	CHECK_ENUM(EFE_TYPE_GROUP);
	CHECK_ENUM(EFE_TYPE_EXPRESSION);
	CHECK_ENUM(EFE_TYPE_MORPH_TARGET);
	CHECK_ENUM(EFE_TYPE_BONE);
	CHECK_ENUM(EFE_TYPE_MATERIAL);
	CHECK_ENUM(EFE_TYPE_ATTACHMENT);
}

TYPE_MIRROR enum FacialEffectorFlags
{
	EFE_FLAG_ROOT_check = 0x00001,

	EFE_FLAG_UI_EXTENDED_check = 0x01000,
	EFE_FLAG_UI_MODIFIED_check = 0x02000,
	EFE_FLAG_UI_PREVIEW_check = 0x04000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialEffectorFlags::x ## _check == EFacialEffectorFlags::x, "EFacialEffectorFlags enumeration has been changed.")

inline void CheckFacialEffectorFlags()
{
	CHECK_ENUM(EFE_FLAG_ROOT);

	CHECK_ENUM(EFE_FLAG_UI_EXTENDED);
	CHECK_ENUM(EFE_FLAG_UI_MODIFIED);
	CHECK_ENUM(EFE_FLAG_UI_PREVIEW);
}

TYPE_MIRROR enum FacialEffectorParam
{
	EFE_PARAM_BONE_NAME_check,
	EFE_PARAM_BONE_ROT_AXIS_check,
	EFE_PARAM_BONE_POS_AXIS_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialEffectorParam::x ## _check == EFacialEffectorParam::x, "EFacialEffectorParam enumeration has been changed.")

inline void CheckFacialEffectorParam()
{
	CHECK_ENUM(EFE_PARAM_BONE_NAME);
	CHECK_ENUM(EFE_PARAM_BONE_ROT_AXIS);
	CHECK_ENUM(EFE_PARAM_BONE_POS_AXIS);
}

TYPE_MIRROR enum FacialEffCtrlControlType
{
	CTRL_LINEAR_check,
	CTRL_SPLINE_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialEffCtrlControlType::x ## _check == IFacialEffCtrl::x, "IFacialEffCtrl::ControlType enumeration has been changed.")

inline void CheckFacialEffCtrlControlType()
{
	CHECK_ENUM(CTRL_LINEAR);
	CHECK_ENUM(CTRL_SPLINE);
}

TYPE_MIRROR enum FacialEffCtrlControlFlags
{
	CTRL_FLAG_UI_EXPENDED_check = 0x01000
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (FacialEffCtrlControlFlags::x ## _check == IFacialEffCtrl::x, "IFacialEffCtrl::ControlFlags enumeration has been changed.")

inline void CheckFacialEffCtrlControlFlags()
{
	CHECK_ENUM(CTRL_FLAG_UI_EXPENDED);
}

TYPE_MIRROR struct FaceIdentifier
{
	const char *str;
	uint32 crc;

	explicit FaceIdentifier()
	{
		CHECK_TYPES_SIZE(FaceIdentifier, CFaceIdentifierHandle);

		str = nullptr;
		crc = 0;
	}
};

TYPE_MIRROR struct PhonemeInfo
{
	wchar_t codeIPA;
	char ASCII[4];
	const char* description;

	explicit PhonemeInfo(const SPhonemeInfo &other)
	{
		CHECK_TYPE_SIZE(PhonemeInfo);

		ASSIGN_FIELD(codeIPA);
		ASSIGN_FIELD(ASCII[0]);
		ASSIGN_FIELD(description);

		CHECK_TYPE(codeIPA);
		CHECK_TYPE(ASCII);
		CHECK_TYPE(description);
	}
};