#include "stdafx.h"

#include "CheckingBasics.h"
#include <IGameRulesSystem.h>

TYPE_MIRROR enum DisconnectionCause
{
	eDC_Timeout_check = 0,
	eDC_ProtocolError_check,
	eDC_ResolveFailed_check,
	eDC_VersionMismatch_check,
	eDC_ServerFull_check,
	eDC_Kicked_check,
	eDC_Banned_check,
	eDC_ContextCorruption_check,
	eDC_AuthenticationFailed_check,
	eDC_GameError_check,
	eDC_NotDX11Capable_check,
	eDC_NubDestroyed_check,
	eDC_ICMPError_check,
	eDC_NatNegError_check,
	eDC_PunkDetected_check,
	eDC_DemoPlaybackFinished_check,
	eDC_DemoPlaybackFileNotFound_check,
	eDC_UserRequested_check,
	eDC_NoController_check,
	eDC_CantConnect_check,
	eDC_ArbitrationFailed_check,
	eDC_FailedToMigrateToNewHost_check,
	eDC_SessionDeleted_check,
	eDC_KickedHighPing_check,
	eDC_KickedReservedUser_check,
	eDC_ClassRegistryMismatch_check,
	eDC_GloballyBanned_check,
	eDC_Global_Ban1_check,
	eDC_Global_Ban2_check,
	eDC_Unknown_check
};

#define CHECK_ENUM(x) static_assert (DisconnectionCause::x ## _check == EDisconnectionCause::x, "EDisconnectionCause enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eDC_Timeout);
	CHECK_ENUM(eDC_ProtocolError);
	CHECK_ENUM(eDC_ResolveFailed);
	CHECK_ENUM(eDC_VersionMismatch);
	CHECK_ENUM(eDC_ServerFull);
	CHECK_ENUM(eDC_Kicked);
	CHECK_ENUM(eDC_Banned);
	CHECK_ENUM(eDC_ContextCorruption);
	CHECK_ENUM(eDC_AuthenticationFailed);
	CHECK_ENUM(eDC_GameError);
	CHECK_ENUM(eDC_NotDX11Capable);
	CHECK_ENUM(eDC_NubDestroyed);
	CHECK_ENUM(eDC_ICMPError);
	CHECK_ENUM(eDC_NatNegError);
	CHECK_ENUM(eDC_PunkDetected);
	CHECK_ENUM(eDC_DemoPlaybackFinished);
	CHECK_ENUM(eDC_DemoPlaybackFileNotFound);
	CHECK_ENUM(eDC_UserRequested);
	CHECK_ENUM(eDC_NoController);
	CHECK_ENUM(eDC_CantConnect);
	CHECK_ENUM(eDC_ArbitrationFailed);
	CHECK_ENUM(eDC_FailedToMigrateToNewHost);
	CHECK_ENUM(eDC_SessionDeleted);
	CHECK_ENUM(eDC_KickedHighPing);
	CHECK_ENUM(eDC_KickedReservedUser);
	CHECK_ENUM(eDC_ClassRegistryMismatch);
	CHECK_ENUM(eDC_GloballyBanned);
	CHECK_ENUM(eDC_Global_Ban1);
	CHECK_ENUM(eDC_Global_Ban2);
	CHECK_ENUM(eDC_Unknown);
}

TYPE_MIRROR enum TextMessageType
{
	eTextMessageCenter_check = 0,
	eTextMessageConsole_check,
	eTextMessageError_check,
	eTextMessageInfo_check,
	eTextMessageServer_check,
	eTextMessageAnnouncement_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (TextMessageType::x ## _check == ETextMessageType::x, "ETextMessageType enumeration has been changed.")

inline void CheckTextMessageType()
{
	CHECK_ENUM(eTextMessageCenter);
	CHECK_ENUM(eTextMessageConsole);
	CHECK_ENUM(eTextMessageError);
	CHECK_ENUM(eTextMessageInfo);
	CHECK_ENUM(eTextMessageServer);
	CHECK_ENUM(eTextMessageAnnouncement);
}

TYPE_MIRROR enum ChatMessageType
{
	eChatToTarget_check = 0,
	eChatToTeam_check,
	eChatToAll_check
};

#undef CHECK_ENUM
#define CHECK_ENUM(x) static_assert (ChatMessageType::x ## _check == EChatMessageType::x, "EChatMessageType enumeration has been changed.")

inline void CheckChatMessageType()
{
	CHECK_ENUM(eChatToTarget);
	CHECK_ENUM(eChatToTeam);
	CHECK_ENUM(eChatToAll);
}