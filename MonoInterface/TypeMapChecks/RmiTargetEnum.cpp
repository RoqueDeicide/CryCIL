#include "stdafx.h"

#include "CheckingBasics.h"

#include "IGameObject.h"

TYPE_MIRROR enum RMInvocation
{
	eRMI_ToClientChannel_check = 0x01,
	eRMI_ToOwnClient_check = 0x02,
	eRMI_ToOtherClients_check = 0x04,
	eRMI_ToAllClients_check = 0x08,

	eRMI_ToServer_check = 0x100,

	eRMI_NoLocalCalls_check = 0x10000,
	eRMI_NoRemoteCalls_check = 0x20000,

	eRMI_ToRemoteClients_check = eRMI_NoLocalCalls_check | eRMI_ToAllClients_check
};

#define CHECK_ENUM(x) static_assert (RMInvocation::x ## _check == ERMInvocation::x, "ERMInvocation enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eRMI_ToClientChannel);
	CHECK_ENUM(eRMI_ToOwnClient);
	CHECK_ENUM(eRMI_ToOtherClients);
	CHECK_ENUM(eRMI_ToAllClients);

	CHECK_ENUM(eRMI_ToServer);

	CHECK_ENUM(eRMI_NoLocalCalls);
	CHECK_ENUM(eRMI_NoRemoteCalls);

	CHECK_ENUM(eRMI_ToRemoteClients);
}