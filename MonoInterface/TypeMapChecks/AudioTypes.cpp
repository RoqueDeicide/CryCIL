#include "stdafx.h"

#include "CheckingBasics.h"

typedef uint32 ATLIDType;

typedef ATLIDType AudioObjectID;
#define INVALID_AUDIO_OBJECT_ID_check ((AudioObjectID)(0))
#define GLOBAL_AUDIO_OBJECT_ID_check ((AudioObjectID)(1))
typedef ATLIDType AudioControlID;
#define INVALID_AUDIO_CONTROL_ID_check ((AudioControlID)(0))
typedef ATLIDType AudioSwitchStateID;
#define INVALID_AUDIO_SWITCH_STATE_ID_check ((AudioSwitchStateID)(0))
typedef ATLIDType AudioEnvironmentID;
#define INVALID_AUDIO_ENVIRONMENT_ID_check ((AudioEnvironmentID)(0))
typedef ATLIDType AudioPreloadRequestID;
#define INVALID_AUDIO_PRELOAD_REQUEST_ID_check ((AudioPreloadRequestID)(0))
typedef ATLIDType AudioEventID;
#define INVALID_AUDIO_EVENT_ID_check ((AudioEventID)(0))
typedef ATLIDType AudioFileEntryID;
#define INVALID_AUDIO_FILE_ENTRY_ID_check ((AudioFileEntryID)(0))
typedef ATLIDType AudioTriggerImplID;
#define INVALID_AUDIO_TRIGGER_IMPL_ID_check ((AudioTriggerImplID)(0))
typedef ATLIDType AudioTriggerInstanceID;
#define INVALID_AUDIO_TRIGGER_INSTANCE_ID_check ((AudioTriggerInstanceID)(0))
typedef ATLIDType ATLEnumFlagsType;
#define INVALID_AUDIO_ENUM_FLAG_TYPE_check ((ATLEnumFlagsType)(0))
#define ALL_AUDIO_REQUEST_SPECIFIC_TYPE_FLAGS_check ((ATLEnumFlagsType)(0xFFFFFFFF))
typedef ATLIDType AudioProxyID;
#define INVALID_AUDIO_PROXY_ID_check ((AudioProxyID)(0))
#define DEFAULT_AUDIO_PROXY_ID_check ((AudioProxyID)(1))

#define CHECK_BASE_AUDIO_TYPE(type) static_assert(is_same_type<type, T##type>::value, "T"#type" now uses different base type.")

inline void CheckAudioTypes()
{
	CHECK_BASE_AUDIO_TYPE(ATLIDType);
	CHECK_BASE_AUDIO_TYPE(AudioControlID);
	CHECK_BASE_AUDIO_TYPE(AudioSwitchStateID);
	CHECK_BASE_AUDIO_TYPE(AudioEnvironmentID);
	CHECK_BASE_AUDIO_TYPE(AudioPreloadRequestID);
	CHECK_BASE_AUDIO_TYPE(AudioEventID);
	CHECK_BASE_AUDIO_TYPE(AudioFileEntryID);
	CHECK_BASE_AUDIO_TYPE(AudioTriggerImplID);
	CHECK_BASE_AUDIO_TYPE(AudioTriggerInstanceID);
	CHECK_BASE_AUDIO_TYPE(ATLEnumFlagsType);
	CHECK_BASE_AUDIO_TYPE(AudioProxyID);
}

#define CHECK_AUDIO_SPECIAL_VALUE(name) static_assert(name == name##_check, #name" has been changed.")

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

TYPE_MIRROR struct ATLWorldPosition
{
	Matrix34 mPosition;

	explicit ATLWorldPosition(const SATLWorldPosition &other)
	{
		CHECK_TYPE_SIZE(ATLWorldPosition);

		ASSIGN_FIELD(mPosition);

		CHECK_TYPE(mPosition);
	}
};

TYPE_MIRROR enum AudioTypesReservedValues
{
	AUDIO_TRIGGER_IMPL_ID_NUM_RESERVED_check = 100,
	MAX_AUDIO_FILE_PATH_LENGTH_check = 256,
	MAX_AUDIO_FILE_NAME_LENGTH_check = 128,
	MAX_AUDIO_OBJECT_NAME_LENGTH_check = 256
};

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
	eARF_NONE_check = 0,
	eARF_PRIORITY_NORMAL_check = BIT(0),
	eARF_PRIORITY_HIGH_check = BIT(1),
	eARF_EXECUTE_BLOCKING_check = BIT(2),
	eARF_SYNC_CALLBACK_check = BIT(3),
	eARF_SYNC_FINISHED_CALLBACK_check = BIT(4),
	eARF_STAY_IN_MEMORY_check = BIT(5),
	eARF_THREAD_SAFE_PUSH_check = BIT(6)
};

#define CHECK_ENUM(x) static_assert (AudioRequestFlags::x ## _check == EAudioRequestFlags::x, "EAudioRequestFlags enumeration have been changed.")

inline void CheckAudioRequestFlags()
{
	CHECK_ENUM(eARF_NONE);
	CHECK_ENUM(eARF_PRIORITY_NORMAL);
	CHECK_ENUM(eARF_PRIORITY_HIGH);
	CHECK_ENUM(eARF_EXECUTE_BLOCKING);
	CHECK_ENUM(eARF_SYNC_CALLBACK);
	CHECK_ENUM(eARF_SYNC_FINISHED_CALLBACK);
	CHECK_ENUM(eARF_STAY_IN_MEMORY);
	CHECK_ENUM(eARF_THREAD_SAFE_PUSH);
}

TYPE_MIRROR enum AudioRequestType
{
	eART_NONE_check = 0,
	eART_AUDIO_MANAGER_REQUEST_check = 1,
	eART_AUDIO_CALLBACK_MANAGER_REQUEST_check = 2,
	eART_AUDIO_OBJECT_REQUEST_check = 3,
	eART_AUDIO_LISTENER_REQUEST_check = 4,
	eART_AUDIO_ALL_REQUESTS_check = 0xFFFFFFFF
};

#define CHECK_ENUM(x) static_assert (AudioRequestType::x ## _check == EAudioRequestType::x, "EAudioRequestType enumeration have been changed.")

inline void CheckAudioRequestType()
{
	CHECK_ENUM(eART_NONE);
	CHECK_ENUM(eART_AUDIO_MANAGER_REQUEST);
	CHECK_ENUM(eART_AUDIO_CALLBACK_MANAGER_REQUEST);
	CHECK_ENUM(eART_AUDIO_OBJECT_REQUEST);
	CHECK_ENUM(eART_AUDIO_LISTENER_REQUEST);
	CHECK_ENUM(eART_AUDIO_ALL_REQUESTS);
}

TYPE_MIRROR enum AudioRequestResult
{
	eARR_NONE_check = 0,
	eARR_SUCCESS_check = 1,
	eARR_FAILURE_check = 2
};

#define CHECK_ENUM(x) static_assert (AudioRequestResult::x ## _check == EAudioRequestResult::x, "EAudioRequestResult enumeration have been changed.")

inline void CheckAudioRequestResult()
{
	CHECK_ENUM(eARR_NONE);
	CHECK_ENUM(eARR_SUCCESS);
	CHECK_ENUM(eARR_FAILURE);
}

TYPE_MIRROR enum ATLDataScope
{
	eADS_NONE_check = 0,
	eADS_GLOBAL_check = 1,
	eADS_LEVEL_SPECIFIC_check = 2,
	eADS_ALL_check = 3
};

#define CHECK_ENUM(x) static_assert (ATLDataScope::x ## _check == EATLDataScope::x, "EATLDataScope enumeration have been changed.")

inline void CheckATLDataScope()
{
	CHECK_ENUM(eADS_NONE);
	CHECK_ENUM(eADS_GLOBAL);
	CHECK_ENUM(eADS_LEVEL_SPECIFIC);
	CHECK_ENUM(eADS_ALL);
}

TYPE_MIRROR enum AudioManagerRequestType
{
	eAMRT_NONE_check = 0,
	eAMRT_SET_AUDIO_IMPL_check = BIT(0),
	eAMRT_REFRESH_AUDIO_SYSTEM_check = BIT(1),
	eAMRT_RESERVE_AUDIO_OBJECT_ID_check = BIT(2),
	eAMRT_LOSE_FOCUS_check = BIT(3),
	eAMRT_GET_FOCUS_check = BIT(4),
	eAMRT_MUTE_ALL_check = BIT(5),
	eAMRT_UNMUTE_ALL_check = BIT(6),
	eAMRT_STOP_ALL_SOUNDS_check = BIT(7),
	eAMRT_PARSE_CONTROLS_DATA_check = BIT(8),
	eAMRT_PARSE_PRELOADS_DATA_check = BIT(9),
	eAMRT_CLEAR_CONTROLS_DATA_check = BIT(10),
	eAMRT_CLEAR_PRELOADS_DATA_check = BIT(11),
	eAMRT_PRELOAD_SINGLE_REQUEST_check = BIT(12),
	eAMRT_UNLOAD_SINGLE_REQUEST_check = BIT(13),
	eAMRT_UNLOAD_AFCM_DATA_BY_SCOPE_check = BIT(14),
	eAMRT_DRAW_DEBUG_INFO_check = BIT(15),	// Only used internally!
	eAMRT_ADD_REQUEST_LISTENER_check = BIT(16),
	eAMRT_REMOVE_REQUEST_LISTENER_check = BIT(17),
	eAMRT_CHANGE_LANGUAGE_check = BIT(18),
	eAMRT_RETRIGGER_AUDIO_CONTROLS_check = BIT(19)
};

#define CHECK_ENUM(x) static_assert (AudioManagerRequestType::x ## _check == EAudioManagerRequestType::x, "EAudioManagerRequestType enumeration have been changed.")

inline void CheckAudioManagerRequestType()
{
	CHECK_ENUM(eAMRT_NONE);
	CHECK_ENUM(eAMRT_SET_AUDIO_IMPL);
	CHECK_ENUM(eAMRT_REFRESH_AUDIO_SYSTEM);
	CHECK_ENUM(eAMRT_RESERVE_AUDIO_OBJECT_ID);
	CHECK_ENUM(eAMRT_LOSE_FOCUS);
	CHECK_ENUM(eAMRT_GET_FOCUS);
	CHECK_ENUM(eAMRT_MUTE_ALL);
	CHECK_ENUM(eAMRT_UNMUTE_ALL);
	CHECK_ENUM(eAMRT_STOP_ALL_SOUNDS);
	CHECK_ENUM(eAMRT_PARSE_CONTROLS_DATA);
	CHECK_ENUM(eAMRT_PARSE_PRELOADS_DATA);
	CHECK_ENUM(eAMRT_CLEAR_CONTROLS_DATA);
	CHECK_ENUM(eAMRT_CLEAR_PRELOADS_DATA);
	CHECK_ENUM(eAMRT_PRELOAD_SINGLE_REQUEST);
	CHECK_ENUM(eAMRT_UNLOAD_SINGLE_REQUEST);
	CHECK_ENUM(eAMRT_UNLOAD_AFCM_DATA_BY_SCOPE);
	CHECK_ENUM(eAMRT_DRAW_DEBUG_INFO);	// Only used internally!
	CHECK_ENUM(eAMRT_ADD_REQUEST_LISTENER);
	CHECK_ENUM(eAMRT_REMOVE_REQUEST_LISTENER);
	CHECK_ENUM(eAMRT_CHANGE_LANGUAGE);
	CHECK_ENUM(eAMRT_RETRIGGER_AUDIO_CONTROLS);
}

TYPE_MIRROR enum AudioCallbackManagerRequestType
{
	eACMRT_NONE_check = 0,
	eACMRT_REPORT_FINISHED_EVENT_check = BIT(0),	// Only used internally!
	eACMRT_REPORT_FINISHED_TRIGGER_INSTANCE_check = BIT(1),	// Only used internally!
	eACMRT_REPORT_PROCESSED_OBSTRUCTION_RAY_check = BIT(2)
};

#define CHECK_ENUM(x) static_assert (AudioCallbackManagerRequestType::x ## _check == EAudioCallbackManagerRequestType::x, "EAudioCallbackManagerRequestType enumeration have been changed.")

inline void CheckAudioCallbackManagerRequestType()
{
	CHECK_ENUM(eACMRT_NONE);
	CHECK_ENUM(eACMRT_REPORT_FINISHED_EVENT);	// Only used internally!
	CHECK_ENUM(eACMRT_REPORT_FINISHED_TRIGGER_INSTANCE);	// Only used internally!
	CHECK_ENUM(eACMRT_REPORT_PROCESSED_OBSTRUCTION_RAY);
}

TYPE_MIRROR enum AudioListenerRequestType
{
	eALRT_NONE_check = 0,
	eALRT_SET_POSITION_check = BIT(0)
};

#define CHECK_ENUM(x) static_assert (AudioListenerRequestType::x ## _check == EAudioListenerRequestType::x, "EAudioListenerRequestType enumeration have been changed.")

inline void CheckAudioListenerRequestType()
{
	CHECK_ENUM(eALRT_NONE);
	CHECK_ENUM(eALRT_SET_POSITION);
}

TYPE_MIRROR enum AudioObjectRequestType
{
	eAORT_NONE_check = 0,
	eAORT_PREPARE_TRIGGER_check = BIT(0),
	eAORT_UNPREPARE_TRIGGER_check = BIT(1),
	eAORT_EXECUTE_TRIGGER_check = BIT(2),
	eAORT_STOP_TRIGGER_check = BIT(3),
	eAORT_STOP_ALL_TRIGGERS_check = BIT(4),
	eAORT_SET_POSITION_check = BIT(5),
	eAORT_SET_RTPC_VALUE_check = BIT(6),
	eAORT_SET_SWITCH_STATE_check = BIT(7),
	eAORT_SET_VOLUME_check = BIT(8),
	eAORT_SET_ENVIRONMENT_AMOUNT_check = BIT(9),
	eAORT_RESET_ENVIRONMENTS_check = BIT(10),
	eAORT_RELEASE_OBJECT_check = BIT(11)
};

#define CHECK_ENUM(x) static_assert (AudioObjectRequestType::x ## _check == EAudioObjectRequestType::x, "EAudioObjectRequestType enumeration have been changed.")

inline void CheckAudioObjectRequestType()
{
	CHECK_ENUM(eAORT_NONE);
	CHECK_ENUM(eAORT_PREPARE_TRIGGER);
	CHECK_ENUM(eAORT_UNPREPARE_TRIGGER);
	CHECK_ENUM(eAORT_EXECUTE_TRIGGER);
	CHECK_ENUM(eAORT_STOP_TRIGGER);
	CHECK_ENUM(eAORT_STOP_ALL_TRIGGERS);
	CHECK_ENUM(eAORT_SET_POSITION);
	CHECK_ENUM(eAORT_SET_RTPC_VALUE);
	CHECK_ENUM(eAORT_SET_SWITCH_STATE);
	CHECK_ENUM(eAORT_SET_VOLUME);
	CHECK_ENUM(eAORT_SET_ENVIRONMENT_AMOUNT);
	CHECK_ENUM(eAORT_RESET_ENVIRONMENTS);
	CHECK_ENUM(eAORT_RELEASE_OBJECT);
}

TYPE_MIRROR enum AudioObjectObstructionCalcType
{
	eAOOCT_NONE_check = 0,
	eAOOCT_IGNORE_check = 1,
	eAOOCT_SINGLE_RAY_check = 2,
	eAOOCT_MULTI_RAY_check = 3,

	eAOOCT_COUNT_check
};

#define CHECK_ENUM(x) static_assert (AudioObjectObstructionCalcType::x ## _check == EAudioObjectObstructionCalcType::x, "EAudioObjectObstructionCalcType enumeration have been changed.")

inline void CheckAudioObjectObstructionCalcType()
{
	CHECK_ENUM(eAOOCT_NONE);
	CHECK_ENUM(eAOOCT_IGNORE);
	CHECK_ENUM(eAOOCT_SINGLE_RAY);
	CHECK_ENUM(eAOOCT_MULTI_RAY);

	CHECK_ENUM(eAOOCT_COUNT);
}

TYPE_MIRROR enum AudioControlType
{
	eACT_NONE_check = 0,
	eACT_AUDIO_OBJECT_check = 1,
	eACT_TRIGGER_check = 2,
	eACT_RTPC_check = 3,
	eACT_SWITCH_check = 4,
	eACT_SWITCH_STATE_check = 5,
	eACT_PRELOAD_check = 6,
	eACT_ENVIRONMENT_check = 7
};

#define CHECK_ENUM(x) static_assert (AudioControlType::x ## _check == EAudioControlType::x, "EAudioControlType enumeration have been changed.")

inline void CheckAudioControlType()
{
	CHECK_ENUM(eACT_NONE);
	CHECK_ENUM(eACT_AUDIO_OBJECT);
	CHECK_ENUM(eACT_TRIGGER);
	CHECK_ENUM(eACT_RTPC);
	CHECK_ENUM(eACT_SWITCH);
	CHECK_ENUM(eACT_SWITCH_STATE);
	CHECK_ENUM(eACT_PRELOAD);
	CHECK_ENUM(eACT_ENVIRONMENT);
}