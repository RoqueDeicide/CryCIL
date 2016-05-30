#include "stdafx.h"

#include "CheckingBasics.h"

typedef uint32 TestAudioIdType;

typedef TestAudioIdType TestAudioObjectId;
#define INVALID_AUDIO_OBJECT_ID_check ((TestAudioObjectId)(0))
#define GLOBAL_AUDIO_OBJECT_ID_check ((TestAudioObjectId)(1))
typedef TestAudioIdType TestAudioControlId;
#define INVALID_AUDIO_CONTROL_ID_check ((TestAudioControlId)(0))
typedef TestAudioIdType TestAudioSwitchStateId;
#define INVALID_AUDIO_SWITCH_STATE_ID_check ((TestAudioSwitchStateId)(0))
typedef TestAudioIdType TestAudioEnvironmentId;
#define INVALID_AUDIO_ENVIRONMENT_ID_check ((TestAudioEnvironmentId)(0))
typedef TestAudioIdType TestAudioPreloadRequestId;
#define INVALID_AUDIO_PRELOAD_REQUEST_ID_check ((TestAudioPreloadRequestId)(0))
typedef TestAudioIdType TestAudioEventId;
#define INVALID_AUDIO_EVENT_ID_check ((TestAudioEventId)(0))
typedef TestAudioIdType TestAudioFileEntryId;
#define INVALID_AUDIO_FILE_ENTRY_ID_check ((TestAudioFileEntryId)(0))
typedef TestAudioIdType TestAudioTriggerImplId;
#define INVALID_AUDIO_TRIGGER_IMPL_ID_check ((TestAudioTriggerImplId)(0))
typedef TestAudioIdType TestAudioTriggerInstanceId;
#define INVALID_AUDIO_TRIGGER_INSTANCE_ID_check ((TestAudioTriggerInstanceId)(0))
typedef TestAudioIdType TestAudioEnumFlagsType;
#define INVALID_AUDIO_ENUM_FLAG_TYPE_check ((TestAudioEnumFlagsType)(0))
#define ALL_AUDIO_REQUEST_SPECIFIC_TYPE_FLAGS_check ((TestAudioEnumFlagsType)(0xFFFFFFFF))
typedef TestAudioIdType TestAudioProxyId;
#define INVALID_AUDIO_PROXY_ID_check ((TestAudioProxyId)(0))
#define DEFAULT_AUDIO_PROXY_ID_check ((TestAudioProxyId)(1))

#define CHECK_BASE_AUDIO_TYPE(type) static_assert(is_same_type<type, Test ## type>::value, #type " now uses different base type.")

inline void CheckAudioTypes()
{
	CHECK_BASE_AUDIO_TYPE(AudioIdType);
	CHECK_BASE_AUDIO_TYPE(AudioControlId);
	CHECK_BASE_AUDIO_TYPE(AudioSwitchStateId);
	CHECK_BASE_AUDIO_TYPE(AudioEnvironmentId);
	CHECK_BASE_AUDIO_TYPE(AudioPreloadRequestId);
	CHECK_BASE_AUDIO_TYPE(AudioEventId);
	CHECK_BASE_AUDIO_TYPE(AudioFileEntryId);
	CHECK_BASE_AUDIO_TYPE(AudioTriggerImplId);
	CHECK_BASE_AUDIO_TYPE(AudioTriggerInstanceId);
	CHECK_BASE_AUDIO_TYPE(AudioEnumFlagsType);
	CHECK_BASE_AUDIO_TYPE(AudioProxyId);
}

#define CHECK_AUDIO_SPECIAL_VALUE(name) static_assert(name == name ## _check, #name " has been changed.")

inline void CheckAudioObjectSpecialValues()
{
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_OBJECT_ID);
	CHECK_AUDIO_SPECIAL_VALUE(GLOBAL_AUDIO_OBJECT_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_CONTROL_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_SWITCH_STATE_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_ENVIRONMENT_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_PRELOAD_REQUEST_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_EVENT_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_FILE_ENTRY_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_TRIGGER_IMPL_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_TRIGGER_INSTANCE_ID);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_ENUM_FLAG_TYPE);
	CHECK_AUDIO_SPECIAL_VALUE(ALL_AUDIO_REQUEST_SPECIFIC_TYPE_FLAGS);
	CHECK_AUDIO_SPECIAL_VALUE(INVALID_AUDIO_PROXY_ID);
	CHECK_AUDIO_SPECIAL_VALUE(DEFAULT_AUDIO_PROXY_ID);
}

TYPE_MIRROR struct AudioObjectTransformation
{
	Vec3 m_position;
	Vec3 m_forward;
	Vec3 m_up;

	explicit AudioObjectTransformation(const CAudioObjectTransformation &other)
	{
		static_assert(sizeof(AudioObjectTransformation) == sizeof(CAudioObjectTransformation), "CAudioObjectTransformation structure has been changed.");

		auto position = other.GetPosition();
		auto forward  = other.GetForward();
		auto up       = other.GetUp();

		this->m_position = position;
		this->m_forward  = forward;
		this->m_up       = up;

		static_assert (is_same_type<decltype(this->m_position), decltype(position)>::value, "Type of the field named " "m_position" " has been changed.");
		static_assert (is_same_type<decltype(this->m_forward), decltype(forward)>::value,   "Type of the field named " "m_forward" " has been changed.");
		static_assert (is_same_type<decltype(this->m_up), decltype(up)>::value,             "Type of the field named " "m_up" " has been changed.");
	}
};

TYPE_MIRROR enum AudioTypesReservedValues
{
	AUDIO_TRIGGER_IMPL_ID_NUM_RESERVED_check = 100,
	MAX_AUDIO_FILE_PATH_LENGTH_check         = 256,
	MAX_AUDIO_FILE_NAME_LENGTH_check         = 128,
	MAX_AUDIO_OBJECT_NAME_LENGTH_check       = 256
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioTypesReservedValues::x ## _check == x, "Some values common to audio API have been changed.")

inline void CheckAudioTypesReservedValues()
{
	CHECK_ENUM(AUDIO_TRIGGER_IMPL_ID_NUM_RESERVED);
	CHECK_ENUM(MAX_AUDIO_FILE_PATH_LENGTH);
	CHECK_ENUM(MAX_AUDIO_FILE_NAME_LENGTH);
	CHECK_ENUM(MAX_AUDIO_OBJECT_NAME_LENGTH);
}

TYPE_MIRROR enum AudioRequestFlags
{
	eAudioRequestFlags_None_check                 = 0,
	eAudioRequestFlags_PriorityNormal_check       = BIT(0),
	eAudioRequestFlags_PriorityHigh_check         = BIT(1),
	eAudioRequestFlags_ExecuteBlocking_check      = BIT(2),
	eAudioRequestFlags_SyncCallback_check         = BIT(3),
	eAudioRequestFlags_SyncFinishedCallback_check = BIT(4),
	eAudioRequestFlags_StayInMemory_check         = BIT(5),
	eAudioRequestFlags_ThreadSafePush_check       = BIT(6)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioRequestFlags::x ## _check == EAudioRequestFlags::x, "EAudioRequestFlags enumeration have been changed.")

inline void CheckAudioRequestFlags()
{
	CHECK_ENUM(eAudioRequestFlags_None);
	CHECK_ENUM(eAudioRequestFlags_PriorityNormal);
	CHECK_ENUM(eAudioRequestFlags_PriorityHigh);
	CHECK_ENUM(eAudioRequestFlags_ExecuteBlocking);
	CHECK_ENUM(eAudioRequestFlags_SyncCallback);
	CHECK_ENUM(eAudioRequestFlags_SyncFinishedCallback);
	CHECK_ENUM(eAudioRequestFlags_StayInMemory);
	CHECK_ENUM(eAudioRequestFlags_ThreadSafePush);
}

TYPE_MIRROR enum AudioRequestType
{
	eAudioRequestType_None_check                        = 0,
	eAudioRequestType_AudioManagerRequest_check         = 1,
	eAudioRequestType_AudioCallbackManagerRequest_check = 2,
	eAudioRequestType_AudioObjectRequest_check          = 3,
	eAudioRequestType_AudioListenerRequest_check        = 4,
	eAudioRequestType_AudioAllRequests_check            = 0xFFFFFFFF
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioRequestType::x ## _check == EAudioRequestType::x, "EAudioRequestType enumeration have been changed.")

inline void CheckAudioRequestType()
{
	CHECK_ENUM(eAudioRequestType_None);
	CHECK_ENUM(eAudioRequestType_AudioManagerRequest);
	CHECK_ENUM(eAudioRequestType_AudioCallbackManagerRequest);
	CHECK_ENUM(eAudioRequestType_AudioObjectRequest);
	CHECK_ENUM(eAudioRequestType_AudioListenerRequest);
	CHECK_ENUM(eAudioRequestType_AudioAllRequests);
}

TYPE_MIRROR enum AudioRequestResult
{
	eAudioRequestResult_None_check    = 0,
	eAudioRequestResult_Success_check = 1,
	eAudioRequestResult_Failure_check = 2
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioRequestResult::x ## _check == EAudioRequestResult::x, "EAudioRequestResult enumeration have been changed.")

inline void CheckAudioRequestResult()
{
	CHECK_ENUM(eAudioRequestResult_None);
	CHECK_ENUM(eAudioRequestResult_Success);
	CHECK_ENUM(eAudioRequestResult_Failure);
}

TYPE_MIRROR enum AudioDataScope
{
	eAudioDataScope_None_check          = 0,
	eAudioDataScope_Global_check        = 1,
	eAudioDataScope_LevelSpecific_check = 2,
	eAudioDataScope_All_check           = 3
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioDataScope::x ## _check == EAudioDataScope::x, "EAudioDataScope enumeration have been changed.")

inline void CheckAudioDataScope()
{
	CHECK_ENUM(eAudioDataScope_None);
	CHECK_ENUM(eAudioDataScope_Global);
	CHECK_ENUM(eAudioDataScope_LevelSpecific);
	CHECK_ENUM(eAudioDataScope_All);
}

TYPE_MIRROR enum AudioManagerRequestType
{
	eAudioManagerRequestType_None_check                   = 0,
	eAudioManagerRequestType_SetAudioImpl_check           = BIT(0),
	eAudioManagerRequestType_ReleaseAudioImpl_check       = BIT(1),
	eAudioManagerRequestType_RefreshAudioSystem_check     = BIT(2),
	eAudioManagerRequestType_ReserveAudioObjectId_check   = BIT(3),
	eAudioManagerRequestType_LoseFocus_check              = BIT(4),
	eAudioManagerRequestType_GetFocus_check               = BIT(5),
	eAudioManagerRequestType_MuteAll_check                = BIT(6),
	eAudioManagerRequestType_UnmuteAll_check              = BIT(7),
	eAudioManagerRequestType_StopAllSounds_check          = BIT(8),
	eAudioManagerRequestType_ParseControlsData_check      = BIT(9),
	eAudioManagerRequestType_ParsePreloadsData_check      = BIT(10),
	eAudioManagerRequestType_ClearControlsData_check      = BIT(11),
	eAudioManagerRequestType_ClearPreloadsData_check      = BIT(12),
	eAudioManagerRequestType_PreloadSingleRequest_check   = BIT(13),
	eAudioManagerRequestType_UnloadSingleRequest_check    = BIT(14),
	eAudioManagerRequestType_UnloadAFCMDataByScope_check  = BIT(15),
	eAudioManagerRequestType_DrawDebugInfo_check          = BIT(16), // Only used internally!
	eAudioManagerRequestType_AddRequestListener_check     = BIT(17),
	eAudioManagerRequestType_RemoveRequestListener_check  = BIT(18),
	eAudioManagerRequestType_ChangeLanguage_check         = BIT(19),
	eAudioManagerRequestType_RetriggerAudioControls_check = BIT(20),
	eAudioManagerRequestType_ReleasePendingRays_check     = BIT(21), //!< Only used internally!
	eAudioManagerRequestType_ReloadControlsData_check     = BIT(22)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioManagerRequestType::x ## _check == EAudioManagerRequestType::x, "EAudioManagerRequestType enumeration have been changed.")

inline void CheckAudioManagerRequestType()
{
	CHECK_ENUM(eAudioManagerRequestType_None);
	CHECK_ENUM(eAudioManagerRequestType_SetAudioImpl);
	CHECK_ENUM(eAudioManagerRequestType_ReleaseAudioImpl);
	CHECK_ENUM(eAudioManagerRequestType_RefreshAudioSystem);
	CHECK_ENUM(eAudioManagerRequestType_ReserveAudioObjectId);
	CHECK_ENUM(eAudioManagerRequestType_LoseFocus);
	CHECK_ENUM(eAudioManagerRequestType_GetFocus);
	CHECK_ENUM(eAudioManagerRequestType_MuteAll);
	CHECK_ENUM(eAudioManagerRequestType_UnmuteAll);
	CHECK_ENUM(eAudioManagerRequestType_StopAllSounds);
	CHECK_ENUM(eAudioManagerRequestType_ParseControlsData);
	CHECK_ENUM(eAudioManagerRequestType_ParsePreloadsData);
	CHECK_ENUM(eAudioManagerRequestType_ClearControlsData);
	CHECK_ENUM(eAudioManagerRequestType_ClearPreloadsData);
	CHECK_ENUM(eAudioManagerRequestType_PreloadSingleRequest);
	CHECK_ENUM(eAudioManagerRequestType_UnloadSingleRequest);
	CHECK_ENUM(eAudioManagerRequestType_UnloadAFCMDataByScope);
	CHECK_ENUM(eAudioManagerRequestType_DrawDebugInfo);                            // Only used internally!
	CHECK_ENUM(eAudioManagerRequestType_AddRequestListener);
	CHECK_ENUM(eAudioManagerRequestType_RemoveRequestListener);
	CHECK_ENUM(eAudioManagerRequestType_ChangeLanguage);
	CHECK_ENUM(eAudioManagerRequestType_RetriggerAudioControls);
	CHECK_ENUM(eAudioManagerRequestType_ReleasePendingRays);
	CHECK_ENUM(eAudioManagerRequestType_ReloadControlsData);
}

TYPE_MIRROR enum AudioCallbackManagerRequestType
{
	eAudioCallbackManagerRequestType_None_check                          = 0,
	eAudioCallbackManagerRequestType_ReportStartedEvent_check            = BIT(0), //!< Only relevant for delayed playback.
	eAudioCallbackManagerRequestType_ReportFinishedEvent_check           = BIT(1), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportFinishedTriggerInstance_check = BIT(2), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportStartedFile_check             = BIT(3), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportStoppedFile_check             = BIT(4), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportProcessedObstructionRay_check = BIT(5), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportVirtualizedEvent_check        = BIT(6), //!< Only used internally!
	eAudioCallbackManagerRequestType_ReportPhysicalizedEvent_check       = BIT(7)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioCallbackManagerRequestType::x ## _check == EAudioCallbackManagerRequestType::x, "EAudioCallbackManagerRequestType enumeration have been changed.")

inline void CheckAudioCallbackManagerRequestType()
{
	CHECK_ENUM(eAudioCallbackManagerRequestType_None);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportStartedEvent);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportFinishedEvent);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportFinishedTriggerInstance);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportStartedFile);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportStoppedFile);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportProcessedObstructionRay);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportVirtualizedEvent);
	CHECK_ENUM(eAudioCallbackManagerRequestType_ReportPhysicalizedEvent);
}

TYPE_MIRROR enum AudioListenerRequestType
{
	eAudioListenerRequestType_None_check              = 0,
	eAudioListenerRequestType_SetTransformation_check = BIT(0)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioListenerRequestType::x ## _check == EAudioListenerRequestType::x, "EAudioListenerRequestType enumeration have been changed.")

inline void CheckAudioListenerRequestType()
{
	CHECK_ENUM(eAudioListenerRequestType_None);
	CHECK_ENUM(eAudioListenerRequestType_SetTransformation);
}

TYPE_MIRROR enum AudioObjectRequestType
{
	eAudioObjectRequestType_None_check                 = 0,
	eAudioObjectRequestType_PrepareTrigger_check       = BIT(0),
	eAudioObjectRequestType_UnprepareTrigger_check     = BIT(1),
	eAudioObjectRequestType_PlayFile_check             = BIT(2),
	eAudioObjectRequestType_StopFile_check             = BIT(3),
	eAudioObjectRequestType_ExecuteTrigger_check       = BIT(4),
	eAudioObjectRequestType_StopTrigger_check          = BIT(5),
	eAudioObjectRequestType_StopAllTriggers_check      = BIT(6),
	eAudioObjectRequestType_SetTransformation_check    = BIT(7),
	eAudioObjectRequestType_SetRtpcValue_check         = BIT(8),
	eAudioObjectRequestType_SetSwitchState_check       = BIT(9),
	eAudioObjectRequestType_SetVolume_check            = BIT(10),
	eAudioObjectRequestType_SetEnvironmentAmount_check = BIT(11),
	eAudioObjectRequestType_ResetEnvironments_check    = BIT(12),
	eAudioObjectRequestType_ReleaseObject_check        = BIT(13)
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioObjectRequestType::x ## _check == EAudioObjectRequestType::x, "EAudioObjectRequestType enumeration have been changed.")

inline void CheckAudioObjectRequestType()
{
	CHECK_ENUM(eAudioObjectRequestType_None);
	CHECK_ENUM(eAudioObjectRequestType_PrepareTrigger);
	CHECK_ENUM(eAudioObjectRequestType_UnprepareTrigger);
	CHECK_ENUM(eAudioObjectRequestType_PlayFile);
	CHECK_ENUM(eAudioObjectRequestType_StopFile);
	CHECK_ENUM(eAudioObjectRequestType_ExecuteTrigger);
	CHECK_ENUM(eAudioObjectRequestType_StopTrigger);
	CHECK_ENUM(eAudioObjectRequestType_StopAllTriggers);
	CHECK_ENUM(eAudioObjectRequestType_SetTransformation);
	CHECK_ENUM(eAudioObjectRequestType_SetRtpcValue);
	CHECK_ENUM(eAudioObjectRequestType_SetSwitchState);
	CHECK_ENUM(eAudioObjectRequestType_SetVolume);
	CHECK_ENUM(eAudioObjectRequestType_SetEnvironmentAmount);
	CHECK_ENUM(eAudioObjectRequestType_ResetEnvironments);
	CHECK_ENUM(eAudioObjectRequestType_ReleaseObject);
}

TYPE_MIRROR enum AudioOcclusionType
{
	eAudioOcclusionType_None_check      = 0,
	eAudioOcclusionType_Ignore_check    = 1,
	eAudioOcclusionType_SingleRay_check = 2,
	eAudioOcclusionType_MultiRay_check  = 3,

	eAudioOcclusionType_Count_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioOcclusionType::x ## _check == EAudioOcclusionType::x, "EAudioOcclusionType enumeration have been changed.")

inline void CheckAudioObjectObstructionCalcType()
{
	CHECK_ENUM(eAudioOcclusionType_None);
	CHECK_ENUM(eAudioOcclusionType_Ignore);
	CHECK_ENUM(eAudioOcclusionType_SingleRay);
	CHECK_ENUM(eAudioOcclusionType_MultiRay);

	CHECK_ENUM(eAudioOcclusionType_Count);
}

TYPE_MIRROR enum AudioControlType
{
	eAudioControlType_None_check        = 0,
	eAudioControlType_AudioObject_check = 1,
	eAudioControlType_Trigger_check     = 2,
	eAudioControlType_Rtpc_check        = 3,
	eAudioControlType_Switch_check      = 4,
	eAudioControlType_SwitchState_check = 5,
	eAudioControlType_Preload_check     = 6,
	eAudioControlType_Environment_check = 7
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (AudioControlType::x ## _check == EAudioControlType::x, "EAudioControlType enumeration have been changed.")

inline void CheckAudioControlType()
{
	CHECK_ENUM(eAudioControlType_None);
	CHECK_ENUM(eAudioControlType_AudioObject);
	CHECK_ENUM(eAudioControlType_Trigger);
	CHECK_ENUM(eAudioControlType_Rtpc);
	CHECK_ENUM(eAudioControlType_Switch);
	CHECK_ENUM(eAudioControlType_SwitchState);
	CHECK_ENUM(eAudioControlType_Preload);
	CHECK_ENUM(eAudioControlType_Environment);
}

TYPE_MIRROR enum LipSyncMethod
{
	eLSM_None_check,
	eLSM_MatchAnimationToSoundName_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (LipSyncMethod::x ## _check == ELipSyncMethod::x, "ELipSyncMethod enumeration have been changed.")

inline void CheckLipSyncMethod()
{
	CHECK_ENUM(eLSM_None);
	CHECK_ENUM(eLSM_MatchAnimationToSoundName);
}